using NumericValidation.EG.Models.BankCard.Algorithm;
using NumericValidation.EG.Models.BankCard.Enums;
using NumericValidation.EG.Models.BankCard.Info;
using NumericValidation.EG.Models.BankCard.Issuer;
using NumericValidation.EG.Models.BankCard.Logger;
using NumericValidation.EG.Models.BankCard.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace NumericValidation.EG.Models.BankCard.Validator
{
    // ═══════════════════════════════════════════════════════════════════════════
    //  ArabicNumberConverter.EG.BankCard — BankCardValidator
    //  Standards: ISO/IEC 7812-1:2017 | Luhn (ISO/IEC 7812-1 §6)
    //             CBE Tokenization Regulations 2024-2025 | PCI-DSS v4.0
    //
    //  External Dependencies (must be referenced in the host .csproj):
    //    • NumericValidation.EG (same solution) — provides:
    //        - NumericValidation.EG.Models.BankCard.Enums  (CardNetwork, CardType, …)
    //        - NumericValidation.EG.Models.BankCard.Info   (BankCardInfo)
    //        - NumericValidation.EG.Models.BankCard.Issuer (IssuerDatabase, IssuerInfo)
    //        - NumericValidation.EG.Models.BankCard.Algorithm (LuhnAlgorithm)
    //        - NumericValidation.EG.Models.BankCard.Logger (IBankCardLogger)
    //        - NumericValidation.EG.Models.BankCard.Extensions (BankCardExtensions)
    //        - NumericValidation.EG.Models.NumbersToText.Enums (Currency enum,
    //          NumberToWordsConverter) — used by IssuerDatabase and BankCardInfo
    //
    //  No NuGet packages are required beyond the .NET BCL (System.*).
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Thread-safe validator and analyser for payment card numbers (PANs).
    /// <para>
    /// <b>Key capabilities:</b>
    /// <list type="bullet">
    ///   <item>Luhn (Modulus-10) check digit validation per ISO/IEC 7812-1:2017 §6.</item>
    ///   <item>IIN/BIN detection for Egyptian banks (Meeza, Visa EG, Mastercard EG) and all major global schemes.</item>
    ///   <item>Full bilingual output (English + Arabic) with caller-selectable display language via <see cref="DisplayLanguage"/>.</item>
    ///   <item>Optional expiry, CVV, and cardholder-name validation via <see cref="AnalyzeFull"/>.</item>
    ///   <item>Built-in LRU-style result cache to avoid redundant computation for repeated inputs.</item>
    ///   <item>Optional structured logging via <see cref="IBankCardLogger"/>.</item>
    /// </list>
    /// </para>
    /// <para>
    /// مُحلِّل ومُحقِّق شامل لأرقام البطاقات البنكية — آمن للخيوط المتعددة.
    /// يدعم اللغتين العربية والإنجليزية مع كاش داخلي وتسجيل اختياري.
    /// </para>
    /// </summary>
    /// <example>
    /// <code>
    /// // Simple usage
    /// var validator = new BankCardValidator();
    /// var info = validator.Analyze("4111 1111 1111 1111");
    /// Console.WriteLine(info.IsValid);      // true
    /// Console.WriteLine(info.NetworkName);  // "Visa"
    ///
    /// // Arabic output
    /// var validator = new BankCardValidator(language: DisplayLanguage.Arabic);
    /// var info = validator.Analyze("4111111111111111");
    /// Console.WriteLine(info.NetworkName);  // "فيزا"
    ///
    /// // With logging
    /// var validator = new BankCardValidator(logger: new ConsoleBankCardLogger());
    ///
    /// // Full validation
    /// var info = validator.AnalyzeFull("4111111111111111", expiry:"12/28", cvv:"123");
    /// </code>
    /// </example>
    public sealed class BankCardValidator
    {
        // ─── Name lookup tables ───────────────────────────────────────────────────

        private static readonly Dictionary<CardNetwork, (string En, string Ar)> NetworkNames = new()
        {
            [CardNetwork.Visa] = ("Visa", "فيزا"),
            [CardNetwork.Mastercard] = ("Mastercard", "ماستر كارد"),
            [CardNetwork.AmericanExpress] = ("American Express", "أمريكان إكسبريس"),
            [CardNetwork.Discover] = ("Discover", "ديسكوفر"),
            [CardNetwork.UnionPay] = ("UnionPay", "يونيون باي"),
            [CardNetwork.JCB] = ("JCB", "JCB"),
            [CardNetwork.DinersClub] = ("Diners Club", "دايرز كلوب"),
            [CardNetwork.Meeza] = ("Meeza", "ميزة"),
            [CardNetwork.Maestro] = ("Maestro", "مايسترو"),
            [CardNetwork.MastercardDebit] = ("Mastercard Debit", "ماستر كارد خصم"),
            [CardNetwork.VisaElectron] = ("Visa Electron", "فيزا إلكترون"),
            [CardNetwork.Verve] = ("Verve", "فيرف"),
            [CardNetwork.MirCard] = ("MIR", "مير"),
            [CardNetwork.Elo] = ("Elo", "إيلو"),
            [CardNetwork.Unknown] = ("Unknown", "غير معروف"),
        };

        private static readonly Dictionary<CardType, (string En, string Ar)> CardTypeNames = new()
        {
            [CardType.Credit] = ("Credit", "ائتماني"),
            [CardType.Debit] = ("Debit", "خصم مباشر"),
            [CardType.Prepaid] = ("Prepaid", "مدفوع مسبقاً"),
            [CardType.Virtual] = ("Virtual", "بطاقة افتراضية"),
            [CardType.Corporate] = ("Corporate", "مؤسسي"),
            [CardType.Government] = ("Government", "حكومي"),
            [CardType.Unknown] = ("Unknown", "غير معروف"),
        };

        private static readonly Dictionary<CardCategory, (string En, string Ar)> CategoryNames = new()
        {
            [CardCategory.Classic] = ("Classic", "كلاسيك"),
            [CardCategory.Gold] = ("Gold", "ذهبية"),
            [CardCategory.Platinum] = ("Platinum", "بلاتينيوم"),
            [CardCategory.Signature] = ("Signature", "سيجنتشر"),
            [CardCategory.Infinite] = ("Infinite", "إنفينيت"),
            [CardCategory.Business] = ("Business", "أعمال"),
            [CardCategory.World] = ("World", "ورلد"),
            [CardCategory.Unknown] = ("Unknown", "غير معروف"),
        };

        // ─── Instance state ───────────────────────────────────────────────────────

        private readonly DisplayLanguage _defaultLanguage;
        private readonly IBankCardLogger _logger;

        // Thread-safe cache: key = "sanitizedPan|tokenSim|luhnSteps|lang"
        // ConcurrentDictionary is lock-free for reads on hot paths.
        private readonly ConcurrentDictionary<string, BankCardInfo> _cache = new();
        private const int MaxCacheSize = 512; // evict oldest beyond this threshold

        // ─── Constructor ──────────────────────────────────────────────────────────

        /// <summary>
        /// Initialises a new <see cref="BankCardValidator"/> instance.
        /// </summary>
        /// <param name="language">
        /// Default display language for bilingual alias properties on <see cref="BankCardInfo"/>
        /// (e.g. <see cref="BankCardInfo.NetworkName"/>).
        /// Defaults to <see cref="DisplayLanguage.English"/>.
        /// Can be overridden per-call via the <c>language</c> parameter on each Analyze method.
        /// </param>
        /// <param name="logger">
        /// Optional logger. Pass <c>new ConsoleBankCardLogger()</c> during development.
        /// When <c>null</c>, the silent <see cref="NullBankCardLogger.Instance"/> is used.
        /// </param>
        public BankCardValidator(
            DisplayLanguage language = DisplayLanguage.English,
            IBankCardLogger? logger = null)
        {
            _defaultLanguage = language;
            _logger = logger ?? NullBankCardLogger.Instance;
        }

        // ─── Public API ───────────────────────────────────────────────────────────

        /// <summary>
        /// Analyses a payment card number (PAN) and returns a fully populated <see cref="BankCardInfo"/>.
        /// </summary>
        /// <param name="cardNumber">
        /// The PAN to analyse. Spaces and dashes are stripped automatically.
        /// Accepts <c>null</c> — returns an invalid result with <see cref="ValidationFailureReason.NullOrEmpty"/>.
        /// </param>
        /// <param name="includeTokenSimulation">
        /// When <c>true</c> and the detected issuer supports Tokenization,
        /// a deterministic test token is generated and stored in <see cref="BankCardInfo.SimulatedToken"/>.
        /// <b>Never use the generated token in production payment flows.</b>
        /// </param>
        /// <param name="includeLuhnSteps">
        /// When <c>true</c>, a step-by-step Luhn computation breakdown is attached
        /// to <see cref="BankCardInfo.LuhnDetails"/>. Useful for educational tooling and debugging.
        /// </param>
        /// <param name="language">
        /// Override the instance-level display language for this call only.
        /// When <c>null</c> the instance default (<see cref="DisplayLanguage"/>) is used.
        /// </param>
        /// <returns>
        /// A <see cref="BankCardInfo"/> instance. <see cref="BankCardInfo.IsValid"/> is the primary
        /// pass/fail indicator.
        /// </returns>
        public BankCardInfo Analyze(
            string? cardNumber,
            bool includeTokenSimulation = false,
            bool includeLuhnSteps = false,
            DisplayLanguage? language = null)
        {
            DisplayLanguage lang = language ?? _defaultLanguage;

            // ── Build cache key from the *sanitised* PAN so that equivalent inputs
            //    that differ only in formatting (spaces / dashes) share the same entry.
            //    e.g. "4111 1111 1111 1111" and "4111-1111-1111-1111" → same key.
            string sanitizedForKey = (cardNumber ?? string.Empty)
                .Replace(" ", "")
                .Replace("-", "")
                .Trim();
            string cacheKey = $"{sanitizedForKey}|{includeTokenSimulation}|{includeLuhnSteps}|{(int)lang}";

            if (_cache.TryGetValue(cacheKey, out var cached))
                return cached;

            var result = PerformAnalysis(cardNumber, includeTokenSimulation, includeLuhnSteps, lang);

            // Evict oldest entry if cache is full (simple size guard — not true LRU)
            if (_cache.Count >= MaxCacheSize)
            {
                foreach (var key in _cache.Keys.Take(MaxCacheSize / 4))
                    _cache.TryRemove(key, out _);
            }

            _cache[cacheKey] = result;

            // Log masked PAN (never full PAN)
            string maskedForLog = result.MaskedNumber.Length > 0 ? result.MaskedNumber : "••••";
            _logger.LogValidation(maskedForLog, result.IsValid, result.NetworkNameEn, result.FailureReason);

            return result;
        }

        /// <summary>
        /// Performs a full card validation including optional expiry date, CVV, and cardholder name checks.
        /// Wraps <see cref="Analyze"/> and adds supplementary field validation.
        /// </summary>
        /// <param name="cardNumber">The PAN to validate.</param>
        /// <param name="expiry">
        /// Optional expiry date string. Accepted formats: "MM/YY", "MM/YYYY", "MMYY", "MMYYYY",
        /// "MM YY", "MM YYYY", "MM-YY", "MM-YYYY" (spaces and separators are stripped).
        /// </param>
        /// <param name="cvv">Optional CVV / CVC / CID value (digit string).</param>
        /// <param name="cardholderName">
        /// Optional cardholder name (Latin letters, spaces, hyphens, apostrophes; 2–26 chars).
        /// </param>
        /// <param name="includeTokenSimulation">See <see cref="Analyze"/>.</param>
        /// <param name="includeLuhnSteps">See <see cref="Analyze"/>.</param>
        /// <param name="language">See <see cref="Analyze"/>.</param>
        /// <returns>
        /// A <see cref="BankCardInfo"/> where <see cref="BankCardInfo.IsValid"/> is <c>true</c>
        /// only if the PAN itself, plus all supplied optional fields, all pass validation.
        /// </returns>
        public BankCardInfo AnalyzeFull(
            string? cardNumber,
            string? expiry = null,
            string? cvv = null,
            string? cardholderName = null,
            bool includeTokenSimulation = false,
            bool includeLuhnSteps = false,
            DisplayLanguage? language = null)
        {
            // Full analysis bypasses the cache (too many combinations)
            DisplayLanguage lang = language ?? _defaultLanguage;
            var info = PerformAnalysis(cardNumber, includeTokenSimulation, includeLuhnSteps, lang);

            if (expiry is not null)
            {
                info.ExpiryInput = expiry;
                ValidateExpiry(expiry, info);
            }

            if (cvv is not null)
                ValidateCvv(cvv, info);

            if (cardholderName is not null)
                ValidateCardholderName(cardholderName, info);

            // Re-evaluate overall validity
            if (info.IsValid)
            {
                if (info.IsExpired == true) info.IsValid = false;
                if (info.IsCvvValid == false) info.IsValid = false;
                if (info.IsCardholderNameValid == false) info.IsValid = false;
            }

            string maskedForLog = info.MaskedNumber.Length > 0 ? info.MaskedNumber : "••••";
            _logger.LogValidation(maskedForLog, info.IsValid, info.NetworkNameEn, info.FailureReason);

            return info;
        }

        /// <summary>
        /// Clears the internal result cache.
        /// Call this if issuer data has been updated at runtime and you need fresh results.
        /// </summary>
        public void ClearCache() => _cache.Clear();

        // ─── Core analysis ────────────────────────────────────────────────────────

        private BankCardInfo PerformAnalysis(
            string? cardNumber,
            bool includeTokenSimulation,
            bool includeLuhnSteps,
            DisplayLanguage lang)
        {
            try
            {
                var info = new BankCardInfo
                {
                    RawInput = cardNumber ?? string.Empty,
                    Language = lang
                };

                // Sanitise — null-safe, strips spaces, dashes, and surrounding whitespace
                string sanitized = (cardNumber ?? string.Empty)
                    .Replace(" ", "")
                    .Replace("-", "")
                    .Trim();

                info.SanitizedNumber = sanitized;

                // ── Guard: empty ───────────────────────────────────────────────
                if (sanitized.Length == 0)
                    return Fail(info, ValidationFailureReason.NullOrEmpty,
                        "Card number is null or empty.",
                        "رقم البطاقة فارغ أو غير مُدخَل.");

                // ── Guard: non-digit characters ────────────────────────────────
                if (!sanitized.All(char.IsDigit))
                {
                    info.IsNumericOnly = false;
                    return Fail(info, ValidationFailureReason.ContainsNonDigits,
                        "Card number must contain digits only (spaces and dashes are allowed).",
                        "رقم البطاقة يجب أن يحتوي على أرقام فقط (المسافات والشرطات مسموح بها).");
                }

                info.IsNumericOnly = true;
                info.Length = sanitized.Length;
                info.CheckDigit = sanitized[^1] - '0';
                info.LastFourDigits = sanitized.Length >= 4 ? sanitized[^4..] : sanitized;
                info.IIN = sanitized.Length >= 6 ? sanitized[..6] : sanitized;
                info.ExtendedIIN = sanitized.Length >= 8 ? sanitized[..8] : info.IIN;

                // ── Detect network & issuer ────────────────────────────────────
                DetectNetworkAndIssuer(sanitized, info);

                // ── Guard: length ──────────────────────────────────────────────
                info.IsLengthValid = info.ValidPanLengths.Length > 0
                    && info.ValidPanLengths.Contains(sanitized.Length);

                if (!info.IsLengthValid)
                {
                    string expected = string.Join(", ", info.ValidPanLengths);
                    return Fail(info, ValidationFailureReason.InvalidLength,
                        $"Invalid PAN length {sanitized.Length}. Expected: {(expected.Length > 0 ? expected : "standard lengths")}.",
                        $"طول الـ PAN {sanitized.Length} غير صحيح. المتوقع: {(expected.Length > 0 ? expected : "الأطوال المعيارية")}.");
                }

                // ── Guard: Luhn ────────────────────────────────────────────────
                info.IsLuhnValid = LuhnAlgorithm.IsValid(sanitized);
                if (!info.IsLuhnValid)
                    return Fail(info, ValidationFailureReason.LuhnCheckFailed,
                        "PAN failed the Luhn (Modulus-10) check digit verification.",
                        "فشل الـ PAN في اختبار خوارزمية Luhn (Modulus-10) لرقم التحقق.");

                // ── Success ────────────────────────────────────────────────────
                info.IsValid = true;
                info.FormattedNumber = FormatCardNumber(sanitized);
                info.MaskedNumber = MaskCardNumber(sanitized);
                info.FailureReason = ValidationFailureReason.None;

                if (includeTokenSimulation && info.SupportsTokenization)
                    info.SimulatedToken = GenerateSimulatedToken(sanitized);

                if (includeLuhnSteps)
                    info.LuhnDetails = LuhnAlgorithm.GetDetailedSteps(sanitized);

                BuildNotes(info);
                return info;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "••••");
                // Return a safe failure result rather than propagating the exception.
                // FailureReason is InternalError — not NullOrEmpty — so callers can
                // distinguish a genuine input problem from an unexpected runtime fault.
                var errInfo = new BankCardInfo
                {
                    RawInput = cardNumber ?? string.Empty,
                    Language = lang,
                    IsValid = false,
                    FailureReason = ValidationFailureReason.InternalError,
                    ValidationMessageEn = $"Unexpected error during analysis: {ex.Message}",
                    ValidationMessageAr = $"خطأ غير متوقع أثناء التحليل: {ex.Message}",
                };
                SetNames(errInfo);
                return errInfo;
            }
        }

        // ─── Network & issuer detection ───────────────────────────────────────────

        private static void DetectNetworkAndIssuer(string number, BankCardInfo info)
        {
            // Priority 1: exact Egyptian issuer match (most specific)
            foreach (var issuer in IssuerDatabase.EgyptianIssuers)
            {
                if (number.StartsWith(issuer.IIN, StringComparison.Ordinal))
                {
                    ApplyIssuer(info, issuer);
                    SetNames(info);
                    return;
                }
            }

            // Priority 2: global network range match (fallback)
            foreach (var (start, end, network, validLengths, cvvLen) in IssuerDatabase.GlobalNetworkRanges)
            {
                if (IsInRange(number, start, end))
                {
                    info.Network = network;
                    info.ValidPanLengths = validLengths;
                    info.CvvLength = cvvLen;
                    info.CardType = CardType.Unknown;
                    info.CardCategory = CardCategory.Unknown;
                    info.Region = IssuerRegion.Global;
                    info.SupportsTokenization = network is
                        CardNetwork.Visa or CardNetwork.Mastercard or
                        CardNetwork.AmericanExpress or CardNetwork.UnionPay;
                    SetNames(info);
                    return;
                }
            }

            // Priority 3: unknown
            info.Network = CardNetwork.Unknown;
            info.ValidPanLengths = new[] { 13, 14, 15, 16, 17, 18, 19 };
            info.CvvLength = 3;
            SetNames(info);
        }

        private static void ApplyIssuer(BankCardInfo info, IssuerInfo issuer)
        {
            info.Network = issuer.Network;
            info.CardType = issuer.CardType;
            info.CardCategory = issuer.CardCategory;
            info.IssuerNameEn = issuer.IssuerName;
            info.IssuerNameAr = issuer.IssuerNameArabic;
            info.CountryCode = issuer.CountryCode;
            info.CountryNameEn = issuer.CountryName;
            info.CountryNameAr = issuer.CountryNameArabic;
            info.CurrencyCode = issuer.CurrencyCode;
            info.Region = issuer.Region;
            info.IsEgyptian = issuer.IsEgyptian;
            info.IssuerWebsite = issuer.IssuerWebsite;
            info.CustomerServiceEgypt = issuer.CustomerServiceEgypt;
            info.SupportsTokenization = issuer.SupportsTokenization;
            info.CvvLength = issuer.CvvLength;
            info.ValidPanLengths = issuer.ValidPanLengths;
        }

        /// <summary>
        /// Checks whether the prefix of <paramref name="number"/> (of the same length as <paramref name="start"/>)
        /// falls within the numeric range [<paramref name="start"/>, <paramref name="end"/>].
        /// </summary>
        private static bool IsInRange(string number, string start, string end)
        {
            if (number.Length < start.Length)
                return false;

            // Simple equality shortcut
            if (start == end)
                return number.StartsWith(start, StringComparison.Ordinal);

            string prefix = number[..start.Length];

            if (!long.TryParse(prefix, out long prefixNum))
                return false;

            // Trim end to same length as start for numeric comparison
            string endTrimmed = end.Length >= start.Length ? end[..start.Length] : end.PadRight(start.Length, '9');

            if (!long.TryParse(start, out long startNum) ||
                !long.TryParse(endTrimmed, out long endNum))
                return false;

            return prefixNum >= startNum && prefixNum <= endNum;
        }

        private static void SetNames(BankCardInfo info)
        {
            if (NetworkNames.TryGetValue(info.Network, out var net))
            {
                info.NetworkNameEn = net.En;
                info.NetworkNameAr = net.Ar;
            }

            if (CardTypeNames.TryGetValue(info.CardType, out var type))
            {
                info.CardTypeNameEn = type.En;
                info.CardTypeNameAr = type.Ar;
            }

            if (CategoryNames.TryGetValue(info.CardCategory, out var cat))
            {
                info.CardCategoryNameEn = cat.En;
                info.CardCategoryNameAr = cat.Ar;
            }
        }

        // ─── Expiry validation ────────────────────────────────────────────────────

        /// <summary>
        /// Validates an expiry date string.
        /// Accepted separators: "/", "-", " " (or none).
        /// Accepted formats after stripping separators: MMYY (4 digits) or MMYYYY (6 digits).
        /// </summary>
        private static void ValidateExpiry(string expiry, BankCardInfo info)
        {
            // Strip all common separators including spaces
            string clean = Regex.Replace(expiry, @"[\s/\-]", "");

            if (clean.Length != 4 && clean.Length != 6)
            {
                info.IsExpiryValid = false;
                info.IsExpired = null;
                return;
            }

            if (!int.TryParse(clean[..2], out int month) ||
                !int.TryParse(clean[2..], out int yearRaw))
            {
                info.IsExpiryValid = false;
                info.IsExpired = null;
                return;
            }

            int year = clean.Length == 4 ? 2000 + yearRaw : yearRaw;

            bool validMonth = month >= 1 && month <= 12;
            bool validYear = year >= 2000 && year <= 2099;

            info.IsExpiryValid = validMonth && validYear;

            if (info.IsExpiryValid != true)
            {
                info.IsExpired = null;
                return;
            }

            // A card is valid through the last day of its expiry month
            var now = DateTime.UtcNow;
            info.IsExpired = year < now.Year || year == now.Year && month < now.Month;
        }

        // ─── CVV validation ───────────────────────────────────────────────────────

        private static void ValidateCvv(string cvv, BankCardInfo info)
        {
            string cleanCvv = cvv.Trim();
            info.IsCvvValid = cleanCvv.Length == info.CvvLength && cleanCvv.All(char.IsDigit);
        }

        // ─── Cardholder name validation ───────────────────────────────────────────

        private static void ValidateCardholderName(string name, BankCardInfo info)
        {
            string trimmed = name.Trim();
            // ISO/IEC 7813: cardholder name on the magnetic stripe is Latin letters,
            // spaces, hyphens, and apostrophes; max 26 characters.
            info.IsCardholderNameValid =
                trimmed.Length >= 2 &&
                trimmed.Length <= 26 &&
                Regex.IsMatch(trimmed, @"^[A-Za-z\s\-']+$");
        }

        // ─── Formatting helpers ───────────────────────────────────────────────────

        private static string FormatCardNumber(string number)
        {
            return number.Length switch
            {
                15 => $"{number[..4]} {number[4..10]} {number[10..]}",    // Amex: 4-6-5
                14 => $"{number[..4]} {number[4..10]} {number[10..]}",    // Diners: 4-6-4
                13 => $"{number[..4]} {number[4..9]} {number[9..]}",      // Visa 13-digit
                _ => string.Join(" ", Enumerable.Range(0, (number.Length + 3) / 4)
                         .Select(i => number.Substring(i * 4, Math.Min(4, number.Length - i * 4))))
            };
        }

        private static string MaskCardNumber(string number)
        {
            if (number.Length <= 4) return number;

            int visibleStart = Math.Min(4, number.Length - 4);
            string first = number[..visibleStart];
            string last4 = number[^4..];
            string middle = new string('•', number.Length - visibleStart - 4);

            return FormatCardNumber(first + middle + last4);
        }

        // ─── Simulated token ──────────────────────────────────────────────────────

        private static string GenerateSimulatedToken(string pan)
        {
            // Deterministic from PAN hash — same PAN always yields the same test token.
            // ⚠ For testing only. Real tokens are issued by the payment network or CBE TSP.
            var rng = new Random(pan.GetHashCode());
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string body = new string(Enumerable.Range(0, 16)
                .Select(_ => chars[rng.Next(chars.Length)]).ToArray());
            return "TOK_" + body;
        }

        // ─── Notes builder ────────────────────────────────────────────────────────

        private static void BuildNotes(BankCardInfo info)
        {
            if (info.Network == CardNetwork.Meeza)
            {
                info.NotesEn.Add("Meeza is Egypt's national payment card network, launched by the Central Bank of Egypt (CBE).");
                info.NotesAr.Add("ميزة هي شبكة الدفع الوطنية المصرية، أطلقها البنك المركزي المصري.");
            }

            if (info.IsEgyptian)
            {
                info.NotesEn.Add("This card was issued by an Egyptian bank licensed and regulated by the Central Bank of Egypt (CBE).");
                info.NotesAr.Add("هذه البطاقة صادرة من بنك مصري مُرخَّص وخاضع لإشراف البنك المركزي المصري.");
            }

            if (info.SupportsTokenization)
            {
                info.NotesEn.Add("This issuer supports card Tokenization per CBE regulations (2024-2025) and/or major network token schemes.");
                info.NotesAr.Add("تدعم جهة الإصدار تقنية الترميز (Tokenization) وفق لوائح البنك المركزي المصري 2024-2025 و/أو مخططات الرمز المميز للشبكات الكبرى.");
            }

            if (info.CardType == CardType.Prepaid)
            {
                info.NotesEn.Add("Prepaid cards may not be accepted at all merchants or for recurring transactions.");
                info.NotesAr.Add("البطاقات مدفوعة مسبقاً قد لا تُقبَل لدى جميع التجار أو في المعاملات المتكررة.");
            }

            if (info.Network == CardNetwork.AmericanExpress)
            {
                info.NotesEn.Add("American Express cards use a 4-digit CID (Card Identification Number) instead of the standard 3-digit CVV.");
                info.NotesAr.Add("بطاقات أمريكان إكسبريس تستخدم كود CID مكوّن من 4 أرقام بدلاً من CVV المعتاد المكوّن من 3 أرقام.");
            }
        }

        // ─── Failure helper ───────────────────────────────────────────────────────

        private static BankCardInfo Fail(
            BankCardInfo info,
            ValidationFailureReason reason,
            string messageEn,
            string messageAr)
        {
            info.IsValid = false;
            info.FailureReason = reason;
            info.ValidationMessageEn = messageEn;
            info.ValidationMessageAr = messageAr;
            SetNames(info);
            return info;
        }
    }
}
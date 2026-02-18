using NumericValidation.EG.Models.BankCard.Algorithm;
using NumericValidation.EG.Models.BankCard.Enums;
using NumericValidation.EG.Models.BankCard.Validator;
using System;
using System.Collections.Generic;
using static NumericValidation.EG.Models.NumbersToText.Enums.NumberToWordsConverter;

namespace NumericValidation.EG.Models.BankCard.Info
{
    // ═══════════════════════════════════════════════════════════════════════════
    //  ArabicNumberConverter.EG.BankCard — BankCardInfo
    //  Complete analysis result model for a single PAN
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Immutable-by-convention result object returned by <see cref="BankCardValidator.Analyze"/>
    /// and <see cref="BankCardValidator.AnalyzeFull"/>.
    /// <para>
    /// Every string field that has a bilingual equivalent is stored in <b>both</b> languages.
    /// The <em>primary</em> (un-suffixed) property reflects whichever <see cref="DisplayLanguage"/>
    /// was supplied to the validator; the secondary copy always keeps the opposite language.
    /// This means callers can switch display language at any time without re-analysing the card.
    /// </para>
    /// <para>
    /// نموذج النتيجة الكامل لتحليل بطاقة بنكية واحدة.
    /// كل حقل ثنائي اللغة متوفر بالإنجليزية والعربية في آنٍ واحد.
    /// الحقل الأساسي (بدون لاحقة) يعكس اللغة المختارة عند الاستدعاء.
    /// </para>
    /// </summary>
    public class BankCardInfo
    {
        // ═══════════════════════════════════════════════════════════════════
        // §1  Raw & sanitised input
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>The card number string exactly as supplied by the caller.</summary>
        public string RawInput { get; set; } = string.Empty;

        /// <summary>
        /// Card number after removing spaces, dashes, and surrounding whitespace.
        /// This is the value used for all validation and detection logic.
        /// </summary>
        public string SanitizedNumber { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable grouped format (e.g. "4111 1111 1111 1111" for 16-digit Visa;
        /// "3782 822463 10005" for 15-digit Amex).
        /// Empty when the card is invalid.
        /// </summary>
        public string FormattedNumber { get; set; } = string.Empty;

        /// <summary>
        /// Masked PAN for display — first 4 digits visible, last 4 digits visible,
        /// middle replaced with bullet characters (•), then grouped (e.g. "4111 •••• •••• 1111").
        /// Empty when the card is invalid.
        /// </summary>
        public string MaskedNumber { get; set; } = string.Empty;

        /// <summary>Last four digits of the PAN — safe to display and store per PCI-DSS.</summary>
        public string LastFourDigits { get; set; } = string.Empty;

        /// <summary>
        /// First 6 digits of the PAN — the IIN (Issuer Identification Number) as per ISO/IEC 7812-1 §4.
        /// Also called the BIN (Bank Identification Number).
        /// </summary>
        public string IIN { get; set; } = string.Empty;

        /// <summary>
        /// First 8 digits of the PAN — the extended IIN introduced in the 2017 revision of ISO/IEC 7812.
        /// Falls back to <see cref="IIN"/> (6 digits) when the PAN is shorter than 8 digits.
        /// </summary>
        public string ExtendedIIN { get; set; } = string.Empty;

        /// <summary>Total length of the sanitised PAN.</summary>
        public int Length { get; set; }

        /// <summary>Luhn check digit — the last (rightmost) digit of the PAN.</summary>
        public int CheckDigit { get; set; }

        // ═══════════════════════════════════════════════════════════════════
        // §2  Validation results
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// <c>true</c> only when all of the following pass:
        /// input is numeric, PAN length is valid for the detected scheme,
        /// and the Luhn check digit is correct.
        /// When <see cref="AnalyzeFull"/> is used, also requires expiry not expired,
        /// CVV correct, and cardholder name valid (if those fields were supplied).
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary><c>true</c> if the PAN passes the Luhn (Modulus-10) algorithm (ISO/IEC 7812-1 §6).</summary>
        public bool IsLuhnValid { get; set; }

        /// <summary><c>true</c> if the PAN length falls within the allowed range(s) for the detected scheme.</summary>
        public bool IsLengthValid { get; set; }

        /// <summary><c>true</c> if the sanitised input contains only ASCII digit characters.</summary>
        public bool IsNumericOnly { get; set; }

        /// <summary>
        /// Machine-readable reason for failure when <see cref="IsValid"/> is <c>false</c>.
        /// <see cref="ValidationFailureReason.None"/> when the card is valid.
        /// </summary>
        public ValidationFailureReason FailureReason { get; set; }

        /// <summary>Human-readable validation message in <b>English</b>.</summary>
        public string? ValidationMessageEn { get; set; }

        /// <summary>رسالة التحقق بالعربية.</summary>
        public string? ValidationMessageAr { get; set; }

        /// <summary>
        /// Validation message in the display language chosen at analysis time.
        /// Equivalent to <see cref="ValidationMessageEn"/> or <see cref="ValidationMessageAr"/>
        /// depending on <see cref="Language"/>.
        /// </summary>
        public string? ValidationMessage => Language == DisplayLanguage.Arabic
            ? ValidationMessageAr
            : ValidationMessageEn;

        // ═══════════════════════════════════════════════════════════════════
        // §3  Network / scheme information
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>Detected payment network (scheme).</summary>
        public CardNetwork Network { get; set; }

        /// <summary>Network / scheme name in <b>English</b> (e.g. "Visa", "Mastercard", "Meeza").</summary>
        public string NetworkNameEn { get; set; } = string.Empty;

        /// <summary>اسم الشبكة بالعربية (مثال: "فيزا"، "ماستر كارد"، "ميزة").</summary>
        public string NetworkNameAr { get; set; } = string.Empty;

        /// <summary>Network name in the display language chosen at analysis time.</summary>
        public string NetworkName => Language == DisplayLanguage.Arabic ? NetworkNameAr : NetworkNameEn;

        /// <summary>Functional card type (Credit / Debit / Prepaid …).</summary>
        public CardType CardType { get; set; }

        /// <summary>Card type name in <b>English</b>.</summary>
        public string CardTypeNameEn { get; set; } = string.Empty;

        /// <summary>اسم نوع البطاقة بالعربية.</summary>
        public string CardTypeNameAr { get; set; } = string.Empty;

        /// <summary>Card type name in the display language chosen at analysis time.</summary>
        public string CardTypeName => Language == DisplayLanguage.Arabic ? CardTypeNameAr : CardTypeNameEn;

        /// <summary>Card tier / category (Classic / Gold / Platinum …).</summary>
        public CardCategory CardCategory { get; set; }

        /// <summary>Card category name in <b>English</b>.</summary>
        public string CardCategoryNameEn { get; set; } = string.Empty;

        /// <summary>اسم فئة البطاقة بالعربية.</summary>
        public string CardCategoryNameAr { get; set; } = string.Empty;

        /// <summary>Card category name in the display language chosen at analysis time.</summary>
        public string CardCategoryName => Language == DisplayLanguage.Arabic ? CardCategoryNameAr : CardCategoryNameEn;

        /// <summary>Valid PAN lengths for the detected network / issuer combination.</summary>
        public int[] ValidPanLengths { get; set; } = Array.Empty<int>();

        /// <summary>Expected length of the CVV / CVC / CID security code (3 for most networks; 4 for Amex).</summary>
        public int CvvLength { get; set; } = 3;

        // ═══════════════════════════════════════════════════════════════════
        // §4  Issuer information
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>Name of the card-issuing bank or institution in <b>English</b>. <c>null</c> when unknown.</summary>
        public string? IssuerNameEn { get; set; }

        /// <summary>اسم البنك المُصدِر بالعربية. <c>null</c> عند عدم التعرف عليه.</summary>
        public string? IssuerNameAr { get; set; }

        /// <summary>Issuer name in the display language chosen at analysis time.</summary>
        public string? IssuerName => Language == DisplayLanguage.Arabic ? IssuerNameAr : IssuerNameEn;

        /// <summary>Official website URL of the issuing bank.</summary>
        public string? IssuerWebsite { get; set; }

        /// <summary>Egyptian short-dial customer-service number (e.g. "19623" for NBE).</summary>
        public string? CustomerServiceEgypt { get; set; }

        // ═══════════════════════════════════════════════════════════════════
        // §5  Geography & currency
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>ISO 3166-1 alpha-2 country code of the issuer (e.g. "EG", "US").</summary>
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>Country name in <b>English</b>.</summary>
        public string CountryNameEn { get; set; } = string.Empty;

        /// <summary>اسم الدولة بالعربية.</summary>
        public string CountryNameAr { get; set; } = string.Empty;

        /// <summary>Country name in the display language chosen at analysis time.</summary>
        public string CountryName => Language == DisplayLanguage.Arabic ? CountryNameAr : CountryNameEn;

        /// <summary>ISO 4217 currency code of the card's default settlement currency (e.g. "EGP", "USD").</summary>
        public Currency CurrencyCode { get; set; } 

        /// <summary>Geographic region of the issuer.</summary>
        public IssuerRegion Region { get; set; }

        /// <summary><c>true</c> if the card was issued by an Egyptian bank regulated by the CBE.</summary>
        public bool IsEgyptian { get; set; }

        // ═══════════════════════════════════════════════════════════════════
        // §6  Tokenization
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// <c>true</c> if the issuer participates in the CBE Tokenization programme
        /// or a major network tokenization scheme (Apple Pay, Google Pay, etc.).
        /// </summary>
        public bool SupportsTokenization { get; set; }

        /// <summary>
        /// Deterministic test token generated from the PAN hash.
        /// Populated only when <c>includeTokenSimulation: true</c> is passed to the validator
        /// and <see cref="SupportsTokenization"/> is <c>true</c>.
        /// <b>Never use in production payments.</b>
        /// </summary>
        public string? SimulatedToken { get; set; }

        // ═══════════════════════════════════════════════════════════════════
        // §7  Luhn detail (optional)
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Step-by-step Luhn computation breakdown.
        /// Populated only when <c>includeLuhnSteps: true</c> is passed to the validator.
        /// Useful for educational tooling and debugging.
        /// </summary>
        public LuhnStepResult? LuhnDetails { get; set; }

        // ═══════════════════════════════════════════════════════════════════
        // §8  Full-validation fields (AnalyzeFull only)
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// <c>true</c> if the expiry string was parseable into a valid calendar date (MM/YY or MM/YYYY).
        /// <c>null</c> when expiry was not supplied.
        /// </summary>
        public bool? IsExpiryValid { get; set; }

        /// <summary>
        /// <c>true</c> if the card's expiry month/year is in the past.
        /// <c>null</c> when expiry was not supplied or could not be parsed.
        /// </summary>
        public bool? IsExpired { get; set; }

        /// <summary>Raw expiry string as supplied by the caller (e.g. "12/28").</summary>
        public string? ExpiryInput { get; set; }

        /// <summary>
        /// <c>true</c> if the CVV/CVC/CID value is numeric and matches the expected length
        /// for the detected network. <c>null</c> when CVV was not supplied.
        /// </summary>
        public bool? IsCvvValid { get; set; }

        /// <summary>
        /// <c>true</c> if the cardholder name contains only Latin letters, spaces, hyphens,
        /// and apostrophes and is between 2 and 26 characters.
        /// <c>null</c> when the name was not supplied.
        /// </summary>
        public bool? IsCardholderNameValid { get; set; }

        // ═══════════════════════════════════════════════════════════════════
        // §9  Language & computed display helpers
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// The display language that was active when this result was produced.
        /// Controls which value the language-agnostic alias properties
        /// (<see cref="NetworkName"/>, <see cref="IssuerName"/>, etc.) return.
        /// </summary>
        public DisplayLanguage Language { get; set; } = DisplayLanguage.English;

        /// <summary>
        /// Concise one-line card description in the current display language.
        /// Example (EN): "Visa Credit — Commercial International Bank (CIB)"
        /// مثال (AR):    "فيزا ائتماني — البنك التجاري الدولي"
        /// </summary>
        public string DisplayLabel => Language == DisplayLanguage.Arabic
            ? $"{NetworkNameAr} {CardTypeNameAr} — {IssuerNameAr ?? "جهة إصدار غير معروفة"}"
            : $"{NetworkNameEn} {CardTypeNameEn} — {IssuerNameEn ?? "Unknown Issuer"}";

        /// <summary>
        /// <c>true</c> if the card is issued by a non-Egyptian institution
        /// and does not belong to the Meeza domestic scheme.
        /// </summary>
        public bool IsInternational => !IsEgyptian && Network != CardNetwork.Meeza;

        // ═══════════════════════════════════════════════════════════════════
        // §10  Informational notes
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>Contextual notes about the card in <b>English</b>.</summary>
        public List<string> NotesEn { get; set; } = new();

        /// <summary>ملاحظات سياقية عن البطاقة بالعربية.</summary>
        public List<string> NotesAr { get; set; } = new();

        /// <summary>Notes in the current display language.</summary>
        public List<string> Notes => Language == DisplayLanguage.Arabic ? NotesAr : NotesEn;
    }
}

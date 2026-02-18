using NumericValidation.EG.Models.BankCard.Enums;
using NumericValidation.EG.Models.BankCard.Info;
using NumericValidation.EG.Models.BankCard.Validator;
using System.Threading;

namespace NumericValidation.EG.Models.BankCard.Extensions
{
    // ═══════════════════════════════════════════════════════════════════════════
    //  ArabicNumberConverter.EG.BankCard — BankCardExtensions
    //  Fluent string-extension convenience API
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Provides fluent extension methods on <see cref="string"/> for quick,
    /// one-liner card validation and inspection without instantiating a
    /// <see cref="BankCardValidator"/> directly.
    /// <para>
    /// <b>Thread safety:</b> A <c>ThreadLocal&lt;BankCardValidator&gt;</c> is used internally
    /// so that each thread gets its own validator instance, avoiding contention on shared state.
    /// </para>
    /// <para>
    /// يوفر extension methods على <see cref="string"/> لإجراء التحقق والتحليل السريع
    /// دون الحاجة لإنشاء <see cref="BankCardValidator"/> بشكل صريح.
    /// آمن للخيوط المتعددة باستخدام ThreadLocal.
    /// </para>
    /// </summary>
    /// <example>
    /// <code>
    /// // English (default)
    /// bool valid   = "4111111111111111".IsValidCard();
    /// string net   = "4111111111111111".GetCardNetworkName();          // "Visa"
    /// string netAr = "4111111111111111".GetCardNetworkName(DisplayLanguage.Arabic); // "فيزا"
    /// bool isEgy   = "5078031234567890".IsEgyptianCard();
    /// string masked = "4111111111111111".MaskCard();                   // "4111 •••• •••• 1111"
    /// </code>
    /// </example>
    public static class BankCardExtensions
    {
        // ThreadLocal ensures each thread has its own validator instance —
        // eliminates any lock contention while keeping allocation minimal.
        private static readonly ThreadLocal<BankCardValidator> _validatorEn
            = new(() => new BankCardValidator(DisplayLanguage.English));

        private static readonly ThreadLocal<BankCardValidator> _validatorAr
            = new(() => new BankCardValidator(DisplayLanguage.Arabic));

        private static BankCardValidator GetValidator(DisplayLanguage language)
            => language == DisplayLanguage.Arabic
                ? _validatorAr.Value!
                : _validatorEn.Value!;

        // ─── Core analysis ─────────────────────────────────────────────────────

        /// <summary>
        /// Performs a full analysis on this card number string and returns a <see cref="BankCardInfo"/>.
        /// </summary>
        /// <param name="cardNumber">The PAN to analyse. Spaces and dashes are stripped automatically.</param>
        /// <param name="language">Display language for bilingual alias properties. Defaults to English.</param>
        /// <returns>A populated <see cref="BankCardInfo"/> result object.</returns>
        public static BankCardInfo AnalyzeCard(
            this string cardNumber,
            DisplayLanguage language = DisplayLanguage.English)
            => GetValidator(language).Analyze(cardNumber, language: language);

        // ─── Validation helpers ────────────────────────────────────────────────

        /// <summary>
        /// Returns <c>true</c> if this card number passes Luhn, has a valid length,
        /// and belongs to a recognised payment network.
        /// </summary>
        public static bool IsValidCard(this string cardNumber)
            => GetValidator(DisplayLanguage.English).Analyze(cardNumber).IsValid;

        // ─── Network helpers ───────────────────────────────────────────────────

        /// <summary>Returns the detected <see cref="CardNetwork"/> enum value for this card number.</summary>
        public static CardNetwork GetCardNetwork(this string cardNumber)
            => GetValidator(DisplayLanguage.English).Analyze(cardNumber).Network;

        /// <summary>
        /// Returns the network name in the specified language.
        /// </summary>
        /// <param name="cardNumber">The PAN to inspect.</param>
        /// <param name="language">Output language. Defaults to English.</param>
        /// <returns>
        /// E.g. "Visa" / "فيزا", "Mastercard" / "ماستر كارد", "Meeza" / "ميزة".
        /// </returns>
        public static string GetCardNetworkName(
            this string cardNumber,
            DisplayLanguage language = DisplayLanguage.English)
        {
            var info = GetValidator(language).Analyze(cardNumber, language: language);
            return language == DisplayLanguage.Arabic ? info.NetworkNameAr : info.NetworkNameEn;
        }

        // ─── Geography helpers ─────────────────────────────────────────────────

        /// <summary>
        /// Returns <c>true</c> if this card was issued by an Egyptian bank regulated by the CBE.
        /// </summary>
        public static bool IsEgyptianCard(this string cardNumber)
            => GetValidator(DisplayLanguage.English).Analyze(cardNumber).IsEgyptian;

        // ─── Tokenization helper ───────────────────────────────────────────────

        /// <summary>
        /// Returns <c>true</c> if the detected issuer participates in a card Tokenization scheme
        /// (Apple Pay, CBE Tokenization, etc.).
        /// </summary>
        public static bool CardSupportsTokenization(this string cardNumber)
            => GetValidator(DisplayLanguage.English).Analyze(cardNumber).SupportsTokenization;

        // ─── Display helpers ───────────────────────────────────────────────────

        /// <summary>
        /// Returns the PAN in masked form (e.g. "4111 •••• •••• 1111").
        /// Returns the original string unchanged if the PAN is invalid.
        /// </summary>
        public static string MaskCard(this string cardNumber)
        {
            var info = GetValidator(DisplayLanguage.English).Analyze(cardNumber);
            return info.IsValid ? info.MaskedNumber : cardNumber;
        }

        /// <summary>
        /// Returns the PAN in grouped/formatted form (e.g. "4111 1111 1111 1111").
        /// Returns the original string unchanged if the PAN is invalid.
        /// </summary>
        public static string FormatCard(this string cardNumber)
        {
            var info = GetValidator(DisplayLanguage.English).Analyze(cardNumber);
            return info.IsValid ? info.FormattedNumber : cardNumber;
        }

        /// <summary>
        /// Returns the last four digits of the PAN (safe to display per PCI-DSS).
        /// Returns the original string if it is 4 characters or fewer.
        /// </summary>
        public static string GetLastFour(this string cardNumber)
        {
            var info = GetValidator(DisplayLanguage.English).Analyze(cardNumber);
            return info.LastFourDigits.Length > 0 ? info.LastFourDigits : cardNumber;
        }
    }
}

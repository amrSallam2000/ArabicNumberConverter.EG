namespace NumericValidation.EG.Models.Helper
{
    /// <summary>
    /// Options for text unification and processing
    /// </summary>
    public class TextUnificationOptions
    {
        /// <summary>
        /// Unify Arabic text by standardizing letter forms
        /// </summary>
        public bool UnifyArabicText { get; set; } = true;

        /// <summary>
        /// Remove Arabic diacritics (tashkeel)
        /// </summary>
        public bool RemoveTashkeel { get; set; } = true;

        /// <summary>
        /// Unify numeral characters in the text
        /// </summary>
        public bool UnifyNumerals { get; set; } = true;

        /// <summary>
        /// Convert numerals between Arabic and Western forms
        /// </summary>
        public bool ConvertNumerals { get; set; } = true;

        /// <summary>
        /// Prefer Arabic numerals when converting
        /// </summary>
        public bool PreferArabicNumerals { get; set; } = true;

        /// <summary>
        /// Remove special characters from text
        /// </summary>
        public bool RemoveSpecialChars { get; set; } = false;

        /// <summary>
        /// Keep spaces when removing special characters
        /// </summary>
        public bool KeepSpaces { get; set; } = true;

        /// <summary>
        /// Extract only numbers from the text
        /// </summary>
        public bool ExtractNumbersOnly { get; set; } = false;

        /// <summary>
        /// Extract only text (no numbers) from the input
        /// </summary>
        public bool ExtractTextOnly { get; set; } = false;

        /// <summary>
        /// Default text unification options
        /// </summary>
        public static TextUnificationOptions Default => new()
        {
            UnifyArabicText = true,
            RemoveTashkeel = true,
            UnifyNumerals = true,
            PreferArabicNumerals = true
        };

        /// <summary>
        /// Options optimized for Arabic text processing
        /// </summary>
        public static TextUnificationOptions ArabicText => new()
        {
            UnifyArabicText = true,
            RemoveTashkeel = true,
            UnifyNumerals = true,
            PreferArabicNumerals = true
        };

        /// <summary>
        /// Options optimized for English text processing
        /// </summary>
        public static TextUnificationOptions EnglishText => new()
        {
            UnifyArabicText = false,
            ConvertNumerals = false,
            RemoveSpecialChars = true
        };

        /// <summary>
        /// Options for extracting only numbers
        /// </summary>
        public static TextUnificationOptions NumbersOnly => new()
        {
            ConvertNumerals = true,
            ExtractNumbersOnly = true,
            RemoveSpecialChars = true
        };

        /// <summary>
        /// Options for extracting only text (no numbers)
        /// </summary>
        public static TextUnificationOptions TextOnly => new()
        {
            ConvertNumerals = false,
            ExtractTextOnly = true,
            RemoveSpecialChars = true
        };
    }
}
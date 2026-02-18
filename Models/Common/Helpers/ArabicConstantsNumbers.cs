using System.Collections.Generic;

namespace NumericValidation.EG.Models.Common.Helpers
{
    /// <summary>
    /// Contains constant definitions for Arabic text processing and numeral conversion
    /// </summary>
    public static class ArabicConstantsNumbers
    {
        /// <summary>
        /// Dictionary mapping Western numerals and symbols to Arabic-Indic equivalents
        /// </summary>
        public static readonly Dictionary<char, char> WesternToArabicNumerals = new()
        {
            { '0', '٠' }, { '1', '١' }, { '2', '٢' }, { '3', '٣' }, { '4', '٤' },
            { '5', '٥' }, { '6', '٦' }, { '7', '٧' }, { '8', '٨' }, { '9', '٩' },
            { '.', '٫' }, { ',', '٬' }, { '%', '٪' }
        };

        /// <summary>
        /// Dictionary mapping Arabic-Indic numerals and symbols to Western equivalents
        /// </summary>
        public static readonly Dictionary<char, char> ArabicToWesternNumerals = new()
        {
            { '٠', '0' }, { '١', '1' }, { '٢', '2' }, { '٣', '3' }, { '٤', '4' },
            { '٥', '5' }, { '٦', '6' }, { '٧', '7' }, { '٨', '8' }, { '٩', '9' },
            { '٫', '.' }, { '٬', ',' }, { '٪', '%' }
        };

        /// <summary>
        /// Dictionary mapping Arabic character variations to their standard forms
        /// </summary>
        public static readonly Dictionary<char, char> ArabicTextUnification = new()
        {
            { 'أ', 'ا' }, { 'إ', 'ا' }, { 'آ', 'ا' }, { 'ٱ', 'ا' }, { 'ؤ', 'و' },
            { 'ة', 'ه' },
            { 'ى', 'ي' }, { 'ی', 'ي' }, { 'ۍ', 'ي' }, { 'ے', 'ي' }, { 'ۓ', 'ي' },
            { 'ک', 'ك' }, { 'ڪ', 'ك' }, { 'گ', 'ك' }, { 'ګ', 'ك' },
            { 'ڬ', 'ك' }, { 'ڭ', 'ك' }, { 'ڮ', 'ك' }, { 'ڰ', 'ك' },
            { 'ڱ', 'ك' }, { 'ڲ', 'ك' }, { 'ڳ', 'ك' }, { 'ڴ', 'ك' },
            { 'ہ', 'ه' }, { 'ھ', 'ه' }, { 'ڿ', 'ه' }, { 'ۀ', 'ه' },
            { 'ۂ', 'ه' }, { 'ۃ', 'ه' },
            { 'ڨ', 'ق' }, { 'ڧ', 'ق' },
            { 'ڤ', 'ف' }, { 'ڡ', 'ف' }, { 'ڢ', 'ف' }, { 'ڣ', 'ف' },
            { 'ڥ', 'ف' }, { 'ڦ', 'ف' },
            { 'چ', 'ج' },
            { 'ژ', 'ز' }, { 'ڙ', 'ز' },
            { 'پ', 'ب' }, { 'ٻ', 'ب' }, { 'ڀ', 'ب' },
            { 'ٹ', 'ت' }, { 'ٺ', 'ت' }, { 'ټ', 'ت' }, { 'ٽ', 'ت' },
            { 'ڊ', 'د' }, { 'ڋ', 'د' }, { 'ڌ', 'د' }, { 'ڍ', 'د' },
            { 'ڑ', 'ر' }, { 'ڒ', 'ر' }, { 'ړ', 'ر' }, { 'ڔ', 'ر' },
            { 'ڕ', 'ر' }, { 'ږ', 'ر' }, { 'ڗ', 'ر' },
            { 'ښ', 'س' }, { 'ڛ', 'س' }, { 'ڜ', 'س' },
            { 'ڝ', 'ص' }, { 'ڞ', 'ص' },
            { 'ڟ', 'ط' },
            { 'ڠ', 'ع' },
            { 'ڵ', 'ل' }, { 'ڶ', 'ل' }, { 'ڷ', 'ل' }, { 'ڸ', 'ل' },
            { 'ڹ', 'ن' }, { 'ں', 'ن' }, { 'ڻ', 'ن' }, { 'ڼ', 'ن' }, { 'ڽ', 'ن' },
            { 'ۄ', 'و' }, { 'ۅ', 'و' }, { 'ۆ', 'و' }, { 'ۇ', 'و' },
            { 'ۈ', 'و' }, { 'ۉ', 'و' }, { 'ۊ', 'و' }, { 'ۋ', 'ו' },
            { 'ێ', 'ي' }, { 'ې', 'ي' }, { 'ۑ', 'ي' }
        };

        /// <summary>
        /// Set of Arabic diacritical marks (tashkeel) that can be removed during normalization
        /// </summary>
        public static readonly HashSet<char> ArabicDiacritics = new()
        {
            'َ', 'ُ', 'ِ', 'ّ', 'ْ', 'ً', 'ٌ', 'ٍ', 'ٓ', 'ٰ', 'ٔ', 'ٕ',
            'ٖ', 'ٗ', '٘', 'ٙ', 'ٚ', 'ٛ', 'ٜ', 'ٝ', 'ٞ', 'ٟ'
        };

        /// <summary>
        /// Arabic tatweel character (kashida) used for text justification
        /// </summary>
        public const char ArabicTatweel = 'ـ';

        /// <summary>
        /// Arabic comma character
        /// </summary>
        public const char ArabicComma = '،';

        /// <summary>
        /// Arabic semicolon character
        /// </summary>
        public const char ArabicSemicolon = '؛';

        /// <summary>
        /// Arabic question mark
        /// </summary>
        public const char ArabicQuestionMark = '؟';

        /// <summary>
        /// Arabic decimal separator (Arabic-Indic)
        /// </summary>
        public const char ArabicDecimalSeparator = '٫';

        /// <summary>
        /// Arabic thousands separator (Arabic-Indic)
        /// </summary>
        public const char ArabicThousandsSeparator = '٬';

        /// <summary>
        /// Arabic percent sign
        /// </summary>
        public const char ArabicPercentSign = '٪';
    }
}
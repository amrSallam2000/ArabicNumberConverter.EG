namespace NumericValidation.EG.Models.NumbersToText
{
    #region Supporting Classes

    /// <summary>
    /// Result of number-to-words conversion with detailed information
    /// </summary>
    public class ConversionResult
    {
        /// <summary>The original decimal number before conversion</summary>
        public decimal OriginalNumber { get; set; }

        /// <summary>Indicates whether the number is negative</summary>
        public bool IsNegative { get; set; }

        /// <summary>Integer part of the number</summary>
        public long IntegerPart { get; set; }

        /// <summary>Fractional part of the number (as integer without decimal point)</summary>
        public int FractionalPart { get; set; }

        /// <summary>Integer part converted to words</summary>
        public string IntegerPartInWords { get; set; } = string.Empty; // ✅ إضافة قيمة افتراضية

        /// <summary>Fractional part converted to words</summary>
        public string FractionalPartInWords { get; set; } = string.Empty; // ✅ إضافة قيمة افتراضية

        /// <summary>Complete number converted to words including currency/unit</summary>
        public string FullText { get; set; } = string.Empty; // ✅ إضافة قيمة افتراضية

        /// <summary>Language used for conversion</summary>
        public NumberToWordsConverter.Language Language { get; set; }

        /// <summary>Currency/unit used for conversion (if applicable)</summary>
        public NumberToWordsConverter.Currency Currency { get; set; }

        /// <summary>
        /// Returns the full converted text
        /// </summary>
        /// <returns>The complete number in words</returns>
        public override string ToString()
        {
            return FullText;
        }
    }

    #endregion
}
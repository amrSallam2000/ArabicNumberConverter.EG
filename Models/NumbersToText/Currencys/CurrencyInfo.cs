namespace NumericValidation.EG.Models.NumbersToText.Currencys
{
    #region Supporting Classes

    /// <summary>
    /// Contains detailed information about a currency for number-to-words conversion
    /// Supports singular, dual, plural forms in Arabic and singular/plural in English
    /// </summary>
    public class CurrencyInfo
    {

        // Arabic
        public string MainUnitArabic { get; set; }       // Singular
        public string SubunitArabic { get; set; }       // Singular
        public string MainUnitDualArabic { get; set; }  // Dual
        public string SubunitDualArabic { get; set; }   // Dual
        public string MainUnitPluralArabic { get; set; } // Plural
        public string SubunitPluralArabic { get; set; }  // Plural

        // English
        public string MainUnitEnglish { get; set; }       // Singular
        public string SubunitEnglish { get; set; }        // Singular
        public string MainUnitPluralEnglish { get; set; } // Plural
        public string SubunitPluralEnglish { get; set; }  // Plural

        /// <summary>Indicates whether the currency has subunits</summary>
        public bool HasSubunit { get; set; }

        /// <summary>Conversion factor between main unit and subunit (e.g., 100 for most currencies, 1000 for KWD)</summary>
        public int SubunitFactor { get; set; }

        /// <summary>
        /// Initializes a new instance of the CurrencyInfo class
        /// </summary>
        public CurrencyInfo(
            string mainAr, string subAr, string mainEn, string subEn,
            string mainPluralAr, string subPluralAr, string mainPluralEn, string subPluralEn,
            string mainDualAr, string subDualAr,
            bool hasSubunit, int subunitFactor)
        {
            MainUnitArabic = mainAr;
            SubunitArabic = subAr;
            MainUnitDualArabic = mainDualAr;
            SubunitDualArabic = subDualAr;
            MainUnitPluralArabic = mainPluralAr;
            SubunitPluralArabic = subPluralAr;
            MainUnitEnglish = mainEn;
            SubunitEnglish = subEn;
            MainUnitPluralEnglish = mainPluralEn;
            SubunitPluralEnglish = subPluralEn;
            HasSubunit = hasSubunit;
            SubunitFactor = subunitFactor;
        }
    }

    #endregion
}

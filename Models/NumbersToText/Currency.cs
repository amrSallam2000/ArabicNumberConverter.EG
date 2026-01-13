namespace NumericValidation.EG.Models.NumbersToText
{
    public partial class NumberToWordsConverter
    {
        /// <summary>
        /// Supported currencies for number-to-words conversion
        /// </summary>
        public enum Currency
        {
            /// <summary>Egyptian Pound / جنيه مصري</summary>
            EGP,

            /// <summary>United States Dollar / دولار أمريكي</summary>
            USD,

            /// <summary>Euro / يورو</summary>
            EUR,

            /// <summary>British Pound / جنيه إسترليني</summary>
            GBP,

            /// <summary>Saudi Riyal / ريال سعودي</summary>
            SAR,

            /// <summary>UAE Dirham / درهم إماراتي</summary>
            AED,

            /// <summary>Kuwaiti Dinar / دينار كويتي</summary>
            KWD,

            /// <summary>Qatari Riyal / ريال قطري</summary>
            QAR,

            /// <summary>Bahraini Dinar / دينار بحريني</summary>
            BHD,

            /// <summary>Omani Rial / ريال عماني</summary>
            OMR,

            /// <summary>Jordanian Dinar / دينار أردني</summary>
            JOD,

            /// <summary>Lebanese Pound / ليرة لبنانية</summary>
            LBP,

            /// <summary>Syrian Pound / ليرة سورية</summary>
            SYP,

            /// <summary>Iraqi Dinar / دينار عراقي</summary>
            IQD,

            /// <summary>Yemeni Rial / ريال يمني</summary>
            YER,

            /// <summary>Moroccan Dirham / درهم مغربي</summary>
            MAD,

            /// <summary>Tunisian Dinar / دينار تونسي</summary>
            TND,

            /// <summary>Algerian Dinar / دينار جزائري</summary>
            DZD,

            /// <summary>Libyan Dinar / دينار ليبي</summary>
            LYD,

            /// <summary>Sudanese Pound / جنيه سوداني</summary>
            SDG,

            /// <summary>Generic currency without subunits / عملة عامة بدون وحدات فرعية</summary>
            Generic
        }
    }
}
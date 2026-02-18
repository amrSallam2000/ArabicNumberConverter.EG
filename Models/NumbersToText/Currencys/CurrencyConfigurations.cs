using System.Collections.Generic;
using NumericValidation.EG.Models.NumbersToText.Enums;
using static NumericValidation.EG.Models.NumbersToText.Enums.NumberToWordsConverter;
namespace NumericValidation.EG.Models.NumbersToText.Currencys
{
    /// <summary>
    /// Supported currency configurations
    /// </summary>
    public static class CurrencyConfigurations
    {
        /// <summary>
        /// Dictionary containing all supported currencies with their linguistic configurations
        /// </summary>
        public static readonly Dictionary<Currency, CurrencyInfo> CurrencyData = new Dictionary<Currency, CurrencyInfo>
        {
            { Currency.EGP, new CurrencyInfo(
                "جنيه مصري", "قرش",
                "Pound", "Piaster",
                "جنيهات مصرية", "قروش",
                "Pounds", "Piasters",
                "جنيهان مصريان", "قرشان",
                true, 100) },

            { Currency.USD, new CurrencyInfo(
                "دولار أمريكي", "سنت",
                "Dollar", "Cent",
                "دولارات أمريكية", "سنتات",
                "Dollars", "Cents",
                "دولاران أمريكيان", "سنتان",
                true, 100) },

            { Currency.EUR, new CurrencyInfo(
                "يورو", "سنت",
                "Euro", "Cent",
                "يورو", "سنتات",
                "Euros", "Cents",
                "يوروَان", "سنتان",
                true, 100) },

            { Currency.GBP, new CurrencyInfo(
                "جنيه إسترليني", "بنس",
                "Pound Sterling", "Pence",
                "جنيهات إسترلينية", "بنسات",
                "Pounds", "Pence",
                "جنيهان إسترلينيان", "بنسَان",
                true, 100) },

            { Currency.SAR, new CurrencyInfo(
                "ريال سعودي", "هللة",
                "Riyal", "Halala",
                "ريالات سعودية", "هللات",
                "Riyals", "Halalas",
                "ريالان سعوديان", "هللتان",
                true, 100) },

            { Currency.AED, new CurrencyInfo(
                "درهم إماراتي", "فلس",
                "Dirham", "Fils",
                "دراهم إماراتية", "فلوس",
                "Dirhams", "Fils",
                "درهمان إماراتيان", "فلسان",
                true, 100) },

            { Currency.KWD, new CurrencyInfo(
                "دينار كويتي", "فلس",
                "Dinar", "Fils",
                "دنانير كويتية", "فلوس",
                "Dinars", "Fils",
                "ديناران كويتيان", "فلسان",
                true, 1000) },

            { Currency.QAR, new CurrencyInfo(
                "ريال قطري", "درهم",
                "Riyal", "Dirham",
                "ريالات قطرية", "دراهم",
                "Riyals", "Dirhams",
                "ريالان قطريان", "درهمان",
                true, 100) },

            { Currency.BHD, new CurrencyInfo(
                "دينار بحريني", "فلس",
                "Dinar", "Fils",
                "دنانير بحرينية", "فلوس",
                "Dinars", "Fils",
                "ديناران بحرينيان", "فلسان",
                true, 1000) },

            { Currency.OMR, new CurrencyInfo(
                "ريال عماني", "بيسة",
                "Rial", "Baisa",
                "ريالات عمانية", "بيسات",
                "Rials", "Baisas",
                "ريالان عمانيان", "بيستان",
                true, 1000) },

            { Currency.JOD, new CurrencyInfo(
                "دينار أردني", "قرش",
                "Dinar", "Piaster",
                "دنانير أردنية", "قروش",
                "Dinars", "Piasters",
                "ديناران أردنيان", "قرشان",
                true, 1000) },

            { Currency.LBP, new CurrencyInfo(
                "ليرة لبنانية", "قرش",
                "Pound", "Piaster",
                "ليرات لبنانية", "قروش",
                "Pounds", "Piasters",
                "ليرتان لبنانيتان", "قرشان",
                true, 100) },

            { Currency.SYP, new CurrencyInfo(
                "ليرة سورية", "قرش",
                "Pound", "Piaster",
                "ليرات سورية", "قروش",
                "Pounds", "Piasters",
                "ليرتان سوريتان", "قرشان",
                true, 100) },

            { Currency.IQD, new CurrencyInfo(
                "دينار عراقي", "فلس",
                "Dinar", "Fils",
                "دنانير عراقية", "فلوس",
                "Dinars", "Fils",
                "ديناران عراقيان", "فلسان",
                true, 1000) },

            { Currency.YER, new CurrencyInfo(
                "ريال يمني", "فلس",
                "Rial", "Fils",
                "ريالات يمنية", "فلوس",
                "Rials", "Fils",
                "ريالان يمنيان", "فلسان",
                true, 100) },

            { Currency.MAD, new CurrencyInfo(
                "درهم مغربي", "سنتيم",
                "Dirham", "Centime",
                "دراهم مغربية", "سنتيمات",
                "Dirhams", "Centimes",
                "درهمان مغربيان", "سنتيمان",
                true, 100) },

            { Currency.TND, new CurrencyInfo(
                "دينار تونسي", "مليم",
                "Dinar", "Millime",
                "دنانير تونسية", "ملاليم",
                "Dinars", "Millimes",
                "ديناران تونسيان", "مليمان",
                true, 1000) },

            { Currency.DZD, new CurrencyInfo(
                "دينار جزائري", "سنتيم",
                "Dinar", "Centime",
                "دنانير جزائرية", "سنتيمات",
                "Dinars", "Centimes",
                "ديناران جزائريان", "سنتيمان",
                true, 100) },

            { Currency.LYD, new CurrencyInfo(
                "دينار ليبي", "درهم",
                "Dinar", "Dirham",
                "دنانير ليبية", "دراهم",
                "Dinars", "Dirhams",
                "ديناران ليبيان", "درهمان",
                true, 1000) },

            { Currency.SDG, new CurrencyInfo(
                "جنيه سوداني", "قرش",
                "Pound", "Piaster",
                "جنيهات سودانية", "قروش",
                "Pounds", "Piasters",
                "جنيهان سودانيان", "قرشان",
                true, 100) },

            {Currency.Generic, new CurrencyInfo(
                "وحدة", "جزء",
                "Unit", "Fraction",
                "وحدات", "أجزاء",
                "Units", "Fractions",
                "وحدتان", "جزآن",
                true, 100) }
        };
    }
}
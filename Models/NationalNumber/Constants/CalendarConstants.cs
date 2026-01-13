using System.Collections.Generic;

namespace NumericValidation.EG.Models.NationalNumber.Constants
{
    /// <summary>
    /// Calendar and century constants
    /// </summary>
    public static class CalendarConstants
    {

        /// <summary>
        /// Century names in Arabic and English
        /// </summary>
        public static readonly Dictionary<int, (string Arabic, string English)> Centuries = new()
        {
            { 1800, ("التاسع عشر", "Nineteenth") },
            { 1900, ("العشرون", "Twentieth") },
            { 2000, ("الحادي والعشرون", "Twenty-first") }
        };

        /// <summary>
        /// Gets century from year
        /// </summary>
        public static int GetCenturyFromYear(int year)
        {
            return (year / 100) * 100;
        }

        /// <summary>
        /// Gets Arabic century name
        /// </summary>
        public static string GetCenturyNameArabic(int century)
        {
            return Centuries.TryGetValue(century, out var name) ? name.Arabic : "غير معروف";
        }

        /// <summary>
        /// Gets English century name
        /// </summary>
        public static string GetCenturyNameEnglish(int century)
        {
            return Centuries.TryGetValue(century, out var name) ? name.English : "Unknown";
        }

        /// <summary>
        /// Egyptian governorates with codes
        /// </summary>
        public static readonly Dictionary<string, (string Arabic, string English)> Governorates = new()
        {
            { "01", ("القاهرة", "Cairo") },
            { "02", ("الإسكندرية", "Alexandria") },
            { "03", ("بورسعيد", "Port Said") },
            { "04", ("السويس", "Suez") },
            { "11", ("دمياط", "Damietta") },
            { "12", ("الدقهلية", "Dakahlia") },
            { "13", ("الشرقية", "Sharqia") },
            { "14", ("القليوبية", "Qalyubia") },
            { "15", ("كفر الشيخ", "Kafr El Sheikh") },
            { "16", ("الغربية", "Gharbia") },
            { "17", ("المنوفية", "Monufia") },
            { "18", ("البحيرة", "Beheira") },
            { "19", ("الإسماعيلية", "Ismailia") },
            { "21", ("الجيزة", "Giza") },
            { "22", ("بني سويف", "Beni Suef") },
            { "23", ("الفيوم", "Faiyum") },
            { "24", ("المنيا", "Minya") },
            { "25", ("أسيوط", "Assiut") },
            { "26", ("سوهاج", "Sohag") },
            { "27", ("قنا", "Qena") },
            { "28", ("أسوان", "Aswan") },
            { "29", ("الأقصر", "Luxor") },
            { "31", ("البحر الأحمر", "Red Sea") },
            { "32", ("الوادي الجديد", "New Valley") },
            { "33", ("مطروح", "Matrouh") },
            { "34", ("شمال سيناء", "North Sinai") },
            { "35", ("جنوب سيناء", "South Sinai") },
            { "88", ("خارج مصر", "Outside Egypt") }
        };
    }
}
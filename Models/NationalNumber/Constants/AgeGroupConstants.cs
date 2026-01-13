using System.Collections.Generic;

namespace NumericValidation.EG.Models.NationalNumber.Constants
{
    /// <summary>
    /// Age groups and categories
    /// </summary>
    public static class AgeGroupConstants
    {
        /// <summary>
        /// Gets age group in English based on age
        /// </summary>
        public static string GetAgeGroupEnglish(int age)
        {
            if (age < 0) return "Invalid";
            if (age < 1) return "Infant";
            if (age < 3) return "Toddler";
            if (age < 13) return "Child";
            if (age < 18) return "Teenager";
            if (age < 30) return "Young Adult";
            if (age < 60) return "Adult";
            return "Senior";
        }

        /// <summary>
        /// Gets age group in Arabic based on age
        /// </summary>
        public static string GetAgeGroupArabic(int age)
        {
            if (age < 0) return "غير صالح";
            if (age < 1) return "رضيع";
            if (age < 3) return "طفل صغير";
            if (age < 13) return "طفل";
            if (age < 18) return "مراهق";
            if (age < 30) return "شاب";
            if (age < 60) return "بالغ";
            return "مسن";
        }

        /// <summary>
        /// Age categories for different life stages
        /// </summary>
        public static readonly Dictionary<string, (int Min, int Max)> AgeCategories = new()
        {
            { "Infant", (0, 1) },
            { "Toddler", (1, 3) },
            { "Child", (3, 13) },
            { "Teenager", (13, 18) },
            { "Young Adult", (18, 30) },
            { "Adult", (30, 60) },
            { "Senior", (60, 120) }
        };
    }
}
namespace NumericValidation.EG.Models.NumbersToText
{
    /// <summary>
    /// Arabic number words for different numeric values
    /// </summary>
    public static class ArabicNumberWords
    {
        /// <summary>
        /// Used when the number is in the tens place (e.g., واحد وعشرون - twenty-one)
        /// </summary>
        public static readonly string[] OnesForTens =
            { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };

        /// <summary>
        /// Used when the number stands alone (0-9)
        /// </summary>
        public static readonly string[] OnesAlone =
            { "صفر", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };

        /// <summary>
        /// Used with plural scale words (thousands, millions)
        /// </summary>
        public static readonly string[] OnesForPlural =
            { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };

        /// <summary>
        /// Tens (10-90)
        /// </summary>
        public static readonly string[] Tens =
            { "", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };

        /// <summary>
        /// Hundreds (100-900) - Fixed with correct Arabic forms
        /// </summary>
        public static readonly string[] Hundreds =
            { "", "مئة", "مئتان", "ثلاثمئة", "أربعمئة", "خمسمئة", "ستمئة", "سبعمئة", "ثمانمئة", "تسعمئة" };

        /// <summary>
        /// Numbers from 10 to 19 - Special forms in Arabic
        /// </summary>
        public static readonly string[] Teens =
            { "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };

        /// <summary>
        /// Scale words (thousands, millions, etc.) in singular form
        /// </summary>
        public static readonly string[] Scales =          // Singular
            { "", "ألف", "مليون", "مليار", "تريليون" };

        /// <summary>
        /// Scale words in dual form (for number 2)
        /// </summary>
        public static readonly string[] ScalesDual =      // Dual (for number 2)
            { "", "ألفان", "مليونان", "ملياران", "تريليونان" };

        /// <summary>
        /// Scale words in plural form (for numbers 3-10)
        /// </summary>
        public static readonly string[] ScalesPlural =    // Plural (3-10)
            { "", "آلاف", "ملايين", "مليارات", "تريليونات" };
    }
}
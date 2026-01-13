using System;
using System.Collections.Generic;

namespace NumericValidation.EG.Models.NationalNumber.Constants
{
    /// <summary>
    /// Arabic language constants for dates, months, days, etc.
    /// </summary>
    public static class ArabicConstants
    {
        /// <summary>
        /// Arabic month names with English equivalents
        /// </summary>
        public static readonly Dictionary<int, (string Arabic, string English)> MonthNames = new()
        {
            { 1, ("يناير", "January") },
            { 2, ("فبراير", "February") },
            { 3, ("مارس", "March") },
            { 4, ("أبريل", "April") },
            { 5, ("مايو", "May") },
            { 6, ("يونيو", "June") },
            { 7, ("يوليو", "July") },
            { 8, ("أغسطس", "August") },
            { 9, ("سبتمبر", "September") },
            { 10, ("أكتوبر", "October") },
            { 11, ("نوفمبر", "November") },
            { 12, ("ديسمبر", "December") }
        };

        /// <summary>
        /// Arabic day names with English equivalents
        /// </summary>
        public static readonly Dictionary<DayOfWeek, (string Arabic, string English)> DayNames = new()
        {
            { DayOfWeek.Saturday, ("السبت", "Saturday") },
            { DayOfWeek.Sunday, ("الأحد", "Sunday") },
            { DayOfWeek.Monday, ("الاثنين", "Monday") },
            { DayOfWeek.Tuesday, ("الثلاثاء", "Tuesday") },
            { DayOfWeek.Wednesday, ("الأربعاء", "Wednesday") },
            { DayOfWeek.Thursday, ("الخميس", "Thursday") },
            { DayOfWeek.Friday, ("الجمعة", "Friday") }
        };

        /// <summary>
        /// Gets Arabic month name
        /// </summary>
        public static string GetMonthNameArabic(int month)
        {
            return MonthNames.TryGetValue(month, out var name) ? name.Arabic : string.Empty;
        }

        /// <summary>
        /// Gets English month name
        /// </summary>
        public static string GetMonthNameEnglish(int month)
        {
            return MonthNames.TryGetValue(month, out var name) ? name.English : string.Empty;
        }

        /// <summary>
        /// Gets Arabic day name
        /// </summary>
        public static string GetDayNameArabic(DayOfWeek day)
        {
            return DayNames.TryGetValue(day, out var name) ? name.Arabic : string.Empty;
        }

        /// <summary>
        /// Gets English day name
        /// </summary>
        public static string GetDayNameEnglish(DayOfWeek day)
        {
            return DayNames.TryGetValue(day, out var name) ? name.English : string.Empty;
        }
    }
}
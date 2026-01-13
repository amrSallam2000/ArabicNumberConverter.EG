
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NumericValidation.EG.Models.NationalNumber.Constants
{
    /// <summary>
    /// List of all zodiac signs with their date ranges and names
    /// </summary>
    public static class ZodiacConstants
    {
        public static readonly List<ZodiacSign> ZodiacSigns = new()
        {
            // الصيغة الجديدة: (الشهر، اليوم) فقط بدون سنوات
            new ZodiacSign("Aries", "الحمل", "♈", 3, 21, 4, 19),
            new ZodiacSign("Taurus", "الثور", "♉", 4, 20, 5, 20),
            new ZodiacSign("Gemini", "الجوزاء", "♊", 5, 21, 6, 20),
            new ZodiacSign("Cancer", "السرطان", "♋", 6, 21, 7, 22),
            new ZodiacSign("Leo", "الأسد", "♌", 7, 23, 8, 22),
            new ZodiacSign("Virgo", "العذراء", "♍", 8, 23, 9, 22),
            new ZodiacSign("Libra", "الميزان", "♎", 9, 23, 10, 22),
            new ZodiacSign("Scorpio", "العقرب", "♏", 10, 23, 11, 21),
            new ZodiacSign("Sagittarius", "القوس", "♐", 11, 22, 12, 21),
            new ZodiacSign("Capricorn", "الجدي", "♑", 12, 22, 1, 19),
            new ZodiacSign("Aquarius", "الدلو", "♒", 1, 20, 2, 18),
            new ZodiacSign("Pisces", "الحوت", "♓", 2, 19, 3, 20)
        };

        /// <summary>
        /// Represents information about a zodiac sign
        /// </summary>
        public record ZodiacSign(
            string English,
            string Arabic,
            string Symbol,
            int StartMonth,
            int StartDay,
            int EndMonth,
            int EndDay
        );

        /// <summary>
        /// Determines the zodiac sign for a given date
        /// </summary>
        public static ZodiacSign GetZodiacSign(DateTime date)
        {
            int month = date.Month;
            int day = date.Day;

            foreach (var zodiac in ZodiacSigns)
            {
                // Handle signs that cross year boundary (Capricorn)
                if (zodiac.StartMonth > zodiac.EndMonth)
                {
                    // مثل الجدي: ديسمبر 22 إلى يناير 19
                    if ((month == zodiac.StartMonth && day >= zodiac.StartDay) ||
                        (month == zodiac.EndMonth && day <= zodiac.EndDay) ||
                        (month > zodiac.StartMonth) || // ديسمبر بعد 22
                        (month < zodiac.EndMonth))    // يناير قبل 19
                    {
                        return zodiac;
                    }
                }
                else
                {
                    // Signs within same year
                    if ((month == zodiac.StartMonth && day >= zodiac.StartDay) ||
                        (month == zodiac.EndMonth && day <= zodiac.EndDay) ||
                        (month > zodiac.StartMonth && month < zodiac.EndMonth))
                    {
                        return zodiac;
                    }
                }
            }

            // Fallback: Capricorn
            return ZodiacSigns.First(z => z.English == "Capricorn");
        }
    }
}

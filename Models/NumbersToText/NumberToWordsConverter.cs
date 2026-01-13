using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumericValidation.EG.Models.NumbersToText
{
    /// <summary>
    /// Advanced number to words converter supporting Arabic and English languages with multi-currency support
    /// (Arabic grammar fixed, no extra 'and', precise fractions)
    /// </summary>
    public partial class NumberToWordsConverter
    {
        #region Main Conversion Methods

        /// <summary>
        /// Converts a number to words with optional currency
        /// </summary>
        /// <param name="number">The decimal number to convert</param>
        /// <param name="language">Language for conversion (Arabic/English)</param>
        /// <param name="currency">Currency type for monetary amounts</param>
        /// <param name="includeCurrency">Whether to include currency names in output</param>
        /// <returns>Number converted to words in specified language</returns>
        public static string Convert(decimal number, Language language = Language.Arabic, Currency currency = Currency.EGP, bool includeCurrency = true)
        {
            try
            {
                // Validate number range (supports up to 999 billion)
                if (Math.Abs(number) > 999999999999m)
                {
                    return language == Language.Arabic
                        ? "خطأ: الرقم كبير جدًا (أكثر من 999 مليار)"
                        : "Error: Number too large (exceeds 999 billion)";
                }

                // Handle zero case
                if (number == 0)
                    return language == Language.Arabic ? "صفر" : "Zero";

                // Extract sign and work with absolute value
                bool isNegative = number < 0;
                number = Math.Abs(number);

                var currencyInfo = CurrencyConfigurations.CurrencyData[currency];

                // Separate integer and fractional parts
                long integerPart = (long)Math.Truncate(number);

                // Precise fraction handling with rounding tolerance
                decimal fraction = (number - integerPart) * currencyInfo.SubunitFactor;
                int fractionalPart = (int)Math.Floor(fraction + 0.0000001m);

                // Handle overflow (e.g., 99.999 becomes 100)
                if (fractionalPart >= currencyInfo.SubunitFactor)
                {
                    integerPart++;
                    fractionalPart = 0;
                }

                StringBuilder result = new StringBuilder();

                // Add negative prefix if needed
                if (isNegative)
                {
                    result.Append(language == Language.Arabic ? "سالب " : "Negative ");
                }

                // Arabic conversion logic
                if (language == Language.Arabic)
                {
                    if (integerPart > 0)
                    {
                        result.Append(ConvertIntegerToArabic(integerPart));

                        if (includeCurrency)
                        {
                            result.Append(" ");
                            result.Append(GetCurrencyNameArabic(integerPart, currencyInfo));
                        }
                    }
                    else if (integerPart == 0 && fractionalPart == 0)
                    {
                        result.Append("صفر");

                        if (includeCurrency)
                        {
                            result.Append(" ");
                            result.Append(GetCurrencyNameArabic(1, currencyInfo));
                        }
                    }

                    // Add fractional part if exists and currency has subunits
                    if (fractionalPart > 0 && currencyInfo.HasSubunit)
                    {
                        if (integerPart > 0)
                            result.Append(" و");  // Arabic conjunction "and"

                        result.Append(ConvertIntegerToArabic(fractionalPart));

                        if (includeCurrency)
                        {
                            result.Append(" ");
                            result.Append(GetSubunitNameArabic(fractionalPart, currencyInfo));
                        }
                    }
                }
                else // English conversion logic
                {
                    if (integerPart > 0)
                    {
                        result.Append(ConvertIntegerToEnglish(integerPart));

                        if (includeCurrency)
                        {
                            result.Append(" ");
                            result.Append(GetCurrencyNameEnglish(integerPart, currencyInfo));
                        }
                    }
                    else if (integerPart == 0 && fractionalPart == 0)
                    {
                        result.Append("Zero");

                        if (includeCurrency)
                        {
                            result.Append(" ");
                            result.Append(GetCurrencyNameEnglish(1, currencyInfo));
                        }
                    }

                    // Add fractional part if exists and currency has subunits
                    if (fractionalPart > 0 && currencyInfo.HasSubunit)
                    {
                        if (integerPart > 0)
                            result.Append(" and ");  // English conjunction

                        result.Append(ConvertIntegerToEnglish(fractionalPart));

                        if (includeCurrency)
                        {
                            result.Append(" ");
                            result.Append(GetSubunitNameEnglish(fractionalPart, currencyInfo));
                        }
                    }
                }

                return result.ToString().Trim();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Converts number to Arabic words without currency
        /// </summary>
        public static string ConvertToArabic(decimal number)
        {
            return Convert(number, Language.Arabic, Currency.Generic, false);
        }

        /// <summary>
        /// Converts number to English words without currency
        /// </summary>
        public static string ConvertToEnglish(decimal number)
        {
            return Convert(number, Language.English, Currency.Generic, false);
        }

        #endregion

        #region Arabic Conversion Logic

        // Main Arabic conversion method - routes to appropriate converter based on number size
        private static string ConvertIntegerToArabic(long number)
        {
            if (number == 0) return "صفر";
            if (number < 10) return ArabicNumberWords.OnesAlone[number];
            if (number < 20) return ArabicNumberWords.Teens[number - 10];
            if (number < 100) return ConvertTensArabic(number);
            if (number < 1000) return ConvertHundredsArabic(number);
            if (number < 1000000) return ConvertThousandsArabic(number);
            if (number < 1000000000) return ConvertMillionsArabic(number);
            return ConvertBillionsArabic(number);
        }

        // Convert tens (20-99) in Arabic
        private static string ConvertTensArabic(long number)
        {
            int tens = (int)(number / 10);
            int ones = (int)(number % 10);

            if (ones == 0)
                return ArabicNumberWords.Tens[tens];

            // Format: واحد وعشرون (ones and tens)
            return ArabicNumberWords.OnesForTens[ones] + " و" + ArabicNumberWords.Tens[tens];
        }

        // Convert hundreds (100-999) in Arabic
        private static string ConvertHundredsArabic(long number)
        {
            int hundreds = (int)(number / 100);
            int remainder = (int)(number % 100);

            string result = ArabicNumberWords.Hundreds[hundreds];

            if (remainder > 0)
                result += " و" + ConvertIntegerToArabic(remainder);

            return result;
        }

        // Convert thousands (1,000-999,999) in Arabic
        private static string ConvertThousandsArabic(long number)
        {
            if (number < 1000)
                return ConvertHundredsArabic(number);

            long thousands = number / 1000;
            long remainder = number % 1000;

            string thousandsText = GetScaleTextArabic(thousands, 1);  // 1 = thousands scale
            string result = thousandsText;

            if (remainder > 0)
                result += " و" + ConvertIntegerToArabic(remainder);

            return result;
        }

        // Convert millions (1,000,000-999,999,999) in Arabic
        private static string ConvertMillionsArabic(long number)
        {
            if (number < 1000000)
                return ConvertThousandsArabic(number);

            long millions = number / 1000000;
            long remainder = number % 1000000;

            string millionsText = GetScaleTextArabic(millions, 2);  // 2 = millions scale
            string result = millionsText;

            if (remainder > 0)
                result += " و" + ConvertIntegerToArabic(remainder);

            return result;
        }

        // Convert billions (1,000,000,000+) in Arabic
        private static string ConvertBillionsArabic(long number)
        {
            if (number < 1000000000)
                return ConvertMillionsArabic(number);

            long billions = number / 1000000000;
            long remainder = number % 1000000000;

            string billionsText = GetScaleTextArabic(billions, 3);  // 3 = billions scale
            string result = billionsText;

            if (remainder > 0)
                result += " و" + ConvertIntegerToArabic(remainder);

            return result;
        }

        // Gets the appropriate scale word (thousand, million, etc.) in Arabic
        // Handles singular, dual, and plural forms based on the number
        private static string GetScaleTextArabic(long number, int scaleIndex)
        {
            if (number == 1)
                return ArabicNumberWords.Scales[scaleIndex];

            if (number == 2)
                return ArabicNumberWords.ScalesDual[scaleIndex];

            string numberText = ConvertScaleNumberArabic(number, scaleIndex);

            // Numbers 3-10 use plural scale words
            if (number >= 3 && number <= 10)
                return numberText + " " + ArabicNumberWords.ScalesPlural[scaleIndex];

            // Numbers above 10 use singular scale words
            return numberText + " " + ArabicNumberWords.Scales[scaleIndex];
        }

        // Converts the number portion that precedes scale words
        private static string ConvertScaleNumberArabic(long number, int scaleIndex)
        {
            if (number < 10) return ArabicNumberWords.OnesForPlural[number];
            if (number < 20) return ArabicNumberWords.Teens[number - 10];
            if (number < 100) return ConvertTensArabic(number);
            if (number < 1000) return ConvertHundredsArabic(number);
            return ConvertIntegerToArabic(number);
        }

        #endregion

        #region English Conversion Logic (No extra "and")

        // Main English conversion method - routes to appropriate converter based on number size
        private static string ConvertIntegerToEnglish(long number)
        {
            if (number == 0) return "Zero";
            if (number < 10) return EnglishNumberWords.Ones[number];
            if (number < 20) return EnglishNumberWords.Teens[number - 10];
            if (number < 100) return ConvertTensEnglish(number);
            if (number < 1000) return ConvertHundredsEnglish(number);
            if (number < 1000000) return ConvertThousandsEnglish(number);
            if (number < 1000000000) return ConvertMillionsEnglish(number);
            return ConvertBillionsEnglish(number);
        }

        // Convert tens (20-99) in English
        private static string ConvertTensEnglish(long number)
        {
            int tens = (int)(number / 10);
            int ones = (int)(number % 10);

            if (ones == 0)
                return EnglishNumberWords.Tens[tens];

            return EnglishNumberWords.Tens[tens] + " " + EnglishNumberWords.Ones[ones];
        }

        // Convert hundreds (100-999) in English
        private static string ConvertHundredsEnglish(long number)
        {
            int hundreds = (int)(number / 100);
            int remainder = (int)(number % 100);

            string result = EnglishNumberWords.Hundreds[hundreds];

            if (remainder > 0)
                result += " and " + ConvertIntegerToEnglish(remainder);

            return result;
        }

        // Convert thousands (1,000-999,999) in English
        private static string ConvertThousandsEnglish(long number)
        {
            if (number < 1000)
                return ConvertHundredsEnglish(number);

            long thousands = number / 1000;
            long remainder = number % 1000;

            string result = ConvertIntegerToEnglish(thousands) + " Thousand";

            if (remainder > 0)
            {
                // Use "and" only when remainder is less than 100
                if (remainder < 100)
                    result += " and " + ConvertIntegerToEnglish(remainder);
                else
                    result += " " + ConvertHundredsEnglish(remainder);
            }

            return result;
        }

        // Convert millions (1,000,000-999,999,999) in English
        private static string ConvertMillionsEnglish(long number)
        {
            if (number < 1000000)
                return ConvertThousandsEnglish(number);

            long millions = number / 1000000;
            long remainder = number % 1000000;

            string result = ConvertIntegerToEnglish(millions) + " Million";

            if (remainder > 0)
            {
                if (remainder < 100)
                    result += " and " + ConvertIntegerToEnglish(remainder);
                else
                    result += " " + ConvertThousandsEnglish(remainder);
            }

            return result;
        }

        // Convert billions (1,000,000,000+) in English
        private static string ConvertBillionsEnglish(long number)
        {
            if (number < 1000000000)
                return ConvertMillionsEnglish(number);

            long billions = number / 1000000000;
            long remainder = number % 1000000000;

            string result = ConvertIntegerToEnglish(billions) + " Billion";

            if (remainder > 0)
            {
                if (remainder < 100)
                    result += " and " + ConvertIntegerToEnglish(remainder);
                else
                    result += " " + ConvertMillionsEnglish(remainder);
            }

            return result;
        }

        #endregion

        #region Currency Helper Methods (Arabic: singular/dual/plural)

        // Gets Arabic currency name based on amount (singular/dual/plural)
        private static string GetCurrencyNameArabic(long amount, CurrencyInfo info)
        {
            if (amount == 1)
                return info.MainUnitArabic;
            if (amount == 2)
                return info.MainUnitDualArabic;
            if (amount >= 3 && amount <= 10)
                return info.MainUnitPluralArabic;
            return info.MainUnitArabic;              // Above 10 uses singular
        }

        // Gets Arabic subunit name based on amount (singular/dual/plural)
        private static string GetSubunitNameArabic(int amount, CurrencyInfo info)
        {
            if (amount == 1)
                return info.SubunitArabic;
            if (amount == 2)
                return info.SubunitDualArabic;
            if (amount >= 3 && amount <= 10)
                return info.SubunitPluralArabic;
            return info.SubunitArabic;
        }

        // Gets English currency name (singular/plural only)
        private static string GetCurrencyNameEnglish(long amount, CurrencyInfo info)
        {
            return amount == 1 ? info.MainUnitEnglish : info.MainUnitPluralEnglish;
        }

        // Gets English subunit name (singular/plural only)
        private static string GetSubunitNameEnglish(int amount, CurrencyInfo info)
        {
            return amount == 1 ? info.SubunitEnglish : info.SubunitPluralEnglish;
        }

        #endregion

        #region Additional Helper Methods

        /// <summary>
        /// Returns all supported currencies and their configurations
        /// </summary>
        public static Dictionary<Currency, CurrencyInfo> GetSupportedCurrencies()
        {
            return new Dictionary<Currency, CurrencyInfo>(CurrencyConfigurations.CurrencyData);
        }

        /// <summary>
        /// Converts number with detailed breakdown of conversion parts
        /// </summary>
        public static ConversionResult ConvertWithDetails(decimal number, Language language = Language.Arabic, Currency currency = Currency.EGP)
        {
            var result = new ConversionResult
            {
                OriginalNumber = number,
                Language = language,
                Currency = currency,
                IsNegative = number < 0
            };

            number = Math.Abs(number);
            var currencyInfo = CurrencyConfigurations.CurrencyData[currency];

            result.IntegerPart = (long)Math.Truncate(number);

            // Calculate fractional part with precision
            decimal fraction = (number - result.IntegerPart) * currencyInfo.SubunitFactor;
            result.FractionalPart = (int)Math.Floor(fraction + 0.0000001m);
            if (result.FractionalPart >= currencyInfo.SubunitFactor)
            {
                result.IntegerPart++;
                result.FractionalPart = 0;
            }

            // Convert parts to words
            if (language == Language.Arabic)
            {
                result.IntegerPartInWords = ConvertIntegerToArabic(result.IntegerPart);
                result.FractionalPartInWords = result.FractionalPart > 0
                    ? ConvertIntegerToArabic(result.FractionalPart)
                    : "";
            }
            else
            {
                result.IntegerPartInWords = ConvertIntegerToEnglish(result.IntegerPart);
                result.FractionalPartInWords = result.FractionalPart > 0
                    ? ConvertIntegerToEnglish(result.FractionalPart)
                    : "";
            }

            // Generate full text with currency
            result.FullText = Convert(number * (result.IsNegative ? -1 : 1), language, currency, true);

            return result;
        }

        #endregion
    }
}
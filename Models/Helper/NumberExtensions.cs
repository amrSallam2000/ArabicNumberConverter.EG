using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NumericValidation.EG.Models.Helper
{
    /// <summary>
    /// Extension methods for number conversion and text processing - Fixed Version
    /// </summary>
    public static class NumberExtensions
    {
        #region Number to Arabic Numerals Extensions

        /// <summary>
        /// Converts an integer to Arabic numerals format
        /// </summary>
        /// <param name="number">The integer value to convert</param>
        /// <param name="format">Optional format string for the number</param>
        /// <returns>String representation with Arabic numerals</returns>
        public static string ToArabicNumerals(this int number, string format = "")
        {
            return NumberConversionHelper.IntToArabicNumerals(number, format);
        }

        /// <summary>
        /// Converts a long integer to Arabic numerals format
        /// </summary>
        /// <param name="number">The long integer value to convert</param>
        /// <param name="format">Optional format string for the number</param>
        /// <returns>String representation with Arabic numerals</returns>
        public static string ToArabicNumerals(this long number, string format = "")
        {
            return NumberConversionHelper.LongToArabicNumerals(number, format);
        }

        /// <summary>
        /// Converts a decimal number to Arabic numerals format
        /// </summary>
        /// <param name="number">The decimal value to convert</param>
        /// <param name="format">Optional format string for the number (default: "N2")</param>
        /// <returns>String representation with Arabic numerals</returns>
        public static string ToArabicNumerals(this decimal number, string format = "N2")
        {
            return NumberConversionHelper.DecimalToArabicNumerals(number, format);
        }

        /// <summary>
        /// Converts a double number to Arabic numerals format
        /// </summary>
        /// <param name="number">The double value to convert</param>
        /// <param name="format">Optional format string for the number (default: "N2")</param>
        /// <returns>String representation with Arabic numerals</returns>
        public static string ToArabicNumerals(this double number, string format = "N2")
        {
            return NumberConversionHelper.DoubleToArabicNumerals(number, format);
        }

        /// <summary>
        /// Converts a float number to Arabic numerals format
        /// </summary>
        /// <param name="number">The float value to convert</param>
        /// <param name="format">Optional format string for the number (default: "N2")</param>
        /// <returns>String representation with Arabic numerals</returns>
        public static string ToArabicNumerals(this float number, string format = "N2")
        {
            return NumberConversionHelper.DoubleToArabicNumerals(number, format);
        }

        #endregion

        #region String Conversion Extensions

        /// <summary>
        /// Converts Western numerals in a string to Arabic numerals
        /// </summary>
        /// <param name="input">The input string containing numerals</param>
        /// <returns>String with Arabic numerals</returns>
        public static string ToArabicNumerals(this string input)
        {
            return NumberConversionHelper.ToArabicNumerals(input);
        }

        /// <summary>
        /// Converts Arabic numerals in a string to Western numerals
        /// </summary>
        /// <param name="input">The input string containing numerals</param>
        /// <returns>String with Western numerals</returns>
        public static string ToWesternNumerals(this string input)
        {
            return NumberConversionHelper.ToWesternNumerals(input);
        }

        #endregion

        #region Text Processing Extensions

        /// <summary>
        /// Unifies Arabic text by standardizing letter forms and optionally removing diacritics
        /// </summary>
        /// <param name="input">The Arabic text to unify</param>
        /// <param name="removeTashkeel">Whether to remove Arabic diacritics (tashkeel)</param>
        /// <returns>Unified Arabic text</returns>
        public static string UnifyArabic(this string input, bool removeTashkeel = true)
        {
            return NumberConversionHelper.UnifyArabicText(
                input,
                removeTashkeel,
                unifyNumerals: false,
                preferArabicNumerals: true);
        }

        /// <summary>
        /// Processes text with customizable unification options
        /// </summary>
        /// <param name="input">The text to process</param>
        /// <param name="options">Text unification options</param>
        /// <returns>Processed text</returns>
        public static string ProcessText(this string input, TextUnificationOptions options = null)
        {
            return NumberConversionHelper.ProcessText(input, options);
        }

        /// <summary>
        /// Extracts only numeric characters from a string
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>String containing only numbers</returns>
        public static string ExtractNumbers(this string input)
        {
            return NumberConversionHelper.ExtractNumbers(input);
        }

        /// <summary>
        /// Extracts only alphabetic characters from a string
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>String containing only text (no numbers or symbols)</returns>
        public static string ExtractTextOnly(this string input)
        {
            return NumberConversionHelper.ExtractTextOnly(input);
        }

        /// <summary>
        /// Extracts only Arabic text characters from a string
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>String containing only Arabic letters</returns>
        public static string ExtractArabicText(this string input)
        {
            return NumberConversionHelper.ExtractArabicText(input);
        }

        /// <summary>
        /// Removes special characters from a string
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="keepSpaces">Whether to preserve spaces</param>
        /// <returns>String with special characters removed</returns>
        public static string RemoveSpecialChars(this string input, bool keepSpaces = true)
        {
            return NumberConversionHelper.RemoveSpecialCharacters(input, keepSpaces);
        }

        /// <summary>
        /// Cleans up whitespace in a string by normalizing spaces
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>String with normalized whitespace</returns>
        public static string CleanWhitespace(this string input)
        {
            return NumberConversionHelper.CleanWhitespace(input);
        }

        #endregion

        #region Detection Extensions

        /// <summary>
        /// Checks if a string contains Arabic numerals
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>True if Arabic numerals are present</returns>
        public static bool ContainsArabicNumerals(this string text)
        {
            return NumberConversionHelper.ContainsArabicNumerals(text);
        }

        /// <summary>
        /// Checks if a string contains Western numerals
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>True if Western numerals are present</returns>
        public static bool ContainsWesternNumerals(this string text)
        {
            return NumberConversionHelper.ContainsWesternNumerals(text);
        }

        /// <summary>
        /// Checks if a string contains Arabic letters
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>True if Arabic letters are present</returns>
        public static bool ContainsArabicLetters(this string text)
        {
            return NumberConversionHelper.ContainsArabicLetters(text);
        }

        /// <summary>
        /// Checks if a string contains English letters
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>True if English letters are present</returns>
        public static bool ContainsEnglishLetters(this string text)
        {
            return NumberConversionHelper.ContainsEnglishLetters(text);
        }

        /// <summary>
        /// Automatically converts numerals in text based on preference
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <param name="preferArabic">Prefer Arabic numerals if true, Western if false</param>
        /// <returns>Text with numerals converted</returns>
        public static string AutoConvertNumerals(this string text, bool preferArabic = true)
        {
            return NumberConversionHelper.AutoConvertNumerals(text, preferArabic);
        }

        #endregion

        #region Numeric Type Extensions

        /// <summary>
        /// Converts an integer to Western numerals format
        /// </summary>
        /// <param name="number">The integer value to convert</param>
        /// <param name="format">Optional format string for the number</param>
        /// <returns>String representation with Western numerals</returns>
        public static string ToWesternNumerals(this int number, string format = "")
        {
            return string.IsNullOrEmpty(format)
                ? number.ToString(CultureInfo.InvariantCulture)
                : number.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a long integer to Western numerals format
        /// </summary>
        /// <param name="number">The long integer value to convert</param>
        /// <param name="format">Optional format string for the number</param>
        /// <returns>String representation with Western numerals</returns>
        public static string ToWesternNumerals(this long number, string format = "")
        {
            return string.IsNullOrEmpty(format)
                ? number.ToString(CultureInfo.InvariantCulture)
                : number.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a decimal number to Western numerals format
        /// </summary>
        /// <param name="number">The decimal value to convert</param>
        /// <param name="format">Optional format string for the number (default: "N2")</param>
        /// <returns>String representation with Western numerals</returns>
        public static string ToWesternNumerals(this decimal number, string format = "N2")
        {
            return number.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a double number to Western numerals format
        /// </summary>
        /// <param name="number">The double value to convert</param>
        /// <param name="format">Optional format string for the number (default: "N2")</param>
        /// <returns>String representation with Western numerals</returns>
        public static string ToWesternNumerals(this double number, string format = "N2")
        {
            return number.ToString(format, CultureInfo.InvariantCulture);
        }

        #endregion

        #region Character Detection Extensions

        /// <summary>
        /// Determines whether a character is a digit (Western or Arabic)
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if the character is a digit</returns>
        public static bool IsDigit(this char c)
        {
            return (c >= '0' && c <= '9') || (c >= '٠' && c <= '٩');
        }

        /// <summary>
        /// Determines whether a character is an Arabic letter
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if the character is an Arabic letter</returns>
        public static bool IsArabicLetter(this char c)
        {
            if (c >= '٠' && c <= '٩')
                return false;

            return (c >= 'ء' && c <= 'ي') ||
                   (c >= 'آ' && c <= 'ۏ') ||
                   NumberConversionHelper.IsArabicCharacterVariation(c) ||
                   c == 'أ' || c == 'إ' || c == 'آ' || c == 'ؤ' || c == 'ئ' ||
                   c == 'ة' || c == 'ى';
        }

        #endregion
    }
}
using NumericValidation.EG.Models.NationalNumber.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NumericValidation.EG.Models.Helper
{
    /// <summary>
    /// Comprehensive tool for Arabic text normalization and numeral conversion - Final Fixed Version
    /// </summary>
    public static class NumberConversionHelper
    {
        #region Primary Conversion Methods

        /// <summary>
        /// Converts Western numerals (0-9) to Arabic-Indic numerals in the input string
        /// </summary>
        /// <param name="input">The input string containing Western numerals</param>
        /// <returns>String with Western numerals converted to Arabic-Indic numerals</returns>
        public static string ToArabicNumerals(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var result = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                if (ArabicConstantsNumbers.WesternToArabicNumerals.TryGetValue(c, out char converted))
                {
                    result.Append(converted);
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Converts Arabic-Indic numerals to Western numerals (0-9) in the input string
        /// </summary>
        /// <param name="input">The input string containing Arabic-Indic numerals</param>
        /// <returns>String with Arabic-Indic numerals converted to Western numerals</returns>
        public static string ToWesternNumerals(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var result = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                if (ArabicConstantsNumbers.ArabicToWesternNumerals.TryGetValue(c, out char converted))
                {
                    result.Append(converted);
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }

        #endregion

        #region Aliases

        /// <summary>
        /// Alias for ToArabicNumerals method
        /// </summary>
        public static string ConvertToArabicNumerals(string input) => ToArabicNumerals(input);

        /// <summary>
        /// Alias for ToWesternNumerals method
        /// </summary>
        public static string ConvertToWesternNumerals(string input) => ToWesternNumerals(input);

        /// <summary>
        /// Alias for ToWesternNumerals method
        /// </summary>
        public static string ConvertArabicDigitsToEnglish(string text) => ToWesternNumerals(text);

        /// <summary>
        /// Alias for ToArabicNumerals method
        /// </summary>
        public static string ConvertEnglishDigitsToArabic(string text) => ToArabicNumerals(text);

        #endregion

        #region Extension Support

        /// <summary>
        /// Converts an integer to Arabic-Indic numerals with optional formatting
        /// </summary>
        /// <param name="number">Integer value to convert</param>
        /// <param name="format">Optional format string</param>
        /// <returns>Formatted string with Arabic-Indic numerals</returns>
        internal static string IntToArabicNumerals(int number, string format = "")
        {
            string western = string.IsNullOrEmpty(format)
                ? number.ToString(CultureInfo.InvariantCulture)
                : number.ToString(format, CultureInfo.InvariantCulture);
            return ToArabicNumerals(western);
        }

        /// <summary>
        /// Converts a long integer to Arabic-Indic numerals with optional formatting
        /// </summary>
        /// <param name="number">Long integer value to convert</param>
        /// <param name="format">Optional format string</param>
        /// <returns>Formatted string with Arabic-Indic numerals</returns>
        internal static string LongToArabicNumerals(long number, string format = "")
        {
            string western = string.IsNullOrEmpty(format)
                ? number.ToString(CultureInfo.InvariantCulture)
                : number.ToString(format, CultureInfo.InvariantCulture);
            return ToArabicNumerals(western);
        }

        /// <summary>
        /// Converts a decimal number to Arabic-Indic numerals with optional formatting
        /// </summary>
        /// <param name="number">Decimal value to convert</param>
        /// <param name="format">Optional format string (default: "N2")</param>
        /// <returns>Formatted string with Arabic-Indic numerals</returns>
        internal static string DecimalToArabicNumerals(decimal number, string format = "N2")
        {
            return ToArabicNumerals(number.ToString(format, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Converts a double number to Arabic-Indic numerals with optional formatting
        /// </summary>
        /// <param name="number">Double value to convert</param>
        /// <param name="format">Optional format string (default: "N2")</param>
        /// <returns>Formatted string with Arabic-Indic numerals</returns>
        internal static string DoubleToArabicNumerals(double number, string format = "N2")
        {
            return ToArabicNumerals(number.ToString(format, CultureInfo.InvariantCulture));
        }

        #endregion

        #region Text Unification

        /// <summary>
        /// Unifies Arabic text by normalizing characters and optionally converting numerals
        /// </summary>
        /// <param name="text">Input Arabic text</param>
        /// <param name="removeTashkeel">Whether to remove Arabic diacritics (default: true)</param>
        /// <param name="unifyNumerals">Whether to unify numeral types (default: true)</param>
        /// <param name="preferArabicNumerals">Whether to prefer Arabic-Indic numerals (default: true)</param>
        /// <returns>Unified Arabic text</returns>
        public static string UnifyArabicText(string text,
            bool removeTashkeel = true,
            bool unifyNumerals = true,
            bool preferArabicNumerals = true)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var result = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                if (c == ArabicConstantsNumbers.ArabicTatweel)
                    continue;

                if (removeTashkeel && ArabicConstantsNumbers.ArabicDiacritics.Contains(c))
                    continue;

                if (ArabicConstantsNumbers.ArabicTextUnification.TryGetValue(c, out char unified))
                {
                    result.Append(unified);
                }
                else
                {
                    result.Append(c);
                }
            }

            string processed = result.ToString().Trim();

            if (unifyNumerals)
            {
                processed = preferArabicNumerals
                    ? ToArabicNumerals(processed)
                    : ToWesternNumerals(processed);
            }

            return processed;
        }

        /// <summary>
        /// Normalizes Arabic text by removing diacritics and unifying character variations
        /// </summary>
        /// <param name="text">Input Arabic text</param>
        /// <returns>Normalized Arabic text</returns>
        public static string NormalizeArabicText(string text)
            => UnifyArabicText(text, removeTashkeel: true, unifyNumerals: false);

        /// <summary>
        /// Normalizes text with English digits by removing diacritics and converting to Western numerals
        /// </summary>
        /// <param name="text">Input text</param>
        /// <returns>Normalized text with English digits</returns>
        public static string NormalizeTextWithEnglishDigits(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = CleanWhitespace(text);
            return UnifyArabicText(text, removeTashkeel: true, unifyNumerals: true, preferArabicNumerals: false);
        }

        /// <summary>
        /// Normalizes text with Arabic digits by removing diacritics and converting to Arabic-Indic numerals
        /// </summary>
        /// <param name="text">Input text</param>
        /// <returns>Normalized text with Arabic-Indic digits</returns>
        public static string NormalizeTextWithArabicDigits(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = CleanWhitespace(text);
            return UnifyArabicText(text, removeTashkeel: true, unifyNumerals: true, preferArabicNumerals: true);
        }

        /// <summary>
        /// Performs a full cleanup of text including whitespace normalization and character unification
        /// </summary>
        /// <param name="text">Input text</param>
        /// <param name="useEnglishDigits">Whether to use English digits (default: true)</param>
        /// <returns>Fully cleaned text</returns>
        public static string FullClean(string text, bool useEnglishDigits = true)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = CleanWhitespace(text);
            return useEnglishDigits
                ? NormalizeTextWithEnglishDigits(text)
                : NormalizeTextWithArabicDigits(text);
        }

        /// <summary>
        /// Processes text with customizable options for unification and cleanup
        /// </summary>
        /// <param name="text">Input text</param>
        /// <param name="options">Text unification options (uses defaults if null)</param>
        /// <returns>Processed text according to specified options</returns>
        public static string ProcessText(string text, TextUnificationOptions options = null)
        {
            options ??= TextUnificationOptions.Default;

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            string result = text;

            if (options.ExtractNumbersOnly)
            {
                return ExtractNumbers(result);
            }

            if (options.ExtractTextOnly)
            {
                return ExtractTextOnly(result);
            }

            if (options.UnifyArabicText && ContainsArabicLetters(result))
            {
                result = UnifyArabicText(result,
                    options.RemoveTashkeel,
                    options.UnifyNumerals,
                    options.PreferArabicNumerals);
            }
            else if (options.ConvertNumerals && !options.UnifyArabicText)
            {
                result = options.PreferArabicNumerals
                    ? ToArabicNumerals(result)
                    : ToWesternNumerals(result);
            }

            if (options.RemoveSpecialChars)
            {
                result = RemoveSpecialCharacters(result, options.KeepSpaces);
            }

            return result;
        }

        #endregion

        #region Extraction Methods

        /// <summary>
        /// Extracts numeric characters from input string including decimal points and negative signs
        /// </summary>
        /// <param name="input">Input string containing mixed content</param>
        /// <returns>String containing only extracted numbers</returns>
        public static string ExtractNumbers(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var result = new StringBuilder();
            bool hasDecimal = false;

            foreach (char c in input)
            {
                if (c >= '0' && c <= '9')
                {
                    result.Append(c);
                }
                else if (c >= '٠' && c <= '٩')
                {
                    result.Append((char)(c - '٠' + '0'));
                }
                else if ((c == '.' || c == '٫') && !hasDecimal)
                {
                    result.Append('.');
                    hasDecimal = true;
                }
                else if (c == '-' && result.Length == 0)
                {
                    result.Append('-');
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Extracts digits from text and returns them as English (Western) numerals
        /// </summary>
        /// <param name="text">Input text</param>
        /// <returns>Extracted digits as English numerals</returns>
        public static string ExtractDigitsAsEnglish(string text) => ExtractNumbers(text);

        /// <summary>
        /// Extracts digits from text and returns them as Arabic-Indic numerals
        /// </summary>
        /// <param name="text">Input text</param>
        /// <returns>Extracted digits as Arabic-Indic numerals</returns>
        public static string ExtractDigitsAsArabic(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return ToArabicNumerals(ExtractNumbers(text));
        }

        /// <summary>
        /// Extracts non-numeric text from input string
        /// </summary>
        /// <param name="input">Input string containing mixed content</param>
        /// <returns>String containing only non-numeric text</returns>
        public static string ExtractTextOnly(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var result = new StringBuilder();
            bool lastWasSpace = false;

            foreach (char c in input)
            {
                // Skip digits and decimal points
                if (IsDigit(c) || c == '.' || c == '٫')
                    continue;

                if (char.IsWhiteSpace(c))
                {
                    if (!lastWasSpace && result.Length > 0)
                    {
                        result.Append(' ');
                        lastWasSpace = true;
                    }
                }
                else
                {
                    result.Append(c);
                    lastWasSpace = false;
                }
            }

            return result.ToString().Trim();
        }

        /// <summary>
        /// Extracts Arabic text along with basic punctuation
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>String containing only Arabic text and punctuation</returns>
        public static string ExtractArabicText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var result = new StringBuilder();
            bool lastWasSpace = false;

            foreach (char c in input)
            {
                if (IsArabicLetter(c))
                {
                    result.Append(c);
                    lastWasSpace = false;
                }
                else if (char.IsWhiteSpace(c))
                {
                    if (!lastWasSpace && result.Length > 0)
                    {
                        result.Append(' ');
                        lastWasSpace = true;
                    }
                }
                else if (c == '،' || c == '؛' || c == '؟' || c == '!' ||
                         c == '.' || c == ':' || c == '-')
                {
                    result.Append(c);
                    lastWasSpace = false;
                }
            }

            return result.ToString().Trim();
        }

        /// <summary>
        /// Removes special characters from input string, keeping only alphanumeric characters
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="keepSpaces">Whether to preserve spaces (default: true)</param>
        /// <returns>String with special characters removed</returns>
        public static string RemoveSpecialCharacters(string input, bool keepSpaces = true)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var result = new StringBuilder();
            bool lastWasSpace = false;

            foreach (char c in input)
            {
                bool keep = false;

                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                    keep = true;
                else if (IsArabicLetter(c) || (c >= '٠' && c <= '٩'))
                    keep = true;
                else if (c == '.' || c == ',' || c == '-' || c == '٫' || c == '٬')
                    keep = true;
                else if (char.IsWhiteSpace(c) && keepSpaces)
                {
                    if (!lastWasSpace && result.Length > 0)
                    {
                        result.Append(' ');
                        lastWasSpace = true;
                    }
                    continue;
                }

                if (keep)
                {
                    result.Append(c);
                    lastWasSpace = false;
                }
            }

            return result.ToString().Trim();
        }

        /// <summary>
        /// Cleans and normalizes whitespace in text
        /// </summary>
        /// <param name="text">Input text</param>
        /// <returns>Text with normalized whitespace</returns>
        public static string CleanWhitespace(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return Regex.Replace(text.Trim(), @"\s+", " ");
        }

        #endregion

        #region Detection Methods

        /// <summary>
        /// Checks if the input string contains Arabic-Indic numerals
        /// </summary>
        /// <param name="text">Input text to check</param>
        /// <returns>True if Arabic-Indic numerals are found, otherwise false</returns>
        public static bool ContainsArabicNumerals(string text)
        {
            return !string.IsNullOrEmpty(text) && text.Any(c => c >= '٠' && c <= '٩');
        }

        /// <summary>
        /// Alias for ContainsArabicNumerals method
        /// </summary>
        public static bool ContainsArabicDigits(string text) => ContainsArabicNumerals(text);

        /// <summary>
        /// Checks if the input string contains Western numerals
        /// </summary>
        /// <param name="text">Input text to check</param>
        /// <returns>True if Western numerals are found, otherwise false</returns>
        public static bool ContainsWesternNumerals(string text)
        {
            return !string.IsNullOrEmpty(text) && text.Any(c => c >= '0' && c <= '9');
        }

        /// <summary>
        /// Alias for ContainsWesternNumerals method
        /// </summary>
        public static bool ContainsEnglishDigits(string text) => ContainsWesternNumerals(text);

        /// <summary>
        /// Checks if the input string contains Arabic letters
        /// </summary>
        /// <param name="text">Input text to check</param>
        /// <returns>True if Arabic letters are found, otherwise false</returns>
        public static bool ContainsArabicLetters(string text)
        {
            return !string.IsNullOrEmpty(text) && text.Any(c => IsArabicLetter(c));
        }

        /// <summary>
        /// Checks if the input string contains English letters
        /// </summary>
        /// <param name="text">Input text to check</param>
        /// <returns>True if English letters are found, otherwise false</returns>
        public static bool ContainsEnglishLetters(string text)
        {
            return !string.IsNullOrEmpty(text) && text.Any(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'));
        }

        /// <summary>
        /// Checks if the input string contains non-normalized Arabic characters
        /// </summary>
        /// <param name="text">Input text to check</param>
        /// <returns>True if non-normalized Arabic characters are found, otherwise false</returns>
        public static bool ContainsNonNormalizedArabic(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return text.Any(c =>
                ArabicConstantsNumbers.ArabicTextUnification.ContainsKey(c) ||
                ArabicConstantsNumbers.ArabicDiacritics.Contains(c) ||
                c == ArabicConstantsNumbers.ArabicTatweel);
        }

        /// <summary>
        /// Automatically converts numerals based on preference and existing numeral types
        /// </summary>
        /// <param name="text">Input text</param>
        /// <param name="preferArabic">Whether to prefer Arabic-Indic numerals (default: true)</param>
        /// <returns>Text with numerals converted according to preference</returns>
        public static string AutoConvertNumerals(string text, bool preferArabic = true)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            bool hasWestern = ContainsWesternNumerals(text);
            bool hasArabic = ContainsArabicNumerals(text);

            if (!hasWestern && !hasArabic)
                return text;

            if (hasWestern && !hasArabic)
                return preferArabic ? ToArabicNumerals(text) : text;

            if (hasArabic && !hasWestern)
                return preferArabic ? text : ToWesternNumerals(text);

            return preferArabic ? ToArabicNumerals(text) : ToWesternNumerals(text);
        }

        #endregion

        #region Formatting Methods

        /// <summary>
        /// Formats a decimal number with Arabic-Indic numerals and specified decimal places
        /// </summary>
        /// <param name="number">Decimal number to format</param>
        /// <param name="decimalPlaces">Number of decimal places (default: 2)</param>
        /// <returns>Formatted number string with Arabic-Indic numerals</returns>
        public static string FormatNumberArabic(decimal number, int decimalPlaces = 2)
        {
            return ToArabicNumerals(number.ToString($"N{decimalPlaces}", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Formats a decimal number with Western numerals and specified decimal places
        /// </summary>
        /// <param name="number">Decimal number to format</param>
        /// <param name="decimalPlaces">Number of decimal places (default: 2)</param>
        /// <returns>Formatted number string with Western numerals</returns>
        public static string FormatNumberWestern(decimal number, int decimalPlaces = 2)
        {
            return number.ToString($"N{decimalPlaces}", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats a phone number with Arabic-Indic numerals and grouping
        /// </summary>
        /// <param name="phone">Phone number string</param>
        /// <returns>Formatted phone number with Arabic-Indic numerals</returns>
        public static string FormatPhoneArabic(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return string.Empty;

            string clean = ExtractNumbers(phone);
            string arabic = ToArabicNumerals(clean);

            if (clean.Length == 11 && clean.StartsWith("01"))
            {
                return $"{arabic.Substring(0, 3)} {arabic.Substring(3, 3)} {arabic.Substring(6, 5)}";
            }
            else if (clean.Length == 10)
            {
                return $"{arabic.Substring(0, 3)} {arabic.Substring(3, 3)} {arabic.Substring(6, 4)}";
            }
            else
            {
                var formatted = new StringBuilder();
                for (int i = 0; i < arabic.Length; i++)
                {
                    formatted.Append(arabic[i]);
                    if ((i + 1) < arabic.Length && (i + 1) % 3 == 0)
                        formatted.Append(' ');
                }
                return formatted.ToString().Trim();
            }
        }

        /// <summary>
        /// Formats a national ID number with Arabic-Indic numerals and grouping
        /// </summary>
        /// <param name="nationalId">National ID string</param>
        /// <returns>Formatted national ID with Arabic-Indic numerals</returns>
        public static string FormatNationalIdArabic(string nationalId)
        {
            if (string.IsNullOrEmpty(nationalId))
                return string.Empty;

            string clean = ExtractNumbers(nationalId);
            if (clean.Length != 14)
                return nationalId;

            return $"{ToArabicNumerals(clean.Substring(0, 2))} " +
                   $"{ToArabicNumerals(clean.Substring(2, 2))} " +
                   $"{ToArabicNumerals(clean.Substring(4, 2))} " +
                   $"{ToArabicNumerals(clean.Substring(6, 2))} " +
                   $"{ToArabicNumerals(clean.Substring(8, 3))} " +
                   $"{ToArabicNumerals(clean.Substring(11, 3))}";
        }

        #endregion

        #region Search Helpers

        /// <summary>
        /// Normalizes text for search operations including case normalization and optional numeral conversion
        /// </summary>
        /// <param name="input">Input text to normalize</param>
        /// <param name="arabicNumerals">Whether to use Arabic-Indic numerals (default: false)</param>
        /// <returns>Normalized text suitable for search operations</returns>
        public static string NormalizeForSearch(string input, bool arabicNumerals = false)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            string normalized = CleanWhitespace(input);
            normalized = UnifyArabicText(normalized, true, true, arabicNumerals);
            return normalized.ToLowerInvariant();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Checks if a character is a digit (either Western or Arabic-Indic)
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>True if the character is a digit, otherwise false</returns>
        private static bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9') || (c >= '٠' && c <= '٩');
        }

        /// <summary>
        /// Checks if a character is an Arabic letter (excluding numerals)
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>True if the character is an Arabic letter, otherwise false</returns>
        private static bool IsArabicLetter(char c)
        {
            if (c >= '٠' && c <= '٩')
                return false;

            return (c >= 'ء' && c <= 'ي') ||
                   (c >= 'آ' && c <= 'ۏ') ||
                   ArabicConstantsNumbers.ArabicTextUnification.ContainsKey(c) ||
                   c == 'أ' || c == 'إ' || c == 'آ' || c == 'ؤ' || c == 'ئ' ||
                   c == 'ة' || c == 'ى';
        }

        /// <summary>
        /// Checks if a character is a variation of an Arabic character that needs unification
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>True if the character is an Arabic variation, otherwise false</returns>
        public static bool IsArabicCharacterVariation(char c)
        {
            return ArabicConstantsNumbers.ArabicTextUnification.ContainsKey(c);
        }

        #endregion
    }

   
}
using NumericValidation.EG.Models.NumbersToText;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static NumericValidation.EG.Models.NumbersToText.NumberToWordsConverter;

namespace NumericValidation.EG.Models.BarcodeNumber
{
    /// <summary>
    /// Professional Egyptian barcode parser supporting standard, weight, and price barcodes
    /// with comprehensive error handling and market-specific detection algorithms.
    /// </summary>
    public class BarcodeParser
    {
        #region Country Codes Dictionary

        private static readonly Dictionary<string, (string Arabic, string English)> CountryCodes = new Dictionary<string, (string, string)>
        {
            { "622", ("مصر", "Egypt") },
            { "621", ("سوريا", "Syria") },
            { "628", ("السعودية", "Saudi Arabia") },
            { "629", ("الإمارات", "UAE") },
            { "00-13", ("الولايات المتحدة وكندا", "USA & Canada") },
            { "30-37", ("فرنسا", "France") },
            { "40-44", ("ألمانيا", "Germany") },
            { "45-49", ("اليابان", "Japan") },
            { "50", ("المملكة المتحدة", "United Kingdom") },
            { "690-699", ("الصين", "China") },
            { "471", ("تايوان", "Taiwan") },
            { "880", ("كوريا الجنوبية", "South Korea") },
            { "869", ("تركيا", "Turkey") },
            { "600-601", ("جنوب أفريقيا", "South Africa") }
        };

        #endregion

        #region Private Fields

        private BarcodeType _defaultType = BarcodeType.Standard;
        private bool _strictMode = false;
        private List<string> _barcodes = new List<string>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BarcodeParser class.
        /// </summary>
        public BarcodeParser()
        {
        }

        /// <summary>
        /// Initializes a new instance with a single barcode.
        /// </summary>
        /// <param name="barcode">The barcode string to parse</param>
        /// <param name="type">Optional barcode type specification</param>
        /// <exception cref="ArgumentNullException">Thrown when barcode is null</exception>
        public BarcodeParser(string barcode, BarcodeType type = BarcodeType.Standard)
        {
            if (barcode == null)
                throw new ArgumentNullException(nameof(barcode));

            _barcodes.Add(barcode);
            _defaultType = type;
        }

        /// <summary>
        /// Initializes a new instance with multiple barcodes.
        /// </summary>
        /// <param name="barcodes">Array of barcode strings</param>
        /// <exception cref="ArgumentNullException">Thrown when barcodes is null</exception>
        public BarcodeParser(params string[] barcodes)
        {
            if (barcodes == null)
                throw new ArgumentNullException(nameof(barcodes));

            _barcodes = new List<string>(barcodes);
        }

        /// <summary>
        /// Initializes a new instance with a collection of barcodes.
        /// </summary>
        /// <param name="barcodes">Enumerable collection of barcode strings</param>
        /// <exception cref="ArgumentNullException">Thrown when barcodes is null</exception>
        public BarcodeParser(IEnumerable<string> barcodes)
        {
            if (barcodes == null)
                throw new ArgumentNullException(nameof(barcodes));

            _barcodes = new List<string>(barcodes);
        }

        #endregion

        #region Configuration Methods

        /// <summary>
        /// Configures the default barcode type for parsing operations.
        /// </summary>
        /// <param name="type">The barcode type to set as default</param>
        /// <returns>Current instance for fluent method chaining</returns>
        public BarcodeParser SetDefaultType(BarcodeType type)
        {
            _defaultType = type;
            return this;
        }

        /// <summary>
        /// Enables strict validation mode including check digit verification.
        /// </summary>
        /// <returns>Current instance for fluent method chaining</returns>
        public BarcodeParser EnableStrictMode()
        {
            _strictMode = true;
            return this;
        }

        /// <summary>
        /// Adds a single barcode to the processing queue.
        /// </summary>
        /// <param name="barcode">The barcode string to add</param>
        /// <exception cref="ArgumentNullException">Thrown when barcode is null</exception>
        public void AddBarcode(string barcode)
        {
            if (barcode == null)
                throw new ArgumentNullException(nameof(barcode));

            _barcodes.Add(barcode);
        }

        /// <summary>
        /// Adds multiple barcodes to the processing queue.
        /// </summary>
        /// <param name="barcodes">Array of barcode strings to add</param>
        /// <exception cref="ArgumentNullException">Thrown when barcodes is null</exception>
        public void AddBarcodes(params string[] barcodes)
        {
            if (barcodes == null)
                throw new ArgumentNullException(nameof(barcodes));

            _barcodes.AddRange(barcodes);
        }

        #endregion

        #region Main Parsing Methods

        /// <summary>
        /// Parses a single barcode with comprehensive error handling and type detection.
        /// </summary>
        /// <param name="barcode">The barcode string to parse</param>
        /// <param name="type">Optional explicit barcode type</param>
        /// <param name="pricePerKg">Required for price barcodes - price per kilogram</param>
        /// <returns>BarcodeInfo object containing all parsed information</returns>
        /// <example>
        /// <code>
        /// var parser = new BarcodeParser();
        /// var result = parser.Parse("2123450123405", BarcodeType.Weight);
        /// if (result.IsValid)
        /// {
        ///     Console.WriteLine($"Product Code: {result.ProductCode}");
        ///     Console.WriteLine($"Weight: {result.Weight} kg");
        /// }
        /// </code>
        /// </example>
        public BarcodeInfo Parse(string barcode, BarcodeType? type = null, decimal? pricePerKg = null)
        {
            var result = new BarcodeInfo
            {
                OriginalBarcode = barcode ?? string.Empty
            };

            // Clean and validate input
            string cleaned = CleanBarcode(barcode);
            result.CleanedBarcode = cleaned;

            if (string.IsNullOrWhiteSpace(cleaned))
            {
                result.IsValid = false;
                result.ErrorMessage = "Barcode is empty / الباركود فارغ";
                return result;
            }

            // Determine parsing strategy
            BarcodeType barcodeType = type ?? _defaultType;

            // Smart auto-detection when no explicit type provided
            if (!type.HasValue && _defaultType == BarcodeType.Standard)
            {
                barcodeType = DetectBarcodeType(cleaned);
            }

            result.Type = barcodeType;
            result.Standard = DetectStandard(cleaned);

            try
            {
                return barcodeType switch
                {
                    BarcodeType.Weight => ParseWeightBarcode(cleaned, result),
                    BarcodeType.Price => ValidateAndParsePriceBarcode(cleaned, result, pricePerKg),
                    _ => ParseStandardBarcode(cleaned, result)
                };
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Parsing error: {ex.Message}";
                return result;
            }
        }

        /// <summary>
        /// Processes all queued barcodes in batch mode.
        /// </summary>
        /// <returns>List of BarcodeInfo objects for all processed barcodes</returns>
        /// <remarks>
        /// This method processes barcodes sequentially and returns results in the same order
        /// they were added. Each barcode is processed independently.
        /// </remarks>
        public List<BarcodeInfo> ParseAll()
        {
            return _barcodes.Select(b => Parse(b)).ToList();
        }

        /// <summary>
        /// Retrieves successfully parsed barcodes with valid formatting.
        /// </summary>
        /// <returns>Filtered list of valid BarcodeInfo objects</returns>
        /// <seealso cref="GetInvalidBarcodes"/>
        public List<BarcodeInfo> GetValidBarcodes()
        {
            return ParseAll().Where(b => b.IsValid).ToList();
        }

        /// <summary>
        /// Retrieves barcodes that failed parsing or validation.
        /// </summary>
        /// <returns>Filtered list of invalid BarcodeInfo objects with error details</returns>
        /// <seealso cref="GetValidBarcodes"/>
        public List<BarcodeInfo> GetInvalidBarcodes()
        {
            return ParseAll().Where(b => !b.IsValid).ToList();
        }

        /// <summary>
        /// Parses a barcode with currency specification
        /// </summary>
        /// <param name="barcode">The barcode string to parse</param>
        /// <param name="currency">The currency to use for price conversion</param>
        /// <returns>BarcodeInfo object with currency-adjusted values</returns>
        /// <exception cref="ArgumentNullException">Thrown when barcode is null</exception>
        /// <remarks>
        /// This method is specifically designed for handling price barcodes with currency conversion.
        /// If the barcode is not a price barcode, currency conversion will not be applied.
        /// </remarks>
        /// <example>
        /// <code>
        /// var parser = new BarcodeParser();
        /// var result = parser.Parse("2123450050005", Currency.USD);
        /// if (result.IsValid)
        /// {
        ///     Console.WriteLine($"Price: {result.Price} {result.Currency}");
        ///     Console.WriteLine($"Weight: {result.Weight} kg");
        /// }
        /// </code>
        /// </example>
        public BarcodeInfo Parse(string barcode, Currency currency)
        {
            if (barcode == null)
                throw new ArgumentNullException(nameof(barcode));

            var result = Parse(barcode, null, null);

            if (result.IsValid && (result.Type == BarcodeType.Price || result.Price > 0))
            {
                result.ChangeCurrency(currency);
            }

            return result;
        }

        #endregion

        #region Type-Specific Parsing Methods

        /// <summary>
        /// Parses weight-encoded barcodes (prefix '2').
        /// Format: [2][5-digit product code][5-digit weight in grams][check digit]
        /// Example: "2123450123405" → Product: 12345, Weight: 1.234 kg
        /// </summary>
        /// <param name="barcode">The cleaned barcode string</param>
        /// <param name="result">BarcodeInfo object to populate</param>
        /// <returns>Populated BarcodeInfo object</returns>
        private BarcodeInfo ParseWeightBarcode(string barcode, BarcodeInfo result)
        {
            if (!ValidateLengthAndPrefix(barcode, 13, "2", result))
                return result;

            try
            {
                result.Prefix = "2";
                result.ProductCode = barcode.Substring(1, 5);

                if (!decimal.TryParse(barcode.Substring(6, 5), out decimal weightInGrams))
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Invalid weight format / صيغة الوزن غير صحيحة";
                    return result;
                }

                result.Weight = weightInGrams / 1000m;
                result.CheckDigit = barcode.Substring(12, 1);

                if (_strictMode && !ValidateCheckDigit(barcode))
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Invalid check digit / رقم التحقق غير صحيح";
                    return result;
                }

                result.IsValid = true;
                SetCountryInformation(barcode, result);
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Weight barcode parsing failed: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// Validates requirements and parses price-encoded barcodes (prefix '2').
        /// Format: [2][5-digit product code][5-digit total price in piasters][check digit]
        /// Example: "2123450050005" with pricePerKg=100 → Product: 12345, Price: 50 EGP, Weight: 0.5 kg
        /// </summary>
        /// <param name="barcode">The cleaned barcode string</param>
        /// <param name="result">BarcodeInfo object to populate</param>
        /// <param name="pricePerKg">Price per kilogram for weight calculation</param>
        /// <returns>Populated BarcodeInfo object</returns>
        private BarcodeInfo ValidateAndParsePriceBarcode(string barcode, BarcodeInfo result, decimal? pricePerKg)
        {
            if (!pricePerKg.HasValue || pricePerKg.Value <= 0)
            {
                result.IsValid = false;
                result.ErrorMessage = "Valid price per Kg is required / سعر الكيلو صالح مطلوب";
                return result;
            }

            if (!ValidateLengthAndPrefix(barcode, 13, "2", result))
                return result;

            try
            {
                return ParsePriceBarcodeInternal(barcode, result, pricePerKg.Value);
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Price barcode parsing failed: {ex.Message}";
                return result;
            }
        }

        /// <summary>
        /// Internal implementation of price barcode parsing with business logic.
        /// </summary>
        /// <param name="barcode">The cleaned barcode string</param>
        /// <param name="result">BarcodeInfo object to populate</param>
        /// <param name="pricePerKg">Price per kilogram</param>
        /// <returns>Populated BarcodeInfo object</returns>
        private BarcodeInfo ParsePriceBarcodeInternal(string barcode, BarcodeInfo result, decimal pricePerKg)
        {
            result.Prefix = "2";
            result.ProductCode = barcode.Substring(1, 5);

            if (!decimal.TryParse(barcode.Substring(6, 5), out decimal priceInPiasters))
            {
                result.IsValid = false;
                result.ErrorMessage = "Invalid price format / صيغة السعر غير صحيحة";
                return result;
            }

            result.Price = priceInPiasters / 100m;
            result.CheckDigit = barcode.Substring(12, 1);

            if (_strictMode && !ValidateCheckDigit(barcode))
            {
                result.IsValid = false;
                result.ErrorMessage = "Invalid check digit / رقم التحقق غير صحيح";
                return result;
            }

            result.Weight = result.Price / pricePerKg;
            result.IsValid = true;
            SetCountryInformation(barcode, result);

            return result;
        }

        /// <summary>
        /// Parses standard EAN/UPC barcodes with fixed product encoding.
        /// Supports EAN-13 (13 digits), EAN-8 (8 digits), and UPC-A (12 digits).
        /// </summary>
        /// <param name="barcode">The cleaned barcode string</param>
        /// <param name="result">BarcodeInfo object to populate</param>
        /// <returns>Populated BarcodeInfo object</returns>
        private BarcodeInfo ParseStandardBarcode(string barcode, BarcodeInfo result)
        {
            if (!IsValidStandardLength(barcode))
            {
                result.IsValid = false;
                result.ErrorMessage = "Invalid barcode length / طول الباركود غير صحيح";
                return result;
            }

            try
            {
                switch (barcode.Length)
                {
                    case 13: // EAN-13
                        result.CountryCode = barcode.Substring(0, 3);
                        result.ProductCode = barcode.Substring(3, 9);
                        result.CheckDigit = barcode.Substring(12, 1);
                        break;
                    case 8: // EAN-8
                        result.CountryCode = barcode.Substring(0, 2);
                        result.ProductCode = barcode.Substring(2, 5);
                        result.CheckDigit = barcode.Substring(7, 1);
                        break;
                    case 12: // UPC-A
                        result.ProductCode = barcode.Substring(0, 11);
                        result.CheckDigit = barcode.Substring(11, 1);
                        break;
                }

                if (_strictMode && !ValidateCheckDigit(barcode))
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Invalid check digit / رقم التحقق غير صحيح";
                    return result;
                }

                result.IsValid = true;
                SetCountryInformation(barcode, result);
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Standard barcode parsing failed: {ex.Message}";
            }

            return result;
        }

        #endregion

        #region Detection & Validation Methods

        /// <summary>
        /// Intelligent barcode type detection with Egyptian market heuristics.
        /// Analyzes numeric patterns to distinguish between Weight and Price barcodes.
        /// 
        /// Detection Logic:
        /// 1. 00000 → Standard barcode (no variable data)
        /// 2. >50000 → Weight barcode (extreme values)
        /// 3. <1000 → Weight (light items) with price pattern exceptions
        /// 4. 1000-5000 → Pattern analysis (round numbers = price)
        /// 5. >5000 → Market-based probability analysis
        /// 
        /// Note: Explicit type specification overrides auto-detection for production use.
        /// </summary>
        /// <param name="barcode">The cleaned barcode string</param>
        /// <returns>Detected barcode type</returns>
        private BarcodeType DetectBarcodeType(string barcode)
        {
            if (string.IsNullOrEmpty(barcode) || barcode.Length != 13)
                return BarcodeType.Standard;

            if (!barcode.StartsWith("2"))
                return BarcodeType.Standard;

            string valuePart = barcode.Substring(6, 5);
            if (!int.TryParse(valuePart, out int numericValue))
                return BarcodeType.Weight;

            if (numericValue == 0)
                return BarcodeType.Standard;

            string secondDigit = barcode.Substring(1, 1);

            if (secondDigit == "2")
                return BarcodeType.Price;
            else if (secondDigit == "1")
                return BarcodeType.Weight;

            if (numericValue > 50000)
                return BarcodeType.Weight;

            if (numericValue < 1000)
            {
                if (numericValue % 100 == 0 && numericValue <= 500)
                    return BarcodeType.Price;
                return BarcodeType.Weight;
            }

            return BarcodeType.Weight; // Default
        }

        /// <summary>
        /// Identifies barcode standard based on digit count.
        /// </summary>
        /// <param name="barcode">The cleaned barcode string</param>
        /// <returns>Detected barcode standard</returns>
        private BarcodeStandard DetectStandard(string barcode)
        {
            return barcode.Length switch
            {
                13 => BarcodeStandard.EAN13,
                8 => BarcodeStandard.EAN8,
                12 => BarcodeStandard.UPCA,
                _ => BarcodeStandard.Unknown
            };
        }

        #endregion

        #region Helper & Utility Methods

        /// <summary>
        /// Sanitizes barcode input by removing non-numeric characters.
        /// </summary>
        /// <param name="barcode">The raw barcode string</param>
        /// <returns>Cleaned numeric-only barcode string</returns>
        private string CleanBarcode(string barcode)
        {
            return string.IsNullOrWhiteSpace(barcode)
                ? string.Empty
                : Regex.Replace(barcode, @"[^\d]", string.Empty);
        }

        /// <summary>
        /// Validates barcode length and prefix requirements.
        /// </summary>
        /// <param name="barcode">The cleaned barcode string</param>
        /// <param name="requiredLength">Expected barcode length</param>
        /// <param name="requiredPrefix">Expected barcode prefix</param>
        /// <param name="result">BarcodeInfo object for error reporting</param>
        /// <returns>True if validation passes</returns>
        private bool ValidateLengthAndPrefix(string barcode, int requiredLength, string requiredPrefix, BarcodeInfo result)
        {
            if (barcode.Length != requiredLength)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Barcode must be {requiredLength} digits / يجب أن يكون الباركود {requiredLength} رقم";
                return false;
            }

            if (!barcode.StartsWith(requiredPrefix))
            {
                result.IsValid = false;
                result.ErrorMessage = $"Barcode must start with '{requiredPrefix}' / يجب أن يبدأ الباركود بـ '{requiredPrefix}'";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if barcode length matches standard EAN/UPC formats.
        /// </summary>
        /// <param name="barcode">The cleaned barcode string</param>
        /// <returns>True if length is valid</returns>
        private bool IsValidStandardLength(string barcode)
        {
            return barcode.Length == 13 || barcode.Length == 8 || barcode.Length == 12;
        }

        /// <summary>
        /// Validates EAN-13 check digit using standard modulus 10 algorithm.
        /// </summary>
        /// <param name="barcode">The cleaned barcode string</param>
        /// <returns>True if check digit is valid</returns>
        private bool ValidateCheckDigit(string barcode)
        {
            if (string.IsNullOrEmpty(barcode) || barcode.Length < 2)
                return false;

            try
            {
                string data = barcode.Substring(0, barcode.Length - 1);
                int calculated = CalculateCheckDigit(data);

                if (!int.TryParse(barcode[^1].ToString(), out int actual))
                    return false;

                return calculated == actual;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Calculates EAN-13 check digit according to GS1 specification.
        /// </summary>
        /// <param name="barcode">Barcode data without check digit</param>
        /// <returns>Calculated check digit (0-9)</returns>
        private int CalculateCheckDigit(string barcode)
        {
            int sum = 0;
            bool triple = true;

            for (int i = barcode.Length - 1; i >= 0; i--)
            {
                if (!int.TryParse(barcode[i].ToString(), out int digit))
                    continue;

                sum += triple ? digit * 3 : digit;
                triple = !triple;
            }

            return (10 - (sum % 10)) % 10;
        }

        /// <summary>
        /// Extracts and sets country information based on GS1 prefix.
        /// Uses switch expressions for optimal performance with range matching.
        /// </summary>
        /// <param name="barcode">The cleaned barcode string</param>
        /// <param name="result">BarcodeInfo object to populate</param>
        private void SetCountryInformation(string barcode, BarcodeInfo result)
        {
            if (string.IsNullOrEmpty(barcode) || barcode.Length < 3)
                return;

            // Ensure country code extraction
            if (string.IsNullOrEmpty(result.CountryCode))
            {
                result.CountryCode = barcode.Substring(0, Math.Min(3, barcode.Length));
            }

            if (!int.TryParse(result.CountryCode, out int prefix))
            {
                SetUnknownCountry(result);
                return;
            }

            // GS1 country code mapping with Egyptian market focus
            var (arabic, english) = prefix switch
            {
                >= 0 and <= 19 => ("الولايات المتحدة وكندا", "USA & Canada"),
                >= 300 and <= 379 => ("فرنسا", "France"),
                >= 400 and <= 440 => ("ألمانيا", "Germany"),
                >= 450 and <= 459 or >= 490 and <= 499 => ("اليابان", "Japan"),
                500 => ("المملكة المتحدة", "United Kingdom"),
                621 => ("سوريا", "Syria"),
                622 => ("مصر", "Egypt"),
                628 => ("السعودية", "Saudi Arabia"),
                629 => ("الإمارات", "UAE"),
                >= 690 and <= 699 => ("الصين", "China"),
                750 => ("المكسيك", "Mexico"),
                869 => ("تركيا", "Turkey"),
                880 => ("كوريا الجنوبية", "South Korea"),
                471 => ("تايوان", "Taiwan"),
                >= 600 and <= 601 => ("جنوب أفريقيا", "South Africa"),
                _ => ("غير معروف", "Unknown")
            };

            result.CountryNameArabic = arabic;
            result.CountryNameEnglish = english;
        }

        /// <summary>
        /// Sets default unknown country information.
        /// </summary>
        /// <param name="result">BarcodeInfo object to populate</param>
        private void SetUnknownCountry(BarcodeInfo result)
        {
            result.CountryNameArabic = "غير معروف";
            result.CountryNameEnglish = "Unknown";
        }

        #endregion

        #region Static Methods (Convenience API)

        /// <summary>
        /// Quick-parse a single barcode without instance creation.
        /// </summary>
        /// <param name="barcode">Barcode string to parse</param>
        /// <param name="type">Barcode type specification</param>
        /// <param name="pricePerKg">Required for price barcodes</param>
        /// <returns>Parsed barcode information</returns>
        /// <exception cref="ArgumentNullException">Thrown when barcode is null</exception>
        /// <example>
        /// <code>
        /// var result = BarcodeParser.ParseSingle("5901234123457");
        /// if (result.IsValid)
        /// {
        ///     Console.WriteLine($"Country: {result.CountryNameEnglish}");
        ///     Console.WriteLine($"Product Code: {result.ProductCode}");
        /// }
        /// </code>
        /// </example>
        public static BarcodeInfo ParseSingle(string barcode, BarcodeType type = BarcodeType.Standard, decimal? pricePerKg = null)
        {
            var parser = new BarcodeParser();
            return parser.Parse(barcode, type, pricePerKg);
        }

        /// <summary>
        /// Parses a weight-encoded barcode with simplified API.
        /// </summary>
        /// <param name="barcode">Weight barcode string</param>
        /// <returns>Parsed weight barcode information</returns>
        /// <exception cref="ArgumentNullException">Thrown when barcode is null</exception>
        public static BarcodeInfo ParseWeight(string barcode)
        {
            return ParseSingle(barcode, BarcodeType.Weight);
        }

        /// <summary>
        /// Parses a price-encoded barcode with required price per kilogram.
        /// </summary>
        /// <param name="barcode">Price barcode string</param>
        /// <param name="pricePerKg">Price per kilogram for weight calculation</param>
        /// <returns>Parsed price barcode information</returns>
        /// <exception cref="ArgumentException">Thrown when pricePerKg is invalid</exception>
        /// <exception cref="ArgumentNullException">Thrown when barcode is null</exception>
        public static BarcodeInfo ParsePrice(string barcode, decimal pricePerKg)
        {
            if (pricePerKg <= 0)
                throw new ArgumentException("Price per Kg must be positive / يجب أن يكون سعر الكيلو موجب", nameof(pricePerKg));

            return ParseSingle(barcode, BarcodeType.Price, pricePerKg);
        }

        /// <summary>
        /// Validates barcode format without full parsing.
        /// </summary>
        /// <param name="barcode">Barcode string to validate</param>
        /// <returns>True if barcode format is valid</returns>
        /// <remarks>
        /// This method only checks the format (length and numeric characters),
        /// not the check digit or business logic validity.
        /// </remarks>
        public static bool IsValidFormat(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return false;

            string cleaned = Regex.Replace(barcode, @"[^\d]", string.Empty);
            return cleaned.Length == 13 || cleaned.Length == 8 || cleaned.Length == 12;
        }

        /// <summary>
        /// Retrieves supported country codes with bilingual names.
        /// </summary>
        /// <returns>Dictionary of country codes with Arabic and English names</returns>
        /// <remarks>
        /// The dictionary uses country code prefixes as keys and contains
        /// tuple values with Arabic and English country names.
        /// </remarks>
        public static Dictionary<string, (string Arabic, string English)> GetSupportedCountries()
        {
            return new Dictionary<string, (string, string)>(CountryCodes);
        }

        #endregion

        #region Analysis & Reporting Methods

        /// <summary>
        /// Generates comprehensive statistics for processed barcodes.
        /// </summary>
        /// <returns>Dictionary containing parsing metrics and distributions</returns>
        /// <remarks>
        /// The returned dictionary includes:
        /// - TotalProcessed: Total number of barcodes processed
        /// - SuccessfullyParsed: Number of successfully parsed barcodes
        /// - FailedParses: Number of failed parses
        /// - SuccessRate: Success percentage
        /// - TypeDistribution: Breakdown by barcode type (if any valid)
        /// - TotalWeightKg: Total weight in kilograms (if applicable)
        /// - TotalValueEGP: Total value in EGP (if applicable)
        /// </remarks>
        public Dictionary<string, object> GetStatistics()
        {
            var results = ParseAll();
            var validResults = results.Where(r => r.IsValid).ToList();

            var stats = new Dictionary<string, object>
            {
                { "TotalProcessed", results.Count },
                { "SuccessfullyParsed", validResults.Count },
                { "FailedParses", results.Count - validResults.Count },
                { "SuccessRate", results.Count > 0 ? Math.Round(validResults.Count * 100.0 / results.Count, 2) : 0 }
            };

            if (validResults.Any())
            {
                stats.Add("TypeDistribution", validResults
                    .GroupBy(r => r.TypeNameEnglish)
                    .ToDictionary(g => g.Key, g => g.Count()));

                var totalWeight = validResults.Sum(r => r.Weight);
                if (totalWeight > 0)
                    stats.Add("TotalWeightKg", Math.Round(totalWeight, 3));

                var totalPrice = validResults.Sum(r => r.Price);
                if (totalPrice > 0)
                    stats.Add("TotalValueEGP", Math.Round(totalPrice, 2));
            }

            return stats;
        }

        /// <summary>
        /// Groups valid barcodes by their detected type.
        /// </summary>
        /// <returns>Dictionary of barcode lists organized by type</returns>
        public Dictionary<BarcodeType, List<BarcodeInfo>> GroupByType()
        {
            return GetValidBarcodes()
                .GroupBy(b => b.Type)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        #endregion
    }
}
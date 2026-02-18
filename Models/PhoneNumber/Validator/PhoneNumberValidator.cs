using NumericValidation.EG.Models.PhoneNumber.Data;
using NumericValidation.EG.Models.PhoneNumber.Enums;
using NumericValidation.EG.Models.PhoneNumber.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NumericValidation.EG.Models.PhoneNumber.Validator
{
    /// <summary>
    /// Validates Egyptian mobile numbers with multiple input formats support
    /// </summary>
    public partial class PhoneNumberValidator
    {
        #region Private Fields

        private List<string> _numbers = new List<string>();
        private ValidationOptions _options = ValidationOptions.Default;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor - creates empty validator with default options
        /// </summary>
        public PhoneNumberValidator()
        {
            _numbers = new List<string>();
            _options = ValidationOptions.Default;
        }

        /// <summary>
        /// Constructor with custom validation options
        /// </summary>
        public PhoneNumberValidator(ValidationOptions options)
        {
            _numbers = new List<string>();
            _options = options; // The fix has been completed here.
        }

        /// <summary>
        /// Constructor with single phone number and default options
        /// </summary>
        public PhoneNumberValidator(string number) : this()
        {
            _numbers = new List<string> { number };
        }

        /// <summary>
        /// Constructor with single phone number and custom options
        /// </summary>
        public PhoneNumberValidator(string number, ValidationOptions options) : this(options)
        {
            _numbers = new List<string> { number };
        }

        /// <summary>
        /// Constructor with single phone number as long integer
        /// </summary>
        public PhoneNumberValidator(long number) : this()
        {
            _numbers = new List<string> { number.ToString() };
        }

        /// <summary>
        /// Constructor with multiple phone numbers as string array
        /// </summary>
        public PhoneNumberValidator(params string[] numbers) : this()
        {
            _numbers = new List<string>(numbers);
        }

        /// <summary>
        /// Constructor with multiple phone numbers and custom options
        /// </summary>
        public PhoneNumberValidator(ValidationOptions options, params string[] numbers) : this(options)
        {
            _numbers = new List<string>(numbers);
        }

        /// <summary>
        /// Constructor with multiple phone numbers as long array
        /// </summary>
        public PhoneNumberValidator(params long[] numbers) : this()
        {
            _numbers = numbers.Select(n => n.ToString()).ToList();
        }

        /// <summary>
        /// Constructor with comma-separated phone numbers string
        /// </summary>
        public PhoneNumberValidator(string commaSeparated, bool isCommaSeparated = true) : this()
        {
            if (isCommaSeparated)
            {
                _numbers = commaSeparated
                    .Split(new[] { ',', ';', '|', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => n.Trim())
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .ToList();
            }
            else
            {
                _numbers = new List<string> { commaSeparated };
            }
        }

        /// <summary>
        /// Constructor with collection of phone numbers
        /// </summary>
        public PhoneNumberValidator(IEnumerable<string> numbers) : this()
        {
            _numbers = new List<string>(numbers);
        }

        /// <summary>
        /// Constructor with collection of phone numbers as integers
        /// </summary>
        public PhoneNumberValidator(IEnumerable<long> numbers) : this()
        {
            _numbers = numbers.Select(n => n.ToString()).ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the validation options for this validator instance
        /// </summary>
        public ValidationOptions Options
        {
            get => _options;
            set => _options = value; // The fix has been completed here.
        }

        /// <summary>
        /// Gets the count of phone numbers currently in the validator
        /// </summary>
        public int Count => _numbers.Count;

        /// <summary>
        /// Gets all phone numbers currently in the validator
        /// </summary>
        public IEnumerable<string> Numbers => _numbers.AsReadOnly();

        #endregion

        #region Configuration Methods

        /// <summary>
        /// Clears all phone numbers from the validator
        /// </summary>
        public void Clear()
        {
            _numbers.Clear();
        }

        /// <summary>
        /// Removes duplicate phone numbers from the list
        /// </summary>
        public void RemoveDuplicates()
        {
            _numbers = _numbers.Distinct().ToList();
        }

        /// <summary>
        /// Updates the validation options for this validator
        /// </summary>
        public void SetOptions(ValidationOptions options)
        {
            _options = options; // The fix has been completed here.
        }

        /// <summary>
        /// Resets validation options to default
        /// </summary>
        public void ResetOptions()
        {
            _options = ValidationOptions.Default;
        }

        #endregion

        #region Public Instance Methods

        /// <summary>
        /// Adds a phone number to the validator
        /// </summary>
        public bool AddNumber(string number)
        {
            if (!string.IsNullOrWhiteSpace(number))
            {
                _numbers.Add(number);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a phone number as integer to the validator
        /// </summary>
        public void AddNumber(long number)
        {
            _numbers.Add(number.ToString());
        }

        /// <summary>
        /// Adds multiple phone numbers from comma-separated string
        /// </summary>
        public int AddNumbers(string commaSeparated)
        {
            var nums = commaSeparated
                .Split(new[] { ',', ';', '|', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.Trim())
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .ToList();

            _numbers.AddRange(nums);
            return nums.Count;
        }

        /// <summary>
        /// Adds multiple phone numbers from array
        /// </summary>
        public int AddNumbers(params string[] numbers)
        {
            var validNumbers = numbers.Where(n => !string.IsNullOrWhiteSpace(n)).ToList();
            _numbers.AddRange(validNumbers);
            return validNumbers.Count;
        }

        /// <summary>
        /// Validates all phone numbers in the validator using current options
        /// </summary>
        public List<PhoneNumberInfo> ValidateAll()
        {
            return _numbers.Select(n => Validate(n, _options)).ToList();
        }

        /// <summary>
        /// Validates all phone numbers using custom options
        /// </summary>
        public List<PhoneNumberInfo> ValidateAll(ValidationOptions options)
        {
            return _numbers.Select(n => Validate(n, options)).ToList();
        }

        /// <summary>
        /// Validates all phone numbers and returns only valid ones
        /// </summary>
        public List<PhoneNumberInfo> GetValidNumbers()
        {
            return ValidateAll().Where(info => info.IsValid).ToList();
        }

        /// <summary>
        /// Validates all phone numbers and returns only invalid ones
        /// </summary>
        public List<PhoneNumberInfo> GetInvalidNumbers()
        {
            return ValidateAll().Where(info => !info.IsValid).ToList();
        }

        /// <summary>
        /// Gets validation summary with carrier distribution
        /// </summary>
        public Dictionary<string, object> GetValidationSummary()
        {
            var results = ValidateAll();
            var summary = new Dictionary<string, object>
            {
                { "Total", results.Count },
                { "Valid", results.Count(r => r.IsValid) },
                { "Invalid", results.Count(r => !r.IsValid) },
                { "Options", _options }
            };

            // Add carrier statistics
            var carriers = results
                .Where(r => r.IsValid && !string.IsNullOrEmpty(r.Carrier))
                .GroupBy(r => r.Carrier)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var carrier in carriers)
            {
                if (!summary.ContainsKey(carrier.Key))
                {
                    summary.Add(carrier.Key, carrier.Value);
                }
            }

            // Calculate success rate
            int total = results.Count;
            int valid = results.Count(r => r.IsValid);
            summary["SuccessRate"] = total > 0 ? valid * 100.0 / total : 0.0;
            summary["SuccessRatePercentage"] = total > 0 ? $"{valid * 100.0 / total:F1}%" : "0%";

            return summary;
        }

        /// <summary>
        /// Gets phone numbers grouped by carrier
        /// </summary>
        public Dictionary<string, List<PhoneNumberInfo>> GroupByCarrier()
        {
            var validNumbers = GetValidNumbers();
            return validNumbers
                .GroupBy(info => info.Carrier)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Exports valid numbers to a comma-separated string
        /// </summary>
        public string ExportValidNumbers()
        {
            var validNumbers = GetValidNumbers();
            return string.Join(", ", validNumbers.Select(info => info.Number));
        }

        /// <summary>
        /// Exports valid numbers with specified format
        /// </summary>
        public string ExportValidNumbers(string format)
        {
            var validNumbers = GetValidNumbers();
            return string.Join(", ", validNumbers.Select(info => string.Format(format, info.Number)));
        }

        #endregion

        #region Static Validation Methods

        /// <summary>
        /// Validates a single Egyptian mobile number with default options
        /// </summary>
        public static PhoneNumberInfo Validate(string number)
        {
            return Validate(number, ValidationOptions.Default);
        }

        /// <summary>
        /// Validates a single Egyptian mobile number with custom options
        /// </summary>
        public static PhoneNumberInfo Validate(string number, ValidationOptions options)
        {
            var info = new PhoneNumberInfo
            {
                OriginalNumber = number,
                Number = number
            };

            try
            {
                if (string.IsNullOrWhiteSpace(number))
                {
                    info.IsValid = false;
                    info.ErrorMessage = "Phone number is empty";
                    return info;
                }

                // Clean the phone number based on options
                string cleanedNumber = CleanPhoneNumber(number, options);
                info.CleanedNumber = cleanedNumber;

                if (string.IsNullOrEmpty(cleanedNumber))
                {
                    info.IsValid = false;
                    info.ErrorMessage = "Phone number contains no valid digits";
                    return info;
                }

                // Normalize to local format based on options
                string localFormat = NormalizeToLocalFormat(cleanedNumber, options);

                if (localFormat == "INVALID")
                {
                    info.IsValid = false;
                    info.ErrorMessage = "Invalid phone number format";
                    return info;
                }

                info.Number = localFormat;

                // Basic format validation
                if (!Regex.IsMatch(localFormat, @"^01\d{9}$"))
                {
                    info.IsValid = false;

                    if (localFormat.Length != 11)
                    {
                        info.ErrorMessage = $"Invalid length ({localFormat.Length} digits). Egyptian mobile numbers must be 11 digits";
                    }
                    else if (!localFormat.StartsWith("01"))
                    {
                        info.ErrorMessage = "Must start with 01";
                    }
                    else
                    {
                        info.ErrorMessage = "Invalid Egyptian mobile number format";
                    }

                    return info;
                }

                // Extract first 4 digits (prefix)
                string prefix = localFormat.Substring(0, 4);

                // Check if we should validate special services
                if ((options & ValidationOptions.ValidateSpecialServices) == 0 && prefix.StartsWith("019"))
                {
                    info.IsValid = false;
                    info.ErrorMessage = $"Special service numbers ({prefix}) validation is disabled";
                    return info;
                }

                // Carrier validation based on options
                if ((options & ValidationOptions.StrictCarrierValidation) != 0)
                {
                    if (CarrierData.CarrierPrefixes.TryGetValue(prefix, out string carrier))
                    {
                        info.IsValid = true;
                        info.Carrier = carrier;
                        info.Prefix = prefix;
                        info.ServiceType = GetServiceType(prefix, carrier);
                        info.ErrorMessage = $"Valid {carrier} number";
                    }
                    else
                    {
                        info.IsValid = false;
                        info.ErrorMessage = $"Unknown carrier for prefix {prefix}. This may not be a valid Egyptian mobile number";
                    }
                }
                else
                {
                    // Lenient mode: accept any valid format even if prefix not in dictionary
                    if (CarrierData.CarrierPrefixes.TryGetValue(prefix, out string carrier))
                    {
                        info.Carrier = carrier;
                        info.ServiceType = GetServiceType(prefix, carrier);
                        info.ErrorMessage = $"Valid {carrier} number";
                    }
                    else
                    {
                        info.Carrier = "Unknown Carrier";
                        info.ServiceType = "Mobile";
                        info.ErrorMessage = "Valid Egyptian mobile number (carrier unknown)";
                    }

                    info.IsValid = true;
                    info.Prefix = prefix;
                }
            }
            catch (Exception ex)
            {
                info.IsValid = false;
                info.ErrorMessage = $"Validation error: {ex.Message}";
            }

            return info;
        }

        /// <summary>
        /// Validates a phone number as integer with default options
        /// </summary>
        public static PhoneNumberInfo Validate(long number)
        {
            return Validate(number.ToString());
        }

        /// <summary>
        /// Validates a phone number as integer with custom options
        /// </summary>
        public static PhoneNumberInfo Validate(long number, ValidationOptions options)
        {
            return Validate(number.ToString(), options);
        }

        /// <summary>
        /// Validates multiple phone numbers from comma-separated string with default options
        /// </summary>
        public static List<PhoneNumberInfo> ValidateCommaSeparated(string commaSeparated)
        {
            return ValidateCommaSeparated(commaSeparated, ValidationOptions.Default);
        }

        /// <summary>
        /// Validates multiple phone numbers from comma-separated string with custom options
        /// </summary>
        public static List<PhoneNumberInfo> ValidateCommaSeparated(string commaSeparated, ValidationOptions options)
        {
            var numbers = commaSeparated
                .Split(new[] { ',', ';', '|', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.Trim())
                .Where(n => !string.IsNullOrWhiteSpace(n));

            return numbers.Select(n => Validate(n, options)).ToList();
        }

        /// <summary>
        /// Validates a list of phone numbers with default options
        /// </summary>
        public static List<PhoneNumberInfo> ValidateList(IEnumerable<string> numbers)
        {
            return ValidateList(numbers, ValidationOptions.Default);
        }

        /// <summary>
        /// Validates a list of phone numbers with custom options
        /// </summary>
        public static List<PhoneNumberInfo> ValidateList(IEnumerable<string> numbers, ValidationOptions options)
        {
            return numbers.Select(n => Validate(n, options)).ToList();
        }

        /// <summary>
        /// Validates a list of phone numbers as integers with default options
        /// </summary>
        public static List<PhoneNumberInfo> ValidateList(IEnumerable<long> numbers)
        {
            return ValidateList(numbers, ValidationOptions.Default);
        }

        /// <summary>
        /// Validates a list of phone numbers as integers with custom options
        /// </summary>
        public static List<PhoneNumberInfo> ValidateList(IEnumerable<long> numbers, ValidationOptions options)
        {
            return numbers.Select(n => Validate(n.ToString(), options)).ToList();
        }

        /// <summary>
        /// Quick validation - returns true/false without details using default options
        /// </summary>
        public static bool IsValid(string number)
        {
            return IsValid(number, ValidationOptions.Default);
        }

        /// <summary>
        /// Quick validation - returns true/false without details using custom options
        /// </summary>
        public static bool IsValid(string number, ValidationOptions options)
        {
            try
            {
                var info = Validate(number, options);
                return info.IsValid;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Quick validation for integer numbers with default options
        /// </summary>
        public static bool IsValid(long number)
        {
            return IsValid(number.ToString());
        }

        /// <summary>
        /// Quick validation for integer numbers with custom options
        /// </summary>
        public static bool IsValid(long number, ValidationOptions options)
        {
            return IsValid(number.ToString(), options);
        }

        #endregion

        #region Information Methods

        /// <summary>
        /// Gets all supported carrier prefixes
        /// </summary>
        public static Dictionary<string, string> GetSupportedPrefixes()
        {
            return CarrierData.GetSupportedPrefixes();
        }

        /// <summary>
        /// Gets all carriers operating in Egypt
        /// </summary>
        public static List<string> GetCarriers()
        {
            return CarrierData.GetCarriers();
        }

        /// <summary>
        /// Checks if a prefix belongs to a specific carrier
        /// </summary>
        public static bool IsCarrierPrefix(string prefix, string carrier)
        {
            return CarrierData.IsCarrierPrefix(prefix, carrier);
        }

        /// <summary>
        /// Gets prefixes for a specific carrier
        /// </summary>
        public static List<string> GetPrefixesForCarrier(string carrier)
        {
            return CarrierData.GetPrefixesForCarrier(carrier);
        }

        /// <summary>
        /// Checks if a number is from a specific carrier using default options
        /// </summary>
        public static bool IsFromCarrier(string number, string carrier)
        {
            return IsFromCarrier(number, carrier, ValidationOptions.Default);
        }

        /// <summary>
        /// Checks if a number is from a specific carrier using custom options
        /// </summary>
        public static bool IsFromCarrier(string number, string carrier, ValidationOptions options)
        {
            var info = Validate(number, options);
            if (!info.IsValid)
                return false;

            return info.Carrier?.Equals(carrier, StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Gets detailed information about a carrier
        /// </summary>
        public static Dictionary<string, object> GetCarrierInfo(string carrier)
        {
            return CarrierData.GetCarrierInfo(carrier);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Cleans phone number from special characters and spaces based on options
        /// </summary>
        private static string CleanPhoneNumber(string number, ValidationOptions options)
        {
            if (string.IsNullOrWhiteSpace(number))
                return string.Empty;

            string processed = number;

            // Convert Arabic Indic numerals if enabled
            if ((options & ValidationOptions.AcceptArabicIndicNumerals) != 0)
            {
                processed = ConvertArabicIndicDigits(processed);
            }

            // Remove formatting characters if formatted numbers are not accepted
            if ((options & ValidationOptions.AcceptFormattedNumbers) == 0)
            {
                // Only keep digits and plus sign
                processed = Regex.Replace(processed, @"[^\d+]", "");
            }
            else
            {
                // Remove common formatting characters but keep structure for now
                processed = Regex.Replace(processed, @"[^\d+\-().\s]", "");
            }

            return processed;
        }

        /// <summary>
        /// Converts Arabic Indic numerals to Latin numerals
        /// </summary>
        private static string ConvertArabicIndicDigits(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var arabicIndic = "٠١٢٣٤٥٦٧٨٩";
            var latin = "0123456789";

            char[] chars = input.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                int index = arabicIndic.IndexOf(chars[i]);
                if (index >= 0)
                {
                    chars[i] = latin[index];
                }
            }

            return new string(chars);
        }

        /// <summary>
        /// Normalizes international format to local Egyptian format based on options
        /// </summary>
        private static string NormalizeToLocalFormat(string number, ValidationOptions options)
        {
            if (string.IsNullOrEmpty(number))
                return "INVALID";

            string cleanNumber = Regex.Replace(number, @"[^\d+]", ""); // Clean all non-digit characters

            // === International format (+20) ===
            if ((options & ValidationOptions.AcceptInternationalFormats) != 0 && cleanNumber.StartsWith("+20"))
            {
                // +2010XXXXXXXXX → 010XXXXXXXXX
                if (cleanNumber.Length == 13 && cleanNumber[3] == '1')
                {
                    string rest = cleanNumber.Substring(4); // Remove +201
                    if (rest.Length == 9)
                    {
                        char secondDigit = cleanNumber[4];
                        if (secondDigit == '0' || secondDigit == '1' || secondDigit == '2' || secondDigit == '4' || secondDigit == '5')
                        {
                            return "01" + rest;
                        }
                    }
                }
                return "INVALID";
            }

            // === International format (20 without +) ===
            if ((options & ValidationOptions.AcceptInternationalFormats) != 0 && cleanNumber.StartsWith("20") && cleanNumber.Length == 12)
            {
                if (cleanNumber[2] == '1')
                {
                    string rest = cleanNumber.Substring(3); // Remove 201
                    if (rest.Length == 9)
                    {
                        char secondDigit = cleanNumber[3];
                        if (secondDigit == '0' || secondDigit == '1' || secondDigit == '2' || secondDigit == '4' || secondDigit == '5')
                        {
                            return "01" + rest;
                        }
                    }
                }
                return "INVALID";
            }

            // === International format (0020) ===
            if ((options & ValidationOptions.AcceptInternationalFormats) != 0 && cleanNumber.StartsWith("0020"))
            {
                // 002010XXXXXXXXX → 010XXXXXXXXX
                if (cleanNumber.Length == 14 && cleanNumber[4] == '1')
                {
                    string rest = cleanNumber.Substring(5); // Remove 00201
                    if (rest.Length == 9)
                    {
                        char secondDigit = cleanNumber[5];
                        if (secondDigit == '0' || secondDigit == '1' || secondDigit == '2' || secondDigit == '4' || secondDigit == '5')
                        {
                            return "01" + rest;
                        }
                    }
                }
                return "INVALID";
            }

            // === Local format (01) ===
            if (cleanNumber.Length == 11 && cleanNumber.StartsWith("01"))
                return cleanNumber;

            // === Numbers without leading zero ===
            if ((options & ValidationOptions.AcceptNumbersWithoutLeadingZero) != 0 && cleanNumber.Length == 10 && cleanNumber.StartsWith("1"))
            {
                char secondDigit = cleanNumber[1];
                if (secondDigit == '0' || secondDigit == '1' || secondDigit == '2' || secondDigit == '4' || secondDigit == '5')
                {
                    return "0" + cleanNumber;
                }
            }

            // === Auto-fix incomplete numbers ===
            if ((options & ValidationOptions.AutoFixIncompleteNumbers) != 0 && cleanNumber.Length == 10 && cleanNumber.StartsWith("01"))
            {
                // Add missing digit (typically 0 or repeat last digit)
                return cleanNumber + "0";
            }

            return "INVALID";
        }

        /// <summary>
        /// Determines service type based on prefix
        /// </summary>
        private static string GetServiceType(string prefix, string carrier)
        {
            if (prefix.StartsWith("019"))
                return "Value Added Service";

            if (carrier.Contains("WE", StringComparison.OrdinalIgnoreCase) ||
                carrier.Contains("Telecom Egypt", StringComparison.OrdinalIgnoreCase))
                return "Fixed & Mobile";

            return "Mobile";
        }

        #endregion

        #region Bulk Operations

        /// <summary>
        /// Validates a large batch of phone numbers with progress reporting using default options
        /// </summary>
        public static List<PhoneNumberInfo> ValidateBatch(IEnumerable<string> numbers, int batchSize = 1000)
        {
            return ValidateBatch(numbers, ValidationOptions.Default, batchSize);
        }

        /// <summary>
        /// Validates a large batch of phone numbers with progress reporting using custom options
        /// </summary>
        public static List<PhoneNumberInfo> ValidateBatch(IEnumerable<string> numbers, ValidationOptions options, int batchSize = 1000)
        {
            var allResults = new List<PhoneNumberInfo>();
            var numberList = numbers.ToList();

            for (int i = 0; i < numberList.Count; i += batchSize)
            {
                var batch = numberList.Skip(i).Take(batchSize);
                var batchResults = batch.Select(n => Validate(n, options)).ToList();
                allResults.AddRange(batchResults);
            }

            return allResults;
        }

        /// <summary>
        /// Filters valid numbers from a list using default options
        /// </summary>
        public static List<string> FilterValidNumbers(IEnumerable<string> numbers)
        {
            return FilterValidNumbers(numbers, ValidationOptions.Default);
        }

        /// <summary>
        /// Filters valid numbers from a list using custom options
        /// </summary>
        public static List<string> FilterValidNumbers(IEnumerable<string> numbers, ValidationOptions options)
        {
            return numbers
                .Where(n => IsValid(n, options))
                .Select(n => Validate(n, options).Number)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Formats phone number to standard Egyptian format
        /// </summary>
        public static string FormatNumber(string number)
        {
            var info = Validate(number);
            if (!info.IsValid || string.IsNullOrEmpty(info.Number))
                return number;

            string cleanNumber = info.Number;
            if (cleanNumber.Length == 11)
            {
                return $"{cleanNumber.Substring(0, 3)} {cleanNumber.Substring(3, 3)} {cleanNumber.Substring(6, 4)}";
            }

            return cleanNumber;
        }

        /// <summary>
        /// Formats phone number with custom format
        /// </summary>
        public static string FormatNumber(string number, string format)
        {
            var info = Validate(number);
            if (!info.IsValid || string.IsNullOrEmpty(info.Number))
                return number;

            try
            {
                return string.Format(format, info.Number);
            }
            catch
            {
                return info.Number;
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Extracts just the digits from a phone number
        /// </summary>
        public static string ExtractDigits(string number)
        {
            if (string.IsNullOrEmpty(number))
                return string.Empty;

            return Regex.Replace(number, @"\D", "");
        }

        /// <summary>
        /// Checks if two phone numbers are the same (after normalization)
        /// </summary>
        public static bool AreSameNumber(string number1, string number2)
        {
            var info1 = Validate(number1);
            var info2 = Validate(number2);

            if (!info1.IsValid || !info2.IsValid)
                return false;

            return info1.Number == info2.Number;
        }

        /// <summary>
        /// Generates sample phone numbers for a specific carrier
        /// </summary>
        public static List<string> GenerateSampleNumbers(string carrier, int count = 5)
        {
            var prefixes = GetPrefixesForCarrier(carrier);
            if (prefixes.Count == 0)
                return new List<string>();

            var samples = new List<string>();
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                string prefix = prefixes[random.Next(prefixes.Count)];
                string suffix = random.Next(1000000, 9999999).ToString();
                samples.Add($"{prefix}{suffix}");
            }

            return samples;
        }

        #endregion
    }
}
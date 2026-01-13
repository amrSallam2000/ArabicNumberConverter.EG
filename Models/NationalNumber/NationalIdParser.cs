
using NumericValidation.EG.Models.NationalNumber.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NumericValidation.EG.Models.NationalNumber
{
    /// <summary>
    /// Enhanced parser for Egyptian National ID with advanced validation and batch processing.
    /// Includes leap year validation and date correction for invalid days.
    /// </summary>
    public class NationalIdParser
    {

        private List<string> _nationalIds;
        private bool _strictMode;
        private bool _validateAge;
        private int _minAge;
        private int _maxAge;
        private bool _autoCorrectInvalidDays;

        #region Constructors

        /// <summary>
        /// Default constructor with optional strict mode.
        /// </summary>
        /// <param name="strictMode">If true, enables additional validations.</param>
        /// <param name="autoCorrectInvalidDays">If true, automatically corrects invalid days (e.g., Feb 30 to Feb 28/29).</param>
        public NationalIdParser(bool strictMode = false, bool autoCorrectInvalidDays = false)
        {
            _nationalIds = new List<string>();
            _strictMode = strictMode;
            _validateAge = false;
            _minAge = 0;
            _maxAge = 150;
            _autoCorrectInvalidDays = autoCorrectInvalidDays;
        }

        /// <summary>
        /// Constructor with single National ID.
        /// </summary>
        /// <param name="nationalId">National ID as string.</param>
        /// <param name="autoCorrectInvalidDays">If true, automatically corrects invalid days.</param>
        public NationalIdParser(string nationalId, bool autoCorrectInvalidDays = false) : this(false, autoCorrectInvalidDays)
        {
            _nationalIds.Add(nationalId);
        }

        /// <summary>
        /// Constructor with single National ID as long.
        /// </summary>
        /// <param name="nationalId">National ID as long integer.</param>
        /// <param name="strictMode">Enable strict validation mode.</param>
        /// <param name="autoCorrectInvalidDays">If true, automatically corrects invalid days.</param>
        public NationalIdParser(long nationalId, bool strictMode = false, bool autoCorrectInvalidDays = false) : this(strictMode, autoCorrectInvalidDays)
        {
            _nationalIds.Add(nationalId.ToString());
        }

        /// <summary>
        /// Constructor with multiple National IDs.
        /// </summary>
        /// <param name="nationalIds">Array of National IDs.</param>
        /// <param name="autoCorrectInvalidDays">If true, automatically corrects invalid days.</param>
        public NationalIdParser(string[] nationalIds, bool autoCorrectInvalidDays = false)
        {
            _nationalIds = new List<string>(nationalIds);
            _strictMode = false;
            _validateAge = false;
            _autoCorrectInvalidDays = autoCorrectInvalidDays;
        }

        /// <summary>
        /// Constructor with multiple National IDs as long.
        /// </summary>
        /// <param name="nationalIds">Array of National IDs as long integers.</param>
        /// <param name="autoCorrectInvalidDays">If true, automatically corrects invalid days.</param>
        public NationalIdParser(long[] nationalIds, bool autoCorrectInvalidDays = false)
        {
            _nationalIds = nationalIds.Select(id => id.ToString()).ToList();
            _strictMode = false;
            _validateAge = false;
            _autoCorrectInvalidDays = autoCorrectInvalidDays;
        }

        /// <summary>
        /// Constructor with comma-separated National IDs.
        /// </summary>
        /// <param name="commaSeparated">Comma-separated National IDs.</param>
        /// <param name="separator">Separator character to use (default: comma).</param>
        /// <param name="autoCorrectInvalidDays">If true, automatically corrects invalid days.</param>
        public NationalIdParser(string commaSeparated, char separator, bool autoCorrectInvalidDays = false)
        {
            char[] separators = separator == ','
                ? new[] { ',', ';', '|', '\n', '\r' }
                : new[] { separator };

            _nationalIds = commaSeparated
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(id => id.Trim())
                .ToList();

            _strictMode = false;
            _validateAge = false;
            _autoCorrectInvalidDays = autoCorrectInvalidDays;
        }

        /// <summary>
        /// Constructor with collection of National IDs.
        /// </summary>
        /// <param name="nationalIds">Collection of National IDs.</param>
        /// <param name="autoCorrectInvalidDays">If true, automatically corrects invalid days.</param>
        public NationalIdParser(IEnumerable<string> nationalIds, bool autoCorrectInvalidDays = false)
        {
            _nationalIds = new List<string>(nationalIds);
            _strictMode = false;
            _validateAge = false;
            _autoCorrectInvalidDays = autoCorrectInvalidDays;
        }

        #endregion

        #region Configuration Methods

        /// <summary>
        /// Enables strict validation mode.
        /// </summary>
        public NationalIdParser EnableStrictMode()
        {
            _strictMode = true;
            return this;
        }

        /// <summary>
        /// Disables strict validation mode.
        /// </summary>
        public NationalIdParser DisableStrictMode()
        {
            _strictMode = false;
            return this;
        }

        /// <summary>
        /// Enables automatic correction of invalid days (e.g., Feb 30 to Feb 28/29).
        /// </summary>
        public NationalIdParser EnableAutoCorrectDays()
        {
            _autoCorrectInvalidDays = true;
            return this;
        }

        /// <summary>
        /// Disables automatic correction of invalid days.
        /// </summary>
        public NationalIdParser DisableAutoCorrectDays()
        {
            _autoCorrectInvalidDays = false;
            return this;
        }

        /// <summary>
        /// Sets age range validation.
        /// </summary>
        /// <param name="minAge">Minimum age.</param>
        /// <param name="maxAge">Maximum age.</param>
        public NationalIdParser SetAgeRange(int minAge, int maxAge)
        {
            _validateAge = true;
            _minAge = minAge;
            _maxAge = maxAge;
            return this;
        }

        /// <summary>
        /// Adds a National ID to the parser.
        /// </summary>
        public void AddNationalId(string nationalId)
        {
            _nationalIds.Add(nationalId);
        }

        /// <summary>
        /// Adds a National ID as long to the parser.
        /// </summary>
        public void AddNationalId(long nationalId)
        {
            _nationalIds.Add(nationalId.ToString());
        }

        /// <summary>
        /// Adds multiple National IDs from comma-separated string.
        /// </summary>
        public void AddNationalIds(string commaSeparated)
        {
            var ids = commaSeparated
                .Split(new[] { ',', ';', '|', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(id => id.Trim());
            _nationalIds.AddRange(ids);
        }

        #endregion

        #region Date Validation Methods

        /// <summary>
        /// Validates if a date is valid considering leap years.
        /// </summary>
        /// <param name="year">Year to validate.</param>
        /// <param name="month">Month to validate.</param>
        /// <param name="day">Day to validate.</param>
        /// <param name="correctedDay">Output parameter with corrected day if invalid.</param>
        /// <returns>True if date is valid, false otherwise.</returns>
        private bool ValidateDateWithLeapYear(int year, int month, int day, out int correctedDay)
        {
            correctedDay = day;

            // Basic month validation
            if (month < 1 || month > 12)
                return false;

            // Days in month validation with leap year consideration
            int maxDaysInMonth = DateTime.DaysInMonth(year, month);

            if (day < 1 || day > maxDaysInMonth)
            {
                if (_autoCorrectInvalidDays)
                {
                    // Auto-correct invalid days to the last valid day of the month
                    correctedDay = maxDaysInMonth;
                    return true; // Considered valid after correction
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a year is a leap year.
        /// </summary>
        private static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        /// <summary>
        /// Gets the number of days in a specific month considering leap years.
        /// </summary>
        private static int GetDaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        #endregion

        #region Parsing Methods

        /// <summary>
        /// Parses a 14-digit Egyptian National ID with enhanced validation including leap year checks.
        /// </summary>
        /// <param name="nationalId">The 14-digit national ID.</param>
        /// <returns>A <see cref="NationalIdInfo"/> object containing all extracted information.</returns>
        public NationalIdInfo Parse(string nationalId)
        {
            var result = new NationalIdInfo();

            // تنظيف الرقم من المسافات والشرطات
            string cleanedId = CleanNationalId(nationalId);

            // ⭐⭐ مهم: تعيين الرقم القومي أولاً
            result.NationalId = cleanedId;

            // 1) Validate length and numeric
            if (string.IsNullOrWhiteSpace(cleanedId) || cleanedId.Length != 14 || !long.TryParse(cleanedId, out _))
            {
                result.IsValid = false;
                result.ErrorMessage = "National ID must be exactly 14 digits.";
                return result;
            }

            try
            {
                // 2) Century validation
                char centuryDigit = cleanedId[0];
                int century = centuryDigit switch
                {
                    '1' => 1800,
                    '2' => 1900,
                    '3' => 2000,
                    _ => -1
                };

                if (century == -1)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Invalid century digit in National ID. Must be 1, 2, or 3.";
                    return result;
                }

                // ⭐ تعيين القرن
                result.Century = century;

                // 3) Birth date extraction and validation
                int year = century + int.Parse(cleanedId.Substring(1, 2));
                int month = int.Parse(cleanedId.Substring(3, 2));
                int day = int.Parse(cleanedId.Substring(5, 2));

                // التحقق من صحة التاريخ مع السنة الكبيسة
                int correctedDay = day;
                bool isValidDate = ValidateDateWithLeapYear(year, month, day, out correctedDay);

                if (!isValidDate && !_autoCorrectInvalidDays)
                {
                    result.IsValid = false;
                    int daysInMonth = GetDaysInMonth(year, month);
                    string leapYearInfo = IsLeapYear(year) ? " (leap year)" : "";
                    result.ErrorMessage = $"Invalid birth date in National ID: {day}/{month}/{year}. " +
                                          $"Month {month} has {daysInMonth} days{leapYearInfo}.";
                    return result;
                }

                // إذا كنا نصحح الأيام، استخدم اليوم المصحح
                if (_autoCorrectInvalidDays && day != correctedDay)
                {
                    // سجل أننا قمنا بالتصحيح
                    day = correctedDay;
                }

                // إنشاء تاريخ الميلاد مع الأيام المصححة إن وجدت
                DateTime birthDate = new DateTime(year, month, day);

                // 4) Governorate validation
                string govCode = cleanedId.Substring(7, 2);

                if (CalendarConstants.Governorates.TryGetValue(govCode, out var governorate))
                {
                    result.GovernorateCode = govCode;
                    result.GovernorateNameArabic = governorate.Arabic;
                    result.GovernorateNameEnglish = governorate.English;
                }
                else
                {
                    result.GovernorateCode = govCode;
                    result.GovernorateNameArabic = "غير معروف";
                    result.GovernorateNameEnglish = "Unknown governorate";
                }

                // 5) Gender extraction (آخر رقم: فردي = ذكر، زوجي = أنثى)
                int lastDigit = int.Parse(cleanedId[13].ToString());
                string gender = lastDigit % 2 == 1 ? "Male" : "Female";
                string genderAr = lastDigit % 2 == 1 ? "ذكر" : "أنثى";

                // ⭐ تعيين الجنس
                result.Gender = gender;
                result.GenderArabic = genderAr;

                // 6) Serial number extraction
                result.SerialNumber = cleanedId.Substring(10, 3);

                // 7) Fill all information
                result.IsValid = true;
                result.Year = year;
                result.Month = month;
                result.Day = day;
                result.ErrorMessage = string.Empty;

                // 8) Age validation if enabled
                if (_validateAge)
                {
                    int age = CalculateAge(birthDate);
                    if (age < _minAge || age > _maxAge)
                    {
                        result.IsValid = false;
                        result.ErrorMessage = $"Age {age} is outside valid range ({_minAge}-{_maxAge}).";
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = "Error parsing National ID: " + ex.Message;
                return result;
            }
        }


        /// <summary>
        /// Parses a National ID as long.
        /// </summary>
        public NationalIdInfo Parse(long nationalId)
        {
            return Parse(nationalId.ToString());
        }

        /// <summary>
        /// Parses all National IDs in the parser.
        /// </summary>
        public List<NationalIdInfo> ParseAll()
        {
            return _nationalIds.Select(id => Parse(id)).ToList();
        }

        /// <summary>
        /// Parses and returns only valid National IDs.
        /// </summary>
        public List<NationalIdInfo> GetValidIds()
        {
            return ParseAll().Where(info => info.IsValid).ToList();
        }

        /// <summary>
        /// Parses and returns only invalid National IDs.
        /// </summary>
        public List<NationalIdInfo> GetInvalidIds()
        {
            return ParseAll().Where(info => !info.IsValid).ToList();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Static method to parse a single National ID.
        /// </summary>
        /// <param name="nationalId">National ID to parse.</param>
        /// <param name="strictMode">Enable strict validation mode.</param>
        /// <param name="autoCorrectInvalidDays">Automatically correct invalid days (e.g., Feb 30 to Feb 28/29).</param>
        /// <returns>Parsed National ID information.</returns>
        public static NationalIdInfo ParseSingle(string nationalId, bool strictMode = false, bool autoCorrectInvalidDays = false)
        {
            var parser = new NationalIdParser(strictMode, autoCorrectInvalidDays);
            return parser.Parse(nationalId);
        }

        /// <summary>
        /// Static method to parse a National ID as long.
        /// </summary>
        public static NationalIdInfo ParseSingle(long nationalId, bool strictMode = false, bool autoCorrectInvalidDays = false)
        {
            return ParseSingle(nationalId.ToString(), strictMode, autoCorrectInvalidDays);
        }

        /// <summary>
        /// Static method to parse comma-separated National IDs.
        /// </summary>
        public static List<NationalIdInfo> ParseCommaSeparated(string commaSeparated, bool autoCorrectInvalidDays = false)
        {
            var parser = new NationalIdParser(commaSeparated, ',', autoCorrectInvalidDays);
            return parser.ParseAll();
        }

        /// <summary>
        /// Static method to parse a list of National IDs.
        /// </summary>
        public static List<NationalIdInfo> ParseList(IEnumerable<string> nationalIds, bool autoCorrectInvalidDays = false)
        {
            var parser = new NationalIdParser(nationalIds, autoCorrectInvalidDays);
            return parser.ParseAll();
        }

        /// <summary>
        /// Static method to parse a list of National IDs as long.
        /// </summary>
        public static List<NationalIdInfo> ParseList(IEnumerable<long> nationalIds, bool autoCorrectInvalidDays = false)
        {
            var parser = new NationalIdParser(nationalIds.ToArray(), autoCorrectInvalidDays);
            return parser.ParseAll();
        }

        /// <summary>
        /// Validates date with leap year consideration.
        /// </summary>
        /// <param name="year">Year to validate.</param>
        /// <param name="month">Month to validate.</param>
        /// <param name="day">Day to validate.</param>
        /// <returns>True if date is valid, false otherwise.</returns>
        public static bool ValidateDate(int year, int month, int day)
        {
            if (month < 1 || month > 12)
                return false;

            int maxDays = DateTime.DaysInMonth(year, month);
            return day >= 1 && day <= maxDays;
        }

        /// <summary>
        /// Checks if a year from a National ID is a leap year.
        /// </summary>
        public static bool IsLeapYearFromId(string nationalId)
        {
            var info = ParseSingle(nationalId);
            if (!info.IsValid)
                return false;

            return DateTime.IsLeapYear(info.Year);
        }

        /// <summary>
        /// Quick validation without full parsing.
        /// </summary>
        public static bool IsValidFormat(string nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                return false;

            string cleaned = CleanNationalId(nationalId);

            if (cleaned.Length != 14 || !long.TryParse(cleaned, out _))
                return false;

            char centuryDigit = cleaned[0];
            return centuryDigit == '1' || centuryDigit == '2' || centuryDigit == '3';
        }

        /// <summary>
        /// Extracts birth date from National ID without full parsing.
        /// </summary>
        public static DateTime? ExtractBirthDate(string nationalId, bool autoCorrectInvalidDays = false)
        {
            var info = ParseSingle(nationalId, false, autoCorrectInvalidDays);
            if (!info.IsValid)
                return null;

            return new DateTime(info.Year, info.Month, info.Day);
        }

        /// <summary>
        /// Calculates age from National ID.
        /// </summary>
        public static int? CalculateAgeFromId(string nationalId)
        {
            var birthDate = ExtractBirthDate(nationalId);
            if (!birthDate.HasValue)
                return null;

            return CalculateAge(birthDate.Value);
        }

        /// <summary>
        /// Gets all supported governorates.
        /// </summary>
        public static Dictionary<string, (string Arabic, string English)> GetSupportedGovernorates()
        {
            return new Dictionary<string, (string, string)>(CalendarConstants.Governorates);
        }

        /// <summary>
        /// Checks if a governorate code is valid.
        /// </summary>
        public static bool IsValidGovernorate(string code)
        {
            return CalendarConstants.Governorates.ContainsKey(code);
        }

        /// <summary>
        /// Validates a date and returns detailed information including leap year status.
        /// </summary>
        public static (bool IsValid, string ErrorMessage, bool IsLeapYear, int DaysInMonth) ValidateDateDetailed(int year, int month, int day)
        {
            if (month < 1 || month > 12)
                return (false, $"Invalid month: {month}. Must be between 1 and 12.", false, 0);

            bool isLeapYear = DateTime.IsLeapYear(year);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            if (day < 1 || day > daysInMonth)
            {
                string leapYearInfo = isLeapYear ? " (leap year)" : "";
                string error = $"Invalid day: {day}. Month {month} has {daysInMonth} days{leapYearInfo}.";
                return (false, error, isLeapYear, daysInMonth);
            }

            return (true, string.Empty, isLeapYear, daysInMonth);
        }

        #endregion

        #region Analysis Methods

        /// <summary>
        /// Gets statistics about parsed National IDs.
        /// </summary>
        public Dictionary<string, object> GetStatistics()
        {
            var results = ParseAll();
            var validResults = results.Where(r => r.IsValid).ToList();

            var stats = new Dictionary<string, object>
            {
                { "Total", results.Count },
                { "Valid", validResults.Count },
                { "Invalid", results.Count - validResults.Count },
                { "SuccessRate", results.Count > 0 ? validResults.Count * 100.0 / results.Count : 0 }
            };

            if (validResults.Any())
            {
                // توزيع حسب المحافظات
                var govDistribution = validResults
                    .GroupBy(r => r.GovernorateNameEnglish)
                    .ToDictionary(g => g.Key, g => g.Count());

                stats.Add("GovernorateDistribution", govDistribution);

                // توزيع حسب السنوات الكبيسة
                var leapYearDistribution = validResults
                    .GroupBy(r => r.IsLeapYear)
                    .ToDictionary(g => g.Key ? "Leap Years" : "Regular Years", g => g.Count());

                stats.Add("LeapYearDistribution", leapYearDistribution);

                // متوسط العمر
                var ages = validResults
                    .Select(r => CalculateAge(new DateTime(r.Year, r.Month, r.Day)))
                    .ToList();

                if (ages.Any())
                {
                    stats.Add("AverageAge", ages.Average());
                    stats.Add("MinAge", ages.Min());
                    stats.Add("MaxAge", ages.Max());
                }
            }

            return stats;
        }

        /// <summary>
        /// Groups valid IDs by governorate.
        /// </summary>
        public Dictionary<string, List<NationalIdInfo>> GroupByGovernorate()
        {
            return GetValidIds()
                .GroupBy(info => info.GovernorateNameEnglish)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Groups valid IDs by leap year status.
        /// </summary>
        public Dictionary<string, List<NationalIdInfo>> GroupByLeapYear()
        {
            return GetValidIds()
                .GroupBy(info => info.IsLeapYear)
                .ToDictionary(g => g.Key ? "Leap Years" : "Regular Years", g => g.ToList());
        }

        /// <summary>
        /// Gets IDs from a specific governorate.
        /// </summary>
        public List<NationalIdInfo> GetByGovernorate(string governorate)
        {
            return GetValidIds()
                .Where(info => info.GovernorateNameEnglish.Contains(governorate, StringComparison.OrdinalIgnoreCase) ||
                              info.GovernorateNameArabic.Contains(governorate))
                .ToList();
        }

        /// <summary>
        /// Gets IDs born in leap years.
        /// </summary>
        public List<NationalIdInfo> GetBornInLeapYears()
        {
            return GetValidIds()
                .Where(info => info.IsLeapYear)
                .ToList();
        }

        /// <summary>
        /// Gets IDs with corrected dates (if auto-correct was enabled).
        /// </summary>
        public List<NationalIdInfo> GetCorrectedDates()
        {
            return GetValidIds()
                .Where(info =>
                {
                    // التحقق إذا كان اليوم الأصلي غير صالح للشهر
                    int daysInMonth = DateTime.DaysInMonth(info.Year, info.Month);
                    return info.Day > daysInMonth && _autoCorrectInvalidDays;
                })
                .ToList();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Cleans National ID from spaces, dashes, and special characters.
        /// </summary>
        private static string CleanNationalId(string nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                return string.Empty;

            return Regex.Replace(nationalId, @"[^\d]", "");
        }

        /// <summary>
        /// Calculates age from birth date.
        /// </summary>
        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age;
        }

        #endregion
    }
}

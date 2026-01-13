using NumericValidation.EG.Models.Helper;
using NumericValidation.EG.Models.NationalNumber.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericValidation.EG.Models.NationalNumber
{
    /// <summary>
    /// Contains comprehensive information extracted from an Egyptian National ID.
    /// Supports Arabic and English output for all fields.
    /// </summary>
    public class NationalIdInfo
    {

        #region Basic Properties

        /// <summary>Indicates if the National ID is valid.</summary>
        public bool IsValid { get; set; }

        /// <summary>Error message if invalid.</summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>The original National ID string.</summary>
        public string NationalId { get; set; } = string.Empty;

        #endregion

        #region Date Properties - Numeric

        /// <summary>Birth year (numeric).</summary>
        public int Year { get; set; }

        /// <summary>Birth month (numeric 1-12).</summary>
        public int Month { get; set; }

        /// <summary>Birth day (numeric 1-31).</summary>
        public int Day { get; set; }

        /// <summary>Birth date as DateTime object.</summary>
        public DateTime? BirthDate => IsValid && Year > 0 && Month > 0 && Day > 0
            ? new DateTime(Year, Month, Day)
            : null;

        /// <summary>Century value (1800, 1900, or 2000).</summary>
        public int Century { get; set; }

        /// <summary>Indicates if the birth year is a leap year.</summary>
        public bool IsLeapYear => Year > 0 && DateTime.IsLeapYear(Year);

        #endregion

        #region Date Properties - Text (English)

        /// <summary>Birth year as text in English (e.g., "1995").</summary>
        public string YearText => Year > 0 ? Year.ToString() : string.Empty;

        /// <summary>Month name in English (e.g., "January").</summary>
        public string MonthNameEnglish => Month > 0
        ? ArabicConstants.GetMonthNameEnglish(Month)
        : string.Empty;

        /// <summary>Day as text in English (e.g., "15").</summary>
        public string DayText => Day > 0 ? Day.ToString() : string.Empty;

        /// <summary>Day of week in English (e.g., "Friday").</summary>
        public string DayOfWeekEnglish => BirthDate.HasValue
          ? ArabicConstants.GetDayNameEnglish(BirthDate.Value.DayOfWeek)
        : string.Empty;

        /// <summary>Full birth date in English (e.g., "Friday, January 15, 1995").</summary>
        public string BirthDateFullEnglish => BirthDate.HasValue
            ? $"{DayOfWeekEnglish}, {MonthNameEnglish} {Day}, {Year}"
            : string.Empty;

        /// <summary>Birth date in short English format (e.g., "01/15/1995").</summary>
        public string BirthDateShortEnglish => BirthDate.HasValue
            ? BirthDate.Value.ToString("MM/dd/yyyy")
            : string.Empty;

        /// <summary>Birth date in ISO format (e.g., "1995-01-15").</summary>
        public string BirthDateISO => BirthDate.HasValue
            ? BirthDate.Value.ToString("yyyy-MM-dd")
            : string.Empty;

        /// <summary>Century name in English (e.g., "Twentieth").</summary>
        public string CenturyNameEnglish => CalendarConstants.GetCenturyNameEnglish(Century);

        /// <summary>Leap year status in English (e.g., "Leap Year" or "Regular Year").</summary>
        public string LeapYearStatusEnglish => Year > 0
            ? (IsLeapYear ? "Leap Year" : "Regular Year")
            : string.Empty;

        /// <summary>Days in birth month (e.g., 31 for January).</summary>
        public int DaysInMonth => Year > 0 && Month > 0 && Month <= 12
            ? DateTime.DaysInMonth(Year, Month)
            : 0;

        /// <summary>Days in birth month as text (e.g., "31 days").</summary>
        public string DaysInMonthText => DaysInMonth > 0
            ? $"{DaysInMonth} days"
            : string.Empty;

        #endregion

        #region Date Properties - Text (Arabic)

        /// <summary>Birth year as text in Arabic (e.g., "١٩٩٥").</summary>
        public string YearTextArabic => Year > 0 ? (Year.ToArabicNumerals()) : string.Empty;

        /// <summary>Month name in Arabic (e.g., "يناير").</summary>
        public string MonthNameArabic => Month > 0
          ? ArabicConstants.GetMonthNameArabic(Month)
          : string.Empty;

        /// <summary>Day as text in Arabic (e.g., "١٥").</summary>
        public string DayTextArabic => Day > 0 ? Day.ToArabicNumerals() : string.Empty;

        /// <summary>Day of week in Arabic (e.g., "الجمعة").</summary>
        public string DayOfWeekArabic => BirthDate.HasValue
      ? ArabicConstants.GetDayNameArabic(BirthDate.Value.DayOfWeek)
      : string.Empty;

        /// <summary>Full birth date in Arabic (e.g., "الجمعة، 15 يناير 1995").</summary>
        public string BirthDateFullArabic => BirthDate.HasValue
            ? $"{DayOfWeekArabic}، {DayTextArabic} {MonthNameArabic} {YearTextArabic}"
            : string.Empty;

        /// <summary>Birth date in short Arabic format (e.g., "١٥/٠١/١٩٩٥").</summary>
        public string BirthDateShortArabic => BirthDate.HasValue
       ? BirthDate.Value.ToString("dd/MM/yyyy").ToArabicNumerals()
          : string.Empty;

        /// <summary>Century name in Arabic (e.g., "العشرون").</summary>
        public string CenturyNameArabic => CalendarConstants.GetCenturyNameArabic(Century);

        /// <summary>Leap year status in Arabic (e.g., "سنة كبيسة" or "سنة عادية").</summary>
        public string LeapYearStatusArabic => Year > 0
            ? (IsLeapYear ? "سنة كبيسة" : "سنة عادية")
            : string.Empty;

        /// <summary>Days in birth month in Arabic (e.g., "٣١ يوم").</summary>
        public string DaysInMonthArabic => DaysInMonth > 0
            ? $"{DaysInMonth.ToArabicNumerals()} يوم"
            : string.Empty;

        #endregion

        #region Age Properties - Numeric

        /// <summary>Current age in years (numeric).</summary>
        public int Age => BirthDate.HasValue ? CalculateAge(BirthDate.Value) : 0;

        /// <summary>Age in months (numeric).</summary>
        public int AgeInMonths => BirthDate.HasValue ? CalculateAgeInMonths(BirthDate.Value) : 0;

        /// <summary>Age in days (numeric).</summary>
        public int AgeInDays => BirthDate.HasValue ? (int)(DateTime.Today - BirthDate.Value).TotalDays : 0;

        /// <summary>Years component for detailed age.</summary>
        public int AgeYears => Age;

        /// <summary>Months component for detailed age (0-11).</summary>
        public int AgeMonthsComponent => BirthDate.HasValue ? CalculateAgeMonths(BirthDate.Value) : 0;

        /// <summary>Days component for detailed age (0-30).</summary>
        public int AgeDaysComponent => BirthDate.HasValue ? CalculateAgeDays(BirthDate.Value) : 0;

        #endregion

        #region Age Properties - Text (English)

        /// <summary>Age in English (e.g., "28 years old").</summary>
        public string AgeTextEnglish => Age > 0
            ? $"{Age} {(Age == 1 ? "year" : "years")} old"
            : string.Empty;

        /// <summary>Detailed age in English (e.g., "28 years, 3 months, 15 days").</summary>
        public string AgeDetailedEnglish
        {
            get
            {
                if (!BirthDate.HasValue) return string.Empty;

                var parts = new List<string>();

                if (AgeYears > 0)
                    parts.Add($"{AgeYears} {(AgeYears == 1 ? "year" : "years")}");

                if (AgeMonthsComponent > 0)
                    parts.Add($"{AgeMonthsComponent} {(AgeMonthsComponent == 1 ? "month" : "months")}");

                if (AgeDaysComponent > 0)
                    parts.Add($"{AgeDaysComponent} {(AgeDaysComponent == 1 ? "day" : "days")}");

                return parts.Any() ? string.Join(", ", parts) : "0 days";
            }
        }

        /// <summary>Age group in English (e.g., "Adult", "Child", "Senior").</summary>
        public string AgeGroupEnglish => Age >= 0
            ? AgeGroupConstants.GetAgeGroupEnglish(Age)
            : string.Empty;

        /// <summary>Generation in English (e.g., "Gen Z", "Millennial").</summary>
        public string GenerationEnglish => Year > 0
            ? GenerationConstants.GetGenerationEnglish(Year)
            : string.Empty;

        #endregion

        #region Age Properties - Text (Arabic)

        /// <summary>Age in Arabic (e.g., "٢٨ سنة").</summary>
        public string AgeTextArabic => Age > 0
            ? $"{(Age.ToArabicNumerals())} {(Age == 1 ? "سنة" : Age == 2 ? "سنتان" : "سنة")}"
            : string.Empty;

        /// <summary>Detailed age in Arabic (e.g., "٢٨ سنة، ٣ أشهر، ١٥ يوم").</summary>
        public string AgeDetailedArabic
        {
            get
            {
                if (!BirthDate.HasValue) return string.Empty;

                var parts = new List<string>();

                if (AgeYears > 0)
                {
                    string yearWord = AgeYears == 1 ? "سنة" : AgeYears == 2 ? "سنتان" : "سنة";
                    parts.Add($"{(AgeYears.ToArabicNumerals())} {yearWord}");
                }

                if (AgeMonthsComponent > 0)
                {
                    string monthWord = AgeMonthsComponent == 1 ? "شهر" : AgeMonthsComponent == 2 ? "شهران" : "أشهر";
                    parts.Add($"{(AgeMonthsComponent.ToArabicNumerals())} {monthWord}");
                }

                if (AgeDaysComponent > 0)
                {
                    string dayWord = AgeDaysComponent == 1 ? "يوم" : AgeDaysComponent == 2 ? "يومان" : "يوم";
                    parts.Add($"{(AgeDaysComponent.ToArabicNumerals())} {dayWord}");
                }

                return parts.Any() ? string.Join("، ", parts) : "٠ يوم";
            }
        }

        /// <summary>Age group in Arabic (e.g., "بالغ", "طفل", "مسن").</summary>
        public string AgeGroupArabic => Age >= 0
            ? AgeGroupConstants.GetAgeGroupArabic(Age)
            : string.Empty;

        /// <summary>Generation in Arabic (e.g., "جيل زد", "جيل الألفية").</summary>
        public string GenerationArabic => Year > 0
      ? GenerationConstants.GetGenerationArabic(Year)
      : string.Empty;

        #endregion

        #region Governorate Properties

        /// <summary>Governorate code (e.g., "001" for Cairo).</summary>
        public string GovernorateCode { get; set; } = string.Empty;

        /// <summary>Governorate name in Arabic (e.g., "القاهرة").</summary>
        public string GovernorateNameArabic { get; set; } = string.Empty;

        /// <summary>Governorate name in English (e.g., "Cairo").</summary>
        public string GovernorateNameEnglish { get; set; } = string.Empty;

        /// <summary>Governorate code in Arabic numerals (e.g., "٠٠١").</summary>
        public string GovernorateCodeArabic => !string.IsNullOrEmpty(GovernorateCode)
            ? NumberConversionHelper.ToArabicNumerals(GovernorateCode)
            : string.Empty;

        #endregion

        #region Gender Properties

        /// <summary>Gender in English ("Male" or "Female").</summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>Gender in Arabic ("ذكر" or "أنثى").</summary>
        public string GenderArabic { get; set; } = string.Empty;

        /// <summary>Gender symbol ("♂" or "♀").</summary>
        public string GenderSymbol => Gender == "Male" ? "♂" : Gender == "Female" ? "♀" : string.Empty;

        #endregion

        #region Serial Number Properties

        /// <summary>Serial number (positions 11-13 in ID).</summary>
        public string SerialNumber { get; set; } = string.Empty;

        /// <summary>Serial number in Arabic numerals.</summary>
        public string SerialNumberArabic => !string.IsNullOrEmpty(SerialNumber)
            ? NumberConversionHelper.ToArabicNumerals(SerialNumber)
            : string.Empty;

        #endregion

        #region Zodiac Properties

        /// <summary>Zodiac sign in English (e.g., "Capricorn").</summary>
        public string ZodiacSignEnglish => BirthDate.HasValue
              ? ZodiacConstants.GetZodiacSign(BirthDate.Value).English
              : string.Empty;

        /// <summary>Zodiac sign in Arabic (e.g., "الجدي").</summary>
        public string ZodiacSignArabic => BirthDate.HasValue
            ? ZodiacConstants.GetZodiacSign(BirthDate.Value).Arabic
            : string.Empty;

        /// <summary>Zodiac symbol (e.g., "♑").</summary>
        public string ZodiacSymbol => BirthDate.HasValue
               ? ZodiacConstants.GetZodiacSign(BirthDate.Value).Symbol
               : string.Empty;
        #endregion

        #region Summary Properties

        /// <summary>Complete summary in English.</summary>
        public string SummaryEnglish => IsValid
            ? $"{Gender}, born on {BirthDateFullEnglish} in {GovernorateNameEnglish}, {AgeTextEnglish}, {ZodiacSignEnglish}"
            : ErrorMessage;

        /// <summary>Complete summary in Arabic.</summary>
        public string SummaryArabic => IsValid
            ? $"{GenderArabic}، مواليد {BirthDateFullArabic} في {GovernorateNameArabic}، {AgeTextArabic}، برج {ZodiacSignArabic}"
            : ErrorMessage;

        #endregion

        #region Helper Methods

        /// <summary>Calculates current age from birth date.</summary>
        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
                age--;
            return age;
        }

        /// <summary>Calculates age in total months.</summary>
        private static int CalculateAgeInMonths(DateTime birthDate)
        {
            var today = DateTime.Today;
            int months = (today.Year - birthDate.Year) * 12 + today.Month - birthDate.Month;
            if (today.Day < birthDate.Day)
                months--;
            return months;
        }

        /// <summary>Calculates months component of age.</summary>
        private static int CalculateAgeMonths(DateTime birthDate)
        {
            var today = DateTime.Today;
            int months = today.Month - birthDate.Month;
            if (today.Day < birthDate.Day)
                months--;
            if (months < 0)
                months += 12;
            return months;
        }

        /// <summary>Calculates days component of age.</summary>
        private static int CalculateAgeDays(DateTime birthDate)
        {
            var today = DateTime.Today;
            var lastBirthdayMonth = new DateTime(today.Year, birthDate.Month, 1);
            if (today.Month < birthDate.Month || today.Month == birthDate.Month && today.Day < birthDate.Day)
                lastBirthdayMonth = lastBirthdayMonth.AddYears(-1);

            var effectiveBirthDay = Math.Min(birthDate.Day, DateTime.DaysInMonth(lastBirthdayMonth.Year, lastBirthdayMonth.Month));
            var lastBirthday = new DateTime(lastBirthdayMonth.Year, lastBirthdayMonth.Month, effectiveBirthDay);

            return (today - lastBirthday).Days;
        }

        #endregion

        #region ToString Override

        /// <summary>Returns a comprehensive string representation of the National ID information in both Arabic and English.</summary>
        public override string ToString()
        {
            if (!IsValid)
                return $"❌ Invalid National ID\n   Error: {ErrorMessage}\n   خطأ: {ErrorMessage}";

            var sb = new StringBuilder();

            sb.AppendLine("═══════════════════════════════════════════════════════════════");
            sb.AppendLine("                    EGYPTIAN NATIONAL ID INFO                   ");
            sb.AppendLine("                  معلومات الرقم القومي المصري                  ");
            sb.AppendLine("═══════════════════════════════════════════════════════════════");
            sb.AppendLine();

            // National ID
            sb.AppendLine($"🆔 National ID / الرقم القومي:");
            sb.AppendLine($"   {NationalId}");
            sb.AppendLine();

            // Personal Info
            sb.AppendLine($"👤 Personal Information / المعلومات الشخصية:");
            sb.AppendLine($"   Gender / النوع: {Gender} ({GenderSymbol}) / {GenderArabic}");
            sb.AppendLine();

            // Birth Date
            sb.AppendLine($"📅 Birth Date / تاريخ الميلاد:");
            sb.AppendLine($"   English: {BirthDateFullEnglish}");
            sb.AppendLine($"   العربية: {BirthDateFullArabic}");
            sb.AppendLine($"   Short / مختصر: {BirthDateShortEnglish} / {BirthDateShortArabic}");
            sb.AppendLine($"   ISO Format: {BirthDateISO}");
            sb.AppendLine($"   Day of Week / يوم الأسبوع: {DayOfWeekEnglish} / {DayOfWeekArabic}");
            sb.AppendLine($"   Leap Year Status / حالة السنة: {LeapYearStatusEnglish} / {LeapYearStatusArabic}");
            sb.AppendLine($"   Days in Month / أيام الشهر: {DaysInMonthText} / {DaysInMonthArabic}");
            sb.AppendLine();

            // Age
            sb.AppendLine($"🎂 Age / العمر:");
            sb.AppendLine($"   Current Age / العمر الحالي: {AgeTextEnglish} / {AgeTextArabic}");
            sb.AppendLine($"   Detailed / تفصيلي: {AgeDetailedEnglish}");
            sb.AppendLine($"                {AgeDetailedArabic}");
            sb.AppendLine($"   In Days / بالأيام: {AgeInDays:N0} days / {AgeInDays.ToArabicNumerals("N0")} يوم");
            sb.AppendLine($"   In Months / بالشهور: {AgeInMonths:N0} months / {(AgeInMonths.ToArabicNumerals("N0"))} شهر");
            sb.AppendLine($"   Age Group / الفئة العمرية: {AgeGroupEnglish} / {AgeGroupArabic}");
            sb.AppendLine($"   Generation / الجيل: {GenerationEnglish} / {GenerationArabic}");
            sb.AppendLine();

            // Location
            sb.AppendLine($"🌍 Location / الموقع:");
            sb.AppendLine($"   Governorate / المحافظة: {GovernorateNameEnglish} / {GovernorateNameArabic}");
            sb.AppendLine($"   Code / الكود: {GovernorateCode} / {GovernorateCodeArabic}");
            sb.AppendLine();

            // Additional Info
            sb.AppendLine($"✨ Additional Information / معلومات إضافية:");
            sb.AppendLine($"   Century / القرن: {CenturyNameEnglish} / {CenturyNameArabic}");
            sb.AppendLine($"   Zodiac Sign / البرج: {ZodiacSignEnglish} ({ZodiacSymbol}) / {ZodiacSignArabic}");
            sb.AppendLine($"   Serial Number / الرقم التسلسلي: {SerialNumber} / {SerialNumberArabic}");
            sb.AppendLine();

            // Summary
            sb.AppendLine($"📋 Summary / الملخص:");
            sb.AppendLine($"   EN: {SummaryEnglish}");
            sb.AppendLine($"   AR: {SummaryArabic}");
            sb.AppendLine();

            sb.AppendLine("═══════════════════════════════════════════════════════════════");

            return sb.ToString();
        }

        /// <summary>Returns a compact string representation in English only.</summary>
        public string ToStringEnglish()
        {
            return IsValid ? SummaryEnglish : $"Invalid: {ErrorMessage}";
        }

        /// <summary>Returns a compact string representation in Arabic only.</summary>
        public string ToStringArabic()
        {
            return IsValid ? SummaryArabic : $"غير صالح: {ErrorMessage}";
        }

        /// <summary>Returns detailed information in English only.</summary>
        public string ToStringDetailedEnglish()
        {
            if (!IsValid)
                return $"Invalid National ID: {ErrorMessage}";

            var sb = new StringBuilder();
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine("    EGYPTIAN NATIONAL ID DETAILS");
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine($"ID: {NationalId}");
            sb.AppendLine($"Gender: {Gender} {GenderSymbol}");
            sb.AppendLine($"Birth Date: {BirthDateFullEnglish}");
            sb.AppendLine($"Leap Year: {LeapYearStatusEnglish}");
            sb.AppendLine($"Age: {AgeDetailedEnglish}");
            sb.AppendLine($"Age Group: {AgeGroupEnglish}");
            sb.AppendLine($"Generation: {GenerationEnglish}");
            sb.AppendLine($"Governorate: {GovernorateNameEnglish}");
            sb.AppendLine($"Zodiac: {ZodiacSignEnglish} {ZodiacSymbol}");
            sb.AppendLine($"Serial: {SerialNumber}");
            sb.AppendLine("═══════════════════════════════════════");
            return sb.ToString();
        }

        /// <summary>Returns detailed information in Arabic only.</summary>
        public string ToStringDetailedArabic()
        {
            if (!IsValid)
                return $"رقم قومي غير صالح: {ErrorMessage}";

            var sb = new StringBuilder();
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine("       تفاصيل الرقم القومي المصري");
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine($"الرقم القومي: {NationalId}");
            sb.AppendLine($"النوع: {GenderArabic} {GenderSymbol}");
            sb.AppendLine($"تاريخ الميلاد: {BirthDateFullArabic}");
            sb.AppendLine($"سنة كبيسة: {LeapYearStatusArabic}");
            sb.AppendLine($"العمر: {AgeDetailedArabic}");
            sb.AppendLine($"الفئة العمرية: {AgeGroupArabic}");
            sb.AppendLine($"الجيل: {GenerationArabic}");
            sb.AppendLine($"المحافظة: {GovernorateNameArabic}");
            sb.AppendLine($"البرج: {ZodiacSignArabic} {ZodiacSymbol}");
            sb.AppendLine($"الرقم التسلسلي: {SerialNumberArabic}");
            sb.AppendLine("═══════════════════════════════════════");
            return sb.ToString();
        }

        #endregion
    }
}

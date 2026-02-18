using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericValidation.EG.Models.PhoneNumber.Info
{
    /// <summary>
    /// Contains comprehensive information about a phone number validation result.
    /// This class represents the parsed and validated data for Egyptian phone numbers.
    /// </summary>
    /// <remarks>
    /// The PhoneNumberInfo class is the primary data structure returned by phone number
    /// validation operations. It contains both the original input and parsed components
    /// with validation status and carrier information.
    /// </remarks>
    /// <example>
    /// <code>
    /// var validator = new PhoneNumberValidator();
    /// var result = validator.Parse("01234567890");
    /// 
    /// if (result.IsValid)
    /// {
    ///     Console.WriteLine($"Number: {result.Number}");
    ///     Console.WriteLine($"Carrier: {result.Carrier}");
    ///     Console.WriteLine($"Service Type: {result.ServiceType}");
    /// }
    /// else
    /// {
    ///     Console.WriteLine($"Error: {result.ErrorMessage}");
    /// }
    /// </code>
    /// </example>
    public class PhoneNumberInfo
    {
        /// <summary>
        /// Gets or sets the original input phone number as provided by the user.
        /// </summary>
        /// <value>
        /// The raw phone number string exactly as entered, including any formatting,
        /// spaces, or special characters.
        /// </value>
        /// <remarks>
        /// This property preserves the exact user input for audit trails and
        /// debugging purposes. It is not modified during the validation process.
        /// </remarks>
        public string OriginalNumber { get; set; } = string.Empty; // الرقم المدخل الأصلي

        /// <summary>
        /// Gets or sets the phone number after cleaning and normalization.
        /// </summary>
        /// <value>
        /// The phone number with all non-digit characters removed, but before
        /// any country code or prefix transformations.
        /// </value>
        /// <remarks>
        /// This intermediate representation is used internally during validation
        /// and contains only numeric digits.
        /// </remarks>
        public string CleanedNumber { get; set; } = string.Empty; // The number after cleaning

        /// <summary>
        /// Gets or sets the final formatted phone number in local format.
        /// </summary>
        /// <value>
        /// The complete phone number in standard Egyptian format (e.g., "01234567890").
        /// This is the canonical representation used for storage and display.
        /// </value>
        /// <remarks>
        /// The number includes the carrier prefix and subscriber number in a
        /// consistent format without international dialing codes.
        /// </remarks>
        public string Number { get; set; } = string.Empty; // Final number (local format)

        /// <summary>
        /// Gets or sets the carrier prefix (first 4 digits of the phone number).
        /// </summary>
        /// <value>
        /// A 4-digit string representing the carrier-specific prefix that identifies
        /// the mobile network operator (e.g., "0123" for Vodafone Egypt).
        /// </value>
        /// <remarks>
        /// The prefix determines both the carrier and service type. Each prefix
        /// is assigned to specific mobile operators by the National Telecom Regulatory Authority (NTRA).
        /// </remarks>
        public string Prefix { get; set; } = string.Empty; //Prefix (4 digits)

        /// <summary>
        /// Gets or sets the mobile network carrier name.
        /// </summary>
        /// <value>
        /// The name of the telecom operator providing service for this number
        /// (e.g., "Vodafone Egypt", "Orange Egypt", "Etisalat Misr", "We").
        /// </value>
        /// <remarks>
        /// Carrier information is derived from the prefix using NTRA's official
        /// numbering plan for Egypt.
        /// </remarks>
        public string Carrier { get; set; } = string.Empty; //Company

        /// <summary>
        /// Gets or sets the type of telecommunication service.
        /// </summary>
        /// <value>
        /// A string describing the service category: "Mobile" for regular mobile
        /// numbers or "Value Added Service" for special service numbers.
        /// </value>
        /// <remarks>
        /// Service type classification helps differentiate between standard
        /// mobile services and premium/specialized services with different
        /// billing rates and regulations.
        /// </remarks>
        public string ServiceType { get; set; } = string.Empty; // Service type (mobile/value added)

        /// <summary>
        /// Gets or sets a value indicating whether the phone number is valid.
        /// </summary>
        /// <value>
        /// <c>true</c> if the phone number passes all validation checks including
        /// format, length, and prefix verification; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property represents the overall validation status. When false,
        /// the <see cref="ErrorMessage"/> property contains details about the
        /// validation failure.
        /// </remarks>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the error message describing validation failure.
        /// </summary>
        /// <value>
        /// A descriptive error message in Arabic and English explaining why
        /// the phone number failed validation. Empty string when validation succeeds.
        /// </value>
        /// <remarks>
        /// Error messages are designed to be user-friendly and help identify
        /// specific issues with the input phone number.
        /// </remarks>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Returns a string representation of the phone number validation result.
        /// </summary>
        /// <returns>
        /// A formatted string containing key validation information including
        /// the phone number, validation status, carrier, and any error message.
        /// </returns>
        /// <remarks>
        /// This method is primarily used for debugging and logging purposes.
        /// The format is: "Number: {Number}, IsValid: {IsValid}, Carrier: {Carrier}, ErrorMessage: {ErrorMessage}"
        /// </remarks>
        /// <example>
        /// <code>
        /// var info = new PhoneNumberInfo 
        /// { 
        ///     Number = "01234567890", 
        ///     IsValid = true, 
        ///     Carrier = "Vodafone Egypt",
        ///     ErrorMessage = ""
        /// };
        /// Console.WriteLine(info.ToString());
        /// // Output: "Number: 01234567890, IsValid: True, Carrier: Vodafone Egypt, ErrorMessage: "
        /// </code>
        /// </example>
        public override string ToString()
        {
            return $"Number: {Number}, IsValid: {IsValid}, Carrier: {Carrier}, ErrorMessage: {ErrorMessage}";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Two PhoneNumberInfo objects are considered equal if their <see cref="Number"/>
        /// properties are equal, regardless of other properties.
        /// </remarks>
        public override bool Equals(object obj)
        {
            return obj is PhoneNumberInfo info &&
                   Number == info.Number;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        /// <remarks>
        /// The hash code is based on the <see cref="Number"/> property.
        /// </remarks>
        public override int GetHashCode()
        {
            return HashCode.Combine(Number);
        }

        /// <summary>
        /// Creates a deep copy of the current PhoneNumberInfo object.
        /// </summary>
        /// <returns>A new PhoneNumberInfo object with the same property values.</returns>
        /// <remarks>
        /// This method creates a completely independent copy that can be
        /// modified without affecting the original object.
        /// </remarks>
        public PhoneNumberInfo Clone()
        {
            return new PhoneNumberInfo
            {
                OriginalNumber = OriginalNumber,
                CleanedNumber = CleanedNumber,
                Number = Number,
                Prefix = Prefix,
                Carrier = Carrier,
                ServiceType = ServiceType,
                IsValid = IsValid,
                ErrorMessage = ErrorMessage
            };
        }

        /// <summary>
        /// Formats the phone number for international dialing.
        /// </summary>
        /// <returns>The phone number in international format with Egypt country code (+20).</returns>
        /// <remarks>
        /// This method assumes the number is already in valid Egyptian format.
        /// It prepends the international access code and Egypt country code.
        /// </remarks>
        /// <example>
        /// <code>
        /// var info = new PhoneNumberInfo { Number = "01234567890" };
        /// string international = info.ToInternationalFormat();
        /// // Result: "+201234567890"
        /// </code>
        /// </example>
        public string ToInternationalFormat()
        {
            if (string.IsNullOrEmpty(Number))
                return string.Empty;

            // Remove leading zero if present and add Egypt country code
            string nationalNumber = Number.StartsWith("0") ? Number.Substring(1) : Number;
            return $"+20{nationalNumber}";
        }

        /// <summary>
        /// Formats the phone number for display with standard spacing.
        /// </summary>
        /// <returns>The phone number formatted with spaces for readability.</returns>
        /// <remarks>
        /// The standard Egyptian format is: 0XXX XXX XXXX (e.g., "0123 456 7890")
        /// </remarks>
        public string ToDisplayFormat()
        {
            if (string.IsNullOrEmpty(Number) || Number.Length != 11)
                return Number;

            return $"{Number.Substring(0, 4)} {Number.Substring(4, 3)} {Number.Substring(7, 4)}";
        }

        /// <summary>
        /// Validates that all required properties are populated for a valid phone number.
        /// </summary>
        /// <returns><c>true</c> if the object represents a complete valid phone number; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method performs additional consistency checks beyond the basic IsValid flag,
        /// ensuring that all derived properties are correctly populated.
        /// </remarks>
        public bool IsComplete()
        {
            return IsValid &&
                   !string.IsNullOrEmpty(Number) &&
                   Number.Length == 11 &&
                   !string.IsNullOrEmpty(Prefix) &&
                   Prefix.Length == 4 &&
                   !string.IsNullOrEmpty(Carrier) &&
                   !string.IsNullOrEmpty(ServiceType) &&
                   string.IsNullOrEmpty(ErrorMessage);
        }
    }
}
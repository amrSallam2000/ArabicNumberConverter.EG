using System;

namespace NumericValidation.EG.Models.PhoneNumber
{
    /// <summary>
    /// Supported phone number formats and validation options
    /// </summary>
    [Flags]
    public enum ValidationOptions
    {
        /// <summary>
        /// No special options
        /// </summary>
        None = 0,

        /// <summary>
        /// Accept numbers with international format (+20)
        /// </summary>
        AcceptInternationalFormats = 1,

        /// <summary>
        /// Accept numbers without leading zero (10XXXXXXXXX)
        /// </summary>
        AcceptNumbersWithoutLeadingZero = 2,

        /// <summary>
        /// Accept formatted numbers with spaces, dashes, etc.
        /// </summary>
        AcceptFormattedNumbers = 4,

        /// <summary>
        /// Accept Arabic/Indic numerals (٠١٢٣٤٥٦٧٨٩)
        /// </summary>
        AcceptArabicIndicNumerals = 8,

        /// <summary>
        /// Validate special service numbers (019XXXXXXX)
        /// </summary>
        ValidateSpecialServices = 16,

        /// <summary>
        /// Auto-fix incomplete numbers by adding missing digits
        /// </summary>
        AutoFixIncompleteNumbers = 32,

        /// <summary>
        /// Strict carrier validation - only accept prefixes in dictionary
        /// </summary>
        StrictCarrierValidation = 64,

        /// <summary>
        /// Default validation options (balanced between strictness and flexibility)
        /// </summary>
        Default = AcceptInternationalFormats | AcceptFormattedNumbers | AcceptArabicIndicNumerals | ValidateSpecialServices
    }
}
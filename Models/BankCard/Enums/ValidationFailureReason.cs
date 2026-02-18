using NumericValidation.EG.Models.BankCard.Info;
using NumericValidation.EG.Models.BankCard.Logger;

namespace NumericValidation.EG.Models.BankCard.Enums
{

    /// <summary>
    /// Reason why a card number or associated data failed validation.
    /// Returned on <see cref="BankCardInfo.FailureReason"/> when <see cref="BankCardInfo.IsValid"/> is <c>false</c>.
    /// </summary>
    public enum ValidationFailureReason
    {
        /// <summary>No failure — validation passed.</summary>
        None = 0,

        /// <summary>Input was null, empty, or contained only whitespace.</summary>
        NullOrEmpty = 1,

        /// <summary>Input contained non-digit characters after stripping spaces and dashes.</summary>
        ContainsNonDigits = 2,

        /// <summary>PAN length is not valid for the detected card network.</summary>
        InvalidLength = 3,

        /// <summary>Luhn (Modulus-10) check digit verification failed.</summary>
        LuhnCheckFailed = 4,

        /// <summary>IIN/BIN prefix is not recognised in any known network range.</summary>
        UnknownIIN = 5,

        /// <summary>Expiry date string could not be parsed (MM/YY or MM/YYYY expected).</summary>
        InvalidExpiryDate = 6,

        /// <summary>Card expiry date is in the past.</summary>
        CardExpired = 7,

        /// <summary>CVV/CVC/CID value does not meet length or digit requirements.</summary>
        InvalidCVV = 8,

        /// <summary>Cardholder name contains invalid characters or is out of allowed length.</summary>
        InvalidCardholderName = 9,

        /// <summary>
        /// An unexpected internal exception was thrown during analysis.
        /// See <see cref="IBankCardLogger.LogError"/> for exception details.
        /// </summary>
        InternalError = 10,
    }
}
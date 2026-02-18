using NumericValidation.EG.Models.BankCard.Enums;
using NumericValidation.EG.Models.BankCard.Validator;
using System;

namespace NumericValidation.EG.Models.BankCard.Logger
{
    // ═══════════════════════════════════════════════════════════════════════════
    //  ArabicNumberConverter.EG.BankCard — IBankCardLogger
    //  Optional logging seam — implement to integrate with ILogger, Serilog, etc.
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Optional logging abstraction for <see cref="BankCardValidator"/>.
    /// Implement this interface and pass an instance to the validator constructor
    /// to receive structured audit events for every card analysis.
    /// <para>
    /// This design keeps the library free of any hard dependency on
    /// <c>Microsoft.Extensions.Logging</c>, Serilog, NLog, or any other framework.
    /// You can bridge to any logging backend in your own implementation.
    /// </para>
    /// <para>
    /// واجهة التسجيل الاختيارية — نفِّذها وأرسلها للـ BankCardValidator
    /// لتلقّي أحداث التدقيق لكل عملية تحليل بطاقة.
    /// </para>
    /// </summary>
    public interface IBankCardLogger
    {
        /// <summary>
        /// Called after every successful or failed card analysis.
        /// </summary>
        /// <param name="maskedPan">
        /// The masked PAN (e.g. "4111 •••• •••• 1111") — never the full PAN.
        /// </param>
        /// <param name="isValid">Whether the card passed all enabled validations.</param>
        /// <param name="networkName">Detected card network name (English), or "Unknown".</param>
        /// <param name="failureReason">
        /// The reason for failure, or <see cref="ValidationFailureReason.None"/> when valid.
        /// </param>
        void LogValidation(
            string maskedPan,
            bool isValid,
            string networkName,
            ValidationFailureReason failureReason);

        /// <summary>
        /// Called when an unexpected exception is caught inside the validator.
        /// </summary>
        /// <param name="ex">The exception that was caught.</param>
        /// <param name="maskedPan">
        /// The masked PAN at the time of the error, or an empty string if masking itself failed.
        /// </param>
        void LogError(Exception ex, string maskedPan);
    }

    /// <summary>
    /// A no-op <see cref="IBankCardLogger"/> implementation that silently discards all events.
    /// This is the default used by <see cref="BankCardValidator"/> when no logger is supplied.
    /// </summary>
    public sealed class NullBankCardLogger : IBankCardLogger
    {
        /// <summary>Singleton instance — avoids unnecessary allocations.</summary>
        public static readonly NullBankCardLogger Instance = new();

        private NullBankCardLogger() { }

        /// <inheritdoc/>
        public void LogValidation(string maskedPan, bool isValid, string networkName, ValidationFailureReason failureReason) { }

        /// <inheritdoc/>
        public void LogError(Exception ex, string maskedPan) { }
    }

    /// <summary>
    /// A simple <see cref="IBankCardLogger"/> that writes to <see cref="Console"/>.
    /// Suitable for development and integration testing; <b>not recommended for production</b>.
    /// </summary>
    public sealed class ConsoleBankCardLogger : IBankCardLogger
    {
        /// <inheritdoc/>
        public void LogValidation(string maskedPan, bool isValid, string networkName, ValidationFailureReason failureReason)
        {
            string status = isValid ? "VALID  ✅" : $"INVALID ❌ ({failureReason})";
            Console.WriteLine($"[BankCard] {status} | Network: {networkName,-20} | PAN: {maskedPan}");
        }

        /// <inheritdoc/>
        public void LogError(Exception ex, string maskedPan)
        {
            Console.Error.WriteLine($"[BankCard] ERROR | PAN: {maskedPan} | {ex.GetType().Name}: {ex.Message}");
        }
    }
}

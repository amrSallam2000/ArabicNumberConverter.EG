using System;
using System.Collections.Generic;
using System.Linq;

namespace NumericValidation.EG.Models.BankCard.Algorithm
{
    // ═══════════════════════════════════════════════════════════════════════════
    //  ArabicNumberConverter.EG.BankCard — LuhnAlgorithm
    //  Implements ISO/IEC 7812-1:2017 §6 — Modulus-10 check digit algorithm
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Static implementation of the <b>Luhn algorithm</b> (Modulus-10),
    /// as mandated by ISO/IEC 7812-1:2017 §6 for PAN check-digit computation.
    /// <para>
    /// The algorithm detects single-digit transcription errors and most
    /// digit-transposition errors. It is <b>not</b> a cryptographic security
    /// mechanism — it guards against accidental mis-entry, not intentional fraud.
    /// </para>
    /// <para>
    /// خوارزمية Luhn (Modulus-10) — المُعتمَدة وفق ISO/IEC 7812-1:2017 للتحقق من
    /// رقم التحقق (Check Digit) في أرقام البطاقات البنكية.
    /// </para>
    /// </summary>
    public static class LuhnAlgorithm
    {
        // ─── Public API ──────────────────────────────────────────────────────────

        /// <summary>
        /// Validates a complete PAN (including its check digit) against the Luhn algorithm.
        /// </summary>
        /// <param name="cardNumber">
        /// Full card number string. Spaces and dashes are stripped automatically.
        /// Leading / trailing whitespace is ignored.
        /// </param>
        /// <returns>
        /// <c>true</c> if the number passes the Luhn check; <c>false</c> otherwise.
        /// Returns <c>false</c> for null, empty, or non-numeric input.
        /// </returns>
        /// <example>
        /// <code>
        /// bool ok = LuhnAlgorithm.IsValid("4111 1111 1111 1111"); // true
        /// bool bad = LuhnAlgorithm.IsValid("4111111111111112");   // false
        /// </code>
        /// </example>
        public static bool IsValid(string? cardNumber)
        {
            // Null-safe sanitisation
            string sanitized = (cardNumber ?? string.Empty)
                .Replace(" ", "")
                .Replace("-", "")
                .Trim();

            if (sanitized.Length == 0)
                return false;

            if (!sanitized.All(char.IsDigit))
                return false;

            return CalculateChecksum(sanitized) == 0;
        }

        /// <summary>
        /// Computes the Luhn checksum of a digit string (including the check digit).
        /// A valid PAN returns 0.
        /// </summary>
        /// <param name="digits">Digit-only string (no spaces or dashes).</param>
        /// <returns>Checksum value (0–9). A result of 0 indicates a valid PAN.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="digits"/> is null, empty, or contains non-digit characters.</exception>
        public static int CalculateChecksum(string digits)
        {
            if (string.IsNullOrEmpty(digits))
                throw new ArgumentException("Digits string cannot be null or empty.", nameof(digits));

            if (!digits.All(char.IsDigit))
                throw new ArgumentException("Input must contain digits only.", nameof(digits));

            int sum = 0;
            bool doubleIt = false;

            // Traverse right-to-left (the check digit itself is NOT doubled)
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                int digit = digits[i] - '0';

                if (doubleIt)
                {
                    digit *= 2;
                    if (digit > 9) digit -= 9; // equivalent to summing the two resulting digits
                }

                sum += digit;
                doubleIt = !doubleIt;
            }

            return sum % 10;
        }

        /// <summary>
        /// Computes the Luhn check digit for a partial PAN (all digits except the last one).
        /// Append the returned value to <paramref name="partialNumber"/> to produce a Luhn-valid PAN.
        /// </summary>
        /// <param name="partialNumber">
        /// The PAN without its final check digit — digits only (spaces/dashes stripped automatically).
        /// </param>
        /// <returns>Check digit in the range 0–9.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="partialNumber"/> is null, empty, or contains non-digit characters.
        /// </exception>
        /// <example>
        /// <code>
        /// int cd = LuhnAlgorithm.CalculateCheckDigit("411111111111111"); // returns 1
        /// </code>
        /// </example>
        public static int CalculateCheckDigit(string partialNumber)
        {
            // Null-safe sanitisation
            string sanitized = (partialNumber ?? string.Empty)
                .Replace(" ", "")
                .Replace("-", "")
                .Trim();

            if (sanitized.Length == 0)
                throw new ArgumentException("Partial number cannot be null or empty.", nameof(partialNumber));

            if (!sanitized.All(char.IsDigit))
                throw new ArgumentException("Partial number must contain digits only.", nameof(partialNumber));

            // Append 0, compute checksum, then find the complement to 10
            string withZero = sanitized + "0";
            int checksum = CalculateChecksum(withZero);
            return checksum == 0 ? 0 : 10 - checksum;
        }

        /// <summary>
        /// Generates a Luhn-valid test card number for a given IIN/BIN prefix.
        /// <para>
        /// <b>⚠ For testing and development only.</b>
        /// The generated number is mathematically valid but does not correspond to any real account.
        /// </para>
        /// </summary>
        /// <param name="iin">
        /// IIN / BIN prefix (6 or 8 digits). Must be digit-only.
        /// Example: "507803" (Meeza NBE), "411111" (Visa test prefix).
        /// </param>
        /// <param name="totalLength">
        /// Desired total PAN length including the check digit (default: 16).
        /// Must be greater than the length of <paramref name="iin"/> + 1.
        /// </param>
        /// <returns>A Luhn-valid PAN string of length <paramref name="totalLength"/>.</returns>
        /// <exception cref="ArgumentException">Thrown for invalid <paramref name="iin"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="totalLength"/> is too short.</exception>
        public static string GenerateTestCardNumber(string iin, int totalLength = 16)
        {
            string sanitizedIin = (iin ?? string.Empty).Trim();

            if (sanitizedIin.Length == 0 || !sanitizedIin.All(char.IsDigit))
                throw new ArgumentException("IIN must be a non-empty digit-only string.", nameof(iin));

            if (totalLength <= sanitizedIin.Length + 1)
                throw new ArgumentOutOfRangeException(nameof(totalLength),
                    $"Total length ({totalLength}) must exceed IIN length ({sanitizedIin.Length}) by at least 2.");

            var rng = new Random();
            int fillCount = totalLength - sanitizedIin.Length - 1;
            string fill = string.Concat(Enumerable.Range(0, fillCount).Select(_ => rng.Next(0, 10).ToString()));
            string partial = sanitizedIin + fill;
            int checkDigit = CalculateCheckDigit(partial);
            return partial + checkDigit;
        }

        /// <summary>
        /// Returns a detailed, step-by-step breakdown of the Luhn computation for educational
        /// or debugging purposes.
        /// </summary>
        /// <param name="cardNumber">
        /// Full PAN to inspect. Spaces and dashes are stripped automatically.
        /// </param>
        /// <returns>
        /// A <see cref="LuhnStepResult"/> containing every processing step,
        /// the total sum, the check digit, and the overall validity.
        /// </returns>
        public static LuhnStepResult GetDetailedSteps(string? cardNumber)
        {
            string sanitized = (cardNumber ?? string.Empty)
                .Replace(" ", "")
                .Replace("-", "")
                .Trim();

            var steps = new List<LuhnStep>();
            int sum = 0;
            bool doubleIt = false;

            for (int i = sanitized.Length - 1; i >= 0; i--)
            {
                int original = sanitized[i] - '0';
                int processed = original;

                if (doubleIt)
                {
                    processed *= 2;
                    if (processed > 9) processed -= 9;
                }

                sum += processed;

                steps.Insert(0, new LuhnStep
                {
                    Position = i,
                    OriginalDigit = original,
                    Doubled = doubleIt,
                    ProcessedValue = processed,
                    RunningSum = sum
                });

                doubleIt = !doubleIt;
            }

            return new LuhnStepResult
            {
                CardNumber = sanitized,
                TotalSum = sum,
                IsValid = sanitized.Length > 0 && sum % 10 == 0,
                CheckDigit = sanitized.Length > 0 ? sanitized[^1] - '0' : -1,
                Steps = steps
            };
        }
    }

    // ─── Supporting types ──────────────────────────────────────────────────────

    /// <summary>
    /// Represents the processing of a single digit during the Luhn algorithm traversal.
    /// </summary>
    public class LuhnStep
    {
        /// <summary>Zero-based position of this digit within the PAN string (left-to-right).</summary>
        public int Position { get; set; }

        /// <summary>Original digit value before any Luhn processing (0–9).</summary>
        public int OriginalDigit { get; set; }

        /// <summary><c>true</c> if this digit was doubled as part of the Luhn algorithm.</summary>
        public bool Doubled { get; set; }

        /// <summary>
        /// Processed digit value after optional doubling and carry subtraction
        /// (i.e. if doubled result &gt; 9, subtract 9).
        /// </summary>
        public int ProcessedValue { get; set; }

        /// <summary>Cumulative sum of all processed digits from the rightmost position up to and including this step.</summary>
        public int RunningSum { get; set; }
    }

    /// <summary>
    /// Encapsulates the complete, step-by-step result of a Luhn algorithm computation.
    /// Returned by <see cref="LuhnAlgorithm.GetDetailedSteps"/>.
    /// </summary>
    public class LuhnStepResult
    {
        /// <summary>The sanitised PAN that was analysed.</summary>
        public string CardNumber { get; set; } = string.Empty;

        /// <summary>Final sum of all processed digits.</summary>
        public int TotalSum { get; set; }

        /// <summary><c>true</c> if <see cref="TotalSum"/> mod 10 equals 0 (Luhn-valid PAN).</summary>
        public bool IsValid { get; set; }

        /// <summary>The check digit (last digit of the PAN). −1 if input was empty.</summary>
        public int CheckDigit { get; set; }

        /// <summary>Ordered list of per-digit processing steps (left-to-right order).</summary>
        public List<LuhnStep> Steps { get; set; } = new();
    }
}

namespace NumericValidation.EG.Models.BankCard.Enums
{
    /// <summary>
    /// Functional type of the payment card, indicating how funds are accessed.
    /// </summary>
    public enum CardType
    {
        /// <summary>Type could not be determined.</summary>
        Unknown = 0,

        /// <summary>Credit card — spend now, pay later (post-paid).</summary>
        Credit = 1,

        /// <summary>Debit card — funds deducted directly from a bank account.</summary>
        Debit = 2,

        /// <summary>Prepaid card — pre-loaded with a fixed balance.</summary>
        Prepaid = 3,

        /// <summary>Virtual card — exists only digitally; no physical plastic.</summary>
        Virtual = 4,

        /// <summary>Corporate / commercial card — issued to a business entity.</summary>
        Corporate = 5,

        /// <summary>Government card — issued for government disbursements or payroll.</summary>
        Government = 6,
    }
}

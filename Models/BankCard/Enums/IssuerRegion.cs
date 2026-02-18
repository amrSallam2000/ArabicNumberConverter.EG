namespace NumericValidation.EG.Models.BankCard.Enums
{
    /// <summary>
    /// Geographic region of the card issuer.
    /// </summary>
    public enum IssuerRegion
    {
        /// <summary>Region could not be determined.</summary>
        Unknown = 0,

        /// <summary>Egypt — cards regulated by the Central Bank of Egypt (CBE).</summary>
        Egypt = 1,

        /// <summary>Middle East (excluding Egypt).</summary>
        MiddleEast = 2,

        /// <summary>Europe.</summary>
        Europe = 3,

        /// <summary>North America (USA, Canada, Mexico).</summary>
        NorthAmerica = 4,

        /// <summary>Asia-Pacific.</summary>
        AsiaPacific = 5,

        /// <summary>Latin America and Caribbean.</summary>
        LatinAmerica = 6,

        /// <summary>Africa (excluding Egypt).</summary>
        Africa = 7,

        /// <summary>Global / not region-specific (e.g., network-level ranges).</summary>
        Global = 8,
    }
}

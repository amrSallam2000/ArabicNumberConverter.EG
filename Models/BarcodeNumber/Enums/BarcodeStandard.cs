namespace NumericValidation.EG.Models.BarcodeNumber.Enums
{
    /// <summary>
    /// Barcode standards and formats
    /// </summary>
    public enum BarcodeStandard
    {
        /// <summary>EAN-13 standard (13 digits)</summary>
        EAN13,

        /// <summary>EAN-8 standard (8 digits)</summary>
        EAN8,

        /// <summary>UPC-A standard (12 digits)</summary>
        UPCA,

        /// <summary>Code-128 standard (variable length)</summary>
        Code128,

        /// <summary>Local/custom format (specific to regional implementations)</summary>
        Custom,

        /// <summary>Unknown or undetermined standard</summary>
        Unknown
    }
}
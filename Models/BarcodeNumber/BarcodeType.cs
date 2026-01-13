using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NumericValidation.EG.Models.BarcodeNumber
{
    /// <summary>
    /// Types of barcodes supported by the parser
    /// </summary>
    public enum BarcodeType
    {
        /// <summary>Standard barcode (fixed product with predetermined price)</summary>
        Standard,

        /// <summary>Weight-based barcode (contains weight information)</summary>
        Weight,

        /// <summary>Price-based barcode (contains price information)</summary>
        Price,

        /// <summary>Unknown or undetermined barcode type</summary>
        Unknown
    }
}
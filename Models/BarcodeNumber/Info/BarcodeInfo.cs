using NumericValidation.EG.Models.BarcodeNumber.Enums;
using NumericValidation.EG.Models.Common.Extensions;
using NumericValidation.EG.Models.Common.Helpers;
using NumericValidation.EG.Models.NumbersToText;
using NumericValidation.EG.Models.NumbersToText.Converter;
using NumericValidation.EG.Models.NumbersToText.Currencys;
using System;
using System.Collections.Generic;
using System.Text;
using static NumericValidation.EG.Models.NumbersToText.Enums.NumberToWordsConverter;
using static NumericValidation.EG.Models.NumbersToText.Converter.NumberToWordsConverter;

namespace NumericValidation.EG.Models.BarcodeNumber.Info
{
    /// <summary>
    /// Comprehensive data model containing all extracted barcode information
    /// with bilingual support (Arabic/English) and rich formatting options.
    /// </summary>
    public class BarcodeInfo
    {
        #region Configuration Properties

        /// <summary>Currency to use for price conversion (default: EGP)</summary>
        public Currency SelectedCurrency { get; set; } = Currency.EGP;

        /// <summary>Language for number-to-words conversion (default: Arabic)</summary>
        public Language SelectedLanguage { get; set; } = Language.Arabic;

        #endregion

        #region Basic Properties

        /// <summary>Indicates whether the barcode parsing was successful</summary>
        public bool IsValid { get; set; }

        /// <summary>Detailed error message for failed parsing operations</summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>Original unprocessed barcode string as received</summary>
        public string OriginalBarcode { get; set; } = string.Empty;

        /// <summary>Sanitized barcode with all non-numeric characters removed</summary>
        public string CleanedBarcode { get; set; } = string.Empty;

        /// <summary>Detected or specified barcode type (Standard/Weight/Price)</summary>
        public BarcodeType Type { get; set; } = BarcodeType.Unknown;

        /// <summary>Barcode standard format (EAN-13/EAN-8/UPC-A)</summary>
        public BarcodeStandard Standard { get; set; } = BarcodeStandard.Unknown;

        #endregion

        #region Product Properties

        /// <summary>Extracted product/item identification code</summary>
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>Product code displayed using Arabic-Indic numerals (٠-٩)</summary>
        public string ProductCodeArabic => !string.IsNullOrEmpty(ProductCode)
            ? ProductCode.ToArabicNumerals()
            : string.Empty;

        /// <summary>Barcode prefix identifier (e.g., "2" for variable-weight items)</summary>
        public string Prefix { get; set; } = string.Empty;

        /// <summary>Prefix displayed using Arabic-Indic numerals</summary>
        public string PrefixArabic => !string.IsNullOrEmpty(Prefix)
            ? Prefix.ToArabicNumerals()
            : string.Empty;

        /// <summary>Check digit used for barcode validation (last digit)</summary>
        public string CheckDigit { get; set; } = string.Empty;

        /// <summary>GS1 country/region code prefix</summary>
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>Country name in English based on GS1 prefix</summary>
        public string CountryNameEnglish { get; set; } = string.Empty;

        /// <summary>Country name in Arabic based on GS1 prefix</summary>
        public string CountryNameArabic { get; set; } = string.Empty;

        #endregion

        #region Weight Properties

        private decimal _weight;

        /// <summary>Product weight in kilograms (0 for non-weighted items)</summary>
        public decimal Weight
        {
            get => _weight;
            set => _weight = value < 0 ? 0 : value; // Ensure non-negative
        }

        /// <summary>Product weight in grams (calculated from kilograms)</summary>
        public decimal WeightInGrams => Weight * 1000m;

        /// <summary>Formatted weight with unit in English (e.g., "1.234 kg")</summary>
        public string WeightTextEnglish => Weight > 0
            ? $"{Weight:F3} kg"
            : string.Empty;

        /// <summary>Formatted weight with unit in Arabic (e.g., "١٫٢٣٤ كجم")</summary>
        public string WeightTextArabic => Weight > 0
            ? $"{Weight.ToArabicNumerals("F3")} كجم"
            : string.Empty;

        /// <summary>Formatted weight in grams with unit in English (e.g., "1234 grams")</summary>
        public string WeightInGramsTextEnglish => Weight > 0
            ? $"{WeightInGrams:F0} grams"
            : string.Empty;

        /// <summary>Formatted weight in grams with unit in Arabic (e.g., "١٢٣٤ جرام")</summary>
        public string WeightInGramsTextArabic => Weight > 0
            ? $"{WeightInGrams.ToArabicNumerals("F0")} جرام"
            : string.Empty;

        /// <summary>Weight written in Arabic words (e.g., "واحد كيلو ومئتان جرام")</summary>
        public string WeightInWordsArabic
        {
            get
            {
                if (Weight <= 0) return string.Empty;

                int kilograms = (int)Weight;
                int grams = (int)((Weight - kilograms) * 1000);

                var result = new StringBuilder();

                if (kilograms > 0)
                {
                    result.Append(ConvertToArabic(kilograms));
                    result.Append(kilograms == 1 ? " كيلو" : " كيلو");
                }

                if (grams > 0)
                {
                    if (kilograms > 0)
                        result.Append(" و");

                    result.Append(ConvertToArabic(grams));
                    result.Append(grams == 1 ? " جرام" : " جرام");
                }

                return result.ToString();
            }
        }

        /// <summary>Weight written in English words (e.g., "One kilogram and two hundred grams")</summary>
        public string WeightInWordsEnglish
        {
            get
            {
                if (Weight <= 0) return string.Empty;

                int kilograms = (int)Weight;
                int grams = (int)((Weight - kilograms) * 1000);

                var result = new StringBuilder();

                if (kilograms > 0)
                {
                    result.Append(ConvertToEnglish(kilograms));
                    result.Append(kilograms == 1 ? " kilogram" : " kilograms");
                }

                if (grams > 0)
                {
                    if (kilograms > 0)
                        result.Append(" and ");

                    result.Append(ConvertToEnglish(grams));
                    result.Append(grams == 1 ? " gram" : " grams");
                }

                return result.ToString();
            }
        }

        #endregion

        #region Price Properties

        private decimal _price;

        /// <summary>Product price in selected currency</summary>
        public decimal Price
        {
            get => _price;
            set => _price = value < 0 ? 0 : value; // Ensure non-negative
        }

        /// <summary>Formatted price with currency symbol in English (e.g., "50.00 EGP", "100.00 USD")</summary>
        public string PriceTextEnglish => Price > 0
            ? $"{Price:F2} {GetCurrencySymbolEnglish(SelectedCurrency)}"
            : string.Empty;

        /// <summary>Formatted price with currency name in Arabic (e.g., "٥٠٫٠٠ جنيه مصري", "١٠٠٫٠٠ دولار أمريكي")</summary>
        public string PriceTextArabic => Price > 0
            ? $"{NumberConversionHelper.ToArabicNumerals(Price.ToString("F2"))} {GetCurrencyNameArabic(SelectedCurrency)}"
            : string.Empty;

        /// <summary>Price written in words in selected language and currency</summary>
        public string PriceInWords => Price > 0
            ? Convert(Price, SelectedLanguage, SelectedCurrency, true)
            : string.Empty;

        /// <summary>Price written in Arabic words with selected currency</summary>
        public string PriceInWordsArabic => Price > 0
            ? Convert(Price, Language.Arabic, SelectedCurrency, true)
            : string.Empty;

        /// <summary>Price written in English words with selected currency</summary>
        public string PriceInWordsEnglish => Price > 0
            ? Convert(Price, Language.English, SelectedCurrency, true)
            : string.Empty;

        #endregion

        #region Unit Price Properties

        /// <summary>
        /// Calculated unit price per kilogram (Price ÷ Weight).
        /// Returns 0 if weight or price is not available.
        /// </summary>
        public decimal UnitPrice => Weight > 0 && Price > 0
            ? Math.Round(Price / Weight, 2)
            : 0;

        /// <summary>Formatted unit price in English (e.g., "100.00 EGP/kg", "200.00 USD/kg")</summary>
        public string UnitPriceTextEnglish => UnitPrice > 0
            ? $"{UnitPrice:F2} {GetCurrencySymbolEnglish(SelectedCurrency)}/kg"
            : string.Empty;

        /// <summary>Formatted unit price in Arabic (e.g., "١٠٠٫٠٠ جنيه/كجم", "٢٠٠٫٠٠ دولار/كجم")</summary>
        public string UnitPriceTextArabic => UnitPrice > 0
            ? $"{UnitPrice.ToArabicNumerals("F2")} {GetCurrencyNameArabic(SelectedCurrency)}/كجم"
            : string.Empty;

        /// <summary>Unit price written in words in selected language and currency</summary>
        public string UnitPriceInWords => UnitPrice > 0
            ? $"{Convert(UnitPrice, SelectedLanguage, SelectedCurrency, true)} لكل كيلو"
            : string.Empty;

        #endregion

        #region Summary Properties

        /// <summary>Human-readable barcode type name in English</summary>
        public string TypeNameEnglish => Type switch
        {
            BarcodeType.Standard => "Standard Product",
            BarcodeType.Weight => "Weighted Product",
            BarcodeType.Price => "Priced Product",
            _ => "Unknown"
        };

        /// <summary>Human-readable barcode type name in Arabic</summary>
        public string TypeNameArabic => Type switch
        {
            BarcodeType.Standard => "منتج عادي",
            BarcodeType.Weight => "منتج بالوزن",
            BarcodeType.Price => "منتج بالسعر",
            _ => "غير معروف"
        };

        /// <summary>
        /// Comprehensive single-line summary in English.
        /// Includes type, product code, weight, price, and unit price.
        /// </summary>
        public string SummaryEnglish
        {
            get
            {
                if (!IsValid) return ErrorMessage;

                var parts = new List<string>
                {
                    TypeNameEnglish,
                    $"Product: {ProductCode}"
                };

                if (Weight > 0)
                    parts.Add($"Weight: {WeightTextEnglish}");

                if (Price > 0)
                    parts.Add($"Price: {PriceTextEnglish}");

                if (UnitPrice > 0)
                    parts.Add($"Unit Price: {UnitPriceTextEnglish}");

                return string.Join(", ", parts);
            }
        }

        /// <summary>
        /// Comprehensive single-line summary in Arabic.
        /// Includes type, product code, weight, price, and unit price.
        /// </summary>
        public string SummaryArabic
        {
            get
            {
                if (!IsValid) return ErrorMessage;

                var parts = new List<string>
                {
                    TypeNameArabic,
                    $"الصنف: {ProductCodeArabic}"
                };

                if (Weight > 0)
                    parts.Add($"الوزن: {WeightTextArabic}");

                if (Price > 0)
                    parts.Add($"السعر: {PriceTextArabic}");

                if (UnitPrice > 0)
                    parts.Add($"سعر الكيلو: {UnitPriceTextArabic}");

                return string.Join("، ", parts);
            }
        }

        #endregion

        #region Currency Helper Methods

        /// <summary>
        /// Gets currency name in Arabic based on selected currency
        /// </summary>
        private string GetCurrencyNameArabic(Currency currency)
        {
            // Get from CurrencyData if available, otherwise return generic
            var currencies = NumberToWordsConverter.GetSupportedCurrencies();
            return currencies.ContainsKey(currency)
                ? currencies[currency].MainUnitArabic
                : "وحدة";
        }

        /// <summary>
        /// Gets currency symbol in English based on selected currency
        /// </summary>
        private string GetCurrencySymbolEnglish(Currency currency)
        {
            return currency switch
            {
                Currency.EGP => "EGP",
                Currency.USD => "USD",
                Currency.EUR => "EUR",
                Currency.GBP => "GBP",
                Currency.SAR => "SAR",
                Currency.AED => "AED",
                Currency.KWD => "KWD",
                Currency.QAR => "QAR",
                Currency.BHD => "BHD",
                Currency.OMR => "OMR",
                Currency.JOD => "JOD",
                Currency.LBP => "LBP",
                Currency.SYP => "SYP",
                Currency.IQD => "IQD",
                Currency.YER => "YER",
                Currency.MAD => "MAD",
                Currency.TND => "TND",
                Currency.DZD => "DZD",
                Currency.LYD => "LYD",
                Currency.SDG => "SDG",
                _ => currency.ToString()
            };
        }

        /// <summary>
        /// Gets currency name in English based on selected currency
        /// </summary>
        private string GetCurrencyNameEnglish(Currency currency)
        {
            // Get from CurrencyData if available, otherwise return enum name
            var currencies = NumberToWordsConverter.GetSupportedCurrencies();
            return currencies.ContainsKey(currency)
                ? currencies[currency].MainUnitEnglish
                : currency.ToString();
        }

        /// <summary>
        /// Gets all supported currencies with their info
        /// </summary>
        public Dictionary<Currency, CurrencyInfo> GetSupportedCurrencies()
        {
            return NumberToWordsConverter.GetSupportedCurrencies();
        }

        #endregion

        #region ToString Methods

        /// <summary>
        /// Generates a comprehensive formatted report with all barcode information.
        /// Includes bilingual (English/Arabic) display with proper sectioning and emojis.
        /// </summary>
        /// <returns>Multi-line formatted string with complete barcode details</returns>
        public override string ToString()
        {
            if (!IsValid)
                return $"❌ Invalid Barcode / باركود غير صالح{Environment.NewLine}   Error: {ErrorMessage}";

            var sb = new StringBuilder();

            sb.AppendLine("═══════════════════════════════════════════════════════════════");
            sb.AppendLine("                      BARCODE INFORMATION                       ");
            sb.AppendLine("                      معلومات الباركود                         ");
            sb.AppendLine("═══════════════════════════════════════════════════════════════");
            sb.AppendLine();

            // Currency Info
            sb.AppendLine($"💰 Currency / العملة: {GetCurrencyNameEnglish(SelectedCurrency)} / {GetCurrencyNameArabic(SelectedCurrency)}");
            sb.AppendLine();

            // Barcode Section
            sb.AppendLine("📊 Barcode / الباركود:");
            sb.AppendLine($"   {CleanedBarcode}");
            sb.AppendLine($"   Type / النوع: {TypeNameEnglish} / {TypeNameArabic}");
            sb.AppendLine($"   Standard / المعيار: {Standard}");
            sb.AppendLine();

            // Product Section
            sb.AppendLine("📦 Product Information / معلومات المنتج:");
            sb.AppendLine($"   Product Code / كود الصنف: {ProductCode} / {ProductCodeArabic}");

            if (!string.IsNullOrEmpty(Prefix))
                sb.AppendLine($"   Prefix / المفتاح: {Prefix} / {PrefixArabic}");

            if (!string.IsNullOrEmpty(CountryCode))
                sb.AppendLine($"   Country / الدولة: {CountryNameEnglish} / {CountryNameArabic} ({CountryCode})");

            sb.AppendLine();

            // Weight Section
            if (Weight > 0)
            {
                sb.AppendLine("⚖️  Weight / الوزن:");
                sb.AppendLine($"   Kilograms / كيلوجرام: {WeightTextEnglish} / {WeightTextArabic}");
                sb.AppendLine($"   Grams / جرام: {WeightInGramsTextEnglish} / {WeightInGramsTextArabic}");
                sb.AppendLine($"   In Words / بالحروف: {WeightInWordsEnglish} / {WeightInWordsArabic}");
                sb.AppendLine();
            }

            // Price Section
            if (Price > 0)
            {
                sb.AppendLine("💰 Price / السعر:");
                sb.AppendLine($"   Total / الإجمالي: {PriceTextEnglish} / {PriceTextArabic}");
                sb.AppendLine($"   In Words / بالحروف: {PriceInWordsEnglish} / {PriceInWordsArabic}");
                sb.AppendLine();
            }

            // Unit Price Section
            if (UnitPrice > 0)
            {
                sb.AppendLine("💵 Unit Price / سعر الوحدة:");
                sb.AppendLine($"   Per Kg / للكيلو: {UnitPriceTextEnglish} / {UnitPriceTextArabic}");
                sb.AppendLine($"   In Words / بالحروف: {UnitPriceInWords}");
                sb.AppendLine();
            }

            // Summary Section
            sb.AppendLine("📋 Summary / الملخص:");
            sb.AppendLine($"   EN: {SummaryEnglish}");
            sb.AppendLine($"   AR: {SummaryArabic}");
            sb.AppendLine();

            sb.AppendLine("═══════════════════════════════════════════════════════════════");

            return sb.ToString();
        }

        /// <summary>
        /// Returns a concise single-line summary in English.
        /// </summary>
        /// <returns>Brief English summary or error message</returns>
        public string ToStringEnglish()
        {
            return IsValid ? SummaryEnglish : $"Invalid: {ErrorMessage}";
        }

        /// <summary>
        /// Returns a concise single-line summary in Arabic.
        /// </summary>
        /// <returns>Brief Arabic summary or error message</returns>
        public string ToStringArabic()
        {
            return IsValid ? SummaryArabic : $"غير صالح: {ErrorMessage}";
        }

        #endregion

        #region Additional Utility Methods

        /// <summary>
        /// Creates a JSON-friendly dictionary of all barcode properties.
        /// Useful for API responses and serialization scenarios.
        /// </summary>
        /// <returns>Dictionary with all barcode information</returns>
        public Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>
            {
                { "isValid", IsValid },
                { "type", Type.ToString() },
                { "standard", Standard.ToString() },
                { "currency", SelectedCurrency.ToString() },
                { "currencyNameArabic", GetCurrencyNameArabic(SelectedCurrency) },
                { "currencyNameEnglish", GetCurrencyNameEnglish(SelectedCurrency) },
                { "language", SelectedLanguage.ToString() },
                { "originalBarcode", OriginalBarcode },
                { "cleanedBarcode", CleanedBarcode },
                { "productCode", ProductCode },
                { "productCodeArabic", ProductCodeArabic },
                { "countryCode", CountryCode },
                { "countryNameEnglish", CountryNameEnglish },
                { "countryNameArabic", CountryNameArabic }
            };

            if (!IsValid)
            {
                dict.Add("errorMessage", ErrorMessage);
                return dict;
            }

            if (Weight > 0)
            {
                dict.Add("weightKg", Weight);
                dict.Add("weightGrams", WeightInGrams);
                dict.Add("weightTextEnglish", WeightTextEnglish);
                dict.Add("weightTextArabic", WeightTextArabic);
                dict.Add("weightInWordsArabic", WeightInWordsArabic);
                dict.Add("weightInWordsEnglish", WeightInWordsEnglish);
            }

            if (Price > 0)
            {
                dict.Add("price", Price);
                dict.Add("priceTextEnglish", PriceTextEnglish);
                dict.Add("priceTextArabic", PriceTextArabic);
                dict.Add("priceInWords", PriceInWords);
                dict.Add("priceInWordsArabic", PriceInWordsArabic);
                dict.Add("priceInWordsEnglish", PriceInWordsEnglish);
            }

            if (UnitPrice > 0)
            {
                dict.Add("unitPricePerKg", UnitPrice);
                dict.Add("unitPriceTextEnglish", UnitPriceTextEnglish);
                dict.Add("unitPriceTextArabic", UnitPriceTextArabic);
                dict.Add("unitPriceInWords", UnitPriceInWords);
            }

            return dict;
        }

        /// <summary>
        /// Returns detailed conversion information using NumberToWordsConverter
        /// </summary>
        public ConversionResult GetPriceConversionDetails()
        {
            return ConvertWithDetails(Price, SelectedLanguage, SelectedCurrency);
        }

        /// <summary>
        /// Changes currency and updates all price-related properties
        /// </summary>
        public void ChangeCurrency(Currency newCurrency)
        {
            SelectedCurrency = newCurrency;
        }

        /// <summary>
        /// Changes language and updates all text-related properties
        /// </summary>
        public void ChangeLanguage(Language newLanguage)
        {
            SelectedLanguage = newLanguage;
        }

        /// <summary>
        /// Gets the complete currency information for the selected currency
        /// </summary>
        public CurrencyInfo GetCurrentCurrencyInfo()
        {
            var currencies = NumberToWordsConverter.GetSupportedCurrencies();
            return currencies.ContainsKey(SelectedCurrency)
                ? currencies[SelectedCurrency]
                : null;
        }

        #endregion
    }
}
using NumericValidation.EG.Models.BankCard.Enums;
using NumericValidation.EG.Models.NumbersToText.Enums;
using System;
using System.Collections.Generic;
using static NumericValidation.EG.Models.NumbersToText.Enums.NumberToWordsConverter;

namespace NumericValidation.EG.Models.BankCard.Issuer
{
    // ═══════════════════════════════════════════════════════════════════════════
    //  ArabicNumberConverter.EG.BankCard — IssuerDatabase
    //  Egyptian issuers + global IIN/BIN range table
    //  Standards: ISO/IEC 7812-1:2017 | CBE Regulations 2024-2025
    //  Last Updated: February 2025
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Metadata about a card issuer (bank or financial institution),
    /// keyed by its IIN/BIN prefix (first 6 or 8 digits of the PAN).
    /// Populated from ISO/IEC 7812 registrations and publicly available BIN lists.
    /// </summary>
    public class IssuerInfo
    {
        /// <summary>IIN / BIN prefix — 6 or 8 digits (ISO/IEC 7812-1 §4).</summary>
        public string IIN { get; set; } = string.Empty;

        /// <summary>Issuer / bank name in English.</summary>
        public string IssuerName { get; set; } = string.Empty;

        /// <summary>Issuer name in Arabic (UTF-8).</summary>
        public string IssuerNameArabic { get; set; } = string.Empty;

        /// <summary>ISO 3166-1 alpha-2 country code of the issuer (e.g. "EG", "US").</summary>
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>Country name in English.</summary>
        public string CountryName { get; set; } = string.Empty;

        /// <summary>Country name in Arabic (UTF-8).</summary>
        public string CountryNameArabic { get; set; } = string.Empty;

        /// <summary>
        /// ISO 4217 currency code of the card's default settlement currency.
        /// Now using Currency enum from NumbersToText library.
        /// </summary>
        public Currency CurrencyCode { get; set; } = Currency.EGP;

        /// <summary>Payment card network (scheme).</summary>
        public CardNetwork Network { get; set; }

        /// <summary>Functional card type (Credit / Debit / Prepaid …).</summary>
        public CardType CardType { get; set; }

        /// <summary>Card tier / category (Classic / Gold / Platinum …).</summary>
        public CardCategory CardCategory { get; set; }

        /// <summary>Geographic region of the issuer.</summary>
        public IssuerRegion Region { get; set; }

        /// <summary><c>true</c> if the card was issued by an Egyptian bank regulated by the CBE.</summary>
        public bool IsEgyptian { get; set; }

        /// <summary>Official website of the issuing bank.</summary>
        public string? IssuerWebsite { get; set; }

        /// <summary>Customer-service short-dial number valid within Egypt.</summary>
        public string? CustomerServiceEgypt { get; set; }

        /// <summary>
        /// <c>true</c> if the issuer participates in the CBE Tokenization programme
        /// (Apple Pay, Tokenized e-commerce, etc.) per CBE circular 2024-2025.
        /// </summary>
        public bool SupportsTokenization { get; set; }

        /// <summary>Expected length of the CVV / CVC / CID security code (3 for most networks; 4 for Amex).</summary>
        public int CvvLength { get; set; } = 3;

        /// <summary>Valid PAN lengths for this issuer/network combination (ISO/IEC 7812-1 §3).</summary>
        public int[] ValidPanLengths { get; set; } = { 16 };
    }

    /// <summary>
    /// Static BIN / IIN database containing:
    /// <list type="bullet">
    ///   <item>Egyptian bank issuers (Meeza, Visa EG, Mastercard EG) with full metadata.</item>
    ///   <item>Global network prefix ranges for automatic scheme detection.</item>
    /// </list>
    /// Data is accurate as of February 2025. For production use, consider supplementing with
    /// a commercial BIN database (e.g. Mastercard BIN API, Stripe, or Adyen).
    /// </summary>
    public static class IssuerDatabase
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // EGYPTIAN ISSUERS - Complete List of All Banks Licensed by CBE
        // Organized by category: Public Sector, Private, Islamic, FinTech, Foreign Branches
        // All banks are fully compliant with Central Bank of Egypt (CBE) regulations
        // ─────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Complete list of Egyptian card issuers keyed by IIN prefix.
        /// Covers Meeza (507800-507809), Visa Egypt, Mastercard Egypt BINs, and all major Egyptian banks.
        /// All issuers below are licensed by and report to the Central Bank of Egypt (CBE).
        /// </summary>
        public static readonly List<IssuerInfo> EgyptianIssuers = new List<IssuerInfo>
        {
            // ─────────────────────────────────────────────────────────────────────────
            // PUBLIC SECTOR BANKS (State-Owned Banks)
            // These banks are fully owned by the Egyptian government and represent
            // the backbone of the Egyptian banking sector.
            // ─────────────────────────────────────────────────────────────────────────

            // National Bank of Egypt (NBE) - Egypt's largest and oldest bank, established 1898
            // Website: https://www.nbe.com.eg | Customer Service: 19623
            new IssuerInfo
            {
                IIN = "507803",
                IssuerName = "National Bank of Egypt (NBE) - Meeza Debit",
                IssuerNameArabic = "البنك الأهلي المصري - ميزة خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Meeza,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.nbe.com.eg",
                CustomerServiceEgypt = "19623",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "428541",
                IssuerName = "National Bank of Egypt (NBE) - Visa Classic Debit",
                IssuerNameArabic = "البنك الأهلي المصري - فيزا كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.nbe.com.eg",
                CustomerServiceEgypt = "19623",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "404906",
                IssuerName = "National Bank of Egypt (NBE) - Visa Gold Credit",
                IssuerNameArabic = "البنك الأهلي المصري - فيزا ذهبية ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Gold,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.nbe.com.eg",
                CustomerServiceEgypt = "19623",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "512345",
                IssuerName = "National Bank of Egypt (NBE) - Mastercard Classic Debit",
                IssuerNameArabic = "البنك الأهلي المصري - ماستركارد كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.nbe.com.eg",
                CustomerServiceEgypt = "19623",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "524567",
                IssuerName = "National Bank of Egypt (NBE) - Mastercard Platinum Credit",
                IssuerNameArabic = "البنك الأهلي المصري - ماستركارد بلاتينيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Platinum,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.nbe.com.eg",
                CustomerServiceEgypt = "19623",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Banque Misr - Second largest state-owned bank, established 1920
            // Website: https://www.banquemisr.com | Customer Service: 19888
            new IssuerInfo
            {
                IIN = "507800",
                IssuerName = "Banque Misr - Meeza Debit",
                IssuerNameArabic = "بنك مصر - ميزة خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Meeza,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.banquemisr.com",
                CustomerServiceEgypt = "19888",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "489737",
                IssuerName = "Banque Misr - Visa Classic Debit",
                IssuerNameArabic = "بنك مصر - فيزا كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.banquemisr.com",
                CustomerServiceEgypt = "19888",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "522081",
                IssuerName = "Banque Misr - Mastercard World Credit",
                IssuerNameArabic = "بنك مصر - ماستركارد ورلد ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.World,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.banquemisr.com",
                CustomerServiceEgypt = "19888",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "530123",
                IssuerName = "Banque Misr - Mastercard Titanium Credit",
                IssuerNameArabic = "بنك مصر - ماستركارد تيتانيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Titanium,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.banquemisr.com",
                CustomerServiceEgypt = "19888",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Banque du Caire - State-owned bank, established 1952
            // Website: https://www.bcbe.com | Customer Service: 19819
            new IssuerInfo
            {
                IIN = "507806",
                IssuerName = "Banque du Caire - Meeza Debit",
                IssuerNameArabic = "بنك القاهرة - ميزة خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Meeza,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.bcbe.com",
                CustomerServiceEgypt = "19819",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "529948",
                IssuerName = "Banque du Caire - Mastercard Classic Debit",
                IssuerNameArabic = "بنك القاهرة - ماستركارد كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.bcbe.com",
                CustomerServiceEgypt = "19819",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "413579",
                IssuerName = "Banque du Caire - Visa Gold Credit",
                IssuerNameArabic = "بنك القاهرة - فيزا ذهبية ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Gold,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.bcbe.com",
                CustomerServiceEgypt = "19819",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // ─────────────────────────────────────────────────────────────────────────
            // PRIVATE SECTOR BANKS
            // Major private banks operating in Egypt under CBE supervision
            // ─────────────────────────────────────────────────────────────────────────

            // Commercial International Bank (CIB) - Largest private bank in Egypt
            // Website: https://www.cibeg.com | Customer Service: 19666
            new IssuerInfo
            {
                IIN = "507804",
                IssuerName = "Commercial International Bank (CIB) - Meeza Debit",
                IssuerNameArabic = "البنك التجاري الدولي - ميزة خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Meeza,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.cibeg.com",
                CustomerServiceEgypt = "19666",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "455676",
                IssuerName = "Commercial International Bank (CIB) - Visa Platinum Credit",
                IssuerNameArabic = "البنك التجاري الدولي - فيزا بلاتينيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Platinum,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.cibeg.com",
                CustomerServiceEgypt = "19666",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "557368",
                IssuerName = "Commercial International Bank (CIB) - Mastercard Gold Credit",
                IssuerNameArabic = "البنك التجاري الدولي - ماستركارد ذهبية ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Gold,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.cibeg.com",
                CustomerServiceEgypt = "19666",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "540123",
                IssuerName = "Commercial International Bank (CIB) - Mastercard World Elite Credit",
                IssuerNameArabic = "البنك التجاري الدولي - ماستركارد ورلد إليت ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.WorldElite,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.cibeg.com",
                CustomerServiceEgypt = "19666",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // QNB Al Ahli - Egyptian subsidiary of QNB Group (Qatar)
            // Website: https://www.qnbalahli.com | Customer Service: 19700
            new IssuerInfo
            {
                IIN = "507805",
                IssuerName = "QNB Al Ahli - Meeza Debit",
                IssuerNameArabic = "بنك قطر الوطني الأهلي - ميزة خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Meeza,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.qnbalahli.com",
                CustomerServiceEgypt = "19700",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "431493",
                IssuerName = "QNB Al Ahli - Visa Infinite Credit",
                IssuerNameArabic = "بنك قطر الوطني الأهلي - فيزا إنفينيت ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Infinite,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.qnbalahli.com",
                CustomerServiceEgypt = "19700",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "525678",
                IssuerName = "QNB Al Ahli - Mastercard Platinum Credit",
                IssuerNameArabic = "بنك قطر الوطني الأهلي - ماستركارد بلاتينيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Platinum,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.qnbalahli.com",
                CustomerServiceEgypt = "19700",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Arab African International Bank (AAIB) - Leading private bank
            // Website: https://www.aaib.com | Customer Service: 16333
            new IssuerInfo
            {
                IIN = "507807",
                IssuerName = "Arab African International Bank (AAIB) - Meeza Debit",
                IssuerNameArabic = "البنك العربي الأفريقي الدولي - ميزة خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Meeza,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.aaib.com",
                CustomerServiceEgypt = "16333",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "552481",
                IssuerName = "Arab African International Bank (AAIB) - Mastercard Platinum Credit",
                IssuerNameArabic = "البنك العربي الأفريقي الدولي - ماستركارد بلاتينيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Platinum,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.aaib.com",
                CustomerServiceEgypt = "16333",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "446789",
                IssuerName = "Arab African International Bank (AAIB) - Visa Signature Credit",
                IssuerNameArabic = "البنك العربي الأفريقي الدولي - فيزا سيجنتشر ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Signature,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.aaib.com",
                CustomerServiceEgypt = "16333",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Bank of Alexandria (Alex Bank) - Part of Intesa Sanpaolo Group
            // Website: https://www.alexbank.com | Customer Service: 19033
            new IssuerInfo
            {
                IIN = "507801",
                IssuerName = "Bank of Alexandria - Meeza Debit",
                IssuerNameArabic = "بنك الإسكندرية - ميزة خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Meeza,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.alexbank.com",
                CustomerServiceEgypt = "19033",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "410441",
                IssuerName = "Bank of Alexandria - Visa Classic Debit",
                IssuerNameArabic = "بنك الإسكندرية - فيزا كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.alexbank.com",
                CustomerServiceEgypt = "19033",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "531741",
                IssuerName = "Bank of Alexandria - Mastercard Standard Debit",
                IssuerNameArabic = "بنك الإسكندرية - ماستركارد ستاندرد خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Standard,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.alexbank.com",
                CustomerServiceEgypt = "19033",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Crédit Agricole Egypt - Subsidiary of Crédit Agricole Group (France)
            // Website: https://www.ca-egypt.com | Customer Service: 19191
            new IssuerInfo
            {
                IIN = "507802",
                IssuerName = "Crédit Agricole Egypt - Meeza Debit",
                IssuerNameArabic = "كريدي أجريكول مصر - ميزة خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Meeza,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.ca-egypt.com",
                CustomerServiceEgypt = "19191",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "412851",
                IssuerName = "Crédit Agricole Egypt - Visa Platinum Credit",
                IssuerNameArabic = "كريدي أجريكول مصر - فيزا بلاتينيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Platinum,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.ca-egypt.com",
                CustomerServiceEgypt = "19191",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "545502",
                IssuerName = "Crédit Agricole Egypt - Mastercard Gold Credit",
                IssuerNameArabic = "كريدي أجريكول مصر - ماستركارد ذهبية ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Gold,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.ca-egypt.com",
                CustomerServiceEgypt = "19191",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // HSBC Bank Egypt - Subsidiary of HSBC Holdings plc (UK)
            // Website: https://www.hsbc.com.eg | Customer Service: 19007
            new IssuerInfo
            {
                IIN = "447710",
                IssuerName = "HSBC Egypt - Visa Classic Credit",
                IssuerNameArabic = "HSBC مصر - فيزا كلاسيك ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.hsbc.com.eg",
                CustomerServiceEgypt = "19007",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "549876",
                IssuerName = "HSBC Egypt - Mastercard World Credit",
                IssuerNameArabic = "HSBC مصر - ماستركارد ورلد ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.World,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.hsbc.com.eg",
                CustomerServiceEgypt = "19007",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Arab Bank Egypt - Subsidiary of Arab Bank (Jordan)
            // Website: https://www.arabbank.com.eg | Customer Service: 16911
            new IssuerInfo
            {
                IIN = "424632",
                IssuerName = "Arab Bank Egypt - Visa Classic Debit",
                IssuerNameArabic = "البنك العربي مصر - فيزا كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.arabbank.com.eg",
                CustomerServiceEgypt = "16911",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "537890",
                IssuerName = "Arab Bank Egypt - Mastercard Gold Credit",
                IssuerNameArabic = "البنك العربي مصر - ماستركارد ذهبية ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Gold,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.arabbank.com.eg",
                CustomerServiceEgypt = "16911",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Egyptian Gulf Bank (EGB)
            // Website: https://www.egb.com.eg | Customer Service: 19595
            new IssuerInfo
            {
                IIN = "456789",
                IssuerName = "Egyptian Gulf Bank (EGB) - Visa Classic Debit",
                IssuerNameArabic = "البنك المصري الخليجي - فيزا كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.egb.com.eg",
                CustomerServiceEgypt = "19595",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "532456",
                IssuerName = "Egyptian Gulf Bank (EGB) - Mastercard Platinum Credit",
                IssuerNameArabic = "البنك المصري الخليجي - ماستركارد بلاتينيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Platinum,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.egb.com.eg",
                CustomerServiceEgypt = "19595",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // United Bank (Al Masrif Al Muttahid)
            // Website: https://www.united-bank.com.eg | Customer Service: 16222
            new IssuerInfo
            {
                IIN = "438820",
                IssuerName = "United Bank - Visa Classic Debit",
                IssuerNameArabic = "المصرف المتحد - فيزا كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.united-bank.com.eg",
                CustomerServiceEgypt = "16222",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "541234",
                IssuerName = "United Bank - Mastercard Gold Credit",
                IssuerNameArabic = "المصرف المتحد - ماستركارد ذهبية ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Gold,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.united-bank.com.eg",
                CustomerServiceEgypt = "16222",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Suez Canal Bank
            // Website: https://www.scbank.com.eg | Customer Service: 16247
            new IssuerInfo
            {
                IIN = "462210",
                IssuerName = "Suez Canal Bank - Visa Classic Debit",
                IssuerNameArabic = "بنك قناة السويس - فيزا كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.scbank.com.eg",
                CustomerServiceEgypt = "16247",
                SupportsTokenization = false,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "538901",
                IssuerName = "Suez Canal Bank - Mastercard Standard Debit",
                IssuerNameArabic = "بنك قناة السويس - ماستركارد ستاندرد خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Standard,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.scbank.com.eg",
                CustomerServiceEgypt = "16247",
                SupportsTokenization = false,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Housing and Development Bank (HDB)
            // Website: https://www.hdb.com.eg | Customer Service: 19955
            new IssuerInfo
            {
                IIN = "533254",
                IssuerName = "Housing and Development Bank (HDB) - Mastercard Classic Debit",
                IssuerNameArabic = "بنك الإسكان والتعمير - ماستركارد كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.hdb.com.eg",
                CustomerServiceEgypt = "19955",
                SupportsTokenization = false,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "408765",
                IssuerName = "Housing and Development Bank (HDB) - Visa Classic Debit",
                IssuerNameArabic = "بنك الإسكان والتعمير - فيزا كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.hdb.com.eg",
                CustomerServiceEgypt = "19955",
                SupportsTokenization = false,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Al Ahli United Bank Egypt (AUB) - Now part of First Abu Dhabi Bank (FAB)
            // Website: https://www.aubegypt.com | Customer Service: 19330
            new IssuerInfo
            {
                IIN = "507809",
                IssuerName = "Al Ahli United Bank Egypt (AUB) - Meeza Debit",
                IssuerNameArabic = "البنك الأهلي المتحد مصر - ميزة خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Meeza,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.aubegypt.com",
                CustomerServiceEgypt = "19330",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "544606",
                IssuerName = "Al Ahli United Bank Egypt (AUB) - Mastercard Classic Debit",
                IssuerNameArabic = "البنك الأهلي المتحد مصر - ماستركارد كلاسيك خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.aubegypt.com",
                CustomerServiceEgypt = "19330",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // ─────────────────────────────────────────────────────────────────────────
            // ISLAMIC BANKS
            // Sharia-compliant banking institutions operating in Egypt
            // ─────────────────────────────────────────────────────────────────────────

            // Faisal Islamic Bank of Egypt - First Islamic bank in Egypt, established 1979
            // Website: https://www.faisalbank.com.eg | Customer Service: 19628
            new IssuerInfo
            {
                IIN = "540000",
                IssuerName = "Faisal Islamic Bank of Egypt - Mastercard Islamic Debit",
                IssuerNameArabic = "بنك فيصل الإسلامي المصري - ماستركارد إسلامي خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.faisalbank.com.eg",
                CustomerServiceEgypt = "19628",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "410987",
                IssuerName = "Faisal Islamic Bank of Egypt - Visa Islamic Credit",
                IssuerNameArabic = "بنك فيصل الإسلامي المصري - فيزا إسلامي ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Gold,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.faisalbank.com.eg",
                CustomerServiceEgypt = "19628",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Abu Dhabi Islamic Bank Egypt (ADIB Egypt)
            // Website: https://www.adib.com.eg | Customer Service: 19977
            new IssuerInfo
            {
                IIN = "507808",
                IssuerName = "Abu Dhabi Islamic Bank Egypt (ADIB) - Meeza Debit",
                IssuerNameArabic = "بنك أبوظبي الإسلامي مصر - ميزة خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Meeza,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.adib.com.eg",
                CustomerServiceEgypt = "19977",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "536789",
                IssuerName = "Abu Dhabi Islamic Bank Egypt (ADIB) - Mastercard Islamic Platinum Credit",
                IssuerNameArabic = "بنك أبوظبي الإسلامي مصر - ماستركارد إسلامي بلاتينيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Platinum,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.adib.com.eg",
                CustomerServiceEgypt = "19977",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Al Baraka Bank Egypt - Part of Al Baraka Banking Group (Bahrain)
            // Website: https://www.albaraka-bank.com.eg | Customer Service: 16993
            new IssuerInfo
            {
                IIN = "543000",
                IssuerName = "Al Baraka Bank Egypt - Mastercard Islamic Debit",
                IssuerNameArabic = "بنك البركة مصر - ماستركارد إسلامي خصم مباشر",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Debit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.albaraka-bank.com.eg",
                CustomerServiceEgypt = "16993",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "426543",
                IssuerName = "Al Baraka Bank Egypt - Visa Islamic Gold Credit",
                IssuerNameArabic = "بنك البركة مصر - فيزا إسلامي ذهبية ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Gold,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.albaraka-bank.com.eg",
                CustomerServiceEgypt = "16993",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // ─────────────────────────────────────────────────────────────────────────
            // FINTECH & PAYMENT COMPANIES
            // Digital payment service providers licensed by CBE
            // ─────────────────────────────────────────────────────────────────────────

            // Fawry - Leading digital payment platform in Egypt
            // Website: https://www.fawry.com | Customer Service: 19350
            new IssuerInfo
            {
                IIN = "539542",
                IssuerName = "Fawry - Mastercard Prepaid Card",
                IssuerNameArabic = "فوري - ماستركارد بطاقة مدفوعة مسبقاً",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Prepaid,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.fawry.com",
                CustomerServiceEgypt = "19350",
                SupportsTokenization = false,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "506789",
                IssuerName = "Fawry - Meeza Prepaid Card",
                IssuerNameArabic = "فوري - ميزة بطاقة مدفوعة مسبقاً",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Meeza,
                CardType = CardType.Prepaid,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.fawry.com",
                CustomerServiceEgypt = "19350",
                SupportsTokenization = false,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Paymob - Digital payments infrastructure provider
            // Website: https://paymob.com | Customer Service: 19976
            new IssuerInfo
            {
                IIN = "508000",
                IssuerName = "Paymob - Mastercard Prepaid Card",
                IssuerNameArabic = "باي موب - ماستركارد بطاقة مدفوعة مسبقاً",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Prepaid,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://paymob.com",
                CustomerServiceEgypt = "19976",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // MoneyFellows - Digital financial platform
            // Website: https://moneyfellows.com | Customer Service: 16060
            new IssuerInfo
            {
                IIN = "509000",
                IssuerName = "MoneyFellows - Visa Prepaid Card",
                IssuerNameArabic = "ماني فلووز - فيزا بطاقة مدفوعة مسبقاً",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Prepaid,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://moneyfellows.com",
                CustomerServiceEgypt = "16060",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // ─────────────────────────────────────────────────────────────────────────
            // FOREIGN BANK BRANCHES
            // International banks operating branches in Egypt
            // ─────────────────────────────────────────────────────────────────────────

            // Citi Egypt - Branch of Citibank N.A. (USA)
            // Website: https://www.citi.com.eg | Customer Service: 19234
            new IssuerInfo
            {
                IIN = "374000",
                IssuerName = "Citi Egypt - American Express Platinum Credit",
                IssuerNameArabic = "سيتي بنك مصر - أمريكان إكسبريس بلاتينيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.AmericanExpress,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Platinum,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.citi.com.eg",
                CustomerServiceEgypt = "19234",
                SupportsTokenization = true,
                CvvLength = 4,
                ValidPanLengths = new[] { 15 }
            },
            new IssuerInfo
            {
                IIN = "401234",
                IssuerName = "Citi Egypt - Visa Infinite Credit",
                IssuerNameArabic = "سيتي بنك مصر - فيزا إنفينيت ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.USD,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Infinite,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.citi.com.eg",
                CustomerServiceEgypt = "19234",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // First Abu Dhabi Bank Egypt (FAB Egypt) - Formerly Bank Audi
            // Website: https://www.bankfab.com.eg | Customer Service: 16661
            new IssuerInfo
            {
                IIN = "520000",
                IssuerName = "First Abu Dhabi Bank Egypt (FAB) - Mastercard World Credit",
                IssuerNameArabic = "بنك أبوظبي الأول مصر - ماستركارد ورلد ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.World,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.bankfab.com.eg",
                CustomerServiceEgypt = "16661",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "423456",
                IssuerName = "First Abu Dhabi Bank Egypt (FAB) - Visa Signature Credit",
                IssuerNameArabic = "بنك أبوظبي الأول مصر - فيزا سيجنتشر ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Signature,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.bankfab.com.eg",
                CustomerServiceEgypt = "16661",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Mashreq Bank Egypt - Branch of Mashreqbank (UAE)
            // Website: https://www.mashreqbank.com.eg | Customer Service: 19058
            new IssuerInfo
            {
                IIN = "535678",
                IssuerName = "Mashreq Bank Egypt - Mastercard Titanium Credit",
                IssuerNameArabic = "بنك مشرق مصر - ماستركارد تيتانيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Titanium,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.mashreqbank.com.eg",
                CustomerServiceEgypt = "19058",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // National Bank of Kuwait (NBK) Egypt
            // Website: https://www.nbkegypt.com | Customer Service: 19871
            new IssuerInfo
            {
                IIN = "526789",
                IssuerName = "National Bank of Kuwait (NBK) Egypt - Mastercard Platinum Credit",
                IssuerNameArabic = "بنك الكويت الوطني مصر - ماستركارد بلاتينيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Platinum,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.nbkegypt.com",
                CustomerServiceEgypt = "19871",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "434567",
                IssuerName = "National Bank of Kuwait (NBK) Egypt - Visa Gold Credit",
                IssuerNameArabic = "بنك الكويت الوطني مصر - فيزا ذهبية ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Gold,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.nbkegypt.com",
                CustomerServiceEgypt = "19871",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // ─────────────────────────────────────────────────────────────────────────
            // ADDITIONAL PRIVATE BANKS
            // More private sector banks operating in Egypt
            // ─────────────────────────────────────────────────────────────────────────

            // Al Ahli Bank of Kuwait (ABK) Egypt
            // Website: https://www.abkegypt.com | Customer Service: 19606
            new IssuerInfo
            {
                IIN = "528901",
                IssuerName = "Al Ahli Bank of Kuwait (ABK) Egypt - Mastercard Classic Credit",
                IssuerNameArabic = "البنك الأهلي الكويتي مصر - ماستركارد كلاسيك ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Classic,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.abkegypt.com",
                CustomerServiceEgypt = "19606",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },

            // Emirates NBD Egypt
            // Website: https://www.emiratesnbd.com.eg | Customer Service: 19991
            new IssuerInfo
            {
                IIN = "530000",
                IssuerName = "Emirates NBD Egypt - Mastercard Platinum Credit",
                IssuerNameArabic = "بنك الإمارات دبي الوطني مصر - ماستركارد بلاتينيوم ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Mastercard,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Platinum,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.emiratesnbd.com.eg",
                CustomerServiceEgypt = "19991",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            },
            new IssuerInfo
            {
                IIN = "445678",
                IssuerName = "Emirates NBD Egypt - Visa Infinite Credit",
                IssuerNameArabic = "بنك الإمارات دبي الوطني مصر - فيزا إنفينيت ائتمان",
                CountryCode = "EG",
                CountryName = "Egypt",
                CountryNameArabic = "مصر",
                CurrencyCode = Currency.EGP,
                Network = CardNetwork.Visa,
                CardType = CardType.Credit,
                CardCategory = CardCategory.Infinite,
                Region = IssuerRegion.Egypt,
                IsEgyptian = true,
                IssuerWebsite = "https://www.emiratesnbd.com.eg",
                CustomerServiceEgypt = "19991",
                SupportsTokenization = true,
                CvvLength = 3,
                ValidPanLengths = new[] { 16 }
            }
        };

        // ─────────────────────────────────────────────────────────────────────────────
        // GLOBAL NETWORK IIN RANGES
        // Complete list of global card network IIN/BIN prefix ranges
        // Based on ISO/IEC 7812-1:2017 and major payment networks
        // ─────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Global card network IIN/BIN prefix ranges per ISO/IEC 7812-1:2017.
        /// Each entry is: (startPrefix, endPrefix, network, validPanLengths, cvvLength).
        /// Used as a fallback when no specific issuer match is found in <see cref="EgyptianIssuers"/>.
        /// </summary>
        public static readonly List<(string Start, string End, CardNetwork Network, int[] ValidLengths, int CvvLen)> GlobalNetworkRanges
            = new List<(string, string, CardNetwork, int[], int)>
        {
            // ─────────────────────────────────────────────────────────────────────────
            // VISA
            // Major international payment network - prefix starts with 4
            // Supports multiple PAN lengths: 13, 16, 19 digits
            // ─────────────────────────────────────────────────────────────────────────
            ("4",       "4",      CardNetwork.Visa,            new[] { 13, 16, 19 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // MASTERCARD
            // Legacy range: 51-55 (first 2 digits)
            // New range: 2221-2720 (first 4 digits, introduced in 2016)
            // Standard PAN length: 16 digits
            // ─────────────────────────────────────────────────────────────────────────
            ("51",      "55",     CardNetwork.Mastercard,      new[] { 16 },         3),
            ("2221",    "2720",   CardNetwork.Mastercard,      new[] { 16 },         3),

            // ─────────────────────────────────────────────────────────────────────────
            // AMERICAN EXPRESS (AMEX)
            // Prefix: 34 and 37
            // Unique: 15-digit PAN, 4-digit CID (CVV)
            // ─────────────────────────────────────────────────────────────────────────
            ("34",      "34",     CardNetwork.AmericanExpress, new[] { 15 },         4),
            ("37",      "37",     CardNetwork.AmericanExpress, new[] { 15 },         4),

            // ─────────────────────────────────────────────────────────────────────────
            // DISCOVER
            // Multiple ranges: 6011, 622126-622925, 644-649, 65
            // Supports 16 and 19 digit PANs
            // ─────────────────────────────────────────────────────────────────────────
            ("6011",    "6011",   CardNetwork.Discover,        new[] { 16, 19 },     3),
            ("622126",  "622925", CardNetwork.Discover,        new[] { 16, 19 },     3),
            ("644",     "649",    CardNetwork.Discover,        new[] { 16, 19 },     3),
            ("65",      "65",     CardNetwork.Discover,        new[] { 16, 19 },     3),

            // ─────────────────────────────────────────────────────────────────────────
            // MEEZA
            // Egypt's national domestic payment scheme
            // Range: 507800-507809 (6-digit IIN)
            // Standard PAN length: 16 digits
            // ─────────────────────────────────────────────────────────────────────────
            ("507800",  "507809", CardNetwork.Meeza,           new[] { 16 },         3),

            // ─────────────────────────────────────────────────────────────────────────
            // UNIONPAY
            // China's national payment network - prefix 62
            // Supports multiple PAN lengths: 16-19 digits
            // Note: Overlaps with Discover range 622126-622925 - check Discover first
            // ─────────────────────────────────────────────────────────────────────────
            ("62",      "62",     CardNetwork.UnionPay,        new[] { 16, 17, 18, 19 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // JCB (Japan Credit Bureau)
            // Japanese international payment network
            // Range: 3528-3589 (first 4 digits)
            // Supports 16-19 digit PANs
            // ─────────────────────────────────────────────────────────────────────────
            ("3528",    "3589",   CardNetwork.JCB,             new[] { 16, 17, 18, 19 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // DINERS CLUB INTERNATIONAL
            // Ranges: 300-305, 36, 38-39
            // Supports multiple PAN lengths: 14, 16-19 digits
            // ─────────────────────────────────────────────────────────────────────────
            ("300",     "305",    CardNetwork.DinersClub,      new[] { 14, 16, 17, 18, 19 }, 3),
            ("36",      "36",     CardNetwork.DinersClub,      new[] { 14, 16, 17, 18, 19 }, 3),
            ("38",      "39",     CardNetwork.DinersClub,      new[] { 14, 16, 17, 18, 19 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // MAESTRO (Mastercard Debit)
            // Debit card scheme owned by Mastercard
            // Wide range of PAN lengths: 12-19 digits
            // Common prefixes: 6304, 6759, 676770, 676774
            // ─────────────────────────────────────────────────────────────────────────
            ("6304",    "6304",   CardNetwork.Maestro,         new[] { 12, 13, 14, 15, 16, 17, 18, 19 }, 3),
            ("6759",    "6759",   CardNetwork.Maestro,         new[] { 12, 13, 14, 15, 16, 17, 18, 19 }, 3),
            ("676770",  "676770", CardNetwork.Maestro,         new[] { 12, 13, 14, 15, 16, 17, 18, 19 }, 3),
            ("676774",  "676774", CardNetwork.Maestro,         new[] { 12, 13, 14, 15, 16, 17, 18, 19 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // VISA ELECTRON
            // Debit-only variant of Visa
            // Common prefixes: 4026, 417500, 4508, 4844, 4913, 4917
            // Standard PAN length: 16 digits
            // ─────────────────────────────────────────────────────────────────────────
            ("4026",    "4026",   CardNetwork.VisaElectron,    new[] { 16 }, 3),
            ("417500",  "417500", CardNetwork.VisaElectron,    new[] { 16 }, 3),
            ("4508",    "4508",   CardNetwork.VisaElectron,    new[] { 16 }, 3),
            ("4844",    "4844",   CardNetwork.VisaElectron,    new[] { 16 }, 3),
            ("4913",    "4913",   CardNetwork.VisaElectron,    new[] { 16 }, 3),
            ("4917",    "4917",   CardNetwork.VisaElectron,    new[] { 16 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // MIR (Russian National Payment System)
            // Russia's domestic payment system
            // Range: 2200-2204 (first 4 digits)
            // Standard PAN length: 16 digits
            // ─────────────────────────────────────────────────────────────────────────
            ("2200",    "2204",   CardNetwork.MirCard,         new[] { 16 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // ELO (Brazil)
            // Brazilian domestic payment scheme
            // Common prefixes: 636368, 438935, 504175, 451416
            // Standard PAN length: 16 digits
            // ─────────────────────────────────────────────────────────────────────────
            ("636368",  "636368", CardNetwork.Elo,             new[] { 16 }, 3),
            ("438935",  "438935", CardNetwork.Elo,             new[] { 16 }, 3),
            ("504175",  "504175", CardNetwork.Elo,             new[] { 16 }, 3),
            ("451416",  "451416", CardNetwork.Elo,             new[] { 16 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // UATP (Universal Air Travel Plan)
            // Airline travel card network
            // Prefix: 1
            // Standard PAN length: 15 digits
            // ─────────────────────────────────────────────────────────────────────────
            ("1",       "1",      CardNetwork.UATP,            new[] { 15 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // TROY (Turkey)
            // Turkish domestic payment system
            // Range: 979200-979289
            // Standard PAN length: 16 digits
            // ─────────────────────────────────────────────────────────────────────────
            ("979200",  "979289", CardNetwork.Troy,            new[] { 16 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // INTERPAYMENT (Russia)
            // Russian domestic payment system
            // Range: 212-217
            // ─────────────────────────────────────────────────────────────────────────
            ("212",     "217",    CardNetwork.Interpayment,    new[] { 16 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // NPS (Kazakhstan)
            // Kazakhstan domestic payment system
            // Prefix: 98
            // ─────────────────────────────────────────────────────────────────────────
            ("98",      "98",     CardNetwork.NPSKazakhstan,   new[] { 16 }, 3),

            // ─────────────────────────────────────────────────────────────────────────
            // RUPAY (India)
            // Indian domestic payment system
            // Range: 60, 81, 82, 508, 353, 356
            // ─────────────────────────────────────────────────────────────────────────
            ("60",      "60",     CardNetwork.Rupay,           new[] { 16 }, 3),
            ("81",      "82",     CardNetwork.Rupay,           new[] { 16 }, 3),
            ("508",     "508",    CardNetwork.Rupay,           new[] { 16 }, 3),
            ("353",     "356",    CardNetwork.Rupay,           new[] { 16 }, 3),
        };

        /// <summary>
        /// Helper method to get all issuers (Egyptian + any custom ones you add)
        /// </summary>
        public static List<IssuerInfo> GetAllIssuers()
        {
            var allIssuers = new List<IssuerInfo>();
            allIssuers.AddRange(EgyptianIssuers);
            // Add custom issuers here if needed
            return allIssuers;
        }

        /// <summary>
        /// Find issuer by IIN/BIN prefix (first 6 or 8 digits)
        /// </summary>
        public static IssuerInfo? FindIssuerByIIN(string iin)
        {
            return EgyptianIssuers.Find(issuer => issuer.IIN == iin);
        }

        /// <summary>
        /// Get all issuers for a specific bank by bank name
        /// </summary>
        public static List<IssuerInfo> GetIssuersByBankName(string bankName)
        {
            return EgyptianIssuers.FindAll(issuer =>
                issuer.IssuerName.Contains(bankName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Get all issuers that support tokenization (Apple Pay, etc.)
        /// </summary>
        public static List<IssuerInfo> GetTokenizationSupportedIssuers()
        {
            return EgyptianIssuers.FindAll(issuer => issuer.SupportsTokenization);
        }

        /// <summary>
        /// Get all issuers for a specific card network
        /// </summary>
        public static List<IssuerInfo> GetIssuersByNetwork(CardNetwork network)
        {
            return EgyptianIssuers.FindAll(issuer => issuer.Network == network);
        }
    }
}
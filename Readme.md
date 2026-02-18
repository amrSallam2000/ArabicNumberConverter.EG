# NumericValidation.EG

### A Comprehensive .NET Library for Egyptian Numbers & Payment Card Validation

[![NuGet](https://img.shields.io/nuget/v/NumericValidation.EG?color=blue&label=NuGet)](https://www.nuget.org/packages/NumericValidation.EG)
[![.NET](https://img.shields.io/badge/.NET-6%2B-purple)](https://dotnet.microsoft.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![ISO/IEC 7812](https://img.shields.io/badge/Standard-ISO%2FIEC%207812--1%3A2017-orange)](https://www.iso.org/standard/70484.html)
[![PCI-DSS](https://img.shields.io/badge/Compliance-PCI--DSS%20v4.0-red)](https://www.pcisecuritystandards.org)

**Built with ❤️ for the Egyptian developer community**

[📦 NuGet Package](#-installation) · [📖 API Reference](#-api-reference) · [🧪 Test Data](#-test-data) · [🔗 Integration Examples](#-integration-examples) · [🐛 Issues](https://github.com/amrSallam2000/ArabicNumberConverter.EG)


> **Developer:** Amr Sallam &nbsp;|&nbsp; [GitHub](https://github.com/amrSallam2000/ArabicNumberConverter.EG) · [LinkedIn](https://www.linkedin.com/in/amr-sallam-a8363322a/)



---

## 📋 Overview

**NumericValidation.EG** is a production-ready .NET library purpose-built for the Egyptian market. It provides structured validation, parsing, and analysis for six core domains:

| Module | Description |
|--------|-------------|
| 💳 **Bank Card Validation** | Full PAN analysis — Luhn, network detection, 30+ Egyptian banks, bilingual output |
| 🪪 **National ID** | Parse and validate 14-digit Egyptian national IDs with full demographic extraction |
| 📱 **Phone Numbers** | Validate all Egyptian mobile carriers with service-type detection |
| 🏷️ **Barcodes** | Decode EAN-13, weight, and price barcodes for the Egyptian retail market |
| 🔢 **Number to Words** | Convert numbers to Arabic/English words with proper grammar and 20+ currencies |
| 🔤 **NumberConversionHelper** | Arabic ↔ Western numeral conversion, text normalization, and formatting utilities |

---

## 📚 Table of Contents

- [✨ Features](#-features)
- [📦 Installation](#-installation)
- [🚀 Quick Start](#-quick-start)
- [💳 Bank Card Validation](#-bank-card-validation)
- [🪪 National ID Validation](#-national-id-validation)
- [📱 Phone Number Validation](#-phone-number-validation)
- [🏷️ Barcode Parsing](#️-barcode-parsing)
- [🔢 Number to Words](#-number-to-words)
- [🔤 NumberConversionHelper](#-numberconversionhelper)
- [🎯 Advanced Usage](#-advanced-usage)
- [📊 Performance](#-performance)
- [🔧 API Reference](#-api-reference)
- [🧪 Test Data](#-test-data)
- [🔗 Integration Examples](#-integration-examples)
- [⚠️ Limitations](#️-limitations)
- [🔍 Troubleshooting](#-troubleshooting)
- [📄 License](#-license)
- [🆘 Support](#-support)

---

## ✨ Features

### 💳 Bank Card Validation (v1.0.3+)
- **Full PAN validation** with Luhn algorithm (ISO/IEC 7812-1:2017 §6)
- **Network detection** for Visa, Mastercard, Amex, Meeza, UnionPay, JCB, Discover, Diners Club, Maestro, and **30+ global networks**
- Complete **Egyptian bank database** — all banks licensed by the Central Bank of Egypt (CBE)
- **Meeza** (Egypt's national payment network) fully supported — IIN range `507800–507809`
- Full **bilingual output** (English + Arabic) for all fields — network name, issuer, card type, category, country
- **Tokenization detection** — CBE programme, Apple Pay, Google Pay
- **PCI-DSS v4.0 compliant** — automatic PAN masking, never logs full PAN
- **Expiry validation** — MM/YY, MM/YYYY with live expiration checking
- **CVV validation** — 3-digit standard, 4-digit CID for Amex
- **Cardholder name validation** — ISO/IEC 7813 format (Latin, 2–26 chars)
- **Step-by-step Luhn breakdown** for education and debugging
- **Test card generation** — Luhn-valid numbers for any IIN prefix
- **Thread-safe** — `ConcurrentDictionary` cache, `ThreadLocal` per-thread validators
- **Zero NuGet dependencies** — pure BCL only

### 🪪 National ID
- Validates 14-digit Egyptian national IDs
- Extracts birth date, age, gender, governorate, generation, century, zodiac sign
- Arabic and English output with batch processing and statistics

### 📱 Phone Numbers
- Validates all Egyptian mobile carriers: Vodafone, Orange, Etisalat, WE Telecom, Special Services
- Carrier and service-type detection (Banking, Fawry, Ride Hailing, etc.)
- Supports local and international (`+20`) formats

### 🏷️ Barcodes
- Auto-detects EAN-13, EAN-8, UPC-A, weight barcodes, and price barcodes
- Extracts product code, weight, price, and country from Egyptian retail formats
- Batch processing with statistics

> **⚠️ Note on barcode type detection:** The auto-detection heuristic works well for Egyptian retail formats, but for production systems dealing with ambiguous prefixes it is recommended to specify the barcode type explicitly rather than relying solely on auto-detection.

### 🔢 Number to Words
- Arabic with correct grammar (singular, dual, plural) and 20+ currencies including EGP, USD, EUR, SAR, AED, KWD
- English conversion with fraction support (piasters, cents, fils)
- Handles negative numbers and detailed breakdown output

### 🔤 NumberConversionHelper
- Arabic ↔ Western numeral conversion with extension methods
- Arabic text normalization (diacritics, tatweel, hamza unification)
- Number and text extraction from mixed Arabic/English content
- Phone, national ID, and monetary amount formatting utilities

---

## 📦 Installation

```xml
<!-- Package Manager Console -->
Install-Package NumericValidation.EG -Version 1.0.3

<!-- .NET CLI -->
dotnet add package NumericValidation.EG --version 1.0.3

<!-- .csproj -->
<PackageReference Include="NumericValidation.EG" Version="1.0.3" />
```

**Target frameworks:** .NET 6, .NET 7, .NET 8+, .NET 9

**No external NuGet dependencies** — all modules rely exclusively on `System.*` from the BCL.

---

## 🚀 Quick Start

```csharp
using NumericValidation.EG.Models.BankCard.Extensions;
using NumericValidation.EG.Models.BankCard.Enums;
using NumericValidation.EG.Models.NationalNumber;
using NumericValidation.EG.Models.PhoneNumber;
using NumericValidation.EG.Models.BarcodeNumber;
using NumericValidation.EG.Models.NumbersToText;
using NumericValidation.EG.Models.Helper;

// Bank card — one-liner extension methods
bool valid      = "4111111111111111".IsValidCard();
string network  = "4111111111111111".GetCardNetworkName();               // "Visa"
string networkAr= "4111111111111111".GetCardNetworkName(DisplayLanguage.Arabic); // "فيزا"
bool isEgyptian = "5078031234567890".IsEgyptianCard();                   // true
string masked   = "4111111111111111".MaskCard();                         // "4111 •••• •••• 1111"
string lastFour = "4111111111111111".GetLastFour();                      // "1111"
string formatted= "4111111111111111".FormatCard();                       // "4111 1111 1111 1111"

// National ID
var idInfo = new NationalIdParser().Parse("30003098800631");
Console.WriteLine($"{idInfo.Gender} | {idInfo.GovernorateNameEnglish} | Age {idInfo.AgeYears}");

// Phone
var phoneInfo = new PhoneNumberValidator().Validate("01012345678");
Console.WriteLine($"{phoneInfo.Carrier} | {phoneInfo.ServiceType}");

// Barcode
var barcodeInfo = new BarcodeParser().Parse("2123450123405");
Console.WriteLine($"Weight: {barcodeInfo.Weight} kg");

// Number to words
string words = NumberToWordsConverter.Convert(1234.56m, Language.Arabic, Currency.EGP);
// "ألف ومئتان وأربعة وثلاثون جنيهًا مصريًا وستة وخمسون قرشًا"

// NumberConversionHelper
string arabicNum = "12345".ToArabicNumerals(); // "١٢٣٤٥"
```

---

## 💳 Bank Card Validation

### Basic Usage

```csharp
using NumericValidation.EG.Models.BankCard.Validator;
using NumericValidation.EG.Models.BankCard.Enums;
using NumericValidation.EG.Models.BankCard.Extensions;

var validator = new BankCardValidator(DisplayLanguage.English);
var info = validator.Analyze("4111111111111111");

Console.WriteLine($"Valid:         {info.IsValid}");           // true
Console.WriteLine($"Network:       {info.NetworkName}");       // Visa
Console.WriteLine($"Card Type:     {info.CardTypeName}");      // Credit
Console.WriteLine($"Category:      {info.CardCategoryName}");  // Classic
Console.WriteLine($"Issuer:        {info.IssuerName}");        // Unknown Issuer
Console.WriteLine($"Country:       {info.CountryName}");       // United States
Console.WriteLine($"Masked PAN:    {info.MaskedNumber}");      // 4111 •••• •••• 1111
Console.WriteLine($"Formatted PAN: {info.FormattedNumber}");   // 4111 1111 1111 1111
Console.WriteLine($"IIN:           {info.IIN}");               // 411111
Console.WriteLine($"Last Four:     {info.LastFourDigits}");    // 1111
Console.WriteLine($"CVV Length:    {info.CvvLength}");         // 3
Console.WriteLine($"Luhn Valid:    {info.IsLuhnValid}");       // true
Console.WriteLine($"Length Valid:  {info.IsLengthValid}");     // true
```

### Arabic Output

```csharp
var validator = new BankCardValidator(DisplayLanguage.Arabic);
var info = validator.Analyze("5078031234567890"); // NBE Meeza

Console.WriteLine($"صالح:            {info.IsValid}");              // true
Console.WriteLine($"الشبكة:          {info.NetworkName}");          // ميزة
Console.WriteLine($"نوع البطاقة:     {info.CardTypeName}");         // خصم مباشر
Console.WriteLine($"الفئة:           {info.CardCategoryName}");     // كلاسيك
Console.WriteLine($"البنك:           {info.IssuerName}");           // البنك الأهلي المصري
Console.WriteLine($"الدولة:          {info.CountryName}");          // مصر
Console.WriteLine($"خدمة العملاء:   {info.CustomerServiceEgypt}"); // 19623
Console.WriteLine($"يدعم التوكنيزيشن: {info.SupportsTokenization}"); // True

// Both languages are always populated — switch without re-analysing
Console.WriteLine(info.NetworkNameEn); // Meeza
Console.WriteLine(info.NetworkNameAr); // ميزة
Console.WriteLine(info.IssuerNameEn);  // National Bank of Egypt (NBE) - Meeza Debit
Console.WriteLine(info.IssuerNameAr);  // البنك الأهلي المصري - ميزة خصم مباشر
```

### Full Validation (Expiry + CVV + Name)

```csharp
var validator = new BankCardValidator(DisplayLanguage.English);
var info = validator.AnalyzeFull(
    cardNumber:     "4111111111111111",
    expiry:         "12/28",
    cvv:            "123",
    cardholderName: "AHMED MOHAMED"
);

if (info.IsValid)
{
    Console.WriteLine("✅ Card approved for transaction");
}
else
{
    Console.WriteLine($"❌ {info.ValidationMessage}");
    Console.WriteLine($"   Reason: {info.FailureReason}"); // e.g. CardExpired, InvalidCVV
}

// Individual field results
Console.WriteLine($"Expiry valid:  {info.IsExpiryValid}");  // true / false / null
Console.WriteLine($"Expired:       {info.IsExpired}");       // true / false / null
Console.WriteLine($"CVV valid:     {info.IsCvvValid}");      // true / false / null
Console.WriteLine($"Name valid:    {info.IsCardholderNameValid}"); // true / false / null
```

**Accepted expiry formats:** `MM/YY`, `MM/YYYY`, `MMYY`, `MMYYYY`, `MM-YY`, `MM-YYYY`, `MM YY`, `MM YYYY`

### Luhn Algorithm

```csharp
using NumericValidation.EG.Models.BankCard;

// Quick check
bool ok  = LuhnAlgorithm.IsValid("4111 1111 1111 1111"); // true (spaces stripped)
bool bad = LuhnAlgorithm.IsValid("4111111111111112");     // false

// Step-by-step breakdown (useful for debugging / teaching)
var steps = LuhnAlgorithm.GetDetailedSteps("4111111111111111");
Console.WriteLine($"Total Sum: {steps.TotalSum}");  // 20
Console.WriteLine($"Valid:     {steps.IsValid}");   // true
Console.WriteLine($"Check Digit: {steps.CheckDigit}"); // 1

foreach (var step in steps.Steps)
{
    Console.WriteLine(
        $"[{step.Position,2}] digit={step.OriginalDigit} " +
        $"{(step.Doubled ? $"×2={step.ProcessedValue}" : $"   ={step.ProcessedValue}")} " +
        $"(sum={step.RunningSum})");
}

// Compute check digit for a partial PAN
int cd = LuhnAlgorithm.CalculateCheckDigit("411111111111111"); // 1
```

### Test Card Generation

```csharp
// ⚠️ For development / testing ONLY — not real account numbers

// Meeza (Egyptian national scheme)
string meeza = LuhnAlgorithm.GenerateTestCardNumber("507803", totalLength: 16);
// e.g. "5078031234567891" — Luhn-valid

// Visa
string visa = LuhnAlgorithm.GenerateTestCardNumber("411111", totalLength: 16);

// American Express (15 digits)
string amex = LuhnAlgorithm.GenerateTestCardNumber("34", totalLength: 15);
```

### Logging Integration

```csharp
// Development — console output
var validator = new BankCardValidator(
    language: DisplayLanguage.English,
    logger:   new ConsoleBankCardLogger()
);
// Prints: [BankCard] VALID ✅ | Network: Visa                 | PAN: 4111 •••• •••• 1111

// Production — custom logger (Serilog, Application Insights, etc.)
public class AppInsightsCardLogger : IBankCardLogger
{
    private readonly TelemetryClient _telemetry;
    public AppInsightsCardLogger(TelemetryClient tc) => _telemetry = tc;

    public void LogValidation(
        string maskedPan, bool isValid,
        string networkName, ValidationFailureReason failureReason)
    {
        _telemetry.TrackEvent("CardValidation", new Dictionary<string, string>
        {
            ["MaskedPan"]     = maskedPan,   // Never the full PAN
            ["IsValid"]       = isValid.ToString(),
            ["Network"]       = networkName,
            ["FailureReason"] = failureReason.ToString()
        });
    }

    public void LogError(Exception ex, string maskedPan)
        => _telemetry.TrackException(ex, new Dictionary<string, string>
            { ["MaskedPan"] = maskedPan });
}

// No logging (default — zero overhead)
var silent = new BankCardValidator(); // NullBankCardLogger.Instance used internally
```

### 🏦 Egyptian Bank Database

All 30+ Egyptian banks licensed by the Central Bank of Egypt (CBE):

| Bank | IIN Prefixes | Customer Service | Tokenization |
|------|-------------|------------------|:------------:|
| **National Bank of Egypt (NBE)** | 428541, 404906, 512345, 524567, 507803 | 19623 | ✅ |
| **Banque Misr** | 489737, 522081, 507800 | 19888 | ✅ |
| **Banque du Caire** | 413579, 529948, 507806 | 19819 | ✅ |
| **Commercial International Bank (CIB)** | 455676, 557368, 507804 | 19666 | ✅ |
| **QNB Al Ahli** | 431493, 525678, 507805 | 19700 | ✅ |
| **AAIB** | 446789, 552481, 507807 | 16333 | ✅ |
| **Bank of Alexandria** | 410441, 531741, 507801 | 19033 | ✅ |
| **Crédit Agricole Egypt** | 412851, 545502, 507802 | 19191 | ✅ |
| **HSBC Egypt** | 447710, 549876 | 19007 | ✅ |
| **Faisal Islamic Bank** | 410987, 540000 | 19628 | ✅ |
| **ADIB Egypt** | 536789, 507808 | 19977 | ✅ |
| **Al Baraka Bank** | 426543, 543000 | 16993 | ✅ |
| **NBK Egypt** | 526789, 434567 | 19871 | ✅ |
| **Emirates NBD Egypt** | 530000, 445678 | 19991 | ✅ |
| **FAB Egypt** | 520000, 423456 | 16661 | ✅ |
| **Mashreq Bank Egypt** | 535678 | 19058 | ✅ |
| **Citi Egypt** | 374000, 401234 | 19234 | ✅ |
| **Fawry** | 539542, 506789 | 19350 | ❌ |
| **Paymob** | 508000 | 19976 | ✅ |
| **MoneyFellows** | 509000 | 16060 | ✅ |
| *(and more…)* | | | |

> Detection priority: Egyptian issuer exact match → Global network range → Unknown.

### 🌐 Global Network Coverage

| Network | IIN / BIN Range | PAN Lengths | CVV |
|---------|----------------|-------------|:---:|
| **Visa** | 4x | 13, 16, 19 | 3 |
| **Mastercard** | 51–55, 2221–2720 | 16 | 3 |
| **American Express** | 34, 37 | 15 | **4** |
| **Discover** | 6011, 622126–622925, 644–649, 65 | 16, 19 | 3 |
| **Meeza** 🇪🇬 | 507800–507809 | 16 | 3 |
| **UnionPay** | 62 | 16–19 | 3 |
| **JCB** | 3528–3589 | 16–19 | 3 |
| **Diners Club** | 300–305, 36, 38–39 | 14, 16–19 | 3 |
| **Maestro** | 6304, 6759, 676770, 676774 | 12–19 | 3 |
| **Visa Electron** | 4026, 417500, 4508, 4844, 4913, 4917 | 16 | 3 |
| **MIR** 🇷🇺 | 2200–2204 | 16 | 3 |
| **Elo** 🇧🇷 | 636368, 438935, 504175, 451416 | 16 | 3 |
| **Troy** 🇹🇷 | 979200–979289 | 16 | 3 |
| **UATP** | 1 | 15 | 3 |
| **RuPay** 🇮🇳 | 60, 81, 82, 508, 353, 356 | 16 | 3 |
| *(30+ more…)* | | | |

---

## 🪪 National ID Validation

```csharp
using NumericValidation.EG.Models.NationalNumber;

var parser = new NationalIdParser();
var id = parser.Parse("30003098800631");

if (id.IsValid)
{
    Console.WriteLine($"Birth Date:     {id.Day}/{id.Month}/{id.Year}");
    Console.WriteLine($"Age:            {id.AgeYears} years, {id.AgeMonths} months");
    Console.WriteLine($"Gender:         {id.Gender}");                   // Male
    Console.WriteLine($"Governorate:    {id.GovernorateNameEnglish}");   // Cairo
    Console.WriteLine($"Governorate AR: {id.GovernorateNameArabic}");    // القاهرة
    Console.WriteLine($"Zodiac:         {id.ZodiacSignEnglish}");        // Pisces
    Console.WriteLine($"Century:        {id.Century}");                  // 21st
    Console.WriteLine($"Birth Date AR:  {id.BirthDateFullArabic}");
    Console.WriteLine($"Age AR:         {id.AgeTextArabic}");
}

// Batch with statistics
var ids = new[] { "30003098800631", "29505211234567", "28407010101234" };
var batchParser = new NationalIdParser(ids);
var results = batchParser.ParseAll();
var stats = batchParser.GetStatistics();

Console.WriteLine($"Valid:       {stats["Valid"]}");
Console.WriteLine($"Success Rate:{stats["SuccessRate"]}%");
Console.WriteLine($"Average Age: {stats["AverageAge"]} years");
Console.WriteLine($"Gender:      {stats["MaleCount"]}M / {stats["FemaleCount"]}F");
```

### Governorate Codes

| Code | Governorate (English) | Governorate (Arabic) |
|------|----------------------|----------------------|
| 01 | Cairo | القاهرة |
| 02 | Alexandria | الإسكندرية |
| 03 | Port Said | بورسعيد |
| 04 | Suez | السويس |
| 11 | Damietta | دمياط |
| 12 | Dakahlia | الدقهلية |
| 13 | Sharkia | الشرقية |
| 14 | Qalyubia | القليوبية |
| 15 | Kafr El Sheikh | كفر الشيخ |
| 16 | Gharbia | الغربية |
| 17 | Monufia | المنوفية |
| 18 | Beheira | البحيرة |
| 19 | Ismailia | الإسماعيلية |
| 21 | Giza | الجيزة |
| 22 | Beni Suef | بني سويف |
| 23 | Fayoum | الفيوم |
| 24 | Minya | المنيا |
| 25 | Asyut | أسيوط |
| 26 | Sohag | سوهاج |
| 27 | Qena | قنا |
| 28 | Aswan | أسوان |
| 29 | Luxor | الأقصر |
| 31 | Red Sea | البحر الأحمر |
| 32 | New Valley | الوادي الجديد |
| 33 | Matrouh | مطروح |
| 34 | North Sinai | شمال سيناء |
| 35 | South Sinai | جنوب سيناء |

---

## 📱 Phone Number Validation

```csharp
using NumericValidation.EG.Models.PhoneNumber;

var validator = new PhoneNumberValidator();
var phone = validator.Validate("01012345678");

Console.WriteLine($"Valid:         {phone.IsValid}");
Console.WriteLine($"Carrier:       {phone.Carrier}");         // Vodafone
Console.WriteLine($"Service Type:  {phone.ServiceType}");     // Standard
Console.WriteLine($"Formatted:     {phone.FormattedNumber}"); // 010 123 45678

// International format
var phoneIntl = validator.Validate("+201012345678");
Console.WriteLine(phoneIntl.Carrier); // Vodafone

// Batch + carrier grouping
var numbers = new[] { "01012345678", "01112345678", "01501234567" };
var batch = new PhoneNumberValidator(numbers);
var grouped = batch.GroupByCarrier();
foreach (var g in grouped)
    Console.WriteLine($"{g.Key}: {g.Value.Count} number(s)");

var summary = batch.GetValidationSummary();
Console.WriteLine($"Success Rate: {summary["SuccessRate"]}%");
```

**Supported carriers:**

| Carrier | Prefixes | Service Types |
|---------|---------|---------------|
| Vodafone | 0100–0109 | Standard, Banking, Fawry, Ride Hailing |
| Orange | 0110–0113, 0120–0129 | Standard, Banking, Fawry |
| Etisalat | 0114–0119 | Standard, Banking, Fawry |
| WE Telecom | 0150–0159 | Standard, Banking |
| Special Services | 0190–0199 | Fawry, Banking, Ride Hailing |

---

## 🏷️ Barcode Parsing

```csharp
using NumericValidation.EG.Models.BarcodeNumber;

var parser = new BarcodeParser();
var barcode = parser.Parse("2123450123405");

Console.WriteLine($"Type:         {barcode.TypeNameEnglish}"); // Weight Barcode
Console.WriteLine($"Product Code: {barcode.ProductCode}");     // 12345
Console.WriteLine($"Weight:       {barcode.Weight} kg");       // 1.234 kg
Console.WriteLine($"Country:      {barcode.CountryNameEnglish}"); // Egypt

var priceBarcode = parser.Parse("2678901250008");
Console.WriteLine($"Price: {priceBarcode.Price} EGP"); // 125.00 EGP

// Explicit type parsing
var weightBarcode = BarcodeParser.ParseWeight("2123450123405");
var standardBarcode = BarcodeParser.ParseStandard("6221234567890");
```

> **ℹ️ Production tip:** Auto-detection is reliable for standard Egyptian retail formats. For mixed or custom formats, specify the barcode type explicitly to avoid ambiguous detection results.

**Supported formats:**

| Format | Pattern | Example | Description |
|--------|---------|---------|-------------|
| EAN-13 | 13 digits | 6221234567890 | Standard product barcode |
| EAN-8 | 8 digits | 12345670 | Short barcode |
| UPC-A | 12 digits | 123456789012 | US/Canada standard |
| Weight | 2XXXXXYYYYYZZ | 2123450123405 | Product with weight (1.234 kg) |
| Price | 2XXXXXYYYYYZZ | 2678901250008 | Product with price (125.00 EGP) |

---

## 🔢 Number to Words

```csharp
using NumericValidation.EG.Models.NumbersToText;

// Arabic with correct grammar
string ar = NumberToWordsConverter.Convert(1234.56m, Language.Arabic, Currency.EGP);
// "ألف ومئتان وأربعة وثلاثون جنيهًا مصريًا وستة وخمسون قرشًا"

// English
string en = NumberToWordsConverter.Convert(1234.56m, Language.English, Currency.USD);
// "One Thousand Two Hundred Thirty Four Dollars and Fifty Six Cents"

// Detailed breakdown
var details = NumberToWordsConverter.ConvertWithDetails(123.45m, Language.Arabic, Currency.EGP);
Console.WriteLine($"Integer:  {details.IntegerPart}");
Console.WriteLine($"Fraction: {details.FractionalPart}");
Console.WriteLine($"Full:     {details.FullText}");

// Negative numbers
string negative = NumberToWordsConverter.Convert(-1234.56m, Language.Arabic, Currency.EGP);
// "ناقص ألف ومئتان وأربعة وثلاثون جنيهًا مصريًا وستة وخمسون قرشًا"

// Direct methods
string arabicOnly = NumberToWordsConverter.ConvertToArabic(1234.56m, Currency.EGP);
string englishOnly = NumberToWordsConverter.ConvertToEnglish(1234.56m, Currency.USD);
```

**Supported currencies:** EGP, USD, EUR, GBP, SAR, AED, KWD, QAR, BHD, OMR, JOD, LBP, SYP, IQD, YER, MAD, TND, DZD, LYD, SDG.

---

## 🔤 NumberConversionHelper

### Numeral Conversion

```csharp
using NumericValidation.EG.Models.Helper;

// Static methods
string ar = NumberConversionHelper.ToArabicNumerals("12345.678"); // "١٢٣٤٥٫٦٧٨"
string en = NumberConversionHelper.ToWesternNumerals("١٢٣٤٥٫٦٧٨"); // "12345.678"

// Extension methods (fluent API)
string result  = "12345".ToArabicNumerals();     // "١٢٣٤٥"
string amount  = (1234.56m).ToArabicNumerals();  // "١٬٢٣٤٫٥٦"
string western = "١٢٣٤٥".ToWesternNumerals();    // "12345"
```

### Arabic Text Normalization

```csharp
string unified    = NumberConversionHelper.UnifyArabicText("أحــمــدُ بنُ محمــدٍ");
// "احمد بن محمد"

string normalized = NumberConversionHelper.NormalizeArabicText("أَحْمَدُ ٢٥ سَنَةٍ");
// "احمد ٢٥ سنه"

// Full cleanup with options
var options = new TextUnificationOptions
{
    UnifyArabicText      = true,
    RemoveTashkeel       = true,
    RemoveTatweel        = true,
    UnifyNumerals        = true,
    PreferArabicNumerals = true,
    RemoveSpecialChars   = true,
    KeepSpaces           = true
};
string processed = NumberConversionHelper.ProcessText("أحــمــدُ بنُ محمــدٍ ٢٥ سنةٍ", options);
// "احمد بن محمد ٢٥ سنه"

// Extension methods
string clean = "  أحــمــد   عمــره  ٢٥  سنةٍ  ".FullClean(arabicNumerals: true);
// "احمد عمره ٢٥ سنه"
```

### Data Extraction & Detection

```csharp
string numbers  = NumberConversionHelper.ExtractNumbers("السعر: ٢٥٠.٥٠ جنيه"); // "250.50"
string textOnly = NumberConversionHelper.ExtractTextOnly("العمر: ٢٥ سنة");       // "العمر:  سنة"
string arabicText = NumberConversionHelper.ExtractArabicText("Name: أحمد - Age: ٢٥"); // "احمد"

bool hasArabicNumerals  = "العمر: ٢٥ سنة".ContainsArabicNumerals();  // true
bool hasWesternNumerals = "Age: 25 years".ContainsWesternNumerals();   // true
bool hasArabicLetters   = "أحمد محمد".ContainsArabicLetters();         // true
bool hasEnglishLetters  = "John Doe".ContainsEnglishLetters();          // true

// Auto-convert based on content
string autoResult = "العمر: ٢٥ سنة".AutoConvertNumerals(preferArabic: false);
// "العمر: 25 سنة"
```

### Formatting Utilities

```csharp
string formattedNum   = NumberConversionHelper.FormatNumberArabic(1234567.89m, 2);
// "١٬٢٣٤٬٥٦٧٫٨٩"

string formattedPhone = NumberConversionHelper.FormatPhoneArabic("01012345678");
// "٠١٠ ١٢٣ ٤٥٦٧٨"

string formattedId    = NumberConversionHelper.FormatNationalIdArabic("12345678901234");
// "١٢ ٣٤ ٥٦ ٧٨ ٩٠١ ٢٣٤"

// Clean whitespace
string cleaned = "  Hello   World  ".CleanWhitespace(); // "Hello World"

// Remove special characters
string noSpecials = "Hello@World#123".RemoveSpecialChars(); // "HelloWorld123"
```

---

## 🎯 Advanced Usage

### Bank Card Batch Analysis

```csharp
var validator = new BankCardValidator(DisplayLanguage.English);
var cards = new[]
{
    "4111111111111111",
    "5078031234567890",
    "378282246310005"
};

var results = cards.Select(c => validator.Analyze(c)).ToList();

// Filter Egyptian cards
foreach (var card in results.Where(r => r.IsEgyptian))
    Console.WriteLine($"{card.MaskedNumber} — {card.IssuerName}");

// Group by network
foreach (var group in results.GroupBy(r => r.NetworkName))
    Console.WriteLine($"{group.Key}: {group.Count()} card(s)");

// Get statistics
var egyptianCount = results.Count(r => r.IsEgyptian);
var meezaCount = results.Count(r => r.Network == CardNetwork.Meeza);
Console.WriteLine($"Egyptian cards: {egyptianCount}, Meeza cards: {meezaCount}");
```

### Payment Processing System

```csharp
public class PaymentProcessor
{
    private static readonly BankCardValidator _validator =
        new BankCardValidator(DisplayLanguage.English, logger: new ConsoleBankCardLogger());

    public async Task<PaymentResult> ProcessPayment(PaymentRequest req)
    {
        var card = _validator.AnalyzeFull(
            req.CardNumber, req.Expiry, req.CVV, req.CardholderName);

        if (!card.IsValid)
            return PaymentResult.Declined(card.ValidationMessage);

        // Route by network
        string gatewayUrl = card.Network switch
        {
            CardNetwork.Meeza      => "https://api.meeza.eg/v1/process",
            CardNetwork.Visa       => "https://api.visa.com/v1/process",
            CardNetwork.Mastercard => "https://api.mastercard.com/v1/process",
            _                      => "https://api.default-gateway.com/v1/process"
        };

        // card.MaskedNumber is safe for logs — full PAN is NEVER exposed
        _logger.LogInformation("Routing {Network} {MaskedPan} → {Gateway}",
            card.NetworkName, card.MaskedNumber, gatewayUrl);

        return await SendToGateway(gatewayUrl, req, card);
    }
}
```

### E-commerce Checkout

```csharp
public CheckoutViewModel PrepareCheckout(string cardNumber)
{
    var info = cardNumber.AnalyzeCard(DisplayLanguage.Arabic);

    return new CheckoutViewModel
    {
        MaskedCard           = info.MaskedNumber,
        NetworkLogo          = GetNetworkLogo(info.Network),
        CardTypeArabic       = info.CardTypeName,
        BankNameArabic       = info.IssuerName,
        IsEgyptianCard       = info.IsEgyptian,
        CvvLength            = info.CvvLength,
        Notes                = info.Notes      // contextual tips in Arabic
    };
}

private string GetNetworkLogo(CardNetwork network) => network switch
{
    CardNetwork.Visa           => "/img/visa.svg",
    CardNetwork.Mastercard     => "/img/mastercard.svg",
    CardNetwork.Meeza          => "/img/meeza.svg",
    CardNetwork.AmericanExpress=> "/img/amex.svg",
    _                          => "/img/card.svg"
};
```

### Government Registration System

```csharp
public void RegisterCitizen(string nationalId, string phoneNumber, string fullName)
{
    var idInfo    = NationalIdParser.ParseSingle(nationalId);
    var phoneInfo = PhoneNumberValidator.Validate(phoneNumber);

    if (!idInfo.IsValid)    throw new ArgumentException("Invalid national ID");
    if (!phoneInfo.IsValid) throw new ArgumentException("Invalid phone number");

    string normalizedName = NumberConversionHelper.UnifyArabicText(fullName);

    Console.WriteLine($"Name:       {normalizedName}");
    Console.WriteLine($"Birth Date: {idInfo.BirthDateFullArabic}");
    Console.WriteLine($"Age:        {idInfo.AgeTextArabic}");
    Console.WriteLine($"Governorate:{idInfo.GovernorateNameArabic}");
    Console.WriteLine($"Phone:      {NumberConversionHelper.FormatPhoneArabic(phoneNumber)}");
}
```

### Supermarket POS System

```csharp
public class SupermarketPOS
{
    public decimal ProcessSale(List<(string barcode, decimal pricePerKg)> items)
    {
        decimal total = 0;
        
        foreach (var item in items)
        {
            var barcodeInfo = BarcodeParser.ParseSingle(item.barcode);
            
            if (barcodeInfo.Type == BarcodeType.Weight)
            {
                decimal itemPrice = barcodeInfo.Weight * item.pricePerKg;
                total += itemPrice;
                
                Console.WriteLine($"Product: {barcodeInfo.ProductCode}");
                Console.WriteLine($"Weight: {barcodeInfo.Weight:F3} kg");
                Console.WriteLine($"Price per kg: {item.pricePerKg} EGP");
                Console.WriteLine($"Item price: {itemPrice:F2} EGP");
                Console.WriteLine($"Price in words: {NumberToWordsConverter.Convert(itemPrice, Language.Arabic, Currency.EGP)}");
            }
            else if (barcodeInfo.Type == BarcodeType.Price)
            {
                total += barcodeInfo.Price;
            }
        }
        
        return total;
    }
}
```

---

## 📊 Performance

### Thread Safety

```csharp
// BankCardValidator is safe for shared use across threads
var validator = new BankCardValidator();

Parallel.For(0, 10_000, _ =>
{
    var result = validator.Analyze("4111111111111111"); // ConcurrentDictionary cache
});

// Extension methods are safe via ThreadLocal per-thread instances
Parallel.ForEach(cardNumbers, card =>
{
    bool valid = card.IsValidCard(); // ThreadLocal<BankCardValidator>
});
```

### Intelligent Caching

The validator caches results keyed on the **sanitized PAN** (spaces/dashes stripped) so that `"4111 1111 1111 1111"` and `"4111-1111-1111-1111"` share the same cache entry. Cache eviction kicks in at 512 entries.

```csharp
var info1 = validator.Analyze("4111 1111 1111 1111"); // computed
var info2 = validator.Analyze("4111-1111-1111-1111"); // ✅ cache hit — same sanitized key
var info3 = validator.Analyze("4111111111111111");     // ✅ cache hit

validator.ClearCache(); // call after updating issuer data at runtime
```

### Throughput Benchmark

```csharp
var sw = Stopwatch.StartNew();
var validator = new BankCardValidator();
var cards = GenerateTestCards(10_000);

var results = cards.Select(c => validator.Analyze(c)).ToList();
sw.Stop();

Console.WriteLine($"10,000 cards in {sw.ElapsedMilliseconds} ms");
Console.WriteLine($"Average: {sw.ElapsedMilliseconds / 10_000.0:F3} ms/card");
```

### Memory Efficiency

```csharp
long initialMemory = GC.GetTotalMemory(true);

var validator = new BankCardValidator();
var results = Enumerable.Range(0, 5000)
    .Select(i => validator.Analyze($"411111111111{i:D4}"))
    .ToList();

GC.Collect();
long finalMemory = GC.GetTotalMemory(true);

Console.WriteLine($"Memory used: {(finalMemory - initialMemory) / 1024:N0} KB");
Console.WriteLine($"Memory per item: {(finalMemory - initialMemory) / 5000.0:F2} bytes");
```

### Best Practices

```csharp
// ✅ GOOD — reuse a single validator instance
private static readonly BankCardValidator _validator = new(DisplayLanguage.Arabic);

// ✅ GOOD — extension methods for simple checks
if (cardNumber.IsValidCard()) { ... }

// ✅ GOOD — extension method AnalyzeCard() for full info one-liners
var info = cardNumber.AnalyzeCard(DisplayLanguage.Arabic);

// ❌ BAD — new instance per call wastes memory and skips the cache
var info = new BankCardValidator().Analyze(cardNumber);
```

---

## 🔧 API Reference

### Bank Card Classes (v1.0.3+)

| Class | Description | Key Members |
|-------|-------------|-------------|
| `BankCardValidator` | Main validator | `Analyze()`, `AnalyzeFull()`, `ClearCache()` |
| `LuhnAlgorithm` | Luhn algorithm | `IsValid()`, `CalculateChecksum()`, `CalculateCheckDigit()`, `GenerateTestCardNumber()`, `GetDetailedSteps()` |
| `BankCardExtensions` | Fluent string extensions | `IsValidCard()`, `AnalyzeCard()`, `GetCardNetwork()`, `GetCardNetworkName()`, `IsEgyptianCard()`, `CardSupportsTokenization()`, `MaskCard()`, `FormatCard()`, `GetLastFour()` |
| `BankCardInfo` | Result model | All properties listed below |
| `IssuerDatabase` | BIN/IIN database | `EgyptianIssuers`, `GlobalNetworkRanges` |
| `IBankCardLogger` | Logging interface | `LogValidation()`, `LogError()` |
| `NullBankCardLogger` | Silent logger (default) | Singleton `Instance` |
| `ConsoleBankCardLogger` | Console logger | Dev/test use |
| `LuhnStepResult` | Luhn breakdown | `Steps`, `TotalSum`, `IsValid`, `CheckDigit` |

### BankCardInfo Properties

| Section | Property | Type | Description |
|---------|----------|------|-------------|
| **Input** | `RawInput` | `string` | Original input as supplied |
| | `SanitizedNumber` | `string` | After stripping spaces/dashes |
| | `FormattedNumber` | `string` | Grouped (e.g. `4111 1111 1111 1111`) |
| | `MaskedNumber` | `string` | PCI-safe (e.g. `4111 •••• •••• 1111`) |
| | `LastFourDigits` | `string` | Last 4 digits |
| | `IIN` | `string` | First 6 digits |
| | `ExtendedIIN` | `string` | First 8 digits |
| | `Length` | `int` | PAN length |
| **Validation** | `IsValid` | `bool` | Master pass/fail |
| | `IsLuhnValid` | `bool` | Luhn check result |
| | `IsLengthValid` | `bool` | Length within valid range |
| | `IsNumericOnly` | `bool` | Digit-only check |
| | `FailureReason` | `ValidationFailureReason` | Machine-readable failure code |
| | `ValidationMessageEn` | `string?` | English failure message |
| | `ValidationMessageAr` | `string?` | Arabic failure message |
| | `ValidationMessage` | `string?` | Active-language alias |
| **Network** | `Network` | `CardNetwork` | Detected network enum |
| | `NetworkNameEn` | `string` | e.g. `"Visa"` |
| | `NetworkNameAr` | `string` | e.g. `"فيزا"` |
| | `NetworkName` | `string` | Active-language alias |
| | `CardType` | `CardType` | Credit / Debit / Prepaid … |
| | `CardTypeName` | `string` | Active-language |
| | `CardCategory` | `CardCategory` | Classic / Gold / Platinum … |
| | `CardCategoryName` | `string` | Active-language |
| | `ValidPanLengths` | `int[]` | Allowed lengths |
| | `CvvLength` | `int` | 3 or 4 |
| **Issuer** | `IssuerNameEn` | `string?` | English issuer name |
| | `IssuerNameAr` | `string?` | Arabic issuer name |
| | `IssuerName` | `string?` | Active-language alias |
| | `IssuerWebsite` | `string?` | Official bank website |
| | `CustomerServiceEgypt` | `string?` | Short-dial number |
| **Geography** | `CountryCode` | `string` | ISO 3166-1 α-2 |
| | `CountryName` | `string` | Active-language |
| | `CurrencyCode` | `Currency` | ISO 4217 enum |
| | `Region` | `IssuerRegion` | Geographic region |
| | `IsEgyptian` | `bool` | CBE-licensed issuer |
| | `IsInternational` | `bool` | Non-Egyptian, non-Meeza |
| **Tokenization** | `SupportsTokenization` | `bool` | CBE / Apple Pay / Google Pay |
| | `SimulatedToken` | `string?` | Test token (opt-in) |
| **Full Validation** | `IsExpiryValid` | `bool?` | Expiry format valid |
| | `IsExpired` | `bool?` | Card past expiry month |
| | `IsCvvValid` | `bool?` | CVV digit check |
| | `IsCardholderNameValid` | `bool?` | ISO/IEC 7813 name check |
| **Display** | `DisplayLabel` | `string` | One-line description |
| | `Notes` | `List<string>` | Contextual tips (active language) |
| | `Language` | `DisplayLanguage` | Language used at analysis time |

### National ID Classes

| Class | Description | Key Members |
|-------|-------------|-------------|
| `NationalIdParser` | Parses Egyptian national IDs | `Parse()`, `ParseAll()`, `GetStatistics()` |
| `NationalIdInfo` | National ID information | `IsValid`, `BirthDate`, `AgeYears`, `Gender`, `GovernorateNameEnglish` |

### Phone Number Classes

| Class | Description | Key Members |
|-------|-------------|-------------|
| `PhoneNumberValidator` | Validates Egyptian phone numbers | `Validate()`, `ValidateAll()`, `GroupByCarrier()` |
| `PhoneNumberInfo` | Phone information | `IsValid`, `Carrier`, `ServiceType`, `FormattedNumber` |

### Barcode Classes

| Class | Description | Key Members |
|-------|-------------|-------------|
| `BarcodeParser` | Parses Egyptian barcodes | `Parse()`, `ParseWeight()`, `ParsePrice()`, `ParseAll()` |
| `BarcodeInfo` | Barcode information | `Type`, `ProductCode`, `Weight`, `Price`, `CountryNameEnglish` |

### Number to Words Classes

| Class | Description | Key Members |
|-------|-------------|-------------|
| `NumberToWordsConverter` | Converts numbers to words | `Convert()`, `ConvertToArabic()`, `ConvertToEnglish()`, `ConvertWithDetails()` |
| `ConversionDetails` | Detailed conversion result | `IntegerPart`, `FractionalPart`, `FullText` |

### NumberConversionHelper Classes

| Class | Description | Key Members |
|-------|-------------|-------------|
| `NumberConversionHelper` | Static helper methods | `ToArabicNumerals()`, `ToWesternNumerals()`, `UnifyArabicText()`, `ExtractNumbers()` |
| `TextUnificationOptions` | Text processing options | `UnifyArabicText`, `RemoveTashkeel`, `RemoveTatweel`, `UnifyNumerals` |
| `StringExtensions` | Extension methods | `ToArabicNumerals()`, `ToWesternNumerals()`, `UnifyArabic()`, `FullClean()` |

### Enumerations

| Enum | Values |
|------|--------|
| `CardNetwork` | Visa, Mastercard, AmericanExpress, Discover, UnionPay, JCB, DinersClub, **Meeza**, Maestro, MastercardDebit, VisaElectron, Verve, MirCard, Elo, UATP, Troy, Interpayment, NPS, RuPay, Hipercard, Napas, Dankort, Cartes Bancaires, and 30+ more |
| `CardType` | Credit, Debit, Prepaid, Virtual, Corporate, Government, Unknown |
| `CardCategory` | Classic, Gold, Platinum, Signature, Infinite, Business, World, WorldElite, Student, Islamic, Unknown |
| `DisplayLanguage` | English, Arabic |
| `IssuerRegion` | Egypt, MiddleEast, Europe, NorthAmerica, AsiaPacific, LatinAmerica, Africa, Global |
| `ValidationFailureReason` | None, NullOrEmpty, ContainsNonDigits, InvalidLength, LuhnCheckFailed, UnknownIIN, InvalidExpiryDate, CardExpired, InvalidCVV, InvalidCardholderName, **InternalError** |
| `Currency` | EGP, USD, EUR, GBP, SAR, AED, KWD, QAR, BHD, OMR, JOD, LBP, SYP, IQD, YER, MAD, TND, DZD, LYD, SDG |
| `Language` | Arabic, English |
| `BarcodeType` | Standard, Weight, Price |
| `Gender` | Male, Female |

---

## 🧪 Test Data

### Bank Cards

```
Egyptian Banks:
  4285411234567890   — NBE Visa Classic Debit
  4049061234567890   — NBE Visa Gold Credit
  5123451234567890   — NBE Mastercard Classic Debit
  5078031234567890   — NBE Meeza Debit
  4897371234567890   — Banque Misr Visa Classic Debit
  5220811234567890   — Banque Misr Mastercard World Credit
  5078001234567890   — Banque Misr Meeza Debit
  4556761234567890   — CIB Visa Platinum Credit
  5573681234567890   — CIB Mastercard Gold Credit
  5078041234567890   — CIB Meeza Debit
  4314931234567890   — QNB Visa Infinite Credit
  5256781234567890   — QNB Mastercard Platinum Credit
  5078051234567890   — QNB Meeza Debit
  374000123456789    — Citi Egypt Amex Platinum (15 digits)
  5395421234567890   — Fawry Mastercard Prepaid
  5080001234567890   — Paymob Meeza Prepaid
  5090001234567890   — MoneyFellows Virtual Card

International:
  4111111111111111   — Visa test
  5555555555554444   — Mastercard test
  378282246310005    — American Express (15 digits)
  6011111111111117   — Discover
  3530111333300000   — JCB
  30569309025904     — Diners Club (14 digits)
```

### National IDs

```
30003098800631   — Male, born 2000-03-09, Cairo
29505211234567   — Female, born 1995-05-21, Giza
29001123344556   — Female, born 1990-01-12, Alexandria
28407010101234   — Male, born 1984-07-01, Port Said
31003021101234   — Female, born 2010-03-21, Suez
```

### Phone Numbers

```
01012345678   — Vodafone
01112345678   — Orange
01151234567   — Etisalat
01501234567   — WE Telecom
01951234567   — Special Services (Fawry)
+201012345678 — International format
```

### Barcodes

```
2123450123405   — Weight: 1.234 kg, Product: 12345
2678901250008   — Price: 125.00 EGP
6221234567890   — EAN-13, Egypt
5901234567890   — EAN-13, Poland
12345670        — EAN-8
123456789012    — UPC-A
```

---

## 🔗 Integration Examples

### ASP.NET Core Web API

```csharp
[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private static readonly BankCardValidator _validator =
        new(DisplayLanguage.Arabic, logger: new ConsoleBankCardLogger());

    [HttpPost("validate")]
    public IActionResult ValidateCard([FromBody] ValidateCardRequest req)
    {
        var info = _validator.AnalyzeFull(
            req.CardNumber, req.Expiry, req.CVV, req.CardholderName);

        if (!info.IsValid)
            return BadRequest(new
            {
                IsValid = false,
                Message = info.ValidationMessage,
                Reason  = info.FailureReason.ToString()
            });

        return Ok(new
        {
            IsValid              = true,
            Network              = info.NetworkName,
            CardType             = info.CardTypeName,
            CardCategory         = info.CardCategoryName,
            Bank                 = info.IssuerName,
            IsEgyptian           = info.IsEgyptian,
            SupportsTokenization = info.SupportsTokenization,
            MaskedNumber         = info.MaskedNumber,
            LastFour             = info.LastFourDigits,
            CustomerService      = info.CustomerServiceEgypt
        });
    }

    [HttpGet("network/{cardNumber}")]
    public IActionResult DetectNetwork(string cardNumber)
    {
        var network = cardNumber.GetCardNetworkName(DisplayLanguage.Arabic);
        var isEgyptian = cardNumber.IsEgyptianCard();
        
        return Ok(new { Network = network, IsEgyptian = isEgyptian });
    }

    [HttpGet("banks/egyptian")]
    public IActionResult GetEgyptianBanks()
    {
        var banks = IssuerDatabase.EgyptianIssuers
            .GroupBy(i => i.IssuerName)
            .Select(g => new
            {
                BankName    = g.Key,
                BankNameAr  = g.First().IssuerNameArabic,
                CustomerService = g.First().CustomerServiceEgypt,
                Cards = g.Select(i => new
                {
                    Network  = i.Network.ToString(),
                    CardType = i.CardType.ToString(),
                    Category = i.CardCategory.ToString(),
                    IIN      = i.IIN
                })
            });
        return Ok(banks);
    }
}

public record ValidateCardRequest(
    string CardNumber, string? Expiry,
    string? CVV, string? CardholderName);
```

### Blazor WebAssembly — Real-time Card Form

```razor
@page "/checkout"
@using NumericValidation.EG.Models.BankCard.Extensions
@using NumericValidation.EG.Models.BankCard.Enums

<h3>Payment Details</h3>

<div class="form-group">
    <label>Card Number</label>
    <input type="text" @oninput="OnCardInput" class="form-control" maxlength="19" 
           placeholder="1234 5678 9012 3456" />

    @if (cardInfo != null)
    {
        <div class="card-info mt-2 p-2 border rounded">
            <div class="d-flex align-items-center">
                <img src="@GetNetworkLogo()" height="28" alt="@cardInfo.NetworkName" />
                <span class="ms-2 fw-bold">@cardInfo.NetworkName</span>
                @if (cardInfo.IsEgyptian)
                {
                    <span class="badge bg-success ms-2">🇪🇬 Egyptian</span>
                }
            </div>
            <div class="text-muted small mt-1">@cardInfo.MaskedNumber</div>
            <div class="text-muted small">@cardInfo.IssuerName</div>
            @if (cardInfo.SupportsTokenization)
            {
                <div class="badge bg-info mt-1">Supports Apple Pay / Google Pay</div>
            }
        </div>
    }
</div>

<div class="row">
    <div class="col">
        <label>Expiry (MM/YY)</label>
        <input type="text" @bind="expiry" placeholder="MM/YY" class="form-control" />
    </div>
    <div class="col">
        <label>CVV (@(cardInfo?.CvvLength ?? 3) digits)</label>
        <input type="password" @bind="cvv" maxlength="@(cardInfo?.CvvLength ?? 3)" 
               class="form-control" />
    </div>
</div>

<div class="form-group mt-2">
    <label>Cardholder Name</label>
    <input type="text" @bind="cardholderName" class="form-control" 
           placeholder="AS SHOWN ON CARD" />
</div>

<button @onclick="Submit" disabled="@(!isValid)" class="btn btn-primary mt-3">
    Pay Now
</button>

@code {
    BankCardInfo? cardInfo;
    bool isValid;
    string expiry = "", cvv = "", cardholderName = "";

    void OnCardInput(ChangeEventArgs e)
    {
        var raw = e.Value?.ToString() ?? "";
        if (raw.Length >= 6)
        {
            cardInfo = raw.AnalyzeCard(DisplayLanguage.English);
            isValid  = cardInfo.IsLuhnValid && cardInfo.IsLengthValid;
        }
        else { cardInfo = null; isValid = false; }
    }

    string GetNetworkLogo() => cardInfo?.Network switch
    {
        CardNetwork.Visa        => "/img/visa.svg",
        CardNetwork.Mastercard  => "/img/mastercard.svg",
        CardNetwork.Meeza       => "/img/meeza.svg",
        CardNetwork.AmericanExpress => "/img/amex.svg",
        _ => "/img/card.svg"
    };

    async Task Submit() 
    { 
        // Process payment
        await JSRuntime.InvokeVoidAsync("alert", "Payment processed!");
    }
}
```

### Console Application

```csharp
using System.Diagnostics;
using NumericValidation.EG.Models.BankCard.Extensions;
using NumericValidation.EG.Models.BankCard.Enums;
using NumericValidation.EG.Models.NationalNumber;
using NumericValidation.EG.Models.PhoneNumber;
using NumericValidation.EG.Models.BarcodeNumber;
using NumericValidation.EG.Models.NumbersToText;
using NumericValidation.EG.Models.Helper;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Egyptian Data Validator ===\n");

        bool exit = false;
        while (!exit)
        {
            PrintMenu();
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1": TestBankCard(); break;
                case "2": TestNationalId(); break;
                case "3": TestPhoneNumber(); break;
                case "4": TestBarcode(); break;
                case "5": TestNumberToWords(); break;
                case "6": TestNumberConversionHelper(); break;
                case "7": PerformanceTest(); break;
                case "0": exit = true; break;
            }
            
            if (!exit)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }

    static void PrintMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Egyptian Data Validator ===");
        Console.WriteLine("1. Test Bank Card Validation");
        Console.WriteLine("2. Test National ID");
        Console.WriteLine("3. Test Phone Number");
        Console.WriteLine("4. Test Barcode");
        Console.WriteLine("5. Convert Number to Words");
        Console.WriteLine("6. Test NumberConversionHelper");
        Console.WriteLine("7. Performance Test");
        Console.WriteLine("0. Exit");
        Console.Write("Select: ");
    }

    static void TestBankCard()
    {
        Console.WriteLine("\n--- Bank Card Validation ---");
        
        string[] cards = {
            "4111111111111111",   // Visa
            "5078031234567890",   // NBE Meeza
            "378282246310005",    // Amex
            "4285411234567890"    // NBE Visa
        };

        foreach (var card in cards)
        {
            var info = card.AnalyzeCard(DisplayLanguage.Arabic);
            Console.WriteLine($"\nCard: {info.MaskedNumber}");
            Console.WriteLine($"Valid: {info.IsValid} | Network: {info.NetworkName}");
            Console.WriteLine($"Bank: {info.IssuerName}");
            Console.WriteLine($"Country: {info.CountryName}");
            if (info.IsEgyptian)
                Console.WriteLine($"Customer Service: {info.CustomerServiceEgypt}");
        }
    }

    static void TestNationalId()
    {
        Console.WriteLine("\n--- National ID Validation ---");
        
        string id = "30003098800631";
        var info = NationalIdParser.ParseSingle(id);
        
        Console.WriteLine($"ID: {id}");
        Console.WriteLine($"Valid: {info.IsValid}");
        Console.WriteLine($"Birth Date: {info.Day}/{info.Month}/{info.Year}");
        Console.WriteLine($"Age: {info.AgeYears} years");
        Console.WriteLine($"Gender: {info.Gender}");
        Console.WriteLine($"Governorate: {info.GovernorateNameEnglish}");
        Console.WriteLine($"Zodiac: {info.ZodiacSignEnglish}");
    }

    static void TestPhoneNumber()
    {
        Console.WriteLine("\n--- Phone Number Validation ---");
        
        string[] numbers = { "01012345678", "01112345678", "01501234567" };
        
        foreach (var num in numbers)
        {
            var info = PhoneNumberValidator.Validate(num);
            Console.WriteLine($"\nNumber: {info.Number}");
            Console.WriteLine($"Valid: {info.IsValid} | Carrier: {info.Carrier}");
            Console.WriteLine($"Service Type: {info.ServiceType}");
            Console.WriteLine($"Formatted: {info.FormattedNumber}");
        }
    }

    static void TestBarcode()
    {
        Console.WriteLine("\n--- Barcode Parsing ---");
        
        string[] barcodes = { "2123450123405", "2678901250008", "6221234567890" };
        
        foreach (var bc in barcodes)
        {
            var info = BarcodeParser.ParseSingle(bc);
            Console.WriteLine($"\nBarcode: {bc}");
            Console.WriteLine($"Type: {info.TypeNameEnglish}");
            
            if (info.Type == BarcodeType.Weight)
                Console.WriteLine($"Weight: {info.Weight} kg");
            else if (info.Type == BarcodeType.Price)
                Console.WriteLine($"Price: {info.Price} EGP");
                
            Console.WriteLine($"Product Code: {info.ProductCode}");
            Console.WriteLine($"Country: {info.CountryNameEnglish}");
        }
    }

    static void TestNumberToWords()
    {
        Console.WriteLine("\n--- Number to Words Conversion ---");
        
        decimal[] amounts = { 1234.56m, 1000000m, 0.99m, -500.75m };
        
        foreach (var amount in amounts)
        {
            Console.WriteLine($"\nAmount: {amount:N2} EGP");
            
            string arabic = NumberToWordsConverter.Convert(amount, Language.Arabic, Currency.EGP);
            Console.WriteLine($"Arabic: {arabic}");
            
            string english = NumberToWordsConverter.Convert(amount, Language.English, Currency.USD);
            Console.WriteLine($"English: {english}");
        }
    }

    static void TestNumberConversionHelper()
    {
        Console.WriteLine("\n--- NumberConversionHelper ---");
        
        // Numeral conversion
        string western = "12345.678";
        string arabic = western.ToArabicNumerals();
        Console.WriteLine($"Western: {western} → Arabic: {arabic}");
        
        // Text unification
        string messy = "أحــمــدُ بنُ محمــدٍ ٢٥ سنةٍ";
        string clean = messy.UnifyArabic();
        Console.WriteLine($"\nMessy: {messy}");
        Console.WriteLine($"Clean: {clean}");
        
        // Extraction
        string mixed = "السعر: ٢٥٠.٥٠ جنيه";
        string numbers = mixed.ExtractNumbers();
        string textOnly = mixed.ExtractTextOnly();
        Console.WriteLine($"\nMixed: {mixed}");
        Console.WriteLine($"Numbers: {numbers}");
        Console.WriteLine($"Text only: {textOnly}");
        
        // Formatting
        string phone = NumberConversionHelper.FormatPhoneArabic("01012345678");
        Console.WriteLine($"\nFormatted phone: {phone}");
    }

    static void PerformanceTest()
    {
        Console.WriteLine("\n--- Performance Test ---");
        
        var sw = Stopwatch.StartNew();
        var validator = new BankCardValidator();
        var cards = Enumerable.Range(0, 10000)
            .Select(i => $"411111111111{i:D4}")
            .ToList();
            
        var results = cards.Select(c => validator.Analyze(c)).ToList();
        sw.Stop();
        
        Console.WriteLine($"Processed 10,000 cards in {sw.ElapsedMilliseconds} ms");
        Console.WriteLine($"Average: {sw.ElapsedMilliseconds / 10000.0:F3} ms/card");
    }
}
```

---

## ⚠️ Limitations

**Bank Card Validation:**
- PAN length: 13–19 digits (ISO/IEC 7812 standard range)
- IIN database: Egyptian banks complete as of February 2025; global ranges comprehensive but may lag new issuers
- Simulated tokens are for testing only — not real network tokens
- Expiry validation: MM/YY and MM/YYYY with `/`, `-`, or space separators only
- Cardholder name: Latin characters only (ISO/IEC 7813)

**National ID:**
- 14-digit format only
- Birth years: 1900–2099

**Phone Numbers:**
- Egyptian mobile numbers only (`01X`)
- Fixed lines not supported
- International format: `+20` prefix only

**Barcodes:**
- Focus on Egyptian retail formats (EAN-13, EAN-8, UPC-A, weight/price prefix `2`)
- Auto-detection is heuristic — specify type explicitly for ambiguous inputs in production

**Number Conversion:**
- Range: ±999,999,999,999.99
- Fractions: up to 2 decimal places (3 for KWD)

**NumberConversionHelper:**
- Arabic text normalization may not handle all edge cases
- Some Unicode characters may not be properly processed

---

## 🔍 Troubleshooting

### Common Issues:

**"Invalid card number" for a valid Egyptian card**
```csharp
// Diagnose each layer
var info = cardNumber.AnalyzeCard();
Console.WriteLine($"Luhn:   {LuhnAlgorithm.IsValid(cardNumber)}");
Console.WriteLine($"Length: {info.IsLengthValid} (valid: {string.Join(",", info.ValidPanLengths)})");
Console.WriteLine($"Network:{info.NetworkName}");
Console.WriteLine($"Reason: {info.FailureReason}");
```

**"Unknown network" for valid cards**
```csharp
// Check IIN database
string iin = cardNumber.Substring(0, 6);
var issuer = IssuerDatabase.FindIssuerByIIN(iin);
if (issuer == null)
{
    // Check global ranges
    foreach (var range in IssuerDatabase.GlobalNetworkRanges)
    {
        if (IsInRange(cardNumber, range.Start, range.End))
        {
            Console.WriteLine($"Network: {range.Network}");
            break;
        }
    }
}
```

**"Invalid national ID"**
```csharp
// Clean input before validation
string CleanNationalId(string input)
{
    // Remove all non-digits
    string cleaned = Regex.Replace(input, @"[^\d]", "");
    
    // Ensure exactly 14 digits
    if (cleaned.Length != 14)
        throw new ArgumentException("National ID must be 14 digits");
        
    return cleaned;
}
```

**"Unknown phone carrier"**
```csharp
// Use lenient validation
public PhoneNumberInfo ValidatePhoneLenient(string phone)
{
    // First try standard validation
    var result = PhoneNumberValidator.Validate(phone);
    
    if (!result.IsValid)
    {
        // Try cleaning and re-validating
        string cleaned = NumberConversionHelper.ExtractNumbers(phone);
        if (cleaned.Length == 11 && cleaned.StartsWith("01"))
        {
            result = PhoneNumberValidator.Validate(cleaned);
        }
    }
    
    return result;
}
```

**"Number too large for conversion"**
```csharp
// Check limits before conversion
public string SafeConvertToWords(decimal amount, Currency currency)
{
    decimal maxAmount = 999999999999.99m;
    decimal minAmount = -999999999999.99m;
    
    if (amount > maxAmount || amount < minAmount)
    {
        return $"Amount must be between {minAmount:N2} and {maxAmount:N2}";
    }
    
    return NumberToWordsConverter.Convert(amount, Language.Arabic, currency);
}
```

**"Arabic text not normalizing correctly"**
```csharp
// Use comprehensive cleanup
public string NormalizeArabicComprehensive(string text)
{
    // Step 1: Convert all numbers to Arabic
    text = NumberConversionHelper.ToArabicNumerals(text);
    
    // Step 2: Unify Arabic text
    text = NumberConversionHelper.UnifyArabicText(text, 
        removeTashkeel: true, 
        removeTatweel: true, 
        unifyHamza: true);
    
    // Step 3: Clean whitespace and special characters
    text = text.CleanWhitespace();
    text = text.RemoveSpecialChars(keepSpaces: true);
    
    return text;
}
```

**"InternalError" FailureReason**
```csharp
// Set up a logger to capture the full exception
var validator = new BankCardValidator(logger: new ConsoleBankCardLogger());
// Or implement IBankCardLogger.LogError() to send to your monitoring system
```

**Arabic text not rendering correctly in console**
```csharp
Console.OutputEncoding = System.Text.Encoding.UTF8;
// Then re-run — most rendering issues are encoding, not library related
```

**Cache consuming too much memory (high-volume unique PANs)**
```csharp
// Clear after each batch; the cache ceiling is 512 entries
validator.ClearCache();
```

---

## 📄 License

MIT License — Copyright © 2025 NumericValidation.EG

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

---

## 🆘 Support

| Channel | Link |
|---------|------|
| 🐛 Bug Reports | [GitHub Issues](https://github.com/amrSallam2000/NumericValidation.EG/issues) |
| 💬 Questions | [GitHub Discussions](https://github.com/amrSallam2000/NumericValidation.EG/discussions) |
| 👤 Developer | [LinkedIn — Amr Sallam](https://www.linkedin.com/in/amr-sallam-a8363322a/) |
| 📦 Package | [NuGet — NumericValidation.EG](https://www.nuget.org/packages/NumericValidation.EG) |

**Stack Overflow tag:** `#numericvalidation-eg`

---

## 🙏 Acknowledgements

- **Central Bank of Egypt (CBE)** — Regulations, standards, and Tokenization programme
- **ISO/IEC 7812-1:2017** — International standard for payment card identification
- **PCI Security Standards Council** — PCI-DSS v4.0 compliance guidance
- All Egyptian banks for data verification cooperation
- The Egyptian developer community for feedback and contributions

---

<div align="center">

**Happy Coding! 🚀**

*Built with ❤️ for the Egyptian developer community*

**v1.0.3** — Now with complete Bank Card Validation · 💳 Meeza · 🏦 30+ Egyptian Banks · 🔐 PCI-DSS v4.0

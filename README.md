# NumericValidation.EG - Comprehensive Egyptian Numbers Validation Library

## 📋 Project Overview
A complete .NET library for validating and analyzing Egyptian numbers and formats. Specifically designed for the Egyptian market with support for Arabic language and local formats.

## 📚 Table of Contents
- [✨ Key Features](#-key-features)
- [🚀 Quick Start Guide](#-quick-start-guide)
- [📖 Basic Usage Examples](#-basic-usage-examples)
- [🔤 NumberConversionHelper](#-numberconversionhelper)
- [🎯 Advanced Usage](#-advanced-usage)
- [📊 Performance Features](#-performance-features)
- [🔧 API Reference](#-api-reference)
- [🧪 Test Data](#-test-data)
- [🔗 Integration Examples](#-integration-examples)
- [⚡ Performance Optimization](#-performance-optimization)
- [⚠️ Limitations](#️-limitations)
- [🔍 Troubleshooting](#-troubleshooting)
- [📚 Additional Resources](#-additional-resources)
- [📄 License](#-license)
- [🆘 Support](#-support)

---

## ✨ Key Features

### 1️⃣ **Egyptian Barcode System**
- **Weight Barcodes** (starts with 2)
- **Price Barcodes** (starts with 2)  
- **Standard Barcodes** (EAN-13, EAN-8, UPC-A)
- **Auto-detection** of barcode type
- Extract: product code, weight, price, country info
- **Batch processing** and statistics
- **Currency conversion** (EGP, USD, EUR, SAR, KWD, etc.)

### 2️⃣ **Egyptian National ID**
- Validate 14-digit national ID numbers
- Extract: birth date, age, gender, governorate
- Calculate: zodiac sign, generation, century
- Arabic and English output
- **Batch validation** with statistics
- **Age analysis** and filtering
- **Geographic distribution** analysis

### 3️⃣ **Egyptian Phone Numbers**
- Validate all Egyptian mobile prefixes:
  - Vodafone (0100-0109)
  - Orange (0110-0113, 0120-0129)
  - Etisalat (0114-0119)
  - WE Telecom (0150-0159)
  - Special Services (0190-0199)
- Support for local/international formats
- Carrier identification and statistics
- **Service type detection** (Banking, Fawry, Ride Hailing, etc.)
- **Number formatting** in Arabic/English numerals

### 4️⃣ **Number to Words Conversion**
- **Arabic** with proper grammar rules (singular, dual, plural)
- **English** conversion
- **20+ currencies** including EGP, USD, EUR, SAR, AED, KWD
- Support for fractions (piasters, cents, fils)
- **Negative numbers** handling
- **Detailed conversion** with integer/fraction parts

### 5️⃣ **🔤 NumberConversionHelper**
- **Numeral conversion**: Arabic ↔ Western digits
- **Text unification**: Normalize Arabic text with/without diacritics
- **Data extraction**: Extract numbers/text from mixed content
- **Formatting utilities**: Phone numbers, national IDs, monetary amounts
- **Detection functions**: Identify content types automatically
- **Extension methods**: Fluent API for all types
- **Advanced processing**: Customizable text processing pipelines

---

## 🚀 Quick Start Guide

### Step 1: Installation
```xml
<!-- Package Manager Console -->
Install-Package NumericValidation.EG

<!-- Or add to .csproj -->
<PackageReference Include="NumericValidation.EG" Version="1.0.0" />
```

### Step 2: Import Namespaces
```csharp
using NumericValidation.EG.Models.NationalNumber;
using NumericValidation.EG.Models.PhoneNumber;
using NumericValidation.EG.Models.BarcodeNumber;
using NumericValidation.EG.Models.NumbersToText;
using NumericValidation.EG.Models.Helper; // For NumberConversionHelper
```

---

## 📖 Basic Usage Examples

### 1. National ID Validation
```csharp
// Parse single National ID
var parser = new NationalIdParser();
var idInfo = parser.Parse("30003098800631");

if (idInfo.IsValid)
{
    Console.WriteLine($"Valid: {idInfo.IsValid}");
    Console.WriteLine($"Birth Date: {idInfo.Day}/{idInfo.Month}/{idInfo.Year}");
    Console.WriteLine($"Age: {idInfo.AgeYears} years");
    Console.WriteLine($"Gender: {idInfo.Gender}");
    Console.WriteLine($"Governorate: {idInfo.GovernorateNameEnglish}");
    Console.WriteLine($"Zodiac: {idInfo.ZodiacSignEnglish}");
}
```

### 2. Phone Number Validation
```csharp
// Validate phone number
var validator = new PhoneNumberValidator();
var phoneInfo = validator.Validate("01012345678");

Console.WriteLine($"Valid: {phoneInfo.IsValid}");
Console.WriteLine($"Carrier: {phoneInfo.Carrier}");
Console.WriteLine($"Type: {phoneInfo.ServiceType}");
Console.WriteLine($"Formatted: {phoneInfo.FormattedNumber}");
```

### 3. Barcode Parsing
```csharp
// Parse barcode
var parser = new BarcodeParser();
var barcodeInfo = parser.Parse("2123450123405");

Console.WriteLine($"Type: {barcodeInfo.TypeNameEnglish}");
Console.WriteLine($"Weight: {barcodeInfo.Weight} kg");
Console.WriteLine($"Product Code: {barcodeInfo.ProductCode}");
Console.WriteLine($"Country: {barcodeInfo.CountryNameEnglish}");
```

### 4. Number to Words Conversion
```csharp
// Convert number to Arabic words
var arabicText = NumberToWordsConverter.Convert(1234.56m, Language.Arabic, Currency.EGP);
// Result: "ألف ومئتان وأربعة وثلاثون جنيهًا مصريًا وستة وخمسون قرشًا"

// Convert number to English words
var englishText = NumberToWordsConverter.Convert(1234.56m, Language.English, Currency.USD);
// Result: "One Thousand Two Hundred Thirty Four Dollars and Fifty Six Cents"

// Convert with details
var details = NumberToWordsConverter.ConvertWithDetails(123.45m, Language.Arabic, Currency.EGP);
Console.WriteLine($"Integer: {details.IntegerPart}, Fraction: {details.FractionalPart}");
Console.WriteLine($"Full Text: {details.FullText}");
```

---

## 🔤 NumberConversionHelper

### 1. Numeral Conversion
```csharp
// Convert between Arabic and Western numerals
string arabicNumerals = NumberConversionHelper.ToArabicNumerals("12345.678");
// Result: "١٢٣٤٥٫٦٧٨"

string westernNumerals = NumberConversionHelper.ToWesternNumerals("١٢٣٤٥٫٦٧٨");
// Result: "12345.678"

// Using extension methods
string result = "12345".ToArabicNumerals(); // "١٢٣٤٥"
decimal amount = 1234.56m;
string arabicAmount = amount.ToArabicNumerals(); // "١٬٢٣٤٫٥٦"
```

### 2. Text Unification
```csharp
// Unify Arabic text
string unified = NumberConversionHelper.UnifyArabicText("أحــمــدُ بنُ محمــدٍ");
// Result: "احمد بن محمد"

// Normalize with options
string normalized = NumberConversionHelper.NormalizeArabicText("أَحْمَدُ ٢٥ سَنَةٍ");
// Result: "احمد ٢٥ سنه"

// Full cleanup
string clean = NumberConversionHelper.FullClean("  أحــمــد   عمــره  ٢٥  سنةٍ  ", arabicNumerals: true);
// Result: "احمد عمره ٢٥ سنه"
```

### 3. Data Extraction
```csharp
// Extract numbers from text
string numbers = NumberConversionHelper.ExtractNumbers("السعر: ٢٥٠.٥٠ جنيه");
// Result: "250.50"

// Extract text only
string textOnly = NumberConversionHelper.ExtractTextOnly("العمر: ٢٥ سنة");
// Result: "العمر:  سنة"

// Extract Arabic text
string arabicText = NumberConversionHelper.ExtractArabicText("Name: أحمد - Age: ٢٥");
// Result: "احمد "
```

### 4. Detection Functions
```csharp
// Detect content type
bool hasArabicNumerals = NumberConversionHelper.ContainsArabicNumerals("العمر: ٢٥ سنة");
bool hasWesternNumerals = NumberConversionHelper.ContainsWesternNumerals("Age: 25 years");
bool hasArabicLetters = NumberConversionHelper.ContainsArabicLetters("أحمد محمد");
bool hasEnglishLetters = NumberConversionHelper.ContainsEnglishLetters("John Doe");

// Auto-convert based on content
string autoConverted = NumberConversionHelper.AutoConvertNumerals("العمر: ٢٥ سنة", preferArabic: false);
// Result: "العمر: 25 سنة"
```

### 5. Formatting Utilities
```csharp
// Format numbers with Arabic numerals
string formattedNumber = NumberConversionHelper.FormatNumberArabic(1234567.89m, 2);
// Result: "١٬٢٣٤٬٥٦٧٫٨٩"

// Format phone numbers
string formattedPhone = NumberConversionHelper.FormatPhoneArabic("01012345678");
// Result: "٠١٠ ١٢٣ ٤٥٦٧٨"

// Format national IDs
string formattedId = NumberConversionHelper.FormatNationalIdArabic("12345678901234");
// Result: "١٢ ٣٤ ٥٦ ٧٨ ٩٠١ ٢٣٤"
```

### 6. Advanced Processing
```csharp
// Process text with custom options
var options = new TextUnificationOptions
{
    UnifyArabicText = true,
    RemoveTashkeel = true,
    RemoveTatweel = true,
    UnifyNumerals = true,
    PreferArabicNumerals = true,
    RemoveSpecialChars = true,
    KeepSpaces = true
};

string processed = NumberConversionHelper.ProcessText("أحــمــدُ بنُ محمــدٍ ٢٥ سنةٍ", options);
// Result: "احمد بن محمد ٢٥ سنه"

// Normalize for search
string searchText = NumberConversionHelper.NormalizeForSearch("  أحــمــد   العمــرانى  ", arabicNumerals: true);
// Result: "احمد العمرانى"
```

### 7. Extension Methods (Fluent API)
```csharp
// String extensions
string result1 = "أحــمــدُ بنُ محمــدٍ".UnifyArabic();
string result2 = "١٢٣٤٥".ToWesternNumerals();
string result3 = "12345".ToArabicNumerals();
string result4 = "العمر: ٢٥ سنة".ExtractNumbers();
string result5 = "العمر: ٢٥ سنة".ExtractTextOnly();
string result6 = "أحمد@محمود#123".RemoveSpecialChars();
string result7 = "  أحمد    محمود   ".CleanWhitespace();

// Detection extensions
bool hasArabic = "العمر: ٢٥ سنة".ContainsArabicNumerals();
bool hasWestern = "Age: 25 years".ContainsWesternNumerals();
bool hasArabicLetters = "أحمد محمد".ContainsArabicLetters();
bool hasEnglishLetters = "John Doe".ContainsEnglishLetters();

// Auto-convert extension
string autoResult = "العمر: ٢٥ سنة".AutoConvertNumerals(preferArabic: false);

// Process with options
string processedText = "العمر: ٢٥ سنة".ProcessText(TextUnificationOptions.Default);
```

---

## 🎯 Advanced Usage

### Batch Processing
```csharp
// Validate multiple phone numbers
string[] numbers = { "01012345678", "01112345678", "01234567890" };
var validator = new PhoneNumberValidator(numbers);
var results = validator.ValidateAll();

foreach (var result in results)
{
    Console.WriteLine($"{result.Number}: {(result.IsValid ? "✓" : "✗")} - {result.Carrier}");
}

// Get statistics
var stats = validator.GetValidationSummary();
Console.WriteLine($"Total: {stats["Total"]}");
Console.WriteLine($"Success Rate: {stats["SuccessRate"]}%");

// Group by carrier
var grouped = validator.GroupByCarrier();
foreach (var group in grouped)
{
    Console.WriteLine($"{group.Key}: {group.Value.Count} numbers");
}
```

### Advanced National ID Analysis
```csharp
// Batch processing with statistics
var idParser = new NationalIdParser(nationalIds);
var results = idParser.ParseAll();

// Get comprehensive statistics
var stats = idParser.GetStatistics();
Console.WriteLine($"Total Processed: {stats["Total"]}");
Console.WriteLine($"Valid: {stats["Valid"]}");
Console.WriteLine($"Invalid: {stats["Invalid"]}");
Console.WriteLine($"Success Rate: {stats["SuccessRate"]}%");

// Age analysis
if (stats.ContainsKey("AverageAge"))
    Console.WriteLine($"Average Age: {stats["AverageAge"]} years");

// Gender distribution
if (stats.ContainsKey("MaleCount") && stats.ContainsKey("FemaleCount"))
    Console.WriteLine($"Gender Ratio: {stats["MaleCount"]}M : {stats["FemaleCount"]}F");

// Governorate distribution
if (stats.ContainsKey("GovernorateDistribution"))
{
    var distribution = (Dictionary<string, int>)stats["GovernorateDistribution"];
    foreach (var gov in distribution.OrderByDescending(g => g.Value))
    {
        Console.WriteLine($"  {gov.Key}: {gov.Value} IDs");
    }
}
```

### Real-world Scenarios

#### Supermarket System
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

#### Government Registration System
```csharp
public class CitizenRegistration
{
    public void RegisterCitizen(string nationalId, string phoneNumber, string fullName)
    {
        // Validate national ID
        var idInfo = NationalIdParser.ParseSingle(nationalId);
        if (!idInfo.IsValid)
            throw new ArgumentException("Invalid national ID");
        
        // Validate phone number
        var phoneInfo = PhoneNumberValidator.Validate(phoneNumber);
        if (!phoneInfo.IsValid)
            throw new ArgumentException("Invalid phone number");
        
        // Generate secure password
        string password = GenerateSecurePassword(idInfo, phoneInfo);
        
        // Normalize Arabic name
        string normalizedName = NumberConversionHelper.UnifyArabicText(fullName);
        
        Console.WriteLine($"Citizen Registered Successfully:");
        Console.WriteLine($"  Name: {normalizedName}");
        Console.WriteLine($"  Birth Date: {idInfo.BirthDateFullArabic}");
        Console.WriteLine($"  Age: {idInfo.AgeTextArabic}");
        Console.WriteLine($"  Governorate: {idInfo.GovernorateNameArabic}");
        Console.WriteLine($"  Phone: {NumberConversionHelper.FormatPhoneArabic(phoneNumber)}");
        Console.WriteLine($"  Generated Password: {password}");
    }
    
    private string GenerateSecurePassword(NationalIdInfo idInfo, PhoneNumberInfo phoneInfo)
    {
        string birthDate = idInfo.Year.ToString().Substring(2) +
                          idInfo.Month.ToString("D2") +
                          idInfo.Day.ToString("D2");
        
        string phoneLast4 = phoneInfo.Number.Substring(phoneInfo.Number.Length - 4);
        
        return $"EG-{birthDate}-{phoneLast4}";
    }
}
```

#### Financial Application
```csharp
public class FinancialTransaction
{
    public void ProcessPayment(decimal amount, Currency currency, string recipientPhone)
    {
        // Validate recipient phone
        var phoneInfo = PhoneNumberValidator.Validate(recipientPhone);
        if (!phoneInfo.IsValid)
            throw new ArgumentException("Invalid recipient phone number");
        
        // Check amount limits
        if (Math.Abs(amount) > 999999999999m)
            throw new ArgumentException("Amount exceeds maximum limit");
        
        // Convert amount to words
        string amountInWords = NumberToWordsConverter.Convert(amount, 
            NumberToWordsConverter.Language.Arabic, currency);
        
        // Format for display
        string formattedAmount = NumberConversionHelper.FormatNumberArabic(amount, 2);
        
        Console.WriteLine($"💳 Payment Processing:");
        Console.WriteLine($"  Amount: {formattedAmount} {currency}");
        Console.WriteLine($"  Amount in words: {amountInWords}");
        Console.WriteLine($"  Recipient: {phoneInfo.Carrier} - {phoneInfo.Number}");
        Console.WriteLine($"  Service Type: {phoneInfo.ServiceType}");
        
        // Suggest denominations
        var denominations = SuggestDenominations(amount, currency);
        Console.WriteLine($"  Suggested denominations: {denominations}");
    }
    
    private string SuggestDenominations(decimal amount, Currency currency)
    {
        var denominations = currency switch
        {
            Currency.EGP => new[] { 200m, 100m, 50m, 20m, 10m, 5m, 1m },
            Currency.USD => new[] { 100m, 50m, 20m, 10m, 5m, 1m },
            Currency.SAR => new[] { 500m, 200m, 100m, 50m, 20m, 10m, 5m, 1m },
            _ => new[] { 100m, 50m, 20m, 10m, 5m, 1m }
        };
        
        var result = new List<string>();
        decimal remaining = amount;
        
        foreach (var denom in denominations.OrderByDescending(d => d))
        {
            if (remaining >= denom)
            {
                int count = (int)(remaining / denom);
                remaining %= denom;
                result.Add($"{count} × {denom} {currency}");
            }
        }
        
        return string.Join(" + ", result);
    }
}
```

---

## 📊 Performance Features

### 1. **High-Performance Batch Processing**
```csharp
// Process 10,000 items in batch
var stopwatch = Stopwatch.StartNew();

var idParser = new NationalIdParser(GenerateTestNationalIds(10000));
var results = idParser.ParseAll();

stopwatch.Stop();
Console.WriteLine($"Processed 10,000 IDs in {stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Average: {stopwatch.ElapsedMilliseconds / 10000.0:F3}ms per ID");
```

### 2. **Memory-Efficient Operations**
```csharp
// Memory monitoring during batch processing
long initialMemory = GC.GetTotalMemory(true);

var parser = new BarcodeParser(GenerateTestBarcodes(5000));
var results = parser.ParseAll();

GC.Collect();
long finalMemory = GC.GetTotalMemory(true);

Console.WriteLine($"Memory used: {(finalMemory - initialMemory) / 1024:N0} KB");
Console.WriteLine($"Memory per item: {(finalMemory - initialMemory) / 5000.0:F2} bytes");
```

### 3. **Parallel Processing Support**
```csharp
// Parallel validation for large datasets
public List<PhoneNumberInfo> ValidateParallel(List<string> phoneNumbers)
{
    var results = new ConcurrentBag<PhoneNumberInfo>();
    
    Parallel.ForEach(phoneNumbers, number =>
    {
        var result = PhoneNumberValidator.Validate(number);
        results.Add(result);
    });
    
    return results.ToList();
}
```

### 4. **Caching for Repeated Operations**
```csharp
public class CachedNationalIdParser
{
    private readonly ConcurrentDictionary<string, NationalIdInfo> _cache = new();
    
    public NationalIdInfo ParseWithCache(string nationalId)
    {
        return _cache.GetOrAdd(nationalId, id =>
        {
            var parser = new NationalIdParser();
            return parser.Parse(id);
        });
    }
}
```

---

## 🔧 API Reference

### Main Classes

| Class | Description | Key Methods |
|-------|-------------|-------------|
| **NationalIdParser** | Validates Egyptian National IDs | `Parse()`, `ParseAll()`, `GetStatistics()`, `GroupByGovernorate()` |
| **PhoneNumberValidator** | Validates Egyptian phone numbers | `Validate()`, `ValidateAll()`, `GetValidationSummary()`, `GroupByCarrier()` |
| **BarcodeParser** | Parses Egyptian barcodes | `Parse()`, `ParseWeight()`, `ParsePrice()`, `ParseAll()`, `GroupByType()` |
| **NumberToWordsConverter** | Converts numbers to words | `Convert()`, `ConvertToArabic()`, `ConvertToEnglish()`, `ConvertWithDetails()` |
| **NumberConversionHelper** | Text and number utilities | `ToArabicNumerals()`, `ToWesternNumerals()`, `UnifyArabicText()`, `ExtractNumbers()`, `FormatPhoneArabic()` |
| **TextUnificationOptions** | Text processing configuration | Configure normalization, unification, and formatting options |

### Enumerations

| Enum | Values | Description |
|------|--------|-------------|
| **Currency** | EGP, USD, EUR, GBP, SAR, AED, KWD, QAR, BHD, OMR, JOD, LBP, SYP, IQD, YER, MAD, TND, DZD, LYD, SDG | Supported currencies |
| **Language** | Arabic, English | Output languages |
| **BarcodeType** | Standard, Weight, Price | Barcode classification |
| **Gender** | Male, Female | Extracted from national ID |

---

## 🧪 Test Data Examples

### National IDs:
```csharp
// Valid IDs for testing
"30003098800631"  // Male, born 2000-03-09, Cairo
"29505211234567"  // Female, born 1995-05-21, Giza
"29001123344556"  // Female, born 1990-01-12, Alexandria
"28407010101234"  // Male, born 1984-07-01, Port Said
"31003021101234"  // Female, born 2010-03-21, Suez
```

### Phone Numbers:
```csharp
// Valid by carrier
"01012345678"  // Vodafone
"01112345678"  // Orange  
"01151234567"  // Etisalat
"01501234567"  // WE Telecom
"01951234567"  // Fawry (E-Payment)
```

### Barcodes:
```csharp
"2123450123405"  // Weight: 1.234kg, Product: 12345
"2678901250008"  // Price: 125.00 EGP (with 100 EGP/kg)
"6221234567890"  // EAN-13, Egypt country code
"5901234567890"  // EAN-13, Poland country code
```

---

## 🔗 Integration Examples

### ASP.NET Core Web API:
```csharp
[ApiController]
[Route("api/validation")]
public class ValidationController : ControllerBase
{
    [HttpPost("national-id")]
    public IActionResult ValidateNationalId([FromBody] ValidateIdRequest request)
    {
        var parser = new NationalIdParser();
        var result = parser.Parse(request.NationalId);
        
        return Ok(new {
            IsValid = result.IsValid,
            BirthDate = result.BirthDateISO,
            Age = result.AgeYears,
            Gender = result.Gender,
            Governorate = result.GovernorateNameEnglish,
            Zodiac = result.ZodiacSignEnglish,
            Century = result.Century
        });
    }
    
    [HttpPost("phone")]
    public IActionResult ValidatePhone([FromBody] ValidatePhoneRequest request)
    {
        var validator = new PhoneNumberValidator();
        var result = validator.Validate(request.PhoneNumber);
        
        return Ok(new {
            IsValid = result.IsValid,
            Carrier = result.Carrier,
            ServiceType = result.ServiceType,
            FormattedNumber = result.FormattedNumber,
            CleanedNumber = result.CleanedNumber
        });
    }
}
```

### Console Application with Menu:
```csharp
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        bool exit = false;
        while (!exit)
        {
            PrintMenu();
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    TestNationalId();
                    break;
                case "2":
                    TestPhoneNumber();
                    break;
                case "3":
                    TestBarcode();
                    break;
                case "4":
                    TestNumberToWords();
                    break;
                case "5":
                    TestNumberConversionHelper();
                    break;
                case "0":
                    exit = true;
                    break;
            }
        }
    }
    
    static void PrintMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Egyptian Data Validator ===");
        Console.WriteLine("1. Validate National ID");
        Console.WriteLine("2. Validate Phone Number");
        Console.WriteLine("3. Parse Barcode");
        Console.WriteLine("4. Convert Number to Words");
        Console.WriteLine("5. Test NumberConversionHelper");
        Console.WriteLine("0. Exit");
        Console.Write("Select: ");
    }
}
```

### Blazor Web Application:
```razor
@page "/validator"
@inject NationalIdParser IdParser
@inject PhoneNumberValidator PhoneValidator

<h3>Egyptian Data Validator</h3>

<EditForm Model="@model" OnValidSubmit="@ValidateData">
    <div class="form-group">
        <label>National ID:</label>
        <InputText @bind-Value="model.NationalId" class="form-control" />
    </div>
    
    <div class="form-group">
        <label>Phone Number:</label>
        <InputText @bind-Value="model.PhoneNumber" class="form-control" />
    </div>
    
    <button type="submit" class="btn btn-primary">Validate</button>
</EditForm>

@if (validationResult != null)
{
    <div class="alert alert-success mt-3">
        <h5>Validation Results:</h5>
        <p>National ID: @validationResult.NationalIdValid</p>
        <p>Phone: @validationResult.PhoneValid</p>
        <p>Carrier: @validationResult.PhoneCarrier</p>
    </div>
}

@code {
    private ValidationModel model = new();
    private ValidationResult validationResult;
    
    private async Task ValidateData()
    {
        var idResult = await IdParser.ParseAsync(model.NationalId);
        var phoneResult = await PhoneValidator.ValidateAsync(model.PhoneNumber);
        
        validationResult = new ValidationResult
        {
            NationalIdValid = idResult.IsValid,
            PhoneValid = phoneResult.IsValid,
            PhoneCarrier = phoneResult.Carrier
        };
    }
    
    class ValidationModel
    {
        public string NationalId { get; set; }
        public string PhoneNumber { get; set; }
    }
    
    class ValidationResult
    {
        public bool NationalIdValid { get; set; }
        public bool PhoneValid { get; set; }
        public string PhoneCarrier { get; set; }
    }
}
```

---

## ⚡ Performance Optimization

### 1. **Use Batch Processing for Large Datasets**
```csharp
// ✅ GOOD: Process in batches
var batchSize = 1000;
for (int i = 0; i < data.Count; i += batchSize)
{
    var batch = data.Skip(i).Take(batchSize).ToList();
    var results = validator.ValidateAll(batch);
    // Process results
}

// ❌ BAD: Process one by one
foreach (var item in data)
{
    var result = validator.Validate(item); // Repeated overhead
}
```

### 2. **Cache Frequently Used Results**
```csharp
public class OptimizedValidator
{
    private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    
    public NationalIdInfo GetNationalIdInfo(string nationalId)
    {
        return _cache.GetOrCreate(nationalId, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromHours(1);
            var parser = new NationalIdParser();
            return parser.Parse(nationalId);
        });
    }
}
```

### 3. **Use Parallel Processing Wisely**
```csharp
public List<BarcodeInfo> ProcessBarcodesParallel(List<string> barcodes)
{
    // Only use parallel for CPU-bound operations
    if (barcodes.Count > 1000)
    {
        var results = new ConcurrentBag<BarcodeInfo>();
        Parallel.ForEach(barcodes, barcode =>
        {
            var result = BarcodeParser.ParseSingle(barcode);
            results.Add(result);
        });
        return results.ToList();
    }
    else
    {
        // Sequential is faster for small batches
        return barcodes.Select(BarcodeParser.ParseSingle).ToList();
    }
}
```

### 4. **Optimize Memory Usage**
```csharp
public class MemoryEfficientProcessor
{
    public void ProcessLargeDataset(List<string> data)
    {
        // Process in chunks to reduce memory pressure
        const int chunkSize = 500;
        
        for (int i = 0; i < data.Count; i += chunkSize)
        {
            var chunk = data.Skip(i).Take(chunkSize).ToList();
            ProcessChunk(chunk);
            
            // Force garbage collection after each chunk
            if (i % 5000 == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
    
    private void ProcessChunk(List<string> chunk)
    {
        using var parser = new NationalIdParser(chunk);
        var results = parser.ParseAll();
        // Process results immediately
    }
}
```

---

## ⚠️ Limitations

1. **National ID**: 
   - Only 14-digit format supported
   - Maximum birth year: 2099
   - Minimum birth year: 1900

2. **Phone Numbers**: 
   - Egyptian mobile numbers only (01X)
   - Fixed lines not supported
   - International numbers with +20 prefix only

3. **Barcodes**: 
   - Focus on Egyptian market formats
   - Weight barcodes: 2XXXXXYYYYYZZ
   - Price barcodes: 2XXXXXYYYYYZZ
   - Standard: EAN-13, EAN-8, UPC-A

4. **Number Conversion**:
   - Maximum number: 999,999,999,999.99
   - Minimum number: -999,999,999,999.99
   - Fractions up to 2 decimal places (3 for KWD)

5. **NumberConversionHelper**:
   - Arabic text normalization may not handle all edge cases
   - Some Unicode characters may not be properly processed

---

## 🔍 Troubleshooting

### Common Issues:

1. **"Invalid national ID length"**
   ```csharp
   // Solution: Clean input before validation
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

2. **"Unknown phone carrier"**
   ```csharp
   // Solution: Use lenient validation
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

3. **"Number too large for conversion"**
   ```csharp
   // Solution: Check limits before conversion
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

4. **"Arabic text not normalizing correctly"**
   ```csharp
   // Solution: Use comprehensive cleanup
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
       text = NumberConversionHelper.CleanWhitespace(text);
       text = NumberConversionHelper.RemoveSpecialCharacters(text, keepSpaces: true);
       
       return text;
   }
   ```

### Debug Tips:

1. **Enable Detailed Logging**
   ```csharp
   public class LoggingValidator : PhoneNumberValidator
   {
       private readonly ILogger _logger;
       
       public LoggingValidator(ILogger logger)
       {
           _logger = logger;
       }
       
       public override PhoneNumberInfo Validate(string phoneNumber)
       {
           _logger.LogInformation($"Validating phone: {phoneNumber}");
           var result = base.Validate(phoneNumber);
           _logger.LogInformation($"Result: Valid={result.IsValid}, Carrier={result.Carrier}");
           return result;
       }
   }
   ```

2. **Use Validation Events**
   ```csharp
   public class EventedParser : NationalIdParser
   {
       public event EventHandler<ValidationEventArgs> OnValidation;
       
       public override NationalIdInfo Parse(string nationalId)
       {
           var result = base.Parse(nationalId);
           
           OnValidation?.Invoke(this, new ValidationEventArgs
           {
               Input = nationalId,
               Result = result,
               Timestamp = DateTime.Now
           });
           
           return result;
       }
   }
   ```

---

## 📚 Additional Resources

### Documentation:
- [API Reference](docs/api.md) - Complete API documentation
- [Migration Guide](docs/migration.md) - Upgrading from previous versions
- [Performance Tips](docs/performance.md) - Optimization guide
- [Arabic Text Guide](docs/arabic-text.md) - Working with Arabic text

### Samples:
- [Web API Example](samples/WebAPI/) - Complete ASP.NET Core API
- [Console App](samples/ConsoleApp/) - Command-line interface
- [Blazor App](samples/BlazorApp/) - WebAssembly application
- [Desktop App](samples/DesktopApp/) - Windows Forms/WPF
- [Mobile App](samples/MobileApp/) - Xamarin/.NET MAUI

### Testing:
- [Unit Tests](tests/UnitTests/) - Complete test suite
- [Integration Tests](tests/IntegrationTests/) - System integration tests
- [Performance Tests](tests/PerformanceTests/) - Load and stress testing

---

## 📄 License

MIT License

Copyright (c) 2024 NumericValidation.EG

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

---

## 🆘 Support



### Community:


### Enterprise Support:
- **Priority Support**: Available for enterprise customers
- **Custom Development**: Tailored solutions for your business
- **Training & Workshops**: On-site or remote training sessions

---

**Happy Coding!** 🚀

*Built with ❤️ for the Egyptian developer community*


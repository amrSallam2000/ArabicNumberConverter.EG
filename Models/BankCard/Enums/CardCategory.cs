namespace NumericValidation.EG.Models.BankCard.Enums
{
    /// <summary>
    /// Card tier / category as defined by the card network or issuing bank.
    /// Includes all major global categories plus Egypt-specific and regional tiers.
    /// Last Updated: February 2025
    /// </summary>
    public enum CardCategory
    {
        /// <summary>Category could not be determined.</summary>
        Unknown = 0,

        // =============== BASIC / ENTRY LEVEL ===============

        /// <summary>Classic / Standard — entry-level tier.</summary>
        Classic = 1,

        /// <summary>Standard — basic tier, often synonymous with Classic.</summary>
        Standard = 2,

        /// <summary>Entry — basic card with minimal benefits.</summary>
        Entry = 3,

        /// <summary>Electron — Visa Electron basic tier.</summary>
        Electron = 4,

        // =============== MID-TIER ===============

        /// <summary>Gold — mid-tier with enhanced benefits.</summary>
        Gold = 5,

        /// <summary>Silver — tier between Classic and Gold (less common).</summary>
        Silver = 6,

        /// <summary>Bronze — basic tier with limited benefits.</summary>
        Bronze = 7,

        // =============== PREMIUM TIERS ===============

        /// <summary>Platinum — premium tier.</summary>
        Platinum = 8,

        /// <summary>Signature (Visa) — high-end tier with travel and lifestyle perks.</summary>
        Signature = 9,

        /// <summary>Infinite (Visa) — top-tier Visa.</summary>
        Infinite = 10,

        /// <summary>World (Mastercard) — premium international tier.</summary>
        World = 11,

        /// <summary>World Elite (Mastercard) — top-tier Mastercard.</summary>
        WorldElite = 12,

        /// <summary>Black Card — ultra-premium exclusive tier.</summary>
        Black = 13,

        /// <summary>Centurion (American Express) — invitation-only top tier.</summary>
        Centurion = 14,

        // =============== BUSINESS / CORPORATE ===============

        /// <summary>Business — designed for small-to-medium business use.</summary>
        Business = 15,

        /// <summary>Corporate — for large corporations.</summary>
        Corporate = 16,

        /// <summary>Commercial — business-focused with expense management.</summary>
        Commercial = 17,

        /// <summary>Executive — high-level corporate tier.</summary>
        Executive = 18,

        /// <summary>Business Platinum — business premium tier.</summary>
        BusinessPlatinum = 19,

        /// <summary>Business Gold — business mid-tier.</summary>
        BusinessGold = 20,

        /// <summary>Business World Elite — top-tier business Mastercard.</summary>
        BusinessWorldElite = 21,

        /// <summary>Business Infinite — top-tier business Visa.</summary>
        BusinessInfinite = 22,

        // =============== STUDENT / YOUTH ===============

        /// <summary>Student — designed for university students.</summary>
        Student = 23,

        /// <summary>Youth — for young adults/teenagers.</summary>
        Youth = 24,

        /// <summary>Campus — university-specific cards.</summary>
        Campus = 25,

        /// <summary>Graduate — for recent graduates.</summary>
        Graduate = 26,

        // =============== REWARDS / SPECIALIZED ===============

        /// <summary>Rewards — points-based reward card.</summary>
        Rewards = 27,

        /// <summary>Cashback — cashback rewards card.</summary>
        Cashback = 28,

        /// <summary>Travel — travel-focused rewards.</summary>
        Travel = 29,

        /// <summary>Miles — airline miles card.</summary>
        Miles = 30,

        /// <summary>Shopping — retail shopping rewards.</summary>
        Shopping = 31,

        /// <summary>Dining — restaurant/food rewards.</summary>
        Dining = 32,

        /// <summary>Entertainment — movies, events, etc.</summary>
        Entertainment = 33,

        /// <summary>Lifestyle — lifestyle and wellness perks.</summary>
        Lifestyle = 34,

        // =============== ISLAMIC BANKING ===============

        /// <summary>Islamic — Sharia-compliant card.</summary>
        Islamic = 35,

        /// <summary>Islamic Gold — Sharia-compliant gold tier.</summary>
        IslamicGold = 36,

        /// <summary>Islamic Platinum — Sharia-compliant platinum tier.</summary>
        IslamicPlatinum = 37,

        /// <summary>Islamic Business — Sharia-compliant business card.</summary>
        IslamicBusiness = 38,

        /// <summary>Murabaha — Islamic financing card.</summary>
        Murabaha = 39,

        /// <summary>Ijara — Islamic leasing card.</summary>
        Ijara = 40,

        /// <summary>Takaful — Islamic insurance-linked card.</summary>
        Takaful = 41,

        // =============== PREPAID / DIGITAL ===============

        /// <summary>Prepaid — loaded with funds in advance.</summary>
        Prepaid = 42,

        /// <summary>Virtual — digital-only card for online use.</summary>
        Virtual = 43,

        /// <summary>Gift Card — prepaid gift card.</summary>
        GiftCard = 44,

        /// <summary>Travel Card — prepaid travel money card.</summary>
        TravelCard = 45,

        /// <summary>Payroll — salary/employee payment card.</summary>
        Payroll = 46,

        /// <summary>Government Benefit — social benefits/disbursement card.</summary>
        GovernmentBenefit = 47,

        // =============== CO-BRANDED / AFFINITY ===============

        /// <summary>Co-Branded — partnered with another brand.</summary>
        CoBranded = 48,

        /// <summary>Affinity — charitable/organizational support card.</summary>
        Affinity = 49,

        /// <summary>Sports — sports club/team branded.</summary>
        Sports = 50,

        /// <summary>University — university alumni/affiliation.</summary>
        University = 51,

        /// <summary>Military — armed forces personnel.</summary>
        Military = 52,

        /// <summary>Medical — healthcare professional.</summary>
        Medical = 53,

        // =============== EGYPT-SPECIFIC CATEGORIES ===============

        /// <summary>Meeza Classic — standard Egyptian national card.</summary>
        MeezaClassic = 54,

        /// <summary>Meeza Gold — premium Egyptian national card.</summary>
        MeezaGold = 55,

        /// <summary>Meeza Prepaid — Egyptian prepaid national card.</summary>
        MeezaPrepaid = 56,

        /// <summary>Meeza Digital — virtual Egyptian card for mobile wallets.</summary>
        MeezaDigital = 57,

        /// <summary>Meeza Business — Egyptian business card.</summary>
        MeezaBusiness = 58,

        /// <summary>Meeza Government — Egyptian government employee card.</summary>
        MeezaGovernment = 59,

        /// <summary>Meeza Pension — Egyptian pension disbursement card.</summary>
        MeezaPension = 60,

        /// <summary>Meeza Student — Egyptian student card.</summary>
        MeezaStudent = 61,

        /// <summary>InstaPay Card — Egypt's instant payment card.</summary>
        InstaPay = 62,

        /// <summary>Fawry Prepaid — Fawry's prepaid card.</summary>
        FawryPrepaid = 63,

        /// <summary>Fawry Plus — enhanced Fawry card.</summary>
        FawryPlus = 64,

        /// <summary>Paymob Card — Paymob digital card.</summary>
        PaymobCard = 65,

        /// <summary>MoneyFellows Card — MoneyFellows app card.</summary>
        MoneyFellows = 66,

        /// <summary>Khazna Card — Khazna financial app card.</summary>
        Khazna = 67,

        /// <summary>Telda Card — Telda digital banking card.</summary>
        Telda = 68,

        // =============== E-COMMERCE PLATFORMS ===============

        /// <summary>Amazon Egypt Card — Amazon Egypt co-branded.</summary>
        AmazonEgypt = 69,

        /// <summary>Noon Egypt Card — Noon e-commerce co-branded.</summary>
        NoonEgypt = 70,

        /// <summary>Jumia Card — Jumia co-branded.</summary>
        Jumia = 71,

        /// <summary>Talabat Card — Talabat food delivery.</summary>
        Talabat = 72,

        /// <summary>Uber Card — Uber co-branded.</summary>
        Uber = 73,

        /// <summary>Careem Card — Careem co-branded.</summary>
        Careem = 74,

        /// <summary>Swvl Card — Swvl transportation.</summary>
        Swvl = 75,

        // =============== RIDE-HAILING / TRANSPORT ===============

        /// <summary>Transport Card — general transport card.</summary>
        Transport = 76,

        /// <summary>Metro Card — Cairo Metro card.</summary>
        Metro = 77,

        /// <summary>Bus Card — public bus card.</summary>
        Bus = 78,

        /// <summary>Toll Card — road toll payment card.</summary>
        Toll = 79,

        /// <summary>Fuel Card — petrol/gas station card.</summary>
        Fuel = 80,

        // =============== HEALTHCARE ===============

        /// <summary>Health Card — medical/health insurance card.</summary>
        Health = 81,

        /// <summary>Pharmacy Card — pharmacy loyalty/insurance.</summary>
        Pharmacy = 82,

        /// <summary>Hospital Card — hospital-specific card.</summary>
        Hospital = 83,

        /// <summary>Wellness Card — gym/wellness membership.</summary>
        Wellness = 84,

        // =============== REGIONAL (GCC/MENA) ===============

        /// <summary>GCC Classic — Gulf region classic tier.</summary>
        GCCClassic = 85,

        /// <summary>GCC Gold — Gulf region gold tier.</summary>
        GCCGold = 86,

        /// <summary>GCC Platinum — Gulf region platinum tier.</summary>
        GCCPlatinum = 87,

        /// <summary>GCC Signature — Gulf region signature tier.</summary>
        GCCSignature = 88,

        /// <summary>GCC Infinite — Gulf region infinite tier.</summary>
        GCCInfinite = 89,

        /// <summary>Mada Classic — Saudi classic card.</summary>
        MadaClassic = 90,

        /// <summary>Mada Gold — Saudi gold card.</summary>
        MadaGold = 91,

        /// <summary>Mada Platinum — Saudi platinum card.</summary>
        MadaPlatinum = 92,

        /// <summary>KNET Classic — Kuwait classic card.</summary>
        KNETClassic = 93,

        /// <summary>KNET Gold — Kuwait gold card.</summary>
        KNETGold = 94,

        /// <summary>Benefit Classic — Bahrain classic card.</summary>
        BenefitClassic = 95,

        /// <summary>Benefit Gold — Bahrain gold card.</summary>
        BenefitGold = 96,

        /// <summary>OmanNet Classic — Oman classic card.</summary>
        OmanNetClassic = 97,

        /// <summary>OmanNet Gold — Oman gold card.</summary>
        OmanNetGold = 98,

        /// <summary>NAPS Qatar Classic — Qatar classic card.</summary>
        NAPSQatarClassic = 99,

        /// <summary>NAPS Qatar Gold — Qatar gold card.</summary>
        NAPSQatarGold = 100,

        // =============== INTERNATIONAL PREMIUM VARIANTS ===============

        /// <summary>Reserve — ultra-premium (e.g., Chase Sapphire Reserve).</summary>
        Reserve = 101,

        /// <summary>Prestige — Citi Prestige level.</summary>
        Prestige = 102,

        /// <summary>Private Bank — for private banking clients.</summary>
        PrivateBank = 103,

        /// <summary>Wealth — wealth management clients.</summary>
        Wealth = 104,

        /// <summary>Diamond — exclusive diamond tier.</summary>
        Diamond = 105,

        /// <summary>Elite — elite tier.</summary>
        Elite = 106,

        /// <summary>Exclusive — exclusive access tier.</summary>
        Exclusive = 107,

        // =============== ADDITIONAL SPECIALIZED ===============

        /// <summary>Secured — secured/credit-builder card.</summary>
        Secured = 108,

        /// <summary>Charge Card — full balance due monthly (e.g., Amex).</summary>
        Charge = 109,

        /// <summary>Credit — standard credit card.</summary>
        Credit = 110,

        /// <summary>Debit — standard debit card.</summary>
        Debit = 111,

        /// <summary>ATM Only — cash withdrawal only.</summary>
        ATMOnly = 112,

        /// <summary>Microfinance — microfinance institution card.</summary>
        Microfinance = 113,

        /// <summary>SME — Small and Medium Enterprise.</summary>
        SME = 114,

        /// <summary>Startup — startup business card.</summary>
        Startup = 115,

        /// <summary>Freelancer — freelancer-focused card.</summary>
        Freelancer = 116,

        /// <summary>Digital Nomad — for remote workers.</summary>
        DigitalNomad = 117,

        /// <summary>Expat — for expatriates.</summary>
        Expat = 118,

        /// <summary>Non-Resident — for non-resident accounts.</summary>
        NonResident = 119,

        /// <summary>Joint Account — shared account card.</summary>
        Joint = 120,

        /// <summary>Supplementary — additional card on account.</summary>
        Supplementary = 121,

        /// <summary>Authorized User — card for authorized user.</summary>
        AuthorizedUser = 122,
                    /// <summary>Titanium — super-premium tier above Platinum (used by some networks and issuers).</summary>
        Titanium = 123,

    }
}
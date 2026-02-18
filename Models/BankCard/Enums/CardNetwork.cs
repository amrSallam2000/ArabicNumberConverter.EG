namespace NumericValidation.EG.Models.BankCard.Enums
{
    /// <summary>
    /// International payment card network (scheme) as defined by IIN prefix ranges
    /// under ISO/IEC 7812-1:2017 and national payment systems.
    /// Last Updated: February 2025
    /// </summary>
    public enum CardNetwork
    {
        /// <summary>Network could not be determined from the IIN prefix.</summary>
        Unknown = 0,

        /// <summary>Visa — prefix 4x.</summary>
        Visa = 1,

        /// <summary>Mastercard — prefix 51-55 or 2221-2720.</summary>
        Mastercard = 2,

        /// <summary>American Express (Amex) — prefix 34 or 37. CVV is 4 digits (CID).</summary>
        AmericanExpress = 3,

        /// <summary>Discover — prefix 6011, 622126-622925, 644-649, 65.</summary>
        Discover = 4,

        /// <summary>UnionPay — prefix 62. Largest network by cards issued globally.</summary>
        UnionPay = 5,

        /// <summary>JCB (Japan Credit Bureau) — prefix 3528-3589.</summary>
        JCB = 6,

        /// <summary>Diners Club International — prefix 300-305, 36, 38-39.</summary>
        DinersClub = 7,

        /// <summary>
        /// Meeza — Egypt's national payment card network.
        /// Launched by the Central Bank of Egypt (CBE). IIN range: 507800-507809.
        /// بطاقة ميزة — شبكة الدفع الوطنية المصرية.
        /// </summary>
        Meeza = 8,

        /// <summary>Maestro (Mastercard debit/prepaid) — prefix 6304, 6759, 676770, 676774.</summary>
        Maestro = 9,

        /// <summary>Mastercard Debit — a debit-specific sub-brand of Mastercard.</summary>
        MastercardDebit = 10,

        /// <summary>Visa Electron — debit-only variant of Visa; prefix 4026, 417500, 4508, 4844, 4913, 4917.</summary>
        VisaElectron = 11,

        /// <summary>Verve — Nigerian domestic payment network.</summary>
        Verve = 12,

        /// <summary>MIR — Russian national payment system. Prefix 2200-2204.</summary>
        MirCard = 13,

        /// <summary>Elo — Brazilian domestic payment network.</summary>
        Elo = 14,

        // =============== ADDITIONAL NETWORKS FOR COMPREHENSIVE COVERAGE ===============

        /// <summary>
        /// UATP (Universal Air Travel Plan) — Airline travel card network.
        /// Prefix: 1 | PAN Length: 15 digits
        /// </summary>
        UATP = 15,

        /// <summary>
        /// Troy — Turkish domestic payment system.
        /// Range: 979200-979289
        /// </summary>
        Troy = 16,

        /// <summary>
        /// Interpayment — Russian domestic payment system.
        /// Range: 212-217
        /// </summary>
        Interpayment = 17,

        /// <summary>
        /// NPS (National Payment System) — Kazakhstan domestic payment system.
        /// Prefix: 98
        /// </summary>
        NPSKazakhstan = 18,

        /// <summary>
        /// RuPay — Indian domestic payment system.
        /// Ranges: 60, 81, 82, 508, 353, 356
        /// </summary>
        Rupay = 19,

        /// <summary>
        /// Dankort — Danish national payment card.
        /// Prefix: 4571 (Visa co-branded)
        /// </summary>
        Dankort = 20,

        /// <summary>
        /// Carte Bancaire (CB) — French domestic payment network.
        /// Prefix: 497538, 497539
        /// </summary>
        CarteBancaire = 21,

        /// <summary>
        /// GIM (Interbank Monetary Group) — Moroccan domestic payment network.
        /// Prefix: 604626
        /// </summary>
        GIM_UEMOA = 22,

        /// <summary>
        /// BancNet — Philippine domestic payment network.
        /// Prefix: 516390, 547659
        /// </summary>
        BancNet = 23,

        /// <summary>
        /// LankaPay — Sri Lankan domestic payment network.
        /// Prefix: 627053
        /// </summary>
        LankaPay = 24,

        /// <summary>
        /// BC Card — South Korean domestic payment network.
        /// Prefix: 625094
        /// </summary>
        BCCard = 25,

        /// <summary>
        /// KSNET — South Korean domestic payment network.
        /// Prefix: 625094 (co-branded)
        /// </summary>
        KSNET = 26,

        /// <summary>
        /// Napas — Vietnamese domestic payment network.
        /// Prefix: 9704 (co-branded with international schemes)
        /// </summary>
        Napas = 27,

        /// <summary>
        /// MyDebit — Malaysian domestic debit scheme.
        /// Prefix: 6282
        /// </summary>
        MyDebit = 28,

        /// <summary>
        /// PayPak — Pakistani domestic payment scheme.
        /// Launched by SBP (State Bank of Pakistan)
        /// </summary>
        PayPak = 29,

        /// <summary>
        /// ZKA (Zentraler Kreditausschuss) — German domestic debit scheme (Girocard).
        /// Prefix: 414853, 417005, 474491
        /// </summary>
        Girocard = 30,

        /// <summary>
        /// China UnionPay ExpressPay — UnionPay contactless variant.
        /// Same IIN ranges as UnionPay (62)
        /// </summary>
        ExpressPay = 31,

        /// <summary>
        /// RuPay JCB — Co-branded RuPay and JCB cards (India/Japan).
        /// Prefix: 65 (varies by issuer)
        /// </summary>
        RuPayJCB = 32,

        /// <summary>
        /// VISA QIWI — Co-branded Visa and QIWI (Russia).
        /// </summary>
        VisaQIWI = 33,

        /// <summary>
        /// Mastercard QIWI — Co-branded Mastercard and QIWI (Russia).
        /// </summary>
        MastercardQIWI = 34,

        /// <summary>
        /// Meeza Prepaid — Egypt's national prepaid card variant.
        /// Still uses Meeza IIN range but with prepaid configuration.
        /// بطاقة ميزة مسبقة الدفع
        /// </summary>
        MeezaPrepaid = 35,

        /// <summary>
        /// Meeza Contactless — Egypt's contactless-enabled Meeza cards.
        /// Uses same IIN range as Meeza but with contactless technology.
        /// ميزة لا تلامسية
        /// </summary>
        MeezaContactless = 36,

        /// <summary>
        /// Meeza Digital — Virtual Meeza cards for e-commerce and mobile wallets.
        /// ميزة رقمية
        /// </summary>
        MeezaDigital = 37,

        /// <summary>
        /// InstaPay — Egypt's instant payment network card.
        /// بطاقة إنستاباي
        /// </summary>
        InstaPay = 38,

        /// <summary>
        /// Fawry Card — Fawry's own payment cards.
        /// بطاقة فوري
        /// </summary>
        FawryCard = 39,

        /// <summary>
        /// Paymob Card — Paymob's digital payment cards.
        /// بطاقة باي موب
        /// </summary>
        PaymobCard = 40,

        /// <summary>
        /// Amazon Egypt Card — Co-branded cards for Amazon Egypt.
        /// بطاقة أمازون مصر
        /// </summary>
        AmazonEgypt = 41,

        /// <summary>
        /// Souq Card — Former Souq.com (now Amazon) cards.
        /// بطاقة سوق.كوم
        /// </summary>
        SouqCard = 42,

        /// <summary>
        /// Noon Egypt Card — Co-branded cards for Noon e-commerce platform.
        /// بطاقة نون مصر
        /// </summary>
        NoonEgypt = 43,

        /// <summary>
        /// Talabat Card — Co-branded cards for Talabat food delivery.
        /// بطاقة طلبات
        /// </summary>
        TalabatCard = 44,

        /// <summary>
        /// Uber Egypt Card — Co-branded cards for Uber Egypt.
        /// بطاقة أوبر مصر
        /// </summary>
        UberEgypt = 45,

        /// <summary>
        /// Careem Egypt Card — Co-branded cards for Careem Egypt.
        /// بطاقة كريم مصر
        /// </summary>
        CareemEgypt = 46,

        /// <summary>
        /// Swvl Card — Co-branded cards for Swvl transportation.
        /// بطاقة سويفل
        /// </summary>
        SwvlCard = 47,

        /// <summary>
        /// SolarisBank — German digital banking platform cards.
        /// </summary>
        SolarisBank = 48,

        /// <summary>
        /// Railsbank — Global banking-as-a-service platform cards.
        /// </summary>
        Railsbank = 49,

        /// <summary>
        /// Marqeta — Modern card issuing platform cards.
        /// </summary>
        Marqeta = 50,

        /// <summary>
        /// Visa GCC — Visa cards issued in Gulf Cooperation Council countries.
        /// فيزا دول الخليج
        /// </summary>
        VisaGCC = 51,

        /// <summary>
        /// Mastercard GCC — Mastercard cards issued in Gulf Cooperation Council countries.
        /// ماستركارد دول الخليج
        /// </summary>
        MastercardGCC = 52,

        /// <summary>
        /// KNET — Kuwait's national payment network.
        /// الشبكة الوطنية الكويتية
        /// </summary>
        KNET = 53,

        /// <summary>
        /// mada — Saudi Arabia's national payment network.
        /// مدى - الشبكة الوطنية السعودية
        /// </summary>
        Mada = 54,

        /// <summary>
        /// Benefit — Bahrain's national payment network.
        /// بينفت - الشبكة الوطنية البحرينية
        /// </summary>
        Benefit = 55,

        /// <summary>
        /// OmanNet — Oman's national payment network.
        /// عمان نت - الشبكة الوطنية العمانية
        /// </summary>
        OmanNet = 56,

        /// <summary>
        /// NAPS — Qatar's national payment network.
        /// نظام المدفوعات الوطني القطري
        /// </summary>
        NAPS_Qatar = 57,

        /// <summary>
        /// Sadad — UAE's bill payment system (sometimes appears on cards).
        /// سداد - الإمارات
        /// </summary>
        Sadad_UAE = 58,

        /// <summary>
        /// URPay — UAE's domestic payment scheme.
        /// </summary>
        URPay_UAE = 59
    }
}
using NumericValidation.EG.Models.BankCard.Info;
using NumericValidation.EG.Models.BankCard.Validator;

namespace NumericValidation.EG.Models.BankCard.Enums
{
    // ═══════════════════════════════════════════════════════════════════════════
    //  ArabicNumberConverter.EG.BankCard — Enums
    //  Compliant with ISO/IEC 7812-1:2017 and CBE Regulations 2024-2025
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Preferred display language for bilingual fields (English / Arabic).
    /// Pass this to <see cref="BankCardValidator"/> to control which language
    /// is surfaced on <see cref="BankCardInfo"/> primary (non-suffixed) properties.
    /// Both languages are always stored; this only changes which one is "primary".
    /// </summary>
    public enum DisplayLanguage
    {
        /// <summary>Display primary fields in English (default).</summary>
        English = 0,

        /// <summary>عرض الحقول الأساسية باللغة العربية.</summary>
        Arabic = 1,
    }
}

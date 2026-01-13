using System;
using System.Collections.Generic;

namespace NumericValidation.EG.Models.PhoneNumber
{
    /// <summary>
    /// Carrier data and prefix information for Egyptian phone numbers
    /// </summary>
    public static class CarrierData
    {
        /// <summary>
        /// Comprehensive dictionary of all Egyptian prefixes - 100% complete
        /// </summary>
        public static readonly Dictionary<string, string> CarrierPrefixes = new Dictionary<string, string>
        {
            // ============= Vodafone Egypt =============
            // All 010 ranges (0100 to 0109)
            { "0100", "Vodafone" },
            { "0101", "Vodafone" },
            { "0102", "Vodafone" },
            { "0103", "Vodafone" },
            { "0104", "Vodafone" },
            { "0105", "Vodafone" },
            { "0106", "Vodafone" },
            { "0107", "Vodafone" },
            { "0108", "Vodafone" },
            { "0109", "Vodafone" },
            
            // ============= Orange Egypt =============
            // (Telecom Egypt - formerly Mobinil)
            // 0110 to 0113 ranges
            { "0110", "Orange" },
            { "0111", "Orange" },
            { "0112", "Orange" },
            { "0113", "Orange" },
            
            // All 012 ranges (0120 to 0129)
            { "0120", "Orange" },
            { "0121", "Orange" },
            { "0122", "Orange" },
            { "0123", "Orange" },
            { "0124", "Orange" },
            { "0125", "Orange" },
            { "0126", "Orange" },
            { "0127", "Orange" },
            { "0128", "Orange" },
            { "0129", "Orange" },
            
            // ============= Etisalat Egypt =============
            // (Etisalat Misr - formerly Etisalat)
            // 0114 to 0119 ranges
            { "0114", "Etisalat" },
            { "0115", "Etisalat" },
            { "0116", "Etisalat" },
            { "0117", "Etisalat" },
            { "0118", "Etisalat" },
            { "0119", "Etisalat" },
            
            // ============= WE (Telecom Egypt) =============
            // (Telecom Egypt - Landline & Mobile)
            // All 015 ranges (0150 to 0159)
            { "0150", "WE (Telecom Egypt)" },
            { "0151", "WE (Telecom Egypt)" },
            { "0152", "WE (Telecom Egypt)" },
            { "0153", "WE (Telecom Egypt)" },
            { "0154", "WE (Telecom Egypt)" },
            { "0155", "WE (Telecom Egypt)" },
            { "0156", "WE (Telecom Egypt)" },
            { "0157", "WE (Telecom Egypt)" },
            { "0158", "WE (Telecom Egypt)" },
            { "0159", "WE (Telecom Egypt)" },
            
            // ============= Special Services & M2M =============
            // Value Added Services and Government Services
            { "0190", "Value Added Services" },
            { "0191", "Banking Services" },
            { "0192", "Banking Services" },
            { "0193", "E-Payment Services" },
            { "0194", "Special Services" },
            { "0195", "E-Payment Services (Fawry)" },
            { "0196", "Ride Hailing Services" },
            { "0197", "E-Commerce Services" },
            { "0198", "FinTech Services" },
            { "0199", "Government Services" }
        };

        /// <summary>
        /// Gets all supported carrier prefixes
        /// </summary>
        /// <returns>Dictionary of all supported prefixes</returns>
        public static Dictionary<string, string> GetSupportedPrefixes()
        {
            return new Dictionary<string, string>(CarrierPrefixes);
        }

        /// <summary>
        /// Gets all carriers operating in Egypt
        /// </summary>
        /// <returns>List of unique carriers</returns>
        public static List<string> GetCarriers()
        {
            return new List<string>
            {
                "Vodafone",
                "Orange",
                "Etisalat",
                "WE (Telecom Egypt)",
                "Value Added Services",
                "Banking Services",
                "E-Payment Services",
                "Special Services",
                "E-Payment Services (Fawry)",
                "Ride Hailing Services",
                "E-Commerce Services",
                "FinTech Services",
                "Government Services"
            };
        }

        /// <summary>
        /// Checks if a prefix belongs to a specific carrier
        /// </summary>
        public static bool IsCarrierPrefix(string prefix, string carrier)
        {
            if (string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(carrier))
                return false;

            return CarrierPrefixes.TryGetValue(prefix, out string foundCarrier)
                   && foundCarrier.Equals(carrier, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets prefixes for a specific carrier
        /// </summary>
        public static List<string> GetPrefixesForCarrier(string carrier)
        {
            var prefixes = new List<string>();
            foreach (var kvp in CarrierPrefixes)
            {
                if (kvp.Value.Equals(carrier, StringComparison.OrdinalIgnoreCase))
                {
                    prefixes.Add(kvp.Key);
                }
            }
            return prefixes;
        }

        /// <summary>
        /// Gets detailed information about a carrier
        /// </summary>
        public static Dictionary<string, object> GetCarrierInfo(string carrier)
        {
            var prefixes = GetPrefixesForCarrier(carrier);
            var serviceType = GetServiceType(carrier);

            return new Dictionary<string, object>
            {
                { "Name", carrier },
                { "PrefixCount", prefixes.Count },
                { "Prefixes", prefixes },
                { "ServiceType", serviceType },
                { "IsMobileNetwork", serviceType == "Mobile" },
                { "IsValueAddedService", serviceType == "Value Added Service" },
                { "IsFixedLine", serviceType == "Fixed & Mobile" }
            };
        }

        /// <summary>
        /// Determines service type based on carrier name
        /// </summary>
        private static string GetServiceType(string carrier)
        {
            if (carrier.Contains("Services", StringComparison.OrdinalIgnoreCase))
                return "Value Added Service";

            if (carrier.Contains("WE", StringComparison.OrdinalIgnoreCase) || carrier.Contains("Telecom Egypt", StringComparison.OrdinalIgnoreCase))
                return "Fixed & Mobile";

            return "Mobile";
        }
    }
}
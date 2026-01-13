using System;
using System.Collections.Generic;
using System.Linq;
namespace NumericValidation.EG.Models.NationalNumber.Constants
{
    /// <summary>
    /// Generation names and date ranges for birth years
    /// </summary>
    public static class GenerationConstants
    {
        /// <summary>
        /// List of generations with their birth year ranges
        /// </summary>
        public static readonly List<Generation> Generations = new()
        {
            new Generation("Lost Generation", "الجيل الضائع", 1883, 1900),
            new Generation("Greatest Generation", "الجيل الأعظم", 1901, 1927),
            new Generation("Silent Generation", "الجيل الصامت", 1928, 1945),
            new Generation("Baby Boomers", "جيل الطفرة السكانية", 1946, 1964),
            new Generation("Generation X", "الجيل إكس", 1965, 1980),
            new Generation("Millennials", "جيل الألفية", 1981, 1996),
            new Generation("Generation Z", "جيل زد", 1997, 2012),
            new Generation("Generation Alpha", "جيل ألفا", 2013, 2025)
        };

        /// <summary>
        /// Represents a generation with multilingual names
        /// </summary>
        /// <param name="English">English name of the generation</param>
        /// <param name="Arabic">Arabic name of the generation</param>
        /// <param name="StartYear">Start year of the generation</param>
        /// <param name="EndYear">End year of the generation</param>
        public record Generation(
            string English,
            string Arabic,
            int StartYear,
            int EndYear
        );

        /// <summary>
        /// Gets the generation for a specific birth year
        /// </summary>
        /// <param name="birthYear">Birth year to check</param>
        /// <returns>Generation information</returns>
        public static Generation GetGeneration(int birthYear)
        {
            return Generations.FirstOrDefault(g => birthYear >= g.StartYear && birthYear <= g.EndYear)
                ?? Generations.Last(); // Default to latest generation
        }

        /// <summary>
        /// Gets English generation name for a birth year
        /// </summary>
        public static string GetGenerationEnglish(int birthYear)
        {
            return GetGeneration(birthYear).English;
        }

        /// <summary>
        /// Gets Arabic generation name for a birth year
        /// </summary>
        public static string GetGenerationArabic(int birthYear)
        {
            return GetGeneration(birthYear).Arabic;
        }

        /// <summary>
        /// Checks if a year belongs to a specific generation
        /// </summary>
        public static bool IsInGeneration(int birthYear, string generationName)
        {
            var generation = GetGeneration(birthYear);
            return generation.English.Equals(generationName, StringComparison.OrdinalIgnoreCase) ||
                   generation.Arabic.Equals(generationName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets all people born in a specific generation
        /// </summary>
        public static (int StartYear, int EndYear) GetGenerationYears(string generationName)
        {
            var generation = Generations.FirstOrDefault(g =>
                g.English.Equals(generationName, StringComparison.OrdinalIgnoreCase) ||
                g.Arabic.Equals(generationName, StringComparison.OrdinalIgnoreCase));

            return generation != null
                ? (generation.StartYear, generation.EndYear)
                : (1900, DateTime.Now.Year);
        }
    }
}
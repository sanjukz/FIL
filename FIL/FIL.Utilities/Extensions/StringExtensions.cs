namespace FIL.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string inString) => string.IsNullOrWhiteSpace(inString);

        public static bool IsNullOrBlank(this string inString) => string.IsNullOrWhiteSpace(inString?.Trim());
    }
}
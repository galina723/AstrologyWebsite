using System.Text.RegularExpressions;

namespace AstrologyWebsite.Helper
{
    public static class StringExtensions
    {
        public static string StripHtml(this string source)
        {
            if (string.IsNullOrWhiteSpace(source)) return string.Empty;
            return Regex.Replace(source, "<.*?>", string.Empty);
        }
    }

}

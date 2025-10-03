using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EduLog.CoreLayer.Utilities
{
    public static class TextHelper
    {
        public static string ToSlug(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = text.Trim().ToLowerInvariant();

            // نرمال‌سازی یونیکد (مثلا "ي" عربی → "ی" فارسی)
            text = text.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();
            foreach (var ch in text)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc == UnicodeCategory.LowercaseLetter ||
                    uc == UnicodeCategory.UppercaseLetter ||
                    uc == UnicodeCategory.DecimalDigitNumber)
                {
                    sb.Append(ch);
                }
                else if (char.IsWhiteSpace(ch) || ch == '-' || ch == '_')
                {
                    sb.Append('-');
                }
            }

            text = sb.ToString();
            text = Regex.Replace(text, "-{2,}", "-");
            text = text.Trim('-');

            return text;
        }
    }
}
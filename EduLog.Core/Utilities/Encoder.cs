namespace EduLog.CoreLayer.Utilities
{
    public static class Encoder
    {
        /// <summary>
        /// تولید هش با استفاده از الگوریتم BCrypt (برای گذرواژه‌ها توصیه می‌شود)
        /// </summary>
        public static string EncodeToBcrypt(this string text, int workFactor = 12)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("متن ورودی نمی‌تواند خالی باشد", nameof(text));

            return BCrypt.Net.BCrypt.HashPassword(text, workFactor);
        }

        /// <summary>
        /// مقایسه متن با هش BCrypt
        /// </summary>
        public static bool CompareWithBcrypt(this string hashedText, string plainText)
        {
            if (string.IsNullOrEmpty(hashedText))
                throw new ArgumentException("هش نمی‌تواند خالی باشد", nameof(hashedText));

            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("متن اصلی نمی‌تواند خالی باشد", nameof(plainText));

            return BCrypt.Net.BCrypt.Verify(plainText, hashedText);
        }
    }
}
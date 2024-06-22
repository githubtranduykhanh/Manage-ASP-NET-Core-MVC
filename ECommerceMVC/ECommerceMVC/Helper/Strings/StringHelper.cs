using System.Globalization;
using System.Text;

namespace ECommerceMVC.Helper.Strings
{
    public class StringHelper
    {
        public static string NormalizeUsername(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Loại bỏ các ký tự có dấu và chuẩn hóa chuỗi
            var normalized = new StringBuilder();
            foreach (char c in input.Normalize(NormalizationForm.FormKD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    if (!char.IsWhiteSpace(c)) // Loại bỏ các khoảng trắng
                    {
                        normalized.Append(char.ToLower(c)); // Chuyển đổi chữ hoa thành chữ thường
                    }
                }
            }

            return normalized.ToString();
        }

    }
}
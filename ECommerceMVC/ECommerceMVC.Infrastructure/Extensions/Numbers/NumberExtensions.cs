using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMVC.Infrastructure.Extensions.Numbers
{
    public static class NumberExtensions
    {
        public static string ToVietnameseNumberFormat(this int number)
        {
            return number.ToString("N0", new CultureInfo("vi-VN"));
        }

        public static string ToVietnameseNumberFormat(this double number)
        {
            return number.ToString("N0", new CultureInfo("vi-VN"));
        }
    }
}

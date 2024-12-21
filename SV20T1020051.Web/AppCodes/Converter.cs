using System;
using System.Globalization;

namespace SV20T1020051.Web
{
	public static class Converter
	{
        /// <summary>
        /// Chuyển chuỗi s sang giá trị kiểu DateTime (nếu không thành công trả về null)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string s, string formats = "d/M/yyyy; d-M-yyyy;d.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, formats.Split(';'), CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
    }
}


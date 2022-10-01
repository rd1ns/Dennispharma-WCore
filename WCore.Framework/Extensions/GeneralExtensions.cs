using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Framework.Extensions
{

    public static class GeneralExtensions
    {
        #region Number
        public static int MonthDifference(this DateTime now, DateTime date)
        {
            return (now.Month - date.Month) + 12 * (now.Year - date.Year);
        }
        public static int ToInt(this object data)
        {
            try
            {
                return Convert.ToInt32(data);
            }
            catch
            {
                return 0;
            }
        }

        public static decimal ToDecimal(this object data)
        {
            try
            {
                return Convert.ToDecimal(data);
            }
            catch
            {
                return 0;
            }
        }

        public static double ToDouble(this object data)
        {
            try
            {
                return Convert.ToDouble(data);
            }
            catch
            {
                return 0;
            }
        }

        public static DateTime ToDateTime(this object data)
        {
            try
            {
                return Convert.ToDateTime(data);
            }
            catch
            {
                var now = DateTime.Now;
                return now;
            }
        }
        public static DateTime? ToNullableDateTime(this object data)
        {
            try
            {
                return Convert.ToDateTime(data);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Byte
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(_FileName, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);

                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                _FileStream.Close();

                _FileStream.Dispose();

                return true;
            }
            catch
            { return false; }
        }
        #endregion

        #region Date
        public static double GetTimestamp()
        {
            TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (double)span.TotalSeconds;
        }

        public static double GetTimestamp(this DateTime specificDate)
        {
            TimeSpan span = (specificDate - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (double)span.TotalSeconds;
        }

        public static DateTime OverFlowControl(this DateTime value)
        {
            if (value != null)
            {
                if (value < DateTime.MinValue)
                    value = DateTime.MinValue;
                if (value > DateTime.MaxValue)
                    value = DateTime.MaxValue;
                return value;
            }
            else
                return DateTime.MinValue;
        }
        #endregion

        public static bool ToBool(this object data)
        {
            try
            {
                return Convert.ToBoolean(data);
            }
            catch
            {
                return false;
            }
        }
        public static string ListToHtmlString(this List<string> data)
        {
            string list = string.Empty;
            foreach (var item in data)
            {
                list += item + "</br>";
            }
            return list;
        }
    }
}

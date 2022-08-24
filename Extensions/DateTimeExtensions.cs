using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lighthouse.AF_PayPalIPN.Extensions
{
    public class DateTimeExtension
    {
        public string strDateTime { get; set; }

        public DateTime dtDateTime(string strDatetime)
        {
            var dateTime = DateTime.ParseExact(strDateTime.Replace("PDT","-0700"), "HH:mm:ss MMM dd, yyyy zzz", CultureInfo.InvariantCulture);

            return dateTime;
        }
    }
}
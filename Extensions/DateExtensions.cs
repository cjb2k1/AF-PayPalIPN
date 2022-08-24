using System;
using System.Globalization;

namespace Lighthouse.AF_PayPalIPN.Extensions
{
    public class IPNDateParser
    {
        public DateTime parseDateTime (string strDateTime)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            DateTime dtDateTime;

            if(strDateTime.Contains("PDT"))
            {
                dtDateTime = DateTime.ParseExact(strDateTime.Replace("PDT","-08:00"), "dd-MMM-yy HH:mm:ss zzz", culture);
            }
            else
            {
                dtDateTime = DateTime.ParseExact(strDateTime.Replace("PST","-07:00"), "dd-MMM-yy HH:mm:ss zzz", culture);
            }

            dtDateTime = dtDateTime.ToUniversalTime();

            return dtDateTime;
        }
    }
}
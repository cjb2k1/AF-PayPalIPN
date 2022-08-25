using System;
using Lighthouse.AF_PayPalIPN.Model;

namespace Lighthouse.AF_PayPalIPN.Extensions
{
    public class IPNDateParser
    {
        public DateTime parseDateTime (string strDateTime)
        {
            DateTime dtDateTime;
            char[] delimiterChars = {' ',':',','};
            
            string[] strElements = strDateTime.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);

            var strFormat = new DateTimeFormat
            {
                strHour = strElements[0],
                strMin = strElements[1],
                strSec = strElements[2],
                strMonth = strElements[3],
                strDay = strElements[4],
                strYear = strElements[5],
                strTZ = "Pacific Standard Time"
            };

            var strDate = strFormat.strDay + "-" + strFormat.strMonth + "-" + strFormat.strYear;
            var dtDate = new DateTime();

            dtDate = DateTime.Parse(strDate);

            var strTime = strFormat.strHour + ":" + strFormat.strMin + ":" + strFormat.strSec;

            var dtTime = new DateTime();

            dtTime = DateTime.Parse(strTime);

            dtDateTime = dtDate.Date.Add(dtTime.TimeOfDay);

            TimeZoneInfo tzInfo = TimeZoneInfo.FindSystemTimeZoneById(strFormat.strTZ);

            dtDateTime = TimeZoneInfo.ConvertTimeToUtc(dtDateTime,tzInfo);

            return dtDateTime;
        }
    }
}
using System;

namespace SSCEOfflineRegSchApp.Tools
{
    public class DateTimeToLong
    {
        public static long ConvertToLong(int iYear, int iMonth, int iDay, int iHour, int iMinute, int iSecond)
        {
            //DateTime dt2 = new DateTime(2010, 05, 15, 13, 24, 00);
            if (iHour > 1)
                iHour--;

            DateTime dt2 = new DateTime(iYear, iMonth, iDay, iHour, iMinute, iSecond);
            //DateTime dt = DateTime.SpecifyKind(dt2, DateTimeKind.Unspecified);
            long dt2long = dt2.Ticks;
            DateTime epochTime = new DateTime(1970, 1, 1, 0, 0, 0);
            long epochlong = epochTime.Ticks;
            long timeStamp = (dt2long - epochlong) / 10000;
            return timeStamp;
        }

        public static DateTime ConvertToDateTime(double timeStamp)
        {
            //Java timestamp is millisecods past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(Math.Round(timeStamp / 1000));
            // System.DateTime dtDateTime1 = new DateTime(2010, 5, 19, 18, 21, 27);

            return dtDateTime;
        }

        private static long JavaDateTimetoTimeStamp(DateTime dt2)
        {

            long dt2long = dt2.Ticks;
            DateTime epochTime = new DateTime(1970, 1, 1, 0, 0, 0);
            long epochlong = epochTime.Ticks;
            long timeStamp = (dt2long - epochlong) / 10000;
            return timeStamp;
        }

    }
}

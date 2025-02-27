﻿namespace ToolCrawl.Services.Get_Time_VN;

public class GetTimeZone
{
    public static DateTime GetVNTimeZoneNow()
    {
        DateTime thisTime = DateTime.Now;
        TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        DateTime tstTime = TimeZoneInfo.ConvertTime(thisTime, TimeZoneInfo.Local, tst);
        return tstTime;
    }
}

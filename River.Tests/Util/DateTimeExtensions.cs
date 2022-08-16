/*
 * Copyright (c) 2022 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System;

namespace River.Tests.Util; 

internal static class DateTimeExtensions
{
    private const int DefaultYear = 2022;
    internal static DateTime Jan(this int day, int year = DefaultYear) => new(year, 1, day, 0, 0, 0, DateTimeKind.Utc);
    internal static DateTime Feb(this int day, int year = DefaultYear) => new(year, 2, day, 0, 0, 0, DateTimeKind.Utc);
    internal static DateTime Mar(this int day, int year = DefaultYear) => new(year, 3, day, 0, 0, 0, DateTimeKind.Utc);
    internal static DateTime Apr(this int day, int year = DefaultYear) => new(year, 4, day, 0, 0, 0, DateTimeKind.Utc);
    internal static DateTime May(this int day, int year = DefaultYear) => new(year, 5, day, 0, 0, 0, DateTimeKind.Utc);
    internal static DateTime Jun(this int day, int year = DefaultYear) => new(year, 6, day, 0, 0, 0, DateTimeKind.Utc);
    internal static DateTime Jul(this int day, int year = DefaultYear) => new(year, 7, day, 0, 0, 0, DateTimeKind.Utc);
    internal static DateTime Aug(this int day, int year = DefaultYear) => new(year, 8, day, 0, 0, 0, DateTimeKind.Utc);
    internal static DateTime Sep(this int day, int year = DefaultYear) => new(year, 9, day, 0, 0, 0, DateTimeKind.Utc);
    internal static DateTime Oct(this int day, int year = DefaultYear) => new(year, 10, day, 0, 0, 0, DateTimeKind.Utc);
    internal static DateTime Nov(this int day, int year = DefaultYear) => new(year, 11, day, 0, 0, 0, DateTimeKind.Utc);
    internal static DateTime Dec(this int day, int year = DefaultYear) => new(year, 12, day, 0, 0, 0, DateTimeKind.Utc);

    internal static DateTime Hour(this DateTime date, int hour) => new(date.Year, date.Month, date.Day, hour, date.Minute, date.Second, DateTimeKind.Utc);
    internal static DateTime Minute(this DateTime date, int minute) => new(date.Year, date.Month, date.Day, date.Hour, minute, date.Second, DateTimeKind.Utc);
    internal static DateTime Second(this DateTime date, int second) => new(date.Year, date.Month, date.Day, date.Hour, date.Minute, second, DateTimeKind.Utc);
}
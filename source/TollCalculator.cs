﻿using kodtest.source;
using TollFeeCalculator;

public class TollCalculator
{
    /**
     * (int,int) represents time of day.
     * key value represents the cost for passage.
     */
    private readonly Dictionary<(int, int), int> tollFeeSchedule = new Dictionary<(int, int), int>
    {
        {(6, 0), (int)TollFeePriceInformation.LowTraficPrice},
        {(6, 30), (int)TollFeePriceInformation.MidTraficPrice},
        {(7, 0), (int)TollFeePriceInformation.HighTraficPrice},
        {(8, 0), (int)TollFeePriceInformation.MidTraficPrice},
        {(8, 30), (int)TollFeePriceInformation.LowTraficPrice},
        {(15, 0), (int)TollFeePriceInformation.MidTraficPrice},
        {(15, 30), (int)TollFeePriceInformation.HighTraficPrice},
        {(17, 0), (int)TollFeePriceInformation.MidTraficPrice},
        {(18, 0), (int)TollFeePriceInformation.LowTraficPrice}
    };

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] dates, RedDays redDaysInYear)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle, redDaysInYear);
            int tempFee = GetTollFee(intervalStart, vehicle, redDaysInYear);

            long diffInMillies = date.Millisecond - intervalStart.Millisecond;
            long minutes = diffInMillies / 1000 / 60;

            if (minutes <= 60)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }
        if (totalFee > (int)TollFeePriceInformation.MaxToolFee) totalFee = (int)TollFeePriceInformation.MaxToolFee;
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {

        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        List<string> tollFreeVehicles = Enum.GetNames(typeof(TollFreeVehicles)).ToList();

        return tollFreeVehicles.Contains(vehicleType);
    }

    public int GetTollFee(DateTime date, Vehicle vehicle, RedDays redDaysInYear)
    {
        if (IsTollFreeDate(date, redDaysInYear) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        foreach (var time in tollFeeSchedule.OrderByDescending(e => e.Key))
        {
            var (entryHour, entryMinute) = time.Key;
            if (hour > entryHour || (hour == entryHour && minute >= entryMinute))
                return time.Value;
        }

        return 0;
    }

    private Boolean IsTollFreeDate(DateTime date, RedDays redDaysInYear)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (year == redDaysInYear.Year)
            return redDaysInYear.RedDaysInMonth[month].Contains(day);

        return false;
    }
}
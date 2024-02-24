using System;
using System.Globalization;
using TollFeeCalculator;

public class TollCalculator
{
    /**
     * (int,int) represents time of day.
     * key value represents the cost for passage.
     */
    private readonly Dictionary<(int, int), int> tollFeeSchedule = new Dictionary<(int, int), int>
    {
        {(6, 0), 8},
        {(6, 30), 13},
        {(7, 0), 18},
        {(8, 0), 13},
        {(8, 30), 8},
        {(15, 0), 13},
        {(15, 30), 18},
        {(17, 0), 13},
        {(18, 0), 8}
    };

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle);
            int tempFee = GetTollFee(intervalStart, vehicle);

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
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {

        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        List<string> tollFreeVehicles = Enum.GetNames(typeof(TollFreeVehicles)).ToList();

        return tollFreeVehicles.Contains(vehicleType);
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

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

    private Boolean IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (year == 2013)
        {
            if (month == 1 && day == 1 ||
                month == 3 && (day == 28 || day == 29) ||
                month == 4 && (day == 1 || day == 30) ||
                month == 5 && (day == 1 || day == 8 || day == 9) ||
                month == 6 && (day == 5 || day == 6 || day == 21) ||
                month == 7 ||
                month == 11 && day == 1 ||
                month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
        }
        return false;
    }
}
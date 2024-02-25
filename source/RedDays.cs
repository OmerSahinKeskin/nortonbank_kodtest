using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace kodtest.source
{
    /**
     *  This class is used to handle red days.
     * 
     **/
    public class RedDays
    {
        public int Year {  get; set; }
        public required Dictionary<int, List<int>> RedDaysInMonth { get; set; }

        /**
         * @param year - The year the reddays correspond to.
         */

        public RedDays(int year) {
            Year = year;
            RedDaysInMonth = new Dictionary<int, List<int>>();
        }

        /**
         * @param redDaysInMonth - List of all red days for each month in a year.
         */

        public void AddRedDays(Dictionary<int, List<int>> redDaysInMonth)
        {
            if (RedDaysInMonth.Count() == 0)
            {
                RedDaysInMonth = redDaysInMonth;
            }
            else
            {
                foreach (var month in redDaysInMonth)
                {
                    AddRedDays(month.Key, month.Value);
                }
            }
        }

        /**
         * @param month - Month of the year.
         * @param days - List of red days for the month.
         */

        public void AddRedDays(int month, List<int> days)
        {
            if(!RedDaysInMonth.ContainsKey(month))
            { 
                RedDaysInMonth[month] = days;
            }
            else
            {
                days.ForEach(day => AddRedDay(month, day));
            }

            
        }

        /**
         * @param month - Month of the year.
         * @param day - Red day for the month.
         */

        public void AddRedDay(int month, int day)
        {
            if (!RedDaysInMonth.ContainsKey(month))
            {
                RedDaysInMonth[month] = new List<int>();
            }
            RedDaysInMonth[month].Add(day);
        }

    }
}

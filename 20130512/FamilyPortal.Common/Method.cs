using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyPortal.Common
{
    public class Method
    {
        /// <summary>
        /// Get the date period's start date and end date.
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public static void GetDates(Field.TimePeriod timePeriod, ref DateTime startDate, ref DateTime endDate)
        {
            DateTime nowTime = DateTime.Now.Date;
            switch (timePeriod)
            {
                case Field.TimePeriod.Day://this day
                    startDate = DateTime.Now.Date;
                    endDate = DateTime.Now.Date;
                    break;
                case Field.TimePeriod.Week://this week，sunday is the day first of the week.
                    startDate = nowTime.AddDays(0 - Convert.ToInt32(nowTime.DayOfWeek));
                    endDate = startDate.AddDays(6);
                    break;
                case Field.TimePeriod.Month://this month
                    startDate = nowTime.AddDays(1 - nowTime.Day);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case Field.TimePeriod.Quarter://this quarter
                    startDate = nowTime.AddMonths(0 - (nowTime.Month - 1) % 3).AddDays(1 - nowTime.Day);
                    endDate = startDate.AddMonths(3).AddDays(-1);
                    break;
                case Field.TimePeriod.Year://this year
                    startDate = new DateTime(nowTime.Year, 1, 1);
                    endDate = new DateTime(nowTime.Year, 12, 31);
                    break;
                default://this day
                    startDate = DateTime.Now.Date;
                    endDate = DateTime.Now.Date;
                    break;
            }
        }
    }
}

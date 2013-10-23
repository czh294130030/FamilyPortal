using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace FamilyPortal.Silverlight.Common
{
    public class Method
    {
        /// <summary>
        /// According to uri string to show image.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="uriString"></param>
        public static void ShowImageByUriString(Image image, string uriString)
        {
            ImageSource imageSource = new BitmapImage(new Uri(uriString, UriKind.Relative));
            image.Source = imageSource;
        }
        /// <summary>
        /// Equal distribution datagrid.
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="fillColumnNumber"></param>
        public static void EqualDistributionDataGrid(DataGrid dataGrid, int index)
        {
            double fillWidth = dataGrid.ActualWidth;
            fillWidth -= dataGrid.Columns[index].ActualWidth;
            double borderThickness = dataGrid.BorderThickness.Left + dataGrid.BorderThickness.Right;
            fillWidth -= borderThickness;
            if (fillWidth > 0)
            {
                if (dataGrid.Columns.Count > 0)
                {
                    for (int i = 0; i < dataGrid.Columns.Count; i++)
                    {
                        if (i != index)
                        {
                            dataGrid.Columns[i].Width = new DataGridLength(fillWidth / (dataGrid.Columns.Count - 1));
                        }
                    }
                }
            }
        }
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
        /// <summary>
        /// Convert datetime to string.
        /// </summary>
        /// <param name="dateTime"></param>
        public static string ConvertDateTimeToString(DateTime dateTime)
        {
            string date = string.Empty;
            string month = string.Empty;
            switch (dateTime.Month)
            {
                case 1:
                    month = "Jan";
                    break;
                case 2:
                    month = "Feb";
                    break;
                case 3:
                    month = "Mar";
                    break;
                case 4:
                    month = "Apr";
                    break;
                case 5:
                    month = "May";
                    break;
                case 6:
                    month = "Jun";
                    break;
                case 7:
                    month = "Jul";
                    break;
                case 8:
                    month = "Aug";
                    break;
                case 9:
                    month = "Sep";
                    break;
                case 10:
                    month = "Oct";
                    break;
                case 11:
                    month = "Nov";
                    break;
                case 12:
                    month = "Dec";
                    break;
                default:
                    month = "Jan";
                    break;
            }
            date = month + "/" + dateTime.Day + "/" + dateTime.Year;
            return date;
        }
        /// <summary>
        /// Generate display date.
        /// </summary>
        /// <param name="timeType"></param>
        /// <param name="timePeriod"></param>
        /// <returns></returns>
        public static string GenerateDisplayDate(Field.TimeType timeType, string timePeriod)
        {
            string dispalyDate = string.Empty;
            switch (timeType)
            {
                case Field.TimeType.This:
                    if (timePeriod.Equals(Field.TimePeriod.Day.ToString()))
                    {
                        dispalyDate = "Today";
                    }
                    else
                    {
                        dispalyDate = timeType + " " + timePeriod;
                    }
                    break;
                case Field.TimeType.Last:
                    if (Int32.Parse(timePeriod) == 1)
                    {
                        dispalyDate = "Today";
                    }
                    else
                    {
                        dispalyDate = timeType + " " + timePeriod + " Days";
                    }
                    break;
                case Field.TimeType.Specific:
                    if (timePeriod.Split('-')[0].Trim() == timePeriod.Split('-')[1].Trim()
                            && timePeriod.Split('-')[1].Trim() == ConvertDateTimeToString(DateTime.Now))
                    {
                        dispalyDate = "Today";
                    }
                    else
                    {
                        dispalyDate = timePeriod;
                    }
                    break;
                default:
                    dispalyDate = timeType + " " + timePeriod;
                    break;
            }
            return dispalyDate;
        }
    }
}

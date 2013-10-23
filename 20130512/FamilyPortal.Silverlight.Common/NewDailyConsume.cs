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

namespace FamilyPortal.Silverlight.Common
{
    public class NewDailyConsume
    {
        private int dailyID;

        public int DailyID
        {
            get { return dailyID; }
            set { dailyID = value; }
        }
        private decimal? amount;

        public decimal? Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private string date;

        public string Date
        {
            get { return date; }
            set { date = value; }
        } 
    }
}

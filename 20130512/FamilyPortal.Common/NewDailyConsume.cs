using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyPortal.Common
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

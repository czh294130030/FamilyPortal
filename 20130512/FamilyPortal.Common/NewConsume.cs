using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyPortal.Common
{
    [Serializable]
    public class NewConsume
    {
        public NewConsume() { }
        private int consumeID;

        public int ConsumeID
        {
            get { return consumeID; }
            set { consumeID = value; }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private decimal? amount;

        public decimal? Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private int? typeID;

        public int? TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }
        private string typeDesc;

        public string TypeDesc
        {
            get { return typeDesc; }
            set { typeDesc = value; }
        }
        private int? dailyID;

        public int? DailyID
        {
            get { return dailyID; }
            set { dailyID = value; }
        }
    }
}

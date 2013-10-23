using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyPortal.Common
{
    public class NewBankCard
    {
        private int cardID;

        public int CardID
        {
            get { return cardID; }
            set { cardID = value; }
        }
        private string cardNO;

        public string CardNO
        {
            get { return cardNO; }
            set { cardNO = value; }
        }
        private decimal? amount;

        public decimal? Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private DateTime? dateFrom;

        public DateTime? DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; }
        }
        private DateTime? dateTo;

        public DateTime? DateTo
        {
            get { return dateTo; }
            set { dateTo = value; }
        }

        private int? userID;

        public int? UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private int? cardTypeID;

        public int? CardTypeID
        {
            get { return cardTypeID; }
            set { cardTypeID = value; }
        }
        private string cardType;

        public string CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }

        private int? bankID;

        public int? BankID
        {
            get { return bankID; }
            set { bankID = value; }
        }
        private string bank;

        public string Bank
        {
            get { return bank; }
            set { bank = value; }
        }

        private int? cityID;

        public int? CityID
        {
            get { return cityID; }
            set { cityID = value; }
        }
        private string city;

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        private bool? isUsing;

        public bool? IsUsing
        {
            get { return isUsing; }
            set { isUsing = value; }
        }
    }
}

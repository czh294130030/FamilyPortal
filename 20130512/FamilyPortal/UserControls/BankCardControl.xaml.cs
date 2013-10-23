using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FamilyPortal.Silverlight.Common;
using FamilyPortal.BankCard;

namespace FamilyPortal.UserControls
{
    public partial class BankCardControl : UserControl
    {
        private FamilyServiceReference.BankCard model = null;
        public delegate void OperactionDelegate(object sender, EventArgs e);
        public event OperactionDelegate operationEvent;
        public BankCardControl(FamilyServiceReference.NewBankCard item)
        {
            InitializeComponent();
            MainBorder.BorderBrush = new SolidColorBrush(Field.fontColor);
            BankTextBlock.Text = item.Bank;
            CardNOTextBlock.Text = item.CardNO;
            CityTextBlock.Text = item.City;
            OwnerTextBlock.Text = item.UserName;
            string uriString = "../Resources/" + item.Bank + ".png";
            Method.ShowImageByUriString(IconImage, uriString);
            if (item.CardType.Equals("bank card"))
            {
                DateStackPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                DateFromTextBlock.Text = Method.ConvertDateTimeToString(DateTime.Parse(item.DateFrom.ToString()));
                DateToTextBlock.Text = Method.ConvertDateTimeToString(DateTime.Parse(item.DateTo.ToString()));
            }
            model = new FamilyServiceReference.BankCard();
            model.cardID = item.CardID;
            model.cardNO = item.CardNO;
            model.amount = item.Amount;
            model.dateFrom = item.DateFrom;
            model.dateTo = item.DateTo;
            model.userID = item.UserID;
            model.cardTypeID = item.CardTypeID;
            model.bankID = item.BankID;
            model.cityID = item.CityID;
            model.isUsing = item.IsUsing;
        }
        /// <summary>
        /// Edit bank card.
        /// </summary>
        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddBankCard addBankCard = new AddBankCard(model);
            addBankCard.Title = "Edit Bank Card";
            addBankCard.Closed += new EventHandler(addBankCard_Closed);
            addBankCard.Show();
        }

        void addBankCard_Closed(object sender, EventArgs e)
        {
            AddBankCard addBankCard = sender as AddBankCard;
            if (addBankCard.DialogResult == true)//update bank card successfully.
            {
                if (operationEvent != null)
                {
                    operationEvent(null, null);
                }
            }
        }
        /// <summary>
        /// Delete bank card.
        /// </summary>
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PromptWindow DeletePromptWindow = new PromptWindow(Field.PromptInformation.Question, Field.ConfirmToDelete);
            DeletePromptWindow.Closed += new EventHandler(DeletePromptWindow_Closed);
            DeletePromptWindow.Show();
        }

        void DeletePromptWindow_Closed(object sender, EventArgs e)
        {
            PromptWindow DeletePromptWindow = sender as PromptWindow;
            if (DeletePromptWindow.DialogResult == true)
            {
                model.isUsing = false;
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.UpdateBankCardCompleted += new EventHandler<FamilyServiceReference.UpdateBankCardCompletedEventArgs>(client_UpdateBankCardCompleted);
                client.UpdateBankCardAsync(model);
            }
        }

        void client_UpdateBankCardCompleted(object sender, FamilyServiceReference.UpdateBankCardCompletedEventArgs e)
        {
            if (e.Result != 0)
            {
                PromptWindow DeleteSuccessPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.DeleteSuccess);
                DeleteSuccessPromptWindow.Closed += new EventHandler(DeleteSuccessPromptWindow_Closed);
                DeleteSuccessPromptWindow.Show();
            }
            else
            {
                PromptWindow DeleteFailedPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.DeleteFailed);
                DeleteFailedPromptWindow.Show();
            }
        }

        void DeleteSuccessPromptWindow_Closed(object sender, EventArgs e)
        {
            if (operationEvent != null)
            {
                operationEvent(null, null);
            }
        }
    }
}
    
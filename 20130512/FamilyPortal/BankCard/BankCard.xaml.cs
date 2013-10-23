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
using System.Windows.Navigation;

namespace FamilyPortal.BankCard
{
    public partial class BankCard : Page
    {
        private FamilyServiceReference.NewBankCard newBankCard = null;
        public BankCard()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(BankCard_Loaded);
            AddHyperlinkButton.Click += new RoutedEventHandler(AddHyperlinkButton_Click);
        }

        void BankCard_Loaded(object sender, RoutedEventArgs e)
        {
            BindBankCard(true);
        }
        /// <summary>
        /// Binding bank card.
        /// </summary>
        private void BindBankCard(bool isUsing)
        {
            newBankCard = new FamilyServiceReference.NewBankCard();
            newBankCard.IsUsing = isUsing;
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetBackCardCompleted += new EventHandler<FamilyServiceReference.GetBackCardCompletedEventArgs>(client_GetBackCardCompleted);
            client.GetBackCardAsync(newBankCard);
        }

        void client_GetBackCardCompleted(object sender, FamilyServiceReference.GetBackCardCompletedEventArgs e)
        {
            DispalyBankCard(true);
            List<FamilyServiceReference.NewBankCard> items = e.Result;
            MainWrapPanel.Children.Clear();
            if (items != null && items.Count > 0)
            {
                foreach (FamilyServiceReference.NewBankCard item in items)
                {
                    FamilyPortal.UserControls.BankCardControl bankCardControl = new UserControls.BankCardControl(item);
                    bankCardControl.operationEvent += new UserControls.BankCardControl.OperactionDelegate(bankCardControl_operationEvent);
                    bankCardControl.Margin = new Thickness(0, 0, 5, 5);
                    MainWrapPanel.Children.Add(bankCardControl);
                }
            }
        }

        void bankCardControl_operationEvent(object sender, EventArgs e)
        {
            BindBankCard(true);
        }
        /// <summary>
        /// Add or edit bank card.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddHyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            AddBankCard addBankCard = new AddBankCard();
            addBankCard.Title = "Add Bank Card";
            addBankCard.Closed += new EventHandler(addBankCard_Closed);
            addBankCard.Show();
        }
        void addBankCard_Closed(object sender, EventArgs e)
        {
            AddBankCard addBankCard = sender as AddBankCard;
            if (addBankCard.DialogResult == true)//Add or update bank card successfully.
            {
                BindBankCard(true);
            }
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DispalyBankCard(false);
        }
        /// <summary>
        /// Display bank card information.
        /// </summary>
        /// <param name="isGetData"></param>
        private void DispalyBankCard(bool isGetData)
        {
            if (isGetData)//After getting data.
            {
                BankCardProgressBar.Visibility = Visibility.Collapsed;
                MainWrapPanel.Visibility = Visibility.Visible;
            }
            else
            {
                BankCardProgressBar.Visibility = Visibility.Visible;
                MainWrapPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}

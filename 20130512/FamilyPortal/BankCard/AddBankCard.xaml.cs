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
using System.Text.RegularExpressions;

namespace FamilyPortal.BankCard
{
    public partial class AddBankCard : ChildWindow
    {
        private int loadCounter = 0;
        private FamilyServiceReference.BankCard model = null;
        /// <summary>
        /// Add bank card.
        /// </summary>
        public AddBankCard()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(AddBankCard_Loaded);
            OKButton.Click += new RoutedEventHandler(OKButton_Click);
            CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            AmountTextBox.LostFocus += new RoutedEventHandler(AmountTextBox_LostFocus);
            CardTypeComboBox.SelectionChanged += new SelectionChangedEventHandler(CardTypeComboBox_SelectionChanged);
        }
        /// <summary>
        /// Update bank card.
        /// </summary>
        public AddBankCard(FamilyServiceReference.BankCard item)
        {
            InitializeComponent();
            model = new FamilyServiceReference.BankCard();
            model = item;
            this.Loaded += new RoutedEventHandler(AddBankCard_Loaded);
            OKButton.Click += new RoutedEventHandler(OKButton_Click);
            CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            AmountTextBox.LostFocus += new RoutedEventHandler(AmountTextBox_LostFocus);
            CardTypeComboBox.SelectionChanged += new SelectionChangedEventHandler(CardTypeComboBox_SelectionChanged);
        }
        void AmountTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AmountTextBox.Text))
            {
                Match m = Regex.Match(AmountTextBox.Text, Field.pattern);
                if (!m.Success)
                {
                    AmountTextBox.Text = string.Empty;
                    PromptWindow promptWindow = new PromptWindow(Field.PromptInformation.Information, Field.InputAmountInvalid);
                    promptWindow.Show();
                }
            }
        }

        void AddBankCard_Loaded(object sender, RoutedEventArgs e)
        {
            if (model != null)//update bank card/bank book.
            {
                if (model.cardTypeID == Field.BankCardID)//bank card
                {
                    CardNOTextBox.Text = model.cardNO;
                }
                else//bank book
                {
                    CardNOTextBox.Text = model.cardNO;
                    AmountTextBox.Text = model.amount.ToString();
                    DateFromPicker.Text = model.dateFrom.ToString();
                    DateToPicker.Text = model.dateTo.ToString();
                }
            }
            AmountTextBlock.Text = Field.AmountDemo;
            DisplayBankCardControls(false);
            //Get all card type.
            FamilyServiceReference.FamilyServiceClient cardTypeClient = new FamilyServiceReference.FamilyServiceClient();
            cardTypeClient.GetParaDetailByInfoIDCompleted += new EventHandler<FamilyServiceReference.GetParaDetailByInfoIDCompletedEventArgs>(client_GetCardTypeCompleted);
            cardTypeClient.GetParaDetailByInfoIDAsync(Field.CardTypeID);
            //Get all user information.
            FamilyServiceReference.FamilyServiceClient userInfoClient = new FamilyServiceReference.FamilyServiceClient();
            userInfoClient.GetAllUserInfoCompleted += new EventHandler<FamilyServiceReference.GetAllUserInfoCompletedEventArgs>(client_GetAllUserInfoCompleted);
            userInfoClient.GetAllUserInfoAsync();
            //Get all bank
            FamilyServiceReference.FamilyServiceClient bankClient = new FamilyServiceReference.FamilyServiceClient();
            bankClient.GetParaDetailByInfoIDCompleted += new EventHandler<FamilyServiceReference.GetParaDetailByInfoIDCompletedEventArgs>(client_GetBankCompleted);
            bankClient.GetParaDetailByInfoIDAsync(Field.BankID);
            //Get city
            FamilyServiceReference.FamilyServiceClient cityClient = new FamilyServiceReference.FamilyServiceClient();
            cityClient.GetParaDetailByInfoIDCompleted += new EventHandler<FamilyServiceReference.GetParaDetailByInfoIDCompletedEventArgs>(cityClient_GetParaDetailByInfoIDCompleted);
            cityClient.GetParaDetailByInfoIDAsync(Field.CityID);
        }
        /// <summary>
        /// Get city
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cityClient_GetParaDetailByInfoIDCompleted(object sender, FamilyServiceReference.GetParaDetailByInfoIDCompletedEventArgs e)
        {
            loadCounter++;
            if (loadCounter == 4) { DisplayBankCardControls(true); }
            List<FamilyServiceReference.ParaDetail> items = e.Result;
            if (items != null && items.Count > 0)
            {
                for (int i = 0; i < items.Count(); i++)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = items[i].description;
                    comboBoxItem.Tag = items[i].detailID;
                    if (model != null)
                    {
                        if (items[i].detailID == model.cityID) { comboBoxItem.IsSelected = true; }
                    }
                    else
                    {
                        if (i == 0) { comboBoxItem.IsSelected = true; }
                    }
                    CityComboBox.Items.Add(comboBoxItem);
                }
            }
        }
        /// <summary>
        /// Get all bank
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetBankCompleted(object sender, FamilyServiceReference.GetParaDetailByInfoIDCompletedEventArgs e)
        {
            loadCounter++;
            if (loadCounter == 4) { DisplayBankCardControls(true); }
            List<FamilyServiceReference.ParaDetail> items = e.Result;
            if (items != null && items.Count > 0)
            {
                for (int i = 0; i < items.Count(); i++)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    Image image = new Image();
                    image.Margin = new Thickness(0, 0, 5, 0);
                    image.Width = 16;
                    image.Height = 16;
                    string uriString = "../Resources/" + items[i].description + ".png";
                    Method.ShowImageByUriString(image, uriString);
                    stackPanel.Children.Add(image);
                    TextBlock textBlock = new TextBlock();
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.Text = items[i].description;
                    stackPanel.Children.Add(textBlock);
                    comboBoxItem.Content = stackPanel;
                    comboBoxItem.Tag = items[i].detailID;
                    if (model != null)
                    {
                        if (items[i].detailID == model.bankID) { comboBoxItem.IsSelected = true; }
                    }
                    else
                    {
                        if (i == 0) { comboBoxItem.IsSelected = true; }
                    }
                    BankComboBox.Items.Add(comboBoxItem);
                }
            }
        }
        /// <summary>
        /// Get all card type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetCardTypeCompleted(object sender, FamilyServiceReference.GetParaDetailByInfoIDCompletedEventArgs e)
        {
            loadCounter++;
            if (loadCounter == 4) { DisplayBankCardControls(true); }
            List<FamilyServiceReference.ParaDetail> items = e.Result;
            if (items != null && items.Count > 0)
            {
                for (int i = 0; i < items.Count(); i++)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = items[i].description;
                    comboBoxItem.Tag = items[i].detailID;
                    if (model != null)
                    {
                        if (items[i].detailID == model.cardTypeID) { comboBoxItem.IsSelected = true; }
                    }
                    else
                    {
                        if (i == 0) { comboBoxItem.IsSelected = true; }
                    }
                    CardTypeComboBox.Items.Add(comboBoxItem);
                }
            }
        }
        /// <summary>
        /// //Get all user information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetAllUserInfoCompleted(object sender, FamilyServiceReference.GetAllUserInfoCompletedEventArgs e)
        {
            loadCounter++;
            if (loadCounter == 4) { DisplayBankCardControls(true); }
            List<FamilyServiceReference.UserInfo> items = e.Result;
            if (items != null && items.Count > 0)
            {
                for (int i = 0; i < items.Count(); i++)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = items[i].userName;
                    comboBoxItem.Tag = items[i].userID;
                    if (model != null)
                    {
                        if (items[i].userID == model.userID) { comboBoxItem.IsSelected = true; }
                    }
                    else
                    {
                        if (i == 0) { comboBoxItem.IsSelected = true; }
                    }
                    OwnerComboBox.Items.Add(comboBoxItem);
                }
            }
        }
        void CardTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = CardTypeComboBox.SelectedItem as ComboBoxItem;
            if (selectedComboBoxItem != null)
            {
                DisplayControlsByTypeID(Int32.Parse(selectedComboBoxItem.Tag.ToString()));
            }
        }
        /// <summary>
        /// Display controls accroding to card type.
        /// </summary>
        /// <param name="typeID"></param>
        private void DisplayControlsByTypeID(int typeID)
        {
            switch (typeID)
            {
                case Field.BankCardID:
                    AmountStackPanel.Visibility = Visibility.Collapsed;
                    DateFromStackPanel.Visibility = Visibility.Collapsed;
                    DateToStackPanel.Visibility = Visibility.Collapsed;
                    this.Height = 280;
                    break;
                case Field.BankBookID:
                    AmountStackPanel.Visibility = Visibility.Visible;
                    DateFromStackPanel.Visibility = Visibility.Visible;
                    DateToStackPanel.Visibility = Visibility.Visible;
                    this.Height = 400;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Display bank card controls accroding to if gets data or not.
        /// </summary>
        /// <param name="isGetData"></param>
        private void DisplayBankCardControls(bool isGetData)
        {
            if (isGetData)
            {
                ProgressBar1.Visibility = Visibility.Collapsed;
                BankCardStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ProgressBar1.Visibility = Visibility.Visible;
                BankCardStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            OKButton.IsEnabled = false;
            if (VerifyUserInput())
            {
                FamilyServiceReference.BankCard item = new FamilyServiceReference.BankCard();
                item.cardNO = CardNOTextBox.Text;
                ComboBoxItem BankSelectedComboBoxItem = BankComboBox.SelectedItem as ComboBoxItem;
                item.bankID = Int32.Parse(BankSelectedComboBoxItem.Tag.ToString());
                ComboBoxItem CardTypeSelectedComboBoxItem = CardTypeComboBox.SelectedItem as ComboBoxItem;
                item.cardTypeID = Int32.Parse(CardTypeSelectedComboBoxItem.Tag.ToString());
                if (!string.IsNullOrEmpty(AmountTextBox.Text))
                {
                    item.amount = decimal.Parse(AmountTextBox.Text);
                }
                if (!string.IsNullOrEmpty(DateFromPicker.Text))
                {
                    item.dateFrom = DateTime.Parse(DateFromPicker.Text);
                }
                if (!string.IsNullOrEmpty(DateToPicker.Text))
                {
                    item.dateTo = DateTime.Parse(DateToPicker.Text);
                }
                ComboBoxItem OwnerSelectedComboBoxItem = OwnerComboBox.SelectedItem as ComboBoxItem;
                item.userID = Int32.Parse(OwnerSelectedComboBoxItem.Tag.ToString());
                ComboBoxItem CitySelectedComboBoxItem = CityComboBox.SelectedItem as ComboBoxItem;
                item.cityID = Int32.Parse(CitySelectedComboBoxItem.Tag.ToString());
                item.isUsing = true;
                if (model != null)//Update bank card or bank book.
                {
                    item.cardID = model.cardID;
                    FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                    client.UpdateBankCardCompleted += new EventHandler<FamilyServiceReference.UpdateBankCardCompletedEventArgs>(client_UpdateBankCardCompleted);
                    client.UpdateBankCardAsync(item);
                }
                else//Add bank card or bank book.
                {
                    FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                    client.AddBankCardCompleted += new EventHandler<FamilyServiceReference.AddBankCardCompletedEventArgs>(client_AddBankCardCompleted);
                    client.AddBankCardAsync(item); 
                }
            }
        }

        void client_UpdateBankCardCompleted(object sender, FamilyServiceReference.UpdateBankCardCompletedEventArgs e)
        {
            if (e.Result != 0)
            {
                PromptWindow UpdateSuccessPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.UpdateSuccess);
                UpdateSuccessPromptWindow.Closed += new EventHandler(UpdateSuccessPromptWindow_Closed);
                UpdateSuccessPromptWindow.Show();
            }
            else
            {
                PromptWindow UpdateFailedPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.UpdateFailed);
                UpdateFailedPromptWindow.Closed += new EventHandler(UpdateFailedPromptWindow_Closed);
                UpdateFailedPromptWindow.Show();
            }
        }

        void UpdateFailedPromptWindow_Closed(object sender, EventArgs e)
        {
            OKButton.IsEnabled = true;
        }

        void UpdateSuccessPromptWindow_Closed(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }

        void client_AddBankCardCompleted(object sender, FamilyServiceReference.AddBankCardCompletedEventArgs e)
        {
            if (e.Result != 0)
            {
                PromptWindow AddSuccessPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.AddSuccess);
                AddSuccessPromptWindow.Closed += new EventHandler(AddSuccessPromptWindow_Closed);
                AddSuccessPromptWindow.Show();
            }
            else
            {
                PromptWindow AddFailedPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.AddFailed);
                AddFailedPromptWindow.Closed += new EventHandler(AddFailedPromptWindow_Closed);
                AddFailedPromptWindow.Show();
            }
        }

        void AddFailedPromptWindow_Closed(object sender, EventArgs e)
        {
            OKButton.IsEnabled = true;
        }

        void AddSuccessPromptWindow_Closed(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            PromptWindow promptWindowCancel = new PromptWindow(Field.PromptInformation.Question, Field.Unsave);
            promptWindowCancel.Closed += new EventHandler(promptWindowCancel_Closed);
            promptWindowCancel.Show();
        }

        void promptWindowCancel_Closed(object sender, EventArgs e)
        {
            PromptWindow promptWindowCancel = sender as PromptWindow;
            if (promptWindowCancel.DialogResult == true)
            {
                this.DialogResult = false;
            }
        }
        /// <summary>
        /// Verify user's input.
        /// </summary>
        /// <returns></returns>
        private bool VerifyUserInput()
        {
            //Card NO is requried.
            if (string.IsNullOrEmpty(CardNOTextBox.Text))
            {
                PromptWindow CardNOPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.InputCardNO);
                CardNOPromptWindow.Closed += new EventHandler(CardNOPromptWindow_Closed);
                CardNOPromptWindow.Show();
                return false;
            }
            ComboBoxItem selectedComboBoxItem = CardTypeComboBox.SelectedItem as ComboBoxItem;
            if (Int32.Parse(selectedComboBoxItem.Tag.ToString()).Equals(Field.BankBookID))
            {
                if (string.IsNullOrEmpty(AmountTextBox.Text))
                {
                    PromptWindow AmountPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.InputAmount);
                    AmountPromptWindow.Closed += new EventHandler(AmountPromptWindow_Closed);
                    AmountPromptWindow.Show();
                    return false;
                }
                if (string.IsNullOrEmpty(DateFromPicker.Text))
                {
                    PromptWindow DateFromPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.InputStartDate);
                    DateFromPromptWindow.Closed += new EventHandler(DateFromPromptWindow_Closed);
                    DateFromPromptWindow.Show();
                    return false;
                }
                if (string.IsNullOrEmpty(DateToPicker.Text))
                {
                    PromptWindow DateToPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.InputEndDate);
                    DateToPromptWindow.Closed += new EventHandler(DateToPromptWindow_Closed);
                    DateToPromptWindow.Show();
                    return false;
                }
                if (DateTime.Parse(DateFromPicker.Text) > DateTime.Parse(DateToPicker.Text))
                {
                    PromptWindow GreaterPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.EndDateGreaterThanOrEqualToStartDate);
                    GreaterPromptWindow.Closed += new EventHandler(GreaterPromptWindow_Closed);
                    GreaterPromptWindow.Show();
                    return false;
                }
            }
            return true;
        }

        void GreaterPromptWindow_Closed(object sender, EventArgs e)
        {
            DateToPicker.Focus();
            OKButton.IsEnabled = true;
        }

        void DateToPromptWindow_Closed(object sender, EventArgs e)
        {
            DateToPicker.Focus();
            OKButton.IsEnabled = true;
        }

        void DateFromPromptWindow_Closed(object sender, EventArgs e)
        {
            DateFromPicker.Focus();
            OKButton.IsEnabled = true;
        }

        void AmountPromptWindow_Closed(object sender, EventArgs e)
        {
            AmountTextBox.Focus();
            OKButton.IsEnabled = true;
        }

        void CardNOPromptWindow_Closed(object sender, EventArgs e)
        {
            CardNOTextBox.Focus();
            OKButton.IsEnabled = true;
        }
    }
}


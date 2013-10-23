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

namespace FamilyPortal.UserControls
{
    public partial class ConsumeControl : UserControl
    {
        /// <summary>
        /// Define delegate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void AmountDelegate(object sender, EventArgs e);
        public delegate void TypeDelegate(object sender, EventArgs e);
        /// <summary>
        /// Define event.
        /// </summary>
        public event AmountDelegate AmountEvent = null;
        public event TypeDelegate TypeEvent = null;
        public ConsumeControl(List<FamilyPortal.FamilyServiceReference.ConsumeType> consumeTypeList, FamilyServiceReference.Consume consume)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ConsumeControl_Loaded);
            DescriptionTextBox.TextChanged += new TextChangedEventHandler(DescriptionTextBox_TextChanged);
            AmountTextBox.LostFocus += new RoutedEventHandler(AmountTextBox_LostFocus);
            TypeComboBox.DropDownOpened += new EventHandler(TypeComboBox_DropDownOpened);
            BindConsumeTypeList(consumeTypeList, consume);
        }
        void ConsumeControl_Loaded(object sender, RoutedEventArgs e)
        {
            AmountTextBlock.Text = Field.AmountDemo;
        }
        /// <summary>
        /// Binding consume types.
        /// </summary>
        private void BindConsumeTypeList(List<FamilyPortal.FamilyServiceReference.ConsumeType> consumeTypeList, FamilyServiceReference.Consume consume)
        {
            if (consumeTypeList.Count > 0)
            {
                for (int i = 0; i < consumeTypeList.Count; i++)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = consumeTypeList[i].description;
                    comboBoxItem.Tag = consumeTypeList[i].typeID;
                    if (consume == null)//Add consume.
                    {
                        if (i == 0) { comboBoxItem.IsSelected = true; }
                    }
                    else//Edit consume.
                    {
                        if (Int32.Parse(comboBoxItem.Tag.ToString()) == consume.typeID) { comboBoxItem.IsSelected = true; }
                    }
                    TypeComboBox.Items.Add(comboBoxItem);
                }
            }
            if (consume != null)//Edit consume.
            {
                AmountTextBox.Text = consume.amount.ToString();
                DescriptionTextBox.Text = consume.description.ToString();
                DescriptionTextBox_TextChanged(null, null);
            }
        }
        /// <summary>
        /// Get the number of characters, which user still can input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NumberTextBlock.Text = (DescriptionTextBox.MaxLength - DescriptionTextBox.Text.Length).ToString();
        }
        /// <summary>
        /// Verify the amount, which user input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                else
                {
                    //Binding AmountEvent to AmountTextBox_LostFocus
                    if (AmountEvent != null)
                    {
                        AmountEvent(sender, e);
                    }
                }
            }
            else
            {
                //Binding AmountEvent to AmountTextBox_LostFocus
                if (AmountEvent != null)
                {
                    AmountEvent(sender, e);
                }
            }
        }
        /// <summary>
        /// Binding TypeEvent to TypeComboBox_DropDownOpened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TypeComboBox_DropDownOpened(object sender, EventArgs e)
        {
            if (TypeEvent != null)
            {
                TypeEvent(sender, e);
            }
        }
        /// <summary>
        /// Get consume.
        /// </summary>
        /// <returns></returns>
        public FamilyPortal.FamilyServiceReference.Consume GetConsume(int dailyID)
        {
            FamilyPortal.FamilyServiceReference.Consume item = new FamilyPortal.FamilyServiceReference.Consume();
            item.description = DescriptionTextBox.Text.Trim();
            item.amount = decimal.Parse(AmountTextBox.Text.Trim());
            ComboBoxItem comboBoxItem=TypeComboBox.SelectedItem as ComboBoxItem;
            item.typeID = Int32.Parse(comboBoxItem.Tag.ToString());
            item.dailyID = dailyID;
            return item;
        }
    }
}

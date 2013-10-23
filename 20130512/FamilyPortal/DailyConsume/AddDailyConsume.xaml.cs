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
using FamilyPortal.UserControls;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Threading;

namespace FamilyPortal.DailyConsume
{
    public partial class AddDailyConsume : ChildWindow
    {
        private bool isAutoAddConsume = true;
        private int _DailyID = 0;
        private DispatcherTimer timer = null;
        private bool canAdd = true;
        private ConsumeControl verifyConsumeControl = null;
        /// <summary>
        /// Define a list which used to store all consume types.
        /// </summary>
        private List<FamilyPortal.FamilyServiceReference.ConsumeType> allConsumeTypeList = null;
        /// <summary>
        /// Add daily consume.
        /// </summary>
        public AddDailyConsume()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += new EventHandler(timer_Tick);
            //set language in english.
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            PlusImage.MouseLeftButtonDown += new MouseButtonEventHandler(PlusImage_MouseLeftButtonDown);
            PlusImage.MouseEnter += new MouseEventHandler(PlusImage_MouseEnter);
            PlusImage.MouseLeave += new MouseEventHandler(PlusImage_MouseLeave);
            ContentStackPanel.SizeChanged += new SizeChangedEventHandler(ContentStackPanel_SizeChanged);
            this.MouseWheel += new MouseWheelEventHandler(AddDailyConsume_MouseWheel);
            OKButton.Click += new RoutedEventHandler(OKButton_Click);
            CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            this.Loaded += new RoutedEventHandler(AddDailyConsume_Loaded);
            datePicker1.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(datePicker1_SelectedDateChanged);
        }
        /// <summary>
        /// Edit daily consume.
        /// </summary>
        /// <param name="dailyID"></param>
        public AddDailyConsume(int _dailyID)
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += new EventHandler(timer_Tick);
            this._DailyID = _dailyID;
            //set language in english.
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            PlusImage.MouseLeftButtonDown += new MouseButtonEventHandler(PlusImage_MouseLeftButtonDown);
            PlusImage.MouseEnter += new MouseEventHandler(PlusImage_MouseEnter);
            PlusImage.MouseLeave += new MouseEventHandler(PlusImage_MouseLeave);
            ContentStackPanel.SizeChanged += new SizeChangedEventHandler(ContentStackPanel_SizeChanged);
            this.MouseWheel += new MouseWheelEventHandler(AddDailyConsume_MouseWheel);
            OKButton.Click += new RoutedEventHandler(OKButton_Click);
            CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            this.Loaded += new RoutedEventHandler(AddDailyConsume_Loaded);
            datePicker1.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(datePicker1_SelectedDateChanged);
        }
        /// <summary>
        /// The selected date in datepicker was changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void datePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(datePicker1.Text) && _DailyID == 0)
            {
                DateTime date = DateTime.Parse(datePicker1.Text);
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.GetTheDailyConsumeByDateCompleted += new EventHandler<FamilyServiceReference.GetTheDailyConsumeByDateCompletedEventArgs>(client_GetTheDailyConsumeByDateCompleted);
                client.GetTheDailyConsumeByDateAsync(date);
            }
        }

        void client_GetTheDailyConsumeByDateCompleted(object sender, FamilyServiceReference.GetTheDailyConsumeByDateCompletedEventArgs e)
        {
            if (e.Result != null)//the date exists.
            {
                PromptWindow promptWindowDateExist = new PromptWindow(Field.PromptInformation.Information, Field.DateExists);
                promptWindowDateExist.Closed += new EventHandler(promptWindowDateExist_Closed);
                promptWindowDateExist.Show();
            }
        }
        void promptWindowDateExist_Closed(object sender, EventArgs e)
        {
            //When the date exists, clear the datepicker.
            datePicker1.Text = string.Empty;
        }
        /// <summary>
        /// scrollbar scrolls when mouse wheel scrolls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddDailyConsume_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer viewer = ScrollViewer1;
            if (viewer == null)
                return;
            double num = Math.Abs((int)(e.Delta / 2));
            double offset = 0.0;
            if (e.Delta > 0)
            {
                offset = Math.Max((double)0.0, (double)(viewer.VerticalOffset - num));
            }
            else
            {
                offset = Math.Min(viewer.ScrollableHeight, viewer.VerticalOffset + num);
            }
            if (offset != viewer.VerticalOffset)
            {
                viewer.ScrollToVerticalOffset(offset);
                e.Handled = true;
            }
        }
        void ContentStackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!isAutoAddConsume)
            {
                //When StackPanel's actual height is greater than ScrollViewer's，ScrollViewer scroll to the bottom.
                if (ContentStackPanel.ActualHeight > ScrollViewer1.ActualHeight)
                {
                    ScrollViewer1.ScrollToVerticalOffset(ContentStackPanel.ActualHeight);
                }
            }
        }

        void AddDailyConsume_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayDailyConsumeControls(false);
            //Show image.
            Method.ShowImageByUriString(PlusImage, Field.PlusImagePath);
            Method.ShowImageByUriString(PromptImage, Field.PromptImagePath);
            //Show details.
            AddOneMoreConsumeTextBlock.Text = Field.AddOneMoreConsume;
            //Get all consume types.
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetAllConsumeTypeCompleted += new EventHandler<FamilyServiceReference.GetAllConsumeTypeCompletedEventArgs>(client_GetAllConsumeTypeCompleted);
            client.GetAllConsumeTypeAsync();
        }
        void client_GetAllConsumeTypeCompleted(object sender, FamilyServiceReference.GetAllConsumeTypeCompletedEventArgs e)
        {
            allConsumeTypeList = e.Result;
            if (allConsumeTypeList == null)
            {
                return;
            }
            if (_DailyID == 0)//Add daily consume.
            {
                DisplayDailyConsumeControls(true);
                List<FamilyPortal.FamilyServiceReference.ConsumeType> unusedConsumeTypeList = GetUnusedConsumeTypeList();
                if (unusedConsumeTypeList != null)
                {
                    //Add one consume control after completing to get all consume types.
                    AddOneConsumeControl(string.Empty, unusedConsumeTypeList);
                }
                else//All consume types are used.
                {
                    PromptWindow promptWindow = new PromptWindow(Field.PromptInformation.Information, Field.AllConsumeTypesUsed);
                    promptWindow.Show();
                }
            }
            else//Edit daily consume
            {
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.GetDailyConsumeByIDCompleted += new EventHandler<FamilyServiceReference.GetDailyConsumeByIDCompletedEventArgs>(client_GetDailyConsumeByIDCompleted);
                client.GetDailyConsumeByIDAsync(_DailyID);
            }
        }
        /// <summary>
        /// Get daily consume by id.
        /// </summary>
        void client_GetDailyConsumeByIDCompleted(object sender, FamilyServiceReference.GetDailyConsumeByIDCompletedEventArgs e)
        {
            FamilyServiceReference.DailyConsume item=e.Result;
            datePicker1.Text = item.date.ToString();
            datePicker1.IsEnabled = false;
            DailyAmountTextBlock.Text = item.amount.ToString();
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetConsumeByDailyIDCompleted += new EventHandler<FamilyServiceReference.GetConsumeByDailyIDCompletedEventArgs>(client_GetConsumeByDailyIDCompleted);
            client.GetConsumeByDailyIDAsync(_DailyID);
        }
        /// <summary>
        /// Get consumes by dailyID.
        /// </summary>
        void client_GetConsumeByDailyIDCompleted(object sender, FamilyServiceReference.GetConsumeByDailyIDCompletedEventArgs e)
        {
            DisplayDailyConsumeControls(true);
            List<FamilyServiceReference.Consume> items = e.Result.OrderBy(u => u.typeID).ToList();
            if (items != null && items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (i == 0)//The first 'ConsumeControl' control.
                    {
                        List<FamilyPortal.FamilyServiceReference.ConsumeType> unusedConsumeTypeList = GetUnusedConsumeTypeList();
                        if (unusedConsumeTypeList != null)
                        {
                            //Add one 'ConsumeControl' control named after nowString and fill content with consume.
                            AddOneConsumeControl(string.Empty, unusedConsumeTypeList, items[i]);
                        }
                        else//All consume types are used.
                        {
                            PromptWindow promptWindow = new PromptWindow(Field.PromptInformation.Information, Field.AllConsumeTypesUsed);
                            promptWindow.Show();
                        }
                    }
                    else
                    {
                        List<FamilyPortal.FamilyServiceReference.ConsumeType> unusedConsumeTypeList = GetUnusedConsumeTypeList();
                        if (unusedConsumeTypeList != null)
                        {
                            string nowString = i.ToString();
                            AddOneStackPanel(nowString);
                            AddOneConsumeControl(nowString, unusedConsumeTypeList, items[i]);
                        }
                        else//All consume types are used.
                        {
                            PromptWindow promptWindow = new PromptWindow(Field.PromptInformation.Information, Field.AllConsumeTypesUsed);
                            promptWindow.Show();
                        }
                    }
                }
            }
        }
        void PlusImage_MouseLeave(object sender, MouseEventArgs e)
        {
            if (canAdd) { Method.ShowImageByUriString(PlusImage, Field.PlusImagePath); }
        }
        void PlusImage_MouseEnter(object sender, MouseEventArgs e)
        {
            if (canAdd) { Method.ShowImageByUriString(PlusImage, Field.PlusImagePath1); }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            canAdd = true;
            Method.ShowImageByUriString(PlusImage, Field.PlusImagePath);
        }
        void PlusImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isAutoAddConsume = false;
            if (canAdd)
            {
                List<FamilyPortal.FamilyServiceReference.ConsumeType> unusedConsumeTypeList = GetUnusedConsumeTypeList();
                if (unusedConsumeTypeList != null)
                {
                    string nowString = DateTime.Now.ToString("yyyyMMddHHmmss");
                    AddOneStackPanel(nowString);
                    AddOneConsumeControl(nowString, unusedConsumeTypeList);
                }
                else//All consume types are used.
                {
                    PromptWindow promptWindow = new PromptWindow(Field.PromptInformation.Information, Field.AllConsumeTypesUsed);
                    promptWindow.Show();
                }
                canAdd = false;
                Method.ShowImageByUriString(PlusImage, Field.StopImagePath);
                timer.Start();
            }
        }
        void MinusImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image MinusImage = sender as Image;
            PromptWindow promptWindow = new PromptWindow(Field.PromptInformation.Question, Field.DeletePresentConsume);
            promptWindow.Name = "PromptWindow_"+MinusImage.Name.Split('_')[1];
            promptWindow.Show();
            promptWindow.Closed += new EventHandler(promptWindow_Closed);
        }
        void promptWindow_Closed(object sender, EventArgs e)
        {
            PromptWindow promptWindow = sender as PromptWindow;
            if (promptWindow.DialogResult == true)
            {
                DeleteOneConsumeControl(promptWindow.Name.Split('_')[1]);
                SetTotalAmount();
            }
        }
        void MinusImage_MouseEnter(object sender, MouseEventArgs e)
        {
            Image MinusImage = sender as Image;
            Method.ShowImageByUriString(MinusImage, Field.MinusImagePath1);
        }
        void MinusImage_MouseLeave(object sender, MouseEventArgs e)
        {
            Image MinusImage = sender as Image;
            Method.ShowImageByUriString(MinusImage, Field.MinusImagePath);
        }
        /// <summary>
        /// According to nowString to Delete one 'ConsumeControl'.
        /// </summary>
        /// <param name="nowString"></param>
        private void DeleteOneConsumeControl(string nowString)
        {
            int count = ContentStackPanel.Children.Count;
            if (count > 0)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    if (ContentStackPanel.Children[i] is ConsumeControl)
                    {
                        ConsumeControl consumeControl = ContentStackPanel.Children[i] as ConsumeControl;
                        if (consumeControl.Name.Equals("ConsumeControl_" + nowString))
                        {
                            ContentStackPanel.Children.RemoveAt(i);
                        }
                    }
                    else//ContentStackPanel.Children[i] is StackPanel
                    {
                        StackPanel stackPanel = ContentStackPanel.Children[i] as StackPanel;
                        if (stackPanel.Name.Equals("StackPanel_" + nowString))
                        {
                            ContentStackPanel.Children.RemoveAt(i);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Add one 'StackPanel','Rectangle','Image' named after nowString.
        /// </summary>
        /// <param name="nowString"></param>
        private void AddOneStackPanel(string nowString)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Name = "StackPanel_" + nowString;
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
            ContentStackPanel.Children.Add(stackPanel);

            Rectangle rectangle = new Rectangle();
            rectangle.Name = "Rectangle_" + nowString;
            rectangle.Fill = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
            rectangle.Width = 460;
            rectangle.Height = 1;
            stackPanel.Children.Add(rectangle);

            Image MinusImage = new Image();
            MinusImage.Name = "MinusImage_" + nowString;
            MinusImage.Cursor = Cursors.Hand;
            MinusImage.Width = 32;
            MinusImage.Height = 32;
            Method.ShowImageByUriString(MinusImage, Field.MinusImagePath);
            MinusImage.MouseEnter += new MouseEventHandler(MinusImage_MouseEnter);
            MinusImage.MouseLeave += new MouseEventHandler(MinusImage_MouseLeave);
            MinusImage.MouseLeftButtonDown += new MouseButtonEventHandler(MinusImage_MouseLeftButtonDown);
            stackPanel.Children.Add(MinusImage);
        }
        /// <summary>
        /// Add one 'ConsumeControl' control named after nowString .
        /// </summary>
        /// <param name="nowString"></param>
        private void AddOneConsumeControl(string nowString, List<FamilyPortal.FamilyServiceReference.ConsumeType> unusedConsumeTypeList)
        {
            AddOneConsumeControl(nowString, unusedConsumeTypeList, null);
        }
        /// <summary>
        /// Add one 'ConsumeControl' control named after nowString and fill content with consume.
        /// </summary>
        private void AddOneConsumeControl(string nowString, List<FamilyPortal.FamilyServiceReference.ConsumeType> unusedConsumeTypeList, FamilyServiceReference.Consume consume)
        {
            if (unusedConsumeTypeList != null)
            {
                ConsumeControl consumeControl = new ConsumeControl(unusedConsumeTypeList, consume);
                consumeControl.Name = "ConsumeControl_" + nowString;
                consumeControl.Margin = new Thickness(0, 15, 0, 15);
                //Call ConsumeControl's custom event.
                consumeControl.AmountEvent += new ConsumeControl.AmountDelegate(consumeControl_AmountEvent);
                consumeControl.TypeEvent += new ConsumeControl.TypeDelegate(consumeControl_TypeEvent);
                //Add costom user control to StackPanel.
                ContentStackPanel.Children.Add(consumeControl);
            }
        }
        /// <summary>
        /// Get unused consume type list.
        /// </summary>
        /// <returns></returns>
        private List<FamilyPortal.FamilyServiceReference.ConsumeType> GetUnusedConsumeTypeList()
        {
            List<FamilyPortal.FamilyServiceReference.ConsumeType> unusedConsumeTypeList = new List<FamilyPortal.FamilyServiceReference.ConsumeType>(allConsumeTypeList);
            IEnumerable<UIElement> uIElements = ContentStackPanel.Children.Where(u => u.GetType().ToString().Equals("FamilyPortal.UserControls.ConsumeControl"));
            if (uIElements.Count() == 0)
            {
                return unusedConsumeTypeList;
            }
            else if (uIElements.Count() > 0 && uIElements.Count() < allConsumeTypeList.Count)
            {
                foreach (UIElement uIElement in uIElements)
                {
                    if (uIElement is ConsumeControl)
                    {
                        ConsumeControl consumeControl = uIElement as ConsumeControl;
                        ComboBoxItem comboBoxItem = consumeControl.TypeComboBox.SelectedItem as ComboBoxItem;
                        FamilyPortal.FamilyServiceReference.ConsumeType usedConsumeType = unusedConsumeTypeList.SingleOrDefault(u => u.typeID.Equals(Int32.Parse(comboBoxItem.Tag.ToString())));
                        unusedConsumeTypeList.Remove(usedConsumeType);
                    }
                }
                return unusedConsumeTypeList;
            }
            else//All consume type are used.
            {
                return null;
            }
        }
        void consumeControl_TypeEvent(object sender, EventArgs e)
        {
            List<FamilyPortal.FamilyServiceReference.ConsumeType> unusedConsumeTypeList = GetUnusedConsumeTypeList();
            if (unusedConsumeTypeList != null)
            {
                ComboBox comboBox = sender as ComboBox;
                ComboBoxItem selectedComboBoxItem = comboBox.SelectedItem as ComboBoxItem;
                comboBox.Items.Clear();
                FamilyPortal.FamilyServiceReference.ConsumeType item = new FamilyPortal.FamilyServiceReference.ConsumeType();
                item.typeID = Int32.Parse(selectedComboBoxItem.Tag.ToString());
                item.description = selectedComboBoxItem.Content.ToString();
                unusedConsumeTypeList.Add(item);
                IEnumerable<FamilyPortal.FamilyServiceReference.ConsumeType> sortedConsumeTypeList = unusedConsumeTypeList.OrderBy(u => u.typeID);
                foreach (FamilyPortal.FamilyServiceReference.ConsumeType model in sortedConsumeTypeList)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = model.description;
                    comboBoxItem.Tag = model.typeID;
                    if (comboBoxItem.Tag.ToString().Equals(selectedComboBoxItem.Tag.ToString()))
                    {
                        comboBoxItem.IsSelected = true;
                    }
                    comboBox.Items.Add(comboBoxItem);
                }
            }
            else//All consume types are used.
            {
                ComboBox comboBox = sender as ComboBox;
                ComboBoxItem selectedComboBoxItem = comboBox.SelectedItem as ComboBoxItem;
                comboBox.Items.Clear();
                selectedComboBoxItem.IsSelected = true;
                comboBox.Items.Add(selectedComboBoxItem);
            }
        }
        void consumeControl_AmountEvent(object sender, EventArgs e)
        {
            SetTotalAmount();
        }
        /// <summary>
        /// According to ConsumeControls to get total amount.
        /// </summary>
        private void SetTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (UIElement element in ContentStackPanel.Children)
            {
                if (element is ConsumeControl)
                {
                    ConsumeControl consumeControl = element as ConsumeControl;
                    string amountString = consumeControl.AmountTextBox.Text.Trim();
                    decimal amount = string.IsNullOrEmpty(amountString) ? 0 : decimal.Parse(amountString);
                    totalAmount += amount;
                }
            }
            DailyAmountTextBlock.Text = totalAmount.ToString();
        }
        /// <summary>
        /// User click OK button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            OKButton.IsEnabled = false;
            if (VerifyUserInput())
            {
                if (_DailyID == 0)//Add daily consume.
                {
                    AddOneDailyConsume();
                }
                else//Edit daily consume.
                {
                    UpdateDailyConsume();
                }
            }
        }
        /// <summary>
        /// User click Cancel button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            PromptWindow promptWindowCancel = new PromptWindow(Field.PromptInformation.Question, Field.Unsave);
            promptWindowCancel.Closed += new EventHandler(promptWindowCancel_Closed);
            promptWindowCancel.Show();
        }
        /// <summary>
        /// Closing the add window after confirming.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void promptWindowCancel_Closed(object sender, EventArgs e)
        {
            PromptWindow promptWindowCancel = sender as PromptWindow;
            if (promptWindowCancel.DialogResult == true)
            {
                this.DialogResult = false;
            }
        }
        /// <summary>
        /// According to user's input to get daily consume.
        /// </summary>
        /// <returns></returns>
        private FamilyPortal.FamilyServiceReference.DailyConsume GetDailyConsume()
        {
            FamilyPortal.FamilyServiceReference.DailyConsume item = new FamilyServiceReference.DailyConsume();
            item.amount = decimal.Parse(DailyAmountTextBlock.Text.Trim());
            item.date = DateTime.Parse(datePicker1.Text.Trim());
            if (_DailyID != 0)
            {
                item.dailyID = _DailyID;
            }
            return item;
        }
        /// <summary>
        /// According to user's input to get consumes.
        /// </summary>
        /// <returns></returns>
        private List<FamilyPortal.FamilyServiceReference.Consume> GetConsumes(int dailyID)
        {
            List<FamilyPortal.FamilyServiceReference.Consume> items = new List<FamilyServiceReference.Consume>();
            if (ContentStackPanel.Children.Count > 0)
            {
                foreach (UIElement element in ContentStackPanel.Children)
                {
                    if (element is ConsumeControl)
                    {
                        ConsumeControl consumeControl = element as ConsumeControl;
                        FamilyPortal.FamilyServiceReference.Consume item = consumeControl.GetConsume(dailyID);
                        items.Add(item);
                    }
                }
            }
            return items;
        }
        #region Update one daily consume.
        private void UpdateDailyConsume()
        {
            FamilyPortal.FamilyServiceReference.DailyConsume item = GetDailyConsume();
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.UpdateDailyConsumeCompleted += new EventHandler<FamilyServiceReference.UpdateDailyConsumeCompletedEventArgs>(client_UpdateDailyConsumeCompleted);
            client.UpdateDailyConsumeAsync(item);
        }

        void client_UpdateDailyConsumeCompleted(object sender, FamilyServiceReference.UpdateDailyConsumeCompletedEventArgs e)
        {
            int dailyID = e.Result;
            if (dailyID != 0)
            {
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.DeleteConsumeByDailyIDCompleted += new EventHandler<FamilyServiceReference.DeleteConsumeByDailyIDCompletedEventArgs>(client_DeleteConsumeByDailyIDCompleted);
                client.DeleteConsumeByDailyIDAsync(dailyID);
            }
        }

        void client_DeleteConsumeByDailyIDCompleted(object sender, FamilyServiceReference.DeleteConsumeByDailyIDCompletedEventArgs e)
        {
            int dailyID = e.Result;
            if (dailyID != 0)
            {
                List<FamilyPortal.FamilyServiceReference.Consume> items = GetConsumes(dailyID);
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.BatchAddConsumesCompleted += new EventHandler<FamilyServiceReference.BatchAddConsumesCompletedEventArgs>(client_BatchAddConsumesCompleted);
                client.BatchAddConsumesAsync(items);
            }
        }
        #endregion 

        #region Add one daily consume.
        private void AddOneDailyConsume()
        {
            FamilyPortal.FamilyServiceReference.DailyConsume item = GetDailyConsume();
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.AddOneDailyConsumeCompleted += new EventHandler<FamilyServiceReference.AddOneDailyConsumeCompletedEventArgs>(client_AddOneDailyConsumeCompleted);
            client.AddOneDailyConsumeAsync(item);
        }
        void client_AddOneDailyConsumeCompleted(object sender, FamilyServiceReference.AddOneDailyConsumeCompletedEventArgs e)
        {
            int dailyID = e.Result;
            if (dailyID != 0)
            {
                List<FamilyPortal.FamilyServiceReference.Consume> items = GetConsumes(dailyID);
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.BatchAddConsumesCompleted += new EventHandler<FamilyServiceReference.BatchAddConsumesCompletedEventArgs>(client_BatchAddConsumesCompleted);
                client.BatchAddConsumesAsync(items);
            }
            
        }
        void client_BatchAddConsumesCompleted(object sender, FamilyServiceReference.BatchAddConsumesCompletedEventArgs e)
        {
            if (e.Result != 0)
            {
                string message = _DailyID == 0 ? Field.AddSuccess : Field.UpdateSuccess;
                PromptWindow promptWindowSuccess = new PromptWindow(Field.PromptInformation.Information, message);
                promptWindowSuccess.Show();
                promptWindowSuccess.Closed += new EventHandler(promptWindowSuccess_Closed);
            }
            else
            {
                string message = _DailyID == 0 ? Field.AddFailed : Field.UpdateFailed;
                PromptWindow promptWindowFailed = new PromptWindow(Field.PromptInformation.Information, message);
                promptWindowFailed.Show();
                OKButton.IsEnabled = true;
            }
        }
        void promptWindowSuccess_Closed(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }
        #endregion
        /// <summary>
        /// Verify user's input.
        /// </summary>
        /// <returns></returns>
        private bool VerifyUserInput()
        {
            string date=datePicker1.Text;
            //date is requried.
            if (string.IsNullOrEmpty(date))
            {
                PromptWindow promptWindowDate = new PromptWindow(Field.PromptInformation.Information, Field.InputDate);
                promptWindowDate.Show();
                promptWindowDate.Closed += new EventHandler(promptWindowDate_Closed);
                return false;
            }
            if (ContentStackPanel.Children.Count > 0)
            {
                foreach (UIElement element in ContentStackPanel.Children)
                {
                    if (element is ConsumeControl)
                    {
                        verifyConsumeControl = element as ConsumeControl;
                        //verify amount.
                        if (string.IsNullOrEmpty(verifyConsumeControl.AmountTextBox.Text.Trim()))
                        {
                            PromptWindow promptWindowAmount = new PromptWindow(Field.PromptInformation.Information, Field.InputAmount);
                            promptWindowAmount.Show();
                            promptWindowAmount.Closed += new EventHandler(promptWindowAmount_Closed);
                            return false;
                        }
                        //verify description
                        if (string.IsNullOrEmpty(verifyConsumeControl.DescriptionTextBox.Text.Trim()))
                        {
                            PromptWindow promptWindowDescription = new PromptWindow(Field.PromptInformation.Information, Field.InputDescription);
                            promptWindowDescription.Show();
                            promptWindowDescription.Closed += new EventHandler(promptWindowDescription_Closed);
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        void promptWindowDescription_Closed(object sender, EventArgs e)
        {
            ScrollToSpecificConsumeControl(verifyConsumeControl);
            verifyConsumeControl.DescriptionTextBox.Focus();
            OKButton.IsEnabled = true;
        }
        void promptWindowAmount_Closed(object sender, EventArgs e)
        {
            ScrollToSpecificConsumeControl(verifyConsumeControl);
            verifyConsumeControl.AmountTextBox.Focus();
            OKButton.IsEnabled = true;
        }
        /// <summary>
        /// Scroll to specific consume control.
        /// </summary>
        private void ScrollToSpecificConsumeControl(ConsumeControl consumeControl)
        {
            //Get the point of consume control which is being verfied. 
            var gt = consumeControl.TransformToVisual(ContentStackPanel);
            Point point = gt.Transform(new Point(0, 0));
            //Scroll to the consume control which is being verifed.
            ScrollViewer1.ScrollToVerticalOffset(point.Y - 15);
        }
        void promptWindowDate_Closed(object sender, EventArgs e)
        {
            datePicker1.Focus();
            OKButton.IsEnabled = true;
        }
        /// <summary>
        /// Display daily consume controls according to if gets data.
        /// </summary>
        /// <param name="isGetData"></param>
        private void DisplayDailyConsumeControls(bool isGetData)
        {
            if (isGetData)
            {
                if (ScrollViewer1.Visibility == Visibility.Collapsed)
                {
                    ScrollViewer1.Visibility = Visibility.Visible;
                }
                if (ProgressBarCanvas.Visibility == Visibility.Visible)
                {
                    ProgressBarCanvas.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (ScrollViewer1.Visibility == Visibility.Visible)
                {
                    ScrollViewer1.Visibility = Visibility.Collapsed;
                }
                if (ProgressBarCanvas.Visibility == Visibility.Collapsed)
                {
                    ProgressBarCanvas.Visibility = Visibility.Visible;
                }
            }
        }
    }
}


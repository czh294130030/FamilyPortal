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
using System.Windows.Data;
using FamilyPortal.Silverlight.Common;
using System.Reflection;
using System.Windows.Controls.DataVisualization.Charting;

namespace FamilyPortal.DailyConsume
{
    public partial class DailyConsume : Page
    {
        private List<KeyValue> statisticList = null;
        private List<NewDailyConsume> dailyConsumes = null;
        private List<FamilyServiceReference.NewConsume> consumes = null;
        private DateTime startDate = DateTime.Now.Date;
        private DateTime endDate = DateTime.Now.Date;
        private Field.TimeType selectedTimeType = Field.TimeType.This;
        private string selectedTimePeriod = string.Empty;
        private Field.ChartType chartType = Field.ChartType.Column;
        private string strTimePeriod = string.Empty;
        private string strTime = string.Empty;
        public DailyConsume()
        {
            InitializeComponent();
            Method.ShowImageByUriString(CalendarImage, Field.DatePath);
            DailyConsumeDataGrid.SizeChanged += new SizeChangedEventHandler(DailyConsumeDataGrid_SizeChanged);
            CalendarImage.MouseLeftButtonDown += new MouseButtonEventHandler(CalendarImage_MouseLeftButtonDown);
            CancelHyperlinkButton.Click += new RoutedEventHandler(CancelHyperlinkButton_Click);
            ApplyHyperlinkButton.Click += new RoutedEventHandler(ApplyHyperlinkButton_Click);
            TimeTypeComboBox.SelectionChanged += new SelectionChangedEventHandler(TimeTypeComboBox_SelectionChanged);
            AddHyperlinkButton.Click += new RoutedEventHandler(AddHyperlinkButton_Click);
            DailyConsumeDataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(DailyConsumeDataGrid_LoadingRow);

            #region setting color
            TimePeriodTextBlock.Foreground = new SolidColorBrush(Field.fontColor);
            TimeTextBlock.Foreground = new SolidColorBrush(Field.fontColor);
            DaysTextBlock.Foreground = new SolidColorBrush(Field.fontColor);
            ConsumeChart.Foreground = new SolidColorBrush(Field.fontColor);
            #endregion
        }
        /// <summary>
        /// Setting tooltip when loading datagrid's row.
        /// </summary>
        void DailyConsumeDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            NewDailyConsume item = e.Row.DataContext as NewDailyConsume;
            ToolTipService.SetToolTip(e.Row, GenerateTooltipContent(item));
        }

        #region Generate tooltip's content.
        private StackPanel GenerateTooltipContent(NewDailyConsume item)
        {
            StackPanel tooltipStackPanel = new StackPanel();
            tooltipStackPanel.Orientation = Orientation.Vertical;
            tooltipStackPanel.Width = 300;
            //daily consume
            StackPanel dailyConsumeStackPanel = new StackPanel();
            dailyConsumeStackPanel.Orientation = Orientation.Horizontal;
            TextBlock datetb = new TextBlock();
            datetb.Text = "Date";
            datetb.Foreground = new SolidColorBrush(Field.fontColor);
            datetb.Margin = new Thickness(10, 5, 10, 5);
            dailyConsumeStackPanel.Children.Add(datetb);
            TextBlock dateTextBlock = new TextBlock();
            dateTextBlock.Text = item.Date;
            dateTextBlock.Margin = new Thickness(10, 5, 10, 5);
            dailyConsumeStackPanel.Children.Add(dateTextBlock);
            TextBlock dailyamounttb = new TextBlock();
            dailyamounttb.Text = "Daily Amount";
            dailyamounttb.Foreground = new SolidColorBrush(Field.fontColor);
            dailyamounttb.Margin = new Thickness(10, 5, 10, 5);
            dailyConsumeStackPanel.Children.Add(dailyamounttb);
            TextBlock dailyAmountTextBlock = new TextBlock();
            dailyAmountTextBlock.Text = "￥ " + item.Amount.ToString();
            dailyAmountTextBlock.Margin = new Thickness(10, 5, 10, 5);
            dailyConsumeStackPanel.Children.Add(dailyAmountTextBlock);
            tooltipStackPanel.Children.Add(dailyConsumeStackPanel);
            //consume
            IEnumerable<FamilyServiceReference.NewConsume> models = consumes.Where(u => u.dailyID.Equals(item.DailyID));
            if (models.Count() > 0)
            {
                foreach (FamilyServiceReference.NewConsume model in models)
                {
                    //type
                    StackPanel consumeStackPanel = new StackPanel();
                    consumeStackPanel.Orientation = Orientation.Horizontal;
                    TextBlock typetb = new TextBlock();
                    typetb.Text = "Type";
                    typetb.Foreground = new SolidColorBrush(Field.fontColor);
                    typetb.Margin = new Thickness(10, 5, 10, 5);
                    consumeStackPanel.Children.Add(typetb);
                    TextBlock typeTextBlock = new TextBlock();
                    typeTextBlock.Text = model.typeDesc;
                    typeTextBlock.Width = 90;
                    typeTextBlock.Margin = new Thickness(10, 5, 10, 5);
                    consumeStackPanel.Children.Add(typeTextBlock);
                    //amount
                    TextBlock amounttb = new TextBlock();
                    amounttb.Text = "Amount";
                    amounttb.Foreground = new SolidColorBrush(Field.fontColor);
                    amounttb.Margin = new Thickness(16, 5, 10, 5);
                    consumeStackPanel.Children.Add(amounttb);
                    TextBlock amountTextBlock = new TextBlock();
                    amountTextBlock.Text = "￥ " + model.amount.ToString();
                    amountTextBlock.Margin = new Thickness(10, 5, 10, 5);
                    consumeStackPanel.Children.Add(amountTextBlock);
                    tooltipStackPanel.Children.Add(consumeStackPanel);
                    //descrption
                    TextBlock descriptionTextBlock = new TextBlock();
                    descriptionTextBlock.Text = model.description;
                    descriptionTextBlock.Margin = new Thickness(10, 5, 10, 5);
                    tooltipStackPanel.Children.Add(descriptionTextBlock);
                }
            }
            return tooltipStackPanel;
        }
        #endregion

        void ApplyHyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ThisStackPanel.Visibility == Visibility.Visible)//this
            {
                selectedTimeType = Field.TimeType.This;
                ComboBoxItem selectedComboBoxItem = ThisComboBox.SelectedItem as ComboBoxItem;
                selectedTimePeriod = selectedComboBoxItem.Content.ToString();
                strTimePeriod = Method.GenerateDisplayDate(selectedTimeType, selectedTimePeriod);
                //Convert string enum
                Field.TimePeriod timePeriod = (Field.TimePeriod)Enum.Parse(typeof(Field.TimePeriod), selectedComboBoxItem.Content.ToString(), true);
                Method.GetDates(timePeriod, ref startDate, ref endDate);
                TimeTextBlock.Visibility = Visibility.Visible;
            }
            if (LastStackPanel.Visibility == Visibility.Visible)//last
            {
                selectedTimeType = Field.TimeType.Last;
                int days = 0;
                if (!Int32.TryParse(LastTextBox.Text, out days) || days == 0)
                {
                    PromptWindow promptWindowLast = new PromptWindow(Field.PromptInformation.Information, Field.InputLastDays);
                    promptWindowLast.Show();
                    return;
                }
                endDate = DateTime.Now.Date;
                startDate = endDate.AddDays(1 - days);
                strTimePeriod = Method.GenerateDisplayDate(selectedTimeType, days.ToString());
                TimeTextBlock.Visibility = Visibility.Visible;
            }
            if (SpecificStackPanel.Visibility == Visibility.Visible)//specific
            {
                selectedTimeType = Field.TimeType.Specific;
                if (!DateTime.TryParse(startDateDatePicker.Text, out startDate))//start date is required.
                {
                    PromptWindow promptWindowStartDate = new PromptWindow(Field.PromptInformation.Information, Field.InputStartDate);
                    promptWindowStartDate.Show();
                    return;
                }
                if (!DateTime.TryParse(endDateDatePicker.Text, out endDate))//end date is required.
                {
                    PromptWindow promptWindowEndDate = new PromptWindow(Field.PromptInformation.Information, Field.InputEndDate);
                    promptWindowEndDate.Show();
                    return;
                }
                if (startDate > endDate)//end date must be greater than or equal to start date.
                {
                    PromptWindow promptWindowGreater = new PromptWindow(Field.PromptInformation.Information, Field.EndDateGreaterThanOrEqualToStartDate);
                    promptWindowGreater.Show();
                    return;
                }
                selectedTimePeriod = Method.ConvertDateTimeToString(startDate) + " - " + Method.ConvertDateTimeToString(endDate);
                strTimePeriod = Method.GenerateDisplayDate(selectedTimeType, selectedTimePeriod);
                TimeTextBlock.Visibility = Visibility.Collapsed;
            }
            DisplayDailyConsumeControls(false);
            DisplayConsumeControls(false);
            BindDailyConsume(startDate, endDate);
            CollapseTimeSelectElement();
        }
        void CancelHyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            CollapseTimeSelectElement();
        }
        void TimeTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = TimeTypeComboBox.SelectedItem as ComboBoxItem;
            if (selectedComboBoxItem != null)
            {
                Field.TimeType timeType = (Field.TimeType)Enum.Parse(typeof(Field.TimeType), selectedComboBoxItem.Content.ToString(), true);
                switch (timeType)
                {
                    case Field.TimeType.This:
                        ShowThis();
                        CollapseTimeSelectElement(Field.TimeType.This);
                        break;
                    case Field.TimeType.Last:
                        ShowLast();
                        CollapseTimeSelectElement(Field.TimeType.Last);
                        break;
                    case Field.TimeType.Specific:
                        ShowSpecific();
                        CollapseTimeSelectElement(Field.TimeType.Specific);
                        break;
                    default:
                        ShowThis();
                        CollapseTimeSelectElement(Field.TimeType.This);
                        break;
                }
            }
        }
        void CalendarImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (TimeSelectStackPanel.Visibility == Visibility.Collapsed)
            {
                TimeSelectStackPanel.Visibility = Visibility.Visible;
                ShowTimeType();
            }
        }

        #region Collapse elements which are belong TimeSelectStackPanel
        /// <summary>
        /// Collapse all elements which are belong TimeSelectStackPanel.
        /// </summary>
        private void CollapseTimeSelectElement()
        {
            if (TimeSelectStackPanel.Visibility == Visibility.Visible)
            {
                TimeSelectStackPanel.Visibility = Visibility.Collapsed;
            }
            if (ThisStackPanel.Visibility == Visibility.Visible)//This
            {
                ThisStackPanel.Visibility = Visibility.Collapsed;
            }
            if (LastStackPanel.Visibility == Visibility.Visible)//Last
            {
                LastStackPanel.Visibility = Visibility.Collapsed;
            }
            if (SpecificStackPanel.Visibility == Visibility.Visible)//Specific
            {
                SpecificStackPanel.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// Collapse some elements which are belong TimeSelectStackPanel.
        /// </summary>
        /// <param name="type"></param>
        private void CollapseTimeSelectElement(Field.TimeType type)
        {
            switch (type)
            { 
                case Field.TimeType.This:
                    if (LastStackPanel.Visibility == Visibility.Visible)//Last
                    {
                        LastStackPanel.Visibility = Visibility.Collapsed;
                    }
                    if (SpecificStackPanel.Visibility == Visibility.Visible)//Specific
                    {
                        SpecificStackPanel.Visibility = Visibility.Collapsed;
                    }
                    break;
                case Field.TimeType.Last:
                    if (ThisStackPanel.Visibility == Visibility.Visible)//This
                    {
                        ThisStackPanel.Visibility = Visibility.Collapsed;
                    }
                    if (SpecificStackPanel.Visibility == Visibility.Visible)//Specific
                    {
                        SpecificStackPanel.Visibility = Visibility.Collapsed;
                    }
                    break;
                case Field.TimeType.Specific:
                    if (ThisStackPanel.Visibility == Visibility.Visible)//This
                    {
                        ThisStackPanel.Visibility = Visibility.Collapsed;
                    }
                    if (LastStackPanel.Visibility == Visibility.Visible)//Last
                    {
                        LastStackPanel.Visibility = Visibility.Collapsed;
                    }
                    break;
                default:
                    if (LastStackPanel.Visibility == Visibility.Visible)//Last
                    {
                        LastStackPanel.Visibility = Visibility.Collapsed;
                    }
                    if (SpecificStackPanel.Visibility == Visibility.Visible)//Specific
                    {
                        SpecificStackPanel.Visibility = Visibility.Collapsed;
                    }
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Show time type.
        /// </summary>
        private void ShowTimeType()
        {
            TimeTypeComboBox.Items.Clear();
            Type enumType = typeof(Field.TimeType);
            FieldInfo[] items = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo item in items)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item.Name;
                if (item.Name.ToString().Equals(selectedTimeType.ToString())) { comboBoxItem.IsSelected = true; }
                TimeTypeComboBox.Items.Add(comboBoxItem);
            }
        }
        /// <summary>
        /// This
        /// </summary>
        private void ShowThis()
        {
            ThisStackPanel.Visibility = Visibility.Visible;
            ThisComboBox.Items.Clear();
            Type enumType = typeof(Field.TimePeriod);
            FieldInfo[] items = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < items.Count(); i++)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = items[i].Name;
                //The first ComboBoxItem should be selected, when there is no items[i].Name.ToString() equals selectedTimePeriod.
                if (i == 0) { comboBoxItem.IsSelected = true; }
                if (items[i].Name.ToString().Equals(selectedTimePeriod)) { comboBoxItem.IsSelected = true; }
                ThisComboBox.Items.Add(comboBoxItem);
            }
        }
        /// <summary>
        /// Last
        /// </summary>
        private void ShowLast()
        {
            LastStackPanel.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Specific
        /// </summary>
        private void ShowSpecific()
        {
            SpecificStackPanel.Visibility = Visibility.Visible;
        }

        void DailyConsumeDataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Method.EqualDistributionDataGrid(DailyConsumeDataGrid, 2);
        }

        void AddHyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            AddDailyConsume addDailyConsume = new AddDailyConsume();
            addDailyConsume.Title = "Add Daily Consume";
            addDailyConsume.Closed += new EventHandler(addDailyConsume_Closed);
            addDailyConsume.Show();
        }

        void addDailyConsume_Closed(object sender, EventArgs e)
        {
            AddDailyConsume addDailyConsume = sender as AddDailyConsume;
            if (addDailyConsume.DialogResult == true)//Add daily consume successfully.
            {
                BindDailyConsume(startDate, endDate);//Binding daily consume.
            }
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            selectedTimeType = Field.TimeType.This;
            selectedTimePeriod = Field.TimePeriod.Month.ToString();
            strTimePeriod = Method.GenerateDisplayDate(selectedTimeType, selectedTimePeriod);
            Method.GetDates(Field.TimePeriod.Month, ref startDate, ref endDate);
            DisplayDailyConsumeControls(false);
            DisplayConsumeControls(false);
            BindDailyConsume(startDate, endDate);
        }
        /// <summary>
        /// binding daily consume.
        /// </summary>
        private void BindDailyConsume(DateTime startDate, DateTime endDate)
        {
            strTime = "(" + Method.ConvertDateTimeToString(startDate) + " - " + Method.ConvertDateTimeToString(endDate) + ")";
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetDailyConsumeCompleted += new EventHandler<FamilyServiceReference.GetDailyConsumeCompletedEventArgs>(client_GetDailyConsumeCompleted);
            client.GetDailyConsumeAsync(startDate, endDate);
        }

        void client_GetDailyConsumeCompleted(object sender, FamilyServiceReference.GetDailyConsumeCompletedEventArgs e)
        {
            if (e.Result.Count > 0)
            {
                //Get daily consume.
                dailyConsumes = RemoveHourMinuteSecondOfDateTime(e.Result);
                //Get daily consume ids.
                List<int?> dailyConsumeIDs = GetDailyConsumeIDs();
                //Get consumes by daily consume ids.
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.GetConsumeByDailyIDsCompleted += new EventHandler<FamilyServiceReference.GetConsumeByDailyIDsCompletedEventArgs>(client_GetConsumeByDailyIDsCompleted);
                client.GetConsumeByDailyIDsAsync(dailyConsumeIDs);
            }
            else
            {
                DisplayDailyConsumeControls(true);
                DisplayConsumeControls(true);
                PromptWindow noDataPrompt = new PromptWindow(Field.PromptInformation.Information, Field.NoData);
                noDataPrompt.Show();
            }
        }
        /// <summary>
        /// Display daily consume controls according to if gets data.
        /// </summary>
        /// <param name="isGetData"></param>
        private void DisplayDailyConsumeControls(bool isGetsData)
        {
            if (isGetsData)
            {
                if (DailyConsumeDataGrid.Visibility == Visibility.Collapsed)
                {
                    DailyConsumeDataGrid.Visibility = Visibility.Visible;
                }
                if (DailyConsumeDataPager.Visibility == Visibility.Collapsed)
                {
                    DailyConsumeDataPager.Visibility = Visibility.Visible;
                }
                if (DailyConsumeProgressBar.Visibility == Visibility.Visible)
                {
                    DailyConsumeProgressBar.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (DailyConsumeDataGrid.Visibility == Visibility.Visible)
                {
                    DailyConsumeDataGrid.Visibility = Visibility.Collapsed;
                }
                if (DailyConsumeDataPager.Visibility == Visibility.Visible)
                {
                    DailyConsumeDataPager.Visibility = Visibility.Collapsed;
                }
                if (DailyConsumeProgressBar.Visibility == Visibility.Collapsed)
                {
                    DailyConsumeProgressBar.Visibility = Visibility.Visible;
                }
            }
        }
        /// <summary>
        /// Display consume controls according to if get data.
        /// </summary>
        /// <param name="isGetsData"></param>
        private void DisplayConsumeControls(bool isGetsData)
        {
            if (isGetsData)
            {
                if (ConsumeChart.Visibility == Visibility.Collapsed)
                {
                    ConsumeChart.Visibility = Visibility.Visible;
                }
                if (DetailWrapPanel.Visibility == Visibility.Collapsed)
                {
                    DetailWrapPanel.Visibility = Visibility.Visible;
                }
                if (ConsumeProgressBar.Visibility == Visibility.Visible)
                {
                    ConsumeProgressBar.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (ConsumeChart.Visibility == Visibility.Visible)
                {
                    ConsumeChart.Visibility = Visibility.Collapsed;
                }
                if (DetailWrapPanel.Visibility == Visibility.Visible)
                {
                    DetailWrapPanel.Visibility = Visibility.Collapsed;
                }
                if (ConsumeProgressBar.Visibility == Visibility.Collapsed)
                {
                    ConsumeProgressBar.Visibility = Visibility.Visible;
                }
            }
        }
        /// <summary>
        /// Removing hour, minute and second of datetime displayed in datagrid.
        /// </summary>
        private List<NewDailyConsume> RemoveHourMinuteSecondOfDateTime(List<FamilyServiceReference.DailyConsume> list)
        {
            var query = from l in list
                        select new NewDailyConsume
                         {
                             DailyID=l.dailyID,
                             Amount=l.amount,
                             Date=Convert.ToDateTime(l.date).ToString("yyyy-MM-dd")
                         };
            return query.ToList();
        }

        void client_GetConsumeByDailyIDsCompleted(object sender, FamilyServiceReference.GetConsumeByDailyIDsCompletedEventArgs e)
        {
            if (e.Result.Count > 0)
            {
                consumes = e.Result;
                //daily consume
                DisplayDailyConsumeControls(true);
                TimePeriodTextBlock.Text = strTimePeriod;
                TimeTextBlock.Text = strTime;
                PagedCollectionView pagedCollectionView = new PagedCollectionView(dailyConsumes);
                pagedCollectionView.PageSize = Field.PageSize;
                DailyConsumeDataGrid.ItemsSource = pagedCollectionView;
                DailyConsumeDataPager.Source = pagedCollectionView;

                //consume
                DisplayConsumeControls(true);
                statisticList = GetConsumeStatistic(consumes);
                BindingChart();
                BindDetails();
            }
        }
        #region Init context menu of chart.
        private void InitContextMenuOfChart()
        {
            ContextMenu ChartContextMenu = new ContextMenu();
            switch(chartType)
            {
                case Field.ChartType.Column:
                    {
                        //Line Chart
                        MenuItem LineMenuItem = new MenuItem();
                        LineMenuItem.Header = "Line Chart";
                        Image LineImage = new Image();
                        Method.ShowImageByUriString(LineImage, Field.LineChartPath);
                        LineImage.Width = 16;
                        LineImage.Height = 16;
                        LineMenuItem.Icon = LineImage;
                        LineMenuItem.Click += new RoutedEventHandler(LineMenu_Click);
                        ChartContextMenu.Items.Add(LineMenuItem);
                        //Pie Chart
                        MenuItem PieMenuItem = new MenuItem();
                        PieMenuItem.Header = "Pie Chart";
                        Image PieImage = new Image();
                        Method.ShowImageByUriString(PieImage, Field.PieChartPath);
                        PieImage.Width = 16;
                        PieImage.Height = 16;
                        PieMenuItem.Icon = PieImage;
                        PieMenuItem.Click += new RoutedEventHandler(PieMenuItem_Click);
                        ChartContextMenu.Items.Add(PieMenuItem);
                        break;
                    }
                case Field.ChartType.Line:
                    {
                        //Column Chart
                        MenuItem ColumnMenuItem = new MenuItem();
                        ColumnMenuItem.Header = "Column Chart";
                        Image ColumnImage = new Image();
                        Method.ShowImageByUriString(ColumnImage, Field.ColumnChartPath);
                        ColumnImage.Width = 16;
                        ColumnImage.Height = 16;
                        ColumnMenuItem.Icon = ColumnImage;
                        ColumnMenuItem.Click += new RoutedEventHandler(ColumnMenuItem_Click);
                        ChartContextMenu.Items.Add(ColumnMenuItem);
                        //Pie Chart
                        MenuItem PieMenuItem = new MenuItem();
                        PieMenuItem.Header = "Pie Chart";
                        Image PieImage = new Image();
                        Method.ShowImageByUriString(PieImage, Field.PieChartPath);
                        PieImage.Width = 16;
                        PieImage.Height = 16;
                        PieMenuItem.Icon = PieImage;
                        PieMenuItem.Click += new RoutedEventHandler(PieMenuItem_Click);
                        ChartContextMenu.Items.Add(PieMenuItem);
                        break;
                    }
                case Field.ChartType.Pie:
                    {
                        //Column Chart
                        MenuItem ColumnMenuItem = new MenuItem();
                        ColumnMenuItem.Header = "Column Chart";
                        Image ColumnImage = new Image();
                        Method.ShowImageByUriString(ColumnImage, Field.ColumnChartPath);
                        ColumnImage.Width = 16;
                        ColumnImage.Height = 16;
                        ColumnMenuItem.Icon = ColumnImage;
                        ColumnMenuItem.Click += new RoutedEventHandler(ColumnMenuItem_Click);
                        ChartContextMenu.Items.Add(ColumnMenuItem);
                        //Line Chart
                        MenuItem LineMenuItem = new MenuItem();
                        LineMenuItem.Header = "Line Chart";
                        Image LineImage = new Image();
                        Method.ShowImageByUriString(LineImage, Field.LineChartPath);
                        LineImage.Width = 16;
                        LineImage.Height = 16;
                        LineMenuItem.Icon = LineImage;
                        LineMenuItem.Click += new RoutedEventHandler(LineMenu_Click);
                        ChartContextMenu.Items.Add(LineMenuItem);
                        break;
                    }
                default:
                    {
                        //Line Chart
                        MenuItem LineMenuItem = new MenuItem();
                        LineMenuItem.Header = "Line Chart";
                        Image LineImage = new Image();
                        Method.ShowImageByUriString(LineImage, Field.LineChartPath);
                        LineImage.Width = 16;
                        LineImage.Height = 16;
                        LineMenuItem.Icon = LineImage;
                        LineMenuItem.Click += new RoutedEventHandler(LineMenu_Click);
                        ChartContextMenu.Items.Add(LineMenuItem);
                        //Pie Chart
                        MenuItem PieMenuItem = new MenuItem();
                        PieMenuItem.Header = "Pie Chart";
                        Image PieImage = new Image();
                        Method.ShowImageByUriString(PieImage, Field.PieChartPath);
                        PieImage.Width = 16;
                        PieImage.Height = 16;
                        PieMenuItem.Icon = PieImage;
                        PieMenuItem.Click += new RoutedEventHandler(PieMenuItem_Click);
                        ChartContextMenu.Items.Add(PieMenuItem);
                        break;
                    }
            }
            ContextMenuService.SetContextMenu(ConsumeChart, ChartContextMenu);
        }
        void PieMenuItem_Click(object sender, RoutedEventArgs e)
        {
            chartType = Field.ChartType.Pie;
            BindingChart();
        }
        void LineMenu_Click(object sender, RoutedEventArgs e)
        {
            chartType = Field.ChartType.Line;
            BindingChart();
        }
        void ColumnMenuItem_Click(object sender, RoutedEventArgs e)
        {
            chartType = Field.ChartType.Column;
            BindingChart();
        }
        #endregion
        #region Binding chart according to type.
        private void BindingChart()
        {
            switch (chartType)
            { 
                case Field.ChartType.Column:
                    BindingColumnChart();
                    InitContextMenuOfChart();
                    break;
                case Field.ChartType.Line:
                    BindingLineChart();
                    InitContextMenuOfChart();
                    break;
                case Field.ChartType.Pie:
                    BindingPieChart();
                    InitContextMenuOfChart();
                    break;
                default:
                    BindingColumnChart();
                    InitContextMenuOfChart();
                    break;
            }
        }
 
        /// <summary>
        /// Binding line chart
        /// </summary>
        private void BindingLineChart()
        {
            LineSeries lineSeries = new LineSeries();
            lineSeries.ItemsSource = statisticList;
            lineSeries.DependentValueBinding = new Binding("Value");
            lineSeries.IndependentValueBinding = new Binding("Key");
            lineSeries.Title = "Monetary(￥)";
            ConsumeChart.Series.Clear();
            ConsumeChart.Series.Add(lineSeries);
        }
        /// <summary>
        /// Binding pie chart.
        /// </summary>
        private void BindingPieChart()
        {
            PieSeries pieSeries = new PieSeries();
            pieSeries.ItemsSource = statisticList;
            pieSeries.DependentValueBinding = new Binding("Value");
            pieSeries.IndependentValueBinding = new Binding("Key");
            pieSeries.Title = "Monetary(￥)";
            ConsumeChart.Series.Clear();
            ConsumeChart.Series.Add(pieSeries);
        }
        /// <summary>
        /// Binding column chart.
        /// </summary>
        private void BindingColumnChart()
        {
            ColumnSeries columnSeries = new ColumnSeries();
            columnSeries.ItemsSource = statisticList;
            columnSeries.DependentValueBinding = new Binding("Value");
            columnSeries.IndependentValueBinding = new Binding("Key");
            columnSeries.Title = "Monetary(￥)";
            ConsumeChart.Series.Clear();
            ConsumeChart.Series.Add(columnSeries);
        }
        #endregion

        #region binding consume details
        private void BindDetails()
        {
            //Clear all controls which are belong in DetailStackPanel.
            DetailWrapPanel.Children.Clear();

            decimal totalAmount = 0; 
            if (statisticList.Count > 0)
            {
                foreach (KeyValue item in statisticList)
                {
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Margin = new Thickness(10, 0, 10, 0);
                    stackPanel.Orientation = Orientation.Horizontal;
                    TextBlock KeyTextBlock = new TextBlock();
                    KeyTextBlock.Text = item.Key.ToString();
                    KeyTextBlock.Foreground = new SolidColorBrush(Field.fontColor);
                    KeyTextBlock.Margin = new Thickness(0, 0, 5, 0);
                    stackPanel.Children.Add(KeyTextBlock);
                    TextBlock ValueTextBlock = new TextBlock();
                    ValueTextBlock.Text = item.Value.ToString();
                    stackPanel.Children.Add(ValueTextBlock);
                    DetailWrapPanel.Children.Add(stackPanel);
                    totalAmount += decimal.Parse(item.Value.ToString());
                }
                StackPanel TotalStackPanel = new StackPanel();
                TotalStackPanel.Margin = new Thickness(10, 0, 10, 0);
                TotalStackPanel.Orientation = Orientation.Horizontal;
                TextBlock TotalKeyTextBlock = new TextBlock();
                TotalKeyTextBlock.Text = Field.TotalAmount;
                TotalKeyTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                TotalKeyTextBlock.Margin = new Thickness(0, 0, 5, 0);
                TotalStackPanel.Children.Add(TotalKeyTextBlock);
                TextBlock TotalValueTextBlock = new TextBlock();
                TotalValueTextBlock.Text = totalAmount.ToString();
                TotalStackPanel.Children.Add(TotalValueTextBlock);
                DetailWrapPanel.Children.Add(TotalStackPanel);
            }
        }
        #endregion

        /// <summary>
        /// Get Consume statistic information.
        /// </summary>
        private List<KeyValue> GetConsumeStatistic(List<FamilyServiceReference.NewConsume> items)
        {
            var query = from a in items
                        orderby a.typeID ascending
                        group a by a.typeDesc into b
                        select new KeyValue
                        {
                            Key = b.Key,
                            Value = b.Sum(u => u.amount)
                        };
            return query.ToList();
        }
        
        /// <summary>
        /// Get daily consume IDs.
        /// </summary>
        /// <returns></returns>
        private List<int?> GetDailyConsumeIDs()
        {
            List<int?> dailyConsumeIDs = new List<int?>();
            for (int i = 0; i < dailyConsumes.Count;i++ )
            {
                dailyConsumeIDs.Add(dailyConsumes[i].DailyID);
            }
            return dailyConsumeIDs;
        }
        /// <summary>
        /// Edit daily consume by id.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            int dailyID = Int32.Parse(menuItem.Tag.ToString());
            AddDailyConsume addDailyConsume = new AddDailyConsume(dailyID);
            addDailyConsume.Title = "Edit Daily Consume";
            addDailyConsume.Closed += new EventHandler(addDailyConsume_Closed);
            addDailyConsume.Show();
        }

        #region Delete daily consume by id.
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            int dailyID = Int32.Parse(menuItem.Tag.ToString());
            PromptWindow DeletePromptWindow = new PromptWindow(Field.PromptInformation.Question, Field.ConfirmToDelete);
            DeletePromptWindow.Tag = dailyID;
            DeletePromptWindow.Closed += new EventHandler(DeletePromptWindow_Closed);
            DeletePromptWindow.Show();
        }

        void DeletePromptWindow_Closed(object sender, EventArgs e)
        {
            PromptWindow DeletePromptWindow = sender as PromptWindow;
            if (DeletePromptWindow.DialogResult == true)
            {
                int dailyID = Int32.Parse(DeletePromptWindow.Tag.ToString());
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.DeleteDailyConsumeByIDCompleted += new EventHandler<FamilyServiceReference.DeleteDailyConsumeByIDCompletedEventArgs>(client_DeleteDailyConsumeByIDCompleted);
                client.DeleteDailyConsumeByIDAsync(dailyID);
            }
        }

        void client_DeleteDailyConsumeByIDCompleted(object sender, FamilyServiceReference.DeleteDailyConsumeByIDCompletedEventArgs e)
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
            if (e.Result != 0)
            {
                PromptWindow DeleteSuccessPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.DeleteSuccess);
                DeleteSuccessPromptWindow.Closed += new EventHandler(DeleteSuccessPromptWindow_Closed);
                DeleteSuccessPromptWindow.Show();
            }
        }

        void DeleteSuccessPromptWindow_Closed(object sender, EventArgs e)
        {
            BindDailyConsume(startDate, endDate);//Binding daily consume.
        }
        #endregion
    }
}

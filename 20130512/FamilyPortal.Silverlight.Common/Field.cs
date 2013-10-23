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
    public class Field
    {
        #region enum
        /// <summary>
        /// Specifies information to display.
        /// </summary>
        public enum PromptInformation
        {
            Error,
            Question,
            Warning,
            Information,
        }
        /// <summary>
        /// Time type.
        /// </summary>
        public enum TimeType
        { 
            This,
            Last,
            Specific
        }
        /// <summary>
        /// Time period.
        /// </summary>
        public enum TimePeriod
        {
            Day,
            Week,
            Month,
            Quarter,
            Year
        }
        /// <summary>
        /// Type of chart.
        /// </summary>
        public enum ChartType
        { 
            Line,
            Pie,
            Column
        }
        #endregion

        #region int
        public const int PageSize = 18;
        public const int CardTypeID = 1;
        public const int BankID = 2;
        public const int CityID = 3;
        public const int BankCardID = 1;
        public const int BankBookID = 2;
        #endregion

        #region string
        public static string PleaseLogin = "You have no authority to use this block. Please login first.";
        /// <summary>
        /// Such as:0, 0.8, 0.88, 8, 8.8, 88.88
        /// </summary>
        public static string pattern = @"^(0|[1-9]\d{0,17})(\.\d{1,2})?$";
        public static string InputCardNO = "Please input card No.";
        public static string NoData = "There is no data in this period.";
        public static string TotalAmount = "total amount";
        public static string InputLastDays = "Please input correct last xx days.";
        public static string EndDateGreaterThanOrEqualToStartDate = "End date must be greater than or equal to start date.";
        public static string InputEndDate = "Please input correct end date.";
        public static string InputStartDate = "Please input correct start date.";
        public static string DateExists = "There is existed daily consume on this day.";
        public static string Unsave = "Unsaved changes will be lost.";
        public static string ConfirmToDelete = "Confirm to delete?";
        public static string UpdateFailed="Fail to update.";
        public static string UpdateSuccess = "Update successfully.";
        public static string DeleteFailed = "Fail to delete.";
        public static string DeleteSuccess = "Delete successfully.";
        public static string AddFailed = "Fail to add.";
        public static string AddSuccess = "Add successfully.";
        public static string InputDescription = "Please input description.";
        public static string InputAmount = "Please input amount.";
        public static string InputDate = "Please input date.";
        public static string AllConsumeTypesUsed = "All consume types are used.";
        public static string AddOneMoreConsume= "Add one more consume details.";
        public static string DeletePresentConsume = "Do you want to delete present consume?";
        public static string InputAmountInvalid = "Amount you input is not valid.";
        public static string AmountDemo = "Such as: 0, 8, 8.8, 88.88";
        public static string InputAccount = "Please input account.";
        public static string InputPassword = "Please input password.";
        public static string LoginFailed = "Login failed, please try again.";
        public static string CopyRight = "Copy Right © 2012 - 2020 Chad&Catherine. All rights reserved.";
        public static string Guest = "Guest";
        public static string Login = "Login";
        public static string Logout = "Logout";
        public static string ConfirmLogout = "Confirm to logout?";
        #endregion

        #region image path
        public static string ColumnChartPath = "../Resources/ColumnSeries.png";
        public static string PieChartPath = "../Resources/PieSeries.png";
        public static string LineChartPath = "../Resources/LineSeries.png";
        public static string DatePath = "../Resources/date.png";
        public static string StopImagePath = "../Resources/stop.png";
        public static string InfoImagePath = "Resources/info.png";
        public static string HelpImagePath = "Resources/help.png";
        public static string PlusImagePath = "../Resources/plus.png";
        public static string PlusImagePath1 = "../Resources/plus1.png";
        public static string MinusImagePath = "../Resources/minus.png";
        public static string MinusImagePath1 = "../Resources/minus1.png";
        public static string PromptImagePath = "../Resources/prompt.png";
        #endregion

        #region bool
        public static bool IsTest = false;
        #endregion

        #region other
        public static Color fontColor = Color.FromArgb(255, 43, 124, 205);
        #endregion
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FamilyPortal.Silverlight.Common;
using FamilyPortal.FamilyServiceReference;

namespace FamilyPortal
{
    public partial class MainPage : UserControl
    {
        private string targetPage = string.Empty;
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            LogLink.Click += new RoutedEventHandler(LogLink_Click);
            ConsumeLink.Click += new RoutedEventHandler(ConsumeLink_Click);
            CardLink.Click += new RoutedEventHandler(CardLink_Click);
            CopyRightTextBlock.Text = Field.CopyRight;
        }
        /// <summary>
        /// Navigating to bank card page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CardLink_Click(object sender, RoutedEventArgs e)
        {
            targetPage = "/BankCard";
            NavigateToTargetPage();
        }
        /// <summary>
        /// Navigating to consume page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConsumeLink_Click(object sender, RoutedEventArgs e)
        {
            targetPage = "/DailyConsume";
            NavigateToTargetPage();
        }
        /// <summary>
        /// Navigating to target page.
        /// </summary>
        /// <param name="targetPage"></param>
        private void NavigateToTargetPage()
        {
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceClient();
            client.IsTestCompleted += new EventHandler<IsTestCompletedEventArgs>(client_IsTestCompleted);
            client.IsTestAsync();
        }

        void client_IsTestCompleted(object sender, IsTestCompletedEventArgs e)
        {
            if (!e.Result)//When system isn't testing.
            {
                if (UserNameTextBlock.Text == Field.Guest)//user does not login.
                {
                    PromptWindow LoginPromptWindow = new PromptWindow(Field.PromptInformation.Information, Field.PleaseLogin);
                    LoginPromptWindow.Show();
                }
                else
                {
                    ContentFrame.Navigate(new Uri(targetPage, UriKind.Relative));
                }
            }
            else
            {
                ContentFrame.Navigate(new Uri(targetPage, UriKind.Relative));
            }
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetSessionCompleted += new EventHandler<FamilyServiceReference.GetSessionCompletedEventArgs>(client_GetSessionCompleted);
            client.GetSessionAsync();
        }
        /// <summary>
        /// Get session. if session is null display guest and login,else display user name and logout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetSessionCompleted(object sender, FamilyServiceReference.GetSessionCompletedEventArgs e)
        {
            object obj = e.Result;
            if (obj == null)//user doesn't login.
            {
                UserNameTextBlock.Text = Field.Guest;
                LogLink.Content = Field.Login;
                if (!ContentFrame.Source.ToString().Equals("/Home"))
                {
                    ContentFrame.Navigate(new Uri("/Home", UriKind.Relative));
                }
            }
            else
            {
                LogLink.Content = Field.Logout;
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.GetUserInfoByIDCompleted += new EventHandler<FamilyServiceReference.GetUserInfoByIDCompletedEventArgs>(client_GetUserInfoByIDCompleted);
                client.GetUserInfoByIDAsync(Int32.Parse(obj.ToString()));
            }
        }

        void client_GetUserInfoByIDCompleted(object sender, FamilyServiceReference.GetUserInfoByIDCompletedEventArgs e)
        {
            UserInfo item=e.Result;
            UserNameTextBlock.Text = item.userName;
        }
        /// <summary>
        /// According to link's content to execute event(login,logout).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LogLink_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton link = sender as HyperlinkButton;
            if (link.Content.Equals(Field.Login))//login
            {
                Login loginWindow = new Login();
                loginWindow.Show();
                loginWindow.Closed += new EventHandler(login_Closed);
            }
            else//logout
            {
                PromptWindow promptWindow = new PromptWindow(Field.PromptInformation.Question, Field.ConfirmLogout);
                promptWindow.Show();
                promptWindow.Closed += new EventHandler(promptWindow_Closed);
            }
        }

        void promptWindow_Closed(object sender, EventArgs e)
        {
            PromptWindow promptWindow = sender as PromptWindow;
            if (promptWindow.DialogResult == true)//Confirm to logout.
            {
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceClient();
                client.SetSessionCompleted+=new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_SetSessionCompleted);
                client.SetSessionAsync(null);
            }
        }

        void client_SetSessionCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetSessionCompleted += new EventHandler<FamilyServiceReference.GetSessionCompletedEventArgs>(client_GetSessionCompleted);
            client.GetSessionAsync();
        }

        void login_Closed(object sender, EventArgs e)
        {
            Login loginWindow = sender as Login;
            if (loginWindow.DialogResult == true)//login success.
            {
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.GetSessionCompleted += new EventHandler<FamilyServiceReference.GetSessionCompletedEventArgs>(client_GetSessionCompleted);
                client.GetSessionAsync();
            }
        }
        
        // 在 Frame 导航之后，请确保选中表示当前页的 HyperlinkButton
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }

        // 如果导航过程中出现错误，则显示错误窗口
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ChildWindow errorWin = new ErrorWindow(e.Uri);
            errorWin.Show();
        }
    }
}
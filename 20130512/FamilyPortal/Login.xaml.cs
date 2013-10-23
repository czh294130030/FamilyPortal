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

namespace FamilyPortal
{
    public partial class Login : ChildWindow
    {
        public Login()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Login_KeyDown);
        }
        /// <summary>
        /// When user key down 'enter' key, system will automatically trigger the click event of login button(OKButton).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter)&&OKButton.IsEnabled)
            {
                OKButton_Click(null, null);
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            OKButton.IsEnabled = false;
            string account = AccountTextBox.Text.Trim();
            string password = PasswordTextBox.Password;
            if (VerifyUserInput(account, password))
            {
                UserLogin(account, password);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        /// <summary>
        /// user login. 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private void UserLogin(string account, string password)
        {
            FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
            client.GetUserInfoByAccountAndPasswordCompleted += new EventHandler<FamilyServiceReference.GetUserInfoByAccountAndPasswordCompletedEventArgs>(client_GetUserInfoByAccountAndPasswordCompleted);
            client.GetUserInfoByAccountAndPasswordAsync(account, password);
        }

        void client_GetUserInfoByAccountAndPasswordCompleted(object sender, FamilyServiceReference.GetUserInfoByAccountAndPasswordCompletedEventArgs e)
        {
            FamilyPortal.FamilyServiceReference.UserInfo item=e.Result;
            if (item!= null)//login success.
            {
                
                FamilyServiceReference.FamilyServiceClient client = new FamilyServiceReference.FamilyServiceClient();
                client.SetSessionCompleted+=new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_SetSessionCompleted);
                client.SetSessionAsync(item.userID);
            }
            else//login failed.
            {
                PromptWindow promptWindowFailed = new PromptWindow(Field.PromptInformation.Information, Field.LoginFailed);
                promptWindowFailed.Show();
                promptWindowFailed.Closed += new EventHandler(promptWindowFailed_Closed);
            }
        }
        void client_SetSessionCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this.DialogResult = true;
        }
        /// <summary>
        /// Verify user's input, account and password is required.
        /// </summary>
        /// <returns></returns>
        private bool VerifyUserInput(string account,string password)
        {
            if (string.IsNullOrEmpty(account))
            {
                PromptWindow promptWindowAccount = new PromptWindow(Field.PromptInformation.Information, Field.InputAccount);
                promptWindowAccount.Show();
                promptWindowAccount.Closed += new EventHandler(promptWindowAccount_Closed);
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                PromptWindow promptWindowPassowrd = new PromptWindow(Field.PromptInformation.Information, Field.InputPassword);
                promptWindowPassowrd.Show();
                promptWindowPassowrd.Closed += new EventHandler(promptWindowPassowrd_Closed);
                return false;
            }
            return true;
        }
        /// <summary>
        /// After prompting account is required, the textbox where account is filled in should get focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void promptWindowAccount_Closed(object sender, EventArgs e)
        {
            AccountTextBox.Focus();
            OKButton.IsEnabled = true;
        }
        /// <summary>
        /// After prompting password is required, the textbox where password is filled in should get focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void promptWindowPassowrd_Closed(object sender, EventArgs e)
        {
            PasswordTextBox.Focus();
            OKButton.IsEnabled = true;
        }
        void promptWindowFailed_Closed(object sender, EventArgs e)
        {
            OKButton.IsEnabled = true;
        }
    }
}


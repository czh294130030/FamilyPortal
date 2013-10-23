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
using System.Windows.Media.Imaging;

namespace FamilyPortal
{
    public partial class PromptWindow : ChildWindow
    {
        public PromptWindow(Field.PromptInformation promptInformation, string message)
        {
            InitializeComponent();
            ShowInformation(promptInformation,message);
        }
        /// <summary>
        /// According to prompt information to show title and message.
        /// </summary>
        /// <param name="promptInformation">title</param>
        /// <param name="message">message</param>
        private void ShowInformation(Field.PromptInformation promptInformation,string message)
        {
            this.Title = promptInformation.ToString();
            MessageTextBlock.Text = message;
            ShowButtons(promptInformation);
            ShowImages(promptInformation);
        }
        /// <summary>
        /// According to prompt information to show buttons(OKButton,CancelButton).
        /// </summary>
        private void ShowButtons(Field.PromptInformation promptInformation)
        {
            switch (promptInformation)
            { 
                case Field.PromptInformation.Information:
                    CancelButton.Visibility = Visibility.Collapsed;
                    OKButton.Margin = new Thickness(0, 12, 0, 0);
                    break;
                case Field.PromptInformation.Question:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///According to prompt information to show images. 
        /// </summary>
        /// <param name="promptInformation"></param>
        private void ShowImages(Field.PromptInformation promptInformation)
        {
            switch (promptInformation)
            { 
                case Field.PromptInformation.Information:
                    Method.ShowImageByUriString(MessageImage, Field.InfoImagePath);
                    break;
                case Field.PromptInformation.Question:
                    Method.ShowImageByUriString(MessageImage, Field.HelpImagePath);
                    break;
                default:
                    break;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


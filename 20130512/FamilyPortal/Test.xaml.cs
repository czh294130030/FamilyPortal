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
using FamilyPortal.UserControls;

namespace FamilyPortal
{
    public partial class Test : Page
    {
        private ConsumeControl consumeControl = null;
        public Test()
        {
            InitializeComponent();
            consumeControl = new ConsumeControl(null, null);
            LayoutRoot.Children.Add(consumeControl);
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

    }
}

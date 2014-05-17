using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
//using System.Threading;
using System.Windows.Threading;

namespace Raido
{
    public partial class PageStart : PhoneApplicationPage
    {
        public PageStart()
        {
            InitializeComponent();
            this.Loaded += PageStart_Loaded;
        }

        void PageStart_Loaded(object sender, RoutedEventArgs e)
        {
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Tick += (a, b) =>
            //    {
            //        NavigationService.
            //    };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Animation;

namespace Raido.UseControl
{
    public partial class StartLogo : UserControl
    {
        public StartLogo()
        {
            InitializeComponent();
            //this.Loaded += StartLogo_Loaded;
            //this.Unloaded += StartLogo_Unloaded;
        }

        void StartLogo_Unloaded(object sender, RoutedEventArgs e)
        {
            SunshineStory.Stop();
        }

        void StartLogo_Loaded(object sender, RoutedEventArgs e)
        {
            SunshineStory.Begin();
        }
       
    }
}

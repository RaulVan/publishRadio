using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Raido.UseControl
{
    public partial class SpectrumLike : UserControl
    {
        public SpectrumLike()
        {
            InitializeComponent();
            this.Loaded += SpectrumLike_Loaded;
        }

        void SpectrumLike_Loaded(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin();
        }

        public void Begin()
        {
            myStoryboard.Begin();
        }

        public void Stop()
        {
            myStoryboard.Stop();
        }
    }
}

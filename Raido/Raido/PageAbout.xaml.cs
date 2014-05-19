using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Raido
{
    public partial class PageAbout : PhoneApplicationPage
    {
        public PageAbout()
        {
            InitializeComponent();
            txtVersion.Text ="v"+ Radiohelper.GetVersion();
        }

        private void btnRate_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();
        }

        private void btnContract_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask ect = new EmailComposeTask();
            ect.To = "raulgblanco@live.com";
            ect.Subject = " 7.11 FM 反馈";
            ect.Show();
        }
    }
}
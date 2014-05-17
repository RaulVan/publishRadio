using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Raido.Resources;

namespace Raido
{
    public partial class MainPage : PhoneApplicationPage
    {

        RadioList test;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            test = new RadioList();
            test.DownloadFinshed += test_DownloadFinshed;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        void test_DownloadFinshed(object sender, EventArgs e)
        {
            List<RadioContent> temp = test.GetList();
            List<RadioContent> listtemp = new List<RadioContent>();

            foreach(RadioContent item in temp)
            {
                if(item.RadioName.IndexOf("新闻") <0 &&
                    item.RadioName.IndexOf("音乐") <0 &&
                    item.RadioName.IndexOf("经济") <0 &&
                    item.RadioName.IndexOf("娱乐") <0 &&
                    item.RadioName.IndexOf("相声") <0 &&
                    item.RadioName.IndexOf("教育") <0 &&
                    item.RadioName.IndexOf("都市") <0 &&
                    item.RadioName.IndexOf("体育") <0 &&
                    item.RadioName.IndexOf("评书") <0 &&
                    item.RadioName.IndexOf("故事") <0 &&
                    item.RadioName.IndexOf("戏曲") <0 &&
                    item.RadioName.IndexOf("交通") <0 
                     )
                {
                    listtemp.Add(item);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            test.BeginDownload("http://shage.me/app/radiolist.txt");
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}
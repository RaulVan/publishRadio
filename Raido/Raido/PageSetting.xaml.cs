using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using Raido.UseControl;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Raido
{
    public partial class PageSetting : PhoneApplicationPage
    {
        public PageSetting()
        {
            InitializeComponent();
            txtAppName.Text = "7.11 FM ";// Radiohelper.GetAppName();
            txtSettingPage.Text = "设置";
            toggsTile.IsChecked = AppConfig.isCheck;
        }

        /// <summary>
        /// 更换Tile背景图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
           

            
        }

        private void btnAddRadio_Click(object sender, RoutedEventArgs e)
        {
           
        }

       
       
        private void toggsTile_Checked(object sender, RoutedEventArgs e)
        {
                Radiohelper helper = new Radiohelper();
                helper.isShellTileTransparent(true);
                AppConfig.isCheck = true;
               
            
        }

        private void toggsTile_Unchecked(object sender, RoutedEventArgs e)
        {
                Radiohelper helper = new Radiohelper();
                helper.isShellTileTransparent(false);
                AppConfig.isCheck = false;

        }
    }
}
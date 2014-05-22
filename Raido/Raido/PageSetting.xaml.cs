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
        }

        /// <summary>
        /// 更换Tile背景图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            Radiohelper helper = new Radiohelper();
            helper.isShellTileTransparent(true);

            
        }

        private void btnAddRadio_Click(object sender, RoutedEventArgs e)
        {
            PopupCotainer pc = new PopupCotainer(this);
            InputUserRadioInfo inpuitControl = new InputUserRadioInfo();
            pc.Show(inpuitControl);
            inpuitControl.OnButtonOKClickChanged += inpuitControl_OnButtonOKClickChanged;
        }

        /// <summary>
        ///添加自己的Radio数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void inpuitControl_OnButtonOKClickChanged(object sender, InputControlEventArgs e)
        {
            //var info = (sender as InputUserRadioInfo);

            try
            {
                Debug.WriteLine(e.radio.RadioName + e.radio.RadioURL + e.radio.Type);

                Radiohelper helper = new Radiohelper();
                ObservableCollection<RadioContent> radioList = helper.ReadXmltoObject<RadioContent>(AppConfig.UserAddListFile);
                //if (radioList!=null)
                //{
                ////TODO:显示自添加的list
                //}
                if (radioList == null)
                {
                    radioList = new ObservableCollection<RadioContent>();
                }
                radioList.Add(e.radio);

                helper.WriteObjecttoXml<RadioContent>(radioList, AppConfig.UserAddListFile);

            }
            catch (Exception ex)
            {
                UmengSDK.UmengAnalytics.TrackException(ex);
            }

        }
    }
}
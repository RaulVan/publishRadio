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
using AudioPlaybackAgent;
using Microsoft.Phone.BackgroundAudio;
using System.Diagnostics;
using System.Windows.Threading;

namespace Raido
{
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        ///更新UI计时器
        /// </summary>
        private DispatcherTimer timerr;

        /// <summary>
        /// 当前播放
        /// </summary>
        public static int gCurrentTrack = 0;

        public Radiohelper helper;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            if (AppConfig.isFirstRun)
            {
 
            }

            helper = new Radiohelper();
            (Application.Current as App).PlayList = helper.GetRadioList();
            
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;

            this.Loaded += MainPage_Loaded;


        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                this.timerr = new DispatcherTimer();
                this.timerr.Interval = TimeSpan.FromSeconds(.05);
                this.timerr.Tick += UpdateState;
            }
            catch (Exception ex)
            {
                //UmengSDK.UmengAnalytics.TrackException(ex);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            gCurrentTrack = AppConfig.isoCurrentTrack;  

            this.UpdateState(null, null);
           

            base.OnNavigatedTo(e);
        }

        private void UpdateState(object sender, EventArgs e)
        {
            try
            {
                AudioTrack audioTrack = BackgroundAudioPlayer.Instance.Track;

                if (audioTrack != null)
                {
                    //txtPlayName.Text = audioTrack.Title;
                    if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
                    {
                        //btnPause.Visibility = Visibility.Visible;
                        //btnPlay.Visibility = Visibility.Collapsed;
                        //TODO:显示当前播放内容
                        btnPlayStatus.TextContent = audioTrack.Title;
                    }
                    else
                    {
                        //btnPause.Visibility = Visibility.Collapsed;
                        //btnPlay.Visibility = Visibility.Visible;
                        //TODO:播放内容清空
                        btnPlayStatus.TextContent = "";
                    }
                }
            }
            catch (Exception)
            {
                timerr.Stop();
            }
        }

        private void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            try
            {
                BackgroundAudioPlayer play = sender as BackgroundAudioPlayer;
                if (play.Error != null)
                {
                    //TODO:处理后台播放错误
                }

                PlayState playState;

                try
                {
                    playState = BackgroundAudioPlayer.Instance.PlayerState;
                }
                catch (InvalidOperationException ex)
                {
                    playState = PlayState.Stopped;
                    //UmengSDK.UmengAnalytics.TrackException(ex);
                }

                switch (playState)
                {
                    case PlayState.Unknown:
                        //BackgroundAudioPlayer.Instance.Stop();
                        //BackgroundAudioPlayer.Instance.Track = _playList[AppConfig.isoCurrentTrack];
                        //BackgroundAudioPlayer.Instance.Play();
                        //BackgroundAudioPlayer.Instance.Stop();
                        break;

                    case PlayState.Paused:
                        this.UpdateState(null, null);
                        this.timerr.Stop();
                        break;

                    case PlayState.Playing:

                        this.UpdateState(null, null);

                        this.timerr.Start();
                        break;

                    case PlayState.Stopped:
                        this.timerr.Stop();
                        this.UpdateState(null, null);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PlayStateChanged:" + ex.ToString());
                //UmengSDK.UmengAnalytics.TrackException(ex);
            }

        }


        private void btnPlayStatus_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PlayPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageButton_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ChooseChannel.xaml?type=all", UriKind.Relative));
        }

        private void ImageButton_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageSetting.xaml", UriKind.Relative));
        }

        private void ImageButton_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageAbout.xaml", UriKind.Relative));
        }

        /// <summary>
        /// 推荐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageButton_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ChooseChannel.xaml?type=sug", UriKind.Relative));
        }

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageButton_Click_5(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ChooseChannel.xaml?type=fav", UriKind.Relative));
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
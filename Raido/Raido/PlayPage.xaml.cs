using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using UmengSocialSDK;
using UmengSocialSDK.Net.Request;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Threading;
using Microsoft.Phone.BackgroundAudio;
using System.Diagnostics;
using Raido.Models;
using System.Collections.ObjectModel;

namespace Raido
{
    public partial class PlayPage : PhoneApplicationPage
    {
        /// <summary>
        ///更新UI计时器
        /// </summary>
        private DispatcherTimer timerr;



        /// <summary>
        /// 当前播放
        /// </summary>
        public static int gCurrentTrack = 0;


        AudioTrack audioTrack;

        public PlayPage()
        {
            InitializeComponent();
            this.UpdateState(null, null);
            BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;
            this.Loaded += PlayPage_Loaded;
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
        private void UpdateState(object sender, EventArgs e)
        {
            try
            {
                audioTrack = BackgroundAudioPlayer.Instance.Track;

                if (audioTrack != null)
                {
                    //txtPlayName.Text = audioTrack.Title;
                    if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
                    {
                        btnPause.Visibility = Visibility.Visible;
                        btnPlay.Visibility = Visibility.Collapsed;
                        //TODO:显示当前播放内容
                        txtPlayName.Text = audioTrack.Title;
                    }
                    else
                    {
                        btnPause.Visibility = Visibility.Collapsed;
                        btnPlay.Visibility = Visibility.Visible;
                        //TODO:播放内容清空
                        txtPlayName.Text = "";
                    }
                }
                else
                {
                    btnPause.Visibility = Visibility.Collapsed;
                    btnPlay.Visibility = Visibility.Visible;
                    //TODO:播放内容清空
                    txtPlayName.Text = "";
                }
            }
            catch (Exception)
            {
                timerr.Stop();
            }
        }


        void PlayPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.timerr = new DispatcherTimer();
                this.timerr.Interval = TimeSpan.FromMilliseconds(5);
                this.timerr.Tick += UpdateState;
            }
            catch (Exception ex)
            {
                //UmengSDK.UmengAnalytics.TrackException(ex);
            }
        }

        private async void btnShare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Radiohelper helper = new Radiohelper();
                BitmapImage bitmapImage = new BitmapImage();
                WriteableBitmap bitmap = await  helper.Screen();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap.SaveJpeg(memoryStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);
                    bitmapImage.SetSource(memoryStream);
                }

                ShareData shareData = new ShareData();
                string content = "";
                if (string.IsNullOrWhiteSpace(txtPlayName.Text))
                {
                    content = "我正在使用Radio Pro收听广播，分享一个好APP，支持CodeMonkey。";
                }
                else
                {
                    content = string.Format("我正在使用Radio Pro收听{0}，分享一个好APP，支持CodeMonkey。", txtPlayName.Text);
                }
                shareData.Content = content;

                shareData.Picture = bitmapImage;

                ShareOption option = new ShareOption();
                option.ShareCompleted = args =>
                {
                    if (args.StatusCode == UmengSocialSDK.UmEventArgs.Status.Successed)
                    {
                        //分享成功
                        // MessageBox.Show("分享成功");
                    }
                    else
                    {
                        //分享失败
                        //MessageBox.Show("分享失败");
                    }
                };

                UmengSocial.Share(AppConfig.AppKey, shareData, null, this, option);
            }
            catch (Exception ex)
            {
                //UmengSDK.UmengAnalytics.TrackException(ex);
            }

        }

        private void btnFav_Click(object sender, RoutedEventArgs e)
        {
            Radiohelper helper = new Radiohelper();

            ObservableCollection<RadioFavList> favList = helper.ReadXmltoObject();
            if (favList==null)
            {
                favList = new ObservableCollection<RadioFavList>();
            }
            if (audioTrack!=null)
            {
                favList.Add(new RadioFavList() { RadioName = audioTrack.Title, RadioURL = audioTrack.Source.ToString(), Type = audioTrack.Artist, IsFav=true });
                helper.WriteObjecttoXml(favList);
            }
           
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ////BackgroundAudioPlayer.Instance.Track = null;// new AudioTrack();
                BackgroundAudioPlayer.Instance.Track = (Application.Current as App).PlayList[AppConfig.isoCurrentTrack];
                BackgroundAudioPlayer.Instance.Volume = 1.0d;
                ////BackgroundAudioPlayer.Instance.Play();

                if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
                {
                    BackgroundAudioPlayer.Instance.Pause();
                }
                else if (PlayState.Unknown == BackgroundAudioPlayer.Instance.PlayerState)
                {
                    //   BackgroundAudioPlayer.Instance.Stop();

                    BackgroundAudioPlayer.Instance.Track = (Application.Current as App).PlayList[AppConfig.isoCurrentTrack];
                    BackgroundAudioPlayer.Instance.Play();
                }
                else
                {
                    BackgroundAudioPlayer.Instance.Track = (Application.Current as App).PlayList[AppConfig.isoCurrentTrack];
                }

                Debug.WriteLine("Play_Click Play:" + AppConfig.isoCurrentTrack);
            }
            catch (Exception ex)
            {
                //UmengSDK.UmengAnalytics.TrackException(ex);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //BackgroundAudioPlayer.Instance.SkipNext();
                if (++AppConfig.isoCurrentTrack >= (Application.Current as App).PlayList.Count)
                {
                    AppConfig.isoCurrentTrack = 0;
                }
                BackgroundAudioPlayer.Instance.Track = (Application.Current as App).PlayList[AppConfig.isoCurrentTrack];
                //BackgroundAudioPlayer.Instance.Play();
                this.UpdateState(null, null);

                Debug.WriteLine("Next_Click Play:" + AppConfig.isoCurrentTrack);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("btnNext_Click" + ex.ToString());
                //UmengSDK.UmengAnalytics.TrackException(ex);
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                BackgroundAudioPlayer.Instance.Pause();
                Debug.WriteLine("Pause_Click Play:" + AppConfig.isoCurrentTrack);
            }
            catch (Exception ex)
            {
                //UmengSDK.UmengAnalytics.TrackException(ex);
            }
        }
    }
}
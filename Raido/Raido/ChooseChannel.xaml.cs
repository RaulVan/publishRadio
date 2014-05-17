using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using Raido.Models;
using System.Diagnostics;
using Microsoft.Phone.BackgroundAudio;

namespace Raido
{
    public partial class ChooseChannel : PhoneApplicationPage
    {
        /// <summary>
        ///更新UI计时器
        /// </summary>
        private DispatcherTimer timerr;



        /// <summary>
        /// 当前播放
        /// </summary>
        public static int gCurrentTrack = 0;

        
        public ChooseChannel()
        {
            InitializeComponent();
            this.UpdateState(null, null);
            BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;

            longlistAll.SelectionChanged += longlistAll_SelectionChanged;
            this.Loaded += ChooseChannel_Loaded;
        }

        void ChooseChannel_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.timerr = new DispatcherTimer();
            this.timerr.Interval = TimeSpan.FromSeconds(.05);
            this.timerr.Tick += UpdateState;
            var viewModel = new RadioListViewModel();
            longlistAll.ItemsSource = viewModel.GroupedRadios;
            
        }

        private void UpdateState(object sender, EventArgs e)
        {
            try
            {
                AudioTrack audioTrack = BackgroundAudioPlayer.Instance.Track;

                if (audioTrack != null)
                {
                    if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
                    {
                        txtPlayName.Text = audioTrack.Title;
                        btnPause.Visibility = Visibility.Visible;
                        btnPlay.Visibility = Visibility.Collapsed;
                        //TODO:显示当前播放内容
                    }
                    else
                    {
                        btnPause.Visibility = Visibility.Collapsed;
                        btnPlay.Visibility = Visibility.Visible;
                        //TODO:播放内容清空
                        txtPlayName.Text = "停止";
                    }

                }
                else
                {
                    btnPause.Visibility = Visibility.Collapsed;
                    btnPlay.Visibility = Visibility.Visible;
                    //TODO:播放内容清空
                    txtPlayName.Text = "停止";
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

        void longlistAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectenItem = (RadioContent)longlistAll.SelectedItem;
                longlistAll.ScrollTo(selectenItem);

                AudioTrack selectAudioTrack = new AudioTrack(new Uri(selectenItem.RadioURL, UriKind.Absolute), selectenItem.RadioName, selectenItem.Type, "", null, "", EnabledPlayerControls.Pause);
                //TODO：查找当前Select项在_PlayList中的index，然后给 isoCurrentTrack

                foreach (var item in (Application.Current as App).PlayList)
                {
                    if (item.Source == selectAudioTrack.Source)
                    {
                        selectAudioTrack = item;
                    }
                }

                int index = (Application.Current as App).PlayList.IndexOf(selectAudioTrack, 0);

                AppConfig.isoCurrentTrack = index;

                ////TODO:播放当前选择项

                try
                {
                    ////BackgroundAudioPlayer.Instance.Track = null;// new AudioTrack();
                    BackgroundAudioPlayer.Instance.Track = (Application.Current as App).PlayList[index];
                    BackgroundAudioPlayer.Instance.Volume = 1.0d;
                    ////BackgroundAudioPlayer.Instance.Play();

                    if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
                    {
                        BackgroundAudioPlayer.Instance.Pause();
                    }
                    else if (PlayState.Unknown == BackgroundAudioPlayer.Instance.PlayerState)
                    {
                        //   BackgroundAudioPlayer.Instance.Stop();

                        BackgroundAudioPlayer.Instance.Track = (Application.Current as App).PlayList[index];
                        BackgroundAudioPlayer.Instance.Play();
                    }
                    else
                    {
                        BackgroundAudioPlayer.Instance.Track = (Application.Current as App).PlayList[index];
                    }

                    Debug.WriteLine("Play_Click Play:" + index);
                }
                catch (Exception ex)
                {
                    //UmengSDK.UmengAnalytics.TrackException(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("listRadioList_SelectionChanged" + ex.ToString());
                //UmengSDK.UmengAnalytics.TrackException(ex);
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
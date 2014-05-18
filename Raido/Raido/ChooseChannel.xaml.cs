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
using System.Collections.ObjectModel;
using Raido.Service;
using Coding4Fun.Toolkit.Controls;

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

        private NaviTypeEnum naviType;

        protected ObservableCollection<RadioFavList> favList = new ObservableCollection<RadioFavList>();

        public ChooseChannel()
        {
            InitializeComponent();
            this.UpdateState(null, null);
            longlistAll.Visibility = Visibility.Collapsed;
            longlistFav.Visibility = Visibility.Collapsed;
            longlistSug.Visibility = Visibility.Collapsed;

            BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;

            this.Loaded += ChooseChannel_Loaded;

            longlistAll.SelectionChanged += longlistAll_SelectionChanged;
            longlistFav.SelectionChanged += longlistFav_SelectionChanged;
            longlistSug.SelectionChanged += longlistSug_SelectionChanged;


        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string type = NavigationContext.QueryString["type"];
            Debug.WriteLine("============this from " + type);
            if (type == "fav")
            {
                naviType = NaviTypeEnum.Fav;
                txtListName.Text = "收藏列表";
                //TODO:加载收藏
                longlistFav.Visibility = Visibility.Visible;
                Radiohelper helper = new Radiohelper();
                favList = helper.ReadXmltoObject();
                if (favList != null)
                {

                    longlistFav.ItemsSource = favList;
                }
            }
            else if (type == "all")
            {
                naviType = NaviTypeEnum.All;
                txtListName.Text = "选择频道";
                longlistAll.Visibility = Visibility.Visible;
                var viewModel = new RadioListViewModel();
                longlistAll.ItemsSource = viewModel.GroupedRadios;
            }
            else if (type == "sug")
            {
                naviType = NaviTypeEnum.Sug;
                txtListName.Text = "推荐频道";
                //TODO:加载推荐
                longlistSug.Visibility = Visibility.Visible;
                longlistSug.ItemsSource = DataService.GetSuggestRadios();
            }
            base.OnNavigatedTo(e);
        }
        void ChooseChannel_Loaded(object sender, RoutedEventArgs e)
        {

            this.timerr = new DispatcherTimer();
            this.timerr.Interval = TimeSpan.FromSeconds(.05);
            this.timerr.Tick += UpdateState;


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
                        gridPlay.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnPause.Visibility = Visibility.Collapsed;
                        btnPlay.Visibility = Visibility.Visible;
                        //TODO:播放内容清空
                        txtPlayName.Text = "";
                        gridPlay.Visibility = Visibility.Collapsed;
                    }

                }
                else
                {
                    btnPause.Visibility = Visibility.Collapsed;
                    btnPlay.Visibility = Visibility.Visible;
                    //TODO:播放内容清空
                    txtPlayName.Text = "";
                    gridPlay.Visibility = Visibility.Collapsed;
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

        #region Long List SelectionChanged
        void longlistAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectenItem = (RadioContent)longlistAll.SelectedItem;
                //longlistAll.ScrollTo(selectenItem);

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
        void longlistSug_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectenItem = (RadioContent)longlistSug.SelectedItem;
                //longlistAll.ScrollTo(selectenItem);

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

        void longlistFav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectenItem = (RadioFavList)longlistFav.SelectedItem;
                //longlistAll.ScrollTo(selectenItem);

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

        #endregion

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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (favList.Count > 0)
            {
                RadioFavList fav = (sender as Microsoft.Phone.Controls.MenuItem).CommandParameter as RadioFavList;
                favList.Remove(fav);
                longlistFav.ItemsSource.Remove(fav);
                Radiohelper helper = new Radiohelper();
                helper.WriteObjecttoXml(favList);

                ToastPrompt _prompt = new ToastPrompt();
                _prompt.Title = "Radio Pro";
                _prompt.Message = "成功删除";
                _prompt.TextWrapping = TextWrapping.NoWrap;
                _prompt.Show();
            }
        }

    }
}
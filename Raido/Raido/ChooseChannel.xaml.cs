﻿using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Raido.Models;
using Raido.Service;
using Raido.UseControl;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

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

       //private  PopupCotainer pc ;
       // private  InputUserRadioInfo inputControl = new InputUserRadioInfo();

        protected ObservableCollection<RadioFavList> favList = new ObservableCollection<RadioFavList>();

        public ChooseChannel()
        {
            InitializeComponent();
            this.UpdateState(null, null);
            longlistAll.Visibility = Visibility.Collapsed;
            longlistFav.Visibility = Visibility.Collapsed;
            longlistSug.Visibility = Visibility.Collapsed;

            this.Loaded += ChooseChannel_Loaded;
            BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;

            longlistAll.SelectionChanged += longlistAll_SelectionChanged;
            longlistFav.SelectionChanged += longlistFav_SelectionChanged;
            longlistSug.SelectionChanged += longlistSug_SelectionChanged;
           // this.LostFocus += ChooseChannel_LostFocus;
        }

        void ChooseChannel_LostFocus(object sender, RoutedEventArgs e)
        {
            ApplicationBar.IsVisible = false;
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
                favList = helper.ReadXmltoObject<RadioFavList>(AppConfig.FavListFile);
                if (favList != null)
                {
                    longlistFav.ItemsSource = favList;
                }
                ApplicationBar.IsVisible = false;
            }
            else if (type == "all")
            {
                naviType = NaviTypeEnum.All;
                txtListName.Text = "选择频道";
                longlistAll.Visibility = Visibility.Visible;
                var viewModel = new RadioListViewModel();
                longlistAll.ItemsSource = viewModel.GroupedRadios;

                //ApplicationBar.BackgroundColor=Color

                //AppBarPrompt _appBar = new AppBarPrompt();
                
                //_appBar.Show();

                //BulidApplicationBar();
                
               //pc = new PopupCotainer(this);
              

            }
            else if (type == "sug")
            {
                naviType = NaviTypeEnum.Sug;
                txtListName.Text = "推荐频道";
                //TODO:加载推荐
                longlistSug.Visibility = Visibility.Visible;
                longlistSug.ItemsSource = DataService.GetSuggestRadios();
                ApplicationBar.IsVisible = false;
            }
            base.OnNavigatedTo(e);
        }

        private void ChooseChannel_Loaded(object sender, RoutedEventArgs e)
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
                    UmengSDK.UmengAnalytics.TrackException(ex);
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
                UmengSDK.UmengAnalytics.TrackException(ex);
            }
        }

        #region Long List SelectionChanged

        private void longlistAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void longlistSug_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void longlistFav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectenItem = (RadioFavList)longlistFav.SelectedItem;
                //longlistAll.ScrollTo(selectenItem);

                AudioTrack selectAudioTrack = new AudioTrack(new Uri(selectenItem.RadioURL, UriKind.Absolute), selectenItem.RadioName, selectenItem.Type, "", null, "", EnabledPlayerControls.Pause);
                // BackgroundAudioPlayer.Instance.Track
                
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

        #endregion Long List SelectionChanged

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
                UmengSDK.UmengAnalytics.TrackException(ex);
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
                UmengSDK.UmengAnalytics.TrackException(ex);
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
                UmengSDK.UmengAnalytics.TrackException(ex);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (favList.Count > 0)
            {
                ToastPrompt _prompt = new ToastPrompt();
                try
                {
                    RadioFavList fav = (sender as Microsoft.Phone.Controls.MenuItem).CommandParameter as RadioFavList;
                    favList.Remove(fav);
                    longlistFav.ItemsSource.Remove(fav);
                    Radiohelper helper = new Radiohelper();
                    helper.WriteObjecttoXml<RadioFavList>(favList, AppConfig.FavListFile);
                    _prompt.Message = AppConfig.MsgFavDelSuccess;//删除成功
                }
                catch (Exception ex)
                {
                    _prompt.Message = AppConfig.MsgFavDelFailed;//删除失败
                }
                _prompt.Title = AppConfig.ToastTitle;
                _prompt.TextWrapping = TextWrapping.NoWrap;
                _prompt.Show();
            }
        }


       

        private void BulidApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Aessets/AppBar/add.png", UriKind.Relative));
            appBarButton.Text = "添加";
            ApplicationBar.Buttons.Add(appBarButton);

            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.Opacity = 0.2;

            appBarButton.Click += btnAddRadio_Click;
        }

        private void btnAddRadio_Click(object sender, EventArgs e)
        {
           
            
        }

        
        void inputPormpt_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            
        }

        private void btnAddChannel_Click(object sender, EventArgs e)
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
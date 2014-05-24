using Microsoft.Phone.BackgroundAudio;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Net;
using Microsoft.Phone.Net.NetworkInformation;
using System.Windows;

namespace Raido
{
    public class AppConfig
    {
        /// <summary>
        /// 远程数据地址
        /// </summary>
        public static string DatasURL = "http://shage.me/app/radiolist.txt";
        public static string SuggestURL = "http://shage.me/app/suggest.txt";
        public static string filename = "radiolist.txt";
        public static string FavListFile = "fav.xml";
        public static string UserAddListFile = "user.xml";
        public static string ToastTitle = "7.11 FM";
        public static string MsgFavDelSuccess = "删除成功";
        public static string MsgFavDelFailed = "删除失败";
        public static string MsgFavHas = "该频道已在收藏列表";
        public static string MsgFavAddSuccess = "成功添加至收藏列表";

        /// <summary>
        /// 友盟统计API Key
        /// </summary>
        public static string AppKey = "530af5be56240b7e95046e75";

        /// <summary>
        /// DEBUG是使用key
        /// </summary>
        public static string DebugAppKey = "5313e59056240b7a8a1ab50e";
        /// <summary>
        /// 当前播放曲目
        /// </summary>
        public static int isoCurrentTrack
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("isoCurrentTrack") ? (int)IsolatedStorageSettings.ApplicationSettings["isoCurrentTrack"] : 0;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["isoCurrentTrack"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        public static bool isFirstRun
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("isFirstRun") ? (bool)IsolatedStorageSettings.ApplicationSettings["isFirstRun"] : false;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["isFirstRun"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        public static List<AudioTrack> isoPlayTrack
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("isoPlayTrack") ? (List<AudioTrack>)IsolatedStorageSettings.ApplicationSettings["isoPlayTrack"] : null;

            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["isoPlayTrack"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }


        public string GetNetwork()
        {
            return DeviceNetworkInformation.CellularMobileOperator.ToString();
            //var info = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType;
            //switch (info)
            //{
            //    case NetworkInterfaceType.Wireless80211:
            //        return "WiFi";
            //    default:
            //        return "Other";
            //}
        }

        public static int isoCurrentFMFrequency1
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("isoCurrentFMFrequency1") ? (int)IsolatedStorageSettings.ApplicationSettings["isoCurrentFMFrequency1"] : 101;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["isoCurrentFMFrequency1"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        public static int isoCurrentFMFrequency2
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("isoCurrentFMFrequency2") ? (int)IsolatedStorageSettings.ApplicationSettings["isoCurrentFMFrequency2"] : 1;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["isoCurrentFMFrequency2"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        public int GetSecleFactor()
        {
            int height = 0;
            switch (Application.Current.Host.Content.ScaleFactor)
            {
                case 100:
                case 150:
                    height = 854;
                    break;
                case 160:
                    height = 800;
                    break;

            }
            return height;
        }


        public static bool isCheck
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("isCheck") ? (bool)IsolatedStorageSettings.ApplicationSettings["isCheck"] : false;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["isCheck"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

    }
}

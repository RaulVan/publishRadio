using Microsoft.Phone.BackgroundAudio;
using Raido.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using Raido.Models;
using System.Windows.Resources;
using System.Net;
using System.Diagnostics;
using Microsoft.Phone.Shell;

namespace Raido
{
    public class Radiohelper
    {
        public List<AudioTrack> GetRadioList()
        {
            List<AudioTrack> audioList = new List<AudioTrack>();
            List<RadioContent> radios = DataService.GetRadios();
            foreach (var item in radios)
            {
                audioList.Add(new AudioTrack(new Uri(item.RadioURL, UriKind.Absolute), item.RadioName, item.Type, "", null, "", EnabledPlayerControls.Pause));
            }
            return audioList;
        }

        public List<AudioTrack> GetRadioFavList()
        {
            List<AudioTrack> audioList = new List<AudioTrack>();
            if (ReadXmltoObject<RadioFavList>(AppConfig.FavListFile) == null)
            {
                return null;
            }
            List<RadioFavList> radios = ReadXmltoObject<RadioFavList>(AppConfig.FavListFile).ToList();
            foreach (var item in radios)
            {
                audioList.Add(new AudioTrack(new Uri(item.RadioURL, UriKind.Absolute), item.RadioName, item.Type, "", null, "", EnabledPlayerControls.Pause));
            }
            return audioList;
        }

        public List<AudioTrack> GetRadioSugList()
        {
            List<AudioTrack> audioList = new List<AudioTrack>();
            List<RadioContent> radios = DataService.GetSuggestRadios();
            foreach (var item in radios)
            {
                audioList.Add(new AudioTrack(new Uri(item.RadioURL, UriKind.Absolute), item.RadioName, item.Type, "", null, "", EnabledPlayerControls.Pause));
            }
            return audioList;
        }
        public async Task<WriteableBitmap> Screen()
        {
            //截图
            await Task.Delay(200);

            using (System.IO.Stream stream1 = Application.GetResourceStream(new Uri(@"Assets/qcode.png", UriKind.Relative)).Stream)
            {
                double width = Application.Current.Host.Content.ActualWidth;
                double heigth = Application.Current.Host.Content.ActualHeight;
                WriteableBitmap wbmp = new WriteableBitmap((int)width, (int)heigth);
                wbmp.Render(App.Current.RootVisual, null);
                wbmp.Invalidate();


                WriteableBitmap wideBitmap = new WriteableBitmap((int)width, (int)(heigth + 280));

                BitmapImage image1 = new BitmapImage();
                image1.SetSource(stream1);
                stream1.Close();
                //stream1.Flush();

                wideBitmap.Blit(new Rect(0, 0, width, heigth), new WriteableBitmap(wbmp), new Rect(0, 0, wbmp.PixelWidth, wbmp.PixelHeight), WriteableBitmapExtensions.BlendMode.Additive);
                wideBitmap.Blit(new Rect((width - 280) / 2, heigth, 280, 280), new WriteableBitmap(image1), new Rect(0, 0, image1.PixelWidth, image1.PixelHeight), WriteableBitmapExtensions.BlendMode.Additive);
                image1 = null;
                return wideBitmap;
            }
        }

        public async void DownloadDatas()
        {
            //TODO: 如果网络正常，下载feed.xml文件保存至iso
            //读取iso中的xml
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(AppConfig.DatasURL);
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;


            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");
            HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);


            // var buffer = await new HttpClient().GetByteArrayAsync(new Uri(url));
            var strings = await response.Content.ReadAsStringAsync();


            SaveFile(AppConfig.filename, strings);
        }


        public async void SaveFile(string filename, string xml)
        {
            try
            {
                Windows.Storage.StorageFolder localfolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile localfile = await localfolder.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.OpenIfExists);
                Stream s = await localfile.OpenStreamForWriteAsync();
                using (StreamWriter sw = new StreamWriter(s))
                {
                    await sw.WriteAsync(xml);
                }
            }
            catch (Exception ex)
            {

            }
        }

        ///// <summary>
        ///// // Write to the Isolated Storage
        ///// </summary>
        ///// <param name="favRadioList"></param>
        //public void WriteObjecttoXml(ObservableCollection<RadioFavList> favRadioList)
        //{

        //    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
        //    xmlWriterSettings.Indent = true;

        //    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("fav.xml", FileMode.Create))
        //        {
        //            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<RadioFavList>));
        //            using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
        //            {
        //                serializer.Serialize(xmlWriter, favRadioList);
        //            }
        //        }
        //    }

        //}

        //public ObservableCollection<RadioFavList> ReadXmltoObject(string filename)
        //{
        //    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(filename, FileMode.OpenOrCreate))
        //        {
        //            if (stream.Length != 0)
        //            {

        //                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<RadioFavList>));
        //                ObservableCollection<RadioFavList> data = (ObservableCollection<RadioFavList>)serializer.Deserialize(stream);
        //                return data;

        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }

        //}

        /// <summary>
        /// Write to the Isolated Storage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="filename">文件名xml</param>
        public void WriteObjecttoXml<T>(ObservableCollection<T> list, string filename)
        {

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(filename, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<T>));
                    using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                    {
                        serializer.Serialize(xmlWriter, list);
                    }
                }
            }

        }
        /// <summary>
        /// Read from the Isolated Storage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">文件名xml</param>
        /// <returns></returns>
        public ObservableCollection<T> ReadXmltoObject<T>(string filename)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(filename, FileMode.OpenOrCreate))
                {
                    if (stream.Length != 0)
                    {

                        XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<T>));
                        ObservableCollection<T> data = (ObservableCollection<T>)serializer.Deserialize(stream);
                        return data;

                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }

        /// <summary>
        /// 将文件从 Package 复制到 IsolatedStorage 的根目录下
        /// </summary>
        /// <param name="sourceUri"></param>
        /// <param name="targetFileName"></param>
        private void CopyFileToIsolatedStorage(Uri sourceUri, string targetFileName)
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            using (Stream input = Application.GetResourceStream(sourceUri).Stream)
            {
                using (IsolatedStorageFileStream output = isf.CreateFile(targetFileName))
                {
                    byte[] readBuffer = new byte[4096];
                    int bytesRead = -1;

                    while ((bytesRead = input.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    {
                        output.Write(readBuffer, 0, bytesRead);
                    }
                }
            }
        }

        static string Version = null;
        internal static string GetVersion()
        {
            try
            {
                if (Version != null)
                {
                    return Version;
                }
                //Package package = Package.Current;
                //PackageId packageId = package.Id;
                //PKG = packageId.Name;
                //PKG = Assembly.GetExecutingAssembly().;
                //  PKG = AppL;


                var manifestUri = new Uri("WMAppManifest.xml", UriKind.Relative);
                StreamResourceInfo manifestStream = Application.GetResourceStream(manifestUri);
                // string wmapp= manifestStream.ToString();
                if (manifestStream != null)
                {
                    using (var sr = new StreamReader(manifestStream.Stream))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (line.IndexOf("<App xmlns") != -1)
                            {
                                int st;
                                if ((st = line.IndexOf("Version=\"")) != -1)
                                {
                                    line = line.Substring(st + 9);
                                    st = line.IndexOf("\"");
                                    line = line.Substring(0, st);
                                    Version = line;
                                    break;
                                }
                            }
                        }
                    }
                }
                //Debug.WriteLine(wmapp);
                //PKG = wmapp;]


                Version = HttpUtility.UrlEncode(Version);
                return Version;
            }
            catch (Exception e)
            {
                Version = "";
                Debug.WriteLine(e.Message);
                return "";
            }
        }

        /// <summary>
        /// 设置Tile透明
        /// </summary>
        public  void isShellTileTransparent(bool flag)
        {
            ShellTile applictionTile = ShellTile.ActiveTiles.FirstOrDefault();
            FlipTileData flipTitleData = new FlipTileData();
            if (flag)
            {
                flipTitleData = new FlipTileData()
                {
                    Title = "7.11 FM",
                    Count = 0,
                    SmallBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative),
                    BackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative),
                    WideBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative),

                };


            }
            else
            {
                flipTitleData = new FlipTileData()
                {
                    Title = "7.11 FM",
                    Count = 0,
                    SmallBackgroundImage = new Uri("/Assets/Tiles/tile_09.png", UriKind.Relative),
                    BackgroundImage = new Uri("/Assets/Tiles/tile_300.png", UriKind.Relative),
                    WideBackgroundImage = new Uri("/Assets/Tiles/tile_159.png", UriKind.Relative),

                };
            }
                applictionTile.Update(flipTitleData);
        }

    }
}

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

        /// <summary>
        /// // Write to the Isolated Storage
        /// </summary>
        /// <param name="favRadioList"></param>
        public void WriteObjecttoXml(ObservableCollection<RadioFavList> favRadioList)
        {

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("fav.xml", FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<RadioFavList>));
                    using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                    {
                        serializer.Serialize(xmlWriter, favRadioList);
                    }
                }
            }

        }

        public ObservableCollection<RadioFavList> ReadXmltoObject()
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("fav.xml", FileMode.OpenOrCreate))
                {
                    if (stream.Length!=0)
                    {

                        XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<RadioFavList>));
                        ObservableCollection<RadioFavList> data = (ObservableCollection<RadioFavList>)serializer.Deserialize(stream);
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

    }
}

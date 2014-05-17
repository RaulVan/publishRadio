using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Windows.Resources;


namespace Raido
{
    public class RadioContent
    {
        public string RadioName { get; set; }
        public string RadioURL { get; set; }

        public string type { get; set; }
    }

    public class RadioList
    {
        internal List<RadioContent> RList;
        public event EventHandler DownloadFinshed;

        public RadioList()
        {
            IsDownloadCompleted = false;
            RList = new List<RadioContent>();
        }



        public void BeginDownload(string uri)
        {
            AnalysisInfo(uri);
        }

        async Task<byte[]> GetInfoFromHttp(string uri)
        {
            HttpClient httpclient = new HttpClient();
            httpclient.BaseAddress = new Uri(uri);
            HttpRequestMessage message = new HttpRequestMessage();

            message.Method = HttpMethod.Get;

            HttpResponseMessage response = await httpclient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);

            return await response.Content.ReadAsByteArrayAsync();
        }

        

        async void AnalysisInfo(string uri)
        {
            var info = await GetInfoFromHttp(uri);

            StreamReader reader =  new StreamReader(new MemoryStream(info));
            string line = reader.ReadLine();

            while (!string.IsNullOrWhiteSpace(line))
            {
                var ary = line.Split('=');
                RadioContent content = new RadioContent() { RadioName = ary[0], RadioURL = ary[1] };

                RList.Add(content);
                //groups[RadiosInfo.GetNameFirstPinyinKey(radioInfo)].Add(radioInfo);
                line = reader.ReadLine();

            }
            IsDownloadCompleted = true;

            DownloadFinshed(null, null);
        }

        public List<RadioContent> GetList()
        {
            return RList;
        }

        
       // public List<RadioContent> UnableList { get; set; }

        public bool IsDownloadCompleted { get; set; }
    }
}

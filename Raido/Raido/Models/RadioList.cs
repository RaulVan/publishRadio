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

        public string Type { get; set; } 
    }

    //public class RadioList
    //{
    //    internal List<RadioContent> RList;
    //    string[] radiotype = {"新闻" ,"音乐","经济","娱乐","相声","教育","都市","体育","评书","故事","戏曲","交通"};

    //    public RadioList(string uri)
    //    {
    //        IsDownloadCompleted = false;
    //        RList = new List<RadioContent>();
    //        AnalysisInfo(uri);
    //    }


    //    async Task<byte[]> GetInfoFromHttp(string uri)
    //    {
    //        HttpClient httpclient = new HttpClient();
    //        httpclient.BaseAddress = new Uri(uri);
    //        HttpRequestMessage message = new HttpRequestMessage();

    //        message.Method = HttpMethod.Get;

    //        HttpResponseMessage response = await httpclient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);

    //        return await response.Content.ReadAsByteArrayAsync();
    //    }

    //    string GetRadioType(string name)
    //    {
    //        foreach(string item in radiotype)
    //        {
    //            if (name.IndexOf(item) >= 0)
    //                return item;
    //        }

    //        return "其他";
    //    }

    //    async void AnalysisInfo(string uri)
    //    {
    //        var info = await GetInfoFromHttp(uri);

    //        StreamReader reader =  new StreamReader(new MemoryStream(info));
    //        string line = reader.ReadLine();

    //        while (!string.IsNullOrWhiteSpace(line))
    //        {
    //            var ary = line.Split('=');
    //            RadioContent content = new RadioContent() { RadioName = ary[0], RadioURL = ary[1],type = GetRadioType(ary[0]) };

    //            RList.Add(content);
    //            //groups[RadiosInfo.GetNameFirstPinyinKey(radioInfo)].Add(radioInfo);
    //            line = reader.ReadLine();

    //        }
    //        IsDownloadCompleted = true;
    //    }

    //    public List<RadioContent> GetList()
    //    {
    //        return RList;
    //    }

        
    //   // public List<RadioContent> UnableList { get; set; }

    //    public bool IsDownloadCompleted { get; set; }
    //}

}

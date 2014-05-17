using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Resources;

namespace Raido.Service
{
   public static class DataService
    {
       
       public static List<RadioContent> GetRadios()
       {
           List<RadioContent> _radios = new List<RadioContent>();
           StreamResourceInfo resource = App.GetResourceStream(new Uri("Resources/RadioListData.txt", UriKind.Relative));
           StreamReader sr = new StreamReader(resource.Stream);
          string line = sr.ReadLine();
           while (!string.IsNullOrWhiteSpace(line))
           {
               var ary = line.Split('=');
               var radioInfo = new RadioContent {  RadioName = ary[0],  RadioURL = ary[1] , Type=ary[2]};
               _radios.Add(radioInfo);
               line = sr.ReadLine();
           }
           return _radios;
       }

       public static List<RadioContent> GetSuggestRadios()
       {
           List<RadioContent> _radios = new List<RadioContent>();
           StreamResourceInfo resource = App.GetResourceStream(new Uri("Resources/suggest.txt", UriKind.Relative));
           StreamReader sr = new StreamReader(resource.Stream);
           string line = sr.ReadLine();
           while (!string.IsNullOrWhiteSpace(line))
           {
               var ary = line.Split('=');
               var radioInfo = new RadioContent { RadioName = ary[0], RadioURL = ary[1], Type = ary[2] };
               _radios.Add(radioInfo);
               line = sr.ReadLine();
           }
           return _radios;
       }

     
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raido.Heplers;
using Raido.Service;

namespace Raido.Models
{
  public  class RadioListViewModel
    {
      public List<KeyedList<string, RadioContent>> GroupedRadios
      {
          get
          {
              var radios = DataService.GetRadios();

              var groupedRadios = from radio in radios
                                  orderby radio.Type
                                  group radio by radio.Type into radioType
                                  select new KeyedList<string, RadioContent>(radioType);
              return new List<KeyedList<string, RadioContent>>(groupedRadios);
          }
      }


    }
}

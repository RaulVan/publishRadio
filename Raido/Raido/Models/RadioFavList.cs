using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raido.Models
{
   public class RadioFavList
    {
        public string RadioName { get; set; }
        public string RadioURL { get; set; }

        public string Type { get; set; }
        public bool IsFav { get; set; }
    }
}

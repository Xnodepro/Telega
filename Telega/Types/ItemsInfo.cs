using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telega
{
    class ItemsInfo
    {
        private string name="";
        private string site = "";
        //   private string telegram = "";
        public List<string> telegram = new List<string>();
        public string Name { get => name; set => name = value; }
        public string Site { get => site; set => site = value; }
      //  public string Telegram { get => telegram; set => telegram = value; }
        
    }
}

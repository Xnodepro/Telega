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
        private string price = "";
        private string floaat = "";
        private string sticker = "";
        //   private string telegram = "";
        public List<string> telegram = new List<string>();
        public string Name { get => name; set => name = value; }
        public string Site { get => site; set => site = value; }
        public string Price { get => price; set => price = value; }
        public string Floaat { get => floaat; set => floaat = value; }
        public string Sticker { get => sticker; set => sticker = value; }
        //  public string Telegram { get => telegram; set => telegram = value; }

    }
}

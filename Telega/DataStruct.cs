using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telega
{
    static class DataStruct
    {
        static public List<ItemsInfo> ItemsSearch = new List<ItemsInfo>();
        static public Queue<ItemsInfo> GoodItems = new Queue<ItemsInfo>();
        static public Queue<string> ProxyList = new Queue<string>();
        static public List<string> ProxyListFix = new List<string>();

        public struct DataBots
        {
         public  string IdBot { get; set; }
         public string Site { get; set; }
        }
      

    }
}

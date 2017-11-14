using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telega
{
    class UserTelegram
    {
        private string name;
        private string action;
        private string sitename;
        private long idChat;
        public string Name { get => name; set => name = value; }
        public string Action { get => action; set => action = value; }
        public string SiteName { get => sitename; set => sitename = value; }
        public long IdChat { get => idChat; set => idChat = value; }
    }
}

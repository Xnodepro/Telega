using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telega.Types
{
    class LogInfo
    {
        private int id = 0;
        private string site = "";
        private string time = "";
        private string text = "";
        private string timeerror = "";
        private string error = "";

        public int Id { get => id; set => id = value; }
        public string Site { get => site; set => site = value; }
        public string Time { get => time; set => time = value; }
        public string Text { get => text; set => text = value; }
        public string Timeerror { get => timeerror; set => timeerror = value; }
        public string Error { get => error; set => error = value; }
    }
}

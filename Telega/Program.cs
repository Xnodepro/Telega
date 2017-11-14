using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Telega
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        static public Queue<string> Mess = new Queue<string>();
        static public Queue<Types.LogInfo> MessLog = new Queue<Types.LogInfo>();
        public static string CapchaUrl = "";
    }
}

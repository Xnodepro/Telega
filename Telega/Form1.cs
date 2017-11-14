using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Telega
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new System.Threading.Thread(delegate ()
            {
                Telegram.Start();
            }).Start();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DataStruct.ProxyList.Clear();
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            string[] proxy = System.IO.File.ReadAllLines(filename);
            foreach (var item in proxy)
            {
                DataStruct.ProxyListFix.Add(item);
                DataStruct.ProxyList.Enqueue(item);
            }
            MessageBox.Show("подгрузил прокси в количестве:" + DataStruct.ProxyListFix.Count);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Program.Mess.Count > 0)
            {
                for (int i = 0; i < Program.Mess.Count; i++)
                {
                    try
                    {
                        listBox1.Items.Add(Program.Mess.Dequeue());
                    }
                    catch (Exception ex) { }
                }
            }
            if (DataStruct.ProxyList.Count < 100)
            {
                foreach (var item in DataStruct.ProxyListFix)
                {
                    DataStruct.ProxyList.Enqueue(item);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<DataStruct.DataBots> bots = new List<DataStruct.DataBots>();
            bots.Add(new DataStruct.DataBots { IdBot = "76561198301321833", Site= "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198304254804", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198314718898", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198338763319", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198315009050", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198316432411", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198316868795", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198316692345", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198304264386", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198314670754", Site = "cs.money" });
            /////////////////////////////////////////////////////////////////////////////////////
            bots.Add(new DataStruct.DataBots { IdBot = "76561198316667197", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198316079595", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198316020391", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198316800514", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198338013178", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198315155019", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198338195694", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198338066870", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198339299211", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198315437061", Site = "cs.money" });
            /////////////////////////////////////////////////////////////////////////////////////
            bots.Add(new DataStruct.DataBots { IdBot = "76561198314816487", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198315968061", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198316129582", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198316198456", Site = "cs.money" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198364873979", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198364593611", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198365457481", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198329153366", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198285426768", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198361783191", Site = "csgosell.com" });
            /////////////////////////////////////////////////////////////////////////////////////
            bots.Add(new DataStruct.DataBots { IdBot = "76561198362755878", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198363066537", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198362187775", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198362036530", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198363369207", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198360947938", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198364869316", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198365073443", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198365184990", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198365516290", Site = "csgosell.com" });
            /////////////////////////////////////////////////////////////////////////////////////
            bots.Add(new DataStruct.DataBots { IdBot = "76561198364925952", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198364680193", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198364632508", Site = "csgosell.com" });
            bots.Add(new DataStruct.DataBots { IdBot = "76561198364639250", Site = "csgosell.com" });

            bots.Add(new DataStruct.DataBots { IdBot = "76561198410531663", Site = "loot.farm" });//28 бот
            bots.Add(new DataStruct.DataBots { IdBot = "76561198338797339", Site = "loot.farm" });//1
            bots.Add(new DataStruct.DataBots { IdBot = "76561198340077514", Site = "loot.farm" });//2
            bots.Add(new DataStruct.DataBots { IdBot = "76561198338043290", Site = "loot.farm" });//3
            bots.Add(new DataStruct.DataBots { IdBot = "76561198338098835", Site = "loot.farm" });//4
            bots.Add(new DataStruct.DataBots { IdBot = "76561198351303196", Site = "loot.farm" });//5
            bots.Add(new DataStruct.DataBots { IdBot = "76561198375976690", Site = "loot.farm" });//6
            bots.Add(new DataStruct.DataBots { IdBot = "76561198365507674", Site = "loot.farm" });//7
            bots.Add(new DataStruct.DataBots { IdBot = "76561198365557088", Site = "loot.farm" });//8
            bots.Add(new DataStruct.DataBots { IdBot = "76561198366459445", Site = "loot.farm" });//9
            bots.Add(new DataStruct.DataBots { IdBot = "76561198365883555", Site = "loot.farm" });//10
            bots.Add(new DataStruct.DataBots { IdBot = "76561198366228842", Site = "loot.farm" });//11
            bots.Add(new DataStruct.DataBots { IdBot = "76561198365470216", Site = "loot.farm" });//12
            bots.Add(new DataStruct.DataBots { IdBot = "76561198365503367", Site = "loot.farm" });//13
            bots.Add(new DataStruct.DataBots { IdBot = "76561198366506359", Site = "loot.farm" });//14
            bots.Add(new DataStruct.DataBots { IdBot = "76561198367010960", Site = "loot.farm" });//15
            bots.Add(new DataStruct.DataBots { IdBot = "76561198366364978", Site = "loot.farm" });//16
            bots.Add(new DataStruct.DataBots { IdBot = "76561198377070483", Site = "loot.farm" });//17
            bots.Add(new DataStruct.DataBots { IdBot = "76561198375981474", Site = "loot.farm" });//18
            bots.Add(new DataStruct.DataBots { IdBot = "76561198375867520", Site = "loot.farm" });//19
            bots.Add(new DataStruct.DataBots { IdBot = "76561198375784898", Site = "loot.farm" });//20
            bots.Add(new DataStruct.DataBots { IdBot = "76561198375456451", Site = "loot.farm" });//21
            bots.Add(new DataStruct.DataBots { IdBot = "76561198376074522", Site = "loot.farm" });//22
            bots.Add(new DataStruct.DataBots { IdBot = "76561198375165411", Site = "loot.farm" });//23
            bots.Add(new DataStruct.DataBots { IdBot = "76561198376232724", Site = "loot.farm" });//24
            bots.Add(new DataStruct.DataBots { IdBot = "76561198375117834", Site = "loot.farm" });//25
            bots.Add(new DataStruct.DataBots { IdBot = "76561198376466022", Site = "loot.farm" });//26
            bots.Add(new DataStruct.DataBots { IdBot = "76561198375218541", Site = "loot.farm" });//27
            bots.Add(new DataStruct.DataBots { IdBot = "76561198413200947", Site = "loot.farm" });//29
            bots.Add(new DataStruct.DataBots { IdBot = "76561198412071938", Site = "loot.farm" });//30

            StarterBot stBot = new StarterBot(bots);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
            timer3.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Telegram.AddUser(textBox1.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Telegram.RemoveUser(textBox2.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new System.Threading.Thread(delegate ()
            {
             //   Bot bot = new Bot(cookies, item1.IdBot, item1.Site, k);

            
            foreach (var item in DataStruct.ProxyListFix)
            {
               HttpClient client = null;
                
                client = new HttpClient();
                //   Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "ID={" + ID + "}|BOT:{" + BotId + "}|IP:{-" + newProxy + "-}");
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("User-Agent",
     "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
                var a1 = Convert.ToInt32(DateTime.Now.ToString("mmssfff"));
                var response = client.GetAsync("https://www.google.com.ua").Result;
                var a2 = Convert.ToInt32(DateTime.Now.ToString("mmssfff"));
                if (response.IsSuccessStatusCode)
                {
                    Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "IP={" + item + "}|Speed:{" + (a2-a1).ToString() + "}");
                }
                else { Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "IP-Bad={" + item + "}|Speed:{" + (a2 - a1).ToString() + "}"); }
                    
            }
            }).Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var a1 = Convert.ToInt32(DateTime.Now.ToString("ssfff"));
            Thread.Sleep(500);
            var a2 = Convert.ToInt32(DateTime.Now.ToString("ssfff"));
            MessageBox.Show((a2-a1).ToString());
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                //string tmp = "";
                //for (int i = 0; i < Program.Mess.Count; i++)
                //{
                //    var ab = Program.Mess.Dequeue();
                //    tmp = tmp + ab + Environment.NewLine;
                //}
                ////foreach (var item in listBox1.Items)
                ////{
                ////    tmp = tmp + item + Environment.NewLine;
                ////}
                //File.WriteAllText("./log/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".txt", tmp);
                ////listBox1.Items.Clear();
            }
            catch (Exception ex) { }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            label1.Text = "Количество прокси:" + DataStruct.ProxyList.Count;
            label2.Text = "Количество" + DataStruct.GoodItems.Count ;
            //if (Program.MessLog.Count > 0)
            //{
            //    for (int i = 0; i < Program.MessLog.Count; i++)
            //    {
            //        try
            //        {
            //            var item = Program.MessLog.Dequeue();
            //            if (item.Text == "" && item.Error=="")
            //            {
            //                dataGridView1.Rows.Add(item.Id, item.Site, item.Time, item.Text, item.Timeerror, item.Error);
            //            }
            //            else {
            //                if (item.Text != "")
            //                {
            //                    dataGridView1.Rows[item.Id].Cells["text"].Value = item.Text;
            //                    dataGridView1.Rows[item.Id].Cells["time"].Value = item.Time;
            //                }
            //                if (item.Error != "")
            //                {
            //                    dataGridView1.Rows[item.Id].Cells["timeError"].Value = item.Timeerror;
            //                    dataGridView1.Rows[item.Id].Cells["error"].Value = item.Error;

            //                }
            //            }
            //        }
            //        catch (Exception ex) { }
            //    }
            //}
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Program.CapchaUrl = textBox3.Text;
        }
    }
}

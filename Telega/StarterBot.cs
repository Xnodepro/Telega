using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Telega
{
    class StarterBot
    {
        private List<DataStruct.DataBots> Bots = new List<DataStruct.DataBots>();
        
        public StarterBot(List<DataStruct.DataBots> _bots)
        {
            new System.Threading.Thread(delegate (){ SetItemsToFileJson();  }).Start();
            Bots = _bots;
            GetItemsToFileJson();
            Start();
        }

        private void Start()
        {
            try
            {
                Site.money Money = new Site.money();//Экзепляр Cstrade.gg
                new System.Threading.Thread(delegate () { Money.INI(); }).Start(); //Start Cstrade.gg

                //Site.Loot loot = new Site.Loot();//Экзепляр Loot.Farm
                //new System.Threading.Thread(delegate () { loot.INI(); }).Start(); //Start Loot.Farm

                //Site.Deals deals = new Site.Deals();//Экзепляр Cs.Deals
                //new System.Threading.Thread(delegate () { deals.INI(); }).Start(); //Start Cs.Deals

                //Site.tradeskinsfast Tradeskinsfast = new Site.tradeskinsfast();//Экзепляр tradeskinsfast.com
                //new System.Threading.Thread(delegate () { Tradeskinsfast.INI(); }).Start(); //Start tradeskinsfast.com

                //Site.cstrade Cstrade = new Site.cstrade();
                //new System.Threading.Thread(delegate () { Cstrade.INI(); }).Start();

                //Site.csgosell Csgosell = new Site.csgosell();//Экзепляр Cstrade.gg
                //new System.Threading.Thread(delegate () { Csgosell.INI(); }).Start(); //Start Cstrade.gg

                //Site.cspoligon Poligon = new Site.cspoligon();
                //new System.Threading.Thread(delegate () { Poligon.INI(); }).Start();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void SetItemsToFileJson()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(300000);
                    string json = JsonConvert.SerializeObject(DataStruct.ItemsSearch);
                    System.IO.File.WriteAllText("./Config/ItemsSearch.data", json);
                }
                catch (Exception ex) { }
            }
        }
        private void GetItemsToFileJson()
        {
            try
            {
                string lk = System.IO.File.ReadAllText("./Config/ItemsSearch.data");
                if (lk != "")
                {
                    DataStruct.ItemsSearch = JsonConvert.DeserializeObject<List<ItemsInfo>>(lk);
                }
            }
            catch (Exception ex) { }
        }

    }
}

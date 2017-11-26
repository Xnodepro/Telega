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
                int k = 0;
                Program.MessLog.Add(new Types.LogInfo { Site = "cs.money" });
                Site.money Money = new Site.money();
                new System.Threading.Thread(delegate () { Money.INI(k); }).Start();

                Thread.Sleep(1000);
                k++;

                Program.MessLog.Add(new Types.LogInfo { Site = "loot.farm" });
                Site.Loot loot = new Site.Loot();
                new System.Threading.Thread(delegate () { loot.INI(k); }).Start();

                Thread.Sleep(1000);
                k++;

                Program.MessLog.Add(new Types.LogInfo { Site = "cs.deals" });
                Site.Deals deals = new Site.Deals();
                new System.Threading.Thread(delegate () { deals.INI(k); }).Start();

                Thread.Sleep(1000);
                k++;

                Program.MessLog.Add(new Types.LogInfo { Site = "tradeskinsfast.com" });
                Site.tradeskinsfast Tradeskinsfast = new Site.tradeskinsfast();
                new System.Threading.Thread(delegate () { Tradeskinsfast.INI(k); }).Start();

                Thread.Sleep(1000);
                k++;

                Program.MessLog.Add(new Types.LogInfo { Site = "cstrade.gg" });
                Site.cstrade Cstrade = new Site.cstrade();
                new System.Threading.Thread(delegate () { Cstrade.INI(k); }).Start();

                Thread.Sleep(1000);
                k++;

                Program.MessLog.Add(new Types.LogInfo { Site = "csgosell.com" });
                Site.csgosell Csgosell = new Site.csgosell();
                new System.Threading.Thread(delegate () { Csgosell.INI(k); }).Start();

                Thread.Sleep(1000);
                k++;

                Program.MessLog.Add(new Types.LogInfo { Site = "csgopolygon.com" });
                Site.cspoligon Poligon = new Site.cspoligon();
                new System.Threading.Thread(delegate () { Poligon.INI(k); }).Start();

                Thread.Sleep(1000);
                k++;

                Program.MessLog.Add(new Types.LogInfo { Site = "skin.trade" });
                Site.skintrade SkinTrade = new Site.skintrade();
                new System.Threading.Thread(delegate () { SkinTrade.INI(k); }).Start();

                Thread.Sleep(1000);
                k++;

                Program.MessLog.Add(new Types.LogInfo { Site = "trade-skins.com" });
                Site.trade_skins Trade_Skins = new Site.trade_skins();
                new System.Threading.Thread(delegate () { Trade_Skins.INI(k); }).Start();

                Thread.Sleep(1000);
                k++;

                Program.MessLog.Add(new Types.LogInfo { Site = "csoffer.me" });
                Site.csofferme CsOfferMe = new Site.csofferme();
                new System.Threading.Thread(delegate () { CsOfferMe.INI(k); }).Start();

                Thread.Sleep(1000);
                k++;

                Program.MessLog.Add(new Types.LogInfo { Site = "skinsjar.com" });
                Site.skinjar SkinJar = new Site.skinjar();
                new System.Threading.Thread(delegate () { SkinJar.INI(k); }).Start();
                //csoffer.me
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

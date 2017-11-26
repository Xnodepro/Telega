using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Telega.Site
{
    class money
    {
        IWebDriver driver;
        string Site = "cs.money";
        int IndexLog = 0;
        private void start()
        {
            var firstFull = Convert.ToInt32(DateTime.Now.ToString("HHmm"));
            while (true)
            {
                try
                {
                   
                   
                     var LastTime = Convert.ToInt32(DateTime.Now.ToString("HHmm"));
                    if (LastTime - firstFull>=1)
                    {
                        firstFull = LastTime;
                        IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                        for (int i = 0; i < 10; i++)
                        {
                            try
                            {
                                string title = (string)js.ExecuteScript("element = document.getElementsByClassName(\"item\");var aa = element.length;for( i =0; i< aa;i++){element[i].outerHTML = \"\";delete element[i];}");
                            }
                            catch (Exception ex) { }
                        }
                        
                        Program.Mess.Enqueue("[cs.money]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Обновил инвентарь");
                    }
                      //  Program.Mess.Enqueue("[cs.money]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов.");
                        ChekItem(driver.PageSource);
                    
                }
                catch (Exception ex)
                {
                    Program.Mess.Enqueue("[cs.money]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Ошибка2:" + ex.Message);
                }
                Thread.Sleep(100);
            }
        }
        public void INI(int index)
        {
            IndexLog = index;
            SetLogText("-", "-");
            var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
            driverService.HideCommandPromptWindow = true;                    //консоли
            driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl("https://cs.money/ru");

            driver.Navigate().GoToUrl("https://cs.money/authenticate");
            var login = driver.FindElement(By.Id("steamAccountName"));
            login.SendKeys("helpertrader");
            var pass = driver.FindElement(By.Id("steamPassword"));
            pass.SendKeys("Bogrinof114");
            var button = driver.FindElement(By.Id("imageLogin"));
            button.Click();
            Thread.Sleep(10000);
            driver.Navigate().GoToUrl("https://cs.money/ru");
            MessageBox.Show("Введите все данные , после этого программа продолжит работу!");
            var _cookies1 = driver.Manage().Cookies.AllCookies;
            List<KeyValuePair<string, string>> cook11 = new List<KeyValuePair<string, string>>();
            foreach (var item in _cookies1)
            {
                cook11.Add(new KeyValuePair<string, string>(item.Name, item.Value));
                //    Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + item.Name + "|" + item.Value);
            }

            WebSocket4Net.WebSocket websocket = new WebSocket4Net.WebSocket("wss://cs.money/ws", "", cook11, version: WebSocket4Net.WebSocketVersion.Rfc6455, userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
            websocket.MessageReceived += new EventHandler<WebSocket4Net.MessageReceivedEventArgs>(websocket_Opened);

            websocket.Open();
            //     Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|:" + websocket.State);
            new System.Threading.Thread(delegate () {
                while (true)
                {
                    //Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|:" + websocket.State);
                    if (websocket.State.ToString() == "Closed")
                    {
                        // Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|Open Connection");
                        SetLogText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "Open Connection");

                        driver.Navigate().Refresh();
                        Thread.Sleep(10000);
                        var _cookies2 = driver.Manage().Cookies.AllCookies;
                        List<KeyValuePair<string, string>> cook12 = new List<KeyValuePair<string, string>>();
                        foreach (var item in _cookies2)
                        {
                            cook12.Add(new KeyValuePair<string, string>(item.Name, item.Value));
                            // Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|"+ item.Name+"|"+item.Value);
                        }
                        websocket = new WebSocket4Net.WebSocket("wss://cs.money/ws", "", cook12, version: WebSocket4Net.WebSocketVersion.Rfc6455, userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.75 Safari/537.36");
                        websocket.MessageReceived += new EventHandler<WebSocket4Net.MessageReceivedEventArgs>(websocket_Opened);

                        websocket.Open();
                        //Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|:" + websocket.State);
                        SetLogText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), websocket.State.ToString());
                    }
                    Thread.Sleep(1000);
                }

            }).Start();

            new System.Threading.Thread(delegate () {
                var firstFull = Convert.ToInt32(DateTime.Now.ToString("HHmm"));
                while (true)
                {
                    try
                    {
                        var LastTime = Convert.ToInt32(DateTime.Now.ToString("HHmm"));
                        if (LastTime - firstFull >= 9)
                        {
                            firstFull = LastTime;
                            refr:
                            driver.Navigate().Refresh();
                            Thread.Sleep(1000);
                            if (driver.PageSource.Contains("A timeout occurred"))
                            {
                                goto refr;
                            }
                            //Program.Mess.Enqueue("" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Обновил Страницу");
                            SetLogText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "Обновил Страницу");
                        }
                    }
                    catch (Exception)
                    {
                    }
                    Thread.Sleep(1000);
                }

            }).Start();
        }
        private void SetLogText(string _time, string mess)
        {
            Program.MessLog[IndexLog].Time = _time;
            Program.MessLog[IndexLog].Text = mess;
        }
        private void websocket_Opened(object sender, WebSocket4Net.MessageReceivedEventArgs e)
        {
            //  Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|:" + e.Message);
            try
            {
                var da = JsonConvert.DeserializeObject<dynamic>(e.Message.Replace("event", "Event"));
                if (da.Event.Value == "add_items")
                {
                    var countAddItems = da.data.Count;
                    SetLogText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "Добавлено предметов на сайт:" + countAddItems);
                    //Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|Добавлено предметов на сайт:" + countAddItems);
                    ClickItem(da);
                }
            }
            catch (Exception ex)
            {
                Program.Mess.Enqueue(ex.Message);
            }

        }
        private bool ClickItem(dynamic json)
        {

            try
            {
                var da = json;
                var aa = da.Event.Value;
                foreach (var item in da.data)
                {

                    var id = item.id[0].Value;
                    var m = item.m.Value;
                    var p = item.p.Value;
                    string ee = "";
                    
                    if (item.e != null)
                    {
                        try
                        {
                            ee = item.e.Value;
                        }
                        catch (Exception ex) {}
                    }
                    string f = "";
                    if (item.f != null)  { try  {  f = item.f.Last.Value;  } catch (Exception ex) { } }

                    foreach (var itemSearch in DataStruct.ItemsSearch)
                    {
                        string na = m + CheckFactory(ee);
                        ItemsInfo II = new ItemsInfo()
                        {
                            Name = itemSearch.Name,
                            Site = itemSearch.Site,
                            Price = p.ToString(),
                            Floaat = f.ToString(),
                            telegram = itemSearch.telegram
                            
                        };
                        if (na.Replace(" ","")== (itemSearch.Name).Replace(" ", "") && DataStruct.GoodItems.Contains(II) == false)
                        {
                            DataStruct.GoodItems.Enqueue(II);

                        }
                    }

                }
            }
            catch (Exception ex) {/* Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|Ошибка2 :" + ex.Message);*/ }
            return false;
        }
        string CheckFactory(string ss )
        {
            switch (ss)
            {
                case "MW": return  "(MinimalWear)"; 
                case "FN": return "(FactoryNew)";
                case "FT": return "(Field-Tested)";
                case "BS": return "(Battle-Scarred)";
                case "WW": return "(Well-Worn)";
            }
            return "";
        }
        private void ChekItem(string kode)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(kode);
            string url = "";
            int ii = 0;
            try
            {
                var nodes = doc.DocumentNode.SelectNodes("//div[@id=\"inventory_bot\"]").FirstOrDefault();
                var nodes1 = nodes.ChildNodes.Where(n => (n.Name == "div"));
                
                foreach (var item1 in nodes1)
                {
                    ii++;
                    foreach (var it in DataStruct.ItemsSearch)
                    {
                        string name1 = item1.Attributes["hash"].Value.Replace(" ", "");
                        string name2 = (it.Name).Replace(" ", "");
                        if (name1 == name2)
                        {
                            if (Site == it.Site && name1 == name2 && DataStruct.GoodItems.Contains(it) == false)
                            {
                                //  var price = (Convert.ToInt32(item1.FirstChild.Attributes["data-p"].Value) / 100);
                                //var price = (Convert.ToInt32(item1.FirstChild.Attributes["data-p"].Value) / 100);
                                  Program.Mess.Enqueue("[cs.money]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет:" + name1);
                                DataStruct.GoodItems.Enqueue(it);

                            }


                        }
                    }
                }
                Program.Mess.Enqueue("[cs.money]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Количество:" + ii);
            }
            catch (Exception ex) { Program.Mess.Enqueue("[cs.money]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "Количество:|"+ii+"| Ошибка:" + ex.Message); }
           
            

        }
    }
}

using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace Telega.Site
{
    class skinjar
    {
        string Site = "skinsjar.com";
        IWebDriver driver;
        int ID = 0;
        Data ITEMS;
        int IndexLog = 0;
        List<System.Net.Cookie> cook;
        List<System.Net.Cookie> cookAll;


        public struct Data
        {
            public string Bot { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public long Id { get; set; }
            public string floatt { get; set; }
        }

        public void INI(int index)
        {
            IndexLog = index;
            SetLogText("-", "-");
            var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
            driverService.HideCommandPromptWindow = true;                    //консоли
            driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl("https://skinsjar.com/ru");

            driver.Navigate().GoToUrl("https://skinsjar.com/login");
            var login = driver.FindElement(By.Id("steamAccountName"));
            login.SendKeys("helpertrader");
            var pass = driver.FindElement(By.Id("steamPassword"));
            pass.SendKeys("Bogrinof114");
            var button = driver.FindElement(By.Id("imageLogin"));
            button.Click();
            Thread.Sleep(10000);
            driver.Navigate().GoToUrl("https://skinsjar.com/ru");
            MessageBox.Show("Введите все данные , после этого программа продолжит работу!");

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            var _cookies = driver.Manage().Cookies.AllCookies;
            foreach (var item in _cookies)
            {
                handler.CookieContainer.Add(new System.Net.Cookie(item.Name, item.Value) { Domain = item.Domain });
            }


            for (int i = 0; i < 10; i++)
            {
                new System.Threading.Thread(delegate () {
                    Get(handler, i);
                }).Start();
                Thread.Sleep(500);
                //  Program.Mess.Enqueue("БОТ[" + i + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Запустил запрос:" + i);
            }
            start();
        }
        private void SetLogText(string _time, string mess)
        {
            Program.MessLog[IndexLog].Time = _time;
            Program.MessLog[IndexLog].Text = mess;
        }
        public void start()
        {
        }
        private void Get(HttpClientHandler handler, int id)
        {
            HttpClientHandler handler1 = handler;
            HttpClient client = null;
            HttpClientHandler handler2 = new HttpClientHandler();
            handler2.CookieContainer = handler1.CookieContainer;
            handler2.Proxy = null;
            // client = Prox(client, handler2, newProxy);
            client = new HttpClient(handler2);
            client.Timeout = TimeSpan.FromSeconds(300);
            client.DefaultRequestHeaders.Add("User-Agent",
         "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "*/*");
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            while (true)
            {
                try
                {

                    var response = client.GetAsync("https://skinsjar.com/api/v3/load/bots?refresh=0&v=0").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        var it = JsonConvert.DeserializeObject<dynamic>(responseString);
                        ClickItem(it);
                        SetLogText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "Завершил загрузку ботов");
                        // Program.MessTSF.Enqueue("БОТ[" + id + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов:" + it.Count);
                    }
                }
                catch (Exception ex) { /*Thread.Sleep(1000); Program.MessTSF.Enqueue("БОТ[" + id + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + ":" + ex.Message); */}
            }
        }

        private void ClickItem(dynamic itm)
        {
            try
            {
                foreach (var item in itm.items)
                {

                    try
                    {
                        foreach (var it in DataStruct.ItemsSearch)
                        {

                            if (Site == it.Site && item.name.Value.ToString().Replace(" ", "") == (it.Name).Replace(" ", ""))
                            {
                                //  Program.Mess.Enqueue("[trade-skins]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет:" + it.Name);
                                ItemsInfo II = new ItemsInfo()
                                {
                                    Name = it.Name,
                                    Site = it.Site,
                                    Price = item.price.Value.ToString(),
                                    Floaat = "Float:" + item.floatMin.Value.ToString() + "-" + item.floatMax.Value.ToString(),
                                    telegram = it.telegram

                                };
                                if (DataStruct.GoodItems.Any(n => n.Name == II.Name && n.Site == II.Site && n.Price == II.Price && n.Floaat == II.Floaat && n.telegram == II.telegram && n.Sticker == II.Sticker) == false)
                                {
                                    // Program.Mess.Enqueue("[trade-skins]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Отправил:" + it.Name);
                                    DataStruct.GoodItems.Enqueue(II);
                                }
                                //if ( DataStruct.GoodItems.Contains(II) == false)
                                //{
                                //    //    Program.Mess.Enqueue("[cstrade.gg]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет:" + it.Name);
                                //    DataStruct.GoodItems.Enqueue(II);
                                //}

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ss = ex.Message;
                    }

                }
            }
            catch (Exception ex) { }


        }



    }
}

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

namespace Telega.Site
{
    class cstrade
    {
        string Site = "cstrade.gg";
        IWebDriver driver;
        int ID = 0;
        Data ITEMS;
        List<System.Net.Cookie> cook;
        List<System.Net.Cookie> cookAll;
        public struct Data
        {

            public List<inData> inventory { get; set; }
            public string status { get; set; }
        }
        public struct inData
        {
            public int bot { get; set; }

            public string market_hash_name { get; set; }
            public double price { get; set; }

            public string id { get; set; }
        }
       
        public void INI()
        {
            var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
            driverService.HideCommandPromptWindow = true;                    //консоли
            driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl("https://cstrade.gg/");
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

        public void start()
        {
            
            //while (true)
            //{
            //    try
            //    {
                    

            //        Thread.Sleep(200);
            //    }
            //    catch (Exception ex) { }
            //}
        }
        private void Get(HttpClientHandler handler, int id)
        {
            HttpClientHandler handler1 = handler;
            HttpClient client = null;


            while (true)
            {
                try
                {
                    client = new HttpClient(handler1);
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Add("User-Agent",
         "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                    client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                    //client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                    //client.DefaultRequestHeaders.Add("Connection", "keep-alive");

                    var response = client.GetAsync("https://cstrade.gg/loadBotInventory").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        ITEMS = JsonConvert.DeserializeObject<Data>(responseString);
                        ClickItem(ITEMS);
                        Program.Mess.Enqueue("[cstrade.gg] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов:" + ITEMS.inventory.Count);
                    }

                    Random random = new Random();

                    Thread.Sleep(300);
                }
                catch (Exception ex) {/* Program.MessCsTrade.Enqueue("БОТ[" + id + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + ":" + ex.Message);*/ }
            }
            // return new Data();
        }
        private void ClickItem(Data Items)
        {
            if (Items.inventory != null)
            {
                foreach (var item in Items.inventory)
                {
                    foreach (var it in DataStruct.ItemsSearch)
                    {

                        if (Site == it.Site && item.market_hash_name.Replace(" ", "") == (it.Name).Replace(" ", "") && DataStruct.GoodItems.Contains(it) == false)
                        {
                        //    Program.Mess.Enqueue("[cstrade.gg]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет:" + it.Name);
                            DataStruct.GoodItems.Enqueue(it);
                        }
                    }

                }
            }

        }

    }
}

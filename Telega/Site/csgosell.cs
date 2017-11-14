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
    class csgosell
    {
        string Site = "csgosell.com";
        IWebDriver driver;
        int ID = 0;
        List<Data> ITEMS;
        List<System.Net.Cookie> cook;
        List<System.Net.Cookie> cookAll;

        public struct Data
        {
            //    public List<inData> response { get; set; }
            public string h { get; set; }
            public double p { get; set; }
        }
        public struct inData
        {

            public string h { get; set; }
            public double p { get; set; }

        }
        public void INI()
        {
            var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
            driverService.HideCommandPromptWindow = true;                    //консоли
            driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl("https://csgosell.com/");
            MessageBox.Show("Введите все данные , после этого программа продолжит работу!");

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            var _cookies = driver.Manage().Cookies.AllCookies;
            foreach (var item in _cookies)
            {
                handler.CookieContainer.Add(new System.Net.Cookie(item.Name, item.Value) { Domain = item.Domain });
            }


            for (int i = 0; i <8 ; i++)
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
                    client.DefaultRequestHeaders.Add("Accept", "*/*");
                    client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
                    //client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                    //client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    var stringContent = new StringContent("stage=botAll&steamId=76561198364873979&hasBonus=false&coins=0", Encoding.UTF8, "application/x-www-form-urlencoded");
                    //  var response = client.GetAsync("https://csgosell.com/phpLoaders/forceBotUpdate/all.txt").Result;
                    var response = client.PostAsync("https://csgosell.com/phpLoaders/forceBotUpdate/all.txt", stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        ITEMS = JsonConvert.DeserializeObject<List<Data>>(responseString);
                        ClickItem(ITEMS);
                        Program.Mess.Enqueue("["+Site+"] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов:" + ITEMS.Count);
                    }

                    Random random = new Random();

                    Thread.Sleep(300);
                }
                catch (Exception ex) {/* Program.MessCsTrade.Enqueue("БОТ[" + id + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + ":" + ex.Message);*/ }
            }
            // return new Data();
        }
        private void ClickItem(List<Data> Items)
        {
            try
            {
                if (Items != null)
                {
                    foreach (var item in Items)
                    {
                        foreach (var it in DataStruct.ItemsSearch)
                        {
                            try
                            {
                                    if (Site == it.Site && item.h.ToString().Replace(" ", "") == it.Name.Replace(" ", "") && DataStruct.GoodItems.Contains(it) == false)
                                    {
                                        //      Program.Mess.Enqueue("[tradeskinsfast.com]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет:" + it.Name);
                                        DataStruct.GoodItems.Enqueue(it);
                                    }
                                
                            }
                            catch (Exception ex)
                            {
                                //  Program.MessDeals.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| :" +ex.Message);
                            }
                        }

                    }
                }
            }
            catch (Exception ex) { }

        }
    }
}

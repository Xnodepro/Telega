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
        int IndexLog = 0;
        List<System.Net.Cookie> cook;
        List<System.Net.Cookie> cookAll;

        public struct Data
        {
            //    public List<inData> response { get; set; }
            public string h { get; set; }
            public double p { get; set; }
            public string f { get; set; }
        }
        public struct inData
        {

            public string h { get; set; }
           
            public double p { get; set; }

        }
        public void INI(int index)
        {
            IndexLog = index;
            SetLogText("-", "-");
            var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
            driverService.HideCommandPromptWindow = true;                    //консоли
            driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl("https://csgosell.com/");

            driver.Navigate().GoToUrl("https://csgosell.com?login");
            var login = driver.FindElement(By.Id("steamAccountName"));
            login.SendKeys("helpertrader");
            var pass = driver.FindElement(By.Id("steamPassword"));
            pass.SendKeys("Bogrinof114");
            var button = driver.FindElement(By.Id("imageLogin"));
            button.Click();
            Thread.Sleep(10000);
            driver.Navigate().GoToUrl("https://csgosell.com/");
            MessageBox.Show("Введите все данные , после этого программа продолжит работу!");

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            var _cookies = driver.Manage().Cookies.AllCookies;
            foreach (var item in _cookies)
            {
                handler.CookieContainer.Add(new System.Net.Cookie(item.Name, item.Value) { Domain = item.Domain });
            }


            for (int i = 0; i < 8; i++)
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

            //while (true)
            //{
            //    try
            //    {


            //        Thread.Sleep(200);
            //    }
            //    catch (Exception ex) { }
            //}
        }
        public HttpClient Prox(HttpClient client1, HttpClientHandler handler, string paroxyu)
        {

            HttpClient client = client1;
            try
            {
                string
                httpUserName = "webminidead",
                httpPassword = "159357Qq";
                string proxyUri = paroxyu;
                NetworkCredential proxyCreds = new NetworkCredential(
                    httpUserName,
                    httpPassword
                );
                WebProxy proxy = new WebProxy(proxyUri, false)
                {
                    UseDefaultCredentials = false,
                    Credentials = proxyCreds,
                };
                try
                {
                    handler.Proxy = null;
                    handler.Proxy = proxy;
                    handler.PreAuthenticate = true;
                    handler.UseDefaultCredentials = false;
                    handler.Credentials = new NetworkCredential(httpUserName, httpPassword);
                    handler.AllowAutoRedirect = true;
                }
                catch (Exception ex) { }
                client = new HttpClient(handler);
            }
            catch (Exception ex) { }
            return client;
        }
        private void Get(HttpClientHandler handler, int id)
        {
            HttpClientHandler handler1 = handler;

            while (true)
            {
                try
                {
                    string newProxy = DataStruct.ProxyList.Dequeue();
                    HttpClient client = null;
                    HttpClientHandler handler2 = new HttpClientHandler();
                    handler2.CookieContainer = handler1.CookieContainer;
                    handler2.Proxy = null;
                    client = Prox(client, handler2, newProxy);// new HttpClient(handler1);
                    client.Timeout = TimeSpan.FromSeconds(60);
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
                       // Program.Mess.Enqueue("[cs.deals]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|0");
                        ClickItem(ITEMS);
                        SetLogText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "Завершил загрузку предметов:" + ITEMS.Count);
                       // Program.Mess.Enqueue("[" + Site + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов:" + ITEMS.Count);
                    }

                    Random random = new Random();

                    Thread.Sleep(300);
                }
                catch (Exception ex) { /*Program.Mess.Enqueue("БОТ[" + id + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + ":" + ex.Message);*/ }
            }
            // return new Data();
        }
        private void ClickItem(List<Data> Items)
        {
            try
            {
               // Program.Mess.Enqueue("[cs.deals]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| 1");
                if (Items != null)
                {
                    foreach (var item in Items)
                    {
                        foreach (var it in DataStruct.ItemsSearch)
                        {
                            try
                            {
                                ItemsInfo II = new ItemsInfo()
                                {
                                    Name = it.Name,
                                    Site = it.Site,
                                    Price = item.p.ToString(),
                                    Floaat = item.f.ToString(),
                                    telegram = it.telegram
                                };
                                if (Site == it.Site && item.h.Replace(" ", "") == (it.Name).Replace(" ", "") && DataStruct.GoodItems.Contains(II) == false)
                                {
                                    // Program.Mess.Enqueue("[tradeskinsfast.com]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет:" + it.Name);
                                    DataStruct.GoodItems.Enqueue(II);
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }

                    }
                }
               // Program.Mess.Enqueue("[cs.deals]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| 2");
            }
            catch (Exception ex) { }

        }
    }
}

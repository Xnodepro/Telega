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
    class tradeskinsfast
    {
        IWebDriver driver;
        int ID = 0;
        dynamic ITEMS;
        int CountThread = 10;
        List<System.Net.Cookie> cook;
        List<System.Net.Cookie> cookAll;
        string Site = "tradeskinsfast.com";

        public struct Data
        {

            public List<inData> response { get; set; }
            public bool success { get; set; }
        }
        public struct inData
        {
            public int b { get; set; }
            public string m { get; set; }
            public double p { get; set; }

            public string a { get; set; }
        }

        public void INI()
        {
            var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
            driverService.HideCommandPromptWindow = true;                    //консоли
            driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl("https://ru.tradeskinsfast.com/");
            MessageBox.Show("Введите все данные , после этого программа продолжит работу!");

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();

            var _cookies = driver.Manage().Cookies.AllCookies;
            foreach (var item in _cookies)
            {
                handler.CookieContainer.Add(new System.Net.Cookie(item.Name, item.Value) { Domain = item.Domain });
            }
            for (int i = 0; i < CountThread; i++)
            {
                new System.Threading.Thread(delegate () {
                    Get(handler, i);
                }).Start();
                Thread.Sleep(1000);
            }
            start();
        }

        public void start()
        {

            //while (true)
            //{
            //    try
            //    {
            //        //  Program.Mess.Enqueue("[tradeskinsfast.com]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов.");
                    
            //    }
            //    catch (Exception ex) { }
            //    Thread.Sleep(200);
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
         "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                    client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                    client.DefaultRequestHeaders.Add("Origin", "https://ru.tradeskinsfast.com");
                    client.DefaultRequestHeaders.Add("Referer", "https://ru.tradeskinsfast.com/");
                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    var stringContent = new StringContent("", Encoding.UTF8, "application/json");
                    //    stringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue() "application/json";
                    var response = client.PostAsync("https://ru.tradeskinsfast.com/ajax/botsinventory", stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        ITEMS = JsonConvert.DeserializeObject<dynamic>(responseString);
                        ClickItem(ITEMS);
                        Program.Mess.Enqueue("[tradeskinsfast.com]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов:" + ITEMS.response.Count);
                    }

                    Thread.Sleep(2600);
                }
                catch (Exception ex) { /*Program.MessDeals.Enqueue("БОТ[" + id + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + ex.Message); */}
            }

        }
        private void ClickItem(dynamic Items)
        {
            try
            {
                if (Items != null)
                {
                    foreach (var item in Items.response)
                    {
                        foreach (var it in DataStruct.ItemsSearch)
                        {
                            try
                            {
                                if (item.m.Value is String)
                                {
                                    if (Site == it.Site && item.m.Value.ToString().Replace(" ", "") == it.Name.Replace(" ", "") && DataStruct.GoodItems.Contains(it) == false)
                                    {
                                  //      Program.Mess.Enqueue("[tradeskinsfast.com]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет:" + it.Name);
                                        DataStruct.GoodItems.Enqueue(it);
                                    }
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

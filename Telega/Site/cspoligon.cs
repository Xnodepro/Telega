﻿using Newtonsoft.Json;
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
    class cspoligon
    {
        string Site = "csgopolygon.com";
        IWebDriver driver;
        int ID = 0;
        dynamic ITEMS;
        int CountThread = 10;
        List<System.Net.Cookie> cook;
        List<System.Net.Cookie> cookAll;
        string IP = "";

        public void INI()
        {
            try
            {
                var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
                driverService.HideCommandPromptWindow = true;                    //консоли
                driver = new ChromeDriver(driverService);
                driver.Navigate().GoToUrl("https://csgopolygon.com/withdraw.php");

                MessageBox.Show("Введите все данные , после этого программа продолжит работу!");



                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();


                
                MessageBox.Show("Возьмите ключ капчи");
                var _cookies = driver.Manage().Cookies.AllCookies;
                foreach (var item in _cookies)
                {
                    handler.CookieContainer.Add(new System.Net.Cookie(item.Name, item.Value) { Domain = item.Domain });
                }
                for (int i = 0; i < CountThread; i++)
                {
                    new System.Threading.Thread(delegate ()
                    {
                        // Get(handler, i);
                        Get(handler, i);
                    }).Start();
                    Thread.Sleep(1000);
                }
                start();
            }
            catch (Exception ex) { Program.Mess.Enqueue(ex.Message); }
        }

        public void start()
        {
            //var firstFull = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
            //while (true)
            //{

            //    var res = ClickItem(ITEMS);
            //    if (res == true)
            //    {
            //        var first = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
            //    }

            //    Thread.Sleep(200);
            //}
        }
        private void Get(HttpClientHandler handler, int id)
        {
            HttpClientHandler handler1 = handler;
            while (true)
            {
                try
                {

                    HttpClient client = null;
                    HttpClientHandler handler2 = new HttpClientHandler();
                    handler2.CookieContainer = handler1.CookieContainer;
                    handler2.Proxy = null;

                    client = new HttpClient(handler2);
                    client.Timeout = TimeSpan.FromSeconds(100);
                    client.DefaultRequestHeaders.Add("User-Agent",
         "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept", "*/*");
                    client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");

                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    var stringContent = new StringContent("", Encoding.UTF8, "application/json");

                    var response = client.PostAsync("https://csgopolygon.com/scripts/_get_bank.php?captcha=" + Program.CapchaUrl, stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        ITEMS = JsonConvert.DeserializeObject<dynamic>(responseString);
                        ClickItem(ITEMS);
                        Program.Mess.Enqueue("[Poligon] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов:" + ITEMS.items.Count);
                    }
                    
                    Thread.Sleep(1000);

                }
                catch (Exception ex) { Thread.Sleep(1000); Program.Mess.Enqueue("[Poligon]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + ":" + ex.Message); }

            }
            // return new Data();
        }
        private bool ClickItem(dynamic Items)
        {
            try
            {
                if (Items != null)
                {
                    foreach (var item in Items.items)
                    {
                        foreach (var it in DataStruct.ItemsSearch)
                        {
                            try
                            {
                                if (item.name.Value is String)
                                {
                                    if (item.name.Value.ToString().Replace(" ", "") == (it.Name).Replace(" ", "") && DataStruct.GoodItems.Contains(it) == false)
                                    {
                                        DataStruct.GoodItems.Enqueue(it);
                                    }

                                }
                            }
                            catch (Exception ex) { }
                        }

                    }
                }
                
            }
            catch (Exception ex) { }
            return false;
        }
    }
}

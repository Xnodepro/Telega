using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Telega
{
    class Bot
    {
        CookieContainer cookies = new CookieContainer();
        string BotId = "";
        string Site = "";
        int ID = 0;
        int pp = 0;
        public Bot(CookieContainer _cookies,string _botId,string _site, int _ID)
        {
            try
            {
                cookies = _cookies;
                BotId = _botId;
                Site = _site;
                ID = _ID;
                for (int i = 0; i < 1; i++)
                {
                    Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "ID={" + ID + "}|BOT:{" + BotId + "}|Запустил бота!");
                    //Types.LogInfo items = new Types.LogInfo() { Id = _ID, Site = _site };
                    //Program.MessLog.Enqueue(items);
                    //new System.Threading.Thread(delegate ()
                    //{
                        Work();
                    //}).Start();
                    new System.Threading.Thread(delegate ()
                    {
                        CheckWork();
                    }).Start();
                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex) {
                Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "ID={" + ID + "}|BOT:{" + BotId + "}Main|" + ex.Message);
            }
        }
        private void CheckWork()
        {
            int temp = pp;
            while (true)
            {
                try
                {
                    Thread.Sleep(40000);
                    if (temp == pp)
                    {
                        Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "ID={" + ID + "}|BOT:{" + BotId + "}|Перезапустил");
                        //Types.LogInfo log = new Types.LogInfo() { Id = ID, Text = "Перезапустил", Time = DateTime.Now.ToString("HH:mm:ss:fff") };
                        //Program.MessLog.Enqueue(log);
                        temp = pp;
                        //new System.Threading.Thread(delegate ()
                        //{
                            Work();
                        //}).Start();
                    }
                    temp = pp;
                }
                catch (Exception ex) {
                    Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "ID={" + ID + "}|BOT:{" + BotId + "}CheckWork|" + ex.Message);
                }
            }
        }
        private void Work()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        pp++;
                        Random rr = new Random();
                        int sl = rr.Next(100, 500);
                        Thread.Sleep(sl);

                        //Types.LogInfo log = new Types.LogInfo() { Id = ID, Text = "проверка-"+ pp.ToString(), Time = DateTime.Now.ToString("HH:mm:ss:fff") };
                        //Program.MessLog.Enqueue(log);
                        //  Program.MessLog.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "ID={" + ID + "}|[]BOT:" + BotId + "|" + "Запустился в цыкле");
                        var items = Get();
                        if (items != null)
                        {
                            int k = 0;
                            foreach (var item in items.rgDescriptions)
                            {
                                foreach (var it in DataStruct.ItemsSearch)
                                {
                                    if (Site == it.Site && item.First.market_hash_name.Value.ToString().Replace(" ", "") == it.Name.Replace(" ", ""))
                                    {
                                        DataStruct.GoodItems.Enqueue(it);
                                    }
                                }
                                k++;
                            }
                            Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "ID={" + ID + "}|BOT:{" + BotId + "}|Количсетво предметов:{-" + k + "-}");
                        }
                        else
                        {
                            Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "ID={" + ID + "}|BOT:" + BotId + "|" + "|null");
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "ID={" + ID + "}|BOT:{" + BotId + "}Work1|" + ex.Message);
                        //Types.LogInfo er = new Types.LogInfo() { Id = ID, Error = "Work1|"+ex.Message, Timeerror = DateTime.Now.ToString("HH:mm:ss:fff") };
                        //Program.MessLog.Enqueue(er);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "ID={" + ID + "}|BOT:{" + BotId + "}Work2|" + ex.Message);
                //Types.LogInfo er = new Types.LogInfo() { Id = ID, Error = "Work2|" + ex.Message, Timeerror = DateTime.Now.ToString("HH:mm:ss:fff") };
                //Program.MessLog.Enqueue(er);
            }
        }
        private dynamic Get()
        {

            try
                {
                    string newProxy = DataStruct.ProxyList.Dequeue();
                    HttpClient client = null;
                    HttpClientHandler handler2 = new HttpClientHandler();
                    handler2.CookieContainer = cookies;
                    handler2.Proxy = null;
                    client = Prox(client, handler2, newProxy);
                    client.Timeout = TimeSpan.FromSeconds(3);
                    client.DefaultRequestHeaders.Add("User-Agent",
         "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");

                    var response = client.GetAsync("http://steamcommunity.com/profiles/"+BotId+"/inventory/json/730/2").Result;
                    if (response.IsSuccessStatusCode)
                    {
                      var responseContent = response.Content;
                      string responseString = responseContent.ReadAsStringAsync().Result;
                    if (responseString != "null")
                    {
                        return JsonConvert.DeserializeObject<dynamic>(responseString);
                    }
                    //else { Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "ID={" + ID + "}|BOT:" + BotId + "|" + "null" ); }
                    }
                client.Dispose();
                handler2.Dispose();

                }
                catch (Exception ex)
                {
                //Types.LogInfo er = new Types.LogInfo() { Id = ID, Error = "GET|"+ex.Message, Timeerror = DateTime.Now.ToString("HH:mm:ss:fff") };
                //Program.MessLog.Enqueue(er);
            }
            return null;

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
                catch (Exception ex)
                {
                    //Types.LogInfo er = new Types.LogInfo() {Id = ID, Error = "1|"+ex.Message, Timeerror = DateTime.Now.ToString("HH:mm:ss:fff") };
                    //Program.MessLog.Enqueue(er);
                }
                client = new HttpClient(handler);
            }
            catch (Exception ex)
            {
                //Types.LogInfo er = new Types.LogInfo() { Id = ID, Error = "2|" + ex.Message, Timeerror = DateTime.Now.ToString("HH:mm:ss:fff") };
                //Program.MessLog.Enqueue(er);
            }
            return client;
        }
    }
}

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
    class Loot
    {

        IWebDriver driver;
        int IndexLog = 0;
        string Site = "loot.farm";
        private void start()
        {
            var firstFull = Convert.ToInt32(DateTime.Now.ToString("HHmm"));
            while (true)
            {
                try
                {
                    var LastTime = Convert.ToInt32(DateTime.Now.ToString("HHmm"));
                    if (LastTime - firstFull >= 1)
                    {
                        firstFull = LastTime;
                        IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                            try
                            {
                                string title = (string)js.ExecuteScript("document.getElementById(\"UpdateBotInv\").click();");
                            }
                            catch (Exception ex) { }
                        /*  Program.Mess.Enqueue("[cs.money]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Обновил инвентарь");*/
                        SetLogText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "Обновил инвентарь");
                    }
                    /*   Program.Mess.Enqueue("[Loot.Farm]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов.");*/
                    SetLogText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "Завершил загрузку предметов");
                    ChekItem(driver.PageSource);
                }
                catch (Exception)
                {
                }
                Thread.Sleep(200);
            }
        }
        public void INI(int index)
        {
            IndexLog = index;
            SetLogText("-", "-");
            var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
            driverService.HideCommandPromptWindow = true;                    //консоли
            driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl("https://loot.farm/");

            driver.Navigate().GoToUrl("https://loot.farm/steam_auth.php");
            var login = driver.FindElement(By.Id("steamAccountName"));
            login.SendKeys("helpertrader");
            var pass = driver.FindElement(By.Id("steamPassword"));
            pass.SendKeys("Bogrinof114");
            var button = driver.FindElement(By.Id("imageLogin"));
            button.Click();
            Thread.Sleep(10000);
            driver.Navigate().GoToUrl("https://loot.farm/");
            MessageBox.Show("Введите все данные , после этого программа продолжит работу!");

            start();
        }
        

        private void SetLogText(string _time, string mess)
        {
            Program.MessLog[IndexLog].Time = _time;
            Program.MessLog[IndexLog].Text = mess;
        }

        private void ChekItem(string kode)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(kode);
            try
            {
                var nodes = doc.DocumentNode.SelectNodes("//div[@id=\"bots_inv\"]").FirstOrDefault();
                var nodes1 = nodes.ChildNodes.Where(n => (n.Name == "div"));
                foreach (var item1 in nodes1)
                {
                    foreach (var it in DataStruct.ItemsSearch)
                    {
                        string name1 = item1.FirstChild.Attributes["data-name"].Value.Replace(" ", "");
                        var price = (Convert.ToInt32(item1.FirstChild.Attributes["data-p"].Value) / 100).ToString();
                        string name2 = it.Name.Replace(" ", "");
                        ItemsInfo II = new ItemsInfo()
                        {
                            Name = name1,
                            Site = it.Site,
                            Price = price,
                            telegram = it.telegram

                        };
                        if (Site == it.Site && name1 == name2&& DataStruct.GoodItems.Contains(II) ==false)
                        {
                        //  var price = (Convert.ToInt32(item1.FirstChild.Attributes["data-p"].Value) / 100);
                            //var price = (Convert.ToInt32(item1.FirstChild.Attributes["data-p"].Value) / 100);
                          //  Program.Mess.Enqueue("[loot.farm]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет:" + name1);
                            DataStruct.GoodItems.Enqueue(II);

                        }
                    }
                }
            }
            catch (Exception ex) { }

        }
      
       


    }
}

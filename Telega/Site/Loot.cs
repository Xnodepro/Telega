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
                        Program.Mess.Enqueue("[cs.money]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Обновил инвентарь");
                    }
                    Program.Mess.Enqueue("[Loot.Farm]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов.");
                    ChekItem(driver.PageSource);
                }
                catch (Exception)
                {
                }
                Thread.Sleep(200);
            }
        }
        public void INI()
        {
            var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
            driverService.HideCommandPromptWindow = true;                    //консоли
            driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl("https://loot.farm/");
           
            MessageBox.Show("Выберите настройки фильтров.");
            start();
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
                        string name2 = it.Name.Replace(" ", "");
                        if (Site == it.Site && name1 == name2&& DataStruct.GoodItems.Contains(it) ==false)
                        {
                        //  var price = (Convert.ToInt32(item1.FirstChild.Attributes["data-p"].Value) / 100);
                            //var price = (Convert.ToInt32(item1.FirstChild.Attributes["data-p"].Value) / 100);
                          //  Program.Mess.Enqueue("[loot.farm]" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет:" + name1);
                            DataStruct.GoodItems.Enqueue(it);

                        }
                    }
                }
            }
            catch (Exception ex) { }

        }
      
       


    }
}

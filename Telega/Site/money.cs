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
        public void INI()
        {
            var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
            driverService.HideCommandPromptWindow = true;                    //консоли
            driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl("https://cs.money/ru");

            MessageBox.Show("Выберите настройки фильтров.");
            start();
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

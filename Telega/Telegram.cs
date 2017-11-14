using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Telega
{
    static class Telegram
    {
        #region Fields
        private static List<UserTelegram> User = new List<UserTelegram>();
        private static readonly TelegramBotClient Bot = new TelegramBotClient("477705043:AAGeGPgItwxfGrGfXF8W3nJlqWUhJq3lDco");
        private static List<Types.TempGood> TempGodItems = new List<Types.TempGood>();
        #endregion

        #region TelegaBotMethod
        public static void Start()
        {
            UserGetFromFile();
            if (User.Count == 0)
            {
                FirstSetUserNameTeleg();//подгружаем список ников телеграма
            }
            new System.Threading.Thread(delegate (){ CheckGoodItems(); }).Start();
            new System.Threading.Thread(delegate () { UserSetToFile(); }).Start();
            
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;

            var me = Bot.GetMeAsync().Result;
            Bot.StartReceiving();

        }
        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }
        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
        }
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {

            var message = messageEventArgs.Message;
            if (User.Exists(N => (N.Name == message.From.Username)))
            {
                if (message == null || message.Type != MessageType.TextMessage) return;

                else if (message.Text.StartsWith("/ADDME")) // send custom keyboard
                {
                    User.Find(N => (N.Name == message.From.Username)).IdChat = message.Chat.Id;
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Я подтвердил тебя!");
                }
                /////////////////////Вывод  меню при первом посищении бота//////////////////////////
                else if (message.Text.StartsWith("/start")) // send custom keyboard
                {
                    var keyboard = new ReplyKeyboardMarkup(new[]
                    {
                    new [] // first row
                    {
                        new KeyboardButton("Add Items"),
                        new KeyboardButton("Remove Items"),
                    },
                });

                    await Bot.SendTextMessageAsync(message.Chat.Id, "Привет, я умею искать предметы на всех обменниках CSGO",
                        replyMarkup: keyboard);
                }
                ///////////////////////////////////////////////////////////////////

                /////////////////////Вывод стартового меню/////////////////////////
                else if (message.Text.StartsWith("Меню")) // send custom keyboard
                {
                    var keyboard = new ReplyKeyboardMarkup(new[]
                    {
                    new [] // first row
                    {
                        new KeyboardButton("Add Items"),
                        new KeyboardButton("Remove Items"),
                    },
                });

                    await Bot.SendTextMessageAsync(message.Chat.Id, "Перешел в Главное меню.",
                        replyMarkup: keyboard);
                }
                ///////////////////////////////////////////////////////////////////

                /////////////////////Добавление предмета, вывод сайтов на которые можно добавить предмет/////////////////////////
                else if (message.Text.StartsWith("Add Items")) // send custom keyboard
                {
                    SetAction(message.From.Username, "Add");//Устанавливаем екш Add

                    var keyboard = new ReplyKeyboardMarkup(new[]
                    {
                      new [] // first row
                      {
                        new KeyboardButton("Меню"),
                      },
                      new [] // last row
                      {
                        new KeyboardButton("[cs.money]"),
                        new KeyboardButton("[loot.farm]"),
                      },
                      new [] // last row
                      {

                        new KeyboardButton("[csgosell.com]"),
                        new KeyboardButton("[cs.deals]")
                      },
                      new [] // last row
                      {
                        new KeyboardButton("[tradeskinsfast.com]"),
                        new KeyboardButton("[cstrade.gg]")
                      },
                      new [] // last row
                      {
                        new KeyboardButton("[csgopolygon.com]"),
                      }

                    });

                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выберите сайт на который необходимо добавить предмет",
                        replyMarkup: keyboard);
                }

                /////////////////////Удаление предмета, вывод сайтов на которые можно добавить предмет/////////////////////////
                else if (message.Text.StartsWith("Remove Items")) // send custom keyboard
                {
                    SetAction(message.From.Username, "Remove");//Устанавливаем Действие Remove чтобы в дальнейшем понять что нужно выполнить (Удалять / Добавлять)

                    var keyboard = new ReplyKeyboardMarkup(new[]
                    {
                    new [] // first row
                    {
                        new KeyboardButton("Меню"),
                    },
                    new [] // last row
                    {
                        new KeyboardButton("[cs.money]"),
                        new KeyboardButton("[loot.farm]")
                    },
                    new [] // last row
                      {

                        new KeyboardButton("[csgosell.com]"),
                        new KeyboardButton("[cs.deals]")
                      },
                      new [] // last row
                      {

                        new KeyboardButton("[tradeskinsfast.com]"),
                        new KeyboardButton("[cstrade.gg]")
                      },
                      new [] // last row
                      {
                        new KeyboardButton("[csgopolygon.com]"),
                      }

                });

                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выберите сайт на котором хотите УДАЛИТЬ предмет.",
                        replyMarkup: keyboard);
                }

                else if (message.Text.StartsWith("[")) //Запись сайта в профиль юзера, для дальнейшей работы
                {
                    switch (message.Text)
                    {
                        case "[cs.money]": {
                                SetSiteName(message.From.Username, "cs.money");
                                if (GetAction(message.From.Username) == "Remove")
                                {
                                    var count = GetToRemove(message.From.Username, "cs.money");
                                    KeyboardButton[][] keys = new KeyboardButton[count.Count + 1][];
                                    int p = 1;
                                    keys[0] = new KeyboardButton[] { "Меню", };
                                    foreach (var item in count)
                                    {
                                        keys[p] = new KeyboardButton[] { "*" + item.Name, };
                                        p++;
                                    }
                                    var keyboard = new ReplyKeyboardMarkup(keys);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выбран Сайт" + message.Text, replyMarkup: keyboard);
                                }
                                else {
                                    var keyboard = new ReplyKeyboardMarkup(new[]
                                    {
                                        new [] // first row
                                        {
                                          new KeyboardButton("Меню"),
                                        }

                                    });
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Введите название предмета начиная  с *", replyMarkup: keyboard);
                                }
                        } break;
                        case "[loot.farm]": {
                                SetSiteName(message.From.Username, "loot.farm");
                                if (GetAction(message.From.Username) == "Remove")
                                {
                                    var count = GetToRemove(message.From.Username, "loot.farm");
                                    KeyboardButton[][] keys = new KeyboardButton[count.Count + 1][];
                                    int p = 1;
                                    keys[0] = new KeyboardButton[] { "Меню", };
                                    foreach (var item in count)
                                    {
                                        keys[p] = new KeyboardButton[] { "*" + item.Name, };
                                        p++;
                                    }
                                    var keyboard = new ReplyKeyboardMarkup(keys);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выбран Сайт" + message.Text, replyMarkup: keyboard);
                                }
                                else
                                {
                                    var keyboard = new ReplyKeyboardMarkup(new[]
                                    {
                                        new [] // first row
                                        {
                                          new KeyboardButton("Меню"),
                                        }

                                    });
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Введите название предмета начиная  с *", replyMarkup: keyboard);
                                }
                            } break;
                        case "[csgosell.com]":{
                                SetSiteName(message.From.Username, "csgosell.com");
                                if (GetAction(message.From.Username) == "Remove")
                                {
                                    var count = GetToRemove(message.From.Username, "csgosell.com");
                                    KeyboardButton[][] keys = new KeyboardButton[count.Count + 1][];
                                    int p = 1;
                                    keys[0] = new KeyboardButton[] { "Меню", };
                                    foreach (var item in count)
                                    {
                                        keys[p] = new KeyboardButton[] { "*" + item.Name, };
                                        p++;
                                    }
                                    var keyboard = new ReplyKeyboardMarkup(keys);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выбран Сайт" + message.Text, replyMarkup: keyboard);
                                }
                                else
                                {
                                    var keyboard = new ReplyKeyboardMarkup(new[]
                                    {
                                        new [] // first row
                                        {
                                          new KeyboardButton("Меню"),
                                        }

                                    });
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Введите название предмета начиная  с *", replyMarkup: keyboard);
                                }
                            } break;
                        case "[cs.deals]":  {
                                SetSiteName(message.From.Username, "cs.deals");
                                if (GetAction(message.From.Username) == "Remove")
                                {
                                    var count = GetToRemove(message.From.Username, "cs.deals");
                                    KeyboardButton[][] keys = new KeyboardButton[count.Count + 1][];
                                    int p = 1;
                                    keys[0] = new KeyboardButton[] { "Меню", };
                                    foreach (var item in count)
                                    {
                                        keys[p] = new KeyboardButton[] { "*" + item.Name, };
                                        p++;
                                    }
                                    var keyboard = new ReplyKeyboardMarkup(keys);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выбран Сайт" + message.Text, replyMarkup: keyboard);
                                }
                                else
                                {
                                    var keyboard = new ReplyKeyboardMarkup(new[]
                                    {
                                        new [] // first row
                                        {
                                          new KeyboardButton("Меню"),
                                        }

                                    });
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Введите название предмета начиная  с *", replyMarkup: keyboard);
                                }
                            } break;
                        case "[tradeskinsfast.com]":{
                                SetSiteName(message.From.Username, "tradeskinsfast.com");
                                if (GetAction(message.From.Username) == "Remove")
                                {
                                    var count = GetToRemove(message.From.Username, "tradeskinsfast.com");
                                    KeyboardButton[][] keys = new KeyboardButton[count.Count + 1][];
                                    int p = 1;
                                    keys[0] = new KeyboardButton[] { "Меню", };
                                    foreach (var item in count)
                                    {
                                        keys[p] = new KeyboardButton[] { "*" + item.Name, };
                                        p++;
                                    }
                                    var keyboard = new ReplyKeyboardMarkup(keys);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выбран Сайт" + message.Text, replyMarkup: keyboard);
                                }
                                else
                                {
                                    var keyboard = new ReplyKeyboardMarkup(new[]
                                    {
                                        new [] // first row
                                        {
                                          new KeyboardButton("Меню"),
                                        }

                                    });
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Введите название предмета начиная  с *", replyMarkup: keyboard);
                                }
                            } break;
                        case "[cstrade.gg]": {
                                SetSiteName(message.From.Username, "cstrade.gg");
                                if (GetAction(message.From.Username) == "Remove")
                                {
                                    var count = GetToRemove(message.From.Username, "cstrade.gg");
                                    KeyboardButton[][] keys = new KeyboardButton[count.Count + 1][];
                                    int p = 1;
                                    keys[0] = new KeyboardButton[] { "Меню", };
                                    foreach (var item in count)
                                    {
                                        keys[p] = new KeyboardButton[] { "*" + item.Name, };
                                        p++;
                                    }
                                    var keyboard = new ReplyKeyboardMarkup(keys);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выбран Сайт" + message.Text, replyMarkup: keyboard);
                                }
                                else
                                {
                                    var keyboard = new ReplyKeyboardMarkup(new[]
                                    {
                                        new [] // first row
                                        {
                                          new KeyboardButton("Меню"),
                                        }

                                    });
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Введите название предмета начиная  с *", replyMarkup: keyboard);
                                }
                            } break;
                        case "[csgopolygon.com]": {
                                SetSiteName(message.From.Username, "csgopolygon.com");
                                if (GetAction(message.From.Username) == "Remove")
                                {
                                    var count = GetToRemove(message.From.Username, "csgopolygon.com");
                                    KeyboardButton[][] keys = new KeyboardButton[count.Count + 1][];
                                    int p = 1;
                                    keys[0] = new KeyboardButton[] { "Меню", };
                                    foreach (var item in count)
                                    {
                                        keys[p] = new KeyboardButton[] { "*" + item.Name, };
                                        p++;
                                    }
                                    var keyboard = new ReplyKeyboardMarkup(keys);
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выбран Сайт" + message.Text, replyMarkup: keyboard);
                                }
                                else
                                {
                                    var keyboard = new ReplyKeyboardMarkup(new[]
                                    {
                                        new [] // first row
                                        {
                                          new KeyboardButton("Меню"),
                                        }

                                    });
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Введите название предмета начиная  с *", replyMarkup: keyboard);
                                }
                            } break;

                    }
                   // await Bot.SendTextMessageAsync(message.Chat.Id, "Выбран Сайт" + message.Text);
                }
                else if (message.Text.StartsWith("*")) //Происходит удаление или добавлении при введении в телеграм названия предмета
                {
                    string action = GetAction(message.From.Username);
                    string site = GetSiteName(message.From.Username);
                    if (action=="Remove")
                    {
                        try
                        {
                            DataStruct.ItemsSearch.Find(n => (n.Name == message.Text.Replace("*", "") && n.Site == site && n.telegram.Contains(message.From.Username))).telegram.Remove(message.From.Username);
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Удалил предмет \n" + message.Text.Replace("*", ""));
                        }
                        catch (Exception ex) { }
                    }
                    else
                    {
                        try
                        {
                            var items = DataStruct.ItemsSearch.Find(N => (N.Name == message.Text.Replace("*", "") && N.Site == site));
                            if (items != null)
                            {
                                items.telegram.Add(message.From.Username);
                                await Bot.SendTextMessageAsync(message.Chat.Id, "Добавил предмет \n" + items.Name);
                            }
                            else
                            {
                                ItemsInfo item = new ItemsInfo()
                                {
                                    Name = message.Text.Replace("*", ""),
                                    Site = site,

                                };
                                item.telegram.Add(message.From.Username);
                                DataStruct.ItemsSearch.Add(item);
                                await Bot.SendTextMessageAsync(message.Chat.Id, "Добавил предмет \n" + item.Name);
                            }
                        }
                        catch (Exception ex) { }

                    }
                }
            }
            else
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, "Извените, но у вас нету доступа к боту,заплатите и будет Счастье))");
            }

        }
        #endregion

        #region SomeMethod
        private static void CheckGoodItems()//отправляю найденные предметы
        {
            while (true)
            {
                try
                {
                  //  Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "--|Количество хороших предметов:"+ DataStruct.GoodItems.Count);
                    if (DataStruct.GoodItems.Count > 0)
                    {
                        var it = DataStruct.GoodItems.Dequeue();
                        foreach (var item in it.telegram)
                        {
                            if (CheckTemsGood(it.Name, item) == true)
                            {
                                long id = GetIdChat(item);
                                SendMessage(id, it.Site + "\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "\n" + it.Name);
                            }

                        }
                    }
                }
                catch (Exception ex) { }
                Thread.Sleep(300);
            }
        }
        private static bool CheckTemsGood(string NameItems, string _Telegram)
        {
            try
            {
                var itm = TempGodItems.Find(N => (N.Name == NameItems && N.telegram == _Telegram));
                if (itm == null)
                {
                    TempGodItems.Add(new Types.TempGood { Name = NameItems, telegram = _Telegram, DateItems = Convert.ToInt32(DateTime.Now.ToString("HHmm")) });
                    return true;
                }
                else
                {
                    if (Convert.ToInt32(DateTime.Now.ToString("HHmm")) - itm.DateItems > 1)
                    {
                        TempGodItems.Remove(itm);
                        TempGodItems.Add(new Types.TempGood { Name = NameItems, telegram = _Telegram, DateItems = Convert.ToInt32(DateTime.Now.ToString("HHmm")) });
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex) { }
            return false;
        }
        private static void UserGetFromFile()//забираю сохранный список юзеров телеграм
        {
            string lk = System.IO.File.ReadAllText("./Config/UserSetting.data");
            if (lk != "")
            { 
            User = JsonConvert.DeserializeObject<List<UserTelegram>>(lk);
            }
        }

        private static void UserSetToFile() //записываю новые изменения списков юзеров телеграм
        {
            while (true)
            {
                string json = JsonConvert.SerializeObject(User);
                System.IO.File.WriteAllText("./Config/UserSetting.data", json);
                Thread.Sleep(300000);
            }
        }

        private static async void  SendMessage(long id ,string text) // функция отправки сообщения в телеграм
        {
            await Bot.SendTextMessageAsync(id, text);
        }

        private static void FirstSetUserNameTeleg()//Первая запись юзеров
        {
            string[] nameUser = System.IO.File.ReadAllLines("./userTelega/userName.data");
            foreach (var name in nameUser)
            {
                UserTelegram UIT = new UserTelegram
                {
                    Name = name
                };
                User.Add(UIT);
            }
        }

        private static void SetSiteName(string userName, string NameSite)//Устанавливаем значения Названия сайта для дальнейших действий
        {
            User.Find(N => (N.Name == userName)).SiteName = NameSite;
        }

        private static void SetAction(string userName, string Action)//Устанавливаем значения Действия для юзера
        {
            User.Find(N => (N.Name == userName)).Action = Action;
        }

        private static string GetSiteName(string TelegramName)//Берем названия сайта у юзера
        {
            return User.Find(N => (N.Name == TelegramName)).SiteName;
        }

        private static string GetAction(string TelegramName)//Берем екшен у юзера
        {
            return User.Find(N => (N.Name == TelegramName)).Action;
        }

        private static List<ItemsInfo> GetToRemove(string TelegramName,string Site)//возвращаем все предметы в список для удаления
        {
            return DataStruct.ItemsSearch.FindAll(N => (N.telegram.Contains( TelegramName)==true && N.Site == Site));
        }

        private static long GetIdChat(string Name)//Берем Айди чата через имя телеграм
        {
            return User.Find(N => (N.Name == Name)).IdChat;
        }

        public static void AddUser(string _Name)//Добавляем нового юзера телеграм
        {
            UserTelegram UIT = new UserTelegram
            {
                Name = _Name
            };
            User.Add(UIT);
        }

        public static void RemoveUser(string _Name)//Удаляем юзера телеграм
        {  
            User.RemoveAll(N=>(N.Name == _Name));
        }
        #endregion

    }
}

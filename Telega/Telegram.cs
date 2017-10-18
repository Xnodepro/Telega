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

namespace Telega
{
    static class Telegram
    {
        #region Fields
        private static List<UserTelegram> User = new List<UserTelegram>();
        private static readonly TelegramBotClient Bot = new TelegramBotClient("477705043:AAGeGPgItwxfGrGfXF8W3nJlqWUhJq3lDco");
        #endregion

        #region TelegaBotMethod
        public static void Start()
        {
            FirstSetUserNameTeleg();//подгружаем список ников телеграма
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

                    await Bot.SendTextMessageAsync(message.Chat.Id, "",
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
                        new KeyboardButton("[loot.farm]"),
                    }

                });

                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выберите сайт на котором хотите УДАЛИТЬ предмет.",
                        replyMarkup: keyboard);
                }

                else if (message.Text.StartsWith("[")) //Запись сайта в профиль юзера, для дальнейшей работы
                {
                    switch (message.Text)
                    {
                        case "[cs.money]": { SetSiteName(message.From.Username, "cs.money"); } break;
                        case "[loot.farm]": { SetSiteName(message.From.Username, "loot.farm"); } break;

                    }
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Выбран Сайт"+ message.Text);
                }
            }
            else
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, "Извените, но у вас нету доступа к боту,заплатите и будет Счастье))");
            }

        }
        #endregion

        #region SomeMethod
        private static void FirstSetUserNameTeleg()
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


        private static void SetSiteName(string userName, string NameSite)
        {
            User.Find(N => (N.Name == userName)).SiteName = NameSite;
        }


        private static void SetAction(string userName, string Action)
        {
            User.Find(N => (N.Name == userName)).Action = Action;
        }
        #endregion

    }
}

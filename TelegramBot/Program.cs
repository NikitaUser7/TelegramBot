using System;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Linq;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace TelegramBot
{
    class Program
    {
        private static ITelegramBotClient bot;
        private static TestDBContext _TestDB;
        static void Main(string[] args)
        {
            var token = "2088298335:AAF8kMX_k-muZfxgj6Lx1XsP8HpCeUdWvyY";
            bot = new TelegramBotClient(token);
            bot.OnMessage += Bot_OnMessage;
            bot.OnMessageEdited += Bot_OnMessage;
            bot.StartReceiving();
            Console.ReadKey();
        }

        static Regex phoneRegex = new Regex(@"^\+?[0-9]{3}-?[0-9]{6,12}$", RegexOptions.Singleline);

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            _TestDB = new TestDBContext();
            var textType = e.Message.Type;
            var textWithKey = e.Message.Text;
            string text = "";

            var chatId = e.Message.Chat.Id;
            if (textType.ToString() == "Text")
            {
                var key = textWithKey.Last();
                char[] textCh = new char[textWithKey.Length - 1];
                for (int a = 0; a < textWithKey.Length - 1; a++)
                {
                    textCh[a] = textWithKey[a];
                }
                text = new string(textCh);
                if (phoneRegex.IsMatch(text) && (key == 'W' || key == 'M'))
                {

                    await bot.SendTextMessageAsync(
                       chatId: chatId,
                       text: $"Вы ввели номер телефона: {text}"
                      );
                    await bot.SendTextMessageAsync(
                       chatId: chatId,
                       text: "Инфорамация скоро будет..."
                      );
                    await bot.SendTextMessageAsync(
                       chatId: chatId,
                       text: "А вот и я!"
                      );

                    if (key == 'M')
                    {
                        await bot.SendTextMessageAsync(
                       chatId: chatId,
                       text: "Информация по пользователю за Май месяц:"
                      );
                        await bot.SendTextMessageAsync(
                                               chatId: chatId,
                                               text: $"Количество чеков за владельцем этого номера на Май месяц числится: {_TestDB.TblTransactions.Where(d => d.UserPhoneNumber == text && d.CheckDate.Value.Month == 5).Count()}"
                                              );
                        await bot.SendTextMessageAsync(
                           chatId: chatId,
                           text: $"Количество товаров куплено: {_TestDB.TblTransactions.Where(d => d.UserPhoneNumber == text && d.CheckDate.Value.Month == 5).Select(b => b.CheckCountSku).ToArray().Sum()}"
                          );
                        await bot.SendTextMessageAsync(
                           chatId: chatId,
                           text: $"Заработано за месяц бонусов: {_TestDB.TblCheckBonusesUsers.Where(d => d.UserPhoneNumber == text && d.AddBonusesDate.Month == 5).Select(b => b.UserBonuses).ToArray().Sum()}"
                          );
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(
                       chatId: chatId,
                       text: "Информация по пользователю за последнюю неделю:"
                      );
                        var maxDate = _TestDB.TblTransactions.OrderByDescending(t => t.CheckDate).FirstOrDefault().CheckDate;
                        var chekDate = maxDate.Value.AddDays(-7);
                        await bot.SendTextMessageAsync(
                                               chatId: chatId,
                                               text: $"Количество чеков за последнюю неделю: {_TestDB.TblTransactions.Where(d => d.UserPhoneNumber == text && d.CheckDate.Value > chekDate && d.CheckDate.Value <= maxDate).Count()}"
                                              );
                        await bot.SendTextMessageAsync(
                           chatId: chatId,
                           text: $"Количество товаров куплено: {_TestDB.TblTransactions.Where(d => d.UserPhoneNumber == text && d.CheckDate.Value > chekDate && d.CheckDate.Value <= maxDate).Select(b => b.CheckCountSku).ToArray().Sum()}"
                          );
                        await bot.SendTextMessageAsync(
                           chatId: chatId,
                           text: $"Заработано за месяц бонусов: {_TestDB.TblCheckBonusesUsers.Where(d => d.UserPhoneNumber == text && d.AddBonusesDate > chekDate && d.AddBonusesDate <= maxDate).Select(b => b.UserBonuses).ToArray().Sum()}"
                          );
                    }

                }
                else
                {
                    await bot.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Введите пожалуйста номер телефона интересующего вас пользователя, обязательно укажите модификаторы 'W' или 'M' в плотную к номеру телефона, в конце. Я выдам вам информацию, которая у меня связана с этим номером"
                        );

                }
            }
            else
            {
                await bot.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Введите пожалуйста номер телефона интересующего вас пользователя, обязательно укажите модификаторы 'W' или 'M' в плотную к номеру телефона, в конце. Я выдам вам информацию, которая у меня связана с этим номером"
                        );
            }



        }
    }
}

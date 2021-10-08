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
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace TelegramBot
{
    class Program
    {
        private static ITelegramBotClient bot;
        private static SqlConnection connection;
        static async Task Main(string[] args)
        {
            string connectionString = "Server=MiniComp\\MSSQLSWRVER;Database=TestDB;Trusted_Connection=True; Pooling = true";

            // Создание подключения
            connection = new SqlConnection(connectionString);
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
            if(connection.State == ConnectionState.Open)
            {
            await connection.CloseAsync();
            }

            await connection.OpenAsync();
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
                        SqlCommand selectCountChecksM = new SqlCommand("SELECT CONVERT(VARCHAR(10), COUNT(check_date) , 25) AS [YYYY/MM/DD] FROM tblTransactions WHERE UserPhoneNumber=" + text + "AND CONVERT(VARCHAR(10), (check_date) , 25) BETWEEN '2021-05-10' AND '2021-06-01'", connection);
                        var CountM = selectCountChecksM.ExecuteScalar();
                        await bot.SendTextMessageAsync(
                                               chatId: chatId,
                                               text: $"Количество чеков за владельцем этого номера на Май месяц числится: {CountM}"
                                              );
                        CountM = null;
                        SqlCommand selectSumSkuM = new SqlCommand("SELECT SUM(check_count_sku) FROM tblTransactions WHERE UserPhoneNumber='"+ text +"' AND CONVERT(VARCHAR(10), (check_date) , 25) BETWEEN '2021-05-10' AND '2021-06-01'", connection);
                        var SumM = selectSumSkuM.ExecuteScalar();
                        await bot.SendTextMessageAsync(
                           chatId: chatId,
                           text: $"Количество товаров куплено: {SumM}"
                          );
                        SumM = null;
                        SqlCommand selectSumBonusM = new SqlCommand("SELECT SUM(UserBonuses) FROM tblCheckBonusesUser WHERE UserPhoneNumber='" + text + "' AND CONVERT(VARCHAR(10), (AddBonusesDate) , 25) BETWEEN '2021-05-10' AND '2021-06-01'", connection);
                        var BonusSumM = selectSumBonusM.ExecuteScalar();
                        await bot.SendTextMessageAsync(
                           chatId: chatId,
                           text: $"Заработано за месяц бонусов: {BonusSumM}"
                          );
                        BonusSumM = null;
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(
                       chatId: chatId,
                       text: "Информация по пользователю за последнюю неделю:"
                      );
                        SqlCommand getLastDate = new SqlCommand("SELECT MAX (AddBonusesDate) FROM tblCheckBonusesUser", connection);
                        var LastDate = getLastDate.ExecuteScalar();
                        SqlCommand selectCountChecksW = new SqlCommand("SELECT COUNT(check_date) FROM tblTransactions WHERE UserPhoneNumber='" + text + "'AND CONVERT(VARCHAR(10), check_date, 25) BETWEEN CONVERT(VARCHAR(10), DATEADD(DAY, -7, '" + LastDate + "'), 25) AND CONVERT(VARCHAR(10), '" + LastDate + "' , 25)", connection);
                        var CountW = selectCountChecksW.ExecuteScalar();
                        await bot.SendTextMessageAsync(
                                               chatId: chatId,
                                               text: $"Количество чеков за последнюю неделю: {CountW}"
                                              );
                        SqlCommand selectSumSkuW = new SqlCommand("SELECT SUM(check_count_sku) FROM tblTransactions WHERE UserPhoneNumber='"+ text + "' AND CONVERT(VARCHAR(10), (check_date) , 25) BETWEEN CONVERT(VARCHAR(10), DATEADD(DAY, -7, '" + LastDate + "'), 25) AND CONVERT(VARCHAR(10), '" + LastDate + "' , 25)", connection);
                        var SumW = selectSumSkuW.ExecuteScalar();
                        await bot.SendTextMessageAsync(
                           chatId: chatId,
                           text: $"Количество товаров куплено: {SumW}"
                          );
                        SqlCommand selectSumBonusW = new SqlCommand("SELECT SUM(UserBonuses) FROM tblCheckBonusesUser WHERE UserPhoneNumber='" + text + "' AND CONVERT(VARCHAR(10), (AddBonusesDate) , 25) BETWEEN CONVERT(VARCHAR(10), DATEADD(DAY, -7, '" + LastDate + "'), 25) AND CONVERT(VARCHAR(10), '" + LastDate + "' , 25)", connection);
                        var BonusSumW = selectSumBonusW.ExecuteScalar();
                        
                        await bot.SendTextMessageAsync(
                           chatId: chatId,
                           text: $"Заработано за месяц бонусов: {BonusSumW}"
                          );
                        await connection.CloseAsync();
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

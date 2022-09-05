using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace HomeWork10WPF
{
    public class MyTelMess
    {
        public MainWindow Mwin { get; set; } // Привязка к MainWindow
        public string path = "Msg.json";
        public static ObservableCollection<MsgLog> MsgLogs { get; set; } = new ObservableCollection<MsgLog>();  // Коллекция
        public static string token = "";                          //Секретный токен
        public TelegramBotClient client = new TelegramBotClient(token);                                         // Телеграм клиент

        public void StartTel()                        // Создаём метод для того чтоб проинициализироваться в классе MainWindow при запуске
        {
            client.StartReceiving(
                         HandleUpdatesAsync,                                               //(Метод) Обработка обновлений
                         HandleArorAsyns,                                                  //(Метод) Обработка ошибок
                         new ReceiverOptions{AllowedUpdates = {}},                         // Настройка получения обновлений
                         cancellationToken: new CancellationTokenSource().Token);          // Токен отмены
        }
        public async Task HandleUpdatesAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)  //Обработка событий
        {
            if (!(update.Type == UpdateType.Message && update.Message.Text != null))            // если пришло не текстовое сообщение, то игнор
                return;

            var ml = new MsgLog
            {
                Name = update.Message.From.FirstName,                     // Создаём экземпляр класса и вносим значения в автосвойства
                Id = update.Message.Chat.Id,                              //
                Date = DateTime.Now.ToShortDateString(),                  //
                MessageT = update.Message.Text                            //
            };                                                            //

            Mwin.Dispatcher.Invoke(() => {MsgLogs.Add(ml);});               //добавляем сообщение в коллекцию, отображаем её
            
            using (FileStream fs = new FileStream(path, FileMode.Append))         // Сохраняем в jeson формате в файл
            {                                                                     //
                string jsonUser = JsonConvert.SerializeObject(ml);                //
                using (StreamWriter stream = new StreamWriter(fs))                //                                                               
                    stream.WriteLine(jsonUser);                                   //                                                              
            }                                                                     //
        }
        public Task HandleArorAsyns(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)  // Обработка ошибок
        {
            MessageBox.Show(exception.Message);
            return Task.CompletedTask;
        }

    }

}

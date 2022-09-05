using Telegram.Bot;
using System.Windows;
using System;

namespace HomeWork10WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MyTelMess myTelMess;                         // Ссылка на телеграм класс
        public MainWindow()
        {
            myTelMess = new MyTelMess();             // Создаём экземпляр 
            myTelMess.Mwin = this;
            InitializeComponent();
            myTelMess.StartTel();                          // Метод для запуска обновлений с класса телеграм

            MessList.ItemsSource = MyTelMess.MsgLogs;               // Связываем ListBox с классом MsLog

        }


        private void Button_Click(object sender, RoutedEventArgs e)         // Обработка события при клике на кнопку
        {            
            if (textBlockId.Text == "")
            {
                MessageBox.Show("Выберите Id");
                return;                    
            }

            long id = Convert.ToInt64(textBlockId.Text);
            
            myTelMess.client.SendTextMessageAsync(id, $"{textBox.Text}");
            textBox.Text = "";
        }
    }
}

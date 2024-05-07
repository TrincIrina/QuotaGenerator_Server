using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuotaGenerator_App
{
    /// <summary>
    /// Логика взаимодействия для QuoteWindow.xaml
    /// </summary>
    public partial class QuoteWindow : Window
    {
        private Socket _client;
        private byte[] buf = new byte[1024];
        private int bytesReceived;
        private string reply;
        private string command;
        public QuoteWindow(Socket client)
        {
            InitializeComponent();
            _client = client;

            bytesReceived = client.Receive(buf);
            reply = Encoding.UTF8.GetString(buf, 0, bytesReceived);
            MessageBox.Show(reply);
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            // 1) отправить команду
            command = FileButton.Content.ToString();
            _client.Send(Encoding.UTF8.GetBytes(command));
            // 2) получить ответ
            bytesReceived = _client.Receive(buf);
            reply = Encoding.UTF8.GetString(buf, 0, bytesReceived);
            OpenQuotaGenerator(FileButton.Content.ToString(), reply);
        }

        private void DatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            // 1) отправить команду
            command = DatabaseButton.Content.ToString();
            _client.Send(Encoding.UTF8.GetBytes(command));
            // 2) получить ответ
            bytesReceived = _client.Receive(buf);
            reply = Encoding.UTF8.GetString(buf, 0, bytesReceived);
            OpenQuotaGenerator(DatabaseButton.Content.ToString(), reply);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            _client.Shutdown(SocketShutdown.Both);   // завершить общение в 2 стороны
            _client.Close();
            Close();
        }

        // Вспомогательный метод - открытие окна QuotaGenerator
        private void OpenQuotaGenerator(string s, string q)
        {
            QuotaGenerator generator = new QuotaGenerator(s, q);
            Hide();
            generator.ShowDialog();
            Show();
        }
    }
}

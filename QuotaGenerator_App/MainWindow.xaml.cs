using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuotaGenerator_App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // ip адрес и порт сокета сервера
        private string serverIpStr;
        private int serverPort;

        private IPAddress serverIp;
        private IPEndPoint serverEndpoint;
        private Socket client;
        public MainWindow()
        {
            InitializeComponent();            
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            serverIpStr = IpTextBox.Text;
            serverPort = Convert.ToInt32(PortTextBox.Text);

            // 1. подготовить endpoint сервера
            serverIp = IPAddress.Parse(serverIpStr);
            serverEndpoint = new IPEndPoint(serverIp, serverPort);

            // 2. создать сокет клиента
            client = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.IP
            );

            // 3. иницировать подключение к серверу            
            client.Connect(serverEndpoint);
            QuoteWindow window = new QuoteWindow(client);
            Hide();
            window.ShowDialog();
            Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

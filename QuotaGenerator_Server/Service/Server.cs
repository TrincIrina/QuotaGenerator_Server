using QuotaGenerator_Server.Quotas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuotaGenerator_Server.Service
{
    // Server - реализация многопользовательского сервера генератора цитат
    internal class Server
    {
        private readonly Socket _serverSocket;  // сокет сервера
        private readonly int _maxClientCount;   // максимальное кол-во клиентов
        private int connectedClientCount;       // кол-во подключенных клиентов
        private readonly ClientProcessor _clientProcessor;  // обработчик клиентов

        // конструктор
        public Server(string serverIpStr, int serverPort, int maxClientCount, 
            IQuotaGenerator quota_1, IQuotaGenerator quota_2)
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIpStr), serverPort);
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(serverEndPoint);
            //
            _maxClientCount = maxClientCount;
            // прослушиваем на единицу больше, чем максимальное число клиентов
            _serverSocket.Listen(maxClientCount + 1);
            //
            _clientProcessor = new ClientProcessor(quota_1, quota_2, OnClientDisconnected);
            Console.WriteLine($"[server]> server started on {serverIpStr}:{serverPort}");
        }

        // Run - запуск сервера (вечный цикл)
        public void Run()
        {
            while (true)
            {
                // 1. ожидание очередного подключения
                Console.WriteLine($"[server]> waiting for incoming connections");
                Socket client = _serverSocket.Accept();
                // 2. проверить достаточно ли мест (потокобезопасным способом)
                lock (this)
                {
                    string message;
                    if (connectedClientCount >= _maxClientCount)
                    {
                        // если места не достаточно, то отправить ответ клиенту и отключить его
                        message = $"Too many clients connected ({connectedClientCount} / {_maxClientCount}), you ({client.RemoteEndPoint}) will be disconnected.";
                        Console.WriteLine($"[server]> {message}");
                        client.Send(Encoding.UTF8.GetBytes(message));
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                        continue;
                    }
                    // создать ClientProcessor и отправить клиента в работу
                    connectedClientCount++;
                    message = $"You ({client.RemoteEndPoint}) connected to server ({connectedClientCount} / {_maxClientCount}).";
                    Console.WriteLine($"[server]> {message}");
                    client.Send(Encoding.UTF8.GetBytes(message));
                    // запустить обработку клиента в отдельном потоке
                    ThreadPool.QueueUserWorkItem(_ => _clientProcessor.ProcessClient(client));
                }
            }
        }

        // колбэк срабатывающий при отключении клиента
        private void OnClientDisconnected(Socket client)
        {
            // уменьшить кол-во подключенных клиентов
            client.Shutdown(SocketShutdown.Both);
            lock (this)
            {
                connectedClientCount--;
            }
            Console.WriteLine($"[server]> disconnected client {client.RemoteEndPoint} " +
                $"({connectedClientCount} / {_maxClientCount})");
        }
    }
}

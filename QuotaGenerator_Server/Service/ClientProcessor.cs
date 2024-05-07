using QuotaGenerator_Server.Communication;
using QuotaGenerator_Server.Quotas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuotaGenerator_Server.Service
{
    internal class ClientProcessor
    {
        private readonly IQuotaGenerator _quotaFile;
        private readonly IQuotaGenerator _quotaDatabase;
        private Action<Socket> onDisconnect; // делегат, хранящий колбэк отключения клиента
        public ClientProcessor(IQuotaGenerator quotaFile, IQuotaGenerator quotaDatabase, 
            Action<Socket> onDisconnectedEventHandler)
        {
            _quotaFile = quotaFile;
            _quotaDatabase = quotaDatabase;
            onDisconnect = onDisconnectedEventHandler;
        }

        // ProcessClient - цикл обработки одного клиента
        public void ProcessClient(Socket client)
        {
            using (ICommunicator communicator = new FixedBufferCommunicator(client))
            {
                try
                {
                    bool isExit = false;
                    // цикл обработки клиента
                    while (!isExit)
                    {
                        string message = communicator.Receive();
                        switch (message)
                        {                            
                            case "positive quote":
                                communicator.Send(_quotaFile.GetNextQuota());
                                break;
                            case "funny quote":
                                communicator.Send(_quotaDatabase.GetNextQuota());
                                break;
                            case "exit":
                                communicator.Send("Bye-bye");
                                isExit = true;
                                break;
                            default:
                                communicator.Send("Unknown command");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[server[clientProcessor]]> ex occurred: {ex.Message}");
                }
                finally
                {
                    // завершить работу клиента
                    onDisconnect(client);
                }
            }
        }

    }
}

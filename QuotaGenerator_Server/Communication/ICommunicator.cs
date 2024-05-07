using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotaGenerator_Server.Communication
{
    // ICommunicator - интерфейс для общения по сети
    internal interface ICommunicator : IDisposable
    {
        // Send - отправить строковое сообщение
        void Send(string message);

        // Receive - получить строковое сообщение
        string Receive();
    }
}

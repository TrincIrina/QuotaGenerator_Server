using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuotaGenerator_Server.Communication
{
    // FixedBufferCommunicator - коммуникатор работающий через сокет и фиксированный буфер
    internal class FixedBufferCommunicator : ICommunicator
    {
        private Socket socket;      // сокет для общения
        private byte[] buffer;      // буфер для общения
        private bool isDisposed;    // флаг очистки 

        public FixedBufferCommunicator(Socket socket, int bufferSize = 1024)
        {
            this.socket = socket;
            buffer = new byte[bufferSize];
            isDisposed = false;
        }

        public string Receive()
        {
            CheckDisposed();
            int bytesReceived = socket.Receive(buffer);
            return Encoding.UTF8.GetString(buffer, 0, bytesReceived);
        }

        public void Send(string message)
        {
            CheckDisposed();
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            if (messageBytes.Length > buffer.Length)
            {
                throw new InvalidOperationException("too long message");
            }
            socket.Send(messageBytes);
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                socket.Dispose();
                isDisposed = true;
            }
        }

        // CheckDisposed - проверка того, что коммуникатор не закрыли
        private void CheckDisposed()
        {
            if (isDisposed)
            {
                throw new InvalidOperationException("communicator already disposed");
            }
        }
    }
}

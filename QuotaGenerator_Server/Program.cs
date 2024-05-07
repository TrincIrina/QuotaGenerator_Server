using QuotaGenerator_Server.Quotas;
using QuotaGenerator_Server.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotaGenerator_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IQuotaGenerator fileQuote = new FileQuotes();
            IQuotaGenerator databaseQuote = new RdbQuotaRepository();
            Server server = new Server("127.0.0.1", 1024, 2, fileQuote, databaseQuote);
            server.Run();
        }
    }
}

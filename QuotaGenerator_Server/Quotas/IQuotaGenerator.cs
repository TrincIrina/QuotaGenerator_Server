using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotaGenerator_Server.Quotas
{
    // IQuotaGenerator - интерфейс генератора цитата
    internal interface IQuotaGenerator
    {
        // GetNextQuota - метод получения очередной случайной цитаты
        string GetNextQuota();
    }
}

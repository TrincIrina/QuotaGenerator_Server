using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotaGenerator_Server.Quotas
{
    // Класс для работы с файлом, содержащим цитаты
    internal class FileQuotes : IQuotaGenerator
    {
        // Путь к файлу
        private readonly string path = "Короткие позитивные цитаты.txt";
        private List<string> list = new List<string>();
        private Random r = new Random();
        // Конструктор
        public FileQuotes()
        {            
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    list.Add(sr.ReadLine());
                }
            }
        }
        // Метод выделения случайной цитаты
        public string GetNextQuota()
        {            
            string[] quotes = new string[list.Count];
            for (int i = 0; i < quotes.Length; i++)
            {
                quotes[i] = list[i];
            }
            int id = r.Next(quotes.Length);
            string str = quotes[id];
            Quota quota = new Quota()
            {
                Id = id,
                Quote = str.Split(':')[0],
                Author = str.Split(':')[1]
            };
            return quota.ToString();
        }        
    }
}

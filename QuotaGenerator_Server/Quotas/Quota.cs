using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotaGenerator_Server.Quotas
{
    // Класс, описывающий цитату
    internal class Quota
    {
        // id в базе данных
        public int Id { get; set; }
        // Текст цитаты
        public string Quote { get; set; }
        // Автор цитаты
        public string Author { get; set; }
        // Конструктор
        public Quota()
        {
            Quote = string.Empty;
            Author = string.Empty;
        }
        // Метод вывода
        public override string ToString()
        {
            return $"{Quote} - {Author}";
        }
    }
}

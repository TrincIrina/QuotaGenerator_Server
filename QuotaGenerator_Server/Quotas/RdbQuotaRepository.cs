using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotaGenerator_Server.Quotas
{
    internal class RdbQuotaRepository : IQuotaGenerator
    {
        private readonly string connectionString;
        private Random r = new Random();
        public RdbQuotaRepository()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        // вспомогательный приватный метод для открытия соединения к БД
        private SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        // Метод выделения случайной цитаты
        public string GetNextQuota()
        {
            int count = CountQuotes();
            int id = r.Next(count);
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Quotes WHERE id = {id};", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        throw new InvalidOperationException($"no rows with id = {id}");
                    }
                    reader.Read();
                    Quota quota = new Quota()
                    {
                        Id = reader.GetInt32(0),
                        Quote = reader.GetString(1),
                        Author = reader.GetString(2)
                    };
                    return quota.ToString();
                }
            }
        }
        // вспомогательный метод - возвращает количество цитат в БД
        public int CountQuotes()
        {
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(id) FROM Quotes;", connection);
                return (int)command.ExecuteScalar();
            }
        }
    }
}

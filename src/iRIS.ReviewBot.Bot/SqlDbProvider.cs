using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;

namespace iRIS.ReviewBot.Bot
{
    using iRIS.ReviewBot.Bot.Entities;

    public class SqlDbProvider : IDataProvider
    {
        private readonly string _connectionString;

        public SqlDbProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Add(ChatMember member)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Insert(member);
            }
        }

        public void Delete(ChatMember member)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Delete(member);
            }
        }

        public ChatMember Get(string chatId, string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<ChatMember>()
                    .Where(_ => string.Equals(_.ChatId, chatId, StringComparison.OrdinalIgnoreCase) && string.Equals(_.Name, name, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
            }
        }

        public List<ChatMember> GetAll(string chatId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<ChatMember>()
                    .Where(_ => string.Equals(_.ChatId, chatId, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        public void Update(ChatMember member)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Update(member);
            }
        }
    }
}

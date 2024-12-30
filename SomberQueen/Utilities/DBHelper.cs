using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Npgsql;

namespace SomberQueen.Utilities
{
    public class DBHelper
    {
        private readonly string _connectionString;

        public DBHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Execute(string query, object parameters = null)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Execute(query, parameters);
            }
        }

        public IEnumerable<T> Query<T>(string query, object parameters = null)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<T>(query, parameters);
            }
        }

        public T QueryFirstOrDefault<T>(string query, object parameters = null)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(query, parameters);
            }
        }

        public void ExecuteProcedure(string procedureName, object parameters = null)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool TestConnection()
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    return true; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veritabanı bağlantı hatası: {ex.Message}");
                return false;
            }
        }

    }
}

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace HotelManagement.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
            }
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                connection.Open();
                var dataTable = new DataTable();
                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
                return dataTable;
            }
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                connection.Open();
                return command.ExecuteScalar();
            }
        }

        public DataTable ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                connection.Open();
                var dataTable = new DataTable();
                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
                return dataTable;
            }
        }

        public byte[] GetImageBytes(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}
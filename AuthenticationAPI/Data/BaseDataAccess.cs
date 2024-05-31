﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace AuthenticationAPI.Data
{
    public class BaseDataAccess
    {
        protected SqlConnection connection;

        private static string connectionString =
            "Server=tcp:advancedbank.database.windows.net,1433;Initial Catalog=advancedBankdb;Persist Security Info=False;User ID=advancedbankadmin;Password=Advanced@banking;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
          //@"Server=(local);database=AdvancedBanking;integrated security=sspi;trustservercertificate=true;multipleactiveresultsets=true";


        protected void OpenConnection()
        {
            if (connection is null)
            {
                connection = new SqlConnection(connectionString);
            }
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }
        public void CloseConnection()
        {
            if (connection is not null)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Close();
            }
        }

        public SqlDataReader ExecuteReader(string sqltext, CommandType commandType, params SqlParameter[] parameters)
        {
            OpenConnection();
            var command = connection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = sqltext;
            if (parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }
            return command.ExecuteReader();
        }
        public void ExecuteNonQuery(string sqltext, CommandType commandType, params SqlParameter[] parameters)
        {
            OpenConnection();
            var command = connection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = sqltext;
            if (parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }
            command.ExecuteNonQuery();
            return;
        }
    }
}

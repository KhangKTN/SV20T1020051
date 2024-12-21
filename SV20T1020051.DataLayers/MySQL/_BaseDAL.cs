using Microsoft.Data.SqlClient;

namespace SV20T1020051.DataLayers.MySQL
{
    public abstract class _BaseDAL
    {
        protected string _connectionString = "";

        public _BaseDAL(string connectionString) { 
            _connectionString = connectionString;
        }

        public SqlConnection OpenConnection()
        {
            //MySqlConnection connection = new MySqlConnection();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }
    }
}

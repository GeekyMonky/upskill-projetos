using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace LibDB
{
    public static class DALPro
    {
        public static string ConnectionString { get; set; }

        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        public static int Execute(string sql, Dictionary<string, object> parameters = null, SqlTransaction trans = null)
        {
            SqlConnection conn = trans?.Connection ?? GetConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                {
                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);

                    return cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (trans == null) conn.Close();
            }
        }

        public static object ExecuteScalar(string sql, Dictionary<string, object> parameters = null, SqlTransaction trans = null)
        {
            SqlConnection conn = trans?.Connection ?? GetConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                {
                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);

                    return cmd.ExecuteScalar();
                }
            }
            finally
            {
                if (trans == null) conn.Close();
            }
        }

        public static List<T> Query<T>(string sql, Dictionary<string, object> parameters = null, SqlTransaction trans = null) where T : new()
        {
            SqlConnection conn = trans?.Connection ?? GetConnection();
            List<T> resultado = new List<T>();

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                {
                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        PropertyInfo[] props = typeof(T).GetProperties();

                        while (reader.Read())
                        {
                            T item = new T();
                            foreach (var prop in props)
                            {
                                if (HasColumn(reader, prop.Name) && reader[prop.Name] != DBNull.Value)
                                    prop.SetValue(item, reader[prop.Name]);
                            }
                            resultado.Add(item);
                        }
                    }
                }
            }
            finally
            {
                if (trans == null) conn.Close();
            }

            return resultado;
        }

        public static int ExecuteSP(string sp, Dictionary<string, object> parameters = null, SqlTransaction trans = null)
        {
            SqlConnection conn = trans?.Connection ?? GetConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(sp, conn, trans))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);

                    return cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (trans == null) conn.Close();
            }
        }

        public static SqlTransaction BeginTransaction()
        {
            SqlConnection conn = GetConnection();
            return conn.BeginTransaction();
        }

        public static void Commit(SqlTransaction trans)
        {
            SqlConnection conn = trans.Connection;
            trans.Commit();
            conn.Close();
        }

        public static void Rollback(SqlTransaction trans)
        {
            SqlConnection conn = trans.Connection;
            trans.Rollback();
            conn.Close();
        }

        private static bool HasColumn(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }
    }
}
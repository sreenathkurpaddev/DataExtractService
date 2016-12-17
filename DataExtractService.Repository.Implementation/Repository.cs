using DataExtractService.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractService.Repository.Implementation
{
    public class Repository : IRepository
    {
        public void Delete(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                //1. Get the SqlCommand
                SqlCommand command = GetSQLCommand(procedureName, parameters);

                //2. Use SQL Connection and execute the query
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    command.Connection = sqlConnection;
                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteAsync(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                //1. Get the SqlCommand
                SqlCommand command = GetSQLCommand(procedureName, parameters);

                //2. Use SQL Connection and execute the query
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    command.Connection = sqlConnection;
                    await sqlConnection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public object Get(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                //1. Get the SqlCommand 
                SqlCommand command = GetSQLCommand(procedureName, parameters);

                //2.  Use SQl Connection and execute the query
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    command.Connection = sqlConnection;
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        sqlConnection.Open();
                        adapter.Fill(ds);
                        if (ds.Tables != null && ds.Tables.Count > 0)
                            return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        public async Task<object> GetAsync(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                //1. Get the SqlCommand 
                SqlCommand command = GetSQLCommand(procedureName, parameters);

                //2.  Use SQl Connection and execute the query
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    command.Connection = sqlConnection;
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        await sqlConnection.OpenAsync();
                        adapter.Fill(ds);
                        if (ds.Tables != null && ds.Tables.Count > 0)
                            return ds;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        public void Insert(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                //1. Get the SqlCommand 
                SqlCommand command = GetSQLCommand(procedureName, parameters);

                //2.  Use SQl Connection and execute the query
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    command.Connection = sqlConnection;
                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task InsertAsync(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                //1. Get the SqlCommand 
                SqlCommand command = GetSQLCommand(procedureName, parameters);

                //2.  Use SQl Connection and execute the query
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    command.Connection = sqlConnection;
                    await sqlConnection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Update(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                //1. Get the SqlCommand
                SqlCommand command = GetSQLCommand(procedureName, parameters);

                //2. Use SQL Connection and execute the query
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    command.Connection = sqlConnection;
                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateAsync(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                //1. Get the SqlCommand
                SqlCommand command = GetSQLCommand(procedureName, parameters);

                //2. Use SQL Connection and execute the query
                using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    command.Connection = sqlConnection;
                    await sqlConnection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        string GetConnectionString()
       => ConfigurationManager.ConnectionStrings?["Primary"]?.ConnectionString ?? string.Empty;

        SqlCommand GetSQLCommand(string procedureName, Dictionary<string, object> paramsDictionary)
        {
            //1. Initialize a SQLCommand
            SqlCommand command = new SqlCommand();
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            //2. Load the parameters to the proc
            if (paramsDictionary != null && paramsDictionary.Count() > 0)
            {
                foreach (KeyValuePair<string, object> keyValuePair in paramsDictionary)
                {
                    command.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                }
            }
            return command;
        }

    }
}

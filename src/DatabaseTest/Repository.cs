using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;

namespace DatabaseTest
{
    public class BaseRepository : IDisposable
    {
        #region [ PRIVATE ]

        private object _lockObject = new object();

        private string ConnectionString { get; set; }

        private string ProviderName { get; set; }

        private void OpenConnection(IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private IDbConnection CreateDbConnection(string connectionString, string providerName)
        {
            IDbConnection connection = null;

            try
            {
                connection = new MySqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                //throw new Exception("Occurred an error while creating the DbProviderFactory for: {0}, Error details: {1}".FormatWith(providerName, ex.ToString()));
            }

            return connection;
        }

        #endregion

        #region [ PROTECTED ]

        protected IDbTransaction Transaction { get; private set; }

        protected IDbConnection Connection { get; private set; }

        #endregion

        #region [ CONSTRUCTORS ]

        public BaseRepository(string connectionString, string providerName)
        {
            Connection = CreateDbConnection(connectionString, providerName);
            OpenConnection(Connection);

            ConnectionString = connectionString;
            ProviderName = providerName;
        }

        public BaseRepository(IDbConnection connection)
        {
            Connection = connection;

            OpenConnection(Connection);
        }

        #endregion

        public IDbConnection GetConnection()
        {
            lock (_lockObject)
            {
                var connection = CreateDbConnection(ConnectionString, ProviderName);

                OpenConnection(connection);

                return connection;
            }
        }

        public void BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
        }

        public void BeginTransaction(IDbConnection connection)
        {
            Transaction = connection.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public void Close(IDbConnection connection)
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }

        public IDbTransaction BeginAndReturnTransaction(IDbConnection connection)
        {
            throw new NotImplementedException();
        }

        public void Commit(IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public void Rollback(IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}

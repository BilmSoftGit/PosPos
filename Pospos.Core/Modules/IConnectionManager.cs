using Pospos.Core.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Pospos.Core.Modules
{
    public interface IConnectionManager
    {
        IDbConnection Connection { get; }
    }

    public class MainConnectionManager : IConnectionManager
    {
        private readonly ConnectionConfig _config;
        public MainConnectionManager(ConnectionConfig connections)
        {
            _config = connections;
        }
        public IDbConnection Connection
        {
            get
            {
                var conn = SqlClientFactory.Instance.CreateConnection();
                conn.ConnectionString = _config.ManagerConnection;
                conn.Open();
                return conn;
            }
        }
    }
}

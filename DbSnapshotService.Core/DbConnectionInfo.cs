using Microsoft.SqlServer.Management.Smo;

namespace DbSnapshotManager.Core
{
    /// <summary>
    /// This class provides SQL Server Database connection information.
    /// </summary>
    public class DbConnectionInfo
    {

        public string ServerAddress { get; set; }
        public string Instance { get; set; }
        public bool UseImpersonation { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Create a new instance of DbConnectionInfo.
        /// This method uses impersonation and local default SQL Instance.
        /// </summary>
        public DbConnectionInfo()
        {
            UseImpersonation = true;
            ServerAddress = @".\";
        }

        /// <summary>
        /// Create a new instance of DbConnectionInfo.
        /// This method use impersonation.
        /// </summary>
        /// <param name="serverAddress">The IP address or DNS name of the SQL server.</param>
        public DbConnectionInfo(string serverAddress)
        {
            ServerAddress = serverAddress;
            UseImpersonation = true;
        }

        /// <summary>
        /// Create a new instance of DbConnectionInfo.
        /// This method use impersonation.
        /// </summary>
        /// <param name="serverAddress">The IP address or DNS name of the SQL server.</param>
        /// <param name="instanceName">The name of the instance.</param>
        public DbConnectionInfo(string serverAddress, string instanceName)
        {
            ServerAddress = serverAddress;
            Instance = instanceName;
            UseImpersonation = true;
        }

        /// <summary>
        /// Create a new instance of DbConnectionInfo.
        /// </summary>
        /// <param name="serverAddress">The IP address or DNS name, plus instance name of the server.</param>
        /// <param name="userName">The user name to use</param>
        /// <param name="password">The password - in clear - to use.</param>
        public DbConnectionInfo(string serverAddress, string userName, string password)
        {
            ServerAddress = serverAddress;
            
            UseImpersonation = false;
            UserName = userName;
            Password = password;
        }

        public DbConnectionInfo(string serverAddress, string instanceName, string username, string password)
        {
            ServerAddress = serverAddress;
            Instance = instanceName;
            UseImpersonation = false;
            UserName = username;
            Password = password;
        }

        internal Server GetServer()
        {

            Server server = new Server();

            if (!UseImpersonation)
            {
                server.ConnectionContext.LoginSecure = false;
                server.ConnectionContext.Login = UserName;
                server.ConnectionContext.Password = Password;
            }
            else
            {
                server.ConnectionContext.LoginSecure = true;
            }
            
            if (string.IsNullOrWhiteSpace(Instance))
                server.ConnectionContext.ServerInstance = ServerAddress + Instance;

            
            return server;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.Management.Smo;

namespace DbSnapshotManager.Core
{
    public class DbSnapshotMgr
    {
        public void CreateSnapshot(DbConnectionInfo connectionInfo, string databaseName, string snapshotDatabaseName = null)
        {
            if (connectionInfo == null)
                throw new ArgumentNullException("connectionInfo must be initialized to create a snapshot.");
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentNullException("databaseName is required to create a snapshot.");

            // 1. Initiate db connection
            Server server = connectionInfo.GetServer();

            // 2. Check if DB Exists
            Database db = server.Databases[databaseName];
            if (db == null)
                throw new ArgumentOutOfRangeException(string.Format("The database {0} does not exists.", databaseName));

            var snapshot = db.IsDatabaseSnapshot;
            var hasSnapshots = db.IsDatabaseSnapshotBase;


            string dbSnapshotName = string.IsNullOrWhiteSpace(snapshotDatabaseName) ? databaseName + "_" + DateTime.UtcNow.ToString("yyMMdd") : snapshotDatabaseName;

            Database snapshotDatabase = new Database(server, dbSnapshotName);
            snapshotDatabase.DatabaseSnapshotBaseName = db.Name;
            foreach (FileGroup fileGroup in db.FileGroups)
            {
                FileGroup fg = new FileGroup(snapshotDatabase, fileGroup.Name);
                snapshotDatabase.FileGroups.Add(fg);
            }

            foreach (FileGroup fileGroup in db.FileGroups)
            {
                foreach (DataFile dataFile in fileGroup.Files)
                {
                    var df = new DataFile(snapshotDatabase.FileGroups[fileGroup.Name],
                                            dataFile.Name,
                                            Path.Combine(db.PrimaryFilePath, string.Format("{0}_{1}.ss", dataFile.Name, snapshotDatabaseName)));
                    snapshotDatabase.FileGroups[fileGroup.Name].Files.Add(df);
                }
            }
            // 3. 

            snapshotDatabase.Create();
        }

        public void RestoreSnapshot(DbConnectionInfo connectionInfo, string databaseName, string snapshotDatabaseName)
        {
            if (connectionInfo == null)
                throw new ArgumentNullException("connectionInfo must be initialized to create a snapshot.");
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentNullException("databaseName is required to create a snapshot.");

            // 1. Initiate db connection
            Server server = connectionInfo.GetServer();

            // 2. Check if DB Exists and if it's not a snapshot
            Database db = server.Databases[databaseName];
            if (db == null)
                throw new ArgumentOutOfRangeException(string.Format("The database {0} does not exists.", databaseName));

            Database snapshotDb = server.Databases[snapshotDatabaseName];
            if (snapshotDb == null)
                throw new ArgumentOutOfRangeException(string.Format("The database {0} does not exists.", snapshotDatabaseName));

            if (!snapshotDb.IsDatabaseSnapshot)
                throw new ArgumentException(string.Format("The database {0} is not a snapshot database.", snapshotDatabaseName));

            string restoreSQL = "RESTORE DATABASE {0} FROM DATABASE_SNAPSHOT = '{1}'";


            server.ConnectionContext.ExecuteNonQuery(string.Format(restoreSQL, db.Name, snapshotDb.Name));
        }

        public void DeleteSnapshot(DbConnectionInfo connectionInfo, string snapshotDatabaseName)
        {
            if (connectionInfo == null)
                throw new ArgumentNullException("connectionInfo must be initialized to create a snapshot.");
            if (string.IsNullOrWhiteSpace(snapshotDatabaseName))
                throw new ArgumentNullException("databaseName is required to create a snapshot.");

            // 1. Initiate db connection
            Server server = connectionInfo.GetServer();

            // 2. Check if DB Exists and if it's not a snapshot
            Database db = server.Databases[snapshotDatabaseName];
            if (db == null)
                throw new ArgumentOutOfRangeException(string.Format("The database {0} does not exists.", snapshotDatabaseName));

            if (!db.IsDatabaseSnapshot)
                throw new ArgumentException(string.Format("The database {0} is not a snapshot database.", snapshotDatabaseName));

            db.Drop();
        }

        public Dictionary<string, List<string>> ListSnapshots(DbConnectionInfo connectionInfo)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

            if (connectionInfo == null)
                throw new ArgumentNullException("connectionInfo must be initialized to create a snapshot.");
            
            // 1. Initiate db connection
            Server server = connectionInfo.GetServer();

            foreach (Database database in server.Databases)
            {
                if(database.IsDatabaseSnapshot)
                {
                    if(!result.ContainsKey(database.DatabaseSnapshotBaseName))
                    {
                        result.Add(database.DatabaseSnapshotBaseName,new List<string>());
                    }

                    result[database.DatabaseSnapshotBaseName].Add(database.Name);
                }
            }

            return result;
        }

        public void ListSnapshots(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void ListDatabasesWithSnapshot()
        {
            throw new NotImplementedException();
        }

        public void RestoreToNewDb()
        {
            throw new NotImplementedException();
        }
    }
}
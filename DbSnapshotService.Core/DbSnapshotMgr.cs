using System;
using System.IO;
using Microsoft.SqlServer.Management.Smo;

namespace DbSnapshotManager.Core
{
    public class DbSnapshotMgr
    {
        public void CreateSnapshot(DbConnectionInfo connectionInfo, string databaseName,
                                   string snapshotDatabaseName = null)
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

            bool snapshot = db.IsDatabaseSnapshot;
            bool hasSnapshots = db.IsDatabaseSnapshotBase;


            string dbSnapshotName = string.IsNullOrWhiteSpace(snapshotDatabaseName)
                                        ? databaseName + "_" + DateTime.UtcNow.ToString("yymmdd")
                                        : snapshotDatabaseName;

            var snapshotDatabase = new Database(server, dbSnapshotName);
            snapshotDatabase.DatabaseSnapshotBaseName = db.Name;
            foreach (FileGroup fileGroup in db.FileGroups)
            {
                var fg = new FileGroup(snapshotDatabase, fileGroup.Name);
                snapshotDatabase.FileGroups.Add(fg);
            }

            foreach (FileGroup fileGroup in db.FileGroups)
            {
                foreach (DataFile dataFile in fileGroup.Files)
                {
                    var df = new DataFile(snapshotDatabase.FileGroups[fileGroup.Name],
                                          dataFile.Name,
                                          Path.Combine(db.PrimaryFilePath,
                                                       string.Format("{0}_{1}.ss", dataFile.Name, snapshotDatabaseName)));
                    snapshotDatabase.FileGroups[fileGroup.Name].Files.Add(df);
                }
            }
            // 3. 

            snapshotDatabase.Create();
        }

        public void ListSnapshot()
        {
        }

        public void ListDatabasesWithSnapshot()
        {
        }

        public void RestoreSnapshot(DbConnectionInfo connectionInfo, string databaseName,
                                    string snapshotDatabaseName = null)
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

            //#============================================================
            //# Restore a Database using PowerShell and SQL Server SMO
            //# Restore to the same database, overwrite existing db
            //# Donabel Santos
            //#============================================================

            //#clear screen
            //cls

            //#load assemblies
            //[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null
            //#Need SmoExtended for backup
            //[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended") | Out-Null
            //[Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.ConnectionInfo") | Out-Null
            //[Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoEnum") | Out-Null

            //#get backup file
            //#you can also use PowerShell to query the last backup file based on the timestamp
            //#I'll save that enhancement for later
            //$backupFile = "C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\Backup\test_db_20090531153233.bak"

            //#we will query the db name from the backup file later

            //$server = New-Object ("Microsoft.SqlServer.Management.Smo.Server") "(local)"
            //$backupDevice = New-Object ("Microsoft.SqlServer.Management.Smo.BackupDeviceItem") ($backupFile, "File")
            //$smoRestore = new-object("Microsoft.SqlServer.Management.Smo.Restore")

            //#settings for restore
            //$smoRestore.NoRecovery = $false;
            //$smoRestore.ReplaceDatabase = $true;
            //$smoRestore.Action = "Database"

            //#show every 10% progress
            //$smoRestore.PercentCompleteNotification = 10;

            //$smoRestore.Devices.Add($backupDevice)

            //#read db name from the backup file's backup header
            //$smoRestoreDetails = $smoRestore.ReadBackupHeader($server)

            //#display database name
            //"Database Name from Backup Header : " + $smoRestoreDetails.Rows[0]["DatabaseName"]

            //$smoRestore.Database = $smoRestoreDetails.Rows[0]["DatabaseName"]

            //#restore
            //$smoRestore.SqlRestore($server)

            //"Done"
        }

        public void RestoreToNewDb()
        {
//                       #load assemblies
//           [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null
//           #Need SmoExtended for backup
//           [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended") | Out-Null
//           [Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.ConnectionInfo") | Out-Null
//           [Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoEnum") | Out-Null
//            
//           $backupFile = 'C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\Backup\test_db_20090531153233.bak'
//            
//           #we will query the database name from the backup header later
//           $server = New-Object ("Microsoft.SqlServer.Management.Smo.Server") "(local)"
//           $backupDevice = New-Object("Microsoft.SqlServer.Management.Smo.BackupDeviceItem") ($backupFile, "File")
//           $smoRestore = new-object("Microsoft.SqlServer.Management.Smo.Restore")
//            
//           #restore settings
//           $smoRestore.NoRecovery = $false;
//           $smoRestore.ReplaceDatabase = $true;
//           $smoRestore.Action = "Database"
//           $smoRestorePercentCompleteNotification = 10;
//           $smoRestore.Devices.Add($backupDevice)
//            
//           #get database name from backup file
//           $smoRestoreDetails = $smoRestore.ReadBackupHeader($server)
//            
//           #display database name
//           "Database Name from Backup Header : " +$smoRestoreDetails.Rows[0]["DatabaseName"]
//            
//           #give a new database name
//           $smoRestore.Database =$smoRestoreDetails.Rows[0]["DatabaseName"] + "_Copy"
//            
//           #specify new data and log files (mdf and ldf)
//           $smoRestoreFile = New-Object("Microsoft.SqlServer.Management.Smo.RelocateFile")
//           $smoRestoreLog = New-Object("Microsoft.SqlServer.Management.Smo.RelocateFile")
//            
//           #the logical file names should be the logical filename stored in the backup media
//           $smoRestoreFile.LogicalFileName = $smoRestoreDetails.Rows[0]["DatabaseName"]
//           $smoRestoreFile.PhysicalFileName = $server.Information.MasterDBPath + "\" + $smoRestore.Database + "_Data.mdf"
//           $smoRestoreLog.LogicalFileName = $smoRestoreDetails.Rows[0]["DatabaseName"] + "_Log"
//           $smoRestoreLog.PhysicalFileName = $server.Information.MasterDBLogPath + "\" + $smoRestore.Database + "_Log.ldf"
//           $smoRestore.RelocateFiles.Add($smoRestoreFile)
//           $smoRestore.RelocateFiles.Add($smoRestoreLog)
//            
//           #restore database
//           $smoRestore.SqlRestore($server)
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
                throw new ArgumentOutOfRangeException(string.Format("The database {0} does not exists.",
                                                                    snapshotDatabaseName));

            if (!db.IsDatabaseSnapshot)
                throw new ArgumentException(string.Format("The database {0} is not a snapshot database.",
                                                          snapshotDatabaseName));

            db.Drop();
        }
    }
}
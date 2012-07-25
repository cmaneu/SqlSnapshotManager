using DbSnapshotService.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DbSnapshotService.Tests.Core
{
    
    
    /// <summary>
    ///This is a test class for DbSnapshotManagerTest and is intended
    ///to contain all DbSnapshotManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DbSnapshotManagerTest
    {

        [TestClass()]
        public class CreateSnapshot
        {
            [TestMethod()]
            public void CreateSnapshotNoValidconnectionInfoParameters()
            {
                DbSnapshotManager target = new DbSnapshotManager();
                DbConnectionInfo connectionInfo = null;
                string databaseName = string.Empty;
                string snapshotDatabaseName = string.Empty;

                try
                {
                    target.CreateSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
                    Assert.Fail();
                }
                catch (ArgumentNullException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("connectionInfo"));
                }  
            } 
            
            [TestMethod()]
            public void CreateSnapshotNoValidDbNameParameters()
            {
                DbSnapshotManager target = new DbSnapshotManager();
                DbConnectionInfo connectionInfo = new DbConnectionInfo();
                string databaseName = string.Empty;
                string snapshotDatabaseName = string.Empty;

                try
                {
                    target.CreateSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
                    Assert.Fail();
                }
                catch (ArgumentNullException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("databaseName"));
                } 
            }

            [TestMethod()]
            public void CreateSnapshotMissingDb()
            {
                DbSnapshotManager target = new DbSnapshotManager();
                DbConnectionInfo connectionInfo = new DbConnectionInfo(@".\","SQL2012");
                string databaseName = "UnknowDatabase";
                string snapshotDatabaseName = string.Empty;

                try
                {
                    target.CreateSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
              
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Assert.IsTrue(ex.Message.Contains(databaseName));
                }
            }

            [TestMethod()]
            public void CreateSnapshotValid()
            {
                DbSnapshotManager target = new DbSnapshotManager();
                DbConnectionInfo connectionInfo = new DbConnectionInfo(@".\","SQL2012");
                string databaseName = "Certit";
                string snapshotDatabaseName = string.Empty;

                target.CreateSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
            }

            [TestMethod()]
            public void CreateSnapshotValidthCustomSnapshotName()
            {
                DbSnapshotManager target = new DbSnapshotManager();
                DbConnectionInfo connectionInfo = new DbConnectionInfo(@".\","SQL2012");
                string databaseName = "Certit";
                string snapshotDatabaseName = "Certit_pretest";

                try
                {
                    target.CreateSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
              
                }
                catch (ArgumentNullException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("databaseName"));
                }
            }
        }


        [TestClass()]
        public class DeleteSnapshot
        {
            [TestMethod()]
            public void DeleteSnapshotNoValidconnectionInfoParameters()
            {
                DbSnapshotManager target = new DbSnapshotManager();
                DbConnectionInfo connectionInfo = null;
                string snapshotDatabaseName = string.Empty;

                try
                {
                    target.DeleteSnapshot(connectionInfo, snapshotDatabaseName);
                    Assert.Fail();
                }
                catch (ArgumentNullException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("connectionInfo"));
                }
            }

            [TestMethod()]
            public void DeleteSnapshotNoValidDbNameParameters()
            {
                DbSnapshotManager target = new DbSnapshotManager();
                DbConnectionInfo connectionInfo = new DbConnectionInfo();
                string snapshotDatabaseName = string.Empty;

                try
                {
                    target.DeleteSnapshot(connectionInfo, snapshotDatabaseName);
                    Assert.Fail();
                }
                catch (ArgumentNullException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("databaseName"));
                }
            }

            [TestMethod()]
            public void DeleteSnapshotMissingDb()
            {
                DbSnapshotManager target = new DbSnapshotManager();
                DbConnectionInfo connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
                string snapshotDatabaseName = "UnknowDatabase";

                try
                {
                    target.DeleteSnapshot(connectionInfo, snapshotDatabaseName);

                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Assert.IsTrue(ex.Message.Contains(snapshotDatabaseName));
                }
            }

            [TestMethod()]
            public void DeleteSnapshotDbIsNotASnapshot()
            {
                DbSnapshotManager target = new DbSnapshotManager();
                DbConnectionInfo connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
                string snapshotDatabaseName = "Certit";

                try
                {
                    target.DeleteSnapshot(connectionInfo, snapshotDatabaseName);

                }
                catch (ArgumentException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("is not a snapshot database"));
                }
            }

            [TestMethod()]
            public void DeleteSnapshotValid()
            {
                DbSnapshotManager target = new DbSnapshotManager();
                DbConnectionInfo connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
                string snapshotDatabaseName = "Certit_pretest";

                target.DeleteSnapshot(connectionInfo, snapshotDatabaseName);
            }

        }
       
    }
}

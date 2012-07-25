using System;
using DbSnapshotManager.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbSnapshotManager.Tests.Core
{
    /// <summary>
    ///This is a test class for DbSnapshotMgrTest and is intended
    ///to contain all DbSnapshotMgrTest Unit Tests
    ///</summary>
    [TestClass]
    public class DbSnapshotMgrTest
    {
        #region Nested type: CreateSnapshot

        [TestClass]
        public class CreateSnapshot
        {
            [TestMethod]
            public void CreateSnapshotNoValidconnectionInfoParameters()
            {
                var target = new DbSnapshotMgr();
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

            [TestMethod]
            public void CreateSnapshotNoValidDbNameParameters()
            {
                var target = new DbSnapshotMgr();
                var connectionInfo = new DbConnectionInfo();
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

            [TestMethod]
            public void CreateSnapshotMissingDb()
            {
                var target = new DbSnapshotMgr();
                var connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
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

            [TestMethod]
            public void CreateSnapshotValid()
            {
                var target = new DbSnapshotMgr();
                var connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
                string databaseName = "Certit";
                string snapshotDatabaseName = string.Empty;

                target.CreateSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
            }

            [TestMethod]
            public void CreateSnapshotValidthCustomSnapshotName()
            {
                var target = new DbSnapshotMgr();
                var connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
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

        #endregion

        #region Nested type: DeleteSnapshot

        [TestClass]
        public class DeleteSnapshot
        {
            [TestMethod]
            public void DeleteSnapshotNoValidconnectionInfoParameters()
            {
                var target = new DbSnapshotMgr();
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

            [TestMethod]
            public void DeleteSnapshotNoValidDbNameParameters()
            {
                var target = new DbSnapshotMgr();
                var connectionInfo = new DbConnectionInfo();
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

            [TestMethod]
            public void DeleteSnapshotMissingDb()
            {
                var target = new DbSnapshotMgr();
                var connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
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

            [TestMethod]
            public void DeleteSnapshotDbIsNotASnapshot()
            {
                var target = new DbSnapshotMgr();
                var connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
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

            [TestMethod]
            public void DeleteSnapshotValid()
            {
                var target = new DbSnapshotMgr();
                var connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
                string snapshotDatabaseName = "Certit_pretest";

                target.DeleteSnapshot(connectionInfo, snapshotDatabaseName);
            }
        }

        #endregion


        [TestClass()]
        public class RestoreSnapshot
        {
            [TestMethod()]
            public void InvalidConnectionInfos()
            {
                DbSnapshotMgr target = new DbSnapshotMgr();
                DbConnectionInfo connectionInfo = null;
                string databaseName = string.Empty;
                string snapshotDatabaseName = string.Empty;

                try
                {
                    target.RestoreSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
                    Assert.Fail();
                }
                catch (ArgumentNullException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("connectionInfo"));
                }
            }

            [TestMethod()]
            public void InvalidDatabaseName()
            {
                DbSnapshotMgr target = new DbSnapshotMgr();
                DbConnectionInfo connectionInfo = new DbConnectionInfo();
                string databaseName = string.Empty;
                string snapshotDatabaseName = string.Empty;

                try
                {
                    target.RestoreSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
                    Assert.Fail();
                }
                catch (ArgumentNullException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("databaseName"));
                }
            }

            [TestMethod()]
            public void InvalidDatabase()
            {
                DbSnapshotMgr target = new DbSnapshotMgr();
                DbConnectionInfo connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
                string databaseName = "UnknowDatabase";
                string snapshotDatabaseName = string.Empty;

                try
                {
                    target.RestoreSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
                    Assert.Fail();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("does not exists"));
                }
            }

            [TestMethod()]
            public void InvalidSnapshot()
            {
                DbSnapshotMgr target = new DbSnapshotMgr();
                DbConnectionInfo connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
                string databaseName = "Certit";
                string snapshotDatabaseName = "UnknowSnapshotDb";

                try
                {
                    target.RestoreSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
                    Assert.Fail();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("does not exists"));
                }
            }

            [TestMethod()]
            public void SnapshotDbIsNotASnapshot()
            {
                DbSnapshotMgr target = new DbSnapshotMgr();
                DbConnectionInfo connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
                string databaseName = "Certit";
                string snapshotDatabaseName = "Certit";

                try
                {
                    target.RestoreSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
                    Assert.Fail();
                }
                catch (ArgumentException ex)
                {
                    Assert.IsTrue(ex.Message.Contains("is not a snapshot"));
                }
            }

            [TestMethod()]
            public void ValidRestore()
            {
                DbSnapshotMgr target = new DbSnapshotMgr();
                DbConnectionInfo connectionInfo = new DbConnectionInfo(@".\", "SQL2012");
                string databaseName = "Certit";
                string snapshotDatabaseName = "Certit_120725";

                target.RestoreSnapshot(connectionInfo, databaseName, snapshotDatabaseName);
            }

        }

        [TestClass()]
        public class ListSnapshots
        {

            [TestMethod()]
            public void ListSnapshot()
            {
                DbSnapshotMgr target = new DbSnapshotMgr();
                DbConnectionInfo connectionInfo = new DbConnectionInfo(@".\", "SQL2012");

                var result = target.ListSnapshots(connectionInfo);
                Assert.IsTrue(result.Count > 0);
            }
        }
    }
}
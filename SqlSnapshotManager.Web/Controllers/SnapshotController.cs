using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DbSnapshotManager.Core;

namespace SqlSnapshotManager.Web.Controllers
{
    public class SnapshotController : System.Web.Http.ApiController
    {

        // api/snapshot/create?server=srvdb01&instance=SQL2012&db=MaBase&snapshot=Mabase_Pretest
        [AcceptVerbs("GET","POST")]
        public string Create(string server, string instance, string database, string snapshot, string username = "", string password="")
        {
            DbSnapshotMgr mgr = new DbSnapshotMgr();
            DbConnectionInfo connectionInfo;
            if(!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                connectionInfo = new DbConnectionInfo(server, instance, username, password);
            }
            else
            {
                connectionInfo  = new DbConnectionInfo(server,instance);
            }

         
            try
            {
                  mgr.CreateSnapshot(connectionInfo,database,snapshot);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
          
            return "Ok";
        }

        [AcceptVerbs("GET","POST")]
        public string Restore(string server, string instance, string database, string snapshot, string username = "", string password="")
        {
            DbSnapshotMgr mgr = new DbSnapshotMgr();
            DbConnectionInfo connectionInfo;
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                connectionInfo = new DbConnectionInfo(server, instance, username, password);
            }
            else
            {
                connectionInfo = new DbConnectionInfo(server, instance);
            }

            try
            {
                  mgr.RestoreSnapshot(connectionInfo,database,snapshot);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
          
            return "Ok";
        }

        [AcceptVerbs("GET","POST")]
        public string Delete(string server, string instance,  string snapshot)
        {
            DbSnapshotMgr mgr = new DbSnapshotMgr();
            DbConnectionInfo connectionInfo = new DbConnectionInfo(server,instance);
            try
            {
                  mgr.DeleteSnapshot(connectionInfo,snapshot);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
          
            return "Ok";
        }

        [AcceptVerbs("GET","POST")]
        public Dictionary<string,List<string>> List(string server, string instance)
        {
            DbSnapshotMgr mgr = new DbSnapshotMgr();
            DbConnectionInfo connectionInfo = new DbConnectionInfo(server, instance);

            return mgr.ListSnapshots(connectionInfo);          
        }
    }
}

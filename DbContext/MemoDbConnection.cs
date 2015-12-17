using System;
using MySql.Data.Entity;
using System.Data.Entity;
using System.Data.Common;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace NickServer
{
	public class MemoDbConnection : IDisposable {
		private static bool creationAttempt { get; set; }

		public static string DbUser { get; set; }
		public static string DbPwd { get; set; }
		public static string DbServer { get; set; }
		public static string DbPort { get; set; }
		public static string DbDb { get; set; }

		public MySqlConnection Connection { get; set;}
		public MySqlTransaction Transaction { get; set;}

		public MemoDbConnection() {
			if (DbServer == null) {
				DbServer = ConfigurationManager.AppSettings ["db.server"];
			}
			if (DbPort == null) {
				DbPort = ConfigurationManager.AppSettings ["db.port"];
			}
			if (DbUser == null) {
				DbUser = ConfigurationManager.AppSettings ["db.user"];
			}
			if (DbDb == null) {
				DbDb = ConfigurationManager.AppSettings ["db.db"];
			}
			string connectionString = "server="+ DbServer 
				+";port="+ DbPort +";database=" + DbDb +";"+
					"uid=" + DbUser +";password="+ DbPwd
					+ ";Persist Security Info=True;";

			Connection = new MySqlConnection (connectionString);

			if (!creationAttempt) {
				using (MemoDbContext contextDB = new MemoDbContext(Connection, false)) {
					contextDB.Database.CreateIfNotExists ();
					creationAttempt = true;
				}
			}

			Connection.Open ();
		}

		public MySqlTransaction BeginTransaction(){
			Transaction = Connection.BeginTransaction ();
			return Transaction;
		}

		public void CommitTransaction(){
			Transaction.Commit();
		}

		public void RollbackTransaction(){
			Transaction.Rollback();
		}

#region Disposable
		// Flag: Has Dispose already been called?
		bool disposed = false;

		public void Dispose() {
			this.Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected void Dispose (bool disposing)
		{
			if (disposed) {
				return;
			}
			if (disposing) {
				// Free any other managed objects here.
				//
				this.Connection.Dispose ();
			}			
			// Free any unmanaged objects here.
			//
			disposed = true;
		}
#endregion
	}
}


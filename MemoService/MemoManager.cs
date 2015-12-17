using System;
using System.Linq;
using System.Collections.Generic;

namespace NickServer
{
	public class MemoManager
	{
		public static readonly string ANON_NAME = "Anonymous"; 

		public static void putMemo (string message, string to, string from)
		{
			Memo memo = new Memo(message, from);

			using (MemoDbConnection conn = new MemoDbConnection())
			{	
				conn.BeginTransaction();
				try { 
					using (MemoDbContext ctx = new MemoDbContext(conn.Connection, false))
					{
						ctx.Database.UseTransaction(conn.Transaction);

						var q = from u in ctx.Users where
							u.Username.Equals (to)
							select u;

						if (q.Count() != 1) {
							conn.CommitTransaction();
							return;
						}
						User toUser = q.First();

						ctx.Memos.Add(memo);
						toUser.Memos.Add(memo);
						ctx.SaveChanges();
					}
					conn.CommitTransaction();
				}
				catch
				{
					conn.RollbackTransaction();
					throw;
				}
			}
		}


		public static List<Memo> getMemos(string user)
		{
			List<Memo> memoList = new List<Memo>();
			using (MemoDbConnection conn = new MemoDbConnection())
			{	
				conn.BeginTransaction();
				try { 
					using (MemoDbContext ctx = new MemoDbContext(conn.Connection, false))
					{
						ctx.Database.UseTransaction(conn.Transaction);

						var q = from u in ctx.Users where
							u.Username.Equals (user)
							select u;

						if (q.Count() != 1) {
							conn.CommitTransaction();
							return null;
						}
						User theUser = q.First();

						foreach(Memo m in theUser.Memos)
						{
							if(m.Unread){
								memoList.Add(new Memo{
									Message = m.Message,
									Sender = m.Sender,
									Timestamp = m.Timestamp
								});
								m.Unread = false;
							}
							else {
								theUser.Memos.Remove(m);
							}
						}

						ctx.SaveChanges();
					}
					conn.CommitTransaction();
					return memoList;
				}
				catch
				{
					conn.RollbackTransaction();
					throw;
				}
			}


		}


		#region Anonymous_User_region

		public static void initAnon(){
			using (MemoDbConnection conn = new MemoDbConnection()) {	
				using (MemoDbContext ctx = new MemoDbContext(conn.Connection, false))
				{
					//var q = from u in ctx.Users where
					//        u.Username.Equals (ANON_NAME)
					//		select u;

					var q = from u in ctx.Users	select u;

					if (q.Count() < 1) {
						var anon = new User ();
						anon.Username = ANON_NAME;
						anon.LastOnline = DateTime.Now;
						anon.Password = "PWD";

						conn.BeginTransaction ();
						try {
							ctx.Database.UseTransaction (conn.Transaction);
							ctx.Users.Add (anon);
							ctx.SaveChanges ();

							conn.CommitTransaction ();
						} catch {
							conn.RollbackTransaction ();
						}
					}
				}	 
			}
		}
		public static User getAnon(MemoDbContext ctx){
			bool loop = true;
			int threshold = 3;
			while (loop) {
				var q = from u in ctx.Users where
					u.Username.Equals (ANON_NAME)
					select u;

				if (q.Count () < 1) {
					initAnon ();
					threshold--;
				} else {
					return q.First ();
				}
				if (threshold < 1) {
					break;
				}
			}
			return null;
		}

		#endregion
	}
}


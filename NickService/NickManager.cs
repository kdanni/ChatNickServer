using System;
using System.Linq;

namespace NickServer
{
	public class NickRegistrationFailedException: System.Exception
	{
		public NickRegistrationFailedException(string message): base(message)
		{
		}
		public NickRegistrationFailedException(string message, Exception caused): base(message, caused)
		{
		}
	}
	public class NickAlreadyRegistratedException: System.Exception
	{
		public NickAlreadyRegistratedException(): base("Nick already registrated!")
		{
		}
	}
	public class NickNotFoundException: System.Exception
	{
		public NickNotFoundException(): base("Nick not found!")
		{
		}
	}
	public class NickPasswordException: System.Exception
	{
		public NickPasswordException(): base("Password missmatch!")
		{
		}
	}

	public class NickManager
	{
		private static readonly TimeSpan TIMEOUT = new TimeSpan(1,0,0);

		public static void registerNick (string nick, string macHash)
		{

			using (MemoDbConnection conn = new MemoDbConnection())
			{	
				try {
					refreshNick(nick, macHash);
				} catch {}
				conn.BeginTransaction();
				try { 
					using (MemoDbContext ctx = new MemoDbContext(conn.Connection, false))
					{	
						ctx.Database.UseTransaction(conn.Transaction);

						var qu = from u in ctx.Users where
							u.Username.Equals (nick)
							select u;

						if (qu.Count() > 0) {
							foreach(User u in qu){
								if(u.Password.Equals(macHash)){
									ctx.Users.Remove(u);
								} else
								if(u.LastOnline.Add(TIMEOUT).CompareTo(DateTime.UtcNow) > 0){
									throw new NickAlreadyRegistratedException();
								} else {
									ctx.Users.Remove(u);
								}
							}
						}

						User nu = new User {
							LastOnline = DateTime.UtcNow,
							Username = nick,
							Password = macHash
						};

						ctx.Users.Add(nu);
						ctx.SaveChanges();
					}
					conn.CommitTransaction();
				}

				catch (NickAlreadyRegistratedException ne) {
					conn.RollbackTransaction();
					throw ne;
				}
				catch (Exception e)
				{
					conn.RollbackTransaction();

					System.Console.Error.WriteLine (e.Message);
					System.Console.Error.WriteLine (e.StackTrace);
					throw new NickRegistrationFailedException(e.Message, e);
				}
			}
		}

		public static void refreshNick (string nick, string macHash)
		{

			using (MemoDbConnection conn = new MemoDbConnection())
			{	
				conn.BeginTransaction();
				try { 
					using (MemoDbContext ctx = new MemoDbContext(conn.Connection, false))
					{	
						ctx.Database.UseTransaction(conn.Transaction);

						var qu = from u in ctx.Users where
							u.Username.Equals (nick)
							select u;

						if (qu.Count() != 1) {
							throw new NickNotFoundException();
						}
						User user = qu.First();
						if(user == null || user.Password == null ||
							!user.Password.Equals(macHash)){
							throw new NickPasswordException();
						}

						user.LastOnline = DateTime.UtcNow;

						ctx.SaveChanges();
					}
					conn.CommitTransaction();
				}

				catch (NickNotFoundException ne) {
					conn.RollbackTransaction();
					throw ne;
				}
				catch (NickPasswordException pe) {
					conn.RollbackTransaction();
					throw pe;
				}
				catch (Exception e)
				{
					conn.RollbackTransaction();

					System.Console.Error.WriteLine (e.Message);
					System.Console.Error.WriteLine (e.StackTrace);
					throw new NickRegistrationFailedException(e.Message, e);
				}
			}
		}
	}
}


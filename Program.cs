using System;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.ServiceModel;

namespace NickServer
{
	class MainClass
	{

		static void Services ()
		{
			MemoManager.initAnon ();
			
			ServiceHost peerHost = null;
			ServiceHost nickHost = null;
			ServiceHost memoHost = null;
			try {
				peerHost = new ServiceHost (typeof(PeerService));
				peerHost.Open ();
				Console.WriteLine ("PeerService has started up.");

				nickHost = new ServiceHost (typeof(NickService));
				nickHost.Open();
				Console.WriteLine ("NickService has started up.");

				memoHost = new ServiceHost (typeof(MemoService));
				memoHost.Open();
				Console.WriteLine ("MemoService has started up.");

				Console.ReadLine ();
			} catch (Exception e) {
				Console.WriteLine (e);
			} finally {
				if (peerHost != null) {
					peerHost.Close ();
				}
				if (nickHost != null) {
					nickHost.Close ();
				}
				if (memoHost != null) {
					memoHost.Close ();
				}
			}
		}

		static void Debug ()
		{
			Console.WriteLine ("Debug");


			using (MemoDbConnection conn = new MemoDbConnection())
			{
				conn.BeginTransaction();
				try { 
					using (MemoDbContext ctx = new MemoDbContext(conn.Connection, false))
					{
						ctx.Database.Log = (string message) => { Console.WriteLine(message); };
						ctx.Database.UseTransaction(conn.Transaction);

						Memo testMemo = new Memo("Hello", "d");
						User anon = new User {
							Username = "Anonymous",
							Password = "pass",
							LastOnline = DateTime.UtcNow
						};
						Peer peer = new Peer {
							Address = new Uri("http://localhost:8088").ToString(),
							MAC_AddressHash = "pass"
						};

//						ctx.Memos.Add(testMemo);
//						ctx.Users.Add(anon);
//						ctx.Peers.Add(peer);
//						anon.Memos.Add(testMemo);
//						ctx.SaveChanges();
					}

					conn.CommitTransaction();
				}
				catch
				{
					conn.RollbackTransaction();
					throw;
				}
			}

			try {
				NickManager.registerNick ("nickkk", "MAChashMD5");
				System.Console.WriteLine ("registration OK");
			} catch (Exception ex) {
				System.Console.WriteLine (ex.Message);
			}

			try {
				NickManager.refreshNick ("nickkk", "MAChashMD5");
				System.Console.WriteLine ("refresh OK");
			} catch (Exception ex) {
				System.Console.WriteLine (ex.Message);
			}

			try {
				NickManager.refreshNick ("nickkkkk", "MAChashMD5");
				System.Console.WriteLine ("refresh OK");
			} catch (Exception ex) {
				System.Console.WriteLine (ex.Message);
			}
			System.Console.WriteLine ();


			try {
				PeerManager.registerPeer("ddsgsgsdffcdsfg", new Uri ("http://localhost:53215") );
				System.Console.WriteLine ("peer registration OK");
			} catch (Exception ex) {
				System.Console.WriteLine (ex.Message);
			}
			System.Console.WriteLine ();



			try {
				MemoManager.putMemo("hali", "Anonymous", "d");
				MemoManager.putMemo("hali", "nickkk", "d");
				MemoManager.putMemo("hali", "nickkkkkkkkkk", "d");
				System.Console.WriteLine ("Memo added!");
			} catch (Exception ex) {
				System.Console.WriteLine (ex.Message);
			}
			System.Console.WriteLine ();
		}

		public static void Main (string[] args)
		{
			if (args == null)
			{
				Usage ();
				System.Environment.Exit(1);
			}
			else
			{
				for (int i = 0; i < args.Length; i++) // Loop through array
				{
					if ("-h".Equals (args[i])) {
						Usage ();
						System.Environment.Exit(0);
					}
					if ("-s".Equals (args[i])) {
						if(args.Length <= i+1 || args [i+1] == null){
							continue;
						}
						MemoDbConnection.DbPwd = args [i+1];
					}
					if ("-P".Equals (args[i])) {
						if(args.Length <= i+1 || args [i+1] == null){
							continue;
						}
						MemoDbConnection.DbPort = args [i+1];
					}
					if ("-d".Equals (args[i])) {
						if(args.Length <= i+1 || args [i+1] == null){
							continue;
						}
						MemoDbConnection.DbDb = args [i+1];
					}
					if ("-u".Equals (args[i])) {
						if(args.Length <= i+1 || args [i+1] == null){
							continue;
						}
						MemoDbConnection.DbUser = args [i+1];
					}
					if ("-p".Equals (args[i])) {
						if(args.Length <= i+1 || args [i+1] == null){
							continue;
						}
						MemoDbConnection.DbPwd = args [i+1];
					}
				}
				if (MemoDbConnection.DbPwd == null) {
					Usage ();
					System.Environment.Exit (1);
				}
			}

			Console.WriteLine ("Nick server starting up...");

			try {
				//AnonymousUser.initAnon();

				//Debug();

				Services();
			}
			catch(Exception e)
			{
				System.Console.WriteLine (e.Message);
				System.Console.WriteLine (e.StackTrace);
				System.Console.WriteLine ("Something went wrong.");
				System.Console.WriteLine ("Memo Server Closing...");
				Console.Read ();
				System.Environment.Exit(1);
			}
			System.Console.WriteLine ("Nick server closing...");
			Console.Read (); 
		}

		private static void Usage() {
			Console.WriteLine("Usage:");			
			Console.WriteLine("\t-p\tdatabase password (required!)");
			Console.WriteLine("\t-s\tdatabase server");
			Console.WriteLine("\t-P\tdatabase port");
			Console.WriteLine("\t-d\tdatabase");
			Console.WriteLine("\t-u\tdatabase user");
			Console.WriteLine("\t-h\tdisplay this help");
		}
	}
}
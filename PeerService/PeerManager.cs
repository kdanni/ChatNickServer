using System;
using System.Linq;
using System.Collections.Generic;

namespace NickServer
{
	public class PeerManager
	{
		public class PeerRegistrationFailedException: System.Exception
		{
			public PeerRegistrationFailedException(string message): base(message)
			{
			}
			public PeerRegistrationFailedException(string message, Exception caused): base(message, caused)
			{
			}
		}
		public class PeerAlreadyRegistratedException: System.Exception
		{
			public PeerAlreadyRegistratedException(): base("Peer already registrated!")
			{
			}
		}
//		public class PeerNotFoundException: System.Exception
//		{
//			public PeerNotFoundException(): base("Peer not found!")
//			{
//			}
//		}
		public class PeerPasswordException: System.Exception
		{
			public PeerPasswordException(): base("Password missmatch!")
			{
			}
		}

		public static void registerPeer(string hash, Uri clientUri){

			using (MemoDbConnection conn = new MemoDbConnection())
			{	
				conn.BeginTransaction();
				try { 
					using (MemoDbContext ctx = new MemoDbContext(conn.Connection, false))
					{	
						ctx.Database.UseTransaction(conn.Transaction);

						var qp = from p in ctx.Peers
							where
							p.MAC_AddressHash.Equals (hash)
							select p;

						if (qp.Count () > 1) {
							throw new PeerAlreadyRegistratedException();
						}
						Peer peer;
						if (qp.Count () == 0) {
							peer = new Peer {
								Address = clientUri.ToString(),
								MAC_AddressHash = hash
							};
							ctx.Peers.Add(peer);
						} else {
							peer = qp.First ();
							peer.Address = clientUri.ToString();
						}

						ctx.SaveChanges();
					}
					conn.CommitTransaction ();
				}
				catch (PeerAlreadyRegistratedException pare){
					conn.RollbackTransaction();
					throw pare;
				}
				catch (Exception e)
				{
					conn.RollbackTransaction();

					System.Console.Error.WriteLine (e.Message);
					System.Console.Error.WriteLine (e.StackTrace);
					throw new PeerRegistrationFailedException(e.Message, e);
				}
			}
		}

		public static void kickoutPeer (string hash, string kickerHash)
		{
			using (MemoDbConnection conn = new MemoDbConnection ()) {	
				conn.BeginTransaction ();
				try { 
					using (MemoDbContext ctx = new MemoDbContext (conn.Connection, false)) {	
						ctx.Database.UseTransaction(conn.Transaction);

						var qp = from p in ctx.Peers
							where
							p.MAC_AddressHash.Equals (kickerHash)
							select p;

						if (qp.Count () != 1) {
							throw new PeerPasswordException();
						}

						var qk = from p in ctx.Peers
							where
							p.MAC_AddressHash.Equals (kickerHash)
							select p;

						foreach(Peer p in qk){
							ctx.Peers.Remove(p);
						}
						ctx.SaveChanges();
					}
					conn.CommitTransaction ();
				} catch (PeerPasswordException ppe){
					conn.RollbackTransaction ();
					throw ppe;
				} catch (Exception e){
					conn.RollbackTransaction ();
					throw new PeerRegistrationFailedException(e.Message, e);
				}
			}
		}

		public static List<Uri> getPeerList ()
		{
			using (MemoDbConnection conn = new MemoDbConnection ()) {	
				conn.BeginTransaction ();
				try { 
					List<Uri> list = new List<Uri>();
					using (MemoDbContext ctx = new MemoDbContext (conn.Connection, false)) 
					{	
						ctx.Database.UseTransaction(conn.Transaction);

						var qp = from p in ctx.Peers select p;

						foreach(Peer p in qp){

							list.Add(new Uri(p.Address));
						}

					}
					conn.CommitTransaction ();
					return list;
				} catch {
					conn.RollbackTransaction ();
					return null;
				}
			}
		}
	}
}


using System;

namespace NickServer
{
	public class PeerService : IPeerService
	{
		public PeerService ()
		{
		}

		#region IPeerService implementation

		public bool registerPeer (string hash, Uri clientUri)
		{
			try {
				Console.WriteLine(DateTime.Now + " Peer registration called by " + hash + "@" + clientUri);
				PeerManager.registerPeer (hash, clientUri);
				return true;
			} catch {
				return false;
			}
		}

		public bool kickoutPeer (string hash, string kickerHash)
		{
			try {
				Console.WriteLine(DateTime.Now + " Peer kickout called by " + kickerHash );
				PeerManager.kickoutPeer (hash, kickerHash);
				Console.WriteLine(DateTime.Now + " " + hash + " is kickedout called by " + kickerHash );
				return true;
			} catch {
				return false;
			}
		}

		public System.Collections.Generic.List<Uri> getPeerList ()
		{
			Console.WriteLine(DateTime.Now + " Peer list quered");
			return PeerManager.getPeerList ();
		}

		#endregion
	}
}


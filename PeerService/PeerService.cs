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
				Console.WriteLine("Peer registration called by " + hash + "@" + clientUri);
				PeerManager.registerPeer (hash, clientUri);
				return true;
			} catch {
				return false;
			}
		}

		public bool kickoutPeer (string hash, string kickerHash)
		{
			try {
				Console.WriteLine("Peer kickout called by " + kickerHash );
				PeerManager.kickoutPeer (hash, kickerHash);
				return true;
			} catch {
				return false;
			}
		}

		public System.Collections.Generic.List<Uri> getPeerList ()
		{
			Console.WriteLine("Peer list quered");
			return PeerManager.getPeerList ();
		}

		#endregion
	}
}


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
				PeerManager.registerPeer (hash, clientUri);
				return true;
			} catch {
				return false;
			}
		}

		public bool kickoutPeer (string hash, string kickerHash)
		{
			try {
				PeerManager.kickoutPeer (hash, kickerHash);
				return true;
			} catch {
				return false;
			}
		}

		public System.Collections.Generic.List<Uri> getPeerList ()
		{
			return PeerManager.getPeerList ();
		}

		#endregion
	}
}


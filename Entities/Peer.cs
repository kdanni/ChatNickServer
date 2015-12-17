using System;

namespace NickServer
{
	public class Peer
	{
		public Peer ()
		{
			
		}

		public long Id {
			get;
			set;
		}

		public string Address {
			get;
			set;
		}

		public string MAC_AddressHash {
			get;
			set;
		}

	}
}


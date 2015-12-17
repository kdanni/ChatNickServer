using System;
using System.Collections.Generic;

namespace NickServer
{

	public class User
	{
		public User() {
			this.Memos = new List<Memo> ();
		}

		public long Id {
			get;
			set;
		}

		public string Username {
			get;
			set;
		}

		public string Password {
			get;
			set;
		}

		public DateTime LastOnline {
			get;
			set;
		}

		public List<Memo> Memos {
			get;
			set;
		}
	}
}


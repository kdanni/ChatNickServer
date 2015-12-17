using System;

namespace NickServer
{
	public class Memo
	{
		
		public long Id {
			get;
			set;
		}
		public string Message {
			get;
			set;
		}

		public DateTime Timestamp {
			get;
			set;
		}

		public string Sender {
			get;
			set;
		}

		public bool Unread {
			get;
			set;
		}

		public Memo(){}

		public Memo(string message, string sender)
		{
			this.Timestamp = DateTime.UtcNow;
			this.Message = message;
			this.Sender = sender;
			this.Unread = true;
		}
	}
}


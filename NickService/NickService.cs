using System;

namespace NickServer
{
	public class NickService : INickService
	{
		public NickService ()
		{
		}

		#region INickService implementation

		public bool registerNick (string nick, string hash)
		{
			try {
				Console.WriteLine("Nick registration called by " + nick + "@" + hash);
				NickManager.registerNick(nick,hash);
				return true;
			} catch {
				return false;
			}
		}

		#endregion
	}
}


using System;
using System.Collections.Generic;

namespace NickServer
{
	public class MemoService : IMemoService
	{
		public MemoService ()
		{
		}

		#region IMemoService implementation

		public void putPublicMemo (string message, string sender, string hash)
		{
			try{
				Console.WriteLine("Public memo put by " + hash);
				MemoManager.putMemo(message,MemoManager.ANON_NAME, sender);

			} catch {}
		}

		public void putMemo (string message, string to, string from, string hash)
		{
			try{
				Console.WriteLine("Memo put by " + hash);
				MemoManager.putMemo(message,to, from);

			} catch {}
		}

		public List<MemoData> getMyMemos (string nick, string hash)
		{
			try {
				Console.WriteLine("Memos get by " + hash);
				NickManager.refreshNick(nick, hash);

				List<Memo> memoList = MemoManager.getMemos(nick);
				List<MemoData> memoDataList = new List<MemoData>();
				foreach(Memo m in memoList){
					memoDataList.Add(new MemoData{
						Message = m.Message,
						Sender = m.Sender,
						Timestamp = m.Timestamp
					});
				}

				return memoDataList;

			} catch (Exception e) {
				System.Console.WriteLine (e);
				return null;
			}
		} 

		#endregion
	}
}


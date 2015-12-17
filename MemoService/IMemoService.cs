using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace NickServer
{

	[ServiceContract]
	public interface IMemoService
	{
		[OperationContract]
		void putPublicMemo (string message, string sender, string hash);

		[OperationContract]
		void putMemo (string message, string to, string from, string hash);

		[OperationContract]
		List<MemoData> getMyMemos (string nick, string hash);
	}

	[DataContract]
	public class MemoData {

		[DataMember]
		public string Sender { get; set;}

		[DataMember]
		public string Message { get; set;}

		[DataMember]
		public DateTime Timestamp { get; set;}

	}
}


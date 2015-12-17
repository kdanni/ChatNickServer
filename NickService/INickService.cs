using System;
using System.ServiceModel;

namespace NickServer
{
	[ServiceContract]
	public interface INickService
	{
		[OperationContract]
		bool registerNick (string nick, string hash);

	}
}


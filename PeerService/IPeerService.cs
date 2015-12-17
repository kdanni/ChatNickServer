using System;
using System.ServiceModel;
using System.Collections.Generic;

namespace NickServer
{
	[ServiceContract]
	public interface IPeerService
	{

		[OperationContract]
		bool registerPeer (string hash, Uri clientUri);


		[OperationContract]
		bool kickoutPeer (string hash, string kickerHash);

		[OperationContract]
		List<Uri> getPeerList ();

	}
}


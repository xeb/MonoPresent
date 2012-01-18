using System;
using System.ServiceModel;

namespace WcfHost
{
	[ServiceContract(Namespace = "WcfHost")]
	public interface ITestService
	{
		[OperationContract]
		string GetTime();
	}
}


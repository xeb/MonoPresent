using System;
using System.ServiceModel;

namespace WcfHost
{
	[ServiceBehavior(IncludeExceptionDetailInFaults=true)]
	public class TestService : ITestService
	{
		public string GetTime()
		{
			return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
		}
	}
}


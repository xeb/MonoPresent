using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WcfHost
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Starting basicHttp bound service...");
			
		    var binding = new BasicHttpBinding ();
		    var address = new Uri ("http://localhost:8090");
		    var host = new ServiceHost (typeof (TestService), address);
			
		    host.AddServiceEndpoint (typeof (ITestService), binding, "");
			
			// Add the MEX
			var smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
			if (smb == null)
			{
				smb = new ServiceMetadataBehavior();
				host.Description.Behaviors.Add(smb);
			}
			
			smb.HttpGetEnabled = true;
			
			//host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
		    host.Open ();
		    
			Console.WriteLine ("Type [Enter] to stop...");
		    Console.ReadLine ();
			
		    host.Close ();
					
		}
	}
}

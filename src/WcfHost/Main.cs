using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WcfHost
{
	public class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Starting test service...");
			
		    var binding = new BasicHttpBinding ();
			//var binding = new NetTcpBinding();
			
		    var baseAddress = new Uri ("http://localhost:8090");
			
			//var endpointAddress = new Uri("net.tcp://localhost:1111");
			var endpointAddress = new Uri("http://localhost:8090");
			
			// ---- ServiceHost & adding endpoints...
		    var host = new ServiceHost (typeof (TestService), baseAddress);
			
		    host.AddServiceEndpoint (typeof (ITestService), binding, endpointAddress);
			
			// Add the MEX
			var smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
			if (smb == null)
			{
				smb = new ServiceMetadataBehavior();
				host.Description.Behaviors.Add(smb);
			}
			
			smb.HttpGetEnabled = true;
			
			host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
		    host.Open ();
		    
			Console.WriteLine("Testing the connection using {0}...", binding.GetType ().Name);
			
			var sw = new Stopwatch();
			sw.Start();
			var channelFactory = new ChannelFactory<ITestService>(binding, new EndpointAddress(endpointAddress));
			
			var channel = channelFactory.CreateChannel();
			
			for(int i = 0; i < 5; i++)
			{
				Console.WriteLine("Connecting...");
				Console.WriteLine (channel.GetTime());
			}
			
			channelFactory.Close ();
			
			sw.Stop();
			Console.WriteLine("Took {0}ms", sw.ElapsedMilliseconds);
			
			Console.WriteLine ("Type [Enter] to stop...");
			Console.ReadLine();
			
		    host.Close ();
					
		}
	}
}

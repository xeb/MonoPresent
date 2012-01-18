using System;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;

namespace ThreadSpeedTest
{
	public class MainClass
	{
		private static List<long> _allTimes = new List<long>();
		private static List<long> _batchTimes = new List<long>();
		
		public static void Main (string[] args)
		{
			int numBatches = 10;
			
			if(args != null && args.Length >= 1 && !int.TryParse(args[0], out numBatches))
			{
				numBatches = 10;
			}
			
			bool synchronous = false;
			
			if(args != null && args.Length >= 2 && !bool.TryParse(args[1], out synchronous))
			{
				synchronous = false;
			}
			
			// 100 batches
			for(int batch = 1; batch <= numBatches; batch++)
			{
				_allTimes.AddRange(_batchTimes);
				_batchTimes.Clear();
				
				if(synchronous)
				{
					GetASite();
				}
				else
				{
					List<Task> tasks = new List<Task>();
					
					// Get Google 5 times
					for(int i = 0; i < 5; i++)
					{
						tasks.Add(Task.Factory.StartNew(GetASite));
					}
					
					Task.WaitAll(tasks.ToArray());
				}
				
				Console.WriteLine("{0} Batch {1}, Avg={2}, Min={3}, Max={4}", 
				                  synchronous ? "Synchronous" : "Asynchronous",
				                  batch, _batchTimes.Average(), _batchTimes.Min(), _batchTimes.Max());
			}
			
			Console.WriteLine ("All, Avg={0}, Min={1}, Max={2}", _allTimes.Average(), _allTimes.Min(), _allTimes.Max());
		}
		
		private static void GetASite()
		{
			var sw = new Stopwatch();
			sw.Start();
			var wc = new WebClient();
			wc.Headers.Add("user-agent","Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/18.6.872.0 Safari/535.2 UNTRUSTED/1.0 3gpp-gba UNTRUSTED/1.0");
			
			// Assign to a variable just cause
			var data = wc.DownloadString("http://arcaneorb.com/");
			
			sw.Stop();
			
			lock(_batchTimes)
				_batchTimes.Add(sw.ElapsedMilliseconds);
		}
	}
}

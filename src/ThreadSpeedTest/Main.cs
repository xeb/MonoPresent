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
		private enum Operation { Webget, Random }
		private static Operation _operation;
		
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
			
			if(args != null && args.Length >= 3 && args[2].ToLower() == "Random")
			{
				_operation = Operation.Random;
			}
			
			// 100 batches
			for(int batch = 1; batch <= numBatches; batch++)
			{
				_allTimes.AddRange(_batchTimes);
				_batchTimes.Clear();
				
				if(synchronous)
				{
					DoSomething();
				}
				else
				{
					List<Task> tasks = new List<Task>();
					
					// Get Google 5 times
					for(int i = 0; i < 5; i++)
					{
						tasks.Add(Task.Factory.StartNew(DoSomething));
					}
					
					Task.WaitAll(tasks.ToArray());
				}
				
				Console.WriteLine("{0} Batch {1}, Avg={2}, Min={3}, Max={4}", 
				                  (synchronous ? "Synchronous" : "Asynchronous") + " " + (_operation == Operation.Random ? "Random" : "Web"),
				                  batch, _batchTimes.Average(), _batchTimes.Min(), _batchTimes.Max());
			}
			
			Console.WriteLine ("All, Avg={0}, Min={1}, Max={2}", _allTimes.Average(), _allTimes.Min(), _allTimes.Max());
		}
		
		private static void DoSomething()
		{
			var sw = new Stopwatch();
			sw.Start();
			
			switch(_operation)
			{
				case Operation.Webget:
					GetWebsite();
					break;
					
				case Operation.Random:
					GenerateRandom();
					break;
			}
			
			sw.Stop();
			
			lock(_batchTimes)
				_batchTimes.Add(sw.ElapsedMilliseconds);
		}
		
		private static void GenerateRandom()
		{
			var rand = new Random();
			
			for(int i = 0; i < 1000; i++)
			{
				rand.NextDouble();
			}
			
			System.Threading.Thread.Sleep(500);
		}
		
		private static void GetWebsite()
		{
			var wc = new WebClient();
			wc.Headers.Add("user-agent","Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/18.6.872.0 Safari/535.2 UNTRUSTED/1.0 3gpp-gba UNTRUSTED/1.0");
			
			// Assign to a variable just cause
			var data = wc.DownloadString("http://arcaneorb.com/");
			
		}
	}
}

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
			
			// 100 batches
			for(int batch = 1; batch <= 100; batch++)
			{
				_allTimes.AddRange(_batchTimes);
				_batchTimes.Clear();
				List<Task> tasks = new List<Task>();
				
				// Get Google 5 times
				for(int i = 0; i < 5; i++)
				{
					tasks.Add(Task.Factory.StartNew(GetGoogle));
				}
				
				Task.WaitAll(tasks.ToArray());
					
				Console.WriteLine("Batch {0}, Avg={1}, Min={2}, Max={3}", batch, _batchTimes.Average(), _batchTimes.Min(), _batchTimes.Max());
			}
			
			Console.WriteLine ("All, Avg={0}, Min={1}, Max={2}", _allTimes.Average(), _allTimes.Min(), _allTimes.Max());
		}
		
		private static void GetGoogle()
		{
			var sw = new Stopwatch();
			sw.Start();
			var wc = new WebClient();
			
			// Assign to a variable just cause
			var data = wc.DownloadString("http://www.google.com");
			data.GetHashCode();
			
			sw.Stop();
			
			lock(_batchTimes)
				_batchTimes.Add(sw.ElapsedMilliseconds);
		}
	}
}

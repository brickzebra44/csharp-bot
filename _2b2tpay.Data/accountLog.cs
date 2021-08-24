using System;

namespace _2b2tpay.Data
{
	public struct accountLog
	{
		public string detailOfLog;

		public DateTime timeOfLog;

		public accountLog(string x)
		{
			detailOfLog = x;
			timeOfLog = DateTime.Now;
		}
	}
}

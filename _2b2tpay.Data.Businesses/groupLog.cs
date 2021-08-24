using System;

namespace _2b2tpay.Data.Businesses
{
	public struct groupLog
	{
		public string detailOfLog;

		public DateTime timeOfLog;

		public groupLog(string x)
		{
			detailOfLog = x;
			timeOfLog = DateTime.Now;
		}
	}
}

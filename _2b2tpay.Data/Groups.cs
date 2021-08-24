using System;
using System.Collections.Generic;

namespace _2b2tpay.Data
{
	public static class Groups
	{
		public struct Group
		{
			public int groupId;

			public decimal ballance;

			public List<groupLog> groupLogs;

			public List<groupLog> transactions;

			public bool isPublic;

			public List<ulong> members;

			public List<ulong> owner;
		}

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
}

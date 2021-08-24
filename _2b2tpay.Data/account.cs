using System.Collections.Generic;

namespace _2b2tpay.Data
{
	public struct account
	{
		public string name;

		public int accountId;

		public ulong accountUserId;

		public decimal ballance;

		public List<accountLog> accountLogs;

		public List<accountLog> transactions;

		public bool isPublic;
	}
}

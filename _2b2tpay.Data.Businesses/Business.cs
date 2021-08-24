using System.Collections.Generic;

namespace _2b2tpay.Data.Businesses
{
	public class Business
	{
		public string name = "";

		public string description = "";

		public int id = 0;

		public decimal ballance = default(decimal);

		public List<groupLog> logs = new List<groupLog>();

		public bool isPublic = true;

		public List<ulong> members = new List<ulong>();

		public List<ulong> owners = new List<ulong>();
	}
}

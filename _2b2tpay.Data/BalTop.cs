using System;
using System.Collections.Generic;
using System.Linq;

namespace _2b2tpay.Data
{
	public static class BalTop
	{
		public static List<account> SortAccounts(List<account> accounts)
		{
			List<account> list = new List<account>();
			foreach (account account in accounts)
			{
				if (account.isPublic)
				{
					list.Add(account);
				}
			}
			return Enumerable.ToList<account>((IEnumerable<account>)Enumerable.OrderByDescending<account, decimal>((IEnumerable<account>)list, (Func<account, decimal>)((account xz) => xz.ballance)));
		}

		public static List<account> RealSortAccounts(List<account> accounts)
		{
			List<account> list = new List<account>();
			foreach (account account in accounts)
			{
				list.Add(account);
			}
			return Enumerable.ToList<account>((IEnumerable<account>)Enumerable.OrderByDescending<account, decimal>((IEnumerable<account>)list, (Func<account, decimal>)((account xz) => xz.ballance)));
		}
	}
}

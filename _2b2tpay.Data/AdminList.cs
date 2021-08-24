using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace _2b2tpay.Data
{
	public class AdminList
	{
		public static List<ulong> ListOfAdminIds = new List<ulong>();

		public static void AddToList(ulong ID)
		{
			List<ulong> list = new List<ulong>();
			using (StreamReader streamReader = new StreamReader("data/adminList.json"))
			{
				list = JsonConvert.DeserializeObject<List<ulong>>(streamReader.ReadToEnd());
				streamReader.Close();
				list.Add(ID);
				using StreamWriter streamWriter = new StreamWriter("data/adminList.json");
				streamWriter.WriteLine(JsonConvert.SerializeObject((object)list));
			}
			ListOfAdminIds = list;
		}

		public static void UpdateAdminList()
		{
			if (!File.Exists("data/adminList.json"))
			{
				ListOfAdminIds.Add(638246313506373666uL);
				using StreamWriter streamWriter = new StreamWriter("data/adminList.json");
				streamWriter.WriteLine(JsonConvert.SerializeObject((object)ListOfAdminIds));
				streamWriter.Close();
				Console.WriteLine("Upading admin list...");
			}
			List<ulong> listOfAdminIds = new List<ulong>();
			using (StreamReader streamReader = new StreamReader("data/adminList.json"))
			{
				listOfAdminIds = JsonConvert.DeserializeObject<List<ulong>>(streamReader.ReadToEnd());
				streamReader.Close();
			}
			ListOfAdminIds = listOfAdminIds;
		}

		public static void RemoveAdmin(ulong ID)
		{
			List<ulong> list = new List<ulong>();
			using (StreamReader streamReader = new StreamReader("data/adminList.json"))
			{
				list = JsonConvert.DeserializeObject<List<ulong>>(streamReader.ReadToEnd());
				streamReader.Close();
				List<ulong> list2 = new List<ulong>();
				foreach (ulong item in list)
				{
					if (item != ID)
					{
						list2.Add(item);
					}
				}
				using StreamWriter streamWriter = new StreamWriter("data/adminList.json");
				streamWriter.WriteLine(JsonConvert.SerializeObject((object)list2));
				streamWriter.Close();
			}
			ListOfAdminIds = list;
		}
	}
}

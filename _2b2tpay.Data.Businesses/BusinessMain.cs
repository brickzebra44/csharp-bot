using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace _2b2tpay.Data.Businesses
{
	public static class BusinessMain
	{
		public static string directory = "businesses/";

		public static List<Business> businesses = new List<Business>();

		private static Random gen = new Random();

		public static void CreateBuiness(string name, string description, ulong owner)
		{
			Business business = new Business();
			business.name = name;
			business.description = description;
			business.owners = new List<ulong>
			{
				owner
			};
			business.members = new List<ulong>();
			business.isPublic = true;
			business.ballance = default(decimal);
			int id;
			do
			{
				id = gen.Next(0, 923456578);
			}
			while (File.Exists(directory + id));
			business.id = id;
			SaveBuiness(business);
		}

		public static void SaveBuiness(Business business)
		{
			if (!File.Exists(directory + business.id))
			{
				File.Create(directory + business.id);
			}
			using StreamWriter streamWriter = new StreamWriter(directory + business.id);
			streamWriter.WriteLine(JsonConvert.SerializeObject((object)business));
		}

		public static void LoadBusiness(string name)
		{
		}
	}
}

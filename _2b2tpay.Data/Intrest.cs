using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace _2b2tpay.Data
{
	public class Intrest
	{
		public struct IntrestData
		{
			public DateTime nextIntrestPayout;

			public decimal percetage;

			public List<IntrestLog> intrestLogs;
		}

		public struct IntrestLog
		{
			public DateTime PayoutDate;

			public decimal percentage;

			public IntrestLog(DateTime dateTime, decimal number)
			{
				PayoutDate = dateTime;
				percentage = number;
			}
		}

		public static decimal rate = 2m;

		public static DateTime nextPayout;

		public static bool checkNextIntrestPayout()
		{
			if (!File.Exists("nextIntrestPayout.json"))
			{
				Console.WriteLine("Intrest doesnt exsist, creating a new file for it: nextIntrestPayout.json");
				IntrestData intrestData = default(IntrestData);
				intrestData.nextIntrestPayout = DateTime.Now;
				intrestData.percetage = 2m;
				intrestData.intrestLogs = new List<IntrestLog>();
				using StreamWriter streamWriter = new StreamWriter("nextIntrestPayout.json");
				streamWriter.WriteLine(JsonConvert.SerializeObject((object)intrestData));
				Console.WriteLine("Created file: nextIntrestPayout.json");
			}
			IntrestData intrestData2 = default(IntrestData);
			using (StreamReader streamReader = new StreamReader("nextIntrestPayout.json"))
			{
				intrestData2 = JsonConvert.DeserializeObject<IntrestData>(streamReader.ReadToEnd());
				rate = intrestData2.percetage;
				nextPayout = intrestData2.nextIntrestPayout;
				streamReader.Close();
				if (DateTime.Now >= intrestData2.nextIntrestPayout)
				{
					Console.WriteLine("Paying out intrest...");
					PayIntrest(intrestData2.percetage);
					File.WriteAllText("nextIntrestPayout.json", "");
					using (StreamWriter streamWriter2 = new StreamWriter("nextIntrestPayout.json"))
					{
						intrestData2.intrestLogs.Add(new IntrestLog(DateTime.Now, intrestData2.percetage));
						intrestData2.nextIntrestPayout = intrestData2.nextIntrestPayout.AddDays(7.0);
						Console.WriteLine("nextPayout: " + intrestData2.nextIntrestPayout);
						streamWriter2.WriteLine(JsonConvert.SerializeObject((object)intrestData2));
					}
					return true;
				}
			}
			return false;
		}

		public static void PayIntrest(decimal intrestRate)
		{
			string[] files = Directory.GetFiles("accounts/");
			string[] array = files;
			foreach (string path in array)
			{
				account account = default(account);
				using StreamReader streamReader = new StreamReader(path);
				account = JsonConvert.DeserializeObject<account>(streamReader.ReadToEnd());
				streamReader.Close();
				using StreamWriter streamWriter = new StreamWriter(path);
				decimal d = intrestRate / 0m * account.ballance;
				d = Math.Round(d, 2);
				account.ballance += d;
				Console.WriteLine("Intrest for `" + account.accountId + "' is: " + d + "%");
				account.accountLogs.Add(new accountLog("Intrest " + account.ballance + " gold at " + d + "%"));
				account.transactions.Add(new accountLog("Intrest  " + account.ballance + " gold at " + d + "%"));
				streamWriter.WriteLine(JsonConvert.SerializeObject((object)account));
			}
		}
	}
}

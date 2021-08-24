using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _2b2tpay.Moduels;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace _2b2tpay.Data
{
	public class Job
	{
		public struct JobType
		{
			public string name;

			public decimal payRatePerWeek;

			public bool isPercentage;

			public JobType(string NAME, decimal PAYRATE, bool percetnage)
			{
				name = NAME;
				payRatePerWeek = PAYRATE;
				isPercentage = percetnage;
			}
		}

		public static List<JobType> jobs = new List<JobType>();

		public static bool hasBeenPayed = false;

		public static decimal temp_JobPayments = default(decimal);

		public static async Task CheckJobPayments(SocketGuildUser[] users, SocketCommandContext Context)
		{
			try
			{
				if (DateTime.Now.Date.DayOfWeek != 0)
				{
					return;
				}
				bool isGoingToPayOut = true;
				bool fileIsNull = false;
				if (!File.Exists("data/" + Context.get_Guild().get_Name() + "_payouts.txt"))
				{
					fileIsNull = true;
				}
				Thread.Sleep(1000);
				if (!fileIsNull)
				{
					using StreamReader file2 = new StreamReader("data/" + Context.get_Guild().get_Name() + "_payouts.txt");
					DateTime lastPayout = JsonConvert.DeserializeObject<DateTime>(file2.ReadToEnd());
					Console.WriteLine(lastPayout.Date.ToString() + " : " + DateTime.Now.Date);
					if (lastPayout.Date == DateTime.Now.Date)
					{
						isGoingToPayOut = false;
					}
					else
					{
						file2.Close();
						using StreamWriter streamWriter = new StreamWriter("data/" + Context.get_Guild().get_Name() + "_payouts.txt");
						streamWriter.WriteLine(JsonConvert.SerializeObject((object)DateTime.Now));
						streamWriter.Close();
					}
				}
				else
				{
					using StreamWriter streamWriter2 = new StreamWriter("data/" + Context.get_Guild().get_Name() + "_payouts.txt");
					streamWriter2.WriteLine(JsonConvert.SerializeObject((object)DateTime.Now));
					streamWriter2.Close();
				}
				if (!isGoingToPayOut)
				{
					return;
				}
				foreach (SocketGuildUser user in users)
				{
					foreach (JobType job in jobs)
					{
						IRole jobIf = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)((IGuildUser)user).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains(job.name)));
						if (!Enumerable.Contains<IRole>((IEnumerable<IRole>)user.get_Roles(), jobIf))
						{
							continue;
						}
						try
						{
							account result = default(account);
							using (StreamReader file = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
							{
								result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
								file.Close();
								if (!job.isPercentage)
								{
									result.ballance += job.payRatePerWeek;
									temp_JobPayments += job.payRatePerWeek;
									List<accountLog> accountLogs = result.accountLogs;
									string[] obj = new string[5]
									{
										"[Job Wage] Your job as a ",
										job.name,
										" was payed ",
										null,
										null
									};
									decimal payRatePerWeek = job.payRatePerWeek;
									obj[3] = payRatePerWeek.ToString();
									obj[4] = " gold.";
									accountLogs.Add(new accountLog(string.Concat(obj)));
									List<accountLog> transactions = result.transactions;
									string[] obj2 = new string[5]
									{
										"Your job as a ",
										job.name,
										" was payed ",
										null,
										null
									};
									payRatePerWeek = job.payRatePerWeek;
									obj2[3] = payRatePerWeek.ToString();
									obj2[4] = " gold.";
									transactions.Add(new accountLog(string.Concat(obj2)));
									using (StreamWriter streamWriter3 = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
									{
										streamWriter3.WriteLine(JsonConvert.SerializeObject((object)result));
									}
									Task<IDMChannel> h3 = ((SocketUser)user).GetOrCreateDMChannelAsync((RequestOptions)null);
									IDMChannel result2 = h3.Result;
									string title = "Wage for " + job.name;
									payRatePerWeek = job.payRatePerWeek;
									await ((IMessageChannel)result2).SendMessageAsync("", false, Commands.SendEmmbedMessage(title, "You have been payed " + payRatePerWeek + " gold, for this week's work! Your wage is payed out every Sunday.", randomImperialImage: true), (RequestOptions)null);
								}
								else
								{
									result.ballance += job.payRatePerWeek;
									temp_JobPayments += job.payRatePerWeek;
									List<accountLog> accountLogs2 = result.accountLogs;
									string[] obj3 = new string[5]
									{
										"[Job Wage] Your job as a ",
										job.name,
										" was payed ",
										null,
										null
									};
									decimal payRatePerWeek = job.payRatePerWeek;
									obj3[3] = payRatePerWeek.ToString();
									obj3[4] = " gold.";
									accountLogs2.Add(new accountLog(string.Concat(obj3)));
									List<accountLog> transactions2 = result.transactions;
									string[] obj4 = new string[5]
									{
										"Your job as a ",
										job.name,
										" was payed ",
										null,
										null
									};
									payRatePerWeek = job.payRatePerWeek;
									obj4[3] = payRatePerWeek.ToString();
									obj4[4] = " gold.";
									transactions2.Add(new accountLog(string.Concat(obj4)));
									using (StreamWriter accountFile = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
									{
										accountFile.WriteLine(JsonConvert.SerializeObject((object)result));
									}
									Task<IDMChannel> h2 = ((SocketUser)user).GetOrCreateDMChannelAsync((RequestOptions)null);
									IDMChannel result3 = h2.Result;
									string title2 = "Wage for " + job.name;
									payRatePerWeek = job.payRatePerWeek;
									await ((IMessageChannel)result3).SendMessageAsync("", false, Commands.SendEmmbedMessage(title2, "You have been payed " + payRatePerWeek + " gold, for this week's work! Your wage is payed out every Sunday.", randomImperialImage: true), (RequestOptions)null);
								}
							}
							result = default(account);
						}
						catch (Exception e2)
						{
							Console.WriteLine("-- JOB ERROR -- ");
							Console.WriteLine(e2.ToString());
							Task<IDMChannel> h = ((SocketUser)user).GetOrCreateDMChannelAsync((RequestOptions)null);
							await ((IMessageChannel)h.Result).SendMessageAsync("", false, Commands.SendEmmbedMessage("Your job payment for " + job.name + " had an error!", "This is a message to let you know that your " + job.name + " wage might not have been sucsesfull, if it was not please contact a high rank."), (RequestOptions)null);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Exception e = ex;
				Console.WriteLine("JOB PAYMENT ERRO --> " + e.ToString());
			}
		}
	}
}

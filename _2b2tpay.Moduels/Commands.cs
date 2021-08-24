using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _2b2tpay.Data;
using _2b2tpay.Data.Businesses;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace _2b2tpay.Moduels
{
	public class Commands : InteractiveBase
	{
		public struct Rank
		{
			public string name;

			public decimal cost;

			public int minimumDays;

			public Rank(string NAME, decimal COST, int MINDAYS)
			{
				name = NAME;
				cost = COST;
				minimumDays = MINDAYS;
			}
		}

		public class TrendData
		{
			public DateTime date;

			public decimal gainGold = default(decimal);

			public decimal looseGold = default(decimal);

			public decimal transactionGold = default(decimal);

			public int timesCheckedBal = 0;

			public int patriatismCount = 0;

			public int timesRankedUp = 0;

			public int timesCheckedLogs = 0;

			public int timesMadePublic = 0;

			public int timesMadePrivate = 0;

			public string server;
		}

		private static Random random = new Random();

		public List<Rank> ranks = new List<Rank>
		{
			new Rank("Recruit", 5m, 2),
			new Rank("Auxiliary", 60m, 2),
			new Rank("Quaestor", 100m, 5),
			new Rank("Preafect", 200m, 10),
			new Rank("Tribune", 400m, 20),
			new Rank("Legate", 500m, 30)
		};

		public List<TrendData> trendData = new List<TrendData>();

		public ulong SENATE_GUILD = 721677685431992360uL;

		[Command("bal")]
		public async Task Bal(SocketUser user = null)
		{
			if (user == null)
			{
				SocketUser userInfo = base.get_Context().get_User();
				if (!File.Exists("accounts/" + ((SocketEntity<ulong>)(object)userInfo).get_Id()))
				{
					Random random2 = new Random();
					account account2 = default(account);
					account2.accountUserId = ((SocketEntity<ulong>)(object)userInfo).get_Id();
					account2.name = userInfo.get_Username();
					account2.accountId = random2.Next(0, 99999999);
					account2.ballance = default(decimal);
					account2.accountLogs = new List<accountLog>();
					account2.accountLogs.Add(new accountLog("Created account"));
					account2.accountLogs.Add(new accountLog("Credit: 0 gold"));
					account2.transactions = new List<accountLog>();
					account2.transactions.Add(new accountLog("Credit: 0 gold"));
					account2.isPublic = true;
					using (StreamWriter streamWriter = new StreamWriter("accounts/" + account2.accountUserId))
					{
						streamWriter.WriteLine(JsonConvert.SerializeObject((object)account2));
					}
					account2 = default(account);
				}
				account result2 = default(account);
				using (StreamReader streamReader = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)userInfo).get_Id()))
				{
					result2 = JsonConvert.DeserializeObject<account>(streamReader.ReadToEnd());
					streamReader.Close();
					result2.accountLogs.Add(new accountLog("Accessed account balace"));
					using StreamWriter streamWriter2 = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)userInfo).get_Id());
					streamWriter2.WriteLine(JsonConvert.SerializeObject((object)result2));
				}
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage($"{result2.ballance:0.00}" + " gold"), (RequestOptions)null);
			}
			else
			{
				if (!File.Exists("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
				{
					Random random = new Random();
					account account = default(account);
					account.accountUserId = ((SocketEntity<ulong>)(object)user).get_Id();
					account.name = user.get_Username();
					account.accountId = random.Next(0, 99999999);
					account.ballance = default(decimal);
					account.accountLogs = new List<accountLog>();
					account.accountLogs.Add(new accountLog("Created account"));
					account.accountLogs.Add(new accountLog("Credit: 0 gold"));
					account.transactions = new List<accountLog>();
					account.transactions.Add(new accountLog("Credit: 0 gold"));
					account.isPublic = true;
					using (StreamWriter streamWriter3 = new StreamWriter("accounts/" + account.accountUserId))
					{
						streamWriter3.WriteLine(JsonConvert.SerializeObject((object)account));
					}
					account = default(account);
				}
				account result = default(account);
				using (StreamReader file = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
				{
					result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
					file.Close();
					if (!result.isPublic)
					{
						await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("This user's account is currently private and you cannot see what they have."), (RequestOptions)null);
						return;
					}
					result.accountLogs.Add(new accountLog("Accessed account balace"));
					using StreamWriter accountFile = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id());
					accountFile.WriteLine(JsonConvert.SerializeObject((object)result));
				}
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage($"{result.ballance:0.00}" + "gold"), (RequestOptions)null);
			}
			if (base.get_Context().get_Guild().get_Name() != "The Imperials")
			{
				await Job.CheckJobPayments(Enumerable.ToArray<SocketGuildUser>((IEnumerable<SocketGuildUser>)base.get_Context().get_Guild().get_Users()), base.get_Context());
			}
			AddTrendData(new TrendData
			{
				timesCheckedBal = 1,
				gainGold = Job.temp_JobPayments
			}, DateTime.Now, base.get_Context().get_Guild().get_Name());
			Job.temp_JobPayments = default(decimal);
		}

		public static Embed SendEmmbedMessage(string title, string message = "", bool randomImperialImage = false)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			EmbedBuilder val = new EmbedBuilder();
			val.set_Author(new EmbedAuthorBuilder());
			val.get_Author().set_Name(title);
			val.set_Description(message);
			val.WithColor(Color.Gold);
			string[] array = new string[6]
			{
				"https://cdn.discordapp.com/attachments/692682149383634944/692682662493552720/ImperatorWhite.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/692682283651694653/coolguy.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/693354954206740520/LEGIONARY.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/693354975497158737/d760cdc42b97b1da71461d613a7c561b9b2eb43e_hq.jpg",
				"https://cdn.discordapp.com/attachments/692682149383634944/693355033705709638/ImperialCoin.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/693355092149010442/steamworkshop_webupload_previewfile_377822609_preview.png"
			};
			if (randomImperialImage)
			{
				val.WithThumbnailUrl(array[random.Next(0, array.Length)]);
			}
			return val.Build();
		}

		[Command("pay")]
		public async Task Send(SocketUser user = null, string gold = "X")
		{
			if (user == base.get_Context().get_User())
			{
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("No! You can't pay yourself! " + base.get_Context().get_User().get_Mention() + " Patched due to an exploit.", false, (Embed)null, (RequestOptions)null);
				return;
			}
			Console.WriteLine(base.get_Context().get_User().get_Username());
			if (gold.Contains("$"))
			{
				gold = gold.TrimStart(new char[1]
				{
					'$'
				});
				gold = gold.TrimEnd(new char[1]
				{
					'$'
				});
			}
			if (user == null || gold == "X")
			{
				if (user == null)
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("A user must be mentioned.", false, (Embed)null, (RequestOptions)null);
				}
				else
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Gold must be 1 or above and in numeric form", false, (Embed)null, (RequestOptions)null);
				}
				return;
			}
			gold = PaymentVars.CheckData(gold);
			decimal ammountToRemove = default(decimal);
			if (!decimal.TryParse(gold, out ammountToRemove))
			{
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Value of gold must be a number.", false, (Embed)null, (RequestOptions)null);
				return;
			}
			if (ammountToRemove < 0.01m)
			{
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Must be 0.01 gold or above.", false, (Embed)null, (RequestOptions)null);
				return;
			}
			try
			{
				if (user == null)
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("You need to mention someone when sending gold!! | !pay @user x", false, (Embed)null, (RequestOptions)null);
					return;
				}
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Sending " + user.get_Username() + " " + gold + " gold...", false, (Embed)null, (RequestOptions)null);
				account result = default(account);
				using (StreamReader file = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id()))
				{
					result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
					file.Close();
					if (result.ballance < ammountToRemove)
					{
						await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("You don't have enough gold to send.", false, (Embed)null, (RequestOptions)null);
						return;
					}
					result.ballance -= ammountToRemove;
					result.accountLogs.Add(new accountLog("Sent " + user.get_Username() + " " + gold + " gold"));
					result.transactions.Add(new accountLog("sent " + user.get_Username() + " " + gold + " gold"));
					using StreamWriter streamWriter = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id());
					streamWriter.WriteLine(JsonConvert.SerializeObject((object)result));
				}
				bool hasAnAccount = true;
				if (!File.Exists("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
				{
					hasAnAccount = false;
					Random random = new Random();
					account account = default(account);
					account.accountUserId = ((SocketEntity<ulong>)(object)user).get_Id();
					account.name = user.get_Username();
					account.accountId = random.Next(0, 99999999);
					account.ballance = default(decimal);
					account.accountLogs = new List<accountLog>();
					account.accountLogs.Add(new accountLog("Created account"));
					account.accountLogs.Add(new accountLog("Credit: 0 gold"));
					account.transactions = new List<accountLog>();
					account.transactions.Add(new accountLog("Credit: 0 gold"));
					account.isPublic = true;
					using (StreamWriter streamWriter2 = new StreamWriter("accounts/" + account.accountUserId))
					{
						streamWriter2.WriteLine(JsonConvert.SerializeObject((object)account));
					}
					account = default(account);
				}
				account result2 = default(account);
				using (StreamReader streamReader = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
				{
					result2 = JsonConvert.DeserializeObject<account>(streamReader.ReadToEnd());
					streamReader.Close();
					result2.ballance += ammountToRemove;
					result2.accountLogs.Add(new accountLog("Recivved " + gold + " gold from " + (object)base.get_Context().get_User()));
					result2.transactions.Add(new accountLog("received  " + gold + " gold from " + (object)base.get_Context().get_User()));
					using StreamWriter accountFile = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id());
					accountFile.WriteLine(JsonConvert.SerializeObject((object)result2));
				}
				Thread.Sleep(1500);
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Sent " + user.get_Username() + " " + gold + " gold", "If " + user.get_Username() + " has not recived the gold. Your or " + user.get_Username() + " need to contact an admin. You can check you have the gold by doing !transactions"), (RequestOptions)null);
				AddTrendData(new TrendData
				{
					transactionGold = ammountToRemove
				}, DateTime.Now, base.get_Context().get_Guild().get_Name());
				try
				{
					Task<IDMChannel> h = user.GetOrCreateDMChannelAsync((RequestOptions)null);
					if (!hasAnAccount)
					{
						await ((IMessageChannel)h.Result).SendMessageAsync("", false, SendEmmbedMessage("You have recvived " + gold + " gold from " + base.get_Context().get_User().get_Username(), "If you would like to find out more about Imperial Gold, join the discord here: https://discord.gg/QsuTcrt"), (RequestOptions)null);
					}
					else
					{
						await ((IMessageChannel)h.Result).SendMessageAsync("", false, SendEmmbedMessage("You have recvived " + gold + " gold from " + base.get_Context().get_User().get_Username(), "This is a notification to let you know that you have gained some more gold!"), (RequestOptions)null);
					}
				}
				catch
				{
					Thread.Sleep(500);
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Sorry, but discord has blocked the delivery message directed to " + user.get_Username() + ", if you want him to be notifyed of this transaction you may need to notify him yourself."), (RequestOptions)null);
				}
				result = default(account);
				result2 = default(account);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				if (!e.Message.ToLower().Contains("user not found"))
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Sorry, something went wrong when doing the transaction.", false, (Embed)null, (RequestOptions)null);
				}
				else
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("You to need to mention a real user", false, (Embed)null, (RequestOptions)null);
				}
			}
		}

		[Command("adminpay")]
		public async Task adminpay(SocketUser user = null, string gold = "X")
		{
			SocketUser user2 = base.get_Context().get_User();
			IRole role = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user2 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name() == "Imperial Gold Manager"));
			SocketUser user3 = base.get_Context().get_User();
			if (!Enumerable.Contains<IRole>((IEnumerable<IRole>)(user3 as SocketGuildUser).get_Roles(), role))
			{
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Plebs do not have permission to preform this command!", false, (Embed)null, (RequestOptions)null);
				return;
			}
			try
			{
				if (gold.Contains("$"))
				{
					gold = gold.TrimStart(new char[1]
					{
						'$'
					});
					gold = gold.TrimEnd(new char[1]
					{
						'$'
					});
				}
				if (user == null || gold == "X")
				{
					if (user != null)
					{
						await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Must be 0.01 gold or above and in numeric form", false, (Embed)null, (RequestOptions)null);
					}
					else
					{
						await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("A user must be mentioned.", false, (Embed)null, (RequestOptions)null);
					}
					return;
				}
				gold = PaymentVars.CheckData(gold);
				decimal ammountToRemove = default(decimal);
				if (!decimal.TryParse(gold, out ammountToRemove))
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Value of gold must be a number.", false, (Embed)null, (RequestOptions)null);
					return;
				}
				if (user == null)
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("You need to mention someone when sending gold!! | !pay @user x", false, (Embed)null, (RequestOptions)null);
					return;
				}
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Sending " + user.get_Username() + gold + " gold...", false, (Embed)null, (RequestOptions)null);
				bool hasAnAccount = true;
				if (!File.Exists("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
				{
					hasAnAccount = false;
					Random random = new Random();
					account account = default(account);
					account.accountUserId = ((SocketEntity<ulong>)(object)user).get_Id();
					account.name = user.get_Username();
					account.accountId = random.Next(0, 99999999);
					account.ballance = default(decimal);
					account.accountLogs = new List<accountLog>();
					account.accountLogs.Add(new accountLog("Created account"));
					account.accountLogs.Add(new accountLog("Credit: 0 gold"));
					account.transactions = new List<accountLog>();
					account.transactions.Add(new accountLog("Credit: 0 gold"));
					account.isPublic = true;
					using (StreamWriter streamWriter = new StreamWriter("accounts/" + account.accountUserId))
					{
						streamWriter.WriteLine(JsonConvert.SerializeObject((object)account));
					}
					account = default(account);
				}
				account result2 = default(account);
				using (StreamReader file = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
				{
					result2 = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
					file.Close();
					result2.ballance += ammountToRemove;
					result2.accountLogs.Add(new accountLog("Recivved " + gold + " gold from the Bank of Imperial"));
					result2.transactions.Add(new accountLog("received  " + gold + " gold from the Bank of Imperial"));
					using StreamWriter accountFile = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id());
					accountFile.WriteLine(JsonConvert.SerializeObject((object)result2));
				}
				Thread.Sleep(1500);
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Sent " + user.get_Username() + " " + gold + " gold", "If " + user.get_Username() + " has not recived the gold. Your or " + user.get_Username() + " need to contact an admin. You can check you have the gold by doing !transactions"), (RequestOptions)null);
				AddTrendData(new TrendData
				{
					gainGold = ammountToRemove
				}, DateTime.Now, base.get_Context().get_Guild().get_Name());
				Task<IDMChannel> h = user.GetOrCreateDMChannelAsync((RequestOptions)null);
				if (!hasAnAccount)
				{
					await ((IMessageChannel)h.Result).SendMessageAsync("", false, SendEmmbedMessage("You have recvived " + gold + " gold from " + base.get_Context().get_User().get_Username(), "If you would like to find out more about Imperial Bank, join the discord here: https://discord.gg/QsuTcrt"), (RequestOptions)null);
				}
				else
				{
					await ((IMessageChannel)h.Result).SendMessageAsync("", false, SendEmmbedMessage("You have recvived " + gold + " gold from " + base.get_Context().get_User().get_Username(), "This is a notification to let you know that you have gained some more gold!"), (RequestOptions)null);
				}
				result2 = default(account);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				if (!e.Message.ToLower().Contains("user not found"))
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Sorry, something went wrong when doing the transaction.", false, (Embed)null, (RequestOptions)null);
				}
				else
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("You to need to mention a real user", false, (Embed)null, (RequestOptions)null);
				}
			}
		}

		[Command("adminremove")]
		public async Task adminremove(SocketUser user = null, string gold = "X")
		{
			SocketUser user2 = base.get_Context().get_User();
			IRole role = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user2 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name() == "Imperial Gold Manager"));
			SocketUser user3 = base.get_Context().get_User();
			if (!Enumerable.Contains<IRole>((IEnumerable<IRole>)(user3 as SocketGuildUser).get_Roles(), role))
			{
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Plebs do not have permission to preform this command!", false, (Embed)null, (RequestOptions)null);
				return;
			}
			try
			{
				if (gold.Contains("$"))
				{
					gold = gold.TrimStart(new char[1]
					{
						'$'
					});
					gold = gold.TrimEnd(new char[1]
					{
						'$'
					});
				}
				if (user == null || gold == "X")
				{
					if (user != null)
					{
						await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Must be 0.01 gold or above and in numeric form", false, (Embed)null, (RequestOptions)null);
					}
					else
					{
						await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("A user must be mentioned.", false, (Embed)null, (RequestOptions)null);
					}
					return;
				}
				decimal ammountToRemove = default(decimal);
				if (!decimal.TryParse(gold, out ammountToRemove))
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Value of gold must be a number.", false, (Embed)null, (RequestOptions)null);
					return;
				}
				if (user == null)
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("You need to mention someone when removing gold!! | !adminremove @user x", false, (Embed)null, (RequestOptions)null);
					return;
				}
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Removing " + user.get_Username() + gold + " gold...", false, (Embed)null, (RequestOptions)null);
				bool hasAnAccount = true;
				if (!File.Exists("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
				{
					hasAnAccount = false;
					Random random = new Random();
					account account = default(account);
					account.name = user.get_Username();
					account.accountUserId = ((SocketEntity<ulong>)(object)user).get_Id();
					account.accountId = random.Next(0, 99999999);
					account.ballance = default(decimal);
					account.accountLogs = new List<accountLog>();
					account.accountLogs.Add(new accountLog("Created account"));
					account.accountLogs.Add(new accountLog("Credit: 0 gold"));
					account.transactions = new List<accountLog>();
					account.transactions.Add(new accountLog("Credit: 0 gold"));
					account.isPublic = true;
					using (StreamWriter streamWriter = new StreamWriter("accounts/" + account.accountUserId))
					{
						streamWriter.WriteLine(JsonConvert.SerializeObject((object)account));
					}
					account = default(account);
				}
				account result2 = default(account);
				using (StreamReader file = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
				{
					result2 = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
					file.Close();
					result2.ballance -= ammountToRemove;
					if (result2.ballance < 0m)
					{
						result2.ballance = default(decimal);
					}
					result2.accountLogs.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
					result2.transactions.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
					using StreamWriter accountFile = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id());
					accountFile.WriteLine(JsonConvert.SerializeObject((object)result2));
				}
				Thread.Sleep(1500);
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Removed " + user.get_Username() + " " + gold + " gold", "If " + user.get_Username() + " has not had the gold removed. Your need to contact an admin."), (RequestOptions)null);
				AddTrendData(new TrendData
				{
					looseGold = ammountToRemove
				}, DateTime.Now, base.get_Context().get_Guild().get_Name());
				Task<IDMChannel> h = user.GetOrCreateDMChannelAsync((RequestOptions)null);
				if (!hasAnAccount)
				{
					await ((IMessageChannel)h.Result).SendMessageAsync("", false, SendEmmbedMessage("You have had " + gold + " gold removed, done by Admin " + base.get_Context().get_User().get_Username(), "If you would like to find out more about Imperial Bank, join the discord here: https://discord.gg/QsuTcrt"), (RequestOptions)null);
				}
				else
				{
					await ((IMessageChannel)h.Result).SendMessageAsync("", false, SendEmmbedMessage(gold + " gold has been taken from your account by " + base.get_Context().get_User().get_Username(), "This is a notification to let you know that gold has been removed. If your not sure why it has been removed, check with " + base.get_Context().get_User().get_Username() + " to see why it has been removed."), (RequestOptions)null);
				}
				result2 = default(account);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				if (!e.Message.ToLower().Contains("user not found"))
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Sorry, something went wrong when doing the transaction.", false, (Embed)null, (RequestOptions)null);
				}
				else
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("You to need to mention a real user", false, (Embed)null, (RequestOptions)null);
				}
			}
		}

		public async Task Announce(string title, string message, Color color)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			DiscordSocketClient _client = new DiscordSocketClient();
			ulong id = 657355671070834709uL;
			SocketChannel channel = ((BaseSocketClient)_client).GetChannel(id);
			IMessageChannel chnl = channel as IMessageChannel;
			EmbedBuilder builder = new EmbedBuilder();
			builder.set_Title(title);
			builder.set_Description(message);
			builder.WithColor(color);
			await chnl.SendMessageAsync("", false, builder.Build(), (RequestOptions)null);
		}

		[Command("baltop")]
		public async Task BalTop()
		{
			List<account> accounts = new List<account>();
			string[] accountFiles = Directory.GetFiles("accounts/");
			try
			{
				string[] array = accountFiles;
				foreach (string accountFile in array)
				{
					using StreamReader file = new StreamReader(accountFile);
					account result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
					file.Close();
					accounts.Add(result);
				}
			}
			catch (Exception ex)
			{
				Exception e2 = ex;
				Console.WriteLine("1st");
				Console.WriteLine(e2.ToString());
			}
			try
			{
				string ToSay = "";
				Console.WriteLine("YES");
				accounts = _2b2tpay.Data.BalTop.SortAccounts(accounts);
				int i = 0;
				foreach (account account in accounts)
				{
					try
					{
						if (i >= 10)
						{
							break;
						}
						i++;
						ToSay = ToSay + "[" + i + "] " + ((SocketUser)base.get_Context().get_Guild().GetUser(account.accountUserId)).get_Username() + " | " + $"{account.ballance:0.00}" + " gold" + Environment.NewLine;
						continue;
					}
					catch (Exception)
					{
						string[] obj = new string[9]
						{
							ToSay,
							"[",
							i.ToString(),
							"] ",
							account.name,
							" | ",
							null,
							null,
							null
						};
						decimal ballance = account.ballance;
						obj[6] = ballance.ToString();
						obj[7] = " gold";
						obj[8] = Environment.NewLine;
						ToSay = string.Concat(obj);
						continue;
					}
				}
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Imperial Forbes List", ToSay, randomImperialImage: true), (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine("2nd");
				Console.WriteLine(e.ToString());
				Console.WriteLine(Environment.NewLine + e.Message);
			}
		}

		[Command("private")]
		public async Task Private()
		{
			account result = default(account);
			using (StreamReader file = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id()))
			{
				result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
				file.Close();
				if (!result.isPublic)
				{
					await base.get_Context().get_Channel().SendMessageAsync("You account is already private and will not be displayed on the !baltop list.", false, (Embed)null, (RequestOptions)null);
					return;
				}
				result.isPublic = false;
				result.accountLogs.Add(new accountLog("Set account to private. "));
				using StreamWriter accountFile = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id());
				accountFile.WriteLine(JsonConvert.SerializeObject((object)result));
			}
			await base.get_Context().get_Channel().SendMessageAsync("Your account has been set to private, your account will not be displayed on !baltop, nor will anyone be able to see your account value.", false, (Embed)null, (RequestOptions)null);
			AddTrendData(new TrendData
			{
				timesMadePrivate = 1
			}, DateTime.Now, base.get_Context().get_Guild().get_Name());
		}

		[Command("public")]
		public async Task Public()
		{
			account result = default(account);
			using StreamReader file = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id());
			result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
			file.Close();
			if (result.isPublic)
			{
				await base.get_Context().get_Channel().SendMessageAsync("You account is already public and will be displayed on the !baltop list.", false, (Embed)null, (RequestOptions)null);
				return;
			}
			result.isPublic = true;
			result.accountLogs.Add(new accountLog("Set account to public. "));
			using (StreamWriter accountFile = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id()))
			{
				accountFile.WriteLine(JsonConvert.SerializeObject((object)result));
			}
			await base.get_Context().get_Channel().SendMessageAsync("Your account has been set to public, people will be able to see your account value.", false, (Embed)null, (RequestOptions)null);
			AddTrendData(new TrendData
			{
				timesMadePublic = 1
			}, DateTime.Now, base.get_Context().get_Guild().get_Name());
		}

		[Command("help")]
		public async Task Help()
		{
			await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Commands", "!bal - user ballance" + Environment.NewLine + "!pay - pay a user" + Environment.NewLine + "!baltop - check the top users across the Imperial" + Environment.NewLine + "!public - set your account to public" + Environment.NewLine + "!private - set your account to private" + Environment.NewLine + "!rankup - use gold to rankup to next rank (applies to current branch only)" + Environment.NewLine + "!logs - get all account actions in a text file" + Environment.NewLine + "!transactions - get all transactions made on the account" + Environment.NewLine + "!help data - see data-related commands" + Environment.NewLine + "!it - see users time in imperials" + Environment.NewLine + "!advertisement - for 100 gold will ping @everyone your advertisement (make sure to include it after the command, and include an @ everyone ping)" + Environment.NewLine), (RequestOptions)null);
		}

		[Command("help data")]
		public async Task HelpData()
		{
			await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Commands", "!trends - see today's staticits" + Environment.NewLine + "!gdp - see today's gdp increase percent" + Environment.NewLine + "!compare DATE DATE - compare 2 dates gdp." + Environment.NewLine), (RequestOptions)null);
		}

		[Command("adminprivate")]
		public async Task Private(SocketUser user = null)
		{
			SocketUser user2 = base.get_Context().get_User();
			IRole role = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user2 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name() == "Imperial Gold Manager"));
			SocketUser user3 = base.get_Context().get_User();
			if (!Enumerable.Contains<IRole>((IEnumerable<IRole>)(user3 as SocketGuildUser).get_Roles(), role))
			{
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Plebs do not have permission to preform this command!", false, (Embed)null, (RequestOptions)null);
				return;
			}
			account result = default(account);
			using (StreamReader file = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id()))
			{
				result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
				file.Close();
				if (!result.isPublic)
				{
					await base.get_Context().get_Channel().SendMessageAsync(user.get_Mention() + "'s account is already private and will not be displayed on the !baltop list.", false, (Embed)null, (RequestOptions)null);
					return;
				}
				result.isPublic = false;
				result.accountLogs.Add(new accountLog("Admin set your account to private. "));
				using StreamWriter accountFile = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id());
				accountFile.WriteLine(JsonConvert.SerializeObject((object)result));
			}
			await base.get_Context().get_Channel().SendMessageAsync(user.get_Mention() + "'s account has been set to private, " + user.get_Mention() + "'s  account will not be displayed on !baltop, nor will anyone be able to see " + user.get_Mention() + "'s  account value.", false, (Embed)null, (RequestOptions)null);
			AddTrendData(new TrendData
			{
				timesMadePrivate = 1
			}, DateTime.Now, base.get_Context().get_Guild().get_Name());
			result = default(account);
		}

		[Command("logs")]
		public async Task Logs()
		{
			try
			{
				SocketUser userInfo = base.get_Context().get_User();
				account result = JsonConvert.DeserializeObject<account>(File.ReadAllText("accounts/" + ((SocketEntity<ulong>)(object)userInfo).get_Id()));
				result.accountLogs.Add(new accountLog("Accessed account logs"));
				File.WriteAllText("accounts/" + ((SocketEntity<ulong>)(object)userInfo).get_Id(), JsonConvert.SerializeObject((object)result));
				File.WriteAllText("data\\logs.txt", "Imperial 2020 - Logs of " + userInfo.get_Username() + Environment.NewLine);
				using (StreamWriter w = File.AppendText("data\\logs.txt"))
				{
					foreach (accountLog accountLog in result.accountLogs)
					{
						DateTime timeOfLog = accountLog.timeOfLog;
						w.WriteLine("[" + timeOfLog.ToString() + "] " + accountLog.detailOfLog);
					}
					w.Close();
				}
				await base.get_Context().get_Channel().SendFileAsync("data\\logs.txt", "Here is all of the logged data in a text format.", false, (Embed)null, (RequestOptions)null, false);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + Environment.NewLine + " () -- " + e.ToString());
				await base.get_Context().get_Channel().SendMessageAsync("Sorry, something has gone wrong when trying to ascess your logs. If you desperatly need your logs please contact an admin", false, (Embed)null, (RequestOptions)null);
			}
			AddTrendData(new TrendData
			{
				timesCheckedLogs = 1
			}, DateTime.Now, base.get_Context().get_Guild().get_Name());
		}

		[Command("transactions")]
		public async Task transactions()
		{
			try
			{
				SocketUser userInfo = base.get_Context().get_User();
				account result = JsonConvert.DeserializeObject<account>(File.ReadAllText("accounts/" + ((SocketEntity<ulong>)(object)userInfo).get_Id()));
				File.WriteAllText("accounts/" + ((SocketEntity<ulong>)(object)userInfo).get_Id(), JsonConvert.SerializeObject((object)result));
				File.WriteAllText("data\\transactions.txt", "Imperial 2020 - Transactions of " + userInfo.get_Username() + Environment.NewLine);
				using (StreamWriter w = File.AppendText("data\\transactions.txt"))
				{
					foreach (accountLog accountLog in result.transactions)
					{
						DateTime timeOfLog = accountLog.timeOfLog;
						w.WriteLine("[" + timeOfLog.ToString() + "] " + accountLog.detailOfLog);
					}
					w.Close();
				}
				await base.get_Context().get_Channel().SendFileAsync("data\\transactions.txt", "Here is all of the logged transaction data in a text format.", false, (Embed)null, (RequestOptions)null, false);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + Environment.NewLine + " () -- " + e.ToString());
				await base.get_Context().get_Channel().SendMessageAsync("Sorry, something has gone wrong when trying to ascess your transaction logs. If you desperatly need your transaction logs please contact an admin", false, (Embed)null, (RequestOptions)null);
			}
		}

		[Command("rankup2")]
		public async Task RankUp()
		{
			if (base.get_Context().get_Guild().get_Name() == "The Imperials")
			{
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Sorry! But that is Illegal!", "The main imperial server does not allow rank ups, you will need to choose a branch to use it on."), (RequestOptions)null);
				return;
			}
			try
			{
				int counter = -1;
				SocketUser user = base.get_Context().get_User();
				IRole LEGATE = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Legate")));
				SocketUser user2 = base.get_Context().get_User();
				Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user2 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Recruit")));
				Rank lowestRole = default(Rank);
				Rank highestRole = default(Rank);
				bool hasBeenUsed = false;
				foreach (Rank rank1 in ranks)
				{
					SocketUser user3 = base.get_Context().get_User();
					IRole role2 = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user3 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains(rank1.name)));
					SocketUser user4 = base.get_Context().get_User();
					if (Enumerable.Contains<IRole>((IEnumerable<IRole>)(user4 as SocketGuildUser).get_Roles(), role2))
					{
						if (!hasBeenUsed)
						{
							lowestRole = rank1;
							hasBeenUsed = true;
						}
						highestRole = rank1;
					}
				}
				Console.WriteLine(lowestRole.name);
				Console.WriteLine(highestRole.name);
				Rank rankNeedingToProgressTo = ranks[ranks.IndexOf(highestRole) + 1];
				if (rankNeedingToProgressTo.name == "Recruit")
				{
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You can't purchace membership", "You cannot buy ranks unlesss you apply to the Imperials. You need to be a citzen to join!"), (RequestOptions)null);
					return;
				}
				Console.WriteLine(rankNeedingToProgressTo.name);
				SocketUser user5 = base.get_Context().get_User();
				IRole role = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user5 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains(rankNeedingToProgressTo.name)));
				SocketUser user6 = base.get_Context().get_User();
				if (Enumerable.Contains<IRole>((IEnumerable<IRole>)(user6 as SocketGuildUser).get_Roles(), LEGATE) || Enumerable.Contains<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as SocketGuildUser).get_Roles(), Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Emperor")))) || Enumerable.Contains<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as SocketGuildUser).get_Roles(), Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("High General")))) || Enumerable.Contains<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as SocketGuildUser).get_Roles(), Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("High Centurion")))) || Enumerable.Contains<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as SocketGuildUser).get_Roles(), Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Consul")))) || Enumerable.Contains<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as SocketGuildUser).get_Roles(), Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Centurion")))) || Enumerable.Contains<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as SocketGuildUser).get_Roles(), Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Optio")))) || Enumerable.Contains<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as SocketGuildUser).get_Roles(), Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(base.get_Context().get_User() as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Senator")))))
				{
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Max Rank Achieved!", "Congrats! You have progressed to the highest rank you can purchace!"), (RequestOptions)null);
				}
				else
				{
					decimal amountToRemove = rankNeedingToProgressTo.cost;
					account result = default(account);
					using (StreamReader file = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id()))
					{
						result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
						file.Close();
						if (result.ballance < amountToRemove)
						{
							await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You don't have enough gold to purchace rank " + rankNeedingToProgressTo.name + "!", "You need " + (amountToRemove - result.ballance) + " more gold to purchace this rank..."), (RequestOptions)null);
							return;
						}
						if (int.Parse(Math.Round(DateTime.Now.Subtract(result.accountLogs[0].timeOfLog).TotalDays).ToString()) < rankNeedingToProgressTo.minimumDays)
						{
							await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You don't have enough time in the Imperials to purchace rank " + rankNeedingToProgressTo.name + "!", "You need " + ((double)rankNeedingToProgressTo.minimumDays - Math.Round(DateTime.Now.Subtract(result.accountLogs[0].timeOfLog).TotalDays, 2)) + " more days before you can purchace this rank..."), (RequestOptions)null);
							return;
						}
						result.ballance -= amountToRemove;
						SocketUser user7 = base.get_Context().get_User();
						await (user7 as IGuildUser).AddRoleAsync(role, (RequestOptions)null);
						result.accountLogs.Add(new accountLog("Bought rank " + rankNeedingToProgressTo.name + " for " + rankNeedingToProgressTo.cost));
						using (StreamWriter accountFile = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id()))
						{
							accountFile.WriteLine(JsonConvert.SerializeObject((object)result));
						}
						AddTrendData(new TrendData
						{
							looseGold = rankNeedingToProgressTo.cost,
							timesRankedUp = 1
						}, DateTime.Now, base.get_Context().get_Guild().get_Name());
						if (rankNeedingToProgressTo.name == "Auxiliary")
						{
							await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You have purchaced " + rankNeedingToProgressTo.name + "!", "Congratulations! Now that you are an Auxiliary, you can apply to join a base!", randomImperialImage: true), (RequestOptions)null);
						}
						else if (!(rankNeedingToProgressTo.name == "Tribune"))
						{
							await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You have purchaced " + rankNeedingToProgressTo.name + "!", "Congratulations! When you climb ranks in the Imperials, you are more trusted in the Imperial community. All hail the Empire!! ", randomImperialImage: true), (RequestOptions)null);
						}
						else
						{
							await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You have purchaced " + rankNeedingToProgressTo.name + "!", "Congratulations! Now that you are a Tribune you can start to play more of a leader role in this branch, such as running a base or becomming a highway manager. Speak to your centurion to find out more information.", randomImperialImage: true), (RequestOptions)null);
						}
						SocketUser user8 = base.get_Context().get_User();
						IGuildUser test = user8 as IGuildUser;
						SocketUser user9 = base.get_Context().get_User();
						foreach (SocketRole r in (user9 as SocketGuildUser).get_Roles())
						{
							if (r.get_Name().ToLower().Contains(highestRole.name.ToLower()))
							{
								await test.RemoveRoleAsync((IRole)(object)r, (RequestOptions)null);
							}
						}
					}
					result = default(account);
				}
				if (counter != ranks.Count)
				{
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Sorry, something went wrong when trying to purchace your rank!", false, (Embed)null, (RequestOptions)null);
			}
		}

		[Command("rankup")]
		public async Task RankUp2()
		{
			if (base.get_Context().get_Guild().get_Name() == "The Imperials")
			{
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Sorry! But that is Illegal!", "The main imperial server does not allow rank ups, you will need to choose a branch to use it on."), (RequestOptions)null);
				return;
			}
			try
			{
				SocketUser user = base.get_Context().get_User();
				IReadOnlyCollection<SocketRole> roles = (user as SocketGuildUser).get_Roles();
				SocketUser user2 = base.get_Context().get_User();
				if (Enumerable.Contains<IRole>((IEnumerable<IRole>)roles, Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user2 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Legate")))))
				{
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Max Rank Achieved!", "Congrats! You have progressed to the highest rank you can purchace!"), (RequestOptions)null);
					return;
				}
				SocketUser user3 = base.get_Context().get_User();
				IReadOnlyCollection<SocketRole> roles2 = (user3 as SocketGuildUser).get_Roles();
				SocketUser user4 = base.get_Context().get_User();
				if (Enumerable.Contains<IRole>((IEnumerable<IRole>)roles2, Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user4 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Tribune")))))
				{
					await TryUpgradeRank(ranks[5].name, ranks[5].cost, ranks[5].minimumDays);
					return;
				}
				SocketUser user5 = base.get_Context().get_User();
				IReadOnlyCollection<SocketRole> roles3 = (user5 as SocketGuildUser).get_Roles();
				SocketUser user6 = base.get_Context().get_User();
				if (Enumerable.Contains<IRole>((IEnumerable<IRole>)roles3, Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user6 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Preafect")))))
				{
					await TryUpgradeRank(ranks[4].name, ranks[4].cost, ranks[4].minimumDays);
					return;
				}
				SocketUser user7 = base.get_Context().get_User();
				IReadOnlyCollection<SocketRole> roles4 = (user7 as SocketGuildUser).get_Roles();
				SocketUser user8 = base.get_Context().get_User();
				if (Enumerable.Contains<IRole>((IEnumerable<IRole>)roles4, Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user8 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Quaestor")))))
				{
					await TryUpgradeRank(ranks[3].name, ranks[3].cost, ranks[3].minimumDays);
					return;
				}
				SocketUser user9 = base.get_Context().get_User();
				IReadOnlyCollection<SocketRole> roles5 = (user9 as SocketGuildUser).get_Roles();
				SocketUser user10 = base.get_Context().get_User();
				if (Enumerable.Contains<IRole>((IEnumerable<IRole>)roles5, Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user10 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Auxiliary")))))
				{
					await TryUpgradeRank(ranks[2].name, ranks[2].cost, ranks[2].minimumDays);
					return;
				}
				SocketUser user11 = base.get_Context().get_User();
				IReadOnlyCollection<SocketRole> roles6 = (user11 as SocketGuildUser).get_Roles();
				SocketUser user12 = base.get_Context().get_User();
				if (!Enumerable.Contains<IRole>((IEnumerable<IRole>)roles6, Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user12 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains("Recruit")))))
				{
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You can't rankup", "You are either not a citzen of the Imperials, or you at a non-purchaceable rank!"), (RequestOptions)null);
				}
				else
				{
					await TryUpgradeRank(ranks[1].name, ranks[1].cost, ranks[1].minimumDays);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Sorry, something went wrong when trying to purchace your rank!", false, (Embed)null, (RequestOptions)null);
			}
		}

		private async Task<bool> TryUpgradeRank(string rank, decimal amount, int mindays)
		{
			SocketUser user = base.get_Context().get_User();
			IRole role = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Contains(rank)));
			account result = default(account);
			using (StreamReader file = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id()))
			{
				result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
				file.Close();
				if (result.ballance < amount)
				{
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You don't have enough gold to purchace rank " + rank + "!", "You need " + (amount - result.ballance) + " more gold to purchace this rank..."), (RequestOptions)null);
					return false;
				}
				if (int.Parse(Math.Round(DateTime.Now.Subtract(result.accountLogs[0].timeOfLog).TotalDays).ToString()) < mindays)
				{
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You don't have enough time in the Imperials to purchace rank " + rank + "!", "You need " + ((double)mindays - Math.Round(DateTime.Now.Subtract(result.accountLogs[0].timeOfLog).TotalDays, 2)) + " more days before you can purchace this rank..."), (RequestOptions)null);
					return false;
				}
				result.ballance -= amount;
				SocketUser user2 = base.get_Context().get_User();
				await (user2 as IGuildUser).AddRoleAsync(role, (RequestOptions)null);
				result.accountLogs.Add(new accountLog("Bought rank " + rank + " for " + amount));
				using (StreamWriter accountFile = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id()))
				{
					accountFile.WriteLine(JsonConvert.SerializeObject((object)result));
				}
				AddTrendData(new TrendData
				{
					looseGold = amount,
					timesRankedUp = 1
				}, DateTime.Now, base.get_Context().get_Guild().get_Name());
				if (rank == "Auxiliary")
				{
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You have purchaced " + rank + "!", "Congratulations! Now that you are an Auxiliary, you can apply to join a base!", randomImperialImage: true), (RequestOptions)null);
				}
				else if (!(rank == "Tribune"))
				{
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You have purchaced " + rank + "!", "Congratulations! When you climb ranks in the Imperials, you are more trusted in the Imperial community. All hail the Empire!! ", randomImperialImage: true), (RequestOptions)null);
				}
				else
				{
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You have purchaced " + rank + "!", "Congratulations! Now that you are a Tribune you can start to play more of a leader role in this branch, such as running a base or becomming a highway manager. Speak to your centurion to find out more information.", randomImperialImage: true), (RequestOptions)null);
				}
				SocketUser user3 = base.get_Context().get_User();
				IGuildUser test = user3 as IGuildUser;
				SocketUser user4 = base.get_Context().get_User();
				foreach (SocketRole r in (user4 as SocketGuildUser).get_Roles())
				{
					foreach (Rank rank2 in ranks)
					{
						if (rank2.name != rank && rank2.name == r.get_Name())
						{
							await test.RemoveRoleAsync((IRole)(object)r, (RequestOptions)null);
						}
					}
				}
			}
			return true;
		}

		[Command("propaganda")]
		public async Task Propaganda()
		{
			EmbedBuilder builder = new EmbedBuilder();
			builder.set_Author(new EmbedAuthorBuilder());
			builder.get_Author().set_Name("Imperials on top!");
			builder.set_Description("The Empire is law and the law is sacred!");
			builder.WithColor(Color.Red);
			string[] randomImpImagesIndex = new string[44]
			{
				"https://cdn.discordapp.com/attachments/692682149383634944/692682964177518663/Properganda_1.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/692682961623187486/HAIL_IMPGANG.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/692682734619066398/9b9tProperganda_1.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/692682577974394950/Girl.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/692682546655526952/Imperial_MAN.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/692682539009048576/Find_Ur_IQ.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/692682326865608785/funnyimpo.jpg",
				"https://cdn.discordapp.com/attachments/692682149383634944/692682283651694653/coolguy.png",
				"https://cdn.discordapp.com/attachments/625444487828733962/688499087019868317/image0.png",
				"https://cdn.discordapp.com/attachments/625444487828733962/687612106861707325/Givetland_Mar12_21-44.png",
				"https://cdn.discordapp.com/attachments/625444487828733962/686762588318466066/imp_meme.png",
				"https://www.youtube.com/watch?v=TL17pUz9xD0&feature=youtu.be",
				"https://cdn.discordapp.com/attachments/625444487828733962/681774906668875801/gif.gif",
				"https://cdn.discordapp.com/attachments/625444487828733962/673198052290330674/2020-02-01_17.07.38.png",
				"https://cdn.discordapp.com/attachments/625444487828733962/667134951019511828/eeeeecserfgsarg.png",
				"https://youtu.be/n3ia2pERH1U",
				"https://cdn.discordapp.com/attachments/625444487828733962/660073269613035521/Adobe_Post_20191227_1151060.06861931758175954.png",
				"https://cdn.discordapp.com/attachments/625444487828733962/659908622276493337/PSX_20191227_005353.jpg",
				"https://cdn.discordapp.com/attachments/625444487828733962/658306817939603466/PSX_20191222_141907.jpg",
				"https://cdn.discordapp.com/attachments/625444487828733962/658305964071583754/PSX_20191222_143601.jpg",
				"https://cdn.discordapp.com/attachments/625444487828733962/655746640287629323/PSX_20191215_131951.jpg",
				"https://cdn.discordapp.com/attachments/625444487828733962/655520276179714074/image0.jpg",
				"https://cdn.discordapp.com/attachments/625444487828733962/642346547811450890/PSX_20191108_135420.jpg",
				"https://cdn.discordapp.com/attachments/625444487828733962/642083613852172298/PSX_20191107_201946.jpg",
				"https://cdn.discordapp.com/attachments/625444487828733962/642081454242660383/PSX_20191107_192555.jpg",
				"https://cdn.discordapp.com/attachments/641383684200595456/641565362193825802/gif-1.mp4",
				"https://cdn.discordapp.com/attachments/663477518812184586/691628478746984458/unknown.png",
				"https://cdn.discordapp.com/attachments/663477518812184586/691593817345753138/image0.png",
				"https://cdn.discordapp.com/attachments/663477518812184586/689124162178252891/image0.png",
				"https://i.imgur.com/2oIGZQth.jpg",
				"https://cdn.discordapp.com/attachments/692682149383634944/698376885524234311/cooltext354327639099084.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/698376926674419744/ImperialsOnTop.gif",
				"https://cdn.discordapp.com/attachments/692682149383634944/698377197345570876/cooltext354327688554660.png",
				"https://cdn.discordapp.com/attachments/692682149383634944/698376885524234311/cooltext354327639099084.png",
				"https://cdn.discordapp.com/attachments/697779536422633512/697789702903955568/Fuckyousalc1.jpg",
				"https://cdn.discordapp.com/attachments/697779536422633512/697786865482465284/Untitled-1.jpg",
				"https://cdn.discordapp.com/attachments/697779536422633512/697784110499889242/unknown.png",
				"https://cdn.discordapp.com/attachments/697779536422633512/697783855603515402/unknown.png",
				"https://cdn.discordapp.com/attachments/697779536422633512/697783460026122290/unknown.png",
				"https://cdn.discordapp.com/attachments/697779536422633512/697783826641977395/unknown.png",
				"https://cdn.discordapp.com/attachments/697779536422633512/697784067394895872/unknown.png",
				"https://cdn.discordapp.com/attachments/625444487828733962/692175638068723712/keemstar2.png",
				"https://cdn.discordapp.com/attachments/625444487828733962/697631128105910292/anti_imp_v_imp.png",
				"https://cdn.discordapp.com/attachments/680885563988901926/695207929267224608/IMG_20200402_114756.jpg"
			};
			builder.WithImageUrl(randomImpImagesIndex[random.Next(0, randomImpImagesIndex.Length)]);
			await base.get_Context().get_Channel().SendMessageAsync("", false, builder.Build(), (RequestOptions)null);
			AddTrendData(new TrendData
			{
				patriatismCount = 1
			}, DateTime.Now, base.get_Context().get_Guild().get_Name());
		}

		[Command("adminlogs")]
		public async Task Logs(string userID)
		{
			SocketUser user = base.get_Context().get_User();
			IRole role = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name() == "Imperial Gold Manager"));
			SocketUser user2 = base.get_Context().get_User();
			if (!Enumerable.Contains<IRole>((IEnumerable<IRole>)(user2 as SocketGuildUser).get_Roles(), role))
			{
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Plebs do not have permission to preform this command!", false, (Embed)null, (RequestOptions)null);
				return;
			}
			try
			{
				base.get_Context().get_User();
				account result = JsonConvert.DeserializeObject<account>(File.ReadAllText("accounts/" + userID));
				File.WriteAllText("data\\logs.txt", "Imperial 2021 - Logs of " + userID + Environment.NewLine);
				using (StreamWriter w = File.AppendText("data\\logs.txt"))
				{
					foreach (accountLog accountLog in result.accountLogs)
					{
						DateTime timeOfLog = accountLog.timeOfLog;
						w.WriteLine("[" + timeOfLog.ToString() + "] " + accountLog.detailOfLog);
					}
					w.Close();
				}
				await base.get_Context().get_Channel().SendFileAsync("data\\logs.txt", "Here is all of the " + userID + " logged data in a text format.", false, (Embed)null, (RequestOptions)null, false);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + Environment.NewLine + " () -- " + e.ToString());
				await base.get_Context().get_Channel().SendMessageAsync("Sorry, something has gone wrong when trying to ascess your logs. If you desperatly need your logs please contact an admin", false, (Embed)null, (RequestOptions)null);
			}
		}

		[Command("trends")]
		public async Task Trends()
		{
			try
			{
				if (trendData.Count == 0)
				{
					trendData = LoadTrendData();
				}
				string together = "";
				foreach (TrendData trend in trendData)
				{
					if (trend.date == DateTime.Today.Date && trend.server == base.get_Context().get_Guild().get_Name())
					{
						together = $"GDP today: {trend.gainGold} {Environment.NewLine}" + $"GDL today: {trend.looseGold} {Environment.NewLine}" + $"Gold Traded Today: {trend.transactionGold} {Environment.NewLine}" + "-= Other Statistics =-" + Environment.NewLine + $"Ballance Checks Today: {trend.timesCheckedBal} {Environment.NewLine}" + $"Times Ranked Up Today: {trend.timesRankedUp} {Environment.NewLine}" + $"Number of accounts made public today: {trend.timesMadePublic} {Environment.NewLine}" + $"Number of accounts made private today: {trend.timesMadePrivate} {Environment.NewLine}" + $"Imperial Patriotism Count: {trend.patriatismCount} {Environment.NewLine}";
					}
				}
				EmbedBuilder builder = new EmbedBuilder();
				builder.set_Author(new EmbedAuthorBuilder());
				builder.get_Author().set_Name(DateTime.Today.DayOfWeek.ToString() + "'s gold trends for " + base.get_Context().get_Guild().get_Name());
				builder.set_Description(together);
				builder.WithColor(Color.Gold);
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("", false, builder.Build(), (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("GDP calculation error, make sure you run this command in a server that allows GDP", false, (Embed)null, (RequestOptions)null);
			}
		}

		[Command("gdp")]
		public async Task GDP()
		{
			try
			{
				if (trendData.Count == 0)
				{
					trendData = LoadTrendData();
				}
				decimal GDP_YESTERDAY = default(decimal);
				decimal GDP_TODAY = default(decimal);
				foreach (TrendData trend in trendData)
				{
					if (trend.date == DateTime.Today.Date && trend.server == base.get_Context().get_Guild().get_Name())
					{
						GDP_TODAY = trend.gainGold - trend.looseGold;
					}
					if (trend.date == DateTime.Today.AddDays(-1.0).Date && trend.server == base.get_Context().get_Guild().get_Name())
					{
						GDP_YESTERDAY = trend.gainGold - trend.looseGold;
					}
				}
				decimal percentageGroth = (GDP_TODAY - GDP_YESTERDAY) / 2m * 100m;
				EmbedBuilder builder = new EmbedBuilder();
				builder.set_Author(new EmbedAuthorBuilder());
				if (percentageGroth < 0m)
				{
					builder.get_Author().set_Name($"Decreased by {Math.Round(percentageGroth, 2)}%");
					builder.set_Description("The imperial gold market isn't looking good today");
					builder.WithColor(Color.DarkerGrey);
				}
				else
				{
					builder.get_Author().set_Name($"Increased by {Math.Round(percentageGroth, 2)}%");
					builder.set_Description("Nice! The market is going up!");
					builder.WithColor(Color.Gold);
				}
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("", false, builder.Build(), (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("GDP calculation error, make sure you run this command in a server that allows GDP", false, (Embed)null, (RequestOptions)null);
			}
		}

		public void AddTrendData(TrendData dataToAdd, DateTime date, string server)
		{
			try
			{
				if (this.trendData.Count == 0)
				{
					this.trendData = LoadTrendData();
				}
				if (this.trendData == null)
				{
					this.trendData = new List<TrendData>();
				}
				int num = 0;
				try
				{
					foreach (TrendData trendDatum in this.trendData)
					{
						if (trendDatum.server == base.get_Context().get_Guild().get_Name() && trendDatum.date.Date == DateTime.Today)
						{
							TrendData trendData = new TrendData();
							trendData.gainGold = dataToAdd.gainGold + this.trendData[num].gainGold;
							trendData.looseGold = dataToAdd.looseGold + this.trendData[num].looseGold;
							trendData.transactionGold = dataToAdd.transactionGold + this.trendData[num].transactionGold;
							trendData.timesCheckedBal = dataToAdd.timesCheckedBal + this.trendData[num].timesCheckedBal;
							trendData.patriatismCount = dataToAdd.patriatismCount + this.trendData[num].patriatismCount;
							trendData.timesRankedUp = dataToAdd.timesRankedUp + this.trendData[num].timesRankedUp;
							trendData.timesMadePrivate = dataToAdd.timesMadePrivate + this.trendData[num].timesMadePrivate;
							trendData.timesMadePublic = dataToAdd.timesMadePublic + this.trendData[num].timesMadePublic;
							trendData.server = base.get_Context().get_Guild().get_Name();
							trendData.date = DateTime.Today;
							this.trendData[num] = trendData;
							SaveTrendData();
							return;
						}
						num++;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("ERROR IN FOREACH LOOP IN ADD TREND: " + Environment.NewLine + ex.Message);
				}
				TrendData trendData2 = new TrendData();
				trendData2.gainGold = dataToAdd.gainGold;
				trendData2.looseGold = dataToAdd.looseGold;
				trendData2.transactionGold = dataToAdd.transactionGold;
				trendData2.timesCheckedBal = dataToAdd.timesCheckedBal;
				trendData2.patriatismCount = dataToAdd.patriatismCount;
				trendData2.timesRankedUp = dataToAdd.timesRankedUp;
				trendData2.server = base.get_Context().get_Guild().get_Name();
				trendData2.date = DateTime.Today;
				this.trendData.Add(trendData2);
				SaveTrendData();
			}
			catch (Exception ex2)
			{
				Console.WriteLine(ex2.ToString());
			}
		}

		public void SaveTrendData()
		{
			using (StreamWriter streamWriter = new StreamWriter("trends/trends.txt"))
			{
				streamWriter.WriteLine(JsonConvert.SerializeObject((object)trendData));
			}
			List<TrendData> list = new List<TrendData>();
			using StreamReader streamReader = new StreamReader("trends/trends.txt");
			list = JsonConvert.DeserializeObject<List<TrendData>>(streamReader.ReadToEnd());
			streamReader.Close();
		}

		public List<TrendData> LoadTrendData()
		{
			List<TrendData> result = new List<TrendData>();
			using (StreamReader streamReader = new StreamReader("trends/trends.txt"))
			{
				result = JsonConvert.DeserializeObject<List<TrendData>>(streamReader.ReadToEnd());
				streamReader.Close();
			}
			return result;
		}

		[Command("trends")]
		public async Task Trends(string date)
		{
			try
			{
				if (!DateTime.TryParse(date, out var dateTimeTo))
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("", false, SendEmmbedMessage("Not a real date...", "Please enter a valid date."), (RequestOptions)null);
					return;
				}
				if (dateTimeTo.Date > DateTime.Today)
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("", false, SendEmmbedMessage("This bot is not a fortune teller!", "Sorry but Imperial gold bot can't tell you the future! Wait... Maybe we can? Hmmmm, next update maybe?"), (RequestOptions)null);
					return;
				}
				if (trendData.Count == 0)
				{
					trendData = LoadTrendData();
				}
				string together = "";
				foreach (TrendData trend in trendData)
				{
					if (trend.date == dateTimeTo && trend.server == base.get_Context().get_Guild().get_Name())
					{
						together = $"GDP today: {trend.gainGold} {Environment.NewLine}" + $"GDL today: {trend.looseGold} {Environment.NewLine}" + $"Gold Traded Today: {trend.transactionGold} {Environment.NewLine}" + "-= Other Statistics =-" + Environment.NewLine + $"Ballance Checks Today: {trend.timesCheckedBal} {Environment.NewLine}" + $"Times Ranked Up Today: {trend.timesRankedUp} {Environment.NewLine}" + $"Number of accounts made public today: {trend.timesMadePublic} {Environment.NewLine}" + $"Number of accounts made private today: {trend.timesMadePrivate} {Environment.NewLine}" + $"Imperial Patriotism Count: {trend.patriatismCount} {Environment.NewLine}";
					}
				}
				EmbedBuilder builder = new EmbedBuilder();
				builder.set_Author(new EmbedAuthorBuilder());
				builder.get_Author().set_Name(dateTimeTo.Date.DayOfWeek.ToString() + "'s gold trends for " + base.get_Context().get_Guild().get_Name());
				builder.set_Description(together);
				builder.WithColor(Color.Gold);
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("", false, builder.Build(), (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("GDP calculation error, make sure you run this command in a server that allows GDP", false, (Embed)null, (RequestOptions)null);
			}
		}

		[Command("compare")]
		public async Task Compare(string date1, string date2)
		{
			try
			{
				if (!DateTime.TryParse(date1, out var dateTimeTo1))
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("", false, SendEmmbedMessage("Not a real date...", "Please enter a valid date."), (RequestOptions)null);
					return;
				}
				if (!DateTime.TryParse(date1, out var dateTimeTo2))
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("", false, SendEmmbedMessage("Not a real date...", "Please enter a valid date."), (RequestOptions)null);
					return;
				}
				if (dateTimeTo1.Date > DateTime.Today || dateTimeTo2.Date > DateTime.Today)
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("", false, SendEmmbedMessage("This bot is not a fortune teller!", "Sorry but Imperial gold bot can't tell you the future! Wait... Maybe we can? Hmmmm, next update maybe?"), (RequestOptions)null);
					return;
				}
				if (trendData.Count == 0)
				{
					trendData = LoadTrendData();
				}
				decimal one = default(decimal);
				decimal two = default(decimal);
				foreach (TrendData trend in trendData)
				{
					if (trend.date == dateTimeTo1 && trend.server == base.get_Context().get_Guild().get_Name())
					{
						one = trend.gainGold;
					}
					else if (trend.date == dateTimeTo2 && trend.server == base.get_Context().get_Guild().get_Name())
					{
						two = trend.gainGold;
					}
				}
				decimal percentageGroth = (one - two) / 2m * 100m;
				EmbedBuilder builder = new EmbedBuilder();
				builder.set_Author(new EmbedAuthorBuilder());
				if (percentageGroth < 0m)
				{
					builder.get_Author().set_Name($"Decreased by {Math.Round(percentageGroth, 2)}%");
					builder.set_Description("The imperial gold market isn't looking good today");
					builder.WithColor(Color.DarkerGrey);
				}
				else
				{
					builder.get_Author().set_Name($"Increased by {Math.Round(percentageGroth, 2)}%");
					builder.set_Description("Nice! The market is going up!");
					builder.WithColor(Color.Gold);
				}
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("", false, builder.Build(), (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("GDP calculation error, make sure you run this command in a server that allows GDP", false, (Embed)null, (RequestOptions)null);
			}
		}

		[Command(/*Could not decode attribute arguments.*/)]
		public async Task Advertisemet([Remainder] string advertisement)
		{
			try
			{
				int cost = 100;
				account result = default(account);
				using (StreamReader file = new StreamReader("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id()))
				{
					result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
					file.Close();
					if (result.ballance < 1m)
					{
						await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("You don't have enough gold to purchace this advertisement!", "You need " + ((decimal)cost - result.ballance) + " more gold to purchace this..."), (RequestOptions)null);
						return;
					}
					result.ballance -= (decimal)cost;
					bool foundIt = false;
					foreach (SocketTextChannel socketChannel in base.get_Context().get_Guild().get_TextChannels())
					{
						if (((SocketGuildChannel)socketChannel).get_Name() == "imperial-gold-advertisements")
						{
							await socketChannel.SendMessageAsync(advertisement, false, (Embed)null, (RequestOptions)null);
							foundIt = true;
						}
					}
					if (!foundIt)
					{
						await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Sorry but we could not find a channel to put your advertisement in... Tell an administator that there is a problem...", false, (Embed)null, (RequestOptions)null);
						return;
					}
					result.accountLogs.Add(new accountLog("Bought advertisement for " + cost + " gold"));
					using (StreamWriter accountFile = new StreamWriter("accounts/" + ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id()))
					{
						accountFile.WriteLine(JsonConvert.SerializeObject((object)result));
					}
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Advertisement bought!"), (RequestOptions)null);
				}
				result = default(account);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Advertisement Error. Check with Centurion or Optio", false, (Embed)null, (RequestOptions)null);
			}
		}

		[Command("branchtop today")]
		public async Task BalTopBranches()
		{
			List<TrendData> trendDatas = new List<TrendData>();
			if (trendData.Count == 0)
			{
				trendData = LoadTrendData();
			}
			Console.WriteLine(trendData.Count);
			foreach (TrendData trend in trendData)
			{
				if (trend.date.Date == DateTime.Today.Date)
				{
					trendDatas.Add(trend);
					Console.WriteLine(trend.server);
				}
				Console.WriteLine($"{trend.server} | {trend.date} = {DateTime.Today.Date}");
			}
			try
			{
				string ToSay = "";
				Console.WriteLine("YES");
				trendDatas = Enumerable.ToList<TrendData>((IEnumerable<TrendData>)Enumerable.OrderByDescending<TrendData, decimal>((IEnumerable<TrendData>)trendDatas, (Func<TrendData, decimal>)((TrendData xz) => xz.gainGold - xz.looseGold)));
				int i = 0;
				foreach (TrendData tt in trendDatas)
				{
					try
					{
						if (i >= 10)
						{
							break;
						}
						i++;
						ToSay = ToSay + "[" + i + "] " + tt.server + " | " + $"{tt.gainGold - tt.looseGold:0.00}" + " gold" + Environment.NewLine;
						continue;
					}
					catch (Exception)
					{
						ToSay = ToSay + "[" + i + "] " + tt.server + " | " + (tt.gainGold - tt.looseGold) + " gold" + Environment.NewLine;
						continue;
					}
				}
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Imperial Empire Branch List", ToSay, randomImperialImage: true), (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine("2nd");
				Console.WriteLine(e.ToString());
				Console.WriteLine(Environment.NewLine + e.Message);
			}
		}

		[Command("branchtop")]
		public async Task BranchTop()
		{
			await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Wrong Options!", "!branchtop today " + Environment.NewLine + " !branchtop all time", randomImperialImage: true), (RequestOptions)null);
		}

		[Command("branchtop all time")]
		public async Task BalTopBranchesAllTime()
		{
			List<TrendData> trendDatas = new List<TrendData>();
			if (trendData.Count == 0)
			{
				trendData = LoadTrendData();
			}
			Console.WriteLine(trendData.Count);
			foreach (TrendData trend in trendData)
			{
				bool couldnt_find = true;
				int counter = 0;
				foreach (TrendData trendish in trendDatas)
				{
					if (trendish.server == trend.server)
					{
						trendDatas[counter].gainGold += trend.gainGold;
						trendDatas[counter].looseGold += trend.looseGold;
						couldnt_find = false;
					}
					counter++;
				}
				if (couldnt_find)
				{
					trendDatas.Add(trend);
				}
				Console.WriteLine(trend.server);
				Console.WriteLine($"{trend.server} | {trend.date} = {DateTime.Today.Date}");
			}
			try
			{
				string ToSay = "";
				Console.WriteLine("YES");
				trendDatas = Enumerable.ToList<TrendData>((IEnumerable<TrendData>)Enumerable.OrderByDescending<TrendData, decimal>((IEnumerable<TrendData>)trendDatas, (Func<TrendData, decimal>)((TrendData xz) => xz.gainGold)));
				int i = 0;
				foreach (TrendData tt in trendDatas)
				{
					try
					{
						if (i >= 10)
						{
							break;
						}
						i++;
						ToSay = ToSay + "[" + i + "] " + tt.server + " | " + $"{tt.gainGold:0.00}" + " gold" + Environment.NewLine;
						continue;
					}
					catch (Exception)
					{
						ToSay = ToSay + "[" + i + "] " + tt.server + " | " + tt.gainGold + " gold" + Environment.NewLine;
						continue;
					}
				}
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Imperial Empire Branch List", ToSay, randomImperialImage: true), (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine("2nd");
				Console.WriteLine(e.ToString());
				Console.WriteLine(Environment.NewLine + e.Message);
			}
		}

		[Command("citizens")]
		public async Task Citizens()
		{
			List<ulong> usersCounted = new List<ulong>();
			List<string> rolesAcceptable = new List<string>
			{
				"Recruit",
				"Auxiliary",
				"Quaestor",
				"Preafect",
				"Tribune",
				"Legate",
				"Optio",
				"Centurion",
				"Advisor",
				"Imperial Elite",
				"Preator",
				"High General",
				"Emperor"
			};
			foreach (SocketGuild socketGuild in ((BaseSocketClient)base.get_Context().get_Client()).get_Guilds())
			{
				foreach (SocketGuildUser user in socketGuild.get_Users())
				{
					foreach (string role in rolesAcceptable)
					{
						IReadOnlyCollection<SocketRole> roles = user.get_Roles();
						SocketUser user2 = base.get_Context().get_User();
						if (!Enumerable.Contains<IRole>((IEnumerable<IRole>)roles, Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user2 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().ToLower().Contains(role.ToLower())))))
						{
							continue;
						}
						Console.WriteLine(socketGuild.get_Name() + " | " + ((SocketUser)user).get_Username());
						bool found = false;
						foreach (ulong id in usersCounted)
						{
							if (id == ((SocketEntity<ulong>)(object)user).get_Id())
							{
								found = true;
								break;
							}
						}
						if (!found)
						{
							usersCounted.Add(((SocketEntity<ulong>)(object)user).get_Id());
						}
					}
				}
			}
			await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("", false, SendEmmbedMessage("Citizen Count", usersCounted.Count.ToString()), (RequestOptions)null);
		}

		[Command("market top")]
		public async Task MarketTop()
		{
			List<TrendData> trendDatas = new List<TrendData>();
			if (trendData.Count == 0)
			{
				trendData = LoadTrendData();
			}
			Console.WriteLine(trendData.Count);
			foreach (TrendData trend in trendData)
			{
				bool couldnt_find = true;
				int counter = 0;
				foreach (TrendData trendish in trendDatas)
				{
					if (trendish.server == trend.server)
					{
						trendDatas[counter].transactionGold += trend.transactionGold;
						couldnt_find = false;
					}
					counter++;
				}
				if (couldnt_find)
				{
					trendDatas.Add(trend);
				}
				Console.WriteLine(trend.server);
				Console.WriteLine($"{trend.server} | {trend.date} = {DateTime.Today.Date}");
			}
			try
			{
				string ToSay = "";
				trendDatas = Enumerable.ToList<TrendData>((IEnumerable<TrendData>)Enumerable.OrderByDescending<TrendData, decimal>((IEnumerable<TrendData>)trendDatas, (Func<TrendData, decimal>)((TrendData xz) => xz.transactionGold)));
				int i = 0;
				foreach (TrendData tt in trendDatas)
				{
					try
					{
						if (i >= 10)
						{
							break;
						}
						i++;
						ToSay = ToSay + "[" + i + "] " + tt.server + " | " + $"{tt.transactionGold:0.00}" + " gold" + Environment.NewLine;
						continue;
					}
					catch (Exception)
					{
						ToSay = ToSay + "[" + i + "] " + tt.server + " | " + tt.transactionGold + " gold" + Environment.NewLine;
						continue;
					}
				}
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Imperial Empire Branch Trade Market (ALL TIME)", ToSay, randomImperialImage: true), (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine("2nd");
				Console.WriteLine(e.ToString());
				Console.WriteLine(Environment.NewLine + e.Message);
			}
		}

		[Command("print bank growth")]
		public async Task PrintGrowth()
		{
			string data = "";
			TrendData lastDay = new TrendData
			{
				server = ""
			};
			if (trendData.Count == 0)
			{
				trendData = LoadTrendData();
			}
			foreach (TrendData trend in trendData)
			{
				if (!(trend.server == base.get_Context().get_Guild().get_Name()))
				{
					continue;
				}
				if (lastDay.server == base.get_Context().get_Guild().get_Name())
				{
					if (trend.gainGold <= 0m)
					{
						trend.gainGold = 0.01m;
					}
					else if (lastDay.gainGold <= 0m)
					{
						lastDay.gainGold = 0.01m;
					}
					data += $"{trend.date} | {DoubleToPercentageString(CalculateChange(lastDay.gainGold, trend.gainGold))}              | Prev: {lastDay.gainGold} gold | Now: {trend.gainGold} {Environment.NewLine}";
				}
				lastDay = trend;
			}
			File.WriteAllText("data/bankGrowth.txt", data);
			await base.get_Context().get_Channel().SendFileAsync("data/bankGrowth.txt", "", false, SendEmmbedMessage("Gain Gold / Bank Percentage Data"), (RequestOptions)null, false);
		}

		private decimal CalculateChange(decimal previous, decimal current)
		{
			if (previous == 0m)
			{
				throw new InvalidOperationException();
			}
			decimal d = current - previous;
			return d / previous;
		}

		private string DoubleToPercentageString(decimal d)
		{
			return Math.Round(d, 2) * 100m + "%";
		}

		[Command(/*Could not decode attribute arguments.*/)]
		public async Task BusinessCreate()
		{
			await SendMessage("Type 'back' to go to the last question, type 'cancel' to cancel.");
			Business business = new Business();
			SocketMessage response100;
			do
			{
				await SendMessage("Business Name?");
				SocketMessage response99 = await base.NextMessageAsync(true, true, (TimeSpan?)TimeSpan.FromSeconds(3600.0));
				if (response99.get_Content().ToLower().Trim() == "back" || response99.get_Content().ToLower().Trim() == "cancel")
				{
					return;
				}
				business.name = response99.get_Content();
				await SendMessage("Business Description?");
				response100 = await base.NextMessageAsync(true, true, (TimeSpan?)TimeSpan.FromSeconds(3600.0));
			}
			while (response100.get_Content().ToLower().Trim() == "back");
			if (response100.get_Content().ToLower().Trim() == "cancel")
			{
				return;
			}
			business.description = response100.get_Content();
			while (true)
			{
				await SendMessage("Are you sure you want to create a buiness and pay the 20 gold starting fee? (yes/no)");
				SocketMessage response101 = await base.NextMessageAsync(true, true, (TimeSpan?)TimeSpan.FromSeconds(3600.0));
				if (response101.get_Content().ToLower().Trim() == "yes")
				{
					BusinessMain.CreateBuiness(business.name, business.description, ((SocketEntity<ulong>)(object)base.get_Context().get_User()).get_Id());
					await SendMessage("Created your business!");
					return;
				}
				if (response101.get_Content().ToLower().Trim() == "no")
				{
					break;
				}
				await SendMessage("'yes' or 'no' please.");
			}
			await SendMessage("Canceled!");
		}

		public async Task SendMessage(string message)
		{
			await UserExtensions.SendMessageAsync((IUser)(object)base.get_Context().get_User(), "", false, SendEmmbedMessage(message), (RequestOptions)null);
		}

		[Command("it")]
		public async Task ImpTime(SocketUser user = null)
		{
			if (user == null)
			{
				user = base.get_Context().get_User();
			}
			try
			{
				base.get_Context().get_User();
				DateTime startDate = JsonConvert.DeserializeObject<account>(File.ReadAllText("accounts/" + ((SocketEntity<ulong>)(object)user).get_Id())).accountLogs[0].timeOfLog.Date;
				DateTime endDate = DateTime.Today.Date;
				double totalDays = (endDate - startDate).TotalDays;
				double totalYears = Math.Truncate(totalDays / 365.0);
				double totalMonths = Math.Truncate(totalDays % 365.0 / 30.0);
				double remainingDays = Math.Truncate(totalDays % 365.0 % 30.0);
				await base.get_Context().get_Channel().SendMessageAsync($"Estimated duration is {totalYears} year(s), {totalMonths} month(s) and {remainingDays} day(s)", false, (Embed)null, (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + Environment.NewLine + " () -- " + e.ToString());
				await base.get_Context().get_Channel().SendMessageAsync("Sorry, something has gone wrong when trying to ascess your time. If you desperatly need your logs please contact an admin", false, (Embed)null, (RequestOptions)null);
			}
		}

		[Command("accounts")]
		public async Task GoldShip()
		{
			try
			{
				new List<account>();
				string[] accountFiles = Directory.GetFiles("accounts/");
				await base.get_Context().get_Channel().SendMessageAsync("Current Imperial Gold accounts = " + accountFiles.Length, false, (Embed)null, (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + Environment.NewLine + " () -- " + e.ToString());
				await base.get_Context().get_Channel().SendMessageAsync("Sorry, something has gone wrong.", false, (Embed)null, (RequestOptions)null);
			}
		}

		[Command("membership")]
		public async Task membership()
		{
			try
			{
				List<SocketGuildUser> members = new List<SocketGuildUser>();
				List<SocketGuildUser> members_branch = new List<SocketGuildUser>();
				foreach (SocketGuild socketGuild in ((BaseSocketClient)base.get_Context().get_Client()).get_Guilds())
				{
					Console.WriteLine(socketGuild.get_Name());
					Thread.Sleep(1000);
					foreach (SocketGuildUser socketUser in socketGuild.get_Users())
					{
						if (((SocketUser)socketUser).get_IsBot() || members.Contains(socketUser))
						{
							continue;
						}
						string[] citizenRoles = new string[13]
						{
							"Recruit",
							"Auxiliary",
							"Quaestor",
							"Preafect",
							"Tribune",
							"Legate",
							"Optio",
							"Centurion",
							"Advisor",
							"Imperial Elite",
							"Preator",
							"High General",
							"Emperor"
						};
						string[] array = citizenRoles;
						foreach (string role_ in array)
						{
							IRole role = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)((IGuildUser)socketUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name().Trim().ToLower()
								.Contains(role_.ToLower())));
							if (Enumerable.Contains<IRole>((IEnumerable<IRole>)socketUser.get_Roles(), role))
							{
								members_branch.Add(socketUser);
								members.Add(socketUser);
								Console.WriteLine(((SocketUser)socketUser).get_Username() + " | " + role_ + " | " + socketGuild.get_Name());
								break;
							}
						}
					}
					await base.get_Context().get_Channel().SendMessageAsync("Current Imperial Citizens in " + socketGuild.get_Name() + " = " + members_branch.Count, false, (Embed)null, (RequestOptions)null);
					members_branch.Clear();
				}
				await base.get_Context().get_Channel().SendMessageAsync("Current Imperial Citizens = " + members.Count, false, (Embed)null, (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + Environment.NewLine + " () -- " + e.ToString());
				await base.get_Context().get_Channel().SendMessageAsync("Sorry, something has gone wrong.", false, (Embed)null, (RequestOptions)null);
			}
		}

		[Command("setvar")]
		public async Task SetVar(string name, string data)
		{
			PaymentVars.UpdateVariable(name, data);
			await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Set variable " + name + " == " + data, false, (Embed)null, (RequestOptions)null);
		}

		[Command("listvars")]
		public async Task ListVars()
		{
			string together2 = "```";
			foreach (KeyValuePair<string, string> i in PaymentVars.variables)
			{
				together2 = together2 + i.Key + " | " + i.Value + Environment.NewLine;
			}
			together2 += "```";
			await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync(together2, false, (Embed)null, (RequestOptions)null);
		}

		[Command("setpayment")]
		public async Task SetPayment(string name, decimal payRate)
		{
			JobVars.UpdateVariable(name, payRate);
			await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync($"Set variable {name} == {payRate}", false, (Embed)null, (RequestOptions)null);
		}

		[Command("listpayments")]
		public async Task ListPayments()
		{
			string together2 = "```";
			foreach (Job.JobType i in Job.jobs)
			{
				string[] obj = new string[5]
				{
					together2,
					i.name,
					" | ",
					null,
					null
				};
				decimal payRatePerWeek = i.payRatePerWeek;
				obj[3] = payRatePerWeek.ToString();
				obj[4] = Environment.NewLine;
				together2 = string.Concat(obj);
			}
			together2 += "```";
			await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync(together2, false, (Embed)null, (RequestOptions)null);
		}

		[Command("real baltop")]
		public async Task RealBalTop()
		{
			List<account> accounts = new List<account>();
			string[] accountFiles = Directory.GetFiles("accounts/");
			try
			{
				string[] array = accountFiles;
				foreach (string accountFile in array)
				{
					using StreamReader file = new StreamReader(accountFile);
					account result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
					file.Close();
					accounts.Add(result);
				}
			}
			catch (Exception ex)
			{
				Exception e2 = ex;
				Console.WriteLine("1st");
				Console.WriteLine(e2.ToString());
			}
			try
			{
				string ToSay = "";
				Console.WriteLine("YES");
				accounts = _2b2tpay.Data.BalTop.RealSortAccounts(accounts);
				int i = 0;
				foreach (account account in accounts)
				{
					try
					{
						if (i >= 10)
						{
							break;
						}
						i++;
						ToSay = ToSay + "[" + i + "] " + ((SocketUser)base.get_Context().get_Guild().GetUser(account.accountUserId)).get_Username() + " | " + $"{account.ballance:0.00}" + " gold" + Environment.NewLine;
						continue;
					}
					catch (Exception)
					{
						string[] obj = new string[9]
						{
							ToSay,
							"[",
							i.ToString(),
							"] ",
							account.name,
							" | ",
							null,
							null,
							null
						};
						decimal ballance = account.ballance;
						obj[6] = ballance.ToString();
						obj[7] = " gold";
						obj[8] = Environment.NewLine;
						ToSay = string.Concat(obj);
						continue;
					}
				}
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("REAL (UN-HIDE) Imperial Forbes List", ToSay, randomImperialImage: true), (RequestOptions)null);
			}
			catch (Exception e)
			{
				Console.WriteLine("2nd");
				Console.WriteLine(e.ToString());
				Console.WriteLine(Environment.NewLine + e.Message);
			}
		}

		[Command("removeid_bal")]
		public async Task removeid_bal(string user, string gold = "X")
		{
			if (((SocketEntity<ulong>)(object)base.get_Context().get_Guild()).get_Id() != SENATE_GUILD)
			{
				return;
			}
			try
			{
				SocketUser user2 = base.get_Context().get_User();
				IRole role = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user2 as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name() == "Imperial Gold Manager"));
				SocketUser user3 = base.get_Context().get_User();
				if (!Enumerable.Contains<IRole>((IEnumerable<IRole>)(user3 as SocketGuildUser).get_Roles(), role))
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Plebs do not have permission to preform this command!", false, (Embed)null, (RequestOptions)null);
					return;
				}
				try
				{
					if (gold.Contains("$"))
					{
						gold = gold.TrimStart(new char[1]
						{
							'$'
						});
						gold = gold.TrimEnd(new char[1]
						{
							'$'
						});
					}
					if (user == null || gold == "X")
					{
						if (user == null)
						{
							await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("A user ID must be mentioned.", false, (Embed)null, (RequestOptions)null);
						}
						else
						{
							await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Must be 0.01 gold or above and in numeric form", false, (Embed)null, (RequestOptions)null);
						}
						return;
					}
					decimal ammountToRemove = default(decimal);
					if (!decimal.TryParse(gold, out ammountToRemove))
					{
						await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Value of gold must be a number.", false, (Embed)null, (RequestOptions)null);
						return;
					}
					if (user == null)
					{
						await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("You need to mention someone when removing gold!! | !adminremove @user x", false, (Embed)null, (RequestOptions)null);
						return;
					}
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Removing " + user + gold + " gold...", false, (Embed)null, (RequestOptions)null);
					if (!File.Exists("accounts/" + user.ToString()))
					{
						Random random = new Random();
						account account = default(account);
						account.name = user;
						account.accountUserId = ulong.Parse(user);
						account.accountId = random.Next(0, 99999999);
						account.ballance = default(decimal);
						account.accountLogs = new List<accountLog>();
						account.accountLogs.Add(new accountLog("Created account"));
						account.accountLogs.Add(new accountLog("Credit: 0 gold"));
						account.transactions = new List<accountLog>();
						account.transactions.Add(new accountLog("Credit: 0 gold"));
						account.isPublic = true;
						using (StreamWriter streamWriter = new StreamWriter("accounts/" + account.accountUserId))
						{
							streamWriter.WriteLine(JsonConvert.SerializeObject((object)account));
						}
						account = default(account);
					}
					account result2 = default(account);
					using (StreamReader file = new StreamReader("accounts/" + user.ToString()))
					{
						result2 = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
						file.Close();
						result2.ballance -= ammountToRemove;
						if (result2.ballance < 0m)
						{
							result2.ballance = default(decimal);
						}
						result2.accountLogs.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
						result2.transactions.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
						using StreamWriter accountFile = new StreamWriter("accounts/" + user.ToString());
						accountFile.WriteLine(JsonConvert.SerializeObject((object)result2));
					}
					Thread.Sleep(1500);
					await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Removed " + user + " " + gold + " gold", "If " + user + " has not had the gold removed. Your need to contact an admin."), (RequestOptions)null);
					AddTrendData(new TrendData
					{
						looseGold = ammountToRemove
					}, DateTime.Now, base.get_Context().get_Guild().get_Name());
					result2 = default(account);
				}
				catch (Exception e2)
				{
					Console.WriteLine(e2.Message);
					if (e2.Message.ToLower().Contains("user not found"))
					{
						await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("You to need to mention a real user", false, (Embed)null, (RequestOptions)null);
						return;
					}
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Sorry, something went wrong when doing the transaction.", false, (Embed)null, (RequestOptions)null);
				}
			}
			catch (Exception e)
			{
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("ERROR!! " + e.ToString(), false, (Embed)null, (RequestOptions)null);
			}
		}

		[Command("removeuser_bal")]
		public async Task removeuser_bal(string username, string gold = "X")
		{
			if (((SocketEntity<ulong>)(object)base.get_Context().get_Guild()).get_Id() != SENATE_GUILD)
			{
				return;
			}
			SocketUser user = base.get_Context().get_User();
			IRole role = Enumerable.FirstOrDefault<IRole>((IEnumerable<IRole>)(user as IGuildUser).get_Guild().get_Roles(), (Func<IRole, bool>)((IRole x) => x.get_Name() == "Imperial Gold Manager"));
			SocketUser user2 = base.get_Context().get_User();
			if (!Enumerable.Contains<IRole>((IEnumerable<IRole>)(user2 as SocketGuildUser).get_Roles(), role))
			{
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Plebs do not have permission to preform this command!", false, (Embed)null, (RequestOptions)null);
				return;
			}
			try
			{
				if (gold.Contains("$"))
				{
					gold = gold.TrimStart(new char[1]
					{
						'$'
					});
					gold = gold.TrimEnd(new char[1]
					{
						'$'
					});
				}
				if (username == "" || gold == "X")
				{
					if (!(username == ""))
					{
						await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Must be 0.01 gold or above and in numeric form", false, (Embed)null, (RequestOptions)null);
					}
					else
					{
						await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("A user must be mentioned.", false, (Embed)null, (RequestOptions)null);
					}
					return;
				}
				decimal ammountToRemove = default(decimal);
				if (!decimal.TryParse(gold, out ammountToRemove))
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("Value of gold must be a number.", false, (Embed)null, (RequestOptions)null);
					return;
				}
				if (username == "")
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("You need to mention someone when removing gold!! | !adminremove @user x", false, (Embed)null, (RequestOptions)null);
					return;
				}
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("trying Removing " + username + gold + " gold...", false, (Embed)null, (RequestOptions)null);
				account THEACCOUNT = default(account);
				DirectoryInfo d = new DirectoryInfo("accounts/");
				FileInfo[] Files = d.GetFiles();
				FileInfo[] array = Files;
				foreach (FileInfo file in array)
				{
					account result3 = JsonConvert.DeserializeObject<account>(File.ReadAllText(((FileSystemInfo)file).get_FullName()));
					try
					{
						if (result3.name.ToLower() == username.ToLower())
						{
							THEACCOUNT = result3;
						}
					}
					catch
					{
						Console.WriteLine("SKIPPED ON ADMIN USER REMOVE | " + result3.accountUserId);
					}
				}
				account result2 = default(account);
				using (StreamReader streamReader = new StreamReader("accounts/" + THEACCOUNT.accountUserId))
				{
					result2 = JsonConvert.DeserializeObject<account>(streamReader.ReadToEnd());
					streamReader.Close();
					result2.ballance -= ammountToRemove;
					if (result2.ballance < 0m)
					{
						result2.ballance = default(decimal);
					}
					result2.accountLogs.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
					result2.transactions.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
					using StreamWriter accountFile = new StreamWriter("accounts/" + THEACCOUNT.accountUserId);
					accountFile.WriteLine(JsonConvert.SerializeObject((object)result2));
				}
				Thread.Sleep(1500);
				await base.get_Context().get_Channel().SendMessageAsync("", false, SendEmmbedMessage("Removed " + THEACCOUNT.name + " " + gold + " gold", "If " + THEACCOUNT.name + " has not had the gold removed. Your need to contact an admin."), (RequestOptions)null);
				AddTrendData(new TrendData
				{
					looseGold = ammountToRemove
				}, DateTime.Now, base.get_Context().get_Guild().get_Name());
				result2 = default(account);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				if (!e.Message.ToLower().Contains("user not found"))
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync(e.ToString(), false, (Embed)null, (RequestOptions)null);
				}
				else
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("You to need to mention a real user", false, (Embed)null, (RequestOptions)null);
				}
			}
		}

		[Command("joinbranch")]
		public async Task removeuser_bal(string name)
		{
			foreach (SocketGuild g in ((BaseSocketClient)base.get_Context().get_Client()).get_Guilds())
			{
				if (g.get_Name().ToLower().Trim() == name.ToLower().Trim())
				{
					await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync(Enumerable.FirstOrDefault<string>(Enumerable.Select<RestInviteMetadata, string>((IEnumerable<RestInviteMetadata>)(await g.GetInvitesAsync((RequestOptions)null)), (Func<RestInviteMetadata, string>)((RestInviteMetadata x) => ((RestInvite)x).get_Url()))), false, (Embed)null, (RequestOptions)null);
				}
			}
		}

		[Command("list branches")]
		public async Task listbranches()
		{
			string together2 = "```";
			foreach (SocketGuild g in ((BaseSocketClient)base.get_Context().get_Client()).get_Guilds())
			{
				together2 = together2 + g.get_Name() + Environment.NewLine;
			}
			together2 += "```";
			await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync(together2, false, (Embed)null, (RequestOptions)null);
		}

		[Command("imperi")]
		public async Task IMPERIIMPERI()
		{
			decimal TOTALCIRCULATION = default(decimal);
			string[] accountFiles = Directory.GetFiles("accounts/");
			try
			{
				string[] array = accountFiles;
				foreach (string accountFile in array)
				{
					using StreamReader file = new StreamReader(accountFile);
					account result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
					file.Close();
					TOTALCIRCULATION += result.ballance;
				}
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("there is a total of '" + TOTALCIRCULATION + "' gold in circulation.", false, (Embed)null, (RequestOptions)null);
			}
			catch (Exception ex)
			{
				Exception e = ex;
				Console.WriteLine("1st");
				Console.WriteLine(e.ToString());
			}
		}

		[Command("imperi count")]
		public async Task IMPERIIMPERI_COUNT()
		{
			decimal TOTALACCOUNT = default(decimal);
			string[] accountFiles = Directory.GetFiles("accounts/");
			try
			{
				string[] array = accountFiles;
				foreach (string accountFile in array)
				{
					using StreamReader file = new StreamReader(accountFile);
					JsonConvert.DeserializeObject<account>(file.ReadToEnd());
					file.Close();
					TOTALACCOUNT += 1m;
				}
				await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync("there is a total of '" + TOTALACCOUNT + "' imperial gold accounts.", false, (Embed)null, (RequestOptions)null);
			}
			catch (Exception ex)
			{
				Exception e = ex;
				Console.WriteLine("1st");
				Console.WriteLine(e.ToString());
			}
		}

		public Commands()
			: this()
		{
		}
	}
}

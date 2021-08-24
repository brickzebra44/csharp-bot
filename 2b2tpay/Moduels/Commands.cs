// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Moduels.Commands
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using _2b2tpay.Data;
using _2b2tpay.Data.Businesses;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _2b2tpay.Moduels
{
  public class Commands : InteractiveBase
  {
    private static Random random = new Random();
    public List<_2b2tpay.Moduels.Commands.Rank> ranks = new List<_2b2tpay.Moduels.Commands.Rank>()
    {
      new _2b2tpay.Moduels.Commands.Rank("Recruit", 5M, 2),
      new _2b2tpay.Moduels.Commands.Rank("Auxiliary", 60M, 2),
      new _2b2tpay.Moduels.Commands.Rank("Quaestor", 100M, 5),
      new _2b2tpay.Moduels.Commands.Rank("Preafect", 200M, 10),
      new _2b2tpay.Moduels.Commands.Rank("Tribune", 400M, 20),
      new _2b2tpay.Moduels.Commands.Rank("Legate", 500M, 30)
    };
    public List<_2b2tpay.Moduels.Commands.TrendData> trendData = new List<_2b2tpay.Moduels.Commands.TrendData>();
    public ulong SENATE_GUILD = 721677685431992360;

    [Command("bal")]
    public async Task Bal(SocketUser user = null)
    {
      if (user == null)
      {
        SocketUser userInfo = this.Context.User;
        if (!File.Exists("accounts/" + userInfo.Id.ToString()))
        {
          Random random = new Random();
          account account = new account()
          {
            accountUserId = userInfo.Id,
            name = userInfo.Username,
            accountId = random.Next(0, 99999999),
            ballance = 0M,
            accountLogs = new List<accountLog>()
          };
          account.accountLogs.Add(new accountLog("Created account"));
          account.accountLogs.Add(new accountLog("Credit: 0 gold"));
          account.transactions = new List<accountLog>();
          account.transactions.Add(new accountLog("Credit: 0 gold"));
          account.isPublic = true;
          using (StreamWriter accountFile = new StreamWriter("accounts/" + account.accountUserId.ToString()))
            accountFile.WriteLine(JsonConvert.SerializeObject((object) account));
          random = (Random) null;
          account = new account();
        }
        account result = new account();
        ulong id = userInfo.Id;
        using (StreamReader file = new StreamReader("accounts/" + id.ToString()))
        {
          result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
          file.Close();
          result.accountLogs.Add(new accountLog("Accessed account balace"));
          id = userInfo.Id;
          using (StreamWriter accountFile = new StreamWriter("accounts/" + id.ToString()))
            accountFile.WriteLine(JsonConvert.SerializeObject((object) result));
        }
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage(string.Format("{0:0.00}", (object) result.ballance) + " gold"), (RequestOptions) null);
        userInfo = (SocketUser) null;
        result = new account();
      }
      else
      {
        SocketUser userInfo = user;
        ulong id = userInfo.Id;
        if (!File.Exists("accounts/" + id.ToString()))
        {
          Random random = new Random();
          account account = new account()
          {
            accountUserId = userInfo.Id,
            name = userInfo.Username,
            accountId = random.Next(0, 99999999),
            ballance = 0M,
            accountLogs = new List<accountLog>()
          };
          account.accountLogs.Add(new accountLog("Created account"));
          account.accountLogs.Add(new accountLog("Credit: 0 gold"));
          account.transactions = new List<accountLog>();
          account.transactions.Add(new accountLog("Credit: 0 gold"));
          account.isPublic = true;
          using (StreamWriter accountFile = new StreamWriter("accounts/" + account.accountUserId.ToString()))
            accountFile.WriteLine(JsonConvert.SerializeObject((object) account));
          random = (Random) null;
          account = new account();
        }
        account result = new account();
        id = userInfo.Id;
        using (StreamReader file = new StreamReader("accounts/" + id.ToString()))
        {
          result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
          file.Close();
          if (!result.isPublic)
          {
            RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("This user's account is currently private and you cannot see what they have."), (RequestOptions) null);
            return;
          }
          result.accountLogs.Add(new accountLog("Accessed account balace"));
          id = userInfo.Id;
          using (StreamWriter accountFile = new StreamWriter("accounts/" + id.ToString()))
            accountFile.WriteLine(JsonConvert.SerializeObject((object) result));
        }
        RestUserMessage restUserMessage1 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage(string.Format("{0:0.00}", (object) result.ballance) + "gold"), (RequestOptions) null);
        userInfo = (SocketUser) null;
        result = new account();
      }
      if (this.Context.Guild.Name != "The Imperials")
        await Job.CheckJobPayments(this.Context.Guild.Users.ToArray<SocketGuildUser>(), this.Context);
      this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
      {
        timesCheckedBal = 1,
        gainGold = Job.temp_JobPayments
      }, DateTime.Now, this.Context.Guild.Name);
      Job.temp_JobPayments = 0M;
    }

    public static Embed SendEmmbedMessage(
      string title,
      string message = "",
      bool randomImperialImage = false)
    {
      EmbedBuilder embedBuilder = new EmbedBuilder()
      {
        Author = new EmbedAuthorBuilder()
      };
      embedBuilder.Author.Name = title;
      embedBuilder.Description = message;
      embedBuilder.WithColor(Color.Gold);
      string[] strArray = new string[6]
      {
        "https://cdn.discordapp.com/attachments/692682149383634944/692682662493552720/ImperatorWhite.png",
        "https://cdn.discordapp.com/attachments/692682149383634944/692682283651694653/coolguy.png",
        "https://cdn.discordapp.com/attachments/692682149383634944/693354954206740520/LEGIONARY.png",
        "https://cdn.discordapp.com/attachments/692682149383634944/693354975497158737/d760cdc42b97b1da71461d613a7c561b9b2eb43e_hq.jpg",
        "https://cdn.discordapp.com/attachments/692682149383634944/693355033705709638/ImperialCoin.png",
        "https://cdn.discordapp.com/attachments/692682149383634944/693355092149010442/steamworkshop_webupload_previewfile_377822609_preview.png"
      };
      if (randomImperialImage)
        embedBuilder.WithThumbnailUrl(strArray[_2b2tpay.Moduels.Commands.random.Next(0, strArray.Length)]);
      return embedBuilder.Build();
    }

    [Command("pay")]
    public async Task Send(SocketUser user = null, string gold = "X")
    {
      if (user == this.Context.User)
      {
        IUserMessage userMessage1 = await this.ReplyAsync("No! You can't pay yourself! " + this.Context.User.Mention + " Patched due to an exploit.");
      }
      else
      {
        Console.WriteLine(this.Context.User.Username);
        if (gold.Contains("$"))
        {
          gold = gold.TrimStart('$');
          gold = gold.TrimEnd('$');
        }
        if (user == null || gold == "X")
        {
          if (user == null)
          {
            IUserMessage userMessage2 = await this.ReplyAsync("A user must be mentioned.");
          }
          else
          {
            IUserMessage userMessage3 = await this.ReplyAsync("Gold must be 1 or above and in numeric form");
          }
        }
        else
        {
          gold = PaymentVars.CheckData(gold);
          Decimal ammountToRemove = 0M;
          if (!Decimal.TryParse(gold, out ammountToRemove))
          {
            IUserMessage userMessage4 = await this.ReplyAsync("Value of gold must be a number.");
          }
          else if (ammountToRemove < 0.01M)
          {
            IUserMessage userMessage5 = await this.ReplyAsync("Must be 0.01 gold or above.");
          }
          else
          {
            try
            {
              if (user == null)
              {
                IUserMessage userMessage6 = await this.ReplyAsync("You need to mention someone when sending gold!! | !pay @user x");
              }
              else
              {
                IUserMessage userMessage7 = await this.ReplyAsync("Sending " + user.Username + " " + gold + " gold...");
                account result = new account();
                ulong id1 = this.Context.User.Id;
                using (StreamReader file = new StreamReader("accounts/" + id1.ToString()))
                {
                  result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
                  file.Close();
                  if (result.ballance < ammountToRemove)
                  {
                    IUserMessage userMessage8 = await this.ReplyAsync("You don't have enough gold to send.");
                    return;
                  }
                  result.ballance -= ammountToRemove;
                  result.accountLogs.Add(new accountLog("Sent " + user.Username + " " + gold + " gold"));
                  result.transactions.Add(new accountLog("sent " + user.Username + " " + gold + " gold"));
                  id1 = this.Context.User.Id;
                  using (StreamWriter accountFile = new StreamWriter("accounts/" + id1.ToString()))
                    accountFile.WriteLine(JsonConvert.SerializeObject((object) result));
                }
                bool hasAnAccount = true;
                SocketUser userInfo = user;
                if (!File.Exists("accounts/" + userInfo.Id.ToString()))
                {
                  hasAnAccount = false;
                  Random random = new Random();
                  account account = new account()
                  {
                    accountUserId = userInfo.Id,
                    name = userInfo.Username,
                    accountId = random.Next(0, 99999999),
                    ballance = 0M,
                    accountLogs = new List<accountLog>()
                  };
                  account.accountLogs.Add(new accountLog("Created account"));
                  account.accountLogs.Add(new accountLog("Credit: 0 gold"));
                  account.transactions = new List<accountLog>();
                  account.transactions.Add(new accountLog("Credit: 0 gold"));
                  account.isPublic = true;
                  using (StreamWriter accountFile = new StreamWriter("accounts/" + account.accountUserId.ToString()))
                    accountFile.WriteLine(JsonConvert.SerializeObject((object) account));
                  random = (Random) null;
                  account = new account();
                }
                account result2 = new account();
                ulong id2 = user.Id;
                using (StreamReader file = new StreamReader("accounts/" + id2.ToString()))
                {
                  result2 = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
                  file.Close();
                  result2.ballance += ammountToRemove;
                  result2.accountLogs.Add(new accountLog("Recivved " + gold + " gold from " + this.Context.User?.ToString()));
                  result2.transactions.Add(new accountLog("received  " + gold + " gold from " + this.Context.User?.ToString()));
                  id2 = user.Id;
                  using (StreamWriter accountFile = new StreamWriter("accounts/" + id2.ToString()))
                    accountFile.WriteLine(JsonConvert.SerializeObject((object) result2));
                }
                Thread.Sleep(1500);
                RestUserMessage restUserMessage1 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Sent " + user.Username + " " + gold + " gold", "If " + user.Username + " has not recived the gold. Your or " + user.Username + " need to contact an admin. You can check you have the gold by doing !transactions"), (RequestOptions) null);
                this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
                {
                  transactionGold = ammountToRemove
                }, DateTime.Now, this.Context.Guild.Name);
                try
                {
                  Task<IDMChannel> h = user.GetOrCreateDMChannelAsync((RequestOptions) null);
                  if (hasAnAccount)
                  {
                    IUserMessage userMessage9 = await h.Result.SendMessageAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("You have recvived " + gold + " gold from " + this.Context.User.Username, "This is a notification to let you know that you have gained some more gold!"));
                  }
                  else
                  {
                    IUserMessage userMessage10 = await h.Result.SendMessageAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("You have recvived " + gold + " gold from " + this.Context.User.Username, "If you would like to find out more about Imperial Gold, join the discord here: https://discord.gg/QsuTcrt"));
                  }
                  h = (Task<IDMChannel>) null;
                }
                catch (Exception ex)
                {
                  Thread.Sleep(500);
                  RestUserMessage restUserMessage2 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Sorry, but discord has blocked the delivery message directed to " + user.Username + ", if you want him to be notifyed of this transaction you may need to notify him yourself."), (RequestOptions) null);
                }
                result = new account();
                userInfo = (SocketUser) null;
                result2 = new account();
              }
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex.Message);
              if (ex.Message.ToLower().Contains("user not found"))
              {
                IUserMessage userMessage11 = await this.ReplyAsync("You to need to mention a real user");
                return;
              }
              IUserMessage userMessage12 = await this.ReplyAsync("Sorry, something went wrong when doing the transaction.");
            }
          }
        }
      }
    }

    [Command("adminpay")]
    public async Task adminpay(SocketUser user = null, string gold = "X")
    {
      IRole role = (this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name == "Imperial Gold Manager"));
      if (!((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>(role))
      {
        IUserMessage userMessage1 = await this.ReplyAsync("Plebs do not have permission to preform this command!");
      }
      else
      {
        try
        {
          if (gold.Contains("$"))
          {
            gold = gold.TrimStart('$');
            gold = gold.TrimEnd('$');
          }
          if (user == null || gold == "X")
          {
            if (user == null)
            {
              IUserMessage userMessage2 = await this.ReplyAsync("A user must be mentioned.");
              return;
            }
            IUserMessage userMessage3 = await this.ReplyAsync("Must be 0.01 gold or above and in numeric form");
            return;
          }
          gold = PaymentVars.CheckData(gold);
          Decimal ammountToRemove = 0M;
          if (!Decimal.TryParse(gold, out ammountToRemove))
          {
            IUserMessage userMessage4 = await this.ReplyAsync("Value of gold must be a number.");
            return;
          }
          if (user == null)
          {
            IUserMessage userMessage5 = await this.ReplyAsync("You need to mention someone when sending gold!! | !pay @user x");
          }
          else
          {
            IUserMessage userMessage6 = await this.ReplyAsync("Sending " + user.Username + gold + " gold...");
            bool hasAnAccount = true;
            SocketUser userInfo = user;
            ulong id = userInfo.Id;
            if (!File.Exists("accounts/" + id.ToString()))
            {
              hasAnAccount = false;
              Random random = new Random();
              account account = new account()
              {
                accountUserId = userInfo.Id,
                name = userInfo.Username,
                accountId = random.Next(0, 99999999),
                ballance = 0M,
                accountLogs = new List<accountLog>()
              };
              account.accountLogs.Add(new accountLog("Created account"));
              account.accountLogs.Add(new accountLog("Credit: 0 gold"));
              account.transactions = new List<accountLog>();
              account.transactions.Add(new accountLog("Credit: 0 gold"));
              account.isPublic = true;
              using (StreamWriter accountFile = new StreamWriter("accounts/" + account.accountUserId.ToString()))
                accountFile.WriteLine(JsonConvert.SerializeObject((object) account));
              random = (Random) null;
              account = new account();
            }
            account result2 = new account();
            id = user.Id;
            using (StreamReader file = new StreamReader("accounts/" + id.ToString()))
            {
              result2 = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
              file.Close();
              result2.ballance += ammountToRemove;
              result2.accountLogs.Add(new accountLog("Recivved " + gold + " gold from the Bank of Imperial"));
              result2.transactions.Add(new accountLog("received  " + gold + " gold from the Bank of Imperial"));
              id = user.Id;
              using (StreamWriter accountFile = new StreamWriter("accounts/" + id.ToString()))
                accountFile.WriteLine(JsonConvert.SerializeObject((object) result2));
            }
            Thread.Sleep(1500);
            RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Sent " + user.Username + " " + gold + " gold", "If " + user.Username + " has not recived the gold. Your or " + user.Username + " need to contact an admin. You can check you have the gold by doing !transactions"), (RequestOptions) null);
            this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
            {
              gainGold = ammountToRemove
            }, DateTime.Now, this.Context.Guild.Name);
            Task<IDMChannel> h = user.GetOrCreateDMChannelAsync((RequestOptions) null);
            if (hasAnAccount)
            {
              IUserMessage userMessage7 = await h.Result.SendMessageAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("You have recvived " + gold + " gold from " + this.Context.User.Username, "This is a notification to let you know that you have gained some more gold!"));
            }
            else
            {
              IUserMessage userMessage8 = await h.Result.SendMessageAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("You have recvived " + gold + " gold from " + this.Context.User.Username, "If you would like to find out more about Imperial Bank, join the discord here: https://discord.gg/QsuTcrt"));
            }
            userInfo = (SocketUser) null;
            result2 = new account();
            h = (Task<IDMChannel>) null;
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          if (ex.Message.ToLower().Contains("user not found"))
          {
            IUserMessage userMessage9 = await this.ReplyAsync("You to need to mention a real user");
            return;
          }
          IUserMessage userMessage10 = await this.ReplyAsync("Sorry, something went wrong when doing the transaction.");
        }
      }
    }

    [Command("adminremove")]
    public async Task adminremove(SocketUser user = null, string gold = "X")
    {
      IRole role = (this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name == "Imperial Gold Manager"));
      if (!((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>(role))
      {
        IUserMessage userMessage1 = await this.ReplyAsync("Plebs do not have permission to preform this command!");
      }
      else
      {
        try
        {
          if (gold.Contains("$"))
          {
            gold = gold.TrimStart('$');
            gold = gold.TrimEnd('$');
          }
          if (user == null || gold == "X")
          {
            if (user == null)
            {
              IUserMessage userMessage2 = await this.ReplyAsync("A user must be mentioned.");
              return;
            }
            IUserMessage userMessage3 = await this.ReplyAsync("Must be 0.01 gold or above and in numeric form");
            return;
          }
          Decimal ammountToRemove = 0M;
          if (!Decimal.TryParse(gold, out ammountToRemove))
          {
            IUserMessage userMessage4 = await this.ReplyAsync("Value of gold must be a number.");
            return;
          }
          if (user == null)
          {
            IUserMessage userMessage5 = await this.ReplyAsync("You need to mention someone when removing gold!! | !adminremove @user x");
          }
          else
          {
            IUserMessage userMessage6 = await this.ReplyAsync("Removing " + user.Username + gold + " gold...");
            bool hasAnAccount = true;
            SocketUser userInfo = user;
            ulong id = userInfo.Id;
            if (!File.Exists("accounts/" + id.ToString()))
            {
              hasAnAccount = false;
              Random random = new Random();
              account account = new account()
              {
                name = userInfo.Username,
                accountUserId = userInfo.Id,
                accountId = random.Next(0, 99999999),
                ballance = 0M,
                accountLogs = new List<accountLog>()
              };
              account.accountLogs.Add(new accountLog("Created account"));
              account.accountLogs.Add(new accountLog("Credit: 0 gold"));
              account.transactions = new List<accountLog>();
              account.transactions.Add(new accountLog("Credit: 0 gold"));
              account.isPublic = true;
              using (StreamWriter accountFile = new StreamWriter("accounts/" + account.accountUserId.ToString()))
                accountFile.WriteLine(JsonConvert.SerializeObject((object) account));
              random = (Random) null;
              account = new account();
            }
            account result2 = new account();
            id = user.Id;
            using (StreamReader file = new StreamReader("accounts/" + id.ToString()))
            {
              result2 = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
              file.Close();
              result2.ballance -= ammountToRemove;
              if (result2.ballance < 0M)
                result2.ballance = 0M;
              result2.accountLogs.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
              result2.transactions.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
              id = user.Id;
              using (StreamWriter accountFile = new StreamWriter("accounts/" + id.ToString()))
                accountFile.WriteLine(JsonConvert.SerializeObject((object) result2));
            }
            Thread.Sleep(1500);
            RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Removed " + user.Username + " " + gold + " gold", "If " + user.Username + " has not had the gold removed. Your need to contact an admin."), (RequestOptions) null);
            this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
            {
              looseGold = ammountToRemove
            }, DateTime.Now, this.Context.Guild.Name);
            Task<IDMChannel> h = user.GetOrCreateDMChannelAsync((RequestOptions) null);
            if (hasAnAccount)
            {
              IUserMessage userMessage7 = await h.Result.SendMessageAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage(gold + " gold has been taken from your account by " + this.Context.User.Username, "This is a notification to let you know that gold has been removed. If your not sure why it has been removed, check with " + this.Context.User.Username + " to see why it has been removed."));
            }
            else
            {
              IUserMessage userMessage8 = await h.Result.SendMessageAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("You have had " + gold + " gold removed, done by Admin " + this.Context.User.Username, "If you would like to find out more about Imperial Bank, join the discord here: https://discord.gg/QsuTcrt"));
            }
            userInfo = (SocketUser) null;
            result2 = new account();
            h = (Task<IDMChannel>) null;
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          if (ex.Message.ToLower().Contains("user not found"))
          {
            IUserMessage userMessage9 = await this.ReplyAsync("You to need to mention a real user");
            return;
          }
          IUserMessage userMessage10 = await this.ReplyAsync("Sorry, something went wrong when doing the transaction.");
        }
      }
    }

    public async Task Announce(string title, string message, Color color)
    {
      DiscordSocketClient _client = new DiscordSocketClient();
      ulong id = 657355671070834709;
      IMessageChannel chnl = _client.GetChannel(id) as IMessageChannel;
      EmbedBuilder builder = new EmbedBuilder();
      builder.Title = title;
      builder.Description = message;
      builder.WithColor(color);
      IUserMessage userMessage = await chnl.SendMessageAsync("", embed: builder.Build());
    }

    [Command("baltop")]
    public async Task BalTop()
    {
      List<account> accounts = new List<account>();
      string[] accountFiles = Directory.GetFiles("accounts/");
      try
      {
        string[] strArray = accountFiles;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string accountFile = strArray[index];
          account result = new account();
          using (StreamReader file = new StreamReader(accountFile))
          {
            result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
            file.Close();
            accounts.Add(result);
          }
          result = new account();
          accountFile = (string) null;
        }
        strArray = (string[]) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine("1st");
        Console.WriteLine(ex.ToString());
      }
      try
      {
        string ToSay = "";
        Console.WriteLine("YES");
        accounts = _2b2tpay.Data.BalTop.SortAccounts(accounts);
        int i = 0;
        foreach (account account1 in accounts)
        {
          account account = account1;
          try
          {
            if (i < 10)
            {
              ++i;
              ToSay = ToSay + "[" + i.ToString() + "] " + this.Context.Guild.GetUser(account.accountUserId).Username + " | " + string.Format("{0:0.00}", (object) account.ballance) + " gold" + Environment.NewLine;
            }
            else
              break;
          }
          catch (Exception ex)
          {
            Exception e = ex;
            ToSay = ToSay + "[" + i.ToString() + "] " + account.name + " | " + account.ballance.ToString() + " gold" + Environment.NewLine;
          }
          account = new account();
        }
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Imperial Forbes List", ToSay, true), (RequestOptions) null);
        ToSay = (string) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine("2nd");
        Console.WriteLine(ex.ToString());
        Console.WriteLine(Environment.NewLine + ex.Message);
      }
    }

    [Command("private")]
    public async Task Private()
    {
      account result = new account();
      ulong id = this.Context.User.Id;
      using (StreamReader file = new StreamReader("accounts/" + id.ToString()))
      {
        result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
        file.Close();
        if (!result.isPublic)
        {
          RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("You account is already private and will not be displayed on the !baltop list.", false, (Embed) null, (RequestOptions) null);
          return;
        }
        result.isPublic = false;
        result.accountLogs.Add(new accountLog("Set account to private. "));
        id = this.Context.User.Id;
        using (StreamWriter accountFile = new StreamWriter("accounts/" + id.ToString()))
          accountFile.WriteLine(JsonConvert.SerializeObject((object) result));
      }
      RestUserMessage restUserMessage1 = await this.Context.Channel.SendMessageAsync("Your account has been set to private, your account will not be displayed on !baltop, nor will anyone be able to see your account value.", false, (Embed) null, (RequestOptions) null);
      this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
      {
        timesMadePrivate = 1
      }, DateTime.Now, this.Context.Guild.Name);
    }

    [Command("public")]
    public async Task Public()
    {
      account result = new account();
      ulong id = this.Context.User.Id;
      using (StreamReader file = new StreamReader("accounts/" + id.ToString()))
      {
        result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
        file.Close();
        if (result.isPublic)
        {
          RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("You account is already public and will be displayed on the !baltop list.", false, (Embed) null, (RequestOptions) null);
          return;
        }
        result.isPublic = true;
        result.accountLogs.Add(new accountLog("Set account to public. "));
        id = this.Context.User.Id;
        using (StreamWriter accountFile = new StreamWriter("accounts/" + id.ToString()))
          accountFile.WriteLine(JsonConvert.SerializeObject((object) result));
        RestUserMessage restUserMessage1 = await this.Context.Channel.SendMessageAsync("Your account has been set to public, people will be able to see your account value.", false, (Embed) null, (RequestOptions) null);
        this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
        {
          timesMadePublic = 1
        }, DateTime.Now, this.Context.Guild.Name);
      }
    }

    [Command("help")]
    public async Task Help()
    {
      RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage(nameof (Commands), "!bal - user ballance" + Environment.NewLine + "!pay - pay a user" + Environment.NewLine + "!baltop - check the top users across the Imperial" + Environment.NewLine + "!public - set your account to public" + Environment.NewLine + "!private - set your account to private" + Environment.NewLine + "!rankup - use gold to rankup to next rank (applies to current branch only)" + Environment.NewLine + "!logs - get all account actions in a text file" + Environment.NewLine + "!transactions - get all transactions made on the account" + Environment.NewLine + "!help data - see data-related commands" + Environment.NewLine + "!it - see users time in imperials" + Environment.NewLine + "!advertisement - for 100 gold will ping @everyone your advertisement (make sure to include it after the command, and include an @ everyone ping)" + Environment.NewLine), (RequestOptions) null);
    }

    [Command("help data")]
    public async Task HelpData()
    {
      RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage(nameof (Commands), "!trends - see today's staticits" + Environment.NewLine + "!gdp - see today's gdp increase percent" + Environment.NewLine + "!compare DATE DATE - compare 2 dates gdp." + Environment.NewLine), (RequestOptions) null);
    }

    [Command("adminprivate")]
    public async Task Private(SocketUser user = null)
    {
      IRole role = (this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name == "Imperial Gold Manager"));
      if (!((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>(role))
      {
        IUserMessage userMessage = await this.ReplyAsync("Plebs do not have permission to preform this command!");
      }
      else
      {
        account result = new account();
        ulong id = user.Id;
        using (StreamReader file = new StreamReader("accounts/" + id.ToString()))
        {
          result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
          file.Close();
          if (!result.isPublic)
          {
            RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync(user.Mention + "'s account is already private and will not be displayed on the !baltop list.", false, (Embed) null, (RequestOptions) null);
            return;
          }
          result.isPublic = false;
          result.accountLogs.Add(new accountLog("Admin set your account to private. "));
          id = user.Id;
          using (StreamWriter accountFile = new StreamWriter("accounts/" + id.ToString()))
            accountFile.WriteLine(JsonConvert.SerializeObject((object) result));
        }
        RestUserMessage restUserMessage1 = await this.Context.Channel.SendMessageAsync(user.Mention + "'s account has been set to private, " + user.Mention + "'s  account will not be displayed on !baltop, nor will anyone be able to see " + user.Mention + "'s  account value.", false, (Embed) null, (RequestOptions) null);
        this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
        {
          timesMadePrivate = 1
        }, DateTime.Now, this.Context.Guild.Name);
        result = new account();
      }
    }

    [Command("logs")]
    public async Task Logs()
    {
      try
      {
        SocketUser userInfo = this.Context.User;
        account result = new account();
        ulong id = userInfo.Id;
        result = JsonConvert.DeserializeObject<account>(File.ReadAllText("accounts/" + id.ToString()));
        result.accountLogs.Add(new accountLog("Accessed account logs"));
        id = userInfo.Id;
        File.WriteAllText("accounts/" + id.ToString(), JsonConvert.SerializeObject((object) result));
        File.WriteAllText("data\\logs.txt", "Imperial 2020 - Logs of " + userInfo.Username + Environment.NewLine);
        using (StreamWriter w = File.AppendText("data\\logs.txt"))
        {
          foreach (accountLog accountLog1 in result.accountLogs)
          {
            accountLog accountLog = accountLog1;
            w.WriteLine("[" + accountLog.timeOfLog.ToString() + "] " + accountLog.detailOfLog);
            accountLog = new accountLog();
          }
          w.Close();
        }
        RestUserMessage restUserMessage = await this.Context.Channel.SendFileAsync("data\\logs.txt", "Here is all of the logged data in a text format.", false, (Embed) null, (RequestOptions) null, false);
        userInfo = (SocketUser) null;
        result = new account();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message + Environment.NewLine + " () -- " + ex.ToString());
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("Sorry, something has gone wrong when trying to ascess your logs. If you desperatly need your logs please contact an admin", false, (Embed) null, (RequestOptions) null);
      }
      this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
      {
        timesCheckedLogs = 1
      }, DateTime.Now, this.Context.Guild.Name);
    }

    [Command("transactions")]
    public async Task transactions()
    {
      try
      {
        SocketUser userInfo = this.Context.User;
        account result = new account();
        ulong id = userInfo.Id;
        result = JsonConvert.DeserializeObject<account>(File.ReadAllText("accounts/" + id.ToString()));
        id = userInfo.Id;
        File.WriteAllText("accounts/" + id.ToString(), JsonConvert.SerializeObject((object) result));
        File.WriteAllText("data\\transactions.txt", "Imperial 2020 - Transactions of " + userInfo.Username + Environment.NewLine);
        using (StreamWriter w = File.AppendText("data\\transactions.txt"))
        {
          foreach (accountLog transaction in result.transactions)
          {
            accountLog accountLog = transaction;
            w.WriteLine("[" + accountLog.timeOfLog.ToString() + "] " + accountLog.detailOfLog);
            accountLog = new accountLog();
          }
          w.Close();
        }
        RestUserMessage restUserMessage = await this.Context.Channel.SendFileAsync("data\\transactions.txt", "Here is all of the logged transaction data in a text format.", false, (Embed) null, (RequestOptions) null, false);
        userInfo = (SocketUser) null;
        result = new account();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message + Environment.NewLine + " () -- " + ex.ToString());
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("Sorry, something has gone wrong when trying to ascess your transaction logs. If you desperatly need your transaction logs please contact an admin", false, (Embed) null, (RequestOptions) null);
      }
    }

    [Command("rankup2")]
    public async Task RankUp()
    {
      if (this.Context.Guild.Name == "The Imperials")
      {
        RestUserMessage restUserMessage1 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Sorry! But that is Illegal!", "The main imperial server does not allow rank ups, you will need to choose a branch to use it on."), (RequestOptions) null);
      }
      else
      {
        try
        {
          Decimal amountToRemove = 0M;
          int counter = -1;
          IRole LEGATE = (this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Legate")));
          IRole RECRUIT = (this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Recruit")));
          _2b2tpay.Moduels.Commands.Rank lowestRole = new _2b2tpay.Moduels.Commands.Rank();
          _2b2tpay.Moduels.Commands.Rank highestRole = new _2b2tpay.Moduels.Commands.Rank();
          bool hasBeenUsed = false;
          foreach (_2b2tpay.Moduels.Commands.Rank rank in this.ranks)
          {
            _2b2tpay.Moduels.Commands.Rank rank1 = rank;
            IRole role3 = (this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains(rank1.name)));
            if (((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>(role3))
            {
              if (!hasBeenUsed)
              {
                lowestRole = rank1;
                hasBeenUsed = true;
              }
              highestRole = rank1;
            }
            role3 = (IRole) null;
          }
          Console.WriteLine(lowestRole.name);
          Console.WriteLine(highestRole.name);
          _2b2tpay.Moduels.Commands.Rank rankNeedingToProgressTo = this.ranks[this.ranks.IndexOf(highestRole) + 1];
          if (rankNeedingToProgressTo.name == "Recruit")
          {
            RestUserMessage restUserMessage2 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("You can't purchace membership", "You cannot buy ranks unlesss you apply to the Imperials. You need to be a citzen to join!"), (RequestOptions) null);
            return;
          }
          Console.WriteLine(rankNeedingToProgressTo.name);
          IRole role = (this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains(rankNeedingToProgressTo.name)));
          if (((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>(LEGATE) || ((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Emperor")))) || ((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("High General")))) || ((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("High Centurion")))) || ((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Consul")))) || ((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Centurion")))) || ((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Optio")))) || ((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Senator")))))
          {
            RestUserMessage restUserMessage3 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Max Rank Achieved!", "Congrats! You have progressed to the highest rank you can purchace!"), (RequestOptions) null);
          }
          else
          {
            amountToRemove = rankNeedingToProgressTo.cost;
            account result = new account();
            using (StreamReader file = new StreamReader("accounts/" + this.Context.User.Id.ToString()))
            {
              result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
              file.Close();
              if (result.ballance < amountToRemove)
              {
                RestUserMessage restUserMessage4 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("You don't have enough gold to purchace rank " + rankNeedingToProgressTo.name + "!", "You need " + (amountToRemove - result.ballance).ToString() + " more gold to purchace this rank..."), (RequestOptions) null);
                return;
              }
              DateTime now = DateTime.Now;
              if (int.Parse(Math.Round(now.Subtract(result.accountLogs[0].timeOfLog).TotalDays).ToString()) < rankNeedingToProgressTo.minimumDays)
              {
                ISocketMessageChannel channel = this.Context.Channel;
                string title = "You don't have enough time in the Imperials to purchace rank " + rankNeedingToProgressTo.name + "!";
                double minimumDays = (double) rankNeedingToProgressTo.minimumDays;
                now = DateTime.Now;
                double num = Math.Round(now.Subtract(result.accountLogs[0].timeOfLog).TotalDays, 2);
                string message = "You need " + (minimumDays - num).ToString() + " more days before you can purchace this rank...";
                Embed embed = _2b2tpay.Moduels.Commands.SendEmmbedMessage(title, message);
                RestUserMessage restUserMessage5 = await channel.SendMessageAsync("", false, embed, (RequestOptions) null);
                return;
              }
              result.ballance -= amountToRemove;
              await (this.Context.User as IGuildUser).AddRoleAsync(role);
              result.accountLogs.Add(new accountLog("Bought rank " + rankNeedingToProgressTo.name + " for " + rankNeedingToProgressTo.cost.ToString()));
              using (StreamWriter accountFile = new StreamWriter("accounts/" + this.Context.User.Id.ToString()))
                accountFile.WriteLine(JsonConvert.SerializeObject((object) result));
              this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
              {
                looseGold = rankNeedingToProgressTo.cost,
                timesRankedUp = 1
              }, DateTime.Now, this.Context.Guild.Name);
              if (rankNeedingToProgressTo.name == "Auxiliary")
              {
                RestUserMessage restUserMessage6 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("You have purchaced " + rankNeedingToProgressTo.name + "!", "Congratulations! Now that you are an Auxiliary, you can apply to join a base!", true), (RequestOptions) null);
              }
              else if (rankNeedingToProgressTo.name == "Tribune")
              {
                RestUserMessage restUserMessage7 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("You have purchaced " + rankNeedingToProgressTo.name + "!", "Congratulations! Now that you are a Tribune you can start to play more of a leader role in this branch, such as running a base or becomming a highway manager. Speak to your centurion to find out more information.", true), (RequestOptions) null);
              }
              else
              {
                RestUserMessage restUserMessage8 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("You have purchaced " + rankNeedingToProgressTo.name + "!", "Congratulations! When you climb ranks in the Imperials, you are more trusted in the Imperial community. All hail the Empire!! ", true), (RequestOptions) null);
              }
              IGuildUser test = this.Context.User as IGuildUser;
              foreach (SocketRole r in (IEnumerable<SocketRole>) (this.Context.User as SocketGuildUser).Roles)
              {
                if (r.Name.ToLower().Contains(highestRole.name.ToLower()))
                  await test.RemoveRoleAsync((IRole) r);
              }
              test = (IGuildUser) null;
            }
            result = new account();
          }
          if (counter != this.ranks.Count)
            ;
          LEGATE = (IRole) null;
          RECRUIT = (IRole) null;
          lowestRole = new _2b2tpay.Moduels.Commands.Rank();
          highestRole = new _2b2tpay.Moduels.Commands.Rank();
          role = (IRole) null;
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          IUserMessage userMessage = await this.ReplyAsync("Sorry, something went wrong when trying to purchace your rank!");
        }
      }
    }

    [Command("rankup")]
    public async Task RankUp2()
    {
      if (this.Context.Guild.Name == "The Imperials")
      {
        RestUserMessage restUserMessage1 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Sorry! But that is Illegal!", "The main imperial server does not allow rank ups, you will need to choose a branch to use it on."), (RequestOptions) null);
      }
      else
      {
        try
        {
          if (((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Legate")))))
          {
            RestUserMessage restUserMessage2 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Max Rank Achieved!", "Congrats! You have progressed to the highest rank you can purchace!"), (RequestOptions) null);
            return;
          }
          if (((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Tribune")))))
          {
            int num1 = await this.TryUpgradeRank(this.ranks[5].name, this.ranks[5].cost, this.ranks[5].minimumDays) ? 1 : 0;
          }
          else if (((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Preafect")))))
          {
            int num2 = await this.TryUpgradeRank(this.ranks[4].name, this.ranks[4].cost, this.ranks[4].minimumDays) ? 1 : 0;
          }
          else if (((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Quaestor")))))
          {
            int num3 = await this.TryUpgradeRank(this.ranks[3].name, this.ranks[3].cost, this.ranks[3].minimumDays) ? 1 : 0;
          }
          else if (((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Auxiliary")))))
          {
            int num4 = await this.TryUpgradeRank(this.ranks[2].name, this.ranks[2].cost, this.ranks[2].minimumDays) ? 1 : 0;
          }
          else if (((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains("Recruit")))))
          {
            int num5 = await this.TryUpgradeRank(this.ranks[1].name, this.ranks[1].cost, this.ranks[1].minimumDays) ? 1 : 0;
          }
          else
          {
            RestUserMessage restUserMessage3 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("You can't rankup", "You are either not a citzen of the Imperials, or you at a non-purchaceable rank!"), (RequestOptions) null);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          IUserMessage userMessage = await this.ReplyAsync("Sorry, something went wrong when trying to purchace your rank!");
        }
      }
    }

    private async Task<bool> TryUpgradeRank(string rank, Decimal amount, int mindays)
    {
      IRole role = (this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains(rank)));
      account result = new account();
      ulong id = this.Context.User.Id;
      using (StreamReader file = new StreamReader("accounts/" + id.ToString()))
      {
        result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
        file.Close();
        if (result.ballance < amount)
        {
          RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("You don't have enough gold to purchace rank " + rank + "!", "You need " + (amount - result.ballance).ToString() + " more gold to purchace this rank..."), (RequestOptions) null);
          return false;
        }
        DateTime now = DateTime.Now;
        double num1 = Math.Round(now.Subtract(result.accountLogs[0].timeOfLog).TotalDays);
        if (int.Parse(num1.ToString()) < mindays)
        {
          ISocketMessageChannel channel = this.Context.Channel;
          string title = "You don't have enough time in the Imperials to purchace rank " + rank + "!";
          double num2 = (double) mindays;
          now = DateTime.Now;
          double num3 = Math.Round(now.Subtract(result.accountLogs[0].timeOfLog).TotalDays, 2);
          num1 = num2 - num3;
          string message = "You need " + num1.ToString() + " more days before you can purchace this rank...";
          Embed embed = _2b2tpay.Moduels.Commands.SendEmmbedMessage(title, message);
          RestUserMessage restUserMessage = await channel.SendMessageAsync("", false, embed, (RequestOptions) null);
          return false;
        }
        result.ballance -= amount;
        await (this.Context.User as IGuildUser).AddRoleAsync(role);
        result.accountLogs.Add(new accountLog("Bought rank " + rank + " for " + amount.ToString()));
        id = this.Context.User.Id;
        using (StreamWriter accountFile = new StreamWriter("accounts/" + id.ToString()))
          accountFile.WriteLine(JsonConvert.SerializeObject((object) result));
        this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
        {
          looseGold = amount,
          timesRankedUp = 1
        }, DateTime.Now, this.Context.Guild.Name);
        if (rank == "Auxiliary")
        {
          RestUserMessage restUserMessage1 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("You have purchaced " + rank + "!", "Congratulations! Now that you are an Auxiliary, you can apply to join a base!", true), (RequestOptions) null);
        }
        else if (rank == "Tribune")
        {
          RestUserMessage restUserMessage2 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("You have purchaced " + rank + "!", "Congratulations! Now that you are a Tribune you can start to play more of a leader role in this branch, such as running a base or becomming a highway manager. Speak to your centurion to find out more information.", true), (RequestOptions) null);
        }
        else
        {
          RestUserMessage restUserMessage3 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("You have purchaced " + rank + "!", "Congratulations! When you climb ranks in the Imperials, you are more trusted in the Imperial community. All hail the Empire!! ", true), (RequestOptions) null);
        }
        IGuildUser test = this.Context.User as IGuildUser;
        foreach (SocketRole r in (IEnumerable<SocketRole>) (this.Context.User as SocketGuildUser).Roles)
        {
          foreach (_2b2tpay.Moduels.Commands.Rank rank2 in this.ranks)
          {
            _2b2tpay.Moduels.Commands.Rank rank1 = rank2;
            if (rank1.name != rank && rank1.name == r.Name)
              await test.RemoveRoleAsync((IRole) r);
            rank1 = new _2b2tpay.Moduels.Commands.Rank();
          }
        }
        test = (IGuildUser) null;
      }
      return true;
    }

    [Command("propaganda")]
    public async Task Propaganda()
    {
      EmbedBuilder builder = new EmbedBuilder()
      {
        Author = new EmbedAuthorBuilder()
      };
      builder.Author.Name = "Imperials on top!";
      builder.Description = "The Emprie is law and the law is sacred!";
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
      builder.WithImageUrl(randomImpImagesIndex[_2b2tpay.Moduels.Commands.random.Next(0, randomImpImagesIndex.Length)]);
      RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, builder.Build(), (RequestOptions) null);
      this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
      {
        patriatismCount = 1
      }, DateTime.Now, this.Context.Guild.Name);
    }

    [Command("adminlogs")]
    public async Task Logs(string userID)
    {
      IRole role = (this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name == "Imperial Gold Manager"));
      if (!((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>(role))
      {
        IUserMessage userMessage = await this.ReplyAsync("Plebs do not have permission to preform this command!");
      }
      else
      {
        try
        {
          SocketUser userInfo = this.Context.User;
          account result = new account();
          result = JsonConvert.DeserializeObject<account>(File.ReadAllText("accounts/" + userID));
          File.WriteAllText("data\\logs.txt", "Imperial 2020 - Logs of " + userID + Environment.NewLine);
          using (StreamWriter w = File.AppendText("data\\logs.txt"))
          {
            foreach (accountLog accountLog1 in result.accountLogs)
            {
              accountLog accountLog = accountLog1;
              w.WriteLine("[" + accountLog.timeOfLog.ToString() + "] " + accountLog.detailOfLog);
              accountLog = new accountLog();
            }
            w.Close();
          }
          RestUserMessage restUserMessage = await this.Context.Channel.SendFileAsync("data\\logs.txt", "Here is all of the " + userID + " logged data in a text format.", false, (Embed) null, (RequestOptions) null, false);
          userInfo = (SocketUser) null;
          result = new account();
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message + Environment.NewLine + " () -- " + ex.ToString());
          RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("Sorry, something has gone wrong when trying to ascess your logs. If you desperatly need your logs please contact an admin", false, (Embed) null, (RequestOptions) null);
        }
      }
    }

    [Command("trends")]
    public async Task Trends()
    {
      try
      {
        if (this.trendData.Count == 0)
          this.trendData = this.LoadTrendData();
        string together = "";
        foreach (_2b2tpay.Moduels.Commands.TrendData trend in this.trendData)
        {
          if (trend.date == DateTime.Today.Date && trend.server == this.Context.Guild.Name)
            together = string.Format("GDP today: {0} {1}", (object) trend.gainGold, (object) Environment.NewLine) + string.Format("GDL today: {0} {1}", (object) trend.looseGold, (object) Environment.NewLine) + string.Format("Gold Traded Today: {0} {1}", (object) trend.transactionGold, (object) Environment.NewLine) + "-= Other Statistics =-" + Environment.NewLine + string.Format("Ballance Checks Today: {0} {1}", (object) trend.timesCheckedBal, (object) Environment.NewLine) + string.Format("Times Ranked Up Today: {0} {1}", (object) trend.timesRankedUp, (object) Environment.NewLine) + string.Format("Number of accounts made public today: {0} {1}", (object) trend.timesMadePublic, (object) Environment.NewLine) + string.Format("Number of accounts made private today: {0} {1}", (object) trend.timesMadePrivate, (object) Environment.NewLine) + string.Format("Imperial Patriotism Count: {0} {1}", (object) trend.patriatismCount, (object) Environment.NewLine);
        }
        EmbedBuilder builder = new EmbedBuilder()
        {
          Author = new EmbedAuthorBuilder()
        };
        builder.Author.Name = DateTime.Today.DayOfWeek.ToString() + "'s gold trends for " + this.Context.Guild.Name;
        builder.Description = together;
        builder.WithColor(Color.Gold);
        IUserMessage userMessage = await this.ReplyAsync("", embed: builder.Build());
        together = (string) null;
        builder = (EmbedBuilder) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        IUserMessage userMessage = await this.ReplyAsync("GDP calculation error, make sure you run this command in a server that allows GDP");
      }
    }

    [Command("gdp")]
    public async Task GDP()
    {
      try
      {
        if (this.trendData.Count == 0)
          this.trendData = this.LoadTrendData();
        Decimal GDP_YESTERDAY = 0M;
        Decimal GDP_TODAY = 0M;
        foreach (_2b2tpay.Moduels.Commands.TrendData trend in this.trendData)
        {
          DateTime date1 = trend.date;
          DateTime dateTime = DateTime.Today;
          DateTime date2 = dateTime.Date;
          if (date1 == date2 && trend.server == this.Context.Guild.Name)
            GDP_TODAY = trend.gainGold - trend.looseGold;
          DateTime date3 = trend.date;
          dateTime = DateTime.Today;
          dateTime = dateTime.AddDays(-1.0);
          DateTime date4 = dateTime.Date;
          if (date3 == date4 && trend.server == this.Context.Guild.Name)
            GDP_YESTERDAY = trend.gainGold - trend.looseGold;
        }
        Decimal percentageGroth = (GDP_TODAY - GDP_YESTERDAY) / 2M * 100M;
        EmbedBuilder builder = new EmbedBuilder();
        builder.Author = new EmbedAuthorBuilder();
        if (percentageGroth < 0M)
        {
          builder.Author.Name = string.Format("Decreased by {0}%", (object) Math.Round(percentageGroth, 2));
          builder.Description = "The imperial gold market isn't looking good today";
          builder.WithColor(Color.DarkerGrey);
        }
        else
        {
          builder.Author.Name = string.Format("Increased by {0}%", (object) Math.Round(percentageGroth, 2));
          builder.Description = "Nice! The market is going up!";
          builder.WithColor(Color.Gold);
        }
        IUserMessage userMessage = await this.ReplyAsync("", embed: builder.Build());
        builder = (EmbedBuilder) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        IUserMessage userMessage = await this.ReplyAsync("GDP calculation error, make sure you run this command in a server that allows GDP");
      }
    }

    public void AddTrendData(_2b2tpay.Moduels.Commands.TrendData dataToAdd, DateTime date, string server)
    {
      try
      {
        if (this.trendData.Count == 0)
          this.trendData = this.LoadTrendData();
        if (this.trendData == null)
          this.trendData = new List<_2b2tpay.Moduels.Commands.TrendData>();
        int index = 0;
        try
        {
          foreach (_2b2tpay.Moduels.Commands.TrendData trendData in this.trendData)
          {
            if (trendData.server == this.Context.Guild.Name && trendData.date.Date == DateTime.Today)
            {
              this.trendData[index] = new _2b2tpay.Moduels.Commands.TrendData()
              {
                gainGold = dataToAdd.gainGold + this.trendData[index].gainGold,
                looseGold = dataToAdd.looseGold + this.trendData[index].looseGold,
                transactionGold = dataToAdd.transactionGold + this.trendData[index].transactionGold,
                timesCheckedBal = dataToAdd.timesCheckedBal + this.trendData[index].timesCheckedBal,
                patriatismCount = dataToAdd.patriatismCount + this.trendData[index].patriatismCount,
                timesRankedUp = dataToAdd.timesRankedUp + this.trendData[index].timesRankedUp,
                timesMadePrivate = dataToAdd.timesMadePrivate + this.trendData[index].timesMadePrivate,
                timesMadePublic = dataToAdd.timesMadePublic + this.trendData[index].timesMadePublic,
                server = this.Context.Guild.Name,
                date = DateTime.Today
              };
              this.SaveTrendData();
              return;
            }
            ++index;
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("ERROR IN FOREACH LOOP IN ADD TREND: " + Environment.NewLine + ex.Message);
        }
        this.trendData.Add(new _2b2tpay.Moduels.Commands.TrendData()
        {
          gainGold = dataToAdd.gainGold,
          looseGold = dataToAdd.looseGold,
          transactionGold = dataToAdd.transactionGold,
          timesCheckedBal = dataToAdd.timesCheckedBal,
          patriatismCount = dataToAdd.patriatismCount,
          timesRankedUp = dataToAdd.timesRankedUp,
          server = this.Context.Guild.Name,
          date = DateTime.Today
        });
        this.SaveTrendData();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    public void SaveTrendData()
    {
      using (StreamWriter streamWriter = new StreamWriter("trends/trends.txt"))
        streamWriter.WriteLine(JsonConvert.SerializeObject((object) this.trendData));
      List<_2b2tpay.Moduels.Commands.TrendData> trendDataList = new List<_2b2tpay.Moduels.Commands.TrendData>();
      using (StreamReader streamReader = new StreamReader("trends/trends.txt"))
      {
        trendDataList = JsonConvert.DeserializeObject<List<_2b2tpay.Moduels.Commands.TrendData>>(streamReader.ReadToEnd());
        streamReader.Close();
      }
    }

    public List<_2b2tpay.Moduels.Commands.TrendData> LoadTrendData()
    {
      List<_2b2tpay.Moduels.Commands.TrendData> trendDataList = new List<_2b2tpay.Moduels.Commands.TrendData>();
      using (StreamReader streamReader = new StreamReader("trends/trends.txt"))
      {
        trendDataList = JsonConvert.DeserializeObject<List<_2b2tpay.Moduels.Commands.TrendData>>(streamReader.ReadToEnd());
        streamReader.Close();
      }
      return trendDataList;
    }

    [Command("trends")]
    public async Task Trends(string date)
    {
      try
      {
        DateTime dateTimeTo;
        if (!DateTime.TryParse(date, out dateTimeTo))
        {
          IUserMessage userMessage = await this.ReplyAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("Not a real date...", "Please enter a valid date."));
          return;
        }
        if (dateTimeTo.Date > DateTime.Today)
        {
          IUserMessage userMessage = await this.ReplyAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("This bot is not a fortune teller!", "Sorry but Imperial gold bot can't tell you the future! Wait... Maybe we can? Hmmmm, next update maybe?"));
          return;
        }
        if (this.trendData.Count == 0)
          this.trendData = this.LoadTrendData();
        string together = "";
        foreach (_2b2tpay.Moduels.Commands.TrendData trend in this.trendData)
        {
          if (trend.date == dateTimeTo && trend.server == this.Context.Guild.Name)
            together = string.Format("GDP today: {0} {1}", (object) trend.gainGold, (object) Environment.NewLine) + string.Format("GDL today: {0} {1}", (object) trend.looseGold, (object) Environment.NewLine) + string.Format("Gold Traded Today: {0} {1}", (object) trend.transactionGold, (object) Environment.NewLine) + "-= Other Statistics =-" + Environment.NewLine + string.Format("Ballance Checks Today: {0} {1}", (object) trend.timesCheckedBal, (object) Environment.NewLine) + string.Format("Times Ranked Up Today: {0} {1}", (object) trend.timesRankedUp, (object) Environment.NewLine) + string.Format("Number of accounts made public today: {0} {1}", (object) trend.timesMadePublic, (object) Environment.NewLine) + string.Format("Number of accounts made private today: {0} {1}", (object) trend.timesMadePrivate, (object) Environment.NewLine) + string.Format("Imperial Patriotism Count: {0} {1}", (object) trend.patriatismCount, (object) Environment.NewLine);
        }
        EmbedBuilder builder = new EmbedBuilder()
        {
          Author = new EmbedAuthorBuilder()
        };
        builder.Author.Name = dateTimeTo.Date.DayOfWeek.ToString() + "'s gold trends for " + this.Context.Guild.Name;
        builder.Description = together;
        builder.WithColor(Color.Gold);
        IUserMessage userMessage1 = await this.ReplyAsync("", embed: builder.Build());
        together = (string) null;
        builder = (EmbedBuilder) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        IUserMessage userMessage = await this.ReplyAsync("GDP calculation error, make sure you run this command in a server that allows GDP");
      }
    }

    [Command("compare")]
    public async Task Compare(string date1, string date2)
    {
      try
      {
        DateTime dateTimeTo1;
        if (!DateTime.TryParse(date1, out dateTimeTo1))
        {
          IUserMessage userMessage = await this.ReplyAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("Not a real date...", "Please enter a valid date."));
          return;
        }
        DateTime dateTimeTo2;
        if (!DateTime.TryParse(date1, out dateTimeTo2))
        {
          IUserMessage userMessage = await this.ReplyAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("Not a real date...", "Please enter a valid date."));
          return;
        }
        if (dateTimeTo1.Date > DateTime.Today || dateTimeTo2.Date > DateTime.Today)
        {
          IUserMessage userMessage = await this.ReplyAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("This bot is not a fortune teller!", "Sorry but Imperial gold bot can't tell you the future! Wait... Maybe we can? Hmmmm, next update maybe?"));
          return;
        }
        if (this.trendData.Count == 0)
          this.trendData = this.LoadTrendData();
        Decimal one = 0M;
        Decimal two = 0M;
        foreach (_2b2tpay.Moduels.Commands.TrendData trend in this.trendData)
        {
          if (trend.date == dateTimeTo1 && trend.server == this.Context.Guild.Name)
            one = trend.gainGold;
          else if (trend.date == dateTimeTo2 && trend.server == this.Context.Guild.Name)
            two = trend.gainGold;
        }
        Decimal percentageGroth = (one - two) / 2M * 100M;
        EmbedBuilder builder = new EmbedBuilder();
        builder.Author = new EmbedAuthorBuilder();
        if (percentageGroth < 0M)
        {
          builder.Author.Name = string.Format("Decreased by {0}%", (object) Math.Round(percentageGroth, 2));
          builder.Description = "The imperial gold market isn't looking good today";
          builder.WithColor(Color.DarkerGrey);
        }
        else
        {
          builder.Author.Name = string.Format("Increased by {0}%", (object) Math.Round(percentageGroth, 2));
          builder.Description = "Nice! The market is going up!";
          builder.WithColor(Color.Gold);
        }
        IUserMessage userMessage1 = await this.ReplyAsync("", embed: builder.Build());
        builder = (EmbedBuilder) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        IUserMessage userMessage = await this.ReplyAsync("GDP calculation error, make sure you run this command in a server that allows GDP");
      }
    }

    [Command("advertisement", RunMode = RunMode.Async)]
    public async Task Advertisemet([Remainder] string advertisement)
    {
      try
      {
        int cost = 100;
        account result = new account();
        using (StreamReader file = new StreamReader("accounts/" + this.Context.User.Id.ToString()))
        {
          result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
          file.Close();
          if (result.ballance < 1M)
          {
            RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("You don't have enough gold to purchace this advertisement!", "You need " + ((Decimal) cost - result.ballance).ToString() + " more gold to purchace this..."), (RequestOptions) null);
            return;
          }
          result.ballance -= (Decimal) cost;
          bool foundIt = false;
          foreach (SocketTextChannel socketChannel in (IEnumerable<SocketTextChannel>) this.Context.Guild.TextChannels)
          {
            if (socketChannel.Name == "imperial-gold-advertisements")
            {
              RestUserMessage restUserMessage = await socketChannel.SendMessageAsync(advertisement, false, (Embed) null, (RequestOptions) null);
              foundIt = true;
            }
          }
          if (!foundIt)
          {
            IUserMessage userMessage = await this.ReplyAsync("Sorry but we could not find a channle to put your advertisement in... Tell an administator that there is a problem...");
            return;
          }
          result.accountLogs.Add(new accountLog("Bought advertisement for " + cost.ToString() + " gold"));
          using (StreamWriter accountFile = new StreamWriter("accounts/" + this.Context.User.Id.ToString()))
            accountFile.WriteLine(JsonConvert.SerializeObject((object) result));
          RestUserMessage restUserMessage1 = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Advertisement bought!"), (RequestOptions) null);
        }
        result = new account();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        IUserMessage userMessage = await this.ReplyAsync("Advertisement Error. Check with Centurion or Optio");
      }
    }

    [Command("branchtop today")]
    public async Task BalTopBranches()
    {
      List<_2b2tpay.Moduels.Commands.TrendData> trendDatas = new List<_2b2tpay.Moduels.Commands.TrendData>();
      if (this.trendData.Count == 0)
        this.trendData = this.LoadTrendData();
      Console.WriteLine(this.trendData.Count);
      foreach (_2b2tpay.Moduels.Commands.TrendData trend in this.trendData)
      {
        DateTime date1 = trend.date.Date;
        DateTime today = DateTime.Today;
        DateTime date2 = today.Date;
        if (date1 == date2)
        {
          trendDatas.Add(trend);
          Console.WriteLine(trend.server);
        }
        string server = trend.server;
        // ISSUE: variable of a boxed type
        DateTime date3 = trend.date;
        today = DateTime.Today;
        // ISSUE: variable of a boxed type
        DateTime date4 = today.Date;
        Console.WriteLine(string.Format("{0} | {1} = {2}", (object) server, (object) date3, (object) date4));
      }
      try
      {
        string ToSay = "";
        Console.WriteLine("YES");
        trendDatas = trendDatas.OrderByDescending<_2b2tpay.Moduels.Commands.TrendData, Decimal>((Func<_2b2tpay.Moduels.Commands.TrendData, Decimal>) (xz => xz.gainGold - xz.looseGold)).ToList<_2b2tpay.Moduels.Commands.TrendData>();
        int i = 0;
        foreach (_2b2tpay.Moduels.Commands.TrendData tt in trendDatas)
        {
          try
          {
            if (i < 10)
            {
              ++i;
              ToSay = ToSay + "[" + i.ToString() + "] " + tt.server + " | " + string.Format("{0:0.00}", (object) (tt.gainGold - tt.looseGold)) + " gold" + Environment.NewLine;
            }
            else
              break;
          }
          catch (Exception ex)
          {
            Exception e = ex;
            ToSay = ToSay + "[" + i.ToString() + "] " + tt.server + " | " + (tt.gainGold - tt.looseGold).ToString() + " gold" + Environment.NewLine;
          }
        }
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Imperial Empire Branch List", ToSay, true), (RequestOptions) null);
        ToSay = (string) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine("2nd");
        Console.WriteLine(ex.ToString());
        Console.WriteLine(Environment.NewLine + ex.Message);
      }
    }

    [Command("branchtop")]
    public async Task BranchTop()
    {
      RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Wrong Options!", "!branchtop today " + Environment.NewLine + " !branchtop all time", true), (RequestOptions) null);
    }

    [Command("branchtop all time")]
    public async Task BalTopBranchesAllTime()
    {
      List<_2b2tpay.Moduels.Commands.TrendData> trendDatas = new List<_2b2tpay.Moduels.Commands.TrendData>();
      if (this.trendData.Count == 0)
        this.trendData = this.LoadTrendData();
      Console.WriteLine(this.trendData.Count);
      foreach (_2b2tpay.Moduels.Commands.TrendData trend in this.trendData)
      {
        bool couldnt_find = true;
        int counter = 0;
        foreach (_2b2tpay.Moduels.Commands.TrendData trendish in trendDatas)
        {
          if (trendish.server == trend.server)
          {
            trendDatas[counter].gainGold += trend.gainGold;
            trendDatas[counter].looseGold += trend.looseGold;
            couldnt_find = false;
          }
          ++counter;
        }
        if (couldnt_find)
          trendDatas.Add(trend);
        Console.WriteLine(trend.server);
        Console.WriteLine(string.Format("{0} | {1} = {2}", (object) trend.server, (object) trend.date, (object) DateTime.Today.Date));
      }
      try
      {
        string ToSay = "";
        Console.WriteLine("YES");
        trendDatas = trendDatas.OrderByDescending<_2b2tpay.Moduels.Commands.TrendData, Decimal>((Func<_2b2tpay.Moduels.Commands.TrendData, Decimal>) (xz => xz.gainGold)).ToList<_2b2tpay.Moduels.Commands.TrendData>();
        int i = 0;
        foreach (_2b2tpay.Moduels.Commands.TrendData tt in trendDatas)
        {
          try
          {
            if (i < 10)
            {
              ++i;
              ToSay = ToSay + "[" + i.ToString() + "] " + tt.server + " | " + string.Format("{0:0.00}", (object) tt.gainGold) + " gold" + Environment.NewLine;
            }
            else
              break;
          }
          catch (Exception ex)
          {
            Exception e = ex;
            ToSay = ToSay + "[" + i.ToString() + "] " + tt.server + " | " + tt.gainGold.ToString() + " gold" + Environment.NewLine;
          }
        }
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Imperial Empire Branch List", ToSay, true), (RequestOptions) null);
        ToSay = (string) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine("2nd");
        Console.WriteLine(ex.ToString());
        Console.WriteLine(Environment.NewLine + ex.Message);
      }
    }

    [Command("citizens")]
    public async Task Citizens()
    {
      List<ulong> usersCounted = new List<ulong>();
      List<string> rolesAcceptable = new List<string>()
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
      foreach (SocketGuild socketGuild in (IEnumerable<SocketGuild>) this.Context.Client.Guilds)
      {
        foreach (SocketGuildUser user in (IEnumerable<SocketGuildUser>) socketGuild.Users)
        {
          foreach (string str in rolesAcceptable)
          {
            string role = str;
            if (((IEnumerable<IRole>) user.Roles).Contains<IRole>((this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.ToLower().Contains(role.ToLower())))))
            {
              Console.WriteLine(socketGuild.Name + " | " + user.Username);
              bool found = false;
              foreach (ulong id in usersCounted)
              {
                if ((long) id == (long) user.Id)
                {
                  found = true;
                  break;
                }
              }
              if (!found)
                usersCounted.Add(user.Id);
            }
          }
        }
      }
      IUserMessage userMessage = await this.ReplyAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("Citizen Count", usersCounted.Count.ToString()));
    }

    [Command("market top")]
    public async Task MarketTop()
    {
      List<_2b2tpay.Moduels.Commands.TrendData> trendDatas = new List<_2b2tpay.Moduels.Commands.TrendData>();
      if (this.trendData.Count == 0)
        this.trendData = this.LoadTrendData();
      Console.WriteLine(this.trendData.Count);
      foreach (_2b2tpay.Moduels.Commands.TrendData trend in this.trendData)
      {
        bool couldnt_find = true;
        int counter = 0;
        foreach (_2b2tpay.Moduels.Commands.TrendData trendish in trendDatas)
        {
          if (trendish.server == trend.server)
          {
            trendDatas[counter].transactionGold += trend.transactionGold;
            couldnt_find = false;
          }
          ++counter;
        }
        if (couldnt_find)
          trendDatas.Add(trend);
        Console.WriteLine(trend.server);
        Console.WriteLine(string.Format("{0} | {1} = {2}", (object) trend.server, (object) trend.date, (object) DateTime.Today.Date));
      }
      try
      {
        string ToSay = "";
        trendDatas = trendDatas.OrderByDescending<_2b2tpay.Moduels.Commands.TrendData, Decimal>((Func<_2b2tpay.Moduels.Commands.TrendData, Decimal>) (xz => xz.transactionGold)).ToList<_2b2tpay.Moduels.Commands.TrendData>();
        int i = 0;
        foreach (_2b2tpay.Moduels.Commands.TrendData tt in trendDatas)
        {
          try
          {
            if (i < 10)
            {
              ++i;
              ToSay = ToSay + "[" + i.ToString() + "] " + tt.server + " | " + string.Format("{0:0.00}", (object) tt.transactionGold) + " gold" + Environment.NewLine;
            }
            else
              break;
          }
          catch (Exception ex)
          {
            Exception e = ex;
            ToSay = ToSay + "[" + i.ToString() + "] " + tt.server + " | " + tt.transactionGold.ToString() + " gold" + Environment.NewLine;
          }
        }
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Imperial Empire Branch Trade Market (ALL TIME)", ToSay, true), (RequestOptions) null);
        ToSay = (string) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine("2nd");
        Console.WriteLine(ex.ToString());
        Console.WriteLine(Environment.NewLine + ex.Message);
      }
    }

    [Command("print bank growth")]
    public async Task PrintGrowth()
    {
      string data = "";
      _2b2tpay.Moduels.Commands.TrendData lastDay = new _2b2tpay.Moduels.Commands.TrendData()
      {
        server = ""
      };
      if (this.trendData.Count == 0)
        this.trendData = this.LoadTrendData();
      foreach (_2b2tpay.Moduels.Commands.TrendData trend in this.trendData)
      {
        if (trend.server == this.Context.Guild.Name)
        {
          if (lastDay.server == this.Context.Guild.Name)
          {
            if (trend.gainGold <= 0M)
              trend.gainGold = 0.01M;
            else if (lastDay.gainGold <= 0M)
              lastDay.gainGold = 0.01M;
            data += string.Format("{0} | {1}              | Prev: {2} gold | Now: {3} {4}", (object) trend.date, (object) this.DoubleToPercentageString(this.CalculateChange(lastDay.gainGold, trend.gainGold)), (object) lastDay.gainGold, (object) trend.gainGold, (object) Environment.NewLine);
          }
          lastDay = trend;
        }
      }
      File.WriteAllText("data/bankGrowth.txt", data);
      RestUserMessage restUserMessage = await this.Context.Channel.SendFileAsync("data/bankGrowth.txt", "", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Gain Gold / Bank Percentage Data"), (RequestOptions) null, false);
    }

    private Decimal CalculateChange(Decimal previous, Decimal current)
    {
      if (previous == 0M)
        throw new InvalidOperationException();
      return (current - previous) / previous;
    }

    private string DoubleToPercentageString(Decimal d) => (Math.Round(d, 2) * 100M).ToString() + "%";

    [Command("business create", RunMode = RunMode.Async)]
    public async Task BusinessCreate()
    {
      await this.SendMessage("Type 'back' to go to the last question, type 'cancel' to cancel.");
      Business business = new Business();
      SocketMessage response992;
      do
      {
        await this.SendMessage("Business Name?");
        SocketMessage response99 = await this.NextMessageAsync(timeout: new TimeSpan?(TimeSpan.FromSeconds(3600.0)));
        if (!(response99.Content.ToLower().Trim() == "back") && !(response99.Content.ToLower().Trim() == "cancel"))
        {
          business.name = response99.Content;
          await this.SendMessage("Business Description?");
          response992 = await this.NextMessageAsync(timeout: new TimeSpan?(TimeSpan.FromSeconds(3600.0)));
        }
        else
          goto label_11;
      }
      while (response992.Content.ToLower().Trim() == "back");
      goto label_8;
label_11:
      return;
label_8:
      if (response992.Content.ToLower().Trim() == "cancel")
        return;
      business.description = response992.Content;
      while (true)
      {
        await this.SendMessage("Are you sure you want to create a buiness and pay the 20 gold starting fee? (yes/no)");
        SocketMessage response9923 = await this.NextMessageAsync(timeout: new TimeSpan?(TimeSpan.FromSeconds(3600.0)));
        if (!(response9923.Content.ToLower().Trim() == "yes"))
        {
          if (!(response9923.Content.ToLower().Trim() == "no"))
            await this.SendMessage("'yes' or 'no' please.");
          else
            goto label_18;
        }
        else
          break;
      }
      BusinessMain.CreateBuiness(business.name, business.description, this.Context.User.Id);
      await this.SendMessage("Created your buiness!");
      return;
label_18:
      await this.SendMessage("Canceled!");
    }

    public async Task SendMessage(string message)
    {
      IUserMessage userMessage = await this.Context.User.SendMessageAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage(message));
    }

    [Command("it")]
    public async Task ImpTime(SocketUser user = null)
    {
      if (user == null)
        user = this.Context.User;
      try
      {
        SocketUser userInfo = this.Context.User;
        account result = new account();
        result = JsonConvert.DeserializeObject<account>(File.ReadAllText("accounts/" + user.Id.ToString()));
        DateTime startDate = result.accountLogs[0].timeOfLog.Date;
        DateTime endDate = DateTime.Today.Date;
        double totalDays = (endDate - startDate).TotalDays;
        double totalYears = Math.Truncate(totalDays / 365.0);
        double totalMonths = Math.Truncate(totalDays % 365.0 / 30.0);
        double remainingDays = Math.Truncate(totalDays % 365.0 % 30.0);
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync(string.Format("Estimated duration is {0} year(s), {1} month(s) and {2} day(s)", (object) totalYears, (object) totalMonths, (object) remainingDays), false, (Embed) null, (RequestOptions) null);
        userInfo = (SocketUser) null;
        result = new account();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message + Environment.NewLine + " () -- " + ex.ToString());
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("Sorry, something has gone wrong when trying to ascess your time. If you desperatly need your logs please contact an admin", false, (Embed) null, (RequestOptions) null);
      }
    }

    [Command("accounts")]
    public async Task GoldShip()
    {
      try
      {
        List<account> accounts = new List<account>();
        string[] accountFiles = Directory.GetFiles("accounts/");
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("Current Imperial Gold accounts = " + accountFiles.Length.ToString(), false, (Embed) null, (RequestOptions) null);
        accounts = (List<account>) null;
        accountFiles = (string[]) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message + Environment.NewLine + " () -- " + ex.ToString());
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("Sorry, something has gone wrong.", false, (Embed) null, (RequestOptions) null);
      }
    }

    [Command("membership")]
    public async Task membership()
    {
      try
      {
        List<SocketGuildUser> members = new List<SocketGuildUser>();
        List<SocketGuildUser> members_branch = new List<SocketGuildUser>();
        foreach (SocketGuild socketGuild in (IEnumerable<SocketGuild>) this.Context.Client.Guilds)
        {
          Console.WriteLine(socketGuild.Name);
          Thread.Sleep(1000);
          foreach (SocketGuildUser user in (IEnumerable<SocketGuildUser>) socketGuild.Users)
          {
            SocketGuildUser socketUser = user;
            if (!socketUser.IsBot && !members.Contains(socketUser))
            {
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
              string[] strArray = citizenRoles;
              for (int index = 0; index < strArray.Length; ++index)
              {
                string role_ = strArray[index];
                IRole role = ((IGuildUser) socketUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Trim().ToLower().Contains(role_.ToLower())));
                if (((IEnumerable<IRole>) socketUser.Roles).Contains<IRole>(role))
                {
                  members_branch.Add(socketUser);
                  members.Add(socketUser);
                  Console.WriteLine(socketUser.Username + " | " + role_ + " | " + socketGuild.Name);
                  break;
                }
                role = (IRole) null;
              }
              strArray = (string[]) null;
              citizenRoles = (string[]) null;
              socketUser = (SocketGuildUser) null;
            }
          }
          RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("Current Imperial Citizens in " + socketGuild.Name + " = " + members_branch.Count.ToString(), false, (Embed) null, (RequestOptions) null);
          members_branch.Clear();
        }
        RestUserMessage restUserMessage1 = await this.Context.Channel.SendMessageAsync("Current Imperial Citizens = " + members.Count.ToString(), false, (Embed) null, (RequestOptions) null);
        members = (List<SocketGuildUser>) null;
        members_branch = (List<SocketGuildUser>) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message + Environment.NewLine + " () -- " + ex.ToString());
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("Sorry, something has gone wrong.", false, (Embed) null, (RequestOptions) null);
      }
    }

    [Command("setvar")]
    public async Task SetVar(string name, string data)
    {
      PaymentVars.UpdateVariable(name, data);
      IUserMessage userMessage = await this.ReplyAsync("Set variable " + name + " == " + data);
    }

    [Command("listvars")]
    public async Task ListVars()
    {
      string together = "```";
      foreach (KeyValuePair<string, string> variable in PaymentVars.variables)
      {
        KeyValuePair<string, string> k = variable;
        together = together + k.Key + " | " + k.Value + Environment.NewLine;
        k = new KeyValuePair<string, string>();
      }
      together += "```";
      IUserMessage userMessage = await this.ReplyAsync(together);
    }

    [Command("setpayment")]
    public async Task SetPayment(string name, Decimal payRate)
    {
      JobVars.UpdateVariable(name, payRate);
      IUserMessage userMessage = await this.ReplyAsync(string.Format("Set variable {0} == {1}", (object) name, (object) payRate));
    }

    [Command("listpayments")]
    public async Task ListPayments()
    {
      string together = "```";
      foreach (Job.JobType job in Job.jobs)
      {
        Job.JobType j = job;
        together = together + j.name + " | " + j.payRatePerWeek.ToString() + Environment.NewLine;
        j = new Job.JobType();
      }
      together += "```";
      IUserMessage userMessage = await this.ReplyAsync(together);
    }

    [Command("real baltop")]
    public async Task RealBalTop()
    {
      List<account> accounts = new List<account>();
      string[] accountFiles = Directory.GetFiles("accounts/");
      try
      {
        string[] strArray = accountFiles;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string accountFile = strArray[index];
          account result = new account();
          using (StreamReader file = new StreamReader(accountFile))
          {
            result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
            file.Close();
            accounts.Add(result);
          }
          result = new account();
          accountFile = (string) null;
        }
        strArray = (string[]) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine("1st");
        Console.WriteLine(ex.ToString());
      }
      try
      {
        string ToSay = "";
        Console.WriteLine("YES");
        accounts = _2b2tpay.Data.BalTop.RealSortAccounts(accounts);
        int i = 0;
        foreach (account account1 in accounts)
        {
          account account = account1;
          try
          {
            if (i < 10)
            {
              ++i;
              ToSay = ToSay + "[" + i.ToString() + "] " + this.Context.Guild.GetUser(account.accountUserId).Username + " | " + string.Format("{0:0.00}", (object) account.ballance) + " gold" + Environment.NewLine;
            }
            else
              break;
          }
          catch (Exception ex)
          {
            Exception e = ex;
            ToSay = ToSay + "[" + i.ToString() + "] " + account.name + " | " + account.ballance.ToString() + " gold" + Environment.NewLine;
          }
          account = new account();
        }
        RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("REAL (UN-HIDE) Imperial Forbes List", ToSay, true), (RequestOptions) null);
        ToSay = (string) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine("2nd");
        Console.WriteLine(ex.ToString());
        Console.WriteLine(Environment.NewLine + ex.Message);
      }
    }

    [Command("removeid_bal")]
    public async Task removeid_bal(string user, string gold = "X")
    {
      if ((long) this.Context.Guild.Id != (long) this.SENATE_GUILD)
        return;
      try
      {
        IRole role = (this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name == "Imperial Gold Manager"));
        if (!((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>(role))
        {
          IUserMessage userMessage1 = await this.ReplyAsync("Plebs do not have permission to preform this command!");
        }
        else
        {
          try
          {
            if (gold.Contains("$"))
            {
              gold = gold.TrimStart('$');
              gold = gold.TrimEnd('$');
            }
            if (user == null || gold == "X")
            {
              if (user == null)
              {
                IUserMessage userMessage2 = await this.ReplyAsync("A user ID must be mentioned.");
                return;
              }
              IUserMessage userMessage3 = await this.ReplyAsync("Must be 0.01 gold or above and in numeric form");
              return;
            }
            Decimal ammountToRemove = 0M;
            if (!Decimal.TryParse(gold, out ammountToRemove))
            {
              IUserMessage userMessage4 = await this.ReplyAsync("Value of gold must be a number.");
              return;
            }
            if (user == null)
            {
              IUserMessage userMessage5 = await this.ReplyAsync("You need to mention someone when removing gold!! | !adminremove @user x");
            }
            else
            {
              IUserMessage userMessage6 = await this.ReplyAsync("Removing " + user + gold + " gold...");
              bool hasAnAccount = true;
              if (!File.Exists("accounts/" + user.ToString()))
              {
                hasAnAccount = false;
                Random random = new Random();
                account account = new account()
                {
                  name = user,
                  accountUserId = ulong.Parse(user),
                  accountId = random.Next(0, 99999999),
                  ballance = 0M,
                  accountLogs = new List<accountLog>()
                };
                account.accountLogs.Add(new accountLog("Created account"));
                account.accountLogs.Add(new accountLog("Credit: 0 gold"));
                account.transactions = new List<accountLog>();
                account.transactions.Add(new accountLog("Credit: 0 gold"));
                account.isPublic = true;
                using (StreamWriter accountFile = new StreamWriter("accounts/" + account.accountUserId.ToString()))
                  accountFile.WriteLine(JsonConvert.SerializeObject((object) account));
                random = (Random) null;
                account = new account();
              }
              account result2 = new account();
              using (StreamReader file = new StreamReader("accounts/" + user.ToString()))
              {
                result2 = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
                file.Close();
                result2.ballance -= ammountToRemove;
                if (result2.ballance < 0M)
                  result2.ballance = 0M;
                result2.accountLogs.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
                result2.transactions.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
                using (StreamWriter accountFile = new StreamWriter("accounts/" + user.ToString()))
                  accountFile.WriteLine(JsonConvert.SerializeObject((object) result2));
              }
              Thread.Sleep(1500);
              RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Removed " + user + " " + gold + " gold", "If " + user + " has not had the gold removed. Your need to contact an admin."), (RequestOptions) null);
              this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
              {
                looseGold = ammountToRemove
              }, DateTime.Now, this.Context.Guild.Name);
              result2 = new account();
            }
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
            if (ex.Message.ToLower().Contains("user not found"))
            {
              IUserMessage userMessage7 = await this.ReplyAsync("You to need to mention a real user");
              return;
            }
            IUserMessage userMessage8 = await this.ReplyAsync("Sorry, something went wrong when doing the transaction.");
          }
        }
        role = (IRole) null;
      }
      catch (Exception ex)
      {
        IUserMessage userMessage = await this.ReplyAsync("ERROR!! " + ex.ToString());
      }
    }

    [Command("removeuser_bal")]
    public async Task removeuser_bal(string username, string gold = "X")
    {
      if ((long) this.Context.Guild.Id != (long) this.SENATE_GUILD)
        return;
      IRole role = (this.Context.User as IGuildUser).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name == "Imperial Gold Manager"));
      if (!((IEnumerable<IRole>) (this.Context.User as SocketGuildUser).Roles).Contains<IRole>(role))
      {
        IUserMessage userMessage1 = await this.ReplyAsync("Plebs do not have permission to preform this command!");
      }
      else
      {
        try
        {
          if (gold.Contains("$"))
          {
            gold = gold.TrimStart('$');
            gold = gold.TrimEnd('$');
          }
          if (username == "" || gold == "X")
          {
            if (username == "")
            {
              IUserMessage userMessage2 = await this.ReplyAsync("A user must be mentioned.");
              return;
            }
            IUserMessage userMessage3 = await this.ReplyAsync("Must be 0.01 gold or above and in numeric form");
            return;
          }
          Decimal ammountToRemove = 0M;
          if (!Decimal.TryParse(gold, out ammountToRemove))
          {
            IUserMessage userMessage4 = await this.ReplyAsync("Value of gold must be a number.");
            return;
          }
          if (username == "")
          {
            IUserMessage userMessage5 = await this.ReplyAsync("You need to mention someone when removing gold!! | !adminremove @user x");
          }
          else
          {
            IUserMessage userMessage6 = await this.ReplyAsync("trying Removing " + username + gold + " gold...");
            bool hasAnAccount = true;
            account THEACCOUNT = new account();
            DirectoryInfo d = new DirectoryInfo("accounts/");
            FileInfo[] Files = d.GetFiles();
            FileInfo[] fileInfoArray = Files;
            for (int index = 0; index < fileInfoArray.Length; ++index)
            {
              FileInfo file = fileInfoArray[index];
              account result22 = new account();
              result22 = JsonConvert.DeserializeObject<account>(File.ReadAllText(file.FullName));
              try
              {
                if (result22.name.ToLower() == username.ToLower())
                  THEACCOUNT = result22;
              }
              catch
              {
                Console.WriteLine("SKIPPED ON ADMIN USER REMOVE | " + result22.accountUserId.ToString());
                continue;
              }
              result22 = new account();
              file = (FileInfo) null;
            }
            fileInfoArray = (FileInfo[]) null;
            account result2 = new account();
            using (StreamReader file = new StreamReader("accounts/" + THEACCOUNT.accountUserId.ToString()))
            {
              result2 = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
              file.Close();
              result2.ballance -= ammountToRemove;
              if (result2.ballance < 0M)
                result2.ballance = 0M;
              result2.accountLogs.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
              result2.transactions.Add(new accountLog("Removed by Admin " + gold + " gold from the Bank of Imperial"));
              using (StreamWriter accountFile = new StreamWriter("accounts/" + THEACCOUNT.accountUserId.ToString()))
                accountFile.WriteLine(JsonConvert.SerializeObject((object) result2));
            }
            Thread.Sleep(1500);
            RestUserMessage restUserMessage = await this.Context.Channel.SendMessageAsync("", false, _2b2tpay.Moduels.Commands.SendEmmbedMessage("Removed " + THEACCOUNT.name + " " + gold + " gold", "If " + THEACCOUNT.name + " has not had the gold removed. Your need to contact an admin."), (RequestOptions) null);
            this.AddTrendData(new _2b2tpay.Moduels.Commands.TrendData()
            {
              looseGold = ammountToRemove
            }, DateTime.Now, this.Context.Guild.Name);
            THEACCOUNT = new account();
            d = (DirectoryInfo) null;
            Files = (FileInfo[]) null;
            result2 = new account();
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
          if (ex.Message.ToLower().Contains("user not found"))
          {
            IUserMessage userMessage7 = await this.ReplyAsync("You to need to mention a real user");
            return;
          }
          IUserMessage userMessage8 = await this.ReplyAsync(ex.ToString());
        }
      }
    }

    [Command("joinbranch")]
    public async Task removeuser_bal(string name)
    {
      foreach (SocketGuild g in (IEnumerable<SocketGuild>) this.Context.Client.Guilds)
      {
        if (g.Name.ToLower().Trim() == name.ToLower().Trim())
        {
          IReadOnlyCollection<RestInviteMetadata> invites = await g.GetInvitesAsync((RequestOptions) null);
          IUserMessage userMessage = await this.ReplyAsync(invites.Select<RestInviteMetadata, string>((Func<RestInviteMetadata, string>) (x => x.Url)).FirstOrDefault<string>());
          invites = (IReadOnlyCollection<RestInviteMetadata>) null;
        }
      }
    }

    [Command("list branches")]
    public async Task listbranches()
    {
      string together = "```";
      foreach (SocketGuild g in (IEnumerable<SocketGuild>) this.Context.Client.Guilds)
        together = together + g.Name + Environment.NewLine;
      together += "```";
      IUserMessage userMessage = await this.ReplyAsync(together);
    }

    [Command("imperi")]
    public async Task IMPERIIMPERI()
    {
      Decimal TOTALCIRCULATION = 0M;
      string[] accountFiles = Directory.GetFiles("accounts/");
      try
      {
        string[] strArray = accountFiles;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string accountFile = strArray[index];
          account result = new account();
          using (StreamReader file = new StreamReader(accountFile))
          {
            result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
            file.Close();
            TOTALCIRCULATION += result.ballance;
          }
          result = new account();
          accountFile = (string) null;
        }
        strArray = (string[]) null;
        IUserMessage userMessage = await this.ReplyAsync("there is a total of '" + TOTALCIRCULATION.ToString() + "' gold in circulation.");
      }
      catch (Exception ex)
      {
        Console.WriteLine("1st");
        Console.WriteLine(ex.ToString());
      }
    }

    [Command("imperi count")]
    public async Task IMPERIIMPERI_COUNT()
    {
      Decimal TOTALACCOUNT = 0M;
      string[] accountFiles = Directory.GetFiles("accounts/");
      try
      {
        string[] strArray = accountFiles;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string accountFile = strArray[index];
          account result = new account();
          using (StreamReader file = new StreamReader(accountFile))
          {
            result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
            file.Close();
            TOTALACCOUNT += 1M;
          }
          result = new account();
          accountFile = (string) null;
        }
        strArray = (string[]) null;
        IUserMessage userMessage = await this.ReplyAsync("there is a total of '" + TOTALACCOUNT.ToString() + "' imperial gold accounts.");
      }
      catch (Exception ex)
      {
        Console.WriteLine("1st");
        Console.WriteLine(ex.ToString());
      }
    }

    public struct Rank
    {
      public string name;
      public Decimal cost;
      public int minimumDays;

      public Rank(string NAME, Decimal COST, int MINDAYS)
      {
        this.name = NAME;
        this.cost = COST;
        this.minimumDays = MINDAYS;
      }
    }

    public class TrendData
    {
      public DateTime date;
      public Decimal gainGold = 0M;
      public Decimal looseGold = 0M;
      public Decimal transactionGold = 0M;
      public int timesCheckedBal = 0;
      public int patriatismCount = 0;
      public int timesRankedUp = 0;
      public int timesCheckedLogs = 0;
      public int timesMadePublic = 0;
      public int timesMadePrivate = 0;
      public string server;
    }
  }
}

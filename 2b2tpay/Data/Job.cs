// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Data.Job
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _2b2tpay.Data
{
  public class Job
  {
    public static List<Job.JobType> jobs = new List<Job.JobType>();
    public static bool hasBeenPayed = false;
    public static Decimal temp_JobPayments = 0M;

    public static async Task CheckJobPayments(
      SocketGuildUser[] users,
      SocketCommandContext Context)
    {
      try
      {
        DateTime dateTime = DateTime.Now;
        dateTime = dateTime.Date;
        if (dateTime.DayOfWeek != DayOfWeek.Sunday)
          return;
        bool isGoingToPayOut = true;
        bool fileIsNull = false;
        if (!File.Exists("data/" + Context.Guild.Name + "_payouts.txt"))
          fileIsNull = true;
        Thread.Sleep(1000);
        if (!fileIsNull)
        {
          using (StreamReader file69 = new StreamReader("data/" + Context.Guild.Name + "_payouts.txt"))
          {
            DateTime lastPayout = JsonConvert.DeserializeObject<DateTime>(file69.ReadToEnd());
            dateTime = lastPayout.Date;
            string str1 = dateTime.ToString();
            dateTime = DateTime.Now;
            dateTime = dateTime.Date;
            string str2 = dateTime.ToString();
            Console.WriteLine(str1 + " : " + str2);
            DateTime date1 = lastPayout.Date;
            dateTime = DateTime.Now;
            DateTime date2 = dateTime.Date;
            if (date1 == date2)
            {
              isGoingToPayOut = false;
            }
            else
            {
              file69.Close();
              using (StreamWriter accountFile = new StreamWriter("data/" + Context.Guild.Name + "_payouts.txt"))
              {
                accountFile.WriteLine(JsonConvert.SerializeObject((object) DateTime.Now));
                accountFile.Close();
              }
            }
          }
        }
        else
        {
          using (StreamWriter accountFile = new StreamWriter("data/" + Context.Guild.Name + "_payouts.txt"))
          {
            accountFile.WriteLine(JsonConvert.SerializeObject((object) DateTime.Now));
            accountFile.Close();
          }
        }
        if (isGoingToPayOut)
        {
          SocketGuildUser[] socketGuildUserArray = users;
          for (int index = 0; index < socketGuildUserArray.Length; ++index)
          {
            SocketGuildUser user = socketGuildUserArray[index];
            foreach (Job.JobType job1 in Job.jobs)
            {
              Job.JobType job = job1;
              IRole jobIf = ((IGuildUser) user).Guild.Roles.FirstOrDefault<IRole>((Func<IRole, bool>) (x => x.Name.Contains(job.name)));
              if (((IEnumerable<IRole>) user.Roles).Contains<IRole>(jobIf))
              {
                try
                {
                  account result = new account();
                  using (StreamReader file = new StreamReader("accounts/" + user.Id.ToString()))
                  {
                    result = JsonConvert.DeserializeObject<account>(file.ReadToEnd());
                    file.Close();
                    if (!job.isPercentage)
                    {
                      result.ballance += job.payRatePerWeek;
                      Job.temp_JobPayments += job.payRatePerWeek;
                      result.accountLogs.Add(new accountLog("[Job Wage] Your job as a " + job.name + " was payed " + job.payRatePerWeek.ToString() + " gold."));
                      result.transactions.Add(new accountLog("Your job as a " + job.name + " was payed " + job.payRatePerWeek.ToString() + " gold."));
                      using (StreamWriter accountFile = new StreamWriter("accounts/" + user.Id.ToString()))
                        accountFile.WriteLine(JsonConvert.SerializeObject((object) result));
                      Task<IDMChannel> h = user.GetOrCreateDMChannelAsync((RequestOptions) null);
                      IUserMessage userMessage = await h.Result.SendMessageAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("Wage for " + job.name, "You have been payed " + job.payRatePerWeek.ToString() + " gold, for this week's work! Your wage is payed out every Sunday.", true));
                      h = (Task<IDMChannel>) null;
                    }
                    else
                    {
                      result.ballance += job.payRatePerWeek;
                      Job.temp_JobPayments += job.payRatePerWeek;
                      result.accountLogs.Add(new accountLog("[Job Wage] Your job as a " + job.name + " was payed " + job.payRatePerWeek.ToString() + " gold."));
                      result.transactions.Add(new accountLog("Your job as a " + job.name + " was payed " + job.payRatePerWeek.ToString() + " gold."));
                      using (StreamWriter accountFile = new StreamWriter("accounts/" + user.Id.ToString()))
                        accountFile.WriteLine(JsonConvert.SerializeObject((object) result));
                      Task<IDMChannel> h = user.GetOrCreateDMChannelAsync((RequestOptions) null);
                      IUserMessage userMessage = await h.Result.SendMessageAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("Wage for " + job.name, "You have been payed " + job.payRatePerWeek.ToString() + " gold, for this week's work! Your wage is payed out every Sunday.", true));
                      h = (Task<IDMChannel>) null;
                    }
                  }
                  result = new account();
                }
                catch (Exception ex)
                {
                  Console.WriteLine("-- JOB ERROR -- ");
                  Console.WriteLine(ex.ToString());
                  Task<IDMChannel> h = user.GetOrCreateDMChannelAsync((RequestOptions) null);
                  IUserMessage userMessage = await h.Result.SendMessageAsync("", embed: _2b2tpay.Moduels.Commands.SendEmmbedMessage("Your job payment for " + job.name + " had an error!", "This is a message to let you know that your " + job.name + " wage might not have been sucsesfull, if it was not please contact a high rank."));
                  h = (Task<IDMChannel>) null;
                }
              }
              jobIf = (IRole) null;
            }
            user = (SocketGuildUser) null;
          }
          socketGuildUserArray = (SocketGuildUser[]) null;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("JOB PAYMENT ERRO --> " + ex.ToString());
      }
    }

    public struct JobType
    {
      public string name;
      public Decimal payRatePerWeek;
      public bool isPercentage;

      public JobType(string NAME, Decimal PAYRATE, bool percetnage)
      {
        this.name = NAME;
        this.payRatePerWeek = PAYRATE;
        this.isPercentage = percetnage;
      }
    }
  }
}

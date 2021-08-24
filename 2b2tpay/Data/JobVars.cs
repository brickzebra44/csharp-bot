// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Data.JobVars
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace _2b2tpay.Data
{
  public static class JobVars
  {
    private const string fileName = "jobvars.json";

    public static void Load()
    {
      try
      {
        if (!File.Exists("jobvars.json"))
          File.Create("jobvars.json");
        using (StreamReader streamReader = new StreamReader("jobvars.json"))
        {
          Job.jobs = JsonConvert.DeserializeObject<List<Job.JobType>>(streamReader.ReadToEnd());
          streamReader.Close();
          if (Job.jobs.Count != 0)
            return;
          Job.jobs = new List<Job.JobType>();
        }
      }
      catch
      {
        Console.WriteLine("failed to load jobvars.json...!!! going to create new job vars instead!!");
        Job.jobs = new List<Job.JobType>();
      }
    }

    public static void Save()
    {
      using (StreamWriter streamWriter = new StreamWriter("jobvars.json"))
        streamWriter.WriteLine(JsonConvert.SerializeObject((object) Job.jobs));
    }

    public static void UpdateVariable(string rankName, Decimal rankPayment)
    {
      rankName = rankName.Trim();
      int index = 0;
      foreach (Job.JobType job in Job.jobs)
      {
        if (job.name == rankName)
        {
          Job.jobs[index] = new Job.JobType(job.name, rankPayment, job.isPercentage);
          return;
        }
        ++index;
      }
      Job.jobs.Add(new Job.JobType(rankName, rankPayment, false));
      JobVars.Save();
    }
  }
}

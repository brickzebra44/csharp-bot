using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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
				{
					File.Create("jobvars.json");
				}
				using StreamReader streamReader = new StreamReader("jobvars.json");
				Job.jobs = JsonConvert.DeserializeObject<List<Job.JobType>>(streamReader.ReadToEnd());
				streamReader.Close();
				if (Job.jobs.Count == 0)
				{
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
			using StreamWriter streamWriter = new StreamWriter("jobvars.json");
			streamWriter.WriteLine(JsonConvert.SerializeObject((object)Job.jobs));
		}

		public static void UpdateVariable(string rankName, decimal rankPayment)
		{
			rankName = rankName.Trim();
			int num = 0;
			foreach (Job.JobType job in Job.jobs)
			{
				if (job.name == rankName)
				{
					Job.jobs[num] = new Job.JobType(job.name, rankPayment, job.isPercentage);
					return;
				}
				num++;
			}
			Job.jobs.Add(new Job.JobType(rankName, rankPayment, percetnage: false));
			Save();
		}
	}
}

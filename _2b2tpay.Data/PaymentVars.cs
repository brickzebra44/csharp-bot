using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace _2b2tpay.Data
{
	public static class PaymentVars
	{
		private const string fileName = "paymentvariables.json";

		public static Dictionary<string, string> variables = new Dictionary<string, string>();

		public static void LoadVariables()
		{
			try
			{
				if (!File.Exists("paymentvariables.json"))
				{
					File.Create("paymentvariables.json");
				}
				using StreamReader streamReader = new StreamReader("paymentvariables.json");
				variables = JsonConvert.DeserializeObject<Dictionary<string, string>>(streamReader.ReadToEnd());
				streamReader.Close();
				if (variables.Count == 0)
				{
					variables = new Dictionary<string, string>();
				}
			}
			catch
			{
				Console.WriteLine("failed to load paymentvariables.json...!!! going to create new payment variables instead!!");
				variables = new Dictionary<string, string>();
			}
		}

		public static void SaveVariables()
		{
			using StreamWriter streamWriter = new StreamWriter("paymentvariables.json");
			streamWriter.WriteLine(JsonConvert.SerializeObject((object)variables));
		}

		public static void UpdateVariable(string varName, string varData)
		{
			varName = varName.Trim();
			foreach (KeyValuePair<string, string> variable in variables)
			{
				if (variable.Key == varName)
				{
					variables[variable.Key] = varData;
					return;
				}
			}
			variables.Add(varName, varData);
			SaveVariables();
		}

		public static string CheckData(string data)
		{
			string text = data;
			foreach (KeyValuePair<string, string> variable in variables)
			{
				text = text.Replace(variable.Key, variable.Value);
			}
			return text;
		}
	}
}

// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Data.PaymentVars
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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
          File.Create("paymentvariables.json");
        using (StreamReader streamReader = new StreamReader("paymentvariables.json"))
        {
          PaymentVars.variables = JsonConvert.DeserializeObject<Dictionary<string, string>>(streamReader.ReadToEnd());
          streamReader.Close();
          if (PaymentVars.variables.Count != 0)
            return;
          PaymentVars.variables = new Dictionary<string, string>();
        }
      }
      catch
      {
        Console.WriteLine("failed to load paymentvariables.json...!!! going to create new payment variables instead!!");
        PaymentVars.variables = new Dictionary<string, string>();
      }
    }

    public static void SaveVariables()
    {
      using (StreamWriter streamWriter = new StreamWriter("paymentvariables.json"))
        streamWriter.WriteLine(JsonConvert.SerializeObject((object) PaymentVars.variables));
    }

    public static void UpdateVariable(string varName, string varData)
    {
      varName = varName.Trim();
      foreach (KeyValuePair<string, string> variable in PaymentVars.variables)
      {
        if (variable.Key == varName)
        {
          PaymentVars.variables[variable.Key] = varData;
          return;
        }
      }
      PaymentVars.variables.Add(varName, varData);
      PaymentVars.SaveVariables();
    }

    public static string CheckData(string data)
    {
      string str = data;
      foreach (KeyValuePair<string, string> variable in PaymentVars.variables)
        str = str.Replace(variable.Key, variable.Value);
      return str;
    }
  }
}

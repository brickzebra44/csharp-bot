// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Data.Intrest
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace _2b2tpay.Data
{
  public class Intrest
  {
    public static Decimal rate = 2M;
    public static DateTime nextPayout;

    public static bool checkNextIntrestPayout()
    {
      if (!File.Exists("nextIntrestPayout.json"))
      {
        Console.WriteLine("Intrest doesnt exsist, creating a new file for it: nextIntrestPayout.json");
        Intrest.IntrestData intrestData = new Intrest.IntrestData();
        intrestData.nextIntrestPayout = DateTime.Now;
        intrestData.percetage = 2M;
        intrestData.intrestLogs = new List<Intrest.IntrestLog>();
        using (StreamWriter streamWriter = new StreamWriter("nextIntrestPayout.json"))
        {
          streamWriter.WriteLine(JsonConvert.SerializeObject((object) intrestData));
          Console.WriteLine("Created file: nextIntrestPayout.json");
        }
      }
      Intrest.IntrestData intrestData1 = new Intrest.IntrestData();
      using (StreamReader streamReader = new StreamReader("nextIntrestPayout.json"))
      {
        Intrest.IntrestData intrestData2 = JsonConvert.DeserializeObject<Intrest.IntrestData>(streamReader.ReadToEnd());
        Intrest.rate = intrestData2.percetage;
        Intrest.nextPayout = intrestData2.nextIntrestPayout;
        streamReader.Close();
        if (DateTime.Now >= intrestData2.nextIntrestPayout)
        {
          Console.WriteLine("Paying out intrest...");
          Intrest.PayIntrest(intrestData2.percetage);
          File.WriteAllText("nextIntrestPayout.json", "");
          using (StreamWriter streamWriter = new StreamWriter("nextIntrestPayout.json"))
          {
            intrestData2.intrestLogs.Add(new Intrest.IntrestLog(DateTime.Now, intrestData2.percetage));
            intrestData2.nextIntrestPayout = intrestData2.nextIntrestPayout.AddDays(7.0);
            Console.WriteLine("nextPayout: " + intrestData2.nextIntrestPayout.ToString());
            streamWriter.WriteLine(JsonConvert.SerializeObject((object) intrestData2));
          }
          return true;
        }
      }
      return false;
    }

    public static void PayIntrest(Decimal intrestRate)
    {
      foreach (string file in Directory.GetFiles("accounts/"))
      {
        account account1 = new account();
        using (StreamReader streamReader = new StreamReader(file))
        {
          account account2 = JsonConvert.DeserializeObject<account>(streamReader.ReadToEnd());
          streamReader.Close();
          using (StreamWriter streamWriter = new StreamWriter(file))
          {
            Decimal num = Math.Round(intrestRate / 0M * account2.ballance, 2);
            account2.ballance += num;
            Console.WriteLine("Intrest for `" + account2.accountId.ToString() + "' is: " + num.ToString() + "%");
            account2.accountLogs.Add(new accountLog("Intrest " + account2.ballance.ToString() + " gold at " + num.ToString() + "%"));
            account2.transactions.Add(new accountLog("Intrest  " + account2.ballance.ToString() + " gold at " + num.ToString() + "%"));
            streamWriter.WriteLine(JsonConvert.SerializeObject((object) account2));
          }
        }
      }
    }

    public struct IntrestData
    {
      public DateTime nextIntrestPayout;
      public Decimal percetage;
      public List<Intrest.IntrestLog> intrestLogs;
    }

    public struct IntrestLog
    {
      public DateTime PayoutDate;
      public Decimal percentage;

      public IntrestLog(DateTime dateTime, Decimal number)
      {
        this.PayoutDate = dateTime;
        this.percentage = number;
      }
    }
  }
}

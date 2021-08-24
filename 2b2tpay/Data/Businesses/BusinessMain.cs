// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Data.Businesses.BusinessMain
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace _2b2tpay.Data.Businesses
{
  public static class BusinessMain
  {
    public static string directory = "businesses/";
    public static List<Business> businesses = new List<Business>();
    private static Random gen = new Random();

    public static void CreateBuiness(string name, string description, ulong owner)
    {
      Business business = new Business();
      business.name = name;
      business.description = description;
      business.owners = new List<ulong>() { owner };
      business.members = new List<ulong>();
      business.isPublic = true;
      business.ballance = 0M;
      int num;
      do
      {
        num = BusinessMain.gen.Next(0, 923456578);
      }
      while (File.Exists(BusinessMain.directory + num.ToString()));
      business.id = num;
      BusinessMain.SaveBuiness(business);
    }

    public static void SaveBuiness(Business business)
    {
      if (!File.Exists(BusinessMain.directory + business.id.ToString()))
        File.Create(BusinessMain.directory + business.id.ToString());
      using (StreamWriter streamWriter = new StreamWriter(BusinessMain.directory + business.id.ToString()))
        streamWriter.WriteLine(JsonConvert.SerializeObject((object) business));
    }

    public static void LoadBusiness(string name)
    {
    }
  }
}

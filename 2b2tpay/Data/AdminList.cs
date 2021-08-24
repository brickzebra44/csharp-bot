// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Data.AdminList
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace _2b2tpay.Data
{
  public class AdminList
  {
    public static List<ulong> ListOfAdminIds = new List<ulong>();

    public static void AddToList(ulong ID)
    {
      List<ulong> ulongList = new List<ulong>();
      using (StreamReader streamReader = new StreamReader("data/adminList.json"))
      {
        ulongList = JsonConvert.DeserializeObject<List<ulong>>(streamReader.ReadToEnd());
        streamReader.Close();
        ulongList.Add(ID);
        using (StreamWriter streamWriter = new StreamWriter("data/adminList.json"))
          streamWriter.WriteLine(JsonConvert.SerializeObject((object) ulongList));
      }
      AdminList.ListOfAdminIds = ulongList;
    }

    public static void UpdateAdminList()
    {
      if (!File.Exists("data/adminList.json"))
      {
        AdminList.ListOfAdminIds.Add(638246313506373666UL);
        using (StreamWriter streamWriter = new StreamWriter("data/adminList.json"))
        {
          streamWriter.WriteLine(JsonConvert.SerializeObject((object) AdminList.ListOfAdminIds));
          streamWriter.Close();
          Console.WriteLine("Upading admin list...");
        }
      }
      List<ulong> ulongList = new List<ulong>();
      using (StreamReader streamReader = new StreamReader("data/adminList.json"))
      {
        ulongList = JsonConvert.DeserializeObject<List<ulong>>(streamReader.ReadToEnd());
        streamReader.Close();
      }
      AdminList.ListOfAdminIds = ulongList;
    }

    public static void RemoveAdmin(ulong ID)
    {
      List<ulong> ulongList1 = new List<ulong>();
      using (StreamReader streamReader = new StreamReader("data/adminList.json"))
      {
        ulongList1 = JsonConvert.DeserializeObject<List<ulong>>(streamReader.ReadToEnd());
        streamReader.Close();
        List<ulong> ulongList2 = new List<ulong>();
        foreach (ulong num in ulongList1)
        {
          if ((long) num != (long) ID)
            ulongList2.Add(num);
        }
        using (StreamWriter streamWriter = new StreamWriter("data/adminList.json"))
        {
          streamWriter.WriteLine(JsonConvert.SerializeObject((object) ulongList2));
          streamWriter.Close();
        }
      }
      AdminList.ListOfAdminIds = ulongList1;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Data.Groups
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using System;
using System.Collections.Generic;

namespace _2b2tpay.Data
{
  public static class Groups
  {
    public struct Group
    {
      public int groupId;
      public Decimal ballance;
      public List<Groups.groupLog> groupLogs;
      public List<Groups.groupLog> transactions;
      public bool isPublic;
      public List<ulong> members;
      public List<ulong> owner;
    }

    public struct groupLog
    {
      public string detailOfLog;
      public DateTime timeOfLog;

      public groupLog(string x)
      {
        this.detailOfLog = x;
        this.timeOfLog = DateTime.Now;
      }
    }
  }
}

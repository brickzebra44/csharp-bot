// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Data.account
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using System;
using System.Collections.Generic;

namespace _2b2tpay.Data
{
  public struct account
  {
    public string name;
    public int accountId;
    public ulong accountUserId;
    public Decimal ballance;
    public List<accountLog> accountLogs;
    public List<accountLog> transactions;
    public bool isPublic;
  }
}

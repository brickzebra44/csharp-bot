// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Data.Businesses.Business
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using System;
using System.Collections.Generic;

namespace _2b2tpay.Data.Businesses
{
  public class Business
  {
    public string name = "";
    public string description = "";
    public int id = 0;
    public Decimal ballance = 0M;
    public List<groupLog> logs = new List<groupLog>();
    public bool isPublic = true;
    public List<ulong> members = new List<ulong>();
    public List<ulong> owners = new List<ulong>();
  }
}

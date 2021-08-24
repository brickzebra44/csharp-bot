// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Data.Businesses.groupLog
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using System;

namespace _2b2tpay.Data.Businesses
{
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

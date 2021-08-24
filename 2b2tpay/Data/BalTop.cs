// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Data.BalTop
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace _2b2tpay.Data
{
  public static class BalTop
  {
    public static List<account> SortAccounts(List<account> accounts)
    {
      List<account> source = new List<account>();
      foreach (account account in accounts)
      {
        if (account.isPublic)
          source.Add(account);
      }
      return source.OrderByDescending<account, Decimal>((Func<account, Decimal>) (xz => xz.ballance)).ToList<account>();
    }

    public static List<account> RealSortAccounts(List<account> accounts)
    {
      List<account> source = new List<account>();
      foreach (account account in accounts)
        source.Add(account);
      return source.OrderByDescending<account, Decimal>((Func<account, Decimal>) (xz => xz.ballance)).ToList<account>();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: _2b2tpay.Program
// Assembly: 2b2tpay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50B505BC-6C6A-4749-8E87-9AE243CAAAF0
// Assembly location: D:\gamesense\Debug\2b2tpay.exe

using _2b2tpay.Data;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace _2b2tpay
{
  internal class Program
  {
    public static bool inTestMode = false;
    private DiscordSocketClient _client;
    private CommandService _commands;
    private IServiceProvider _services;

    private static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

    public async Task RunBotAsync()
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      this._client = new DiscordSocketClient();
      this._commands = new CommandService();
      this._services = new ServiceCollection().AddSingleton<DiscordSocketClient>(this._client).AddSingleton<CommandService>(this._commands).AddSingleton<InteractiveService>().BuildServiceProvider();
      string token = "NjkyMjU3OTYzNTI3NzY2MDQ3.Xtc4XQ.mDBLUcz6wNQXN54EKMFq_tJT6vM";
      if (Program.inTestMode)
        token = "NjkyOTQwNzcxNjkwNTQ1MTYy.Xn11cA.ypmWGi3C9hiUPP6wm73siRx1QCs";
      PaymentVars.LoadVariables();
      JobVars.Load();
      this._client.Log += new Func<LogMessage, Task>(this._client_Log);
      this._client.MessageReceived += new Func<SocketMessage, Task>(this.MessageReceived);
      await this.RegisterCommandsAsync();
      await this._client.LoginAsync(Discord.TokenType.Bot, token);
      await this._client.StartAsync();
      await Task.Delay(-1);
    }

    private Task _client_Log(LogMessage arg)
    {
      Console.WriteLine((object) arg);
      return Task.CompletedTask;
    }

    private async Task MessageReceived(SocketMessage message)
    {
      if (message.Content[0] != '!')
        return;
      SocketGuildChannel e = message.Channel as SocketGuildChannel;
      await this.SenateLog(string.Format("COMMAND **{0}** | EXTRAINFO: | USER \"{1}\", {2} | SERVER/GUILD \"{3}\", {4} ", (object) message.Content, (object) message.Author.Username, (object) message.Author.Id.ToString(), (object) e.Guild.Name, (object) e.Guild.Id));
      e = (SocketGuildChannel) null;
    }

    public async Task RegisterCommandsAsync()
    {
      this._client.MessageReceived += new Func<SocketMessage, Task>(this.HandleCommandAsync);
      IEnumerable<ModuleInfo> moduleInfos = await this._commands.AddModulesAsync(Assembly.GetEntryAssembly(), this._services);
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
      SocketUserMessage message = arg as SocketUserMessage;
      SocketCommandContext context = new SocketCommandContext(this._client, message);
      if (message.Author.IsBot)
        return;
      int argPos = 0;
      if (!message.HasStringPrefix("!", ref argPos))
        return;
      IResult result = await this._commands.ExecuteAsync((ICommandContext) context, argPos, this._services);
      if (!result.IsSuccess)
        Console.WriteLine(result.ErrorReason);
      result = (IResult) null;
    }

    public async Task SenateLog(string log)
    {
      ulong channelID = 736862799782739999;
      SocketTextChannel channel = this._client.GetChannel(channelID) as SocketTextChannel;
      RestUserMessage restUserMessage = await channel.SendMessageAsync(log, false, (Embed) null, (RequestOptions) null);
    }
  }
}

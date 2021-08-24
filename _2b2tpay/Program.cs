using System;
using System.Reflection;
using System.Threading.Tasks;
using _2b2tpay.Data;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace _2b2tpay
{
	internal class Program
	{
		public static bool inTestMode = false;

		private DiscordSocketClient _client;

		private CommandService _commands;

		private IServiceProvider _services;

		private static void Main(string[] args)
		{
			new Program().RunBotAsync().GetAwaiter().GetResult();
		}

		public async Task RunBotAsync()
		{
			_client = new DiscordSocketClient();
			_commands = new CommandService();
			_services = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(ServiceCollectionServiceExtensions.AddSingleton<InteractiveService>(ServiceCollectionServiceExtensions.AddSingleton<CommandService>(ServiceCollectionServiceExtensions.AddSingleton<DiscordSocketClient>((IServiceCollection)new ServiceCollection(), _client), _commands)));
			string token = "NTE3MDcxMDI4NTE0NDU1NTY4.W_2l6w.1pC5oYB2Xw8i_rX3sUzDEqtj6jM";
			if (inTestMode)
			{
				token = "NTE3MDcxMDI4NTE0NDU1NTY4.W_2l6w.1pC5oYB2Xw8i_rX3sUzDEqtj6jM";
			}
			PaymentVars.LoadVariables();
			JobVars.Load();
			((BaseDiscordClient)_client).add_Log((Func<LogMessage, Task>)_client_Log);
			((BaseSocketClient)_client).add_MessageReceived((Func<SocketMessage, Task>)MessageReceived);
			await RegisterCommandsAsync();
			await ((BaseDiscordClient)_client).LoginAsync((TokenType)2, token, true);
			await ((BaseSocketClient)_client).StartAsync();
			await Task.Delay(-1);
		}

		private Task _client_Log(LogMessage arg)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			Console.WriteLine((object)arg);
			return Task.CompletedTask;
		}

		private async Task MessageReceived(SocketMessage message)
		{
			if (message.get_Content()[0] == '!')
			{
				ISocketMessageChannel channel = message.get_Channel();
				SocketGuildChannel e = channel as SocketGuildChannel;
				await SenateLog($"COMMAND **{message.get_Content()}** | EXTRAINFO: | USER \"{message.get_Author().get_Username()}\", {((SocketEntity<ulong>)(object)message.get_Author()).get_Id().ToString()} | SERVER/GUILD \"{e.get_Guild().get_Name()}\", {((SocketEntity<ulong>)(object)e.get_Guild()).get_Id()} ");
			}
		}

		public async Task RegisterCommandsAsync()
		{
			((BaseSocketClient)_client).add_MessageReceived((Func<SocketMessage, Task>)HandleCommandAsync);
			await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
		}

		private async Task HandleCommandAsync(SocketMessage arg)
		{
			SocketUserMessage message = arg as SocketUserMessage;
			SocketCommandContext context = new SocketCommandContext(_client, message);
			if (((SocketMessage)message).get_Author().get_IsBot())
			{
				return;
			}
			int argPos = 0;
			if (MessageExtensions.HasStringPrefix((IUserMessage)(object)message, "!", ref argPos, StringComparison.Ordinal))
			{
				IResult result = await _commands.ExecuteAsync((ICommandContext)(object)context, argPos, _services, (MultiMatchHandling)0);
				if (!result.get_IsSuccess())
				{
					Console.WriteLine(result.get_ErrorReason());
				}
			}
		}

		public async Task SenateLog(string log)
		{
			ulong channelID = 736862799782739999uL;
			SocketChannel channel2 = ((BaseSocketClient)_client).GetChannel(channelID);
			SocketTextChannel channel = channel2 as SocketTextChannel;
			await channel.SendMessageAsync(log, false, (Embed)null, (RequestOptions)null);
		}
	}
}

using Discord;
using Discord.Commands;
using Discord.WebSocket;
namespace App.Bot
{

    delegate Task MessageReceiver (SocketMessage message);

    /// <summary>
    /// Estrutura base para o bot do discord.
    /// </summary>
    internal struct DiscordBase
    {
        /// <summary>
        /// Elemento de conexão entre o bot e o Discord.
        /// </summary>
        private DiscordSocketClient Client { get; set; }

        /// <summary>
        /// Conjunto de funções assíncronas para o processamento de uma mensagem recebida.
        /// </summary>
        public MessageReceiver? Process = null;

        /// <summary>
        /// Mensagens de log apresentadas pelo bot.
        /// </summary>
        /// <param name="message">Mensagem a ser apresentada.</param>
        /// <returns>Retorna a finalização da tarefa.</returns>
        private Task Log(LogMessage message)
        {
            if (message.Exception is CommandException cmdException)
            {
                Utilits.Log.WriteLine(Utilits.Log.Type.Waring,
                    $"[Command/{message.Severity}] {cmdException.Command.Aliases[0]} failed to execute in {cmdException.Context.Channel}");
                Utilits.Log.WriteLine(Utilits.Log.Type.BotException, cmdException.Message);
            }
            else
                Utilits.Log.WriteLine(Utilits.Log.Type.Bot, $"[General/{message.Severity}] {message.Message}");

            return Task.CompletedTask;
        }
        /// <summary>
        /// Manda uma mensagem recebida para as estruturas de tratamento.
        /// </summary>
        /// <param name="message">Mensage recebida.</param>
        private async Task MessageReceive (SocketMessage message)
        {
            if (!message.Content.StartsWith(Static.Settings.GetBot_StartCommand)) return;
            if (Process == null) return;
            await Process(message);
        }

        /// <summary>
        /// Envia uma mensagem para um canal de mensagem específico.
        /// </summary>
        /// <param name="channel">Canal de mensagem.</param>
        /// <param name="message">Mensagem a ser enviada.</param>
        /// <returns>Retorna a mensagem que foi enviada.</returns>
        public static async Task<IUserMessage> Send(IMessageChannel channel, string message)
        {
            return await channel.SendMessageAsync(message);
        }

        /// <summary>
        /// Envia uma mensagem para um canal de mensagem específico.
        /// </summary>
        /// <param name="channel_id">ID do canal.</param>
        /// <param name="message">Mensagem a ser enviada.</param>
        /// <returns>Retorna a mensagem que foi enviada.</returns>
        public async Task<IUserMessage> Send(ulong channel_id, string message)
        {
            IMessageChannel _channel = (IMessageChannel)await Client.GetChannelAsync(channel_id);
            return await Send(_channel, message);
        }
        /// <summary>
        /// Realiza a conexão entre o Bot e o Discord.
        /// </summary>
        /// <param name="token">Token de autentificação para o serviço.</param>
        /// <returns>Retorna ao finalizar a conexão.</returns>
        public async Task Login (string? token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
            {
                Utilits.Log.WriteLine(Utilits.Log.Type.Error, "Falha na conexão, token inválido.");
                Environment.Exit(1);
            }
            //  > Faz login do bot no discord.
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();
        }

        /// <summary>
        /// Instância uma nova base de conexão entre o serviço e o discord.
        /// </summary>
        public DiscordBase()
        {
            //  > Instância a conexão e atribui as funções aos delegates.
            Client = new DiscordSocketClient();
            Client.Log += Log;
            Client.MessageReceived += MessageReceive;
        }
    }
}
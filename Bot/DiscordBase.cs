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
        /// Lista de servidores atendidos pelo bot.
        /// </summary>
        public List<Database.Guild> Guilds { get; set; }
        /// <summary>
        /// Lista de canais de texto das guilds do Discord.
        /// </summary>
        public List<Database.TextChannel> TextChannels { get; set; }

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
            //  > Verifica se a mensagem menciona o Marvin.
            foreach (SocketUser user in message.MentionedUsers)
            {
                if (user.Id == Static.Settings.GetBot_Id)
                {
                    //  > Se mencionar o marvin, executa os processos posteriores.
                    if (Process == null) return;
                    await Process(message);

                    break;
                }
            }
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
        /// Carrega as Guilds com base na conexão do bot.
        /// </summary>
        /// <returns>Lista com as guilds do Discord.</returns>
        public async Task LoadGuildsFrom()
        {
            //  > Lista para retorno.
            Guilds = new ();
            //  > Lista as guilds.
            foreach (SocketGuild guild in Client.Guilds)
            {
                Utilits.Log.WriteLine(Utilits.Log.Type.System, $"Adicionando '{guild.Name}' . . . ");
                //  > Adiciona a Guild lista.
                Database.Guild _guild = new (guild);
                Guilds.Add(_guild);
                //  > Carrega os canais de texto dessa Guild.
                TextChannels.AddRange(await _guild.GetTextChannels());                
            }
        }

        /// <summary>
        /// Instância uma nova base de conexão entre o serviço e o discord.
        /// </summary>
        public DiscordBase()
        {
            //  > Instância a lista de servidores atendidos.
            Guilds = new();
            TextChannels = new ();
            //  > Instância a conexão e atribui as funções aos delegates.
            Client = new ();
            Client.Log += Log;
            Client.MessageReceived += MessageReceive;
        }
    }
}
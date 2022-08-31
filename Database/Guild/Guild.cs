using Discord.WebSocket;
namespace App.Database
{
    /// <summary>
    /// Representa um servidor do discord.
    /// </summary>
    internal class Guild
    {
        /// <summary>
        /// ID da Guild dentro do discord.
        /// </summary>
        public ulong ID { get; private set; }
        /// <summary>
        /// Representa a conexão da guilda.
        /// </summary>
        public SocketGuild Socket { get; private set; }

        /// <summary>
        /// Pega os canais de texto da guild.
        /// </summary>
        /// <returns>Retorna uma lista com os canais de texto.</returns>
        public Task<List<TextChannel>> GetTextChannels ()
        {
            //  > Lista para retorno.
            List<TextChannel> _return = new ();
            //  > Pega os canais de texto.
            foreach (SocketTextChannel channel in Socket.TextChannels)
            {
                Utilits.Log.WriteLine(Utilits.Log.Type.System, 
                    $"\tCarregando canal '{channel.Name}' em '{channel.Guild.Name}' . . .");
                _return.Add(new TextChannel(channel));
            }
            //  > Retorna a lista construída.
            return Task.FromResult(_return);
        }

        /// <summary>
        /// Constituí uma estrutura de guild.
        /// </summary>
        public Guild (SocketGuild socket)
        {
            Socket = socket;
            ID = socket.Id;
        }
    }
}
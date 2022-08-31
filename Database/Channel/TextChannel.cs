using Discord.WebSocket;
using Discord.Rest;
using MySql.Data.MySqlClient;
namespace App.Database
{
    /// <summary>
    /// Estrutura de um canal de texto do Discord.
    /// </summary>
    internal class TextChannel : MySQLBase
    {
        /// <summary>
        /// ID do canal de texto.
        /// </summary>
        public ulong ID { get; private set; }
        /// <summary>
        /// Nome do canal de texto.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// TAG do canal de texto.
        /// </summary>
        public string? TAG { get; private set; }
        /// <summary>
        /// Socket do canal de texto.
        /// </summary>
        public SocketTextChannel Socket { get; private set; }

        /// <summary>
        /// Envia uma mensagem no respectivo canal do Discord.
        /// </summary>
        /// <param name="message">Mensagem a ser enviada.</param>
        public async Task<RestUserMessage> Send (string message)
        {
            //  > Debug
            Utilits.Log.WriteLine(Utilits.Log.Type.System, $"Enviando para {Socket.Guild.Name} -> {Name}");
            //  > Substitui a estrutura para mention all.
            message = message.Replace("<@everybody>", Socket.Guild.EveryoneRole.Mention);
            //  > Envia a mensagem.
            return await Socket.SendMessageAsync(message);
        }

        /// <summary>
        /// Estrutura e carrega um novo canal.
        /// </summary>
        /// <param name="socket"></param>
        public TextChannel (SocketTextChannel socket)
        {
            //  > Inicializa o ambiente.
            Socket = socket;

            ID = socket.Id;
            Name = socket.Name;

            TAG = null;

            //  > Carrega a Socket via database.
            //  - Comando SQL.
            string _sqlCommand = $"SELECT * FROM textchannels WHERE id='{ID}'";
            //  - Tenta estabelecer a conexão MySQL.
            int tries = 0;
            while (tries <= Convert.ToInt32(Static.Settings.Setting["DB_CONN_TRIES"]))
            {
                try
                {
                    //  > Abre a conexão MySQL.
                    Task<MySqlConnection> _open = Open();
                    _open.Wait();

                    //  > Estrutura o comando.
                    MySqlCommand _command = new (_sqlCommand, _open.Result);

                    //  > Executa a consulta.
                    MySqlDataReader _reader = _command.ExecuteReader();
                    if (_reader.Read())
                    {
                        //  > Pega os dados originados do banco de dados.
                        string _name = _reader.GetString("name");
                        TAG = _reader.GetString("tag");

                        //  > Fecha a leitura.
                        _reader.Close();

                        //  > Verifica se a TAG é nula.
                        if ((TAG == "NULL") || (string.IsNullOrEmpty(TAG) || string.IsNullOrWhiteSpace(TAG)))
                            TAG = null;

                        //  > Verifica se o nome precisa ser atualizado.
                        if (_name != Name)
                        {
                            //  > Se for diferente, atualiza.
                            _sqlCommand = $"UPDATE textchannels SET name='{Name}' WHERE id='{ID}'";
                            _command.CommandText = _sqlCommand;
                            if (_command.ExecuteNonQuery() > 0)
                                Utilits.Log.WriteLine(Utilits.Log.Type.Database, 
                                    $"[UPDATE textchannels] [{ID}] {_name} > {Name}");
                        }

                        //  > Debug.
                        Utilits.Log.WriteLine(Utilits.Log.Type.Database, 
                            $"[DB:SELECT] [{ID}]({TAG} {Name})");
                        break;
                    }
                    else
                    {
                        //  > Fecha o DataReader.
                        _reader.Close();

                        //  > Caso não leia nenhuma resposta, adiciona ao banco de dados.
                        _sqlCommand = $"INSERT INTO textchannels (id, name) VALUES ({ID}, '{Name}')";
                        _command.CommandText = _sqlCommand;
                        if (_command.ExecuteNonQuery() > 0)
                            Utilits.Log.WriteLine(Utilits.Log.Type.Database,
                                $"[INSERT INTO textchannels] [{ID}] {Name}");
                        break;
                    }

                }
                catch (Exception error)
                {
                    tries++;
                    Utilits.Log.WriteLine(Utilits.Log.Type.Error,
                        $"Falha ao tentar estabelecer uma conexão MySQL ({tries}/{Convert.ToInt32(Static.Settings.Setting["DB_CONN_TRIES"])}): \n" +
                        $"{error.Message}\nNova tentativa em {4 + 3 * (tries / 2)} segundos.");
                    Task.Delay((4 + 3 * (tries / 2)) * 1000).Wait();
                }
                finally
                {
                    Close();
                }
            }
        }
        /*
        //  > Cadeia de comando para consulta por um ID específico.
        string _sqlCommand = $"SELECT * FROM ru WHERE id='{id}'";

        //  > Tenta estabelecer uma conexão MySQL e realizar a consulta.
        int tries = 0;
            while (tries <= Static.Settings.GetMySql_ConnTries)
            {
                try
                {
                    //  > Abre a conexão MySQL.
                    Task<MySqlConnection> _open = Open();
        _open.Wait();

                    //  > Estrutura de comando atrelada a conexão MySQL.
                    MySqlCommand _command = new(_sqlCommand, _open.Result);
        //  > Executa a consulta.
        MySqlDataReader _reader = _command.ExecuteReader();
                    if (_reader.Read())
                    {
                        ID = id;
                        Tag = _reader.GetString("tag");
                        Name = _reader.GetString("name");
                        URL = _reader.GetString("url");
                    }
    Utilits.Log.WriteLine(Utilits.Log.Type.Database, $"[DB:SELECT]{this}");

                    //  > Fecha a leitura.
                    _reader.Close();
                    //  > Interrompe o laço de tentativas.
                    break;
                }
                catch (Exception error)
{
    tries++;
    Utilits.Log.WriteLine(Utilits.Log.Type.Error,
        $"Falha ao tentar estabelecer uma conexão MySQL ({tries}/{Static.Settings.GetMySql_ConnTries}): \n" +
        $"{error.Message}\nNova tentativa em {4 + 3 * (tries / 2)} segundos.");
    Task.Delay((4 + 3 * (tries / 2)) * 1000).Wait();
}
finally
{
    Close();
}
            }
        }
        */
    }
}
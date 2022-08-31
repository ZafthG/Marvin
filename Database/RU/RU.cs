using MySql.Data.MySqlClient;
using System.Text;
namespace App.Database
{
    /// <summary>
    /// Estrutura de dados do RU.
    /// </summary>
    internal class RU : MySQLBase
    {
        /// <summary>
        /// ID do RU no sistema interno.
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// Tag de referëncia para o RU.
        /// </summary>
        public string Tag { get; private set; }
        /// <summary>
        /// Nome do RU propriamente dito.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// URL do Site do RU.
        /// </summary>
        public string URL { get; private set; }

        /// <summary>
        /// Menu de hoje.
        /// </summary>
        public Menu? TodayMenu { get; set; }
        /// <summary>
        /// Menu dos próximos dias.
        /// </summary>
        public Dictionary<DateTime, Menu>? NextDays { get; set; }

        /// <summary>
        /// Envia o RU de hoje para o respectivo canal associado.
        /// </summary>
        /// <returns></returns>
        public Task SendRUToday ( )
        {
            //  > Verifica se o RU de hoje foi declarado.
            if (TodayMenu == null) return Task.CompletedTask;

            //  > Prepara o envio.
            StringBuilder _const = new ();

            _const.Append($"Bom dia. Hoje teremos o seguinte cardápio por R$ 1.30 no {Name}: \n");
            _const.Append(TodayMenu.ToString());

            //  > Pega os canais para envio.
            IEnumerable<TextChannel> sendChannels = Static.Global.Marvin.TextChannels
                .Where(channel => (channel.TAG == "TEST" /*|| channel.TAG == $"RU_{Tag}"*/));
            //  > Envia as mensagens.
            foreach (TextChannel channel in sendChannels)
                _ = channel.Send(_const.ToString());

            //  > Finaliza.
            return Task.CompletedTask;
            
        }
        /// <summary>
        /// Obtem todos os RU listados no banco de dados.
        /// </summary>
        /// <returns></returns>
        public async Task<List<RU>> LoadAll ()
        {
            //  > Lista de referência para retorno.
            List<RU> _return = new ();

            //  > Cadeia de comando para consulta por um ID específico.
            string _sqlCommand = "select * from ru";
            //  > Tenta estabelecer uma conexão MySQL e realizar a consulta.
            int tries = 0;
            while (tries <= Static.Settings.GetMySql_ConnTries)
            {
                try
                {
                    //  > Abre a conexão MySQL e estrutura de comando atrelada a conexão MySQL.
                    MySqlCommand _command = new (_sqlCommand, await Open());

                    //  > Executa a consulta.
                    System.Data.Common.DbDataReader _reader = await _command.ExecuteReaderAsync();
                    while (await _reader.ReadAsync())
                    {
                        //  > Instância um novo RU vazio.
                        RU _ref = new()
                        {
                            //  > Salva os valores carregados do banco de dados.
                            ID = _reader.GetInt32(0),
                            Tag = _reader.GetString(1),
                            Name = _reader.GetString(2),
                            URL = _reader.GetString(3)
                        };
                        
                        //  > Adiciona a lista e segue pro próximo.
                        _return.Add(_ref);
                        Utilits.Log.WriteLine(Utilits.Log.Type.Database, $"[DB:SELECT]{_ref}");
                    }

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

            //  > Finaliza.
            return _return;
        }

        /// <summary>
        /// Organiza o formato de impressão do modelo.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{ID}:{Tag}] {Name} - URL: {URL}";
        }

        /// <summary>
        /// Carrega as informações específicas sobre o RU de ID informado no contexto do
        /// banco de dados.
        /// </summary>
        /// <param name="id">ID no RU no banco de dados.</param>
        public RU (int id)
        {
            //  > Inicializa as variáveis só pra IDE parar de encher o saco.
            ID = -1;
            Tag = "";
            Name = "";
            URL = "";

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
                    MySqlCommand _command = new (_sqlCommand, _open.Result);
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
                        $"Falha ao tentar estabelecer uma conexão MySQL ({tries}/{Static.Settings.GetMySql_ConnTries}): \n"+
                        $"{error.Message}\nNova tentativa em {4 + 3 * (tries / 2)} segundos.");
                    Task.Delay((4 + 3 * (tries / 2)) * 1000).Wait();
                }
                finally
                {
                    Close();
                }
            }
        }
        /// <summary>
        /// Método de construção estrutural nulo.
        /// </summary>
        public RU () { ID = -1; Tag = ""; Name = ""; URL = ""; }
    }
}
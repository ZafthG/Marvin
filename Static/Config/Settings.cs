using System.Text;
using System.Text.RegularExpressions;
namespace App.Static
{
    /// <summary>
    /// Estrutura responsável por gerenciar as configurações do programa.
    /// </summary>
    internal static class Settings
    {
        /// <summary>
        /// Nome do arquivo de configurações.
        /// </summary>
        private const string CONF_FILENAME = "bot.conf";
        /// <summary>
        /// Caracter que indica as linhas de configuração que devem ser ignoradas.
        /// Isso não foi implementado para o meio do texto, pois poderia gerar incompatibilidades futuras.
        /// </summary>
        private const char CONF_IGNORE = '#';
        /// <summary>
        /// Caracter que indica o processo de definição 'CHAVE[CONF_DEFINE]VALOR'.
        /// </summary>
        private const char CONF_DEFINE = ' ';

        /// <summary>
        /// Conjunto chave-valor que representa as configurações.
        /// </summary>
        private static readonly Dictionary<string, string> settings = new ();

        /// <summary>
        /// Obtém a cadeia de conexão para o banco de dados MySQL conforme
        /// informações do arquivo CONF.
        /// </summary>
        /// <returns>Retorna a cadeia de conexão.</returns>
        public static string GetMySQL_ConnString { get { 
                return $"server={settings["DB_SERVER"]};uid={settings["DB_USER"]};pwd={settings["DB_PASS"]};database={settings["DB_NAME"]}"; 
        } }
        /// <summary>
        /// Obtém o número de tentativas que devem ser realizadas antes de
        /// desistir ao estabelecer uma nova conexão MySQL.
        /// </summary>
        public static int GetMySql_ConnTries { get {
                return Convert.ToInt32(settings["DB_CONN_TRIES"]);
        } }
        /// <summary>
        /// Obtém a chave de variável de ambiente para o Token do Discord do Marvin.
        /// </summary>
        public static string GetDiscord_Token { get {
                return settings["E_V_MARVIN_TK"];
        } }
        /// <summary>
        /// Obtém o conjunto de caracteres para o início de uma mensagem de comando.
        /// </summary>
        public static ulong GetBot_Id { get {
                return Convert.ToUInt64(settings["BOT_ID"]);
        } }

        /// <summary>
        /// Carrega as configurações a partir do arquivo pré-definido.
        /// </summary>
        public static async Task Load()
        {
            //  > Carrega o conteúdo do arquivo de configurações.
            Utilits.Files.Reader _reader = new (CONF_FILENAME);
            byte[] _conf_content_bytes = await _reader.Read();
            string _conf_content = Encoding.UTF8.GetString(_conf_content_bytes);

            //  > Inicializa o processamento do arquivo de configurações.
            string[] _confs = _conf_content.Split('\n');
            foreach (string _conf in _confs)
            {
                //  - Remove as linhas de configuração comentadas.
                if (_conf.StartsWith(CONF_IGNORE)) continue;
                //  - Remove as linhas em branco.
                if (string.IsNullOrEmpty(_conf) || string.IsNullOrWhiteSpace(_conf)) continue;

                //  - Separa a configuração entre sua TAG e seu valor.
                string[] _s_conf = _conf.Split(CONF_DEFINE);

                //  - Macros de substituição para valor.
                _s_conf[1] = Regex.Replace(_s_conf[1], @"\@'null'", "");

                //  - Corrige o problema de substituição.
                char[] _err_replace = _s_conf[1].ToCharArray();
                StringBuilder _structer = new ();
                for (int i = 0; i < (_err_replace.Length - 1); i++)
                {
                    _structer.Append(_err_replace[i]);
                }

                //  - Conforme será considerado:
                settings.Add(_s_conf[0], _structer.ToString());
            }
        }
    }
}
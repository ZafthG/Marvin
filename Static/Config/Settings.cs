// ------------------------------------------------------------------------------------------------------------ //
//                                                  class   Settings.cs                                         //
// ------------------------------------------------------------------------------------------------------------ //
//          Responsável por carregar e gerenciar as configurações locais do programa. Uma vez carregadas só     //
//  podem ser alteradas caso o programa reinicie e não podem ser modificadas dentro do programa. São constantes //
//  para evitar qualquer problema de alteração.                                                                 //
//                                                                                                              //
//          O único arquivo lido é o 'bot.conf'. Caso ele não possa ser lido ou não exista, um erro é exibido   //
//  e o programa é encerrado, pois, não é possível prosseguir sem sua existência.                               //
//          O acesso aos dados podem ser feitos por meio do dicionário de configurações. Sendo necessária a     //
//  conversão do valor para o desejado dependendo da situação, pois o padrão dos dados é o formato string.      //
// ------------------------------------------------------------------------------------------------------------ //
//  Por Gabriel Ferreira / Zafth                                                                                //
//  Github: https://github.com/ZafthG                                                                           //
// ------------------------------------------------------------------------------------------------------------ //
//  Referências utilizadas no código:                                                                           //
using System.Text;                                                                                              //
using System.Text.RegularExpressions;                                                                           //
// ------------------------------------------------------------------------------------------------------------ //
namespace App.Static
{
    /// <summary>
    /// Classe responsável por gerenciar as configurações do programa.
    /// </summary>
    internal static class Settings
    {
        // ---------------------------------------------------------------------------------------------------- //
        //  Constantes de Settings.cs                                                                           //
        #region Variables
        /// <summary> Nome e extensão do arquivo de configurações presente no diretório raiz do programa. </summary>
        private const string CONF_FILENAME = "bot.conf";
        /// <summary> Caracter que indica um comentário dentro do arquivo de configurações resultando em
        ///         linhas ignoradas. </summary>
        private const char CONF_IGNORE = '#';
        /// <summary> Caracter que indica o conceito de igualdade no contexto do documento de configurações.
        ///         Ele indica a divisão entre a chave e seu valor. </summary>
        private const char CONF_DEFINE = ' ';

        #endregion
        // ---------------------------------------------------------------------------------------------------- //
        //  Variáveis internas de Settings.cs                                                                   //
        #region Variables

        /// <summary> Conjunto chave-valor que indica as configurações carregas do arquivo 'bot.conf'. </summary>
        private static readonly Dictionary<string, object> settings = new ();

        #endregion
        // ---------------------------------------------------------------------------------------------------- //
        //  Propriedades de acesso ao ambiente interno de Settings.cs                                           //
        #region Properties

        /// <summary> Retorna a lista de configurações. Para pegar uma configuração em específico é necessário
        ///         informas a Key conforme indicado no arquivo 'bot.conf'. </summary>
        public static Dictionary<string, object> Setting
        {
            get 
            { 
                return settings; 
            }
        }

        #endregion
        // ---------------------------------------------------------------------------------------------------- //
        //  Métodos sem retorno de Settings.cs                                                                  //
        #region Methods

        /// <summary> Carrega as configurações a partir do arquivo 'bot.conf'.
        /// </summary>
        public static async Task Load()
        {
            //  > Declara as variáveis utilizadas.
            string conf_content = "";

            //  > Tenta fazer a leitura do arquivo.
            try
            {
                //  > Inicializa o ambiente de leitura.
                Utilits.Files.Reader _reader = new (CONF_FILENAME);
                //  > Lê o arquivo e faz a conversão dos binários para string conforme tabela UTF-8.
                byte[] _conf_content_bytes = await _reader.Read();
                conf_content = Encoding.UTF8.GetString(_conf_content_bytes);
            }
            catch (Exception e)
            {
                //  > Exibe o erro e encerra o programa.
                Utilits.Log.WriteLine(Utilits.Log.Type.Error,
                    $"Falha na leitura do arquivo 'bot.conf': {e.Message}");
            }

            //  > Inicializa o processamento do arquivo de configurações.
            //  Quebra o arquivo em linhas.
            string[] confs = conf_content.Split('\n');
            foreach (string conf in confs)
            {
                //  - Remove as linhas de configuração comentadas.
                if (conf.StartsWith(CONF_IGNORE)) continue;
                //  - Remove as linhas em branco.
                if (string.IsNullOrEmpty(conf) || string.IsNullOrWhiteSpace(conf)) continue;

                //  - Separa a configuração entre sua TAG e seu valor.
                string[] _s_conf = conf.Split(CONF_DEFINE);

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

        #endregion
        // ---------------------------------------------------------------------------------------------------- //
    }
}
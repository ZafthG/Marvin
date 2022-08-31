using MySql.Data.MySqlClient;
namespace App.Database
{
    /// <summary>
    /// Interface responsável por determinar as características básicas de uma
    /// conexão MySQL.
    /// </summary>
    internal class MySQLBase
    {
        /// <summary>
        /// Estrutura da conexão MySQL.
        /// </summary>
        private MySqlConnection? Connection { get; set; }

        /// <summary>
        /// Fecha uma conexão MySQL.
        /// </summary>
        protected void Close()
        {
            if (Connection == null) return;
            try
            {
                Connection.Close();
                Utilits.Log.WriteLine(Utilits.Log.Type.Database, $"Conexão MySQL encerrada em {DateTime.Now.ToUniversalTime()}");
            }
            catch (MySqlException error)
            {
                throw new Exception($"Falha ao tentar fechar uma conexão MySQL: {error.Number}\n{error.Message}", error);
            }
        }
        /// <summary>
        /// Método responsável por tentar estabelecer uma conexão MySQL.
        /// </summary>
        protected async Task<MySqlConnection> Open()
        {
            try
            {
                string connString = $"server={Static.Settings.Setting["DB_SERVER"]};" +
                    $"uid={Static.Settings.Setting["DB_USER"]};" +
                    $"pwd={Static.Settings.Setting["DB_PASS"]};" +
                    $"database={Static.Settings.Setting["DB_NAME"]}";
                Connection = new(connString);
                await Connection.OpenAsync();
                Utilits.Log.WriteLine(Utilits.Log.Type.Database, $"Conexão MySQL estabelecida em {DateTime.Now.ToUniversalTime()}");
            }
            catch (MySqlException error)
            {
                throw new Exception($"Falha ao tentar abrir uma conexão MySQL: {error.Number}\n{error.Message}", error);
            }

            return Connection;
        }

    }
}
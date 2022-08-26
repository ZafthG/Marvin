namespace App.Static
{
    /// <summary>
    /// Variáves globais do sistema.
    /// </summary>
    internal static class Global
    {
        /// <summary>
        /// Seta o dia de hoje.
        /// </summary>
        public static DateTime LastDayUpdate = DateTime.MinValue;
        /// <summary>
        /// Verificações de validação do laço global.
        /// </summary>
        public static LoopItem? Verify = null;
        /// <summary>
        /// Eventos que devem ser executados no Runtime do programa.
        /// </summary>
        public static LoopItem? Execute = null;
        /// <summary>
        /// Estrutura base de conexão do Marvin.
        /// </summary>
        public static Bot.DiscordBase Marvin = new ();

        /// <summary>
        /// Lista de RUs com base no banco de dados.
        /// </summary>
        public static List<Database.RU> RUs_List= new ();
    }
}
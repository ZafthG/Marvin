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
        public static DateTime Today = DateTime.Now.ToUniversalTime().AddHours(-3).Date;
        /// <summary>
        /// Eventos que devem ser executados no Runtime do programa.
        /// </summary>
        public static LoopItem? Execute = null;
        /// <summary>
        /// Estrutura base de conexão do Marvin.
        /// </summary>
        public static Bot.DiscordBase Marvin = new ();
    }
}
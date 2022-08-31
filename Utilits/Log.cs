// ------------------------------------------------------------------------------------------------------------ //
//                                                  class   Log.cs                                              //
// ------------------------------------------------------------------------------------------------------------ //
//          Classe responsável pelo gerenciamento de dados no console do servidor. Exibe as saídas e faz        //
//  a distinção por cores.                                                                                      //
// ------------------------------------------------------------------------------------------------------------ //
//  Por Gabriel Ferreira / Zafth                                                                                //
//  Github: https://github.com/ZafthG                                                                           //
// ------------------------------------------------------------------------------------------------------------ //
// Referências utilizadas no código:                                                                            //
using System.Text;                                                                                              //
// ------------------------------------------------------------------------------------------------------------ //
namespace App.Utilits
{
    /// <summary>
    /// Classe responsável pela exibição de elementos do Log no console.
    /// </summary>
    internal static class Log
    {
        // ---------------------------------------------------------------------------------------------------- //
        //      Listas de dados do Log.                                                                         //  
        #region Enum

        /// <summary>
        /// Tipo de mensagem de log..
        /// </summary>
        public enum Type
        {
            System,
            Database,
            Bot,
            BotMessage,
            BotException,
            Waring,
            Error,
            Default
        }
        // ---------------------------------------------------------------------------------------------------- //
        #endregion
        // ---------------------------------------------------------------------------------------------------- //
        //      Métodos sem retorno de Log.cs.                                                                  //  
        #region Methods

        /// <summary> Imprime uma linha de mensagem. </summary>
        /// <param name="logType">Tipo de mensagem de log.</param>
        /// <param name="message">Mensagem a ser adicionada ao log.</param>
        public static void WriteLine(Type logType, string message)
        {
            StringBuilder sb = new ();
            sb.Append(message);
            sb.Append('\n');
            Write(logType, sb.ToString());
        }
        /// <summary> Escreve uma mensagem no console. </summary>
        /// <param name="logType">Tipo de mensagem.</param>
        /// <param name="message">Mensagem a ser adicionada ao log.</param>
        public static void Write(Type logType, string message)
        {
            Console.ForegroundColor = GetConsoleColor(logType);
            Console.Write($"[{logType}]: {message}");
            Console.ForegroundColor = GetConsoleColor(Type.Default);
        }

        #endregion

        /// <summary> Adquire a cor de texto para a escrita do console. </summary>
        /// <param name="logType">Tipo de mensagem de log.</param>
        /// <returns>Retorna a cor do console.</returns>
        private static ConsoleColor GetConsoleColor(Type logType)
        {
            if (logType == Type.System)
                return ConsoleColor.Magenta;
            else if (logType == Type.Database)
                return ConsoleColor.Cyan;
            else if (logType == Type.Bot)
                return ConsoleColor.Gray;
            else if (logType == Type.Waring)
                return ConsoleColor.DarkYellow;
            else if (logType == Type.Error)
                return ConsoleColor.Red;
            else if (logType == Type.BotException)
                return ConsoleColor.DarkCyan;
            else if (logType == Type.BotMessage)
                return ConsoleColor.DarkGreen;
            else
                return ConsoleColor.White;
        }
    }
}
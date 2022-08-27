using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text.RegularExpressions;
namespace App.Runtime.Events
{
    /// <summary>
    /// Eventos de atualialização das informações do RU.
    /// </summary>
    internal class RURefresh
    {
        /// <summary>
        /// Determina o momento em que o método call foi adicionado a lista de execução.
        /// </summary>
        public static DateTime CallMoment { get; set; }
        /// <summary>
        /// RUs que devem sofrer atualização.
        /// </summary>
        public static List<Database.RU>? RefreshRUs { get; set; }

        /// <summary>
        /// Chama o evento para atualização dos RU.
        /// ** Sete RURefresh.CallMoment = DateTime.Now !!
        /// </summary>
        public static Task Call()
        {
            //  Verifica se deve fazer uma nova validação, caso contrário, ignora e segue.
            if (CallMoment.AddHours(1).Ticks > DateTime.Now.ToUniversalTime().Ticks)
                return Task.CompletedTask;

            Console.WriteLine("Em construção");

            //  Remove a função da lista de execução e finaliza.
            Static.Global.Execute -= Call;
            return Task.CompletedTask;
        }
    }
}
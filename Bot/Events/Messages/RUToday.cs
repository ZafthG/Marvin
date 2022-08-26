using Discord;
namespace App.Bot.Events.Messages
{
    internal static class RUToday
    {
        /// <summary>
        /// Seleciona cada RU e faz o seu processamento.
        /// </summary>
        private static async Task Process ()
        {
            foreach (Database.RU ru in Static.Global.RUs_List)
            {
                //  > Carrega a página Web do dia.
                Runtime.Web.Page _page = new (ru.URL);

            }
        }

        /// <summary>
        /// Envia a atualização do que tem hoje no RU.
        /// </summary>
        /// <returns></returns>
        public static async Task Call ()
        {
            await Process ();
        }
    }
}
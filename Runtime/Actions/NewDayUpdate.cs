namespace App.Runtime.Actions
{
    /// <summary>
    /// Eventos que devem ser executados quando um novo dia começa.
    /// </summary>
    internal static class NewDayUpdate
    {
        /// <summary>
        /// Chama os eventos que devem ser executados quando um novo dia for iniciado.
        /// </summary>
        public static Task EventCall ()
        {
            //  > Verifica se é um novo dia.
            if (DateTime.Now.ToUniversalTime().AddHours(-3).Date == Static.Global.LastDayUpdate.Date)
                return Task.CompletedTask;
            //  > Sendo um novo dia, atualiza o dia.
            Static.Global.LastDayUpdate = DateTime.Now.ToUniversalTime().AddHours(-3).Date;

            //  > Chama os eventos que devem ser executados em um novo dia.
            //  - Carrega os dados do site do RU e informa no discord.
            _ = Bot.Events.Messages.RUToday.Call();

            return Task.CompletedTask;
        }
    }
}
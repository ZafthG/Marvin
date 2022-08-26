namespace App.Runtime.Events
{
    /// <summary>
    /// Eventos que devem ser executados quando um novo dia começa.
    /// </summary>
    internal static class NewDayUpdate
    {
        /// <summary>
        /// Chama os eventos que devem ser executados quando um novo dia for iniciado.
        /// </summary>
        public async static Task EventCall ()
        {
            if (DateTime.Now.ToUniversalTime().AddHours(-3).Date == Static.Global.LastDayUpdate.Date)
                return;

            Static.Global.LastDayUpdate = DateTime.Now.ToUniversalTime().AddHours(-3).Date;

            Console.WriteLine($"{DateTime.Now.ToUniversalTime().AddHours(-3).Date} == {Static.Global.LastDayUpdate.Date}");

            await Bot.Events.Messages.RUToday.Call();

            return;
        }
    }
}
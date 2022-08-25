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
        public static Task EventCall ()
        {
            if (DateTime.Now.ToUniversalTime().AddHours(-3).Date != Static.Global.Today.Date)
                return Task.CompletedTask;

            Static.Global.Today = DateTime.Now.ToUniversalTime().AddHours(-3).Date;

            Console.WriteLine($"{DateTime.Now.ToUniversalTime().AddHours(-3).Date} == {Static.Global.Today.Date}");

            return Task.CompletedTask;
        }
    }
}
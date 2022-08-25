namespace App
{
    /// <summary>
    /// Estrutura de entrada do programa.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Método de chamada do programa. A partir dele que o Sistema Operacional
        /// chamará o aplicativo servidor do Marvin.
        /// 
        /// Ele irá chamar os demais métodos, não sendo chamado por nenhum a não ser
        /// o próprio Sistema Operacional.
        /// </summary>
        static void Main() => Static.Boot.Run().Wait();
    }
}
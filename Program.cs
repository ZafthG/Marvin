// ------------------------------------------------------------------------------------------------------------ //
//                                                  class   Program.cs                                          //
// ------------------------------------------------------------------------------------------------------------ //
//      Ponto de presença do método de entrada do programa 'Program.(void) Main()'.                             //
//      Como o código é executado de maneira assíncrona a unica função de entrada é chamar a inicialização      //
//  linear assíncrona a partir do método 'Static.Boot.(Task) Run()'. Isso garantirá a inicialização linear      //
//  do código e dará inicio ao processo paralelo e execução orientada a objetos com uma árvore de cadeira       //
//  multiprocessual.                                                                                            //
//                                                                                                              //
//      Sendo o programa supostamente esperado como leve, e não sendo necessário um agrupamento denso de        //
//  atividades em paralelos, o programa irá processer com apenas uma Thread execudando funções em cadeia        //
//  paralela com o uso de Tasks.                                                                                //
// ------------------------------------------------------------------------------------------------------------ //
//  Por Gabriel Ferreira / Zafth                                                                                //
//  Github: https://github.com/ZafthG                                                                           //
// ------------------------------------------------------------------------------------------------------------ //
namespace App
{ 
    /// <summary>
    /// Entrada do sistema.
    /// É uma estrutura interna que não deve ser chamada a não ser pelo próprio sistema operacional.
    /// </summary>
    internal sealed class Program
    {
        /// <summary>
        /// Método de chamada do programa. A partir dele que o Sistema Operacional
        /// chamará o aplicativo servidor do Marvin.
        /// 
        /// Ele irá chamar os demais métodos, não sendo chamado por nenhum a não ser
        /// o próprio Sistema Operacional.
        /// </summary>
        private static void Main() => Static.Boot.Run().Wait();
    }
}
// ------------------------------------------------------------------------------------------------------------ //
//                                              class   Boot.cs                                                 //
// ------------------------------------------------------------------------------------------------------------ //
//          Responsável pela inicialização do programa em um ambiente assíncrono. É inicializado a partir       //
//  da estrutura Boot.(Task) Run() que inicializa a conexão, carrega as configurações e o banco de dados        //
//  e, por fim, inicia as operações Runtime.                                                                    //
//                                                                                                              //
//          A seguinte ordem de execuções é projetada para a situação:                                          //
//      1.  Carrega as configurações locais a partir do arquivo 'bot.conf' que deve estar presente em seu       //
//  diretório raiz.                                                                                             //
//      2.0.    Conecta o bot ao servidor do Discord Apps via Token.                                            //
//      2.1.    Abre a conexão com o MySQL.                                                                     //
//
using App.Utilits;
namespace App.Static
{
    /// <summary>
    /// Essa delegate determina grupos assíncronos de eventos que devem ser executados ou validados.
    /// </summary>
    delegate Task LoopItem ();

    /// <summary>
    /// Inicializa as operações do servidor em um ambiente assimétrico.
    /// </summary>
    internal static class Boot
    {
        /// <summary>
        /// Roda o sistema de maneira assíncrona.
        /// </summary>
        /// <returns>Retorna ao fechar o serviço.</returns>
        public static async Task Run()
        {
            //  > Seguindo o descrito na introdução do script.
            //  1. Carrega as configurações locais.
            Log.WriteLine(Log.Type.System, "Carregando arquivo 'bot.conf' . . .");
            await Settings.Load();

            //  > Inicializa os eventos em tempo de execução.
            Global.Verify += Runtime.Actions.NewDayUpdate.EventCall;

            //  > Conecta o Marvin ao Discord.
            Log.WriteLine(Log.Type.System, "Conectando 'Marvin' ao Discord . . .");
            await Global.Marvin.Login(Environment.GetEnvironmentVariable((string) Settings.Setting["E_V_MARVIN_TK"]));

            //  > Inicializa o Marvin.
            //  - Carrega a lista de RUs da UFPR.
            Log.WriteLine(Log.Type.System, "Carregando lista de RUs . . .");
            Global.RUs_List = await new Database.RU().LoadAll();

            //  > Dá alguns segundos pro bot inicializar.
            await Task.Delay(5000);

            //  > Carrega as guilds.
            Log.WriteLine(Log.Type.System, "Carregando guilds . . . ");
            await Global.Marvin.LoadGuildsFrom();

            //  > Inicia as operações de Runtime.
            while (true)
            {
                //  O Verify NUNCA vai ser nulo.
                _ = Global.Verify();

                if (Global.Execute == null)
                {
                    await Task.Delay(50);
                    continue;
                }

                await Global.Execute();
            }
        }
    }
}
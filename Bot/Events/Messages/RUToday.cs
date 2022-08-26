using Discord;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text.RegularExpressions;
namespace App.Bot.Events.Messages
{
    internal static class RUToday
    {
        /// <summary>
        /// Adquire as datas e os indices de a qual tabela elas se referem respectivamente.
        /// </summary>
        /// <param name="references">Referências para consulta.</param>
        /// <returns>Retorna uma lista com o index e a data.</returns>
        private static Task <Dictionary<DateTime, int>> GetDate(HtmlNode[] references)
        {
            //  > Valor para retorno.
            Dictionary<DateTime, int> _return = new ();

            //  > Lista os elementos com o index e captura os que desejamos.
            for (int i = 0; i < references.Length; i++)
            {
                //  > Seleciona apenas os textos com data.
                Match _match = Regex.Match(references[i].InnerHtml, @"([0-9][0-9]\/[0-9][0-9]\/[0-9][0-9][0-9][0-9])");
                if (!_match.Success)
                {
                    _match = Regex.Match(references[i].InnerHtml, @"([0-9][0-9]\/[0-9][0-9]\/[0-9][0-9])");
                    if (!_match.Success)
                        continue;
                }
                //  > Adiciona a data a lista de resultados para retorno.
                _return.Add(
                    DateTime.Parse(_match.Value).ToUniversalTime().AddHours(-3).Date,
                    i
                    );

                Console.WriteLine($"{i} = {DateTime.Parse(_match.Value).ToUniversalTime().AddHours(-3).Date}");

            }

            //  > Finaliza.
            return Task.FromResult(_return);
        }
        /// <summary>
        /// Seleciona cada RU e faz o seu processamento.
        /// </summary>
        private static async Task Process ()
        {
            //  > Cria a estrutura de requisição web.
            HtmlWeb _webRequest = new ();

            //  > Executa um analise em cada um dos RU.
            foreach (Database.RU ru in Static.Global.RUs_List)
            {
                Console.WriteLine(ru.Name);
                //  > Carrega a página HTML.
                HtmlDocument _doc = await _webRequest.LoadFromWebAsync(ru.URL);

                //  > Pega as datas desejadas e os indexes a partir dos sites do RU.
                Dictionary<DateTime, int> _datesIndex = await GetDate(
                    _doc.DocumentNode.QuerySelectorAll("#post div p").ToArray()
                    ); ;

                //  > Pega as tabelas da página do RU para separar.
                //  Esse meio é estrutural, feito na "gambiarra" pra pegar o texto
                //  dos cardápios do RU.
                HtmlNode[] _tables = _doc.DocumentNode.QuerySelectorAll("#post div table").ToArray() ;

                //  > Verificação de segurança.
                if (_datesIndex.Count() != _tables.Length)
                    Utilits.Log.WriteLine(Utilits.Log.Type.Error, $"O número de indíces e de tabelas é distinto para {ru.URL}");

                //  > Pega a tabela do dia.
                foreach (KeyValuePair<DateTime, int> date in _datesIndex)
                {
                    //  > Isso indicaria um overflow do índice da matrix de tabelas.
                    if (date.Value >= _tables.Length) break;
                    //  > Pega as linhas da tabela.
                    HtmlNode[] _lines = _tables[date.Value].QuerySelectorAll("tbody tr").ToArray();
                    //  > Processa cada linha individualmente.
                    for (int i = 0; i < _lines.Length; i += 2)
                    {
                        Console.WriteLine(_lines[i].InnerHtml);
                    }
                }
                
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
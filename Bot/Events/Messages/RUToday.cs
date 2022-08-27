using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text.RegularExpressions;
namespace App.Bot.Events.Messages
{
    internal static class RUToday
    {
        /// <summary>
        /// Informa o numero de erros de callback do código. Mais que 3 é crash!
        /// </summary>
        private static int errors = 0;

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
                //  > Informa no console.
                Utilits.Log.WriteLine(Utilits.Log.Type.System, $"Carregando informações de {ru.URL} [{ru.Name}].");
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
                if (_datesIndex.Count != _tables.Length)
                    Utilits.Log.WriteLine(Utilits.Log.Type.Error, $"O número de indíces e de tabelas é distinto para {ru.URL}");

                //  Reseta a lista de cardápio dos próximos dias dessa instância de RU.
                ru.NextDays = new ();
                //  Declara o menu do dia como nulo.
                ru.TodayMenu = null;
                //  Reseta a lista de RUs para Refresh.
                Runtime.Events.RURefresh.RefreshRUs = new ();
                //  Informa se hoje esta na lista de análise.
                bool OkToday = false;

                //  > Pega a tabela do dia.
                foreach (KeyValuePair<DateTime, int> date in _datesIndex)
                {
                    //  > Ignora menus anteriores a hoje.
                    if (date.Key.Date.Ticks < DateTime.Now.ToUniversalTime().AddHours(-3).Date.Ticks)
                        continue;
                    //  > Isso indicaria um overflow do índice da matrix de tabelas.
                    if (date.Value >= _tables.Length) break;
                    //  > Evita o bug de exclusão para dias sem cardápio.
                    if (date.Key.Date == DateTime.Now.ToUniversalTime().AddHours(-3).Date)
                        OkToday = true;
                    //  > Pega as linhas da tabela.
                    HtmlNode[] _lines = _tables[date.Value].QuerySelectorAll("tbody tr").ToArray();

                    //  > Elemento que representa o menu.
                    Database.Menu _menu = new (date.Key);
                    //  > Processa cada linha individualmente.
                    for (int i = 0; i < _lines.Length; i += 2)
                    {
                        //  > Remove lixo e organiza a TAB name que referência o tipo de menu
                        //  com o qual estaremos lidando.
                        string _menuIndex = _lines[i].InnerHtml.Replace("<td>", "")
                            .Replace("<strong>", "")
                            .Replace("</strong>", "")
                            .Replace("</td>", "")
                            .Trim()
                            .ToUpper()
                            .Replace(" ", "_")
                            .Replace("Á", "A")
                            .Replace("É", "E")
                            .Replace("Ã", "A")
                            .Replace("Ç", "C");

                        //  > Organiza o elemento posterior da tabela, removendo dados desnecessários.
                        int _n = i + 1;
                        string _content = Regex.Replace(_lines[_n].InnerHtml, @"<td([^>]*)>", "");
                        _content = Regex.Replace(_content, @"<\/td([^>]*)>", "");
                        _content = Regex.Replace(_content, @"<strong([^>]*)>", "");
                        _content = Regex.Replace(_content, @"<\/strong([^>]*)>", "");
                        _content = Regex.Replace(_content, @"<img([^>]*[^\/])>", "");
                        _content = Regex.Replace(_content, @"<a([^>]*)>", "");
                        _content = Regex.Replace(_content, @"<\/a([^>]*)>", "");
                        _content = Regex.Replace(_content, @"<br>", "\n");
                        //  > Seleciona as comidas dentre o restante.
                        string[] _reference = _content.Split('\n');

                        //  > Constroí o menu com base na tag anterior.
                        if (_menuIndex == "CAFE_DA_MANHA")
                            _menu.Breakfast = new Database.FoodsMenu(_reference);
                        else if (_menuIndex == "ALMOCO")
                            _menu.Lunch = new Database.FoodsMenu(_reference);
                        else if (_menuIndex == "JANTAR")
                            _menu.Dinner = new Database.FoodsMenu(_reference);
                        else
                            Utilits.Log.WriteLine(Utilits.Log.Type.Waring, $"Falha ao identificar um cardápo no RU: \n" +
                                $"[{_menuIndex}]\n{_content}\n- - # - # - -");
                    }

                    //  > Verifica se o menu refere-se ao dia de hoje ou se é de outro dia.
                    if (DateTime.Now.ToUniversalTime().AddHours(-3).Date == date.Key.Date)
                        ru.TodayMenu = _menu;
                    else
                        ru.NextDays.Add(date.Key.Date, _menu);
                }

                //  > Verifica se o menu do dia foi identificado. Caso contrário exibe um erro
                //  e da um ReCall no programa após 1 hora.
                if ((ru.TodayMenu == null) && OkToday)
                {
                    Utilits.Log.WriteLine(Utilits.Log.Type.Error, $"Menu do dia para {ru.Tag}[{ru.ID}] não foi encontrado! Tentando novamente em 30 minutos.");
                    await Task.Delay(new TimeSpan(0, 30, 0));
                    if (errors <= 3)
                        await Call();
                    errors++;
                }
                else if ((ru.TodayMenu != null ))
                    _ = ru.TodayMenu.Save();

                //  > Verifica se foi validado os menus dos próximos dias.
                if (ru.NextDays.Count <= 0)
                {
                    Runtime.Events.RURefresh.RefreshRUs.Add(ru);
                    Runtime.Events.RURefresh.CallMoment = DateTime.Now.ToUniversalTime();
                    Static.Global.Execute += Runtime.Events.RURefresh.Call;
                }
            }
        }

        /// <summary>
        /// Envia a atualização do que tem hoje no RU.
        /// </summary>
        /// <returns></returns>
        public static async Task Call ()
        {
            //  > Carrega os menus dos sites do RU.
            Utilits.Log.WriteLine(Utilits.Log.Type.System, "Atualizando informações de cardápio do RU . . .");
            await Process ();

            //  > Envia para o discord.
            foreach (Database.RU ru in Static.Global.RUs_List)
                _ = ru.SendRUToday();
        }
    }
}
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
namespace App.Runtime.Web
{
    /// <summary>
    /// Estrutura base para uma página carregada da Internet.
    /// </summary>
    internal struct Page
    {
        /// <summary>
        /// URL da página.
        /// </summary>
        public string URL { get; private set; }
        /// <summary>
        /// Conteúdo da página carregada.
        /// </summary>
        public HtmlDocument DOC { get; private set; }

        /// <summary>
        /// Carrega uma página da internet.
        /// </summary>
        private async Task Load ()
        {
            HtmlWeb _web = new ();
            DOC = await _web.LoadFromWebAsync(URL);
        }

        /// <summary>
        /// Representa uma página Web carregada da internet.
        /// </summary>
        /// <param name="url">O link da página que será carregada.</param>
        public Page (string url)
        {
            URL = url;
            DOC = new HtmlDocument ();

            Load().Wait();            
        }
    }
}
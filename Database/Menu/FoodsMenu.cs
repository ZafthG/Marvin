using System.Text;
using System.Text.RegularExpressions;
namespace App.Database
{
    /// <summary>
    /// Representa um menu de café da manhã.
    /// </summary>
    internal class FoodsMenu
    {
        /// <summary>
        /// Lista de comidas do cardápio.
        /// </summary>
        public List<Food> Foods { get; private set; }

        /// <summary>
        /// Retorna a lista de comidas organizada para impressão.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //  > Construtor para a cadeia de caracteres.
            StringBuilder _const = new ();

            //  > Constroí a cadeia de caracteres.
            foreach (Food food in Foods)
            {
                _const.Append($"\t- {food.Name}\n");
            }

            //  > Retorna a cadeia montada.
            return _const.ToString();
        }

        /// <summary>
        /// Instância um novo cardápio de café da manhã.
        /// </summary>
        /// <param name="reference">Referência de texto para processamento.</param>
        public FoodsMenu(string[] reference)
        {
            //  > Inicializa a lista de comidas.
            Foods = new();

            //  > Instância as comidas do cardápio.
            foreach (string food in reference)
            {
                //  > Organiza a cadeia de caracteres em uma normalização simples.
                string _food = food.Trim().Replace(" ", ".");
                _food = Regex.Replace(_food, @"\.{2,}", "");
                _food = _food.Replace(".", " ");
                //  > Ignora cadeias vazias.
                if (string.IsNullOrEmpty(_food) || string.IsNullOrWhiteSpace(_food))
                    continue;

                //  > Adiciona o item do menu a lista.
                Foods.Add(new Food(food.Trim()));
            }
        }
    }
}
using System.Text;
namespace App.Database
{
    /// <summary>
    /// Representa um menu de itens de alimentação.
    /// </summary>
    internal class Menu
    {
        /// <summary>
        /// Representa o ID do menu no banco de dados.
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Representa o dia no qual o menu será ou foi servido.
        /// </summary>
        public DateTime Date { get; private set; }
        /// <summary>
        /// Representa o quê há de café da manhã nesse dia.
        /// </summary>
        public FoodsMenu? Breakfast { get; set; }
        /// <summary>
        /// Representa o quê há de almoço nesse dia.
        /// </summary>
        public FoodsMenu? Lunch { get; set; }
        /// <summary>
        /// Representa o quê há de jantar nesse dia.
        /// </summary>
        public FoodsMenu? Dinner { get; set; }

        /// <summary>
        /// Salva esse respectivo menu no banco de dados.
        /// </summary>
        public async Task Save()
        {

        }

        /// <summary>
        /// Constroí o menu em formato organizado.
        /// </summary>
        /// <returns>Retorna o menu em string.</returns>
        public override string ToString()
        {
            //  > Construtor para a string de saída.
            StringBuilder _builder = new ();

            //  > Informa a data da comida.
            _builder.Append($"***{Date.ToString("dd/MM/yyyy")} - {Date.ToString("dddd").ToUpperInvariant()}***\n");
            //  > Adiciona as comidas do café da manhã.
            if (Breakfast != null)
            {
                _builder.Append($"\n**CAFÉ DA MANHÃ**\n");
                _builder.Append(Breakfast.ToString());
            }
            //  > Adiciona as comidas do almoço.
            if (Lunch != null)
            {
                _builder.Append($"\n**ALMOÇO**\n");
                _builder.Append(Lunch.ToString());
                _builder.Append("");
            }
            //  > Adiciona as comidas do jantar.
            if (Dinner != null)
            {
                _builder.Append($"\n**JANTAR**\n");
                _builder.Append(Dinner.ToString());
                _builder.Append("");
            }

            //  > Retorna a string construída.
            return _builder.ToString();
        }

        /// <summary>
        /// Instância um novo menu para um dia em específico.
        /// </summary>
        /// <param name="date">Referencia a data para a qual o menu indica.</param>
        /// <param name="id">Representa o id do menu no banco de dados.</param>
        public Menu (DateTime date, int id = -1)
        {
            Id = id;
            Date = date.Date;
        }
    }
}
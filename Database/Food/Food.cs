using MySql.Data.MySqlClient;
namespace App.Database
{
    /// <summary>
    /// Estrutura representativa de uma comida.
    /// </summary>
    internal class Food : MySQLBase
    {
        /// <summary>
        /// Indica a soma das caracteristicas da comida.
        /// </summary>
        public readonly int features = 0;

        /// <summary>
        /// ID da comida no banco de dados.
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// Nome da comida no banco de dados (e que será exibido).
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Essa comida é vegana?
        /// </summary>
        public bool IsVegan { get; private set; }

        /// <summary>
        /// Essa comida tem leite?
        /// </summary>
        public bool HasMilk { get; private set; }
        /// <summary>
        /// Tem produtos alergênicos?
        /// </summary>
        public bool HasAllergenicProducts { get; private set; }
        /// <summary>
        /// Essa comida tem gluten?
        /// </summary>
        public bool HaveGluten { get; private set; }
        /// <summary>
        /// Tem produtos de origem animal?
        /// </summary>
        public bool HaveAnimalProducts { get; private set; }
        /// <summary>
        /// Tem ovos?
        /// </summary>
        public bool HaveEggs { get; private set; }
        /// <summary>
        /// Tem mel?
        /// </summary>
        public bool HaveHoney { get; private set; }
        /// <summary>
        /// Tem pimenta?
        /// </summary>
        public bool HavePepper { get; private set;  }

        /// <summary>
        /// Organiza o formato de impressão do modelo.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{ID}] {Name} - <{features}>";
        }

        /// <summary>
        /// A partir da NameTag consulta o banco de dados para capturar a respectiva comida.
        /// </summary>
        /// <param name="name"></param>
        public Food (string name)
        {
            //  > Inicializa as variáveis pra IDE parar de encher o saco.
            ID = -1;
            Name = "";

            IsVegan = false;
            HasMilk = false;
            HasAllergenicProducts = false;
            HaveGluten = false;
            HaveAnimalProducts = false;
            HaveEggs = false;
            HaveHoney = false;
            HavePepper = false;

            //  > Cadeia de comando SQL.
            string _sqlCommand = $"SELECT * FROM food WHERE name='{name}'";

            //  > Tenta estabelecer uma conexão MySQL e realizar a consulta.
            int tries = 0;
            while (tries <= Static.Settings.GetMySql_ConnTries)
            {
                try
                {
                    //  Abre a conexão MySQL.
                    Task<MySqlConnection> _open = Open();

                    //  > Estrutura de comando atrelada a conexão MySQL.
                    MySqlCommand _command = new(_sqlCommand, _open.Result);
                    Task.Delay(100).Wait();

                    //  > Executa a consulta.
                    MySqlDataReader _reader = _command.ExecuteReader();
                    if (_reader.Read())
                    {
                        ID = _reader.GetInt32("id");
                        Name = _reader.GetString("name");
                        features = _reader.GetInt32("features");

                        Utilits.Log.WriteLine(Utilits.Log.Type.Database, $"[DB:SELECT]{this}");

                        //  > Fecha o leitor
                        _reader.Close();
                    }
                    else
                    {
                        Utilits.Log.WriteLine(Utilits.Log.Type.Waring, 
                            $"A comida [{name}] não foi localizada no banco de dados!");

                        //  > Fecha o leitor.
                        _reader.Close();
                        _command.Dispose();

                        //  > Como não existe, cria.
                        _sqlCommand = $"INSERT INTO food (id, name, features) VALUES (NULL, '{name}', 0);";
                        _command.CommandText = _sqlCommand;

                        //  > Executa.
                        if (_command.ExecuteNonQuery() != 0)
                        {
                            Utilits.Log.WriteLine(Utilits.Log.Type.Database,
                                $"[DB:INSERT INTO food] name = {name}");

                            //  > Recarrega os dados da comida.
                            Food _food = new Food(name);
                            ID = _food.ID;
                            Name = _food.Name;
                            features = _food.features;
                        }

                    }
                    //  > Interrompe o laço de tentativas.
                    break;
                }
                catch (Exception error)
                {
                    tries++;
                    Utilits.Log.WriteLine(Utilits.Log.Type.Error,
                        $"Falha ao tentar estabelecer uma conexão MySQL ({tries}/{Static.Settings.GetMySql_ConnTries}): \n" +
                        $"{error.Message}\nNova tentativa em {4 + 3 * (tries / 2)} segundos.");
                    Task.Delay((4 + 3 * (tries / 2)) * 1000).Wait();
                }
                finally
                {
                    Close();
                }
            }

            //  > Separa dos features as condicionais.


            //  > Declara como vegano se ..
            if (!(HasMilk) && !(HaveEggs) && !(HaveAnimalProducts) && !(HaveHoney)) IsVegan = true;
        }
    }
}
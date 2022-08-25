namespace App.Utilits.Files
{
    /// <summary>
    /// Estrutura responsável por ler um arquivo externo.
    /// </summary>
    internal class Reader
    {
        /// <summary>
        /// Caminho até o arquivo para leitura.
        /// </summary>
        private readonly string filePath = "";

        /// <summary>
        /// Faz a leitura dos bytes do arquivo.
        /// </summary>
        /// <returns>Retorna a cadeia de bytes lidos.</returns>
        public async Task<byte[]> Read()
        {
            try
            {
                //  > Cria o Stream responsável por ler o arquivo.
                //  Perceba que o stream é criado apenas para abrir o arquivo em modo leitura!
                //  Ele não pode fazer alterações ou criar arquivos.
                using FileStream _file_stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                //  > Cria um buffer para armazenar os bytes lidos com o tamanho do arquivo.
                byte[] _buffer = new byte[_file_stream.Length];

                //  > Faz a leitura.
                _ = await _file_stream.ReadAsync(_buffer);

                //  > Finaliza.
                _file_stream.Close();
                return _buffer;
            }
            catch (IOException error)
            {
                throw new IOException($"Falha na leitura do arquivo '{filePath}':\n{error.Message}.", error);
            }
        }

        /// <summary>
        /// Inicializa uma estrutura de leitura.
        /// </summary>
        /// <param name="filename">Nome completo do arquivo (com extensão).</param>
        /// <param name="directory">Diretório do arquivo (caso não informado, considera o diretório do programa).</param>
        public Reader(string filename, string directory = "<STD>")
        {
            //  > Verifica os valores para 'filename' e 'directory' a fim de evitar problemas futuros.
            //  caso um erro for ocorrer, esse erro já é logo tratado para evitar a interrupção brusca
            //  do programa.
            if (string.IsNullOrEmpty(filename) || string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename), "Falha na leitura de um arquivo de nome nulo.");
            if (string.IsNullOrEmpty(directory) || string.IsNullOrWhiteSpace(directory))
                throw new ArgumentNullException(nameof(directory), 
                    $"Falha na leitura do arquivo '{filename}': Nome de diretório nulo.");
            if (directory == "<STD>") directory = Directory.GetCurrentDirectory();

            //  > Verifica a existência do diretório.
            if (!Directory.Exists(directory))
                throw new ArgumentException($"O diretório '{directory}' não existe.", nameof(directory));

            //  > Monta o caminho até o arquivo que será lido.
            filePath = Path.Combine(directory, filename);

            //  > Verifica a existência do arquivo.
            if (!File.Exists(filePath))
                throw new ArgumentException($"O arquivo '{filename}' não existe em '{directory}'.", nameof(directory));
        }
    }
}
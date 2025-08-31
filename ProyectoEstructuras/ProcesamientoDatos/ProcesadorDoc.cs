using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Utilidades;

namespace BuscadorIndiceInvertido.ProcesamientoDatos
{
    internal class ProcesadorDoc
    {
        private string url { get; set; }
        private Tokenizer tokenizer;
        private StopWordsFiltro filtroSW;

        public ProcesadorDoc()
        {
            url = "";
            tokenizer = new Tokenizer();
            filtroSW = new StopWordsFiltro();
        }

        public DoubleList<Doc> ProcesarDocumentos(string url)
        {
            DoubleList<Doc> docs = new DoubleList<Doc>();

            foreach (var file in Directory.GetFiles(url, "*.txt"))
            {
                string contenido = File.ReadAllText(file);

                DoubleList<string> tokens = tokenizer.TokenizeTexto(contenido);

                string[] tokenArr = new string[tokens.Count];
                tokens.CopyTo(tokenArr, 0);

                DoubleList<string> tokensFiltrados = filtroSW.FiltrarStopWords(tokenArr);

                docs.Add(new Doc(Path.GetFileName(file), tokensFiltrados));
            }

            return docs;
        }
    }
}

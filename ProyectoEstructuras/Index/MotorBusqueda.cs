using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Ordenamientos;
using BuscadorIndiceInvertido.ProcesamientoDatos;
using BuscadorIndiceInvertido.Strategies;
using BuscadorIndiceInvertido.Utilidades;

namespace BuscadorIndiceInvertido.Index
{
    internal class MotorBusqueda
    {
        private IndiceInvertido indice;
        private ProcesadorQuery procesadorQuery;
        private SimilitudCosenoStrategy procesadorVector;
        private Rankeador rankeador;

        public MotorBusqueda(IndiceInvertido indice)
        {
            this.indice = indice;
            procesadorQuery = new ProcesadorQuery();
            procesadorVector = new SimilitudCosenoStrategy();
            rankeador = new Rankeador();
        }

        public DoubleList<(Doc doc, double score)> Buscar(string query, int topN = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new DoubleList<(Doc doc, double score)>();

            DoubleList<string> queryTokens = procesadorQuery.ProcesarQuery(query);

            if (queryTokens.Count == 0)
                return new DoubleList<(Doc doc, double score)>();

            // calcular scores
            DoubleList<(Doc doc, double score)> resultados = procesadorVector.CalcularScores(queryTokens, indice);

            // ordenar resultados descendente, es ascendente?
            resultados = rankeador.OrdenarResultados(resultados);

            return LimitarResultados(resultados, topN);
        }

        public void IniciarInterfazUsuario()
        {
            while (true)
            {
                Console.WriteLine("Ingrese la consulta ('salir' para terminar):");
                Console.Write("> ");

                string query = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(query))
                    continue;

                if (query.ToLower().Trim() == "salir")
                    break;

                // buscar
                var resultados = Buscar(query);

                MostrarResultados(resultados, query);
                Console.WriteLine();
            }

            Console.WriteLine("Búsqueda terminada");
        }

        private void MostrarResultados(DoubleList<(Doc doc, double score)> resultados, string query)
        {
            Console.WriteLine($"\nResultados para: \"{query}\"");
            Console.WriteLine(new string('-', 60));

            if (resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron documentos relevantes.");
                return;
            }

            int posicion = 1;
            foreach (var (doc, score) in resultados)
            {
                Console.WriteLine($"{posicion}. {doc.FileName}");
                Console.WriteLine($"   Puntaje: {score:F4}");
                Console.WriteLine();
                posicion++;
            }
        }

        private DoubleList<(Doc doc, double score)> LimitarResultados(DoubleList<(Doc doc, double score)> resultados, int topN)
        {
            if (resultados.Count <= topN)
                return resultados;

            var resultadosLimitados = new DoubleList<(Doc doc, double score)>();
            int contador = 0;

            foreach (var resultado in resultados)
            {
                if (contador >= topN) break;
                resultadosLimitados.Add(resultado);
                contador++;
            }

            return resultadosLimitados;
        }
    }
}

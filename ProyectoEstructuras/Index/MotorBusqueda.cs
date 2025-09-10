using System.Runtime.CompilerServices;
using System.Text;
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
            DoubleList<(Doc doc, double score)> resultados = procesadorVector.CalcularRanking(queryTokens, indice);

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

                string query = Console.ReadLine()?.ToLower().Trim();

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
                string base64Name = doc.FileName.Replace(".txt", "");
                string url = DecodificarBase64(base64Name);

                Console.WriteLine($"{posicion}. {url}");
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

        private string DecodificarBase64(string textoBase64)
        {
            // Tabla de conversión rápida: cada carácter ASCII -> valor en Base64
            int[] tabla = new int[256];
            for (int i = 0; i < tabla.Length; i++) tabla[i] = -1;

            string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            for (int i = 0; i < caracteres.Length; i++)
                tabla[caracteres[i]] = i;

            // Lista doble para acumular los bytes resultantes
            var bytes = new DoubleList<byte>();

            // Eliminar el relleno '=' al final
            textoBase64 = textoBase64.TrimEnd('=');

            int acumulador = 0;
            int bits = 0;

            foreach (char c in textoBase64)
            {
                int valor = tabla[c];
                if (valor < 0)
                    throw new ArgumentException("Carácter inválido en Base64");

                acumulador = (acumulador << 6) | valor;
                bits += 6;

                if (bits >= 8)
                {
                    bits -= 8;
                    bytes.Add((byte)((acumulador >> bits) & 0xFF));
                }
            }

            // Pasar de DoubleList<byte> a arreglo de bytes
            byte[] arreglo = new byte[bytes.Count];
            int indice = 0;
            foreach (var b in bytes)
                arreglo[indice++] = b;

            // Convertir los bytes a texto
            return Encoding.UTF8.GetString(arreglo);
        }
    }
}

using System.Text;
using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Utilidades;
using System.Text.RegularExpressions;

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
        
        public DoubleList<Doc> ProcesarDocumentos(string ruta)
        {
            var docs = new DoubleList<Doc>();

            if (!Directory.Exists(ruta))
            {
                Console.WriteLine($"La ruta {ruta} no existe.");
                return docs;
            }

            var archivos = Directory.GetFiles(ruta, "*.txt");
            Console.WriteLine($"Archivos encontrados: {archivos.Length}");

            foreach (var archivo in archivos)
            {
                try
                {
                    string contenido;
                    contenido = File.ReadAllText(archivo);;
                    if (string.IsNullOrWhiteSpace(contenido) || contenido.Contains("\0"))
                    {
                        Console.WriteLine($" {archivo} no contiene texto válido, se omite.");
                        continue;
                    }
                    var tokens = tokenizer.TokenizeTexto(contenido);
                    var tokensFiltrados = filtroSW.FiltrarStopWords(tokens);
                    
                    docs.Add(new Doc(Path.GetFileName(archivo), tokensFiltrados));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR al procesar {archivo}: {e.GetType().Name} - {e.Message}");
                    continue;
                }
            }

            Console.WriteLine($"Documentos procesados correctamente: {docs.Count}");
            return docs;
        }
    }
}

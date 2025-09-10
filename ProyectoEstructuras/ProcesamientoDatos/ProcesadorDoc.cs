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

        private bool EsArchivoTextoValido(string path)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                // Verifica si hay bytes nulos (0x00), típico de binarios
                if (bytes.Contains((byte)0))
                    return false;

                // Intentar decodificar como UTF-8
                Encoding.UTF8.GetString(bytes);
                return true;
            }
            catch
            {
                return false;
            }
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
                Console.WriteLine($"Procesando: {Path.GetFileName(archivo)}");

                try
                {
                    string contenido;

                    // Intentar leer en UTF-8, si falla usar codificación por defecto
                    try
                    {
                        contenido = File.ReadAllText(archivo, Encoding.UTF8);
                    }
                    catch
                    {
                        contenido = File.ReadAllText(archivo, Encoding.Default);
                    }

                    // Validar contenido
                    if (string.IsNullOrWhiteSpace(contenido) || contenido.Contains("\0"))
                    {
                        Console.WriteLine($"ADVERTENCIA: {archivo} no contiene texto válido, se omite.");
                        continue;
                    }
                    var tokens = tokenizer.TokenizeTexto(contenido); // DoubleList<string> limpio

                    var tokensFiltrados = filtroSW.FiltrarStopWords(tokens);

// Guardar en el Doc
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

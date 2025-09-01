using BuscadorIndiceInvertido.Index;

namespace BuscadorIndiceInvertido.Persistencia
{

    internal class ArchivoManager
    {
        private readonly string rutaArchivo;

        public ArchivoManager(string rutaArchivo = "indice.dat")
        {
            this.rutaArchivo = rutaArchivo;
        }

        public bool GuardarIndiceBinario(IndiceInvertido indice)
        {
            try
            {
                using var stream = new FileStream(rutaArchivo, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 1048576); // 1MB lista
                using var writer = new BinaryWriter(stream);

                string[] vocabulario = indice.GetVocabulario();
                int palabrasCount = indice.GetPalabrasCount();

                var lista = new List<byte>();

                EscribirInt32Lista(lista, palabrasCount);

                // escribir todo el vocabulario junto
                foreach (string palabra in vocabulario)
                {
                    EscribirStringLista(lista, palabra);
                    EscribirDoubleLista(lista, indice.GetIDF(palabra));
                }

                foreach (string palabra in vocabulario)
                {
                    var postings = indice.GetPostings(palabra);
                    EscribirInt32Lista(lista, postings.Count);

                    foreach (var (doc, freq) in postings)
                    {
                        EscribirStringLista(lista, doc.FileName);
                        EscribirInt32Lista(lista, freq);
                        EscribirInt32Lista(lista, doc.tokens.Count);

                        foreach (string token in doc.tokens)
                        {
                            EscribirStringLista(lista, token);
                        }
                    }
                }

                // escribir toda la lista de una vez
                writer.Write(lista.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el índice: {ex.Message}");
                return false;
            }
        }

        private void EscribirInt32Lista(List<byte> lista, int valor)
        {
            lista.AddRange(BitConverter.GetBytes(valor));
        }

        private void EscribirStringLista(List<byte> lista, string valor)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(valor);
            EscribirInt32Lista(lista, bytes.Length);
            lista.AddRange(bytes);
        }

        private void EscribirDoubleLista(List<byte> lista, double valor)
        {
            lista.AddRange(BitConverter.GetBytes(valor));
        }
    }
}
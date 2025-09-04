using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Index;
using BuscadorIndiceInvertido.Utilidades;

namespace BuscadorIndiceInvertido.Persistencia
{
    internal class DocumentoUnico
    {
        public int id;
        public string archivo;
        public DoubleList<string> tokens;

        public DocumentoUnico(int id, string archivo, DoubleList<string> tokens)
        {
            this.id = id;
            this.archivo = archivo;
            this.tokens = tokens;
        }
    }

    internal class ArchivoManager
    {
        private readonly string rutaArchivo;

        public ArchivoManager(string rutaArchivo = "indice.dat")
        {
            this.rutaArchivo = rutaArchivo;
        }

        public bool GuardarIndice(IndiceInvertido indice)
        {
            try
            {
                using (FileStream stream = new FileStream(rutaArchivo, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 65536))
                using (BinaryWriter writer = new BinaryWriter(stream))
                {

                    string[] vocabulario = indice.GetVocabulario();
                    int palabrasCount = indice.GetContadorPalabras();

                    var documentosUnicos = new DoubleList<DocumentoUnico>();
                    int docId = 0;

                    for (int i = 0; i < palabrasCount; i++)
                    {
                        string palabra = vocabulario[i];
                        var postings = indice.GetPostings(palabra);

                        foreach (var (doc, freq) in postings)
                        {
                            if (!ExisteDocumento(documentosUnicos, doc.FileName))
                            {
                                documentosUnicos.Add(new DocumentoUnico(docId++, doc.FileName, doc.tokens));
                            }
                        }
                    }
                  writer.Write(documentosUnicos.Count);

                    foreach (var docUnico in documentosUnicos)
                    {
                        writer.Write(docUnico.id);          
                        writer.Write(docUnico.archivo);     
                        writer.Write(docUnico.tokens.Count); 

                        foreach (string token in docUnico.tokens)
                        {
                            writer.Write(token);
                        }
                    }

   
                    writer.Write(palabrasCount);

                    for (int i = 0; i < palabrasCount; i++)
                    {
                        string palabra = vocabulario[i];

                        writer.Write(palabra);
                        writer.Write(indice.GetIDF(palabra));

                        var postings = indice.GetPostings(palabra);
                        writer.Write(postings.Count);

                        foreach (var (doc, freq) in postings)
                        {
                            int docIdRef = BuscarIdDocumento(documentosUnicos, doc.FileName);
                            writer.Write(docIdRef);  // solo el ID
                            writer.Write(freq);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el índice: {ex.Message}");
                return false;
            }
        }

        public IndiceInvertido CargarIndice()
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                {
                    Console.WriteLine("El archivo de índice no existe.");
                    return null;
                }

                using (FileStream stream = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 65536))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    var indice = new IndiceInvertido();

         
                    int contadorDocu = reader.ReadInt32();
                    var documentosById = new DoubleList<DocumentoUnico>();

                    for (int i = 0; i < contadorDocu; i++)
                    {
                        int docId = reader.ReadInt32();
                        string fileName = reader.ReadString();
                        int tokensCount = reader.ReadInt32();

                        var tokens = new DoubleList<string>();
                        for (int j = 0; j < tokensCount; j++)
                        {
                            tokens.Add(reader.ReadString());
                        }

                        documentosById.Add(new DocumentoUnico(docId, fileName, tokens));
                    }
                
                    int palabrasCount = reader.ReadInt32();

                    if (palabrasCount == 0)
                    {
                        return indice;
                    }

              
                    var todosLosDocumentos = new DoubleList<Doc>();
                    foreach (var docUnico in documentosById)
                    {
                        todosLosDocumentos.Add(new Doc(docUnico.archivo, docUnico.tokens));
                    }

                    indice.Construir(todosLosDocumentos);

                    return indice;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el índice: {ex.Message}");
                return null;
            }
        }

        private bool ExisteDocumento(DoubleList<DocumentoUnico> documentos, string fileName)
        {
            foreach (var doc in documentos)
            {
                if (doc.archivo.Equals(fileName, StringComparison.Ordinal))
                {
                    return true;
                }
            }
            return false;
        }

        private int BuscarIdDocumento(DoubleList<DocumentoUnico> documentos, string fileName)
        {
            foreach (var doc in documentos)
            {
                if (doc.archivo.Equals(fileName, StringComparison.Ordinal))
                {
                    return doc.id;
                }
            }
            return -1; // no encontrado
        }
    }
}
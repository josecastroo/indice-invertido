using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Index;
using BuscadorIndiceInvertido.Utilidades;

namespace BuscadorIndiceInvertido.Persistencia
{
    internal class DocumentoUnico // Clase que sirve para serializar en este AM
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

                    // Recopilar documentos únicos
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
                            writer.Write(docIdRef);
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

                using (var reader = new BinaryReader(File.OpenRead(rutaArchivo)))
                {
                    int contadorDocu = reader.ReadInt32();
                    var documentosById = new DocumentoUnico[contadorDocu];

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

                        documentosById[docId] = new DocumentoUnico(docId, fileName, tokens);
                    }
                    
                    int palabrasCount = reader.ReadInt32();

                    if (palabrasCount == 0)
                    {
                        return new IndiceInvertido();
                    }
                    
                    string[] vocabulario = new string[palabrasCount];
                    double[] idfValues = new double[palabrasCount];
                    DoubleList<(Doc doc, int freq)>[] matrizPostings = new DoubleList<(Doc doc, int freq)>[palabrasCount];

                    for (int i = 0; i < palabrasCount; i++)
                    {
                        vocabulario[i] = reader.ReadString();
                        idfValues[i] = reader.ReadDouble();

                        int postingsCount = reader.ReadInt32();
                        matrizPostings[i] = new DoubleList<(Doc doc, int freq)>();

                        for (int j = 0; j < postingsCount; j++)
                        {
                            int docIdRef = reader.ReadInt32();
                            int freq = reader.ReadInt32();
                            
                            DocumentoUnico docUnico = documentosById[docIdRef];
                            Doc doc = new Doc(docUnico.archivo, docUnico.tokens);
                            
                            matrizPostings[i].Add((doc, freq));
                        }
                    }
                    
                    var indice = new IndiceInvertido();
                    indice.RestaurarDesdeArchivo(vocabulario, idfValues, matrizPostings);

                    return indice;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el índice: {ex.Message}");
                return null;
            }
        }

        public bool ActualizarIndice(IndiceInvertido indiceExistente, DoubleList<Doc> nuevosDocumentos, double percentil = 0.0)
        {
            //puede ser más eficiente con un get
            try
            {
                var todosLosDocumentos = new DoubleList<Doc>();
                
                string[] vocabulario = indiceExistente.GetVocabulario();
                for (int i = 0; i < vocabulario.Length; i++)
                {
                    var postings = indiceExistente.GetPostings(vocabulario[i]);
                    foreach (var (doc, freq) in postings)
                    {
                        if (!ContieneDocumento(todosLosDocumentos, doc.FileName))
                        {
                            todosLosDocumentos.Add(doc);
                        }
                    }
                }
                
                foreach (Doc nuevoDoc in nuevosDocumentos)
                {
                    if (!ContieneDocumento(todosLosDocumentos, nuevoDoc.FileName))
                    {
                        todosLosDocumentos.Add(nuevoDoc);
                    }
                }
                
                indiceExistente.Build(todosLosDocumentos, percentil);
                
                return GuardarIndice(indiceExistente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el índice: {ex.Message}");
                return false;
            }
        }

        private bool ContieneDocumento(DoubleList<Doc> documentos, string fileName)
        {
            foreach (Doc doc in documentos)
            {
                if (doc.FileName.Equals(fileName, StringComparison.Ordinal))
                {
                    return true;
                }
            }
            return false;
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
            return -1; 
        }
    }
}
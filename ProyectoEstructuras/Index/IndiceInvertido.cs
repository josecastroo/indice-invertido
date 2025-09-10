using System;
using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Calculos;
using BuscadorIndiceInvertido.Utilidades;
using ProyectoEstructuras.SortStrategies;

namespace BuscadorIndiceInvertido.Index
{
    public class IndiceInvertido
    {
        private string[] palabras;
        private double[] IDFValores;
        private DoubleList<(Doc doc, int frec)>[] matrizFrec;
        private int contadorPalabaras;
        private readonly TFIDFCalculador calculador;

        public IndiceInvertido()
        {
            palabras = new string[0]; // Array.Empty<string>();
            IDFValores = new double[0]; // Array.Empty<double>();
            matrizFrec = new DoubleList<(Doc doc, int freq)>[0]; // Array.Empty<DoubleList<(Doc doc, int freq)>>();
            contadorPalabaras = 0;
            calculador = new TFIDFCalculador();
        }   

        public void Build(DoubleList<Doc> docs, double percentil = 0.0)
        {
            if (docs == null || docs.Count == 0) return;

            int docsTotal = docs.Count;

            Zipf zipf = new Zipf();
            string[] palabrasUnicas = zipf.FiltrarVocabulario(docs, percentil);
            
            if (palabrasUnicas == null || palabrasUnicas.Length == 0)
            {
                Console.WriteLine("Advertencia: el vocabulario está vacío. No se construirá índice.");
                return;
            }
            
            IOrdenamiento<string> radixSort = new RadixSort();
            radixSort.Ordenar(palabrasUnicas, 0, palabrasUnicas.Length - 1);

            InicializarAtributos(palabrasUnicas.Length);

            BuildVocab(palabrasUnicas);

            if (palabrasUnicas.Length > 0)
            {
                Doc[] docArr = docs.ToArray();
                BuildMatrizFrec(docArr);
                CalcularIDF(docsTotal);
            }
        }

        private void BuildMatrizFrec(Doc[] arr)
        {
            int totalDocs = arr.Length;
            int[,] tempFrecs = new int[contadorPalabaras, totalDocs];
        
            Console.WriteLine($"contadorPalabaras={contadorPalabaras}, totalDocs={totalDocs}");
        // contar frecuencias
        for (int docIndex = 0; docIndex < totalDocs; docIndex++)
            {
                Doc doc = arr[docIndex];
                foreach (var token in doc.tokens)
                {
                    int wordIndex = Array.BinarySearch(palabras, token, StringComparer.Ordinal);
                    if (wordIndex < 0)
                        continue; 
                    tempFrecs[wordIndex, docIndex]++;
                }
            }

            // pasar a DoubleList
            for (int j = 0; j < contadorPalabaras; j++)
            {
                matrizFrec[j] = new DoubleList<(Doc doc, int freq)>();
                for (int k = 0; k < totalDocs; k++)
                {
                    if (tempFrecs[j, k] > 0)
                    {
                        matrizFrec[j].Add((arr[k], tempFrecs[j, k]));
                    }
                }
            }
        }

        private void CalcularIDF(int totalDocs)
        {
            for (int j = 0; j < contadorPalabaras; j++)
            {
                int df = matrizFrec[j].Count;
                IDFValores[j] = calculador.CalcularIDF(totalDocs, df);
            }
        }

        public void RestaurarDesdeArchivo(string[] vocabulario, double[] idfValues, 
            DoubleList<(Doc doc, int freq)>[] matrizPostings)
        {
            contadorPalabaras = vocabulario.Length;
    
            palabras = new string[contadorPalabaras];
            IDFValores = new double[contadorPalabaras];
            matrizFrec = new DoubleList<(Doc doc, int freq)>[contadorPalabaras];
    
            Array.Copy(vocabulario, palabras, contadorPalabaras);
            Array.Copy(idfValues, IDFValores, contadorPalabaras);
    
            for (int i = 0; i < contadorPalabaras; i++)
            {
                matrizFrec[i] = matrizPostings[i];
            }
        }
        
        public void InicializarVacio(int tamanoVocabulario)
        {
            contadorPalabaras = tamanoVocabulario;
            palabras = new string[tamanoVocabulario];
            IDFValores = new double[tamanoVocabulario];
            matrizFrec = new DoubleList<(Doc doc, int freq)>[tamanoVocabulario];
        }

        private void InicializarAtributos(int Vocabcount)
        {
            palabras = new string[Vocabcount];
            IDFValores = new double[Vocabcount];
            matrizFrec = new DoubleList<(Doc doc, int freq)>[Vocabcount];
            contadorPalabaras = Vocabcount;
        }

        private void BuildVocab(string[] palabrasUnicas)
        {
            for (int i = 0; i < palabrasUnicas.Length; i++)
            {
                palabras[i] = palabrasUnicas[i];
            }
        }
        
        public DoubleList<(Doc doc, int freq)> GetPostings(string palabra)
        {
            int indice = Array.BinarySearch(palabras, palabra, StringComparer.Ordinal);
            if (indice >= 0)
                return matrizFrec[indice];

            return new DoubleList<(Doc doc, int freq)>();
        }

        public double GetIDF(string palabra)
        {
            int indice = Array.BinarySearch(palabras, palabra, StringComparer.Ordinal);
            if (indice >= 0)
                return IDFValores[indice];

            return 0.0;
        }

        public double GetTFIDF(string palabra, int frecuencia)
        {
            double idf = GetIDF(palabra);
            return calculador.CalcularTFIDF(frecuencia, idf);
        }
        public string[] GetVocabulario()
        {
            string[] vocab = new string[contadorPalabaras];
            Array.Copy(palabras, vocab, contadorPalabaras);
            return vocab;
        }

        public int GetContadorPalabras()
        {
            return contadorPalabaras;
        }
        public void SetPalabra(int indice, string palabra, double idf, DoubleList<(Doc doc, int freq)> postings)
        {
            palabras[indice] = palabra;
            IDFValores[indice] = idf;
            matrizFrec[indice] = postings;
        }
    }

}

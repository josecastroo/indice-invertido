using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Calculos;
using BuscadorIndiceInvertido.Utilidades;
using System;

namespace BuscadorIndiceInvertido.Index
{
    internal class IndiceInvertido
    {
        private string[] palabras;
        private double[] IDFValores;
        private DoubleList<(Doc doc, int frec)>[] matrizFrec;
        private int palabrasCount;
        private readonly TFIDFCalculador calculador;

        public IndiceInvertido()
        {
            palabras = new string[0]; // Array.Empty<string>();
            IDFValores = new double[0]; // Array.Empty<double>();
            matrizFrec = new DoubleList<(Doc doc, int freq)>[0]; // Array.Empty<DoubleList<(Doc doc, int freq)>>();
            palabrasCount = 0;
            calculador = new TFIDFCalculador();
        }   

        public void Build(DoubleList<Doc> docs, double percentil = 0.0)
        {
            if (docs == null || docs.Count == 0) return;

            int docsTotal = docs.Count;

            Zipf zipf = new Zipf();
            string[] palabrasUnicas = zipf.FiltrarVocabulario(docs, percentil);

            // ordenar alfabéticamente para la búsqueda binaria
            Array.Sort(palabrasUnicas, StringComparer.Ordinal);

            InicializarAtributos(palabrasUnicas.Length);

            BuildVocab(palabrasUnicas);

            Doc[] docArr = docs.ToArray();
            BuildMatrizFrec(docArr);

            CalcularIDF(docsTotal);
        }

        private void BuildMatrizFrec(Doc[] arr)
        {
            int totalDocs = arr.Length;
            int[,] tempFrecs = new int[palabrasCount, totalDocs];

            // contar frecuencias
            for (int docIndex = 0; docIndex < totalDocs; docIndex++)
            {
                Doc doc = arr[docIndex];
                foreach (string token in doc.tokens)
                {
                    int wordIndex = Array.BinarySearch(palabras, token, StringComparer.Ordinal);
                    if (wordIndex >= 0)
                        tempFrecs[wordIndex, docIndex]++;
                }
            }

            // pasar a DoubleList
            for (int j = 0; j < palabrasCount; j++)
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
            for (int j = 0; j < palabrasCount; j++)
            {
                int df = matrizFrec[j].Count;
                IDFValores[j] = calculador.CalcularIDF(totalDocs, df);
            }
        }

        

        private void InicializarAtributos(int Vocabcount)
        {
            palabras = new string[Vocabcount];
            IDFValores = new double[Vocabcount];
            matrizFrec = new DoubleList<(Doc doc, int freq)>[Vocabcount];
            palabrasCount = Vocabcount;
        }

        private void BuildVocab(string[] palabrasUnicas)
        {
            for (int i = 0; i < palabrasUnicas.Length; i++)
            {
                palabras[i] = palabrasUnicas[i];
            }
        }

        // metodos de acceso
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
    }
}

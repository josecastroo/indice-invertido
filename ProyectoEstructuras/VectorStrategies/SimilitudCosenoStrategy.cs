using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Index;
using BuscadorIndiceInvertido.Utilidades;
using System;

namespace BuscadorIndiceInvertido.Strategies
{
    public class SimilitudCosenoStrategy : IEstrategiaSimilitud
    {
        public DoubleList<(Doc doc, double score)> CalcularRanking(DoubleList<string> queryTokens, IndiceInvertido indice)
        {
            DoubleList<(Doc doc, double score)> resultados = new DoubleList<(Doc doc, double score)>();

            // crear vector de consulta TF-IDF
            DoubleList<(string termino, double tfidf)> queryVector = ConstruirVectorQuery(queryTokens, indice);
            if (queryVector.Count == 0) return resultados;

            // obtener documentos candidatos (los que tienen un termino de la query minimo)
            DoubleList<Doc> documentosCandidatos = ObtenerDocsCandidatos(queryTokens, indice);

            // calcular similitud para cada documento candidato
            foreach (Doc doc in documentosCandidatos)
            {
                double[] vectorDoc = ConstruirVectorDoc(doc, queryTokens, indice);
                double[] vectorQuery = ConvertirQuery(queryVector, queryTokens);

                double score = CalcularSimilitudCoseno(vectorQuery, vectorDoc);

                if (score > 0)
                {
                    resultados.Add((doc, score));
                }
            }

            return resultados;
        }

        public DoubleList<(string termino, double tfidf)> ConstruirVectorQuery(DoubleList<string> tokens, IndiceInvertido indice)
        {
            DoubleList<(string termino, double tfidf)> queryVector = new DoubleList<(string termino, double tfidf)>();

            // contar frecuencias de terminos en la query
            foreach (string token in tokens)
            {
                int freq = ContarFrecuencia(token, tokens);
                double tfidf = indice.GetTFIDF(token, freq);

                if (tfidf > 0 && !ContieneTermino(queryVector, token))
                {
                    queryVector.Add((token, tfidf));
                }
            }

            return queryVector;
        }

        private DoubleList<Doc> ObtenerDocsCandidatos(DoubleList<string> queryTokens, IndiceInvertido indice)
        {
            var candidatos = new DoubleList<Doc>();

            foreach (string token in queryTokens)
            {
                var postings = indice.GetPostings(token);
                foreach (var (doc, freq) in postings)
                {
                    if (!candidatos.Contains(doc))
                    {
                        candidatos.Add(doc);
                    }
                }
            }

            return candidatos;
        }

        private double[] ConstruirVectorDoc(Doc documento, DoubleList<string> queryTokens, IndiceInvertido indice)
        {
            var vector = new double[queryTokens.Count];
            int i = 0;

            foreach (string token in queryTokens)
            {
                int freq = ContarFrecuenciaEnDocumento(token, documento);
                vector[i++] = indice.GetTFIDF(token, freq);
            }

            return vector;
        }

        private double[] ConvertirQuery(DoubleList<(string termino, double tfidf)> queryVector, DoubleList<string> queryTokens)
        {
            var vector = new double[queryTokens.Count];
            int i = 0;

            foreach (string token in queryTokens)
            {
                vector[i++] = ObtenerTFIDFDelVector(queryVector, token);
            }

            return vector;
        }

        private int ContarFrecuencia(string termino, DoubleList<string> tokens)
        {
            int count = 0;
            foreach (string token in tokens)
            {
                if (token.Equals(termino, StringComparison.Ordinal))
                    count++;
            }
            return count;
        }

        private int ContarFrecuenciaEnDocumento(string termino, Doc documento)
        {
            int count = 0;
            foreach (string token in documento.tokens)
            {
                if (token.Equals(termino, StringComparison.Ordinal))
                    count++;
            }

            return count;
        }

        private bool ContieneTermino(DoubleList<(string termino, double tfidf)> vector, string termino)
        { 
            foreach (var (term, tfidf) in vector)
            {
                if (term.Equals(termino, StringComparison.Ordinal))
                    return true;
            }

            return false;
        }

        private double ObtenerTFIDFDelVector(DoubleList<(string termino, double tfidf)> vector, string termino)
        {
            foreach (var (term, tfidf) in vector)
            {
                if (term.Equals(termino, StringComparison.Ordinal))
                    return tfidf;
            }

            return 0.0;
        }

        // Método específico de similitud coseno
        private double CalcularSimilitudCoseno(double[] vector1, double[] vector2)
        {
            double productoPunto = 0;
            for (int i = 0; i < vector1.Length; i++)
            {
                productoPunto += vector1[i] * vector2[i];
            }

            double magnitud1 = CalcularMagnitud(vector1);
            double magnitud2 = CalcularMagnitud(vector2);

            // evitar division por cero
            if (magnitud1 == 0 || magnitud2 == 0)
                return 0;

            return productoPunto / (magnitud1 * magnitud2);
        }

        private double CalcularMagnitud(double[] vector)
        {
            double suma = 0;

            for (int i = 0; i < vector.Length; i++)
            {
                suma += vector[i] * vector[i];
            }

            return Math.Sqrt(suma);
        }
    }
}
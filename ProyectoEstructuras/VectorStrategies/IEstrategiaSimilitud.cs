using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Index;
using BuscadorIndiceInvertido.Utilidades;
using System;

namespace BuscadorIndiceInvertido.Strategies
{
    internal interface IEstrategiaSimilitud
    {
        DoubleList<(Doc doc, double score)> CalcularScores(DoubleList<string> queryTokens, IndiceInvertido indice);

        DoubleList<(string termino, double tfidf)> ConstruirVectorQuery(DoubleList<string> tokens, IndiceInvertido indice);

        DoubleList<Doc> ObtenerDocsCandidatos(DoubleList<string> queryTokens, IndiceInvertido indice);

        double[] ConstruirVectorDoc(Doc documento, DoubleList<string> queryTokens, IndiceInvertido indice);

        double[] ConvertirQuery(DoubleList<(string termino, double tfidf)> queryVector, DoubleList<string> queryTokens);

        int ContarFrecuencia(string termino, DoubleList<string> tokens);

        int ContarFrecuenciaEnDocumento(string termino, Doc documento);

        bool ContieneTermino(DoubleList<(string termino, double tfidf)> vector, string termino);

        double ObtenerTFIDFDelVector(DoubleList<(string termino, double tfidf)> vector, string termino);
    }
}
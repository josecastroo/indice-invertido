

namespace BuscadorIndiceInvertido.Calculos
{
    internal interface ITFIDFCalculador
    {
        double CalcularIDF(int totalDocs, int docFrec);
        double CalcularTFIDF(int termFrec, double idf);
    }

    internal class TFIDFCalculador : ITFIDFCalculador
    {
        public double CalcularIDF(int totalDocs, int docFrec)
        {
            return Math.Log10((double)totalDocs / Math.Max(1, docFrec));
        }

        public double CalcularTFIDF(int termFrec, double idf)
        {
            return termFrec * idf;
        }
    }
}

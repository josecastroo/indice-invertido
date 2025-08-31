using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Index;
using BuscadorIndiceInvertido.Utilidades;
using BuscadorIndiceInvertido.Strategies;


namespace BuscadorIndiceInvertido.Strategies
{
    internal class ProcesadorVector
    {
        private readonly SimilitudCosenoStrategy strategy;

        public ProcesadorVector()
        {
            strategy = new SimilitudCosenoStrategy();
        }
  
        public DoubleList<(string termino, double tfidf)> ConstruirVectorQuery(DoubleList<string> tokens, IndiceInvertido indice)
        {
            return strategy.ConstruirVectorQuery(tokens, indice);
        }


        public DoubleList<Doc> ObtenerDocsCandidatos(DoubleList<string> queryTokens, IndiceInvertido indice)
        {
            return strategy.ObtenerDocsCandidatos(queryTokens, indice);
        }

       
        public double[] ConstruirVectorDoc(Doc documento, DoubleList<string> queryTokens, IndiceInvertido indice)
        {
            return strategy.ConstruirVectorDoc(documento, queryTokens, indice);
        }

        
        public double[] ConvertirQuery(DoubleList<(string termino, double tfidf)> queryVector, DoubleList<string> queryTokens)
        {
            return strategy.ConvertirQuery(queryVector, queryTokens);
        }

        
        public int ContarFrecuencia(string termino, DoubleList<string> tokens)
        {
            return strategy.ContarFrecuencia(termino, tokens);
        }

       
        public int ContarFrecuenciaEnDocumento(string termino, Doc documento)
        {
            return strategy.ContarFrecuenciaEnDocumento(termino, documento);
        }

        
        public bool ContieneTermino(DoubleList<(string termino, double tfidf)> vector, string termino)
        {
            return strategy.ContieneTermino(vector, termino);
        }

      
        public double ObtenerTFIDFDelVector(DoubleList<(string termino, double tfidf)> vector, string termino)
        {
            return strategy.ObtenerTFIDFDelVector(vector, termino);
        }
    }
}
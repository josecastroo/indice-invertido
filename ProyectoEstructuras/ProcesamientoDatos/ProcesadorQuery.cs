using BuscadorIndiceInvertido.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscadorIndiceInvertido.ProcesamientoDatos
{
    internal class ProcesadorQuery
    {
        private Tokenizer tokenizer;
        private StopWordsFiltro filtroSW;

        public ProcesadorQuery()
        {
            tokenizer = new Tokenizer();
            filtroSW = new StopWordsFiltro();
        }

        public DoubleList<string> ProcesarQuery(string query)
        {
            DoubleList<string> tokens = tokenizer.TokenizeTexto(query);

            string[] tokenArr = new string[tokens.Count];
            tokens.CopyTo(tokenArr, 0);

            DoubleList<string> tokensFiltrados = filtroSW.FiltrarStopWords(tokenArr);

            return tokensFiltrados;
        }
    }
}

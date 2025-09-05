using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Index;
using BuscadorIndiceInvertido.Utilidades;

namespace BuscadorIndiceInvertido.Strategies
{
    public interface IEstrategiaSimilitud
    {
        DoubleList<(Doc doc, double score)> CalcularRanking(DoubleList<string> queryTokens, IndiceInvertido indice);
    }
}
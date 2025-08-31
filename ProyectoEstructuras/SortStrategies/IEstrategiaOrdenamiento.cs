using BuscadorIndiceInvertido.Base;

namespace BuscadorIndiceInvertido.Estrategias
{
    internal interface IEstrategiaOrdenamiento
    {
        void OrdenarAlfabetico(string[] arr, int inicio, int fin);
        void OrdenarPorPuntaje((Doc doc, double score)[] arr, int inicio, int fin);
        int BusquedaBinaria(string palabra, string[] arr);
    }
}
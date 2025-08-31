using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Estrategias;

namespace BuscadorIndiceInvertido.Ordenamientos
{
    internal static class Utils
    {
        private static IEstrategiaOrdenamiento estrategia = new QuickSortStrategy();
     
        public static void OrdenarAlfab(string[] arr, int ini, int final)
        {
            estrategia.OrdenarAlfabetico(arr, ini, final);
        }

        public static void OrdenarPorPuntaje((Doc doc, double score)[] arr, int ini, int final)
        {
            estrategia.OrdenarPorPuntaje(arr, ini, final);
        }

        public static int BusquedaBinaria(string palabra, string[] arr)
        {
            return estrategia.BusquedaBinaria(palabra, arr);
        }

    }
}
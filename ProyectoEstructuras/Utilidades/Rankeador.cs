using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Utilidades;
using ProyectoEstructuras.SortStrategies;

namespace BuscadorIndiceInvertido.Ordenamientos
{
    internal class Rankeador
    {
        public Rankeador()
        {

        }

        // tal vez se pueda optimizar quitando las conversiones
        public DoubleList<(Doc doc, double score)> OrdenarResultados(DoubleList<(Doc doc, double score)> resultados)
        {
            if (resultados.Count <= 1) return resultados;

            // convertir a array para usar QuickSort
            (Doc doc, double score)[] arr = new (Doc doc, double score)[resultados.Count];
            int i = 0;
            foreach (var resultado in resultados)
            {
                arr[i++] = resultado;
            }

            // ordenar por score usando QuickSort
            IOrdenamiento<(Doc doc, double score)> quickSort = new QuickSortDocs();
            quickSort.Ordenar(arr, 0, arr.Length - 1);

            // Utils.OrdenarPorPuntaje(arr, 0, arr.Length - 1); // numerico quick

            // regresar a DoubleList
            var resultadosOrdenados = new DoubleList<(Doc doc, double score)>();
            foreach (var resultado in arr)
            {
                resultadosOrdenados.Add(resultado);
            }

            return resultadosOrdenados;
        }
    }
}

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

        //aqui se usa quick sort (en el peor de los casos n^2 pero raro que ocurra
        public DoubleList<(Doc doc, double score)> OrdenarResultados(DoubleList<(Doc doc, double score)> resultados)
        {
            if (resultados.Count <= 1) return resultados;
            
            (Doc doc, double score)[] arr = new (Doc doc, double score)[resultados.Count];
            int i = 0;
            foreach (var resultado in resultados)
            {
                arr[i++] = resultado;
            }
            
            IOrdenamiento<(Doc doc, double score)> quickSort = new QuickSortDocs();
            quickSort.Ordenar(arr, 0, arr.Length - 1);
            
            var resultadosOrdenados = new DoubleList<(Doc doc, double score)>();
            foreach (var resultado in arr)
            {
                resultadosOrdenados.Add(resultado);
            }

            return resultadosOrdenados;
        }
    }
}

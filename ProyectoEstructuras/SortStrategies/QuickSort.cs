using BuscadorIndiceInvertido.Base;

namespace ProyectoEstructuras.SortStrategies
{
    internal class QuickSortNumerico : IOrdenamiento<int>
    {
        public void Ordenar(int[] arr, int inicio, int fin)
        {
            QuickSort(arr, inicio, fin, (a, b) => a.CompareTo(b)); // ascendente por defecto
        }

        private void QuickSort(int[] arr, int inicio, int fin, Comparison<int> comp)
        {
            if (inicio < fin)
            {
                int pivot = Particion(arr, inicio, fin, comp);
                QuickSort(arr, inicio, pivot - 1, comp);
                QuickSort(arr, pivot + 1, fin, comp);
            }
        }

        private int Particion(int[] arr, int inicio, int fin, Comparison<int> comp)
        {
            int pivot = arr[fin];
            int i = inicio - 1;
            for (int j = inicio; j < fin; j++)
            {
                if (comp(arr[j], pivot) <= 0)
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                }
            }
            (arr[i + 1], arr[fin]) = (arr[fin], arr[i + 1]);
            return i + 1;
        }
    }

    internal class QuickSortDocs : IOrdenamiento<(Doc doc, double score)>
    {
        public void Ordenar((Doc doc, double score)[] arr, int inicio, int fin)
        {
            QuickSort(arr, inicio, fin, (a, b) => b.score.CompareTo(a.score)); // descendente
        }

        private void QuickSort((Doc doc, double score)[] arr, int inicio, int fin, Comparison<(Doc doc, double score)> comp)
        {
            if (inicio < fin)
            {
                int pivot = Particion(arr, inicio, fin, comp);
                QuickSort(arr, inicio, pivot - 1, comp);
                QuickSort(arr, pivot + 1, fin, comp);
            }
        }

        private int Particion((Doc doc, double score)[] arr, int inicio, int fin, Comparison<(Doc doc, double score)> comp)
        {
            var pivot = arr[fin];
            int i = inicio - 1;
            for (int j = inicio; j < fin; j++)
            {
                if (comp(arr[j], pivot) <= 0)
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                }
            }
            (arr[i + 1], arr[fin]) = (arr[fin], arr[i + 1]);
            return i + 1;
        }
    }
}
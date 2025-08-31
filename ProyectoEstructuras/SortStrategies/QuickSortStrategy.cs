using BuscadorIndiceInvertido.Base;

namespace BuscadorIndiceInvertido.Estrategias
{
    internal class QuickSortStrategy : IEstrategiaOrdenamiento
    {
        public void OrdenarAlfabetico(string[] arr, int inicio, int fin)
        {
            if (inicio < fin)
            {
                int pivot = ParticionAlfabetico(arr, inicio, fin);
                OrdenarAlfabetico(arr, inicio, pivot - 1);
                OrdenarAlfabetico(arr, pivot + 1, fin);
            }
        }

        public void OrdenarPorPuntaje((Doc doc, double score)[] arr, int inicio, int fin)
        {
            if (inicio < fin)
            {
                int pivot = ParticionPorScore(arr, inicio, fin);
                OrdenarPorPuntaje(arr, inicio, pivot - 1);
                OrdenarPorPuntaje(arr, pivot + 1, fin);
            }
        }

        public int BusquedaBinaria(string palabra, string[] arr)
        {
            int inicio = 0;
            int fin = arr.Length - 1;

            while (inicio <= fin)
            {
                int medio = inicio + (fin - inicio) / 2;
                int resultado = string.Compare(palabra, arr[medio], StringComparison.Ordinal);

                if (resultado == 0)
                    return medio;
                else if (resultado < 0)
                    fin = medio - 1;
                else
                    inicio = medio + 1;
            }

            return -1;
        }

        private int ParticionAlfabetico(string[] arr, int inicio, int fin)
        {
            string pivot = arr[fin];
            int i = inicio - 1;

            for (int j = inicio; j < fin; j++)
            {
                if (string.Compare(arr[j], pivot, StringComparison.Ordinal) <= 0)
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                }
            }

            (arr[i + 1], arr[fin]) = (arr[fin], arr[i + 1]);
            return i + 1;
        }

        private int ParticionPorScore((Doc doc, double score)[] arr, int inicio, int fin)
        {
            double pivotScore = arr[fin].score;
            int i = inicio - 1;

            for (int j = inicio; j < fin; j++)
            {
                if (arr[j].score >= pivotScore) // Orden descendente
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
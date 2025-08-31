using BuscadorIndiceInvertido.Base;

namespace BuscadorIndiceInvertido.Estrategias
{
    internal class MergeSortStrategy : IEstrategiaOrdenamiento
    {
        public void OrdenarAlfabetico(string[] arr, int inicio, int fin)
        {
            if (inicio < fin)
            {
                int medio = inicio + (fin - inicio) / 2;
                OrdenarAlfabetico(arr, inicio, medio);
                OrdenarAlfabetico(arr, medio + 1, fin);
                MergeAlfabetico(arr, inicio, medio, fin);
            }
        }

        public void OrdenarPorPuntaje((Doc doc, double score)[] arr, int inicio, int fin)
        {
            if (inicio < fin)
            {
                int medio = inicio + (fin - inicio) / 2;
                OrdenarPorPuntaje(arr, inicio, medio);
                OrdenarPorPuntaje(arr, medio + 1, fin);
                MergePorScore(arr, inicio, medio, fin);
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

        private void MergeAlfabetico(string[] arr, int inicio, int medio, int fin)
        {
            int n1 = medio - inicio + 1;
            int n2 = fin - medio;

            string[] izq = new string[n1];
            string[] der = new string[n2];

            Array.Copy(arr, inicio, izq, 0, n1);
            Array.Copy(arr, medio + 1, der, 0, n2);

            int i = 0, j = 0, k = inicio;

            while (i < n1 && j < n2)
            {
                if (string.Compare(izq[i], der[j], StringComparison.Ordinal) <= 0)
                    arr[k++] = izq[i++];
                else
                    arr[k++] = der[j++];
            }

            while (i < n1) arr[k++] = izq[i++];
            while (j < n2) arr[k++] = der[j++];
        }

        private void MergePorScore((Doc doc, double score)[] arr, int inicio, int medio, int fin)
        {
            int n1 = medio - inicio + 1;
            int n2 = fin - medio;

            (Doc doc, double score)[] izq = new (Doc doc, double score)[n1];
            (Doc doc, double score)[] der = new (Doc doc, double score)[n2];

            Array.Copy(arr, inicio, izq, 0, n1);
            Array.Copy(arr, medio + 1, der, 0, n2);

            int i = 0, j = 0, k = inicio;

            while (i < n1 && j < n2)
            {
                if (izq[i].score >= der[j].score) // Orden descendente
                    arr[k++] = izq[i++];
                else
                    arr[k++] = der[j++];
            }

            while (i < n1) arr[k++] = izq[i++];
            while (j < n2) arr[k++] = der[j++];
        }
    }
}
using BuscadorIndiceInvertido.Base;

namespace ProyectoEstructuras.SortStrategies;


internal class MergeSortGenerico<T> : IOrdenamiento<T>
{
    private readonly Comparison<T> comparacion;

    public MergeSortGenerico(Comparison<T> comp)
    {
        comparacion = comp;
    }

    public void Ordenar(T[] arr, int inicio, int fin)
    {
        MergeSort(arr, inicio, fin, comparacion);
    }

    private void MergeSort(T[] arr, int inicio, int fin, Comparison<T> comp)
    {
        if (inicio < fin)
        {
            int medio = (inicio + fin) / 2;
            MergeSort(arr, inicio, medio, comp);
            MergeSort(arr, medio + 1, fin, comp);
            Merge(arr, inicio, medio, fin, comp);
        }
    }

    private void Merge(T[] arr, int inicio, int medio, int fin, Comparison<T> comp)
    {
        int n1 = medio - inicio + 1;
        int n2 = fin - medio;
        T[] izquierda = new T[n1];
        T[] derecha = new T[n2];
        Array.Copy(arr, inicio, izquierda, 0, n1);
        Array.Copy(arr, medio + 1, derecha, 0, n2);
        int i = 0, j = 0, k = inicio;
        while (i < n1 && j < n2)
        {
            if (comp(izquierda[i], derecha[j]) <= 0)
            {
                arr[k] = izquierda[i];
                i++;
            }
            else
            {
                arr[k] = derecha[j];
                j++;
            }
            k++;
        }
        while (i < n1)
        {
            arr[k] = izquierda[i];
            i++;
            k++;
        }
        while (j < n2)
        {
            arr[k] = derecha[j];
            j++;
            k++;
        }
    }
}

using BuscadorIndiceInvertido.Base;

namespace ProyectoEstructuras.SortStrategies
{
    internal interface IOrdenamiento<T>
    {
        void Ordenar(T[] arr, int inicio, int fin);
    }
}
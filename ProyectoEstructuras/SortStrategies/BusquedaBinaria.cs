using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoEstructuras.SortStrategies
{
    internal class BusquedaBinaria<T> : IBusqueda<T> where T : IComparable<T>
    {
        public int Buscar(T[] arr, T clave)
        {
            int izquierda = 0;
            int derecha = arr.Length - 1;

            while (izquierda <= derecha)
            {
                int medio = izquierda + (derecha - izquierda) / 2;
                int comparacion = arr[medio].CompareTo(clave);

                if (comparacion == 0)
                    return medio; // encontrado
                else if (comparacion < 0)
                    izquierda = medio + 1;
                else
                    derecha = medio - 1;
            }

            return -1; 
        }
    }
}

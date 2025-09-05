using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoEstructuras.SortStrategies
{
    internal interface IBusqueda<T>
    {
        int Buscar(T[] arr, T clave);
    }
}

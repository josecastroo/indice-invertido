using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuscadorIndiceInvertido.Base;

namespace ProyectoEstructuras.SortStrategies
{
    internal class RadixSort : IOrdenamiento<string>
    {
        public void Ordenar(string[] arr, int inicio, int fin)
        {
            if (inicio >= fin) return;

            int maxLen = 0;
            for (int i = inicio; i <= fin; i++)
            {
                if (arr[i].Length > maxLen)
                    maxLen = arr[i].Length;
            }

            for (int pos = maxLen - 1; pos >= 0; pos--)
            {
                CountingSort(arr, inicio, fin, pos);
            }
        }

        private void CountingSort(string[] arr, int inicio, int fin, int pos)
        {
            int R = 256; // ASCII extendido
            int[] count = new int[R + 1];
            string[] aux = new string[fin - inicio + 1];

            for (int i = inicio; i <= fin; i++)
            {
                int c = (pos < arr[i].Length) ? arr[i][pos] : 0;
                count[c + 1]++;
            }

            // acumulado
            for (int r = 1; r < count.Length; r++)
            {
                count[r] += count[r - 1];
            }

            // distribuir
            for (int i = inicio; i <= fin; i++)
            {
                int c = (pos < arr[i].Length) ? arr[i][pos] : 0;
                aux[count[c]] = arr[i];
                count[c]++;
            }

            // copiar de vuelta al arreglo original
            for (int i = inicio; i <= fin; i++)
            {
                arr[i] = aux[i - inicio];
            }
        }
    }
}

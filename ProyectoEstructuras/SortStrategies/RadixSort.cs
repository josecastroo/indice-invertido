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
            if (arr == null || arr.Length == 0 || inicio > fin)
                return;
            
            int maxLen = 0;
            for (int i = inicio; i <= fin; i++)
            {
                if (arr[i] != null && arr[i].Length > maxLen)
                    maxLen = arr[i].Length;
            }

            if (maxLen == 0)
                return;
            
            for (int pos = maxLen - 1; pos >= 0; pos--)
            {
                CountingSort(arr, inicio, fin, pos);
            }
        }

        private void CountingSort(string[] arr, int inicio, int fin, int pos)
        {
            if (arr == null || arr.Length == 0 || inicio > fin)
                return;

            const int R = 256; // tema Ascii
            int[] count = new int[R + 2];
            string[] aux = new string[fin - inicio + 1];
            for (int i = inicio; i <= fin; i++)
            {
                int charCode = GetCharAt(arr[i], pos);
                if (charCode >= 0 && charCode < R + 1) 
                {
                    count[charCode + 1]++;
                }
            }
            for (int r = 1; r < count.Length; r++)
                count[r] += count[r - 1];
            
            for (int i = inicio; i <= fin; i++)
            {
                int charCode = GetCharAt(arr[i], pos);
                if (charCode >= 0 && charCode < R + 1 && count[charCode] < aux.Length)
                {
                    aux[count[charCode]++] = arr[i];
                }
            }
            
            for (int i = inicio; i <= fin; i++)
                arr[i] = aux[i - inicio];
        }

        private int GetCharAt(string str, int pos)
        {
            // Si el string es null o la posición está fuera de rango, retorna 0
            // Esto hace que strings más cortos aparezcan primero
            if (str == null || pos >= str.Length)
                return 0;
            
            // Limitar el rango de caracteres a ASCII básico para evitar desbordamientos
            char c = str[pos];
            int charCode = (int)c;
            
            // Si el carácter está fuera del rango ASCII básico, mapearlo a un valor seguro
            if (charCode > 255)
                charCode = 255;
                
            return charCode + 1; // +1 para reservar 0 para strings cortos
        }
    }
}
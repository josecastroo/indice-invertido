using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Utilidades;
using ProyectoEstructuras.SortStrategies;
using System;

namespace BuscadorIndiceInvertido.Index
{
    internal class TerminoFrecuencia
    {
        public string Termino { get; set; }
        public int Frecuencia { get; set; }
    }

    internal class Zipf
    {
        public string[] FiltrarVocabulario(DoubleList<Doc> docs, double percentil)
        {
            if (docs == null || docs.Count == 0)
            {
                Console.WriteLine("No hay documentos para procesar");
                return new string[0];
            }

            Console.WriteLine($"Procesando {docs.Count} documentos con percentil {percentil}");

            DoubleList<string> todasPalabras = ObtenerPalabras(docs);
            Console.WriteLine($"Total de tokens extraídos: {todasPalabras.Count}");

            if (todasPalabras.Count == 0)
            {
                Console.WriteLine("No se encontraron tokens");
                return new string[0];
            }

            // Crear vocabulario único usando RadixSort
            string[] vocabularioUnico = CrearVocabularioUnico(todasPalabras);
            Console.WriteLine($"Vocabulario único creado: {vocabularioUnico.Length} palabras");

            // Mostrar algunas palabras del vocabulario para debug
            Console.WriteLine("Primeras 10 palabras del vocabulario:");
            for (int i = 0; i < Math.Min(10, vocabularioUnico.Length); i++)
            {
                Console.WriteLine($"  {i}: '{vocabularioUnico[i]}'");
            }

            int[] frecuenciasGlobales = CalcularFrecuenciasGlobales(docs, vocabularioUnico);

            // Crear array de TerminoFrecuencia para ordenar por frecuencia
            TerminoFrecuencia[] listaFrecuencias = new TerminoFrecuencia[vocabularioUnico.Length];
            for (int i = 0; i < vocabularioUnico.Length; i++)
            {
                listaFrecuencias[i] = new TerminoFrecuencia 
                { 
                    Termino = vocabularioUnico[i], 
                    Frecuencia = frecuenciasGlobales[i] 
                };
            }

            // Ordenar por frecuencia (descendente)
            var mergeSort = new MergeSortGenerico<TerminoFrecuencia>((x, y) => y.Frecuencia.CompareTo(x.Frecuencia));
            mergeSort.Ordenar(listaFrecuencias, 0, listaFrecuencias.Length - 1);

            // Aplicar filtro de Zipf
            int totalWords = listaFrecuencias.Length;
            int wordsToRemove = (int)(totalWords * percentil);
            
            Console.WriteLine($"Aplicando ley de Zipf - Eliminando {wordsToRemove} palabras más frecuentes de {totalWords}");

            // Verificar si "universidad" está en la lista antes del filtro
            bool universidadAntes = false;
            for (int i = 0; i < listaFrecuencias.Length; i++)
            {
                if (listaFrecuencias[i].Termino == "universidad")
                {
                    universidadAntes = true;
                    Console.WriteLine($"'universidad' encontrada en posición {i} con frecuencia {listaFrecuencias[i].Frecuencia}");
                    break;
                }
            }
            if (!universidadAntes)
            {
                Console.WriteLine("'universidad' NO encontrada en el vocabulario");
            }

            DoubleList<string> vocabularioFiltrado = new DoubleList<string>();
            for (int i = wordsToRemove; i < totalWords; i++)
            {
                // Solo filtrar palabras con frecuencia muy baja si es necesario
                if (listaFrecuencias[i].Frecuencia > 0) // Cambié >= 1 por > 0 para ser más claro
                {
                    vocabularioFiltrado.Add(listaFrecuencias[i].Termino);
                }
            }

            Console.WriteLine($"Vocabulario después del filtro: {vocabularioFiltrado.Count} palabras");

            // Verificar si "universidad" sobrevivió al filtro
            bool universidadDespues = false;
            foreach (var palabra in vocabularioFiltrado)
            {
                if (palabra == "universidad")
                {
                    universidadDespues = true;
                    break;
                }
            }
            Console.WriteLine($"'universidad' después del filtro: {(universidadDespues ? "SÍ" : "NO")}");

            // Convertir a array
            string[] resultado = new string[vocabularioFiltrado.Count];
            vocabularioFiltrado.CopyTo(resultado, 0);
            return resultado;
        }

        private DoubleList<string> ObtenerPalabras(DoubleList<Doc> docs)
        {
            DoubleList<string> tokens = new DoubleList<string>();
            foreach (Doc doc in docs)
            {
                foreach (string token in doc.tokens)
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        tokens.Add(token.ToLower()); // Asegurar minúsculas
                    }
                }
            }
            return tokens;
        }

        private string[] CrearVocabularioUnico(DoubleList<string> todasPalabras)
        {
            // Convertir a array
            string[] arr = new string[todasPalabras.Count];
            todasPalabras.CopyTo(arr, 0);

            // Normalizar a minúsculas
            for (int i = 0; i < arr.Length; i++)
                arr[i] = arr[i].ToLower();

            // USAR RADIX SORT en lugar de Array.Sort
            if (arr.Length > 1)
            {
                IOrdenamiento<string> radixSort = new RadixSort();
                radixSort.Ordenar(arr, 0, arr.Length - 1);
            }

            if (arr.Length == 0) return arr;

            // Eliminar duplicados (el array ya está ordenado)
            int j = 0;
            for (int i = 1; i < arr.Length; i++)
            {
                if (!arr[i].Equals(arr[j], StringComparison.Ordinal))
                {
                    j++;
                    arr[j] = arr[i];
                }
            }

            // Crear array final con solo elementos únicos
            string[] palabrasUnicas = new string[j + 1];
            for (int k = 0; k <= j; k++)
            {
                palabrasUnicas[k] = arr[k];
            }

            return palabrasUnicas;
        }

        private int[] CalcularFrecuenciasGlobales(DoubleList<Doc> docs, string[] vocabularioUnico)
        {
            int[] frecuencias = new int[vocabularioUnico.Length];
            
            foreach (var doc in docs)
            {
                foreach (var token in doc.tokens)
                {
                    string tokenLower = token.ToLower();
                    int index = Array.BinarySearch(vocabularioUnico, tokenLower, StringComparer.Ordinal);
                    if (index >= 0)
                    {
                        frecuencias[index]++;
                    }
                }
            }
            return frecuencias;
        }
    }
}
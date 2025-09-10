using BuscadorIndiceInvertido.Ordenamientos;
using ProyectoEstructuras.SortStrategies;

namespace BuscadorIndiceInvertido.Utilidades
{
    internal class StopWordsFiltro
    {
        private string[] StopWords = new string[] {
                "a", "al", "algo", "algunas", "algunos", "ante", "antes", "como", "con",
                "contra", "cual", "cuando", "de", "del", "desde", "donde", "durante",
                "e", "el", "ella", "ellas", "ellos", "en", "entre", "era", "erais", "eran",
                "eras", "es", "esa", "esas", "ese", "eso", "esos", "esta", "estaba",
                "estabais", "estaban", "estabas", "estad", "estada", "estadas", "estado",
                "estados", "estais", "estan", "estar", "estaremos", "estaria", "estarias",
                "este", "esto", "estos", "estoy", "fue", "fueron", "fui", "fuimos", "ha",
                "habeis", "haber", "habiamos", "habia", "habias", "han", "has", "hasta",
                "hay", "haya", "he", "hemos", "hice", "hicieron", "hizo", "hoy", "la",
                "las", "le", "les", "lo", "los", "me", "mi", "mis", "mas", "nada", "no",
                "nos", "nosotros", "o", "os", "otra", "otras", "otro", "otros", "para",
                "pero", "poco", "por", "porque", "que", "quien", "quienes", "se",
                "sea", "sean", "ser", "si", "sin", "sobre", "sois", "son", "soy", "su",
                "sus", "suya", "suyas", "suyo", "suyos", "te", "tiene", "tienes", "todo",
                "todos", "tu", "tus", "un", "una", "uno", "unos", "vos", "vosotros", "y",
                "ya", "yo"
        };

        public StopWordsFiltro()
        {
            for (int i = 0; i < StopWords.Length; i++)
                StopWords[i] = StopWords[i].ToLower();
            
            // Solo ordenar si hay más de un elemento
            if (StopWords != null && StopWords.Length > 1)
            {
                IOrdenamiento<string> radixSort = new RadixSort();
                radixSort.Ordenar(StopWords, 0, StopWords.Length - 1);
            }
        }
        public DoubleList<string> FiltrarStopWords(DoubleList<string> palabras)
        {
            var tokens = new DoubleList<string>();
            foreach (var palabra in palabras)
            {
                if (Array.BinarySearch(StopWords, palabra) < 0) // no está en stopwords
                    tokens.Add(palabra);
            }
            return tokens;
        }

        public DoubleList<string> FiltrarStopWords(string[] palabras)
        {
            DoubleList<string> tokens = new DoubleList<string>();

            if (palabras == null || palabras.Length == 0)
                return tokens;

            foreach (string palabra in palabras)
            {
                string palabraLower = palabra.ToLower();
                if (!IsStopWord(palabraLower))
                    tokens.Add(palabraLower);
            }

            return tokens;
        }

        private bool IsStopWord(string palabra)
        {
            if (string.IsNullOrEmpty(palabra) || StopWords == null || StopWords.Length == 0)
                return false;

            IBusqueda<string> buscador = new BusquedaBinaria<string>();
            return buscador.Buscar(StopWords, palabra) != -1;
        }
    }
}

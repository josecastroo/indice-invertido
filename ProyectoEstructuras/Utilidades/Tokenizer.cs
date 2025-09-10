using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;


namespace BuscadorIndiceInvertido.Utilidades
{
    internal class Tokenizer
    {
        public Tokenizer()
        {
        }

        public DoubleList<string> TokenizeTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return new DoubleList<string>();
            }

            // Normalizar y quitar tildes
            string textoNormalizado = QuitarTildes(texto.ToLower());

            // Usar regex más flexible para capturar palabras
            // \p{L} captura letras Unicode, + significa una o más
            var matches = Regex.Matches(textoNormalizado, @"\p{L}+", RegexOptions.CultureInvariant);

            var tokens = new DoubleList<string>();
            foreach (Match m in matches)
            {
                string token = m.Value.Trim();
                if (!string.IsNullOrEmpty(token) && token.Length > 1) // Filtrar tokens de una sola letra
                {
                    tokens.Add(token);
                }
            }

            return tokens;
        }

        // Método para debugging - tokenizar y mostrar información
        public DoubleList<string> TokenizeTextoConDebug(string texto, string nombreArchivo = "")
        {
            var tokens = TokenizeTexto(texto);
            
            // Buscar específicamente "universidad" en el texto original y en los tokens
            bool universidadEnTexto = texto.ToLower().Contains("universidad");
            bool universidadEnTokens = false;
            
            foreach (var token in tokens)
            {
                if (token == "universidad")
                {
                    universidadEnTokens = true;
                    break;
                }
            }

            if (universidadEnTexto)
            {
                Console.WriteLine($"[DEBUG] Archivo: {nombreArchivo}");
                Console.WriteLine($"  - 'universidad' en texto original: {universidadEnTexto}");
                Console.WriteLine($"  - 'universidad' en tokens: {universidadEnTokens}");
                Console.WriteLine($"  - Total tokens: {tokens.Count}");
                
                if (universidadEnTokens)
                {
                    Console.WriteLine($"  - ¡'universidad' tokenizada correctamente!");
                }
                else
                {
                    Console.WriteLine($"  - ERROR: 'universidad' se perdió en tokenización");
                    // Mostrar contexto donde aparece "universidad"
                    int indice = texto.ToLower().IndexOf("universidad");
                    if (indice >= 0)
                    {
                        int inicio = Math.Max(0, indice - 20);
                        int fin = Math.Min(texto.Length, indice + 30);
                        string contexto = texto.Substring(inicio, fin - inicio);
                        Console.WriteLine($"  - Contexto: '{contexto}'");
                    }
                }
            }

            return tokens;
        }

        private string QuitarTildes(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            string normalized = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

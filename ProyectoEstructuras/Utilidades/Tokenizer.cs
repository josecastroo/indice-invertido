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
            
            string textoNormalizado = QuitarTildes(texto.ToLower());
            
            var matches = Regex.Matches(textoNormalizado, @"\p{L}+", RegexOptions.CultureInvariant);

            var tokens = new DoubleList<string>();
            foreach (Match m in matches)
            {
                string token = m.Value.Trim();
                if (!string.IsNullOrEmpty(token) && token.Length > 1)
                {
                    tokens.Add(token);
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

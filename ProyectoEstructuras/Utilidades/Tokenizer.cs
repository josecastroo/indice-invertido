using System.Globalization;
using System.Text;


namespace BuscadorIndiceInvertido.Utilidades
{
    internal class Tokenizer
    {
        public Tokenizer()
        {

        }

        public DoubleList<string> TokenizeTexto(string texto)
        {
            texto = QuitarTildes(texto.ToLower());

            var textoLimpio = new StringBuilder();
            foreach (char c in texto)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                {
                    textoLimpio.Append(c);
                }
            }

            string[] tokens = textoLimpio.ToString().Split(
                new char[] { ' ', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries
            );

            return new DoubleList<string>(tokens);
        }

        private string QuitarTildes(string texto)
        {
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

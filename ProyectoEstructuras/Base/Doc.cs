using BuscadorIndiceInvertido.Utilidades;


namespace BuscadorIndiceInvertido.Base
{
    public class Doc
    {
        public string FileName { get; set; }
        public DoubleList<string> tokens { get; set; }

        public Doc(string FileName, DoubleList<string> tokens)
        {
            this.FileName = FileName;
            this.tokens = tokens;
        }
    }
}



namespace BuscadorIndiceInvertido.Calculos
{
    internal class SimilitudCoseno
    {
        public double CalcularSimilitudCoseno(double[] vector1, double[] vector2)
        {
            double productoPunto = 0;
            for (int i = 0; i < vector1.Length; i++)
            {
                productoPunto += vector1[i] * vector2[i];
            }

            double magnitud1 = CalcularMagnitud(vector1);
            double magnitud2 = CalcularMagnitud(vector2);

            // evitar division por cero
            if (magnitud1 == 0 || magnitud2 == 0)
                return 0;

            return productoPunto / (magnitud1 * magnitud2);
        }

        private double CalcularMagnitud(double[] vector)
        {
            double suma = 0;

            for (int i = 0; i < vector.Length; i++)
            {
                suma += vector[i] * vector[i];
            }

            return Math.Sqrt(suma);
        }
    }
}

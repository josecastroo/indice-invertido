namespace ProyectoEstructuras.Utilidades;

public class Vector //Clase que sirve según el enunciado (hay que sobrecargar operadores)
{
    private double[] componentes;
        private int dimension;

        public Vector(int dimension)
        {
            this.dimension = dimension;
            this.componentes = new double[dimension];
        }

        public Vector(double[] valores)
        {
            this.dimension = valores.Length;
            this.componentes = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                this.componentes[i] = valores[i];
            }
        }

        public double this[int indice]
        {
            get { return componentes[indice]; }
            set { componentes[indice] = value; }
        }

        public int Dimension => dimension;

        // aqui se esta sobrecargando 
        public static double operator *(Vector a, Vector b)
        {
            if (a.dimension != b.dimension)
                throw new ArgumentException("Los vectores deben tener la misma dimensión");

            double producto = 0.0;
            for (int i = 0; i < a.dimension; i++)
            {
                producto += a.componentes[i] * b.componentes[i];
            }
            return producto;
        }
        public double Magnitud()
        {
            double suma = 0.0;
            for (int i = 0; i < dimension; i++)
            {
                suma += componentes[i] * componentes[i];
            }
            return Math.Sqrt(suma);
        }
        
        public double SimilitudCoseno(Vector otro)
        {
            double productoPunto = this * otro;  
            double magnitudA = this.Magnitud();
            double magnitudB = otro.Magnitud();

            if (magnitudA == 0.0 || magnitudB == 0.0)
                return 0.0;

            return productoPunto / (magnitudA * magnitudB);
        }
        
        public double[] ToArray()
        {
            double[] copia = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                copia[i] = componentes[i];
            }
            return copia;
        }
    }

using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Index;
using BuscadorIndiceInvertido.ProcesamientoDatos;
using BuscadorIndiceInvertido.Utilidades;




namespace BuscadorIndiceInvertido.ContoladorView
{
    internal class Controller
    {
        private static DoubleList<Doc> documentos;
        private static IndiceInvertido indice;
        private static MotorBusqueda motor;
        private static bool sistemaInicializado = false;

        public static bool Iniciar()
        {
            string rutaDocumentos = @"C:\Users\castr\Desktop\Documentos";
            try
            {
                ProcesadorDoc processor = new ProcesadorDoc();
                documentos = processor.ProcesarDocumentos(rutaDocumentos);

                if (documentos == null || documentos.Count == 0)
                {
                    Console.WriteLine("No se encontraron documentos para procesar");
                    sistemaInicializado = false;
                    return false;

                }

                sistemaInicializado = true;
                return true;

            }
            catch (Exception)
            {
                Console.WriteLine("Error al procesar los documentos");
                sistemaInicializado = false;
                return false;
            }
        }

        public static bool ConstruirIndice(double percentil)
        {
            if (!sistemaInicializado || documentos == null)
            {
                Console.WriteLine("El sistema no ha sido inicializado correctamente.");
                sistemaInicializado = false;
                return false;
            }

            try
            {
                indice = new IndiceInvertido();
                indice.Build(documentos, percentil);

                motor = new MotorBusqueda(indice);
                sistemaInicializado = true;
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error al construir el indice");
                sistemaInicializado = false;
                return false;
            }
        }

        public static bool Inicializar(double percentil)
        {
            
            return Iniciar() && ConstruirIndice(percentil);
        }

        public static void Buscar()
        {
            if (!sistemaInicializado || motor == null)
            {
                Console.WriteLine("El sistema no ha sido inicializado correctamente.");
                return;
            }
            motor.IniciarInterfazUsuario();
        }

    }
}

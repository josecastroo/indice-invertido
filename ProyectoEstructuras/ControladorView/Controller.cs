using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Index;
using BuscadorIndiceInvertido.ProcesamientoDatos;
using BuscadorIndiceInvertido.Utilidades;

namespace BuscadorIndiceInvertido.ContoladorView
{
    internal class Controller
    {
        private static Controller? _instance;
        private DoubleList<Doc>? documentos;
        private IndiceInvertido? indice;
        private MotorBusqueda? motor;
        private bool sistemaInicializado = false;

        private Controller() { }

        public static Controller Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Controller();
                }
                return _instance;
            }
        }

        public bool Iniciar()
        {
            string rutaDocumentos = @"C:\Users\bryan\RiderProjects\indice-invertido\Documentos";

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
            catch (Exception e)
            {
                // Mostrar toda la información de la excepción
                Console.WriteLine("EXCEPCIÓN DETECTADA:");
                Console.WriteLine(e.GetType().FullName);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                sistemaInicializado = false;
                return false;
            }
        }

        public bool BuildIndice(double percentil)
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
            catch (Exception e)
            {
                Console.WriteLine("Error al construir el índice: " + e.Message);
                Console.WriteLine(e.StackTrace);
                sistemaInicializado = false;
                return false;
            }
        }

        public bool Inicializar(double percentil)
        {
            return Iniciar() && BuildIndice(percentil);
        }

        public void Buscar()
        {
            if (!sistemaInicializado || motor == null)
            {
                Console.WriteLine("El sistema no ha sido inicializado correctamente.");
                return;
            }
            motor.IniciarInterfazUsuario();
        }

        public bool TieneIndiceDisponible()
        {
            return sistemaInicializado && indice != null;
        }

        public IndiceInvertido ObtenerIndice()
        {
            if (TieneIndiceDisponible())
                return indice;
            return null;
        }

        public bool ConfigurarConIndiceCargado(IndiceInvertido indiceCargado)
        {
            if (indiceCargado == null) return false;

            try
            {
                indice = indiceCargado;
                motor = new MotorBusqueda(indice);
                sistemaInicializado = true;
                return true;
            }
            catch (Exception)
            {
                sistemaInicializado = false;
                return false;
            }
        }
    }
}
using BuscadorIndiceInvertido.ContoladorView;
using System;

namespace BuscadorIndiceInvertido.Interfaz
{
    public class IniciarSistema
    {
        public static void IniciarMenu()
        {
            MostrarBienvenida();

            int opcion = -1;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Iniciarlizar búsqueda");
                Console.WriteLine("2. Guardar en archivos");
                Console.WriteLine("3. Cargar de archivos");
                Console.WriteLine("0. Salir");
                Console.WriteLine("Ingrese la opción que desea realizar: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out opcion))
                {
                    Console.WriteLine("Debe ingresar un número válido.");
                    continue;
                }
                
                switch (opcion)
                {
                    case 1:
                        IniciarSistemaBusqueda();
                        break;

                    case 2:
                        // guardar en archivos
                        break;

                    case 3:
                        // cargar en archivos
                        break;

                    case 0:
                        Console.WriteLine("Saliendo del sistema...");
                        return;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
            }
        }

        public static void IniciarSistemaBusqueda()
        {
            Console.WriteLine("Iniciando Sistema de Búsqueda...");
            Console.WriteLine(new string('-', 50));

            Console.Write("Procesando documentos... ");
            if (!Controller.Iniciar())
            {
                Console.WriteLine("No se pudo inicializar el sistema.");
                EsperarTecla();
                return;
            }

            double percentil = ObtenerPercentilUsuario();

            Console.Write("Construyendo índice invertido... ");
            if (!Controller.ConstruirIndice(percentil))
            {
                Console.WriteLine("No se pudo construir el índice.");
                EsperarTecla();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Sistema inicializado correctamente");
            Console.WriteLine("Listo para realizar búsquedas");
            Console.WriteLine();

            Controller.Buscar();
        }

        private static double ObtenerPercentilUsuario()
        {
            double percentil;
            while (true)
            {
                Console.WriteLine();
                Console.Write("Ingrese el percentil de palabras a eliminar (rango 0,0 - 0,7): ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out percentil) && percentil >= 0.0 && percentil <= 0.7)
                {
                    return percentil;
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, ingrese un número entre 0.0 y 0.7.");
                }
            }
        }

        private static void MostrarBienvenida()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║        SISTEMA DE BÚSQUEDA POR ÍNDICE INVERTIDO      ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        private static void EsperarTecla()
        {
            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        class SISGECDOC
        {
            // =========================================================================
            // VARIABLES GLOBALES Y ARREGLOS PARALELOS 
            // =========================================================================
            static int max_exp = 100;
            static string[] codigos = new string[max_exp];
            static string[] fechas = new string[max_exp];
            static string[] dnis = new string[max_exp];
            static string[] nombresExp = new string[max_exp];
            static string[] asuntos = new string[max_exp];
            static int[] foliosExp = new int[max_exp];
            static string[] areas = new string[max_exp];
            static string[] estados = new string[max_exp];
            static string[] operadores = new string[max_exp];

            static string rutaArchivo = "expedientes.txt";
            static int cant_exp = 0;
            static string usuarioActual = "";

            // =========================================================================
            // MÓDULO DE TÍTULO / ENCABEZADO
            // =========================================================================
            static public void Titulo()
            {
                Console.WriteLine("**************************************************");
                Console.WriteLine("   SISTEMA MESA DE PARTES - CENTRO DE SALUD MIRONES");
                Console.WriteLine("**************************************************");
            }

            // =========================================================================
            // MÓDULO: CONTROL DE ACCESO
            // =========================================================================
            static public bool Control_acceso()
            {
                int intentos = 0;
                bool autenticado = false;

                while (intentos < 3 && !autenticado)
                {
                    Console.Clear();
                    Titulo();
                    Console.WriteLine("*** CONTROL DE ACCESO ***");
                    Console.Write("Ingrese usuario: ");
                    string user = Console.ReadLine()?.Trim();
                    Console.Write("Ingrese contraseña: ");
                    string pass = Console.ReadLine()?.Trim();

                    if ((user == "admin" && pass == "salud2026") ||
                        (user == "operador1" && pass == "mirones2026") ||
                        (user == "carlos_mirones" && pass == "mirones2026"))
                    {
                        autenticado = true;
                        usuarioActual = user;
                        Console.WriteLine("[ÉXITO] Acceso concedido. Bienvenido(a), " + usuarioActual);
                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        intentos++;
                        Console.WriteLine("[ERROR] Credenciales incorrectas. Intentos restantes: " + (3 - intentos));
                        if (intentos < 3)
                        {
                            Console.WriteLine("Presione una tecla para reintentar...");
                            Console.ReadKey();
                        }
                    }
                }
                return autenticado;
            }

            // =========================================================================
            // MÓDULO DE VALIDACIÓN DE DATOS DE ENTRADA
            // =========================================================================
            static public bool Validar_datos(string dni, string nombres, int folios)
            {
                bool esValido = true;

                if (dni.Length != 8 || !long.TryParse(dni, out _))
                {
                    Console.WriteLine("[ERROR] El DNI debe contener exactamente 8 dígitos numéricos.");
                    esValido = false;
                }

                if (string.IsNullOrWhiteSpace(nombres))
                {
                    Console.WriteLine("[ERROR] El nombre del ciudadano no puede estar vacío.");
                    esValido = false;
                }

                if (folios <= 0)
                {
                    Console.WriteLine("[ERROR] La cantidad de folios debe ser un número entero mayor a 0.");
                    esValido = false;
                }

                return esValido;
            }

            // =========================================================================
            // MÓDULO DE GENERACIÓN DE CÓDIGO CORRELATIVO
            // =========================================================================
            static public string Generar_correlativo(int numero)
            {
                if (numero < 10)
                    return "EXP-2026-00" + numero;
                else if (numero < 100)
                    return "EXP-2026-0" + numero;
                else
                    return "EXP-2026-" + numero;
            }

        }
    }
}   
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
            // =========================================================================
            // MÓDULO 1: REGISTRO DE EXPEDIENTE Y IMPRESIÓN DE CARGO DIGITAL
            // =========================================================================
            static public void Registro_expediente()
            {
                Console.Clear();
                Console.WriteLine("*** REGISTRAR NUEVO EXPEDIENTE ***");
                Console.WriteLine("*********************************");

                if (cant_exp >= max_exp)
                {
                    Console.WriteLine("Alcanzó la capacidad máxima de datos (" + max_exp + " expedientes)");
                    return;
                }

                Console.Write("Ingrese DNI del ciudadano (8 dígitos): ");
                string dni = Console.ReadLine()?.Trim();

                Console.Write("Ingrese Nombres y Apellidos del solicitante: ");
                string nombres = Console.ReadLine()?.Trim();

                Console.Write("Ingrese número de folios (hojas): ");
                if (!int.TryParse(Console.ReadLine(), out int folios))
                {
                    folios = 0;
                }

                // Validar datos de entrada
                if (!Validar_datos(dni, nombres, folios))
                {
                    Console.WriteLine("[REGISTRO CANCELADO] Por favor corrija los datos e intente nuevamente.");
                    return;
                }

                Console.Write("Ingrese el Asunto / Trámite solicitado: ");
                string asunto = Console.ReadLine()?.Trim();

                Console.Write("Ingrese el Área de Destino (ej. Admisión, Historias Clínicas, Dirección): ");
                string areaDestino = Console.ReadLine()?.Trim();

                // Asignación de correlativo y fecha
                string codigoNuevo = Generar_correlativo(cant_exp + 1);
                string fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                string estadoInicial = "Recibido";

                // Almacenar en arreglos paralelos
                codigos[cant_exp] = codigoNuevo;
                fechas[cant_exp] = fechaHora;
                dnis[cant_exp] = dni;
                nombresExp[cant_exp] = nombres;
                asuntos[cant_exp] = asunto;
                foliosExp[cant_exp] = folios;
                areas[cant_exp] = areaDestino;
                estados[cant_exp] = estadoInicial;
                operadores[cant_exp] = usuarioActual;

                cant_exp++;

                // Guardar automáticamente en el archivo de datos
                Guardar_datos_Archivo();
              

                // Imprimir Cargo Digital
                Console.WriteLine("================================================================================");
                Console.WriteLine("              CONSTANCIA DE RECEPCIÓN - CARGO DIGITAL                          ");
                Console.WriteLine("                  CENTRO DE SALUD MIRONES - MINSA                               ");
                Console.WriteLine("================================================================================");
                Console.WriteLine(" N° EXPEDIENTE   : " + codigoNuevo);
                Console.WriteLine(" FECHA Y HORA    : " + fechaHora);
                Console.WriteLine(" CIUDADANO/DNI   : " + nombres + " (DNI: " + dni + ")");
                Console.WriteLine(" TRÁMITE / ASUNTO: " + asunto);
                Console.WriteLine(" N° FOLIOS       : " + folios);
                Console.WriteLine(" ÁREA DESTINO    : " + areaDestino);
                Console.WriteLine(" ESTADO INICIAL  : RECIBIDO");
                Console.WriteLine(" OPERADOR        : " + usuarioActual);
                Console.WriteLine("================================================================================");
                Console.WriteLine("[ÉXITO] Expediente registrado y guardado permanentemente.");
            }

            // =========================================================================
            // MÓDULO 2: BÚSQUEDA SECUENCIAL DE EXPEDIENTES
            // =========================================================================
            static public void buscar_expediente()
            {
                Console.Clear();
                Console.WriteLine("************ BÚSQUEDA DE EXPEDIENTE ************");
                Console.WriteLine("************************************************");

                if (cant_exp == 0)
                {
                    Console.WriteLine("No hay registro de expedientes cargados en el sistema.");
                    return;
                }

                Console.Write("Ingresar DNI o Código de expediente a buscar: ");
                string buscar = Console.ReadLine()?.Trim().ToUpper();

                if (string.IsNullOrEmpty(buscar))
                    return;

                bool encontrado = false;

                for (int i = 0; i < cant_exp; i++)
                {
                    if (codigos[i].ToUpper() == buscar || dnis[i] == buscar)
                    {
                        encontrado = true;
                        Console.WriteLine("----------------------------------------------");
                        Console.WriteLine("Código       : " + codigos[i]);
                        Console.WriteLine("Fecha y Hora : " + fechas[i]);
                        Console.WriteLine("DNI          : " + dnis[i]);
                        Console.WriteLine("Ciudadano    : " + nombresExp[i]);
                        Console.WriteLine("Asunto       : " + asuntos[i]);
                        Console.WriteLine("Folios       : " + foliosExp[i]);
                        Console.WriteLine("Área Destino : " + areas[i]);
                        Console.WriteLine("Estado       : " + estados[i]);
                        Console.WriteLine("Operador     : " + operadores[i]);
                        Console.WriteLine("----------------------------------------------");
                    }
                }

                if (!encontrado)
                {
                    Console.WriteLine("No se encuentran coincidencias de expediente con: " + buscar);
                }
            }

            // =========================================================================
            // MÓDULO 3: DERIVACIÓN DE EXPEDIENTE
            // =========================================================================
            static public void Derivar_expediente()
            {
                Console.Clear();
                Console.WriteLine("************ DERIVACIÓN DE EXPEDIENTE ************");
                Console.WriteLine("**************************************************");

                if (cant_exp == 0)
                {
                    Console.WriteLine("No hay expedientes registrados para derivar.");
                    return;
                }

                Console.Write("Ingrese Código de Expediente a derivar (ej. EXP-2026-001): ");
                string buscarCod = Console.ReadLine()?.Trim().ToUpper();

                bool encontrado = false;

                for (int i = 0; i < cant_exp; i++)
                {
                    if (codigos[i].ToUpper() == buscarCod)
                    {
                        encontrado = true;
                        Console.WriteLine("Expediente encontrado.");
                        Console.WriteLine("Área Actual : " + areas[i]);
                        Console.WriteLine("Estado Actual: " + estados[i]);

                        Console.Write("Ingrese la nueva Área de Destino: ");
                        string nuevaArea = Console.ReadLine()?.Trim();

                        if (!string.IsNullOrEmpty(nuevaArea))
                        {
                            areas[i] = nuevaArea;
                            estados[i] = "Derivado";

                            // Actualizar archivo .txt
                            Guardar_datos_Archivo();

                            Console.WriteLine("[ÉXITO] Expediente " + codigos[i] + " derivado correctamente a " + nuevaArea);
                        }
                        else
                        {
                            Console.WriteLine("[ERROR] El área de destino no puede estar vacía.");
                        }
                        break;
                    }
                }

                if (!encontrado)
                {
                    Console.WriteLine("[ERROR] El código de expediente ingresado no existe.");
                }
            }

            // =========================================================================
            // MÓDULO 4: REIMPRESIÓN DE CARGO DIGITAL
            // =========================================================================
            static public void Reimprimir_cargo()
            {
                Console.Clear();
                Console.WriteLine("** GENERAR / REIMPRIMIR CARGO DIGITAL **");
                Console.WriteLine("****************");

                Console.Write("Ingrese Código de Expediente: ");
                string buscarCod = Console.ReadLine()?.Trim().ToUpper();

                bool encontrado = false;

                for (int i = 0; i < cant_exp; i++)
                {
                    if (codigos[i].ToUpper() == buscarCod)
                    {
                        encontrado = true;
                        Console.WriteLine("================================================================================");
                        Console.WriteLine("              CONSTANCIA DE RECEPCIÓN - CARGO DIGITAL                          ");
                        Console.WriteLine("                  CENTRO DE SALUD MIRONES - MINSA                               ");
                        Console.WriteLine("================================================================================");
                        Console.WriteLine(" N° EXPEDIENTE   : " + codigos[i]);
                        Console.WriteLine(" FECHA Y HORA    : " + fechas[i]);
                        Console.WriteLine(" CIUDADANO/DNI   : " + nombresExp[i] + " (DNI: " + dnis[i] + ")");
                        Console.WriteLine(" TRÁMITE / ASUNTO: " + asuntos[i]);
                        Console.WriteLine(" N° FOLIOS       : " + foliosExp[i]);
                        Console.WriteLine(" ÁREA DESTINO    : " + areas[i]);
                        Console.WriteLine(" ESTADO          : " + estados[i]);
                        Console.WriteLine(" OPERADOR        : " + operadores[i]);
                        Console.WriteLine("================================================================================");
                        break;
                    }
                }

                if (!encontrado)
                {
                    Console.WriteLine("[ERROR] El código de expediente ingresado no existe.");
                }
            }

            // =========================================================================
            // MÓDULO 5: GUARDAR DATOS EN ARCHIVO (.txt) (StreamWriter)
            // Estilo exacto del ejercicio Caso-Empleado de clase
            // =========================================================================
            static public void Guardar_datos_Archivo()
            {
                if (cant_exp == 0)
                {
                    Console.WriteLine("No hay registros para guardar.");
                    return;
                }

                try
                {
                    using (StreamWriter escribir = new StreamWriter(rutaArchivo, false))
                    {
                        for (int i = 0; i < cant_exp; i++)
                        {
                            escribir.WriteLine(codigos[i] + "|" + fechas[i] + "|" + dnis[i] + "|" + nombresExp[i] + "|" + asuntos[i] + "|" + foliosExp[i] + "|" + areas[i] + "|" + estados[i] + "|" + operadores[i]);
                        }
                    }
                    Console.WriteLine("[" + cant_exp + " expedientes guardados en archivo " + rutaArchivo + "]");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[ERROR AL GUARDAR ARCHIVO]: " + ex.Message);
                }
            }

        }
    }
}
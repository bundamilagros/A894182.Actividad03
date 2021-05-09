using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace A894182.Actividad03
{
    class Program
    {
        static void Main(string[] args) {

            Libro l = new Libro();

            int rtdo = MostrarMenu();
            Boolean run = true;

            while (run)
            {
                switch (rtdo)
                {
                    case 1:
                       
                        Asiento a = nuevoAsiento(l, new Asiento());
                       
                        while (a.Debe != a.Haber)
                        {
                            Console.WriteLine("\n--------------------------------\n");
                            Console.WriteLine("Error. Las cuentas de Debe y Haber no son equivalentes.");
                            Console.WriteLine("\n--------------------------------\n");
                            Console.WriteLine("Intente de nuevo.\n");
                            a.vaciarAsiento();
                            a = nuevoAsiento(l, a);
                        }
                        if (a.Debe == a.Haber)
                        {                            
                            a.Numero = l.NroAsientos;
                            l.NroAsientos++;
                            l.Asientos.Add(a);
                        }

                        using (StreamWriter sw = new StreamWriter("Diario.txt"))
                        {
                            foreach (Asiento asiento in l.Asientos)
                            {

                                sw.WriteLine(asiento.Numero + " | " + asiento.Fecha + " |  CODIGO |  DEBE  |  HABER \n");
                                foreach (Cuenta c in asiento.CuentasDebe)
                                {
                                    sw.WriteLine("   |                        | " + c.Code + " | " + c.Monto + " | ");
                                }
                                foreach (Cuenta c in asiento.CuentasHaber)
                                {
                                    sw.WriteLine("   |                        | " + c.Code + " |   | " + c.Monto);
                                }
                            }
                        }
                        
                        rtdo = MostrarMenu();
                        break;
                    case 2:
                        Console.WriteLine("El libro diario contiene los siguientes asientos: \n");
                        using (StreamReader leer = new StreamReader("Diario.txt"))
                        {
                            string lectura;
                            while ((lectura = leer.ReadLine()) != null) { 
                            Console.WriteLine(lectura);                          
                            }
                        }
                        rtdo = MostrarMenu();
                        break;
                    case 3:
                        Console.WriteLine("Presione cualquier tecla para salir.\n");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Opción erronea. Intente de nuevo.\n");
                        rtdo = MostrarMenu();
                        break;
                }
            }
        }
    

    public static int Validar(String input)
    {
        Boolean opcionOk = int.TryParse(input, out int rtdo);
        while (!opcionOk || rtdo < 0)
        {
            Console.WriteLine("La opcion no es valida. Intente de nuevo.\n");
            opcionOk = int.TryParse(Console.ReadLine(), out rtdo);
        }
        return rtdo;
    }

    public static Boolean ValidarYN(String input)
    {
        input = input.ToUpper();
        Boolean seguir = false;

        while (!input.Equals("S") && !input.Equals("N"))
        {
            Console.WriteLine("La opcion no es valida. Intente de nuevo.\n");
            input = Console.ReadLine();
        }
        if (input.ToUpper().Equals("S"))
        {
            seguir = true;
        }
        if (input.ToUpper().Equals("N"))
        {
            seguir = false;
        }
        return seguir;
    }

    public static DateTime ValidarFecha(String input)
    {
        Boolean opcionOk = DateTime.TryParse(input, out DateTime rtdo);
        while (!opcionOk)
        {
            Console.WriteLine("La fecha no es valida. Intente de nuevo.\n");
            opcionOk = DateTime.TryParse(Console.ReadLine(), out rtdo);
        }
        return rtdo;
    }
    public static int MostrarMenu()
    {
        Console.WriteLine("\n Menú: \n");
        Console.WriteLine("1- Cargar asiento.\n");
        Console.WriteLine("2- Leer asientos.\n");
        Console.WriteLine("3- Salir.\n");
        String input = Console.ReadLine();
        return Validar(input);
    }

    public static Cuenta ValidarCode(List<Cuenta> cuentas, string code)
    {
        int codeInput = Validar(code);
        Cuenta encontrada = null;
            Boolean seguir = true;
            while (seguir) {
            foreach (Cuenta c in cuentas)
            {
                if (c.Code == codeInput)
                    {
                        seguir = false;
                        encontrada = c;
                        break;
                    }
                }
             if (encontrada == null)
             {
                Console.WriteLine("El código no es correcto. Intente de nuevo.\n");
                codeInput = Validar(Console.ReadLine());
             }
            }
            
        return encontrada;
    }

    public static Asiento nuevoAsiento(Libro l, Asiento a)
    {

        Console.WriteLine("\nIngrese la fecha en formato dd-MM-yyyy :\n");
        a.Fecha = ValidarFecha(Console.ReadLine());

        Console.WriteLine("\nA continuación se cargarán las cuentas en el DEBE:\n");
        Console.WriteLine("\nIngrese el código de cuenta:\n");

        Cuenta cuenta = ValidarCode(l.Cuentas, Console.ReadLine());
        Console.WriteLine("\nIngrese el monto:\n");
        int monto = Validar(Console.ReadLine());
        cuenta.Monto = monto;
        a.Debe += monto;
        a.CuentasDebe.Add(cuenta);
        Console.WriteLine("\n¿Desea cargar otra cuenta al DEBE?\n");
        Console.WriteLine("S -Si\n");
        Console.WriteLine("N -No\n");
        Boolean seguir = ValidarYN(Console.ReadLine());
        while (seguir)
        {
            Console.WriteLine("\nIngrese el código de cuenta:\n");
            cuenta = ValidarCode(l.Cuentas, Console.ReadLine());           
            Console.WriteLine("\nIngrese el monto:\n");
            monto = Validar(Console.ReadLine());
                cuenta.Monto = monto;
                a.Debe += monto;
            a.CuentasDebe.Add(cuenta);
            Console.WriteLine("\n¿Desea cargar otra cuenta al DEBE?\n");
            Console.WriteLine("S -Si\n");
            Console.WriteLine("N -No\n");
            seguir = ValidarYN(Console.ReadLine());
        }

        Console.WriteLine("\nA continuación se cargarán las cuentas en el HABER:\n");
        Console.WriteLine("\nIngrese el código de cuenta:\n");
        cuenta = ValidarCode(l.Cuentas, Console.ReadLine());   
        Console.WriteLine("\nIngrese el monto:\n");
        monto = Validar(Console.ReadLine());
        a.Haber += monto;
            cuenta.Monto = monto;
            a.CuentasHaber.Add(cuenta);
        Console.WriteLine("\n¿Desea cargar otra cuenta al HABER?\n");
        Console.WriteLine("S -Si\n");
        Console.WriteLine("N -No\n");
        Boolean seguir2 = ValidarYN(Console.ReadLine());
        while (seguir2)
        {
            Console.WriteLine("\nIngrese el código de cuenta:\n");
            cuenta = ValidarCode(l.Cuentas, Console.ReadLine());           
            Console.WriteLine("\nIngrese el monto:\n");
            monto = Validar(Console.ReadLine());
            a.Haber += monto;
                cuenta.Monto = monto;
                a.CuentasHaber.Add(cuenta);
            Console.WriteLine("\n¿Desea cargar otra cuenta al HABER?\n");
            Console.WriteLine("S -Si\n");
            Console.WriteLine("N -No\n");
            seguir2 = ValidarYN(Console.ReadLine());
        }

        return a;
    }
}
}


    class Cuenta
    {
        private int code;
        private String nombre;
        private int tipo;
        private double monto;

    public Cuenta(int code, string nombre,int tipo)
    {
        Code = code;
        Nombre = nombre;
        Tipo = tipo;
    }

        public int Code { get => code; set => code = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public double Monto { get => monto; set => monto = value; }
}

    class Asiento
    {
    private DateTime fecha;
    private int numero;
    private List<Cuenta> cuentasDebe = new List<Cuenta>();
    private List<Cuenta> cuentasHaber = new List<Cuenta>();
    private double debe;
    private double haber;

    public DateTime Fecha { get => fecha; set => fecha = value; }
    public int Numero { get => numero; set => numero = value; }
    public double Debe { get => debe; set => debe = value; }
    public double Haber { get => haber; set => haber = value; }
    public List<Cuenta> CuentasDebe { get => cuentasDebe; set => cuentasDebe = value; }
    public List<Cuenta> CuentasHaber { get => cuentasHaber; set => cuentasHaber = value; }


    public void vaciarAsiento() {
        this.Haber = 0;
        this.Debe = 0;
        this.CuentasDebe.Clear();
        this.CuentasHaber.Clear();
    }
}

    class Libro
    {
    private List<Cuenta> cuentas = new List<Cuenta>();
    private int nroAsientos = 1;
    private List<Asiento> asientos = new List<Asiento>();
      
    public List<Cuenta> Cuentas { get => cuentas; set => cuentas = value; }
    public int NroAsientos { get => nroAsientos; set => nroAsientos = value; }
    public List<Asiento> Asientos { get => asientos; set => asientos = value; }

    public Libro() {

        this.Cuentas.Add(new Cuenta(11, "Construcciones en procesos", 1));
        this.Cuentas.Add(new Cuenta(12, "Otras propiedades, planta y equipo", 1));
        this.Cuentas.Add(new Cuenta(13, "Activos intangibles y plusvalía", 1));
        this.Cuentas.Add(new Cuenta(14, "Marcas comerciales", 1));
        this.Cuentas.Add(new Cuenta(15, "Programas de computador", 1));
        this.Cuentas.Add(new Cuenta(16, "Licencias y franquicias", 1));
        this.Cuentas.Add(new Cuenta(17, "Derechos de propiedad intelectual, patentes y otros", 1));
        this.Cuentas.Add(new Cuenta(18, "Recetas, fórmulas, modelos, diseños y prototipos", 1));
        this.Cuentas.Add(new Cuenta(19, "Activos intangibles en desarrollo", 1));
        this.Cuentas.Add(new Cuenta(20, "Inversiones en subsidiarias, negocios conjuntos y asociadas", 1));
        this.Cuentas.Add(new Cuenta(21, "Deudores comerciales", 1));
        this.Cuentas.Add(new Cuenta(22, "Cuentas por cobrar", 1));
        this.Cuentas.Add(new Cuenta(23, "Activos financieros", 1));
        this.Cuentas.Add(new Cuenta(24, "Efectivo en caja", 1));
        this.Cuentas.Add(new Cuenta(25, "Cuentas corrientes", 1));
        this.Cuentas.Add(new Cuenta(26, "Depósitos a corto plazo", 1));
        this.Cuentas.Add(new Cuenta(27, "Inversiones a corto plazo", 1));
        this.Cuentas.Add(new Cuenta(28, "Cuentas por pagar comerciales", 2));
        this.Cuentas.Add(new Cuenta(29, "Impuestos por pagar", 2));
        this.Cuentas.Add(new Cuenta(30, "Préstamos bancarios", 2));
        this.Cuentas.Add(new Cuenta(31, "Obligaciones con el público", 2));
        this.Cuentas.Add(new Cuenta(32, "Obligaciones por leasing", 2));
        this.Cuentas.Add(new Cuenta(33, "Capital emitido", 3));
        this.Cuentas.Add(new Cuenta(34, "Ganancias (pérdidas) acumuladas", 3));
    }
}


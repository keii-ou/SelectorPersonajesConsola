using System.Globalization;
using System.Text;
using SelectorPersonajes;

Console.OutputEncoding = Encoding.UTF8;

IListaPersonajes lista = ListaFactory.Crear(ElegirTipo(null));
foreach (var p in Personaje.Predeterminados()) lista.Agregar(p);

int indice = 0;            // si indice == lista.Count → casilla "Añadir nuevo"
string mensaje = "";
bool salir = false;

while (!salir)
{
    Dibujar(lista, indice, mensaje);
    mensaje = "";

    int total = lista.Count + 1;
    var tecla = Console.ReadKey(true);

    switch (tecla.Key)
    {
        case ConsoleKey.LeftArrow:
        case ConsoleKey.A:
            indice = (indice - 1 + total) % total;
            break;

        case ConsoleKey.RightArrow:
        case ConsoleKey.D:
            indice = (indice + 1) % total;
            break;

        case ConsoleKey.Enter:
            if (indice == lista.Count)
            {
                var nuevo = PedirPersonaje(null);
                lista.Agregar(nuevo);
                indice = lista.Count - 1;
                mensaje = $"✔ '{nuevo.Name}' añadido.";
            }
            else
            {
                mensaje = "Usa E (editar) o X (eliminar).";
            }
            break;

        case ConsoleKey.E:
            if (indice == lista.Count)
            {
                mensaje = "No hay nada que editar aquí.";
                break;
            }
            lista[indice].CopiarDe(PedirPersonaje(lista[indice]));
            mensaje = "✔ Personaje editado.";
            break;

        case ConsoleKey.X:
        case ConsoleKey.Delete:
            if (indice == lista.Count)
            {
                mensaje = "No puedes eliminar la casilla de añadir.";
                break;
            }
            Console.Write($"\n¿Eliminar a '{lista[indice].Name}'? (S/N): ");
            if (Console.ReadKey(true).Key == ConsoleKey.S)
            {
                lista.EliminarEn(indice);   // el índice sigue siendo válido (≤ Count)
                mensaje = "✔ Personaje eliminado.";
            }
            break;

        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:
            Ordenar(lista, ref indice, Criterios.NivelDesc, ref mensaje, "level (mayor → menor)");
            break;

        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:
            Ordenar(lista, ref indice, Criterios.DanoDesc, ref mensaje, "attack (mayor → menor)");
            break;

        case ConsoleKey.D3:
        case ConsoleKey.NumPad3:
            Ordenar(lista, ref indice, Criterios.VelocidadAsc, ref mensaje, "velocidad (menor → mayor)");
            break;

        case ConsoleKey.L:
            var tipo = ElegirTipo(lista.Tipo);
            if (tipo != lista.Tipo)
            {
                lista = ListaFactory.Convertir(lista, tipo);
                mensaje = $"✔ Ahora se usa: {lista.Nombre}.";
            }
            break;

        case ConsoleKey.B:
            CorrerBenchmark(lista);
            break;

        case ConsoleKey.Escape:
            salir = true;
            break;
    }
}

// ───────────────────────── Funciones ─────────────────────────

static TipoLista ElegirTipo(TipoLista? actual)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("── TIPO DE LISTA ──\n");
        Console.WriteLine("  1. Lista personalizada (nodos doblemente enlazados circulares)");
        Console.WriteLine("  2. LinkedList<T>");
        Console.WriteLine("  3. List<T>");
        Console.Write(actual != null ? "\nElige (1-3) [Enter = mantener]: " : "\nElige (1-3): ");

        string? s = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(s) && actual != null) return actual.Value;

        switch (s)
        {
            case "1": return TipoLista.Personalizada;
            case "2": return TipoLista.LinkedList;
            case "3": return TipoLista.List;
        }
    }
}

static void Ordenar(IListaPersonajes lista, ref int indice, Comparison<Personaje> criterio,
    ref string mensaje, string descripcion)
{
    var seleccionado = indice < lista.Count ? lista[indice] : null;
    lista.Ordenar(criterio);

    // Mantiene la selección sobre el mismo personaje
    if (seleccionado != null) indice = lista.IndiceDe(seleccionado);

    mensaje = $"✔ Ordenado por {descripcion}.";
}

static string Resumen(IListaPersonajes lista, int i) =>
    i == lista.Count ? "★ Añadir nuevo" : $"{lista[i].Name} ({lista[i].Species})";

static void Dibujar(IListaPersonajes lista, int indice, string mensaje)
{
    Console.Clear();
    Console.WriteLine("═══════ SELECTOR DE PERSONAJES ═══════");
    Console.WriteLine($"Lista: {lista.Nombre}\n");

    int total = lista.Count + 1;
    Console.WriteLine($"  [{indice + 1}/{total}]\n");
    Console.WriteLine($"  ◄ {Resumen(lista, (indice - 1 + total) % total)}");
    Console.WriteLine($"  ► {Resumen(lista, (indice + 1) % total)}\n");

    if (indice == lista.Count)
    {
        Console.WriteLine("  ╔══════════════════════════════╗");
        Console.WriteLine("  ║   ★ AÑADIR NUEVO PERSONAJE   ║");
        Console.WriteLine("  ╚══════════════════════════════╝");
    }
    else
    {
        var p = lista[indice];
        Console.WriteLine("  ╔══════════════════════════════╗");
        Console.WriteLine($"  ║ Name:   {p.Name}");
        Console.WriteLine($"  ║ Species:  {p.Species}");
        Console.WriteLine($"  ║ Daño:     {p.Attack}");
        Console.WriteLine($"  ║ Vel. atq: {p.AttackSpeed.ToString("0.##", CultureInfo.InvariantCulture)}");
        Console.WriteLine($"  ║ Level:    {p.Level}");
        Console.WriteLine("  ╚══════════════════════════════╝");
    }

    Console.WriteLine("\n ←/A Anterior   →/D Siguiente   Enter Añadir (en ★)");
    Console.WriteLine(" E Editar   X Eliminar   Esc Salir");
    Console.WriteLine(" 1 Ordenar level↓   2 Ordenar attack↓   3 Ordenar velocidad↑");
    Console.WriteLine(" L Cambiar tipo de lista   B Benchmark");

    if (mensaje != "")
        Console.WriteLine($"\n {mensaje}");
}

static void CorrerBenchmark(IListaPersonajes lista)
{
    Console.Clear();
    Console.WriteLine("── BENCHMARK DE LISTAS ──\n");
    Console.WriteLine("Se generan personajes a partir de los actuales y se ejecutan en serie");
    Console.WriteLine("las mismas operaciones sobre las tres listas.\n");

    int n = LeerEntero("Cantidad de personajes", 100_000, 1_000, 500_000);
    int repeticiones = LeerEntero("Repeticiones por lista", 3, 1, 10);

    Console.WriteLine("\nEjecutando (puede tardar unos segundos)…\n");

    var baseDatos = lista.Recorrer().ToList();
    var r = Benchmark.Ejecutar(baseDatos, n, repeticiones, m => Console.WriteLine("  " + m));

    var tipos = Enum.GetValues<TipoLista>();
    string[] titulos = ["Personalizada", "LinkedList<T>", "List<T>"];

    Console.WriteLine();
    Console.WriteLine($"{r.Cantidad:N0} personajes (derivados de {r.PersonajesBase}) · promedio de {r.Repeticiones} repeticiones · ms · ★ = más rápido\n");
    Console.WriteLine($"{"Operación",-34}{titulos[0],16}{titulos[1],16}{titulos[2],16}");
    Console.WriteLine(new string('─', 82));

    for (int i = 0; i < r.Operaciones.Length; i++)
        ImprimirFila(r.Operaciones[i], tipos.Select(t => r.Tiempos[t][i]).ToArray());

    Console.WriteLine(new string('─', 82));
    ImprimirFila("TOTAL", tipos.Select(t => r.Tiempos[t].Sum()).ToArray());

    Console.WriteLine("\nEstadísticas calculadas:");
    for (int i = 0; i < r.Operaciones.Length; i++)
        Console.WriteLine($"  {r.Operaciones[i]}: {r.Detalles[i]}");

    Console.WriteLine(r.Coinciden
        ? "\n Las tres listas produjeron exactamente los mismos resultados."
        : "\n Los resultados difieren entre listas (revisar).");

    Console.WriteLine("\nPulsa una tecla para volver…");
    Console.ReadKey(true);
}

static void ImprimirFila(string operacion, double[] valores)
{
    double minimo = valores.Min();
    Console.Write($"{operacion,-34}");
    foreach (var v in valores)
    {
        string celda = v.ToString("N3") + (v == minimo ? " ★" : "  ");
        Console.Write($"{celda,16}");
    }
    Console.WriteLine();
}

static Personaje PedirPersonaje(Personaje? original)
{
    bool editando = original != null;
    Console.Clear();
    Console.WriteLine(editando
        ? "── EDITAR PERSONAJE (Enter = mantener valor) ──\n"
        : "── NUEVO PERSONAJE ──\n");

    string name = LeerTexto("Name", original?.Name);
    string species = LeerEspecie(original?.Species);
    int attack = LeerEntero("Daño de ataque", original?.Attack, 0);
    double attackSpeed = LeerDecimal("Velocidad de ataque", original?.AttackSpeed);
    int level = LeerEntero("Level", original?.Level, 1);

    return new Personaje(name, species, attack, attackSpeed, level);
}

static string LeerTexto(string campo, string? Default)
{
    while (true)
    {
        Console.Write(Default != null ? $"{campo} [{Default}]: " : $"{campo}: ");
        string? s = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(s)) return s;
        if (Default != null) return Default;
        Console.WriteLine("  Valor requerido.");
    }
}

static string LeerEspecie(string? Default)
{
    string[] species = Personaje.EspeciesBase;
    Console.WriteLine("Species:");
    for (int i = 0; i < species.Length; i++)
        Console.WriteLine($"  {i + 1}. {species[i]}");
    Console.WriteLine($"  {species.Length + 1}. Otra (escribirla)");

    while (true)
    {
        Console.Write(Default != null ? $"Elige [{Default}]: " : "Elige: ");
        string? s = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(s) && Default != null) return Default;
        if (int.TryParse(s, out int op))
        {
            if (op >= 1 && op <= species.Length) return species[op - 1];
            if (op == species.Length + 1) return LeerTexto("Species personalizada", null);
        }
        Console.WriteLine("  Opción no válida.");
    }
}

static int LeerEntero(string campo, int? Default, int min, int max = int.MaxValue)
{
    while (true)
    {
        Console.Write(Default != null ? $"{campo} [{Default:N0}]: " : $"{campo}: ");
        string? s = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(s) && Default != null) return Default.Value;
        if (int.TryParse(s, NumberStyles.Integer | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out int v)
            && v >= min && v <= max)
            return v;
        Console.WriteLine($"  Introduce un entero entre {min:N0} y {max:N0}.");
    }
}

static double LeerDecimal(string campo, double? porDefecto)
{
    while (true)
    {
        Console.Write(porDefecto != null
            ? $"{campo} [{porDefecto.Value.ToString("0.##", CultureInfo.InvariantCulture)}]: "
            : $"{campo}: ");
        string? s = Console.ReadLine()?.Trim().Replace(',', '.');
        if (string.IsNullOrEmpty(s) && porDefecto != null) return porDefecto.Value;
        if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out double v) && v > 0)
            return v;
        Console.WriteLine("  Introduce un número mayor que 0 (ej. 1.5).");
    }
}

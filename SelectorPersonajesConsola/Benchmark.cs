using System.Diagnostics;

namespace SelectorPersonajes;

public class ResultadoBenchmark
{
    public int Cantidad { get; init; }
    public int PersonajesBase { get; init; }
    public int Repeticiones { get; init; }
    public required string[] Operaciones { get; init; }

    public required string[] Detalles { get; init; }

    public required Dictionary<TipoLista, double[]> Tiempos { get; init; }

    public bool Coinciden { get; init; }
}


public static class Benchmark
{
    public static ResultadoBenchmark Ejecutar(
        IReadOnlyList<Personaje> baseDatos, int cantidad, int repeticiones, Action<string>? progreso = null)
    {
        var datos = GenerarDatos(baseDatos, cantidad);
        int accesos = Math.Min(500, cantidad);
        int eliminaciones = Math.Max(1, Math.Min(1000, cantidad / 10));

        var tiempos = new Dictionary<TipoLista, double[]>();
        var detallesPorTipo = new Dictionary<TipoLista, string[]>();
        string[] operaciones = [];

        foreach (var tipo in Enum.GetValues<TipoLista>())
        {
            string nombre = ListaFactory.Crear(tipo).Nombre;
            double[] acumulado = [];
            string[] detalles = [];

            // Pasada 0 = calentamiento (JIT), no se promedia
            for (int pasada = 0; pasada <= repeticiones; pasada++)
            {
                progreso?.Invoke(pasada == 0
                    ? $"{nombre}: calentamiento…"
                    : $"{nombre}: pasada {pasada}/{repeticiones}…");

                GC.Collect();
                GC.WaitForPendingFinalizers();

                var ops = UnaPasada(tipo, datos, accesos, eliminaciones);

                if (pasada == 0)
                {
                    operaciones = ops.Select(o => o.Operacion).ToArray();
                    acumulado = new double[ops.Count];
                    continue;
                }

                for (int i = 0; i < ops.Count; i++) acumulado[i] += ops[i].Ms;
                detalles = ops.Select(o => o.Detalle).ToArray();
            }

            tiempos[tipo] = acumulado.Select(ms => ms / repeticiones).ToArray();
            detallesPorTipo[tipo] = detalles;
        }

        var referencia = detallesPorTipo[TipoLista.Personalizada];

        return new ResultadoBenchmark
        {
            Cantidad = cantidad,
            PersonajesBase = baseDatos.Count,
            Repeticiones = repeticiones,
            Operaciones = operaciones,
            Detalles = referencia,
            Tiempos = tiempos,
            Coinciden = detallesPorTipo.Values.All(d => d.SequenceEqual(referencia))
        };
    }

    
    private static Personaje[] GenerarDatos(IReadOnlyList<Personaje> baseDatos, int cantidad)
    {
        IReadOnlyList<Personaje> origen = baseDatos.Count > 0 ? baseDatos : Personaje.Predeterminados();
        var datos = new Personaje[cantidad];

        for (int i = 0; i < cantidad; i++)
        {
            var b = origen[i % origen.Count];
            int ciclo = i / origen.Count;

            datos[i] = new Personaje(
                $"{b.Name} #{i + 1}",
                b.Species,
                b.Attack + (ciclo % 50) * 2,
                Math.Round(b.AttackSpeed + (ciclo % 10) * 0.05, 2),
                b.Level + ciclo % 50);
        }

        return datos;
    }

    private static List<(string Operacion, double Ms, string Detalle)> UnaPasada(
        TipoLista tipo, Personaje[] datos, int accesos, int eliminaciones)
    {
        var lista = ListaFactory.Crear(tipo);
        var ops = new List<(string Operacion, double Ms, string Detalle)>();

        // Mide solo la acción; el detalle (estadística calculada) se arma fuera del cronómetro
        void Op(string nombre, Action accion, Func<string> detalle)
        {
            var sw = Stopwatch.StartNew();
            accion();
            sw.Stop();
            ops.Add((nombre, sw.Elapsed.TotalMilliseconds, detalle()));
        }

        long danoTotal = 0, nivelTotal = 0, sumaNiveles = 0;
        double dpsTotal = 0;
        Personaje? mejor = null;
        var porEspecie = new Dictionary<string, int>();
        long paso = (long)(datos.Length * 0.618) + 1;   // salta por toda la lista

        Op("Insertar personajes",
            () => { foreach (var p in datos) lista.Agregar(p); },
            () => $"{lista.Count:N0} personajes en la lista");

        Op("Daño total y nivel promedio",
            () =>
            {
                foreach (var p in lista.Recorrer())
                {
                    danoTotal += p.Attack;
                    nivelTotal += p.Level;
                }
            },
            () => $"Daño total {danoTotal:N0} | Level promedio {(double)nivelTotal / lista.Count:F2}");

        Op("DPS total (daño × velocidad)",
            () => { foreach (var p in lista.Recorrer()) dpsTotal += p.Dps; },
            () => $"DPS total {dpsTotal:N1} | DPS promedio {dpsTotal / lista.Count:F2}");

        Op("Personaje con mayor DPS",
            () =>
            {
                foreach (var p in lista.Recorrer())
                    if (mejor == null || p.Dps > mejor.Dps) mejor = p;
            },
            () => mejor == null ? "—" : $"{mejor.Name} ({mejor.Dps:F2} DPS)");

        Op("Conteo por especie",
            () =>
            {
                porEspecie.Clear();
                foreach (var p in lista.Recorrer())
                    porEspecie[p.Species] = porEspecie.GetValueOrDefault(p.Species) + 1;
            },
            () => string.Join(" | ", porEspecie
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .Select(kv => $"{kv.Key} {kv.Value:N0}")));

        Op($"Acceso por índice ×{accesos:N0}",
            () =>
            {
                for (int i = 0; i < accesos; i++)
                {
                    int indice = (int)((long)i * paso % lista.Count);
                    sumaNiveles += lista[indice].Level;
                }
            },
            () => $"Suma de niveles {sumaNiveles:N0}");

        Op("Ordenar por nivel ↓",
            () => lista.Ordenar(Criterios.NivelDesc),
            () => $"1.º {lista[0].Name} (nivel {lista[0].Level}) | último: nivel {lista[lista.Count - 1].Level}");

        Op("Ordenar por daño ↓",
            () => lista.Ordenar(Criterios.DanoDesc),
            () => $"1.º {lista[0].Name} (daño {lista[0].Attack}) | último: daño {lista[lista.Count - 1].Attack}");

        Op("Ordenar por velocidad ↑",
            () => lista.Ordenar(Criterios.VelocidadAsc),
            () => $"1.º {lista[0].Name} (vel. {lista[0].AttackSpeed:0.##}) | último: vel. {lista[lista.Count - 1].AttackSpeed:0.##}");

        Op($"Eliminar los primeros {eliminaciones:N0}",
            () => { for (int i = 0; i < eliminaciones; i++) lista.EliminarEn(0); },
            () => $"Quedan {lista.Count:N0} | nuevo primero: {lista[0].Name}");

        return ops;
    }
}

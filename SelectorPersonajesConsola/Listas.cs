namespace SelectorPersonajes;

public enum TipoLista { Personalizada, LinkedList, List }

public interface IListaPersonajes
{
    TipoLista Tipo { get; }
    string Nombre { get; }
    int Count { get; }
    Personaje this[int indice] { get; }
    void Agregar(Personaje p);
    void EliminarEn(int indice);
    void Limpiar();
    int IndiceDe(Personaje p);
    IEnumerable<Personaje> Recorrer();
    void Ordenar(Comparison<Personaje> criterio);
}

public static class Criterios
{
    public static readonly Comparison<Personaje> NivelDesc =
        (a, b) => Desempate(b.Level.CompareTo(a.Level), a, b);

    public static readonly Comparison<Personaje> DanoDesc =
        (a, b) => Desempate(b.Attack.CompareTo(a.Attack), a, b);

    public static readonly Comparison<Personaje> VelocidadAsc =
        (a, b) => Desempate(a.AttackSpeed.CompareTo(b.AttackSpeed), a, b);

    
    private static int Desempate(int comparacion, Personaje a, Personaje b) =>
        comparacion != 0 ? comparacion : string.CompareOrdinal(a.Name, b.Name);
}

public static class ListaFactory
{
    public static IListaPersonajes Crear(TipoLista tipo) => tipo switch
    {
        TipoLista.Personalizada => new ListaPersonalizada(),
        TipoLista.LinkedList => new ListaLinkedList(),
        _ => new ListaNet()
    };

    public static IListaPersonajes Convertir(IListaPersonajes origen, TipoLista tipo)
    {
        var nueva = Crear(tipo);
        foreach (var p in origen.Recorrer()) nueva.Agregar(p);
        return nueva;
    }
}

//Lista personalizada

public class Nodo
{
    public Personaje Datos { get; set; }
    public Nodo Siguiente { get; set; }
    public Nodo Anterior { get; set; }

    public Nodo(Personaje datos)
    {
        Datos = datos;
        Siguiente = this;
        Anterior = this;
    }
}

public class ListaPersonalizada : IListaPersonajes
{
    private Nodo? _cabeza;
    private Nodo? _cursor;        
    private int _indiceCursor;
    private int _count;

    public TipoLista Tipo => TipoLista.Personalizada;
    public string Nombre => "Lista personalizada (nodos)";
    public int Count => _count;

    public Personaje this[int indice] => NodoEn(indice).Datos;

  
    private Nodo NodoEn(int indice)
    {
        if ((uint)indice >= (uint)_count) throw new ArgumentOutOfRangeException(nameof(indice));

        var nodo = _cursor!;
        int adelante = ((indice - _indiceCursor) % _count + _count) % _count;
        int atras = _count - adelante;

        if (adelante <= atras)
        {
            for (int i = 0; i < adelante; i++) nodo = nodo.Siguiente;
        }
        else
        {
            for (int i = 0; i < atras; i++) nodo = nodo.Anterior;
        }

        _cursor = nodo;
        _indiceCursor = indice;
        return nodo;
    }

    public void Agregar(Personaje p)
    {
        var nuevo = new Nodo(p);

        if (_cabeza == null)
        {
            _cabeza = nuevo;
            _cursor = nuevo;
            _indiceCursor = 0;
        }
        else
        {
            var ultimo = _cabeza.Anterior;
            nuevo.Anterior = ultimo;
            nuevo.Siguiente = _cabeza;
            ultimo.Siguiente = nuevo;
            _cabeza.Anterior = nuevo;
        }

        _count++;
    }

    public void EliminarEn(int indice)
    {
        var nodo = NodoEn(indice);

        if (_count == 1)
        {
            _cabeza = null;
            _cursor = null;
        }
        else
        {
            nodo.Anterior.Siguiente = nodo.Siguiente;
            nodo.Siguiente.Anterior = nodo.Anterior;
            if (nodo == _cabeza) _cabeza = nodo.Siguiente;
            _cursor = _cabeza;
        }

        _indiceCursor = 0;
        _count--;
    }

    public void Limpiar()
    {
        _cabeza = null;
        _cursor = null;
        _indiceCursor = 0;
        _count = 0;
    }

    public int IndiceDe(Personaje p)
    {
        var nodo = _cabeza;
        for (int i = 0; i < _count; i++)
        {
            if (ReferenceEquals(nodo!.Datos, p)) return i;
            nodo = nodo.Siguiente;
        }
        return -1;
    }

    public IEnumerable<Personaje> Recorrer()
    {
        var nodo = _cabeza;
        for (int i = 0; i < _count; i++)
        {
            yield return nodo!.Datos;
            nodo = nodo.Siguiente;
        }
    }

    public void Ordenar(Comparison<Personaje> criterio)
    {
        if (_count < 2) return;

        var arreglo = new Personaje[_count];
        var nodo = _cabeza!;
        for (int i = 0; i < _count; i++)
        {
            arreglo[i] = nodo.Datos;
            nodo = nodo.Siguiente;
        }

        Array.Sort(arreglo, criterio);

        nodo = _cabeza!;
        for (int i = 0; i < _count; i++)
        {
            nodo.Datos = arreglo[i];
            nodo = nodo.Siguiente;
        }
    }
}

// LinkedList<T>

public class ListaLinkedList : IListaPersonajes
{
    private readonly LinkedList<Personaje> _lista = new();
    private LinkedListNode<Personaje>? _cursor;
    private int _indiceCursor;

    public TipoLista Tipo => TipoLista.LinkedList;
    public string Nombre => "LinkedList<T>";
    public int Count => _lista.Count;

    public Personaje this[int indice] => NodoEn(indice).Value;

    private LinkedListNode<Personaje> NodoEn(int indice)
    {
        int n = _lista.Count;
        if ((uint)indice >= (uint)n) throw new ArgumentOutOfRangeException(nameof(indice));

        if (_cursor == null)
        {
            _cursor = _lista.First;
            _indiceCursor = 0;
        }

        var nodo = _cursor!;
        int adelante = ((indice - _indiceCursor) % n + n) % n;
        int atras = n - adelante;

        if (adelante <= atras)
        {
            for (int i = 0; i < adelante; i++) nodo = nodo.Next ?? _lista.First!;
        }
        else
        {
            for (int i = 0; i < atras; i++) nodo = nodo.Previous ?? _lista.Last!;
        }

        _cursor = nodo;
        _indiceCursor = indice;
        return nodo;
    }

    public void Agregar(Personaje p) => _lista.AddLast(p);

    public void EliminarEn(int indice)
    {
        _lista.Remove(NodoEn(indice));
        _cursor = null;
        _indiceCursor = 0;
    }

    public void Limpiar()
    {
        _lista.Clear();
        _cursor = null;
        _indiceCursor = 0;
    }

    public int IndiceDe(Personaje p)
    {
        int i = 0;
        for (var nodo = _lista.First; nodo != null; nodo = nodo.Next, i++)
            if (ReferenceEquals(nodo.Value, p)) return i;
        return -1;
    }

    public IEnumerable<Personaje> Recorrer() => _lista;

    public void Ordenar(Comparison<Personaje> criterio)
    {
        if (_lista.Count < 2) return;

        var arreglo = _lista.ToArray();
        Array.Sort(arreglo, criterio);

        int i = 0;
        for (var nodo = _lista.First; nodo != null; nodo = nodo.Next)
            nodo.Value = arreglo[i++];
    }
}

//List<T>

public class ListaNet : IListaPersonajes
{
    private readonly List<Personaje> _lista = new();

    public TipoLista Tipo => TipoLista.List;
    public string Nombre => "List<T>";
    public int Count => _lista.Count;

    public Personaje this[int indice] => _lista[indice];

    public void Agregar(Personaje p) => _lista.Add(p);
    public void EliminarEn(int indice) => _lista.RemoveAt(indice);
    public void Limpiar() => _lista.Clear();
    public int IndiceDe(Personaje p) => _lista.IndexOf(p);  
    public IEnumerable<Personaje> Recorrer() => _lista;
    public void Ordenar(Comparison<Personaje> criterio) => _lista.Sort(criterio);
}

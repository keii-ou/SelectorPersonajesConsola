⚔️ Selector de Personajes (Versión Consola)

Aplicación de consola desarrollada en C# (.NET 10) que permite navegar, añadir, editar, eliminar y ordenar personajes de fantasía desde la terminal, usando el teclado. Además, incluye un benchmark que compara el rendimiento de tres implementaciones de lista: una lista doblemente enlazada circular escrita desde cero y dos listas de .NET (LinkedList<T> y List<T>).

Es la versión en modo texto del proyecto Selector de Personajes; comparte con la versión de Windows Forms el mismo diseño de estructuras de datos y el mismo motor de benchmark.                                                                                                                        Autores 👥

👤 Alexa Abigail Fraire Sandoval

👤 Elizabeth Anabely García Rivera

👤 Celeste Anyelique Garza Mauricio

👤 Rosembert Jared Ortiz Reyes

👤 Lia Kei Uchino Bonney                                                                                                                                                                                    📖 Descripción general

Al iniciar, el programa pide elegir qué tipo de lista usar para guardar los personajes y carga cinco personajes predeterminados. Después muestra una pantalla tipo carrusel en la que se ve el personaje actual, junto con un resumen del anterior y del siguiente. Todo se controla con el teclado: flechas para navegar, letras para editar o eliminar, números para ordenar, L para cambiar de estructura en caliente y B para correr el benchmark.

El propósito académico es demostrar el uso de estructuras de datos y su impacto en el rendimiento. Todo el programa trabaja contra una interfaz común (IListaPersonajes), por lo que se puede cambiar de implementación sin perder los datos ni el orden.

✨ Características
🎴 Navegación circular entre personajes, con una casilla extra «★ Añadir nuevo» al final.
👀 Vista previa del personaje anterior (◄) y del siguiente (►) en cada pantalla.
⌨️ Control total por teclado, sin necesidad de presionar Enter para la mayoría de las acciones.
➕ Alta y edición guiadas con validación de datos y valores por defecto al editar.
🗑️ Eliminación con confirmación (S/N).
🔀 Tres criterios de ordenamiento: nivel, ataque y velocidad de ataque.
🔁 Cambio de estructura en tiempo real entre las tres listas, conservando datos y orden.
📊 Benchmark integrado con 10 operaciones, hasta 500,000 personajes, marca ★ al resultado más rápido y verificación de que las tres listas dan resultados idénticos.
🌐 Multiplataforma: al ser una aplicación de consola (net10.0, no net10.0-windows), puede ejecutarse en cualquier sistema compatible con .NET.
🔤 Salida configurada en UTF-8 para mostrar correctamente acentos y símbolos como ★ ◄ ► ═ ║ ✔ ↓ ↑.
🧰 Requisitos
Requisito	Detalle
Sistema operativo	Windows, Linux o macOS
Framework	.NET 10 (net10.0)
Para ejecutar	.NET 10 Runtime
Para compilar	.NET 10 SDK
Terminal	Interactiva, con soporte UTF-8 y ancho mínimo de unos 85 columnas para que la tabla del benchmark se vea completa

⚠️ El programa lee teclas individuales con Console.ReadKey, por lo que debe ejecutarse en una terminal interactiva. No funciona con entrada redirigida (por ejemplo, con < o tuberías).

🚀 Instalación y ejecución
Opción 1: ejecutar el binario ya compilado

El proyecto incluye una compilación en modo Debug:

SelectorPersonajesConsola/bin/Debug/net10.0/SelectorPersonajesConsola.exe

Ábrelo desde una terminal (CMD, PowerShell o Windows Terminal) para que la ventana no se cierre al terminar.

Opción 2: compilar y ejecutar con la CLI de .NET
bash
cd SelectorPersonajesConsola
dotnet run

Para compilar sin ejecutar:

bash
dotnet build
Opción 3: Visual Studio / Rider / VS Code

Abre el archivo SelectorPersonajesConsola.csproj y ejecuta el proyecto. En Visual Studio, el programa se abrirá en su propia ventana de consola.

🕹️ Guía de uso
1. Selección del tipo de lista (al iniciar)
── TIPO DE LISTA ──

  1. Lista personalizada (nodos doblemente enlazados circulares)
  2. LinkedList<T>
  3. List<T>

Elige (1-3):

Escribe 1, 2 o 3 y presiona Enter. Si se escribe algo distinto, la pregunta se repite. Esta misma pantalla vuelve a aparecer al pulsar L dentro del programa, pero con la opción de mantener la lista actual presionando solo Enter.

2. Pantalla principal
═══════ SELECTOR DE PERSONAJES ═══════
Lista: Lista personalizada (nodos)

  [1/6]

  ◄ ★ Añadir nuevo
  ► Merlín (Mago)

  ╔══════════════════════════════╗
  ║ Name:   Arthur
  ║ Species:  Caballero
  ║ Daño:     45
  ║ Vel. atq: 1.2
  ║ Level:    12
  ╚══════════════════════════════╝

 ←/A Anterior   →/D Siguiente   Enter Añadir (en ★)
 E Editar   X Eliminar   Esc Salir
 1 Ordenar level↓   2 Ordenar attack↓   3 Ordenar velocidad↑
 L Cambiar tipo de lista   B Benchmark
Elemento	Significado
Lista:	Estructura de datos que se está usando
[1/6]	Posición actual / total (el total incluye la casilla de «Añadir nuevo»)
◄ / ►	Resumen del personaje anterior y siguiente (Nombre (Especie))
Recuadro	Datos del personaje actual: nombre, especie, daño, velocidad de ataque y nivel
Mensaje inferior	Confirmación de la última acción (por ejemplo, ✔ Personaje editado.)
3. Controles
Tecla	Acción
← o A	Ir al personaje anterior
→ o D	Ir al personaje siguiente
Enter	En la casilla ★ Añadir nuevo: abre el formulario de alta. En un personaje: muestra el aviso «Usa E (editar) o X (eliminar)»
E	Editar el personaje actual
X o Supr (Delete)	Eliminar el personaje actual (pide confirmación)
1 (o NumPad 1)	Ordenar por nivel de mayor a menor
2 (o NumPad 2)	Ordenar por ataque de mayor a menor
3 (o NumPad 3)	Ordenar por velocidad de menor a mayor
L	Cambiar el tipo de lista
B	Ejecutar el benchmark
Esc	Salir del programa
4. Navegar

La navegación es circular: después del último personaje aparece la casilla ★ AÑADIR NUEVO PERSONAJE y, tras ella, se regresa al primero. Lo mismo ocurre hacia atrás.

5. Añadir un personaje
Navega hasta la casilla ★ AÑADIR NUEVO PERSONAJE.
Presiona Enter. Se abre el formulario ── NUEVO PERSONAJE ──.
Responde, en orden:
Name: texto obligatorio.
Species: elige una opción numerada (ver abajo).
Daño de ataque: entero mayor o igual a 0.
Velocidad de ataque: número mayor que 0 (acepta 1.5 o 1,5).
Level: entero mayor o igual a 1.
El personaje se agrega al final, el cursor se posiciona sobre él y aparece ✔ '<nombre>' añadido..

Menú de especies:

Species:
  1. Humano
  2. Mago
  3. Caballero
  4. Arquero
  5. Elfo
  6. Enano
  7. Orco
  8. Otra (escribirla)

Al elegir 8 se pide escribir el nombre de una especie personalizada.

6. Editar un personaje

Presiona E sobre el personaje deseado. El formulario muestra ── EDITAR PERSONAJE (Enter = mantener valor) ── y, en cada campo, el valor actual entre corchetes. Si presionas solo Enter, se conserva ese valor. Al terminar aparece ✔ Personaje editado..

En la casilla de «Añadir» la tecla E solo muestra No hay nada que editar aquí..

7. Eliminar un personaje

Presiona X o Supr. Aparece ¿Eliminar a '<nombre>'? (S/N):. Solo la tecla S confirma; cualquier otra tecla cancela. En la casilla de «Añadir» se muestra No puedes eliminar la casilla de añadir..

8. Ordenar

Las teclas 1, 2 y 3 ordenan la lista completa. Después de ordenar, la selección sigue al mismo personaje que se estaba viendo, aunque cambie de posición. Se muestra un mensaje como ✔ Ordenado por level (mayor → menor)..

9. Cambiar de lista

Presiona L, elige 1, 2 o 3 (o solo Enter para mantener la actual). Si se elige otro tipo, el programa crea la nueva lista copiando a todos los personajes en el mismo orden y muestra ✔ Ahora se usa: <nombre>..

10. Benchmark

Presiona B. Se solicitan dos datos (ambos con valor por defecto entre corchetes, que se acepta con Enter):

Parámetro	Rango	Valor por defecto
Cantidad de personajes	1,000 – 500,000	100,000
Repeticiones por lista	1 – 10	3

Mientras corre, se imprimen mensajes de progreso (calentamiento…, pasada 2/3…). Al terminar se muestra la tabla de resultados y se espera una tecla para volver. Más detalles en la sección Benchmark.

🗂️ Estructura del proyecto
SelectorPersonajesConsola/
├── Program.cs                         # Punto de entrada, bucle principal, pantallas y lectura de datos
├── Personaje.cs                       # Modelo de datos y personajes predeterminados
├── Listas.cs                          # Interfaz, criterios, fábrica y las 3 listas
├── Benchmark.cs                       # Motor del benchmark
├── SelectorPersonajesConsola.csproj   # Configuración del proyecto (.NET 10)
├── bin/Debug/net10.0/                 # Compilación (exe, dll, pdb, configs)
└── obj/                               # Archivos intermedios de compilación
🏗️ Arquitectura y diseño
Archivos y responsabilidades
Program.cs

Usa top-level statements (sin clase Program explícita). Contiene:

Bucle principal: dibuja la pantalla, lee una tecla con Console.ReadKey(true) (sin mostrarla) y ejecuta la acción correspondiente con un switch.
Estado: la lista activa (lista), el índice actual (indice), un mensaje de estado (mensaje) y la bandera de salida (salir). Cuando indice == lista.Count, el cursor está en la casilla «★ Añadir nuevo».
Funciones locales estáticas:
Función	Responsabilidad
ElegirTipo	Pantalla de selección de lista (opcionalmente con «Enter = mantener»)
Dibujar	Pinta toda la pantalla principal
Resumen	Texto corto Nombre (Especie) para las vistas previas
Ordenar	Ordena y mantiene la selección sobre el mismo personaje
CorrerBenchmark	Pide parámetros, ejecuta el benchmark e imprime la tabla
ImprimirFila	Imprime una fila de tiempos y marca con ★ el mínimo
PedirPersonaje	Formulario de alta/edición
LeerTexto, LeerEspecie, LeerEntero, LeerDecimal	Lectura y validación de entradas
Personaje.cs

Define la clase Personaje (ver Modelo de datos).

Listas.cs

Contiene TipoLista, la interfaz IListaPersonajes, los comparadores de Criterios, la fábrica ListaFactory y las tres implementaciones.

Benchmark.cs

Define ResultadoBenchmark y la clase estática Benchmark con el motor de mediciones.

Interfaz IListaPersonajes
csharp
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
Miembro	Función
Tipo / Nombre	Tipo de lista y nombre legible que se muestra en pantalla
Count	Número de personajes
this[int]	Acceso por índice (lanza ArgumentOutOfRangeException si está fuera de rango)
Agregar	Agrega al final
EliminarEn	Elimina el elemento en la posición indicada
Limpiar	Vacía la lista
IndiceDe	Posición de un personaje (comparación por referencia) o -1
Recorrer	Recorre la lista en orden
Ordenar	Ordena con un comparador
Patrones aplicados
Programación contra una interfaz: el programa y el benchmark no conocen la implementación concreta.
Fábrica (ListaFactory.Crear): instancia la lista según TipoLista.
Conversión sin pérdida (ListaFactory.Convertir): crea una lista nueva y copia los elementos en orden; los objetos Personaje se comparten por referencia.
🔗 Las tres implementaciones de lista
1. Lista personalizada (ListaPersonalizada)

Lista doblemente enlazada circular implementada desde cero con la clase Nodo (Datos, Siguiente, Anterior).

Un nodo nuevo se apunta a sí mismo en Siguiente y Anterior.
Mantiene la cabeza (_cabeza), el contador (_count) y un cursor (_cursor + _indiceCursor) con el último nodo visitado.
Acceso por índice optimizado (NodoEn): calcula la distancia hacia adelante y hacia atrás desde el cursor y recorre el camino más corto, aprovechando la circularidad. Navegar al personaje siguiente o anterior es casi inmediato; el acceso aleatorio sigue siendo O(n).
Agregar: inserta al final, enlazando con _cabeza.Anterior y _cabeza, en O(1).
Eliminar: reenlaza los vecinos, mueve la cabeza si se borró y reinicia el cursor en la cabeza.
Ordenar: copia los datos a un arreglo, aplica Array.Sort y reescribe Datos de cada nodo en orden (no se recrean nodos).
2. LinkedList<T> de .NET (ListaLinkedList)

Envuelve System.Collections.Generic.LinkedList<Personaje>. Como no es circular, se aplica la misma técnica del cursor y, al llegar a un extremo, salta con Next ?? First o Previous ?? Last. Eliminar reinicia el cursor. Ordenar también pasa por un arreglo.

3. List<T> de .NET (ListaNet)

Envuelve System.Collections.Generic.List<Personaje> (arreglo dinámico): acceso por índice en O(1) y ordenamiento con List.Sort. Eliminar en la posición 0 es O(n) porque desplaza los elementos.

Comparativa teórica
Operación	Personalizada	LinkedList<T>	List<T>
Agregar al final	O(1)	O(1)	O(1) amortizado
Acceso por índice	O(n), optimizado con cursor	O(n), optimizado con cursor	O(1)
Recorrer todo	O(n)	O(n)	O(n)
Eliminar al inicio	O(1) tras ubicar el nodo	O(1) tras ubicar el nodo	O(n)
Ordenar	O(n log n)	O(n log n)	O(n log n)

El benchmark integrado permite comprobar estos comportamientos en la práctica.

🔀 Criterios de ordenamiento

Definidos en la clase estática Criterios:

Tecla	Criterio	Orden
1	NivelDesc	Nivel, de mayor a menor
2	DanoDesc	Ataque, de mayor a menor
3	VelocidadAsc	Velocidad de ataque, de menor a mayor

Todos aplican un desempate por nombre (string.CompareOrdinal). Así, ante valores iguales, las tres listas generan exactamente el mismo orden, condición necesaria para que el benchmark pueda verificar que los resultados coinciden.

📊 Benchmark

Toma como base los personajes actuales y genera miles de personajes sintéticos para medir cada estructura.

Generación de datos

Los personajes se derivan de los actuales repitiéndolos de forma cíclica (si la lista estuviera vacía, se usan los predeterminados). Para el personaje número i, con ciclo = i / cantidadBase:

Nombre: "<nombre base> #<i+1>"
Especie: la del personaje base
Ataque: ataque base + (ciclo % 50) × 2
Velocidad: velocidad base + (ciclo % 10) × 0.05 (redondeada a 2 decimales)
Nivel: nivel base + ciclo % 50
Operaciones medidas

Para cada tipo de lista se ejecutan 10 operaciones, cronometradas con Stopwatch:

#	Operación	Descripción
1	Insertar personajes	Agrega todos los personajes generados
2	Daño total y nivel promedio	Recorre la lista sumando daño y nivel
3	DPS total (daño × velocidad)	Suma el DPS de cada personaje
4	Personaje con mayor DPS	Busca el máximo recorriendo la lista
5	Conteo por especie	Cuenta personajes por especie con un diccionario
6	Acceso por índice ×N	N = min(500, cantidad) accesos dispersos, con un salto basado en la proporción áurea (0.618) para recorrer toda la lista
7	Ordenar por nivel ↓	Ordena con NivelDesc
8	Ordenar por daño ↓	Ordena con DanoDesc
9	Ordenar por velocidad ↑	Ordena con VelocidadAsc
10	Eliminar los primeros K	K = max(1, min(1000, cantidad / 10)) eliminaciones en el índice 0
Metodología
Calentamiento (JIT): la pasada 0 no se promedia, para que la compilación JIT no distorsione las mediciones.
Recolección de basura: antes de cada pasada se ejecuta GC.Collect() y GC.WaitForPendingFinalizers().
Promedio: se reportan los milisegundos promedio de las repeticiones.
Medición limpia: solo se cronometra la acción; el texto de la «estadística calculada» se genera fuera del cronómetro.
Progreso: se imprimen mensajes en consola durante la ejecución.
Resultados en pantalla

La salida incluye:

Un encabezado con la cantidad, el número de personajes base, las repeticiones y la leyenda ★ = más rápido.
Una tabla con columnas Operación, Personalizada, LinkedList<T> y List<T> (tiempos en ms con 3 decimales). El valor más rápido de cada fila se marca con ★.
Una fila final TOTAL con la suma de los tiempos de cada lista.
Una sección «Estadísticas calculadas» con el resultado de cada operación (totales, promedios, mejor DPS, conteo por especie, etc.).
Un mensaje de verificación:
«Las tres listas produjeron exactamente los mismos resultados.»
«Los resultados difieren entre listas (revisar).»
🧬 Modelo de datos

Clase Personaje:

Propiedad	Tipo	Descripción
Name	string	Nombre del personaje
Species	string	Especie
Attack	int	Daño de ataque
AttackSpeed	double	Velocidad de ataque
Level	int	Nivel
Dps	double (calculada)	Attack × AttackSpeed

Otros miembros:

EspeciesBase: arreglo con las especies del menú (Humano, Mago, Caballero, Arquero, Elfo, Enano, Orco).
CopiarDe(Personaje otro): copia todos los campos desde otro personaje (se usa al editar).
Predeterminados(): devuelve la lista inicial.
Personajes iniciales
Nombre	Especie	Ataque	Velocidad	Nivel
Arthur	Caballero	45	1.2	12
Merlín	Mago	70	0.8	20
Legolas	Arquero	38	2.5	15
Thorin	Enano	55	1.0	18
Aldric	Humano	25	1.5	5
✅ Validaciones de entrada
Campo	Regla	Mensaje de error
Tipo de lista	Debe ser 1, 2 o 3	La pantalla se vuelve a mostrar
Nombre	No puede estar vacío (en alta). Se recortan espacios	Valor requerido.
Especie	Número entre 1 y 8; con 8 se escribe una propia	Opción no válida.
Daño de ataque	Entero ≥ 0	Introduce un entero entre 0 y 2,147,483,647.
Velocidad de ataque	Número > 0; acepta coma o punto decimal	Introduce un número mayor que 0 (ej. 1.5).
Nivel	Entero ≥ 1	Introduce un entero entre 1 y 2,147,483,647.
Cantidad (benchmark)	Entero entre 1,000 y 500,000	Introduce un entero entre 1,000 y 500,000.
Repeticiones (benchmark)	Entero entre 1 y 10	Introduce un entero entre 1 y 10.

Los enteros aceptan separador de miles según la cultura del sistema. Al editar, presionar solo Enter en cualquier campo conserva el valor actual.

⚠️ Limitaciones conocidas
Sin persistencia: los datos viven únicamente en memoria; al cerrar el programa se pierden los cambios y se restauran los personajes iniciales.
Sin imágenes: a diferencia de la versión de Windows Forms, esta versión solo muestra datos en texto.
Sin opción de cancelar el formulario de alta o edición una vez iniciado: hay que completar los campos (al editar, basta con Enter para conservar los valores).
Terminal interactiva obligatoria: no admite entrada redirigida.
Ancho de ventana: la tabla del benchmark mide 82 columnas; en terminales más estrechas se desordena.
Mezcla de idiomas: algunas etiquetas combinan español e inglés (por ejemplo Name, Species, Level y Ordenar level↓).
Diferencias con la versión gráfica: los personajes iniciales no son exactamente los mismos (aquí aparecen Legolas y Aldric; en la versión de Windows Forms, Azriel y Arlene).
🛠️ Tecnologías
Lenguaje: C# (con nullable reference types e implicit usings habilitados)
Plataforma: .NET 10 (net10.0), aplicación de consola (OutputType: Exe)
Conceptos de programación: listas doblemente enlazadas circulares, interfaces, patrón fábrica, delegados (Comparison<T>), iteradores (yield return), top-level statements, tuplas con nombre y medición de rendimiento con Stopwatch

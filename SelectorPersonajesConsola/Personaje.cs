namespace SelectorPersonajes;

public class Personaje
{
    public static readonly string[] EspeciesBase =
        ["Humano", "Mago", "Caballero", "Arquero", "Elfo", "Enano", "Orco"];

    public string Name { get; set; } = "";
    public string Species { get; set; } = "";
    public int Attack { get; set; }
    public double AttackSpeed { get; set; }
    public int Level { get; set; }

   
    public double Dps => Attack * AttackSpeed;

    public Personaje() { }

    public Personaje(string name, string species, int attack, double attackSpeed, int level)
    {
        Name = name;
        Species = species;
        Attack = attack;
        AttackSpeed = attackSpeed;
        Level = level;
    }

    public void CopiarDe(Personaje otro)
    {
        Name = otro.Name;
        Species = otro.Species;
        Attack = otro.Attack;
        AttackSpeed = otro.AttackSpeed;
        Level = otro.Level;
        
    }

    public static List<Personaje> Predeterminados() =>
    [
        new("Arthur", "Caballero", 45, 1.2, 12),
        new("Merlín", "Mago", 70, 0.8, 20),
        new("Legolas", "Arquero", 38, 2.5, 15),
        new("Thorin", "Enano", 55, 1.0, 18),
        new("Aldric", "Humano", 25, 1.5, 5),
    ];
}

using Newtonsoft.Json.Linq;
namespace Gra.Klasy;

public class Monster
{
  public string? Name;
  public int Strength;
  public int Speed;
  public int HP;
  private int aktywnaPozycjaMenu = 0;
  private string[] pozycjeMenu = { "Atak", "Znak", "Eliksir", "Ucieczka" };

  public Monster(string monster)
  {
    string data = File.ReadAllText($"./Monsters/{monster}.json");
    JObject load = JObject.Parse(data);
    Init((int)load["Strength"], (int)load["Speed"], (int)load["HP"], (string)load["Name"]);
  }
  private void Init(int strength, int speed, int hp, string name)
  {
    this.Name = name;
    this.Strength = strength;
    this.Speed = speed;
    this.HP = hp;
  }

  public bool Walka()
  {
    Console.CursorVisible = false;
    bool wyjscie = true;

    while (wyjscie)
    {
      PokazMenu();
      WybieranieOpcji();
      wyjscie = UruchomOpcje();
    }
    return wyjscie;
  }
  private void PokazMenu()
  {
    Console.BackgroundColor = ConsoleColor.Black;
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"Napotykasz potwora: {Name}\n Wybierz akcję:");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
    for (int i = 0; i < pozycjeMenu.Length; i++)
    {
      if (i == aktywnaPozycjaMenu)
      {
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("\t⚔️  {0,-10} ", pozycjeMenu[i], pozycjeMenu[i].Length);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
      }
      else
      {
        Console.WriteLine("\t" + pozycjeMenu[i]);
      }
    }
  }
  private void WybieranieOpcji()
  {
    do
    {
      ConsoleKeyInfo klawisz = Console.ReadKey();
      if (klawisz.Key == ConsoleKey.UpArrow)
      {
        aktywnaPozycjaMenu = (aktywnaPozycjaMenu > 0) ? aktywnaPozycjaMenu - 1 : pozycjeMenu.Length - 1;
        PokazMenu();
      }
      else if (klawisz.Key == ConsoleKey.DownArrow)
      {
        aktywnaPozycjaMenu = (aktywnaPozycjaMenu + 1) % pozycjeMenu.Length;
        PokazMenu();
      }
      else if (klawisz.Key == ConsoleKey.Enter)
      {
        break;
      }
    } while (true);
  }
  private bool UruchomOpcje()
  {
    switch (aktywnaPozycjaMenu)
    {
      case 0: Console.Clear(); Console.WriteLine("Atak"); Console.ReadKey(); break;
      case 1: Console.Clear(); Console.WriteLine("Znak"); Console.ReadKey(); break;
      case 2: Console.Clear(); Console.WriteLine("Eliksir"); Console.ReadKey(); break;
      case 3: Console.Clear(); Console.WriteLine("Ucieczka"); Console.ReadKey(); return false; break;
    }
    return true;
  }
}


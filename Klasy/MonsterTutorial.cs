using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Linq;
namespace Gra.Klasy;

public class MonsterTutorial
{
  public string? Name;
  public int Strength;
  public int Speed;
  public int HP;
  private int aktywnaPozycjaMenu = 0;
  private int turyZnak = 0;
  private int turyEliksir = 1;
  private int tury = 0;
  private string[] pozycjeMenu = { "Atak (urzycie spowoduje zaatakowanie wroga)", "Znak (urzycie aktualnie posiadanego zanku)", "Eliksir (urzycie aktualnie używanego Eliksiru)", "Ucieczka (Próba wyjścia z Walki niedostępna w samouczku)" };

  public MonsterTutorial()
  {
    string data = File.ReadAllText($"./Monsters/ghul.json");
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

  public void Walka()
  {
    Console.CursorVisible = false;

    while (HP > 0)
    {
      PokazMenu();
      WybieranieOpcji();
      UruchomOpcje();
    }
  }
  private void PokazMenu()
  {
    Console.BackgroundColor = ConsoleColor.Black;
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"V 100/100");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"\t\t{Name} {HP}/{HP}HP");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"\t\tTury:{tury}");
    Console.WriteLine("\n\n\tWybierz akcję której chcesz użyć:");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
    for (int i = 0; i < pozycjeMenu.Length; i++)
    {
      if (i == aktywnaPozycjaMenu)
      {
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("\t➤  {0,-10} ", pozycjeMenu[i], pozycjeMenu[i].Length);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
      }
      else if (i == 3)
      {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\t" + pozycjeMenu[i]);
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
      case 0: Atak(); break;
      case 1: Znak(); turyZnak++; ; break;
      case 2: Eliksir(); break;
      case 3: break;
    }
    return true;
  }

  private void Atak()
  {
    int aktywnaPozycjaAtaku = 0;
    string[] atakWybor = { "Szybki Atak (Masz większą szansę na trafienie wroga jako pierwszy, ale zadajesz mu mniej obrażeń)", "Wolny Atak (Zmniejsza się szansa na trafienie szybciej niż przeciwnik, ale zadajesz więcej obrażeń)", "Powrót (powrót do menu akcji)" };

    Console.CursorVisible = false;
    bool wyjscie = true;

    while (wyjscie)
    {
      PokazMenuAtaku(aktywnaPozycjaAtaku, atakWybor);
      aktywnaPozycjaAtaku = WybieranieOpcjiAtaku(aktywnaPozycjaAtaku, atakWybor);
      wyjscie = UruchomOpcjeAtaku(aktywnaPozycjaAtaku);
    }
  }
  private void PokazMenuAtaku(int aktywnaPozycjaAtaku, string[] atakWybor)
  {
    Console.BackgroundColor = ConsoleColor.Black;
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"V 100/100");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"\t\t{Name} {HP}/{HP}HP");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"\t\tTury:{tury}");
    Console.WriteLine("\n\n\tWybierz akcję której chcesz użyć:");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
    for (int i = 0; i < atakWybor.Length; i++)
    {
      if (i == aktywnaPozycjaAtaku)
      {
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("\t➤  {0,-10} ", atakWybor[i]);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
      }
      else
      {
        Console.WriteLine("\t" + atakWybor[i]);
      }
    }
  }
  private int WybieranieOpcjiAtaku(int aktywnaPozycjaAtaku, string[] atakWybor)
  {
    do
    {
      ConsoleKeyInfo klawisz = Console.ReadKey();
      if (klawisz.Key == ConsoleKey.UpArrow)
      {
        aktywnaPozycjaAtaku = (aktywnaPozycjaAtaku > 0) ? aktywnaPozycjaAtaku - 1 : atakWybor.Length - 1;
        PokazMenuAtaku(aktywnaPozycjaAtaku, atakWybor);
      }
      else if (klawisz.Key == ConsoleKey.DownArrow)
      {
        aktywnaPozycjaAtaku = (aktywnaPozycjaAtaku + 1) % atakWybor.Length;
        PokazMenuAtaku(aktywnaPozycjaAtaku, atakWybor);
      }
      else if (klawisz.Key == ConsoleKey.Enter)
      {
        break;
      }
    } while (true);
    return aktywnaPozycjaAtaku;
  }
  private bool UruchomOpcjeAtaku(int aktywnaPozycjaAtaku)
  {
    switch (aktywnaPozycjaAtaku)
    {
      case 0:
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        int atakS = 6;
        Console.WriteLine($"Atak Szybki: {atakS}");
        HP -= atakS;
        Console.ForegroundColor = ConsoleColor.White;
        tury++;
        Console.ReadKey();
        break;
      case 1:
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        int atakW = 12;
        Console.WriteLine($"Atak Wolny: {atakW}");
        HP -= atakW;
        Console.ForegroundColor = ConsoleColor.White;
        tury++;
        Console.ReadKey();
        break;
      case 2: return false;
    }
    return false;
  }
  private void Znak()
  {
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Blue;
    string text;
    if (turyZnak % 3 == 0)
    {
      text = "Używasz: Yrden\nPrzeciwnik nie może się ruszyć (do następnej tury)";
      tury++;
    }
    else
    {
      text = $"Następne użycie dopiero za {3 - (turyZnak % 3)} tury";
    }
    Console.WriteLine(text);
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadKey();
  }
  private void Eliksir()
  {
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Blue;
    if (turyEliksir == 1)
    {
      Console.WriteLine("Używasz Eliksiru: Jaskółka (Odnawia 5% życia co rundę)");
      turyEliksir--;
      tury++;
    }
    else
    {
      Console.WriteLine("Użyłeś już Eliksiru w tej walce");
    }
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadKey();
  }
}
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Linq;
using Gra.Klasy;
using System.Runtime.CompilerServices;
namespace Gra.Klasy;

public class MonsterTutorial
{
  public string? Name;
  public int Strength;
  public int Speed;
  public int HP;
  public int CloneHP;
  private int speedMove;
  private int aktywnaPozycjaMenu = 0;
  private bool EliksirUse = false;
  private int tury = 0;
  private int turyZnak = 0;
  private int turyZnakUse = 0;
  private bool znakUse = false;
  private bool wyjscieGracz = true;
  private string[] pozycjeMenu = { "Atak (użycie spowoduje zaatakowanie wroga)", "Znak (użycie aktualnie posiadanego zanku)", "Eliksir (użycie aktualnie używanego Eliksiru)", "Ucieczka (Próba wyjścia z Walki niedostępna w samouczku)" };
  private Hero hero;

  public MonsterTutorial(Hero heroArgs)
  {
    hero = heroArgs;
    speedMove = hero.Speed;
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
    this.CloneHP = hero.HP;
  }
  public void Walka()
  {
    if (speedMove > Speed)
    {
      RuchGracz();
    }
    else
    {
      if (tury == 0)
      {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Przeciwnik jest od Ciebie szybszy i atakuje jako pierwszy");
        Console.ReadKey();
      }
      RuchPrzeciwnik();
    }
    while ((HP > 0) && (CloneHP > 0))
    {
      if (EliksirUse)
      {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Eliksir Jaskółka przywraca Ci 2pkt HP");
        Console.ReadKey();
        Console.ForegroundColor = ConsoleColor.White;
        CloneHP += 5;
      }
      if (turyZnakUse == 1)
      {
        znakUse = false;
      }
      RuchGracz();
    }
  }
  private void RuchGracz()
  {
    wyjscieGracz = true;
    while (wyjscieGracz)
    {
      turyZnak = turyZnak < 0 ? 3 : turyZnak;
      PokazMenu();
      WybieranieOpcji();
      UruchomOpcje();
    }
  }
  private void RuchPrzeciwnik()
  {
    if (HP > 0)
    {
      Console.Clear();
      Random rnd = new Random();
      int atak = rnd.Next() % (Strength + 1 - (Strength - 10));
      Console.ForegroundColor = ConsoleColor.Red;
      if (atak == 0)
      {
        Console.WriteLine("Potwór szarżuje w Twoją stronę. Robisz unik i udaje Ci się uciec przed jego atakiem.");
      }
      else if (atak > 0 && atak < 5)
      {
        Console.WriteLine($"Potwór szykuje się do skoku w Twoją stronę, w ostatniej chwili zauważasz to i próbujesz się uchylić. Potworowi udaje się zahaczyć Cię pazurami.\n\nOtrzymujesz {atak}pkt obrażeń");
      }
      else
      {
        Console.WriteLine($"Potwór rzuca się na Ciebie! Niestety jest szybszy i rani Cię.\n\nOtrzymujesz {atak}pkt obrażeń");
      }
      CloneHP = CloneHP - atak;

      Console.ReadKey();
    }
  }
  private void PokazMenu()
  {
    Console.BackgroundColor = ConsoleColor.Black;
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"{hero.Name} {CloneHP}/{hero.HP}HP");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"\t\t{Name} {HP}/60HP");
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
      case 1: Znak(); break;
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
    Console.Write($"{hero.Name} {CloneHP}/{hero.HP}HP");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"\t\t{Name} {HP}/60HP");
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
    Random rnd = new Random();
    int max;
    int min;
    switch (aktywnaPozycjaAtaku)
    {
      case 0:
        Console.Clear();

        min = hero.Strength - (int)Math.Floor(hero.Strength * 0.70);
        max = (int)Math.Floor(hero.Strength * 0.66);
        int atakS = min + (rnd.Next() % (max + 1 - min));
        speedMove = (int)Math.Round(hero.Speed * 1.20, 2);
        if (speedMove > Speed)
        {
          Console.ForegroundColor = ConsoleColor.Blue;
          Console.WriteLine($"Zadajesz przeciwnikowi {atakS}pkt obrazeń");
          HP -= atakS;
          Console.ReadKey();
          if (!znakUse)
          {
            RuchPrzeciwnik();
          }
          else
          {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Przeciwnik nie może się ruszyć!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
          }
        }
        else
        {
          if (!znakUse)
          {
            RuchPrzeciwnik();
          }
          else
          {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Przeciwnik nie może się ruszyć!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
          }
          Console.Clear();
          Console.ForegroundColor = ConsoleColor.Blue;
          Console.WriteLine($"Zadajesz przeciwnikowi {atakS}pkt obrazeń");
          HP -= atakS;
          Console.ReadKey();
        }
        Console.ForegroundColor = ConsoleColor.White;
        tury++;
        if (znakUse)
        {
          turyZnakUse++;
        }
        else
        {
          turyZnak++;
        }
        wyjscieGracz = false;
        break;
      case 1:
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        min = hero.Strength - (int)Math.Floor(hero.Strength * 0.30);
        max = (int)Math.Floor(hero.Strength * 1.33);
        int atakW = min + (rnd.Next() % (max + 1 - min));
        speedMove = (int)Math.Round(hero.Speed * 0.66, 2);

        if (speedMove > Speed)
        {
          Console.ForegroundColor = ConsoleColor.Blue;
          Console.WriteLine($"Zadajesz przeciwnikowi {atakW}pkt obrazeń");
          HP -= atakW;
          Console.ReadKey();
          if (!znakUse)
          {
            RuchPrzeciwnik();
          }
          else
          {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Przeciwnik nie może się ruszyć!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
          }
        }
        else
        {
          if (!znakUse)
          {
            RuchPrzeciwnik();
          }
          else
          {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Przeciwnik nie może się ruszyć!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
          }
          Console.Clear();
          Console.ForegroundColor = ConsoleColor.Blue;
          Console.WriteLine($"Zadajesz przeciwnikowi {atakW}pkt obrazeń");
          HP -= atakW;
          Console.ReadKey();
        }
        Console.ForegroundColor = ConsoleColor.White;
        tury++;
        if (znakUse)
        {
          turyZnakUse++;
        }
        else
        {
          turyZnak++;
        }
        wyjscieGracz = false;
        break;
      case 2: return false;
    }
    return false;
  }
  private void Znak()
  {
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Blue;
    if (znakUse)
    {
      Console.WriteLine("Zaklęcie jeszcze trwa");
    }
    else
    {
      if (turyZnak >= 3)
      {
        Console.Write("Używasz: ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("Yrden");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\nPrzeciwnik nie może się ruszyć(przez 1 tury)");
        tury++;
        wyjscieGracz = false;
        turyZnak = 0;
        turyZnakUse = 0;
        znakUse = true;
      }
      else
      {
        Console.WriteLine($"Użycie znaku dostępne dopiero za {3 - turyZnak} tury");
      }
    }
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadKey();
  }
  private void Eliksir()
  {
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Blue;
    if (!EliksirUse)
    {
      Console.WriteLine("Używasz Eliksiru: Jaskółka (Odnawia 5HP co rundę)");
      tury++;
      EliksirUse = true;
      wyjscieGracz = false;
      if (znakUse)
      {
        turyZnakUse++;
      }
      else
      {
        turyZnak++;
      }
    }
    else
    {
      Console.WriteLine("Użyłeś już Eliksiru w tej walce");
    }
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadKey();
  }
}
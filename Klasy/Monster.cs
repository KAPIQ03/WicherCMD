using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
namespace Gra.Klasy;

public class Monster
{
  private string? Name;
  private int Strength;
  private int Speed;
  private int HP;
  private int CloneHPMonster;
  private int CloneHPGracz;
  private int CloneStrGracz;
  private int CloneSpeedGracz;
  private int speedMove;
  private int aktywnaPozycjaMenu = 0;
  private bool EliksirUse = false;
  private int tury = 0;
  private int turyZnak = 0;
  private int turyZnakUse = 0;
  private bool znakUse = false;
  private bool ucieczka = false;
  private bool wyjscieGracz = true;
  private string[] pozycjeMenu = { "Atak", "Znak", "Eliksir", "Ucieczka" };
  private Hero hero;
  public Monster(string monster, Hero heroArgs)
  {
    hero = heroArgs;
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
    this.CloneHPGracz = hero.HP;
    this.CloneHPMonster = hp;
    this.CloneStrGracz = hero.Strength;
    this.CloneSpeedGracz = hero.Speed;
    this.speedMove = CloneSpeedGracz;
  }
  public bool Walka()
  {
    Console.Clear();
    Console.WriteLine($"Twoim przeciwnikem jest: {Name}");
    Console.ReadKey();
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
    Console.CursorVisible = false;
    while ((HP > 0) && (CloneHPGracz > 0) && (!ucieczka))
    {
      if ((EliksirUse && hero.SelectedPotion == 0) || (EliksirUse && hero.SelectedPotion == 3))
      {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        if (hero.SelectedPotion == 0)
        {
          Console.WriteLine("Eliksir Jaskółka przywraca Ci 5pkt HP");
          CloneHPGracz += 5;
        }
        else if (hero.SelectedPotion == 3)
        {
          Console.WriteLine("Eliksir Jaskółka przywraca Ci 10pkt HP");
          CloneHPGracz += 10;
        }
        Console.ReadKey();
        Console.ForegroundColor = ConsoleColor.White;
      }
      if (turyZnakUse == 1 && hero.SelectedSign == 0)
      {
        znakUse = false;
      }
      else if (turyZnakUse == 2 && hero.SelectedSign == 1)
      {
        znakUse = false;
      }
      else if (hero.SelectedSign == 2 || hero.SelectedSign == 3 || hero.SelectedSign == 4)
      {
        znakUse = false;
      }
      RuchGracz();
    }
    if (HP <= 0)
    {
      Console.Clear();
      Console.WriteLine("Pokonałeś potwora");
      hero.XP += 100;
      Console.ReadKey();
      if (hero.XP >= 100)
      {
        hero.levelUP();
      }
      hero.MonsterKill++;
      return false;
    }
    if (CloneHPGracz <= 0)
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine("\tGiniesz! Twoje truchło zjadają padlinożerne trupojady.");
      Console.ReadKey();
      Console.Clear();
      Console.WriteLine("\tKONIEC GRY");
      Console.ReadKey();
      Console.ForegroundColor = ConsoleColor.White;
      hero.isAlive = false;
      return true;
    }
    return false;
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
      int atak = rnd.Next() % 11;
      Console.ForegroundColor = ConsoleColor.Red;
      if (atak == 0)
      {
        Console.WriteLine("Potwór szarżuje w Twoją stronę. Robisz unik i udaje Ci się uciec przed jego atakiem.");
      }
      else if (atak > 0 && atak < 7)
      {
        Console.WriteLine($"Potwór szykuje się do skoku w Twoją stronę, w ostatniej chwili zauważasz to i próbujesz się uchylić. Potworowi udaje się zahaczyć Cię pazurami.\n\nOtrzymujesz {(int)(Strength * 0.7)}pkt obrażeń");
        CloneHPGracz -= (int)(Strength * 0.7);
      }
      else
      {
        Console.WriteLine($"Potwór rzuca się na Ciebie! Niestety jest szybszy i rani Cię.\n\nOtrzymujesz {Strength}pkt obrażeń");
        CloneHPGracz -= Strength;
      }
      Console.ReadKey();
    }
  }
  private void PokazMenu()
  {
    Console.BackgroundColor = ConsoleColor.Black;
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"{hero.Name} {CloneHPGracz}/{hero.HP}HP");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"\t\t{Name} {HP}/{CloneHPMonster}HP");
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
      case 3: Ucieczka(); break;
    }
    return true;
  }
  private void Atak()
  {
    int aktywnaPozycjaAtaku = 0;
    string[] atakWybor = { "Szybki Atak", "Wolny Atak", "Powrót" };

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
    Console.Write($"{hero.Name} {CloneHPGracz}/{hero.HP}HP");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"\t\t{Name} {HP}/{CloneHPMonster}HP");
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
    int atak = 0;
    switch (aktywnaPozycjaAtaku)
    {
      case 0://ataka szybki
        Console.Clear();
        min = CloneStrGracz - (int)Math.Floor(CloneStrGracz * 0.70);
        max = (int)Math.Floor(CloneStrGracz * 0.66);
        int atakS = min + (rnd.Next() % (max + 1 - min));
        speedMove = (int)Math.Round(CloneSpeedGracz * 1.20, 2);
        atak = atakS;
        break;
      case 1://atak wolny
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        min = CloneStrGracz - (int)Math.Floor(CloneStrGracz * 0.30);
        max = (int)Math.Floor(CloneStrGracz * 1.33);
        int atakW = min + (rnd.Next() % (max + 1 - min));
        speedMove = (int)Math.Round(CloneSpeedGracz * 0.66, 2);
        atak = atakW;
        break;
      case 2:
        return false;
    }
    if (speedMove > Speed)
    {
      Console.ForegroundColor = ConsoleColor.Blue;
      Console.WriteLine($"Zadajesz przeciwnikowi {atak}pkt obrazeń");
      HP -= atak;
      Console.ReadKey();
      if ((znakUse && hero.SelectedSign == 0) || (znakUse && hero.SelectedSign == 1))
      {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Przeciwnik nie może się ruszyć!");
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey();
      }
      else
      {
        RuchPrzeciwnik();
      }
    }
    else
    {
      if ((znakUse && hero.SelectedSign == 0) || (znakUse && hero.SelectedSign == 1))
      {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Przeciwnik nie może się ruszyć!");
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey();
      }
      else
      {
        RuchPrzeciwnik();
      }
      if (CloneHPGracz > 0)
      {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Zadajesz przeciwnikowi {atak}pkt obrazeń");
        HP -= atak;
        Console.ReadKey();
      }
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
      switch (hero.SelectedSign)
      {
        case 0: //Yrden
          if (turyZnak >= 3)
          {
            Console.Write("\tUżywasz: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Yrden");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\tPrzeciwnik nie może się ruszyć (przez 1 tury)");
            wyjscieGracz = false;
            turyZnak = 0;
            turyZnakUse = 0;
            znakUse = true;
          }
          else
          {
            Console.WriteLine($"Użycie znaku dostępne dopiero za {3 - turyZnak} tury");
          }
          break;
        case 1: //Aksi
          if (turyZnak >= 3)
          {
            Console.Write("\tUżywasz: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Aksi");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\tPrzeciwnik nie może się ruszyć(przez 2 tury)");
            wyjscieGracz = false;
            turyZnak = 0;
            turyZnakUse = 0;
            znakUse = true;
          }
          else
          {
            Console.WriteLine($"Użycie znaku dostępne dopiero za {3 - turyZnak} tury");
          }
          break;
        case 2: //Quen
          if (turyZnak >= 3)
          {
            Console.Write("\tUżywasz: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Quen");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\tOtrzymujesz 50 pkt pancerza");
            wyjscieGracz = false;
            turyZnak = 0;
            turyZnakUse = 0;
            znakUse = true;
            CloneHPGracz += 50;
          }
          else
          {
            Console.WriteLine($"Użycie znaku dostępne dopiero za {3 - turyZnak} tury");
          }
          break;
        case 3: //Ard
          if (turyZnak >= 3)
          {
            Console.Write("\tUżywasz: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Ard");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\tZadajesz 30 pkt obrażeń");
            wyjscieGracz = false;
            turyZnak = 0;
            turyZnakUse = 0;
            znakUse = true;
            HP -= 30;
          }
          else
          {
            Console.WriteLine($"Użycie znaku dostępne dopiero za {3 - turyZnak} tury");
          }
          break;
        case 4: //Igni
          if (turyZnak >= 3)
          {
            Console.Write("\tUżywasz: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Igni");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\tZadajesz 50 pkt obrażeń");
            wyjscieGracz = false;
            turyZnak = 0;
            turyZnakUse = 0;
            znakUse = true;
            HP -= 50;
          }
          else
          {
            Console.WriteLine($"Użycie znaku dostępne dopiero za {3 - turyZnak} tury");
          }
          break;
      }
    }
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadKey();
  }
  private void Eliksir()
  {
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Blue;
    if (EliksirUse)
    {
      Console.WriteLine("Użyłeś już eliksiru w tej walce");
      Console.ReadKey();
    }
    else
    {
      switch (hero.SelectedPotion)
      {
        case 0: //Jaskółka
          Console.WriteLine("\tUżywasz Eliksiru: Jaskółka ");
          Console.WriteLine("\n\tEliksir odnawia 5HP co rundę");
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
          break;
        case 1: //Zamieć
          Console.WriteLine("\tUżywasz Eliksiru: Zamieć ");
          Console.WriteLine("\n\tEliksir zwiększa szybkość o 20%");
          tury++;
          EliksirUse = true;
          wyjscieGracz = false;
          CloneSpeedGracz += (int)Math.Floor(CloneSpeedGracz * 0.2);
          if (znakUse)
          {
            turyZnakUse++;
          }
          else
          {
            turyZnak++;
          }
          break;
        case 2: //Grom
          Console.WriteLine("\tUżywasz Eliksiru: Grom ");
          Console.WriteLine("\n\tEliksir zwiększa siłę zadawanych obrażeń o 10%");
          tury++;
          EliksirUse = true;
          wyjscieGracz = false;
          CloneStrGracz = hero.Strength + (int)Math.Floor(hero.Strength * 0.1);
          if (znakUse)
          {
            turyZnakUse++;
          }
          else
          {
            turyZnak++;
          }
          break;
        case 3: //Ulepszona Jaskółka
          Console.WriteLine("\tUżywasz Eliksiru: Ulepszona Jaskółka ");
          Console.WriteLine("\n\tEliksir odnawia 10HP co rundę");
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
          break;
        case 4: //Ulepszony Grom
          Console.WriteLine("\tUżywasz Eliksiru: Ulepszony Grom ");
          Console.WriteLine("\n\tEliksir zwiększa siłę zadawanych obrażeń o 20% ");
          tury++;
          EliksirUse = true;
          wyjscieGracz = false;
          CloneStrGracz = hero.Strength + (int)Math.Floor(hero.Strength * 0.2);
          if (znakUse)
          {
            turyZnakUse++;
          }
          else
          {
            turyZnak++;
          }
          break;
      }
      Console.ReadKey();
      RuchPrzeciwnik();
    }
    Console.ForegroundColor = ConsoleColor.White;
  }
  private void Ucieczka()
  {
    Console.Clear();
    Random rnd = new Random();
    int szansa = rnd.Next() % 5;
    if (szansa <= 2)
    {
      Console.WriteLine("Pomyślnie udało Ci się uciec przed potworem");
      wyjscieGracz = false;
      ucieczka = true;
    }
    else
    {
      Console.WriteLine("Niestety nie udaje Ci się uciec przed potworem");
      Console.ReadKey();
      RuchPrzeciwnik();
    }
    Console.ReadKey();
  }
}
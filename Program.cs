using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;
// using System.Text.Json;

namespace projekt
{
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
      Console.WriteLine($"Napotykasz potwora: {Name}");
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
  public class Hero
  {
    public string? Name;
    public int Strength;
    public int Speed;
    public int Magic;
    public int Alchemy;
    public int HP;
    public int XP;
    public int Gold;
    public int Level;
    public int Storage;

    public Hero(JObject load)
    {
      JObject Load = load;
      Init((int)Load["Strength"], (int)Load["Speed"], (int)Load["Magic"], (int)Load["Alchemy"], (int)Load["HP"], (int)Load["XP"], (int)Load["Gold"], (int)Load["Level"], (int)Load["Storage"], (string)Load["Name"]);
    }
    private void Init(int strength, int speed, int magic, int alchemy, int hp, int xp, int gold, int level, int storage, string name)
    {
      this.Name = name;
      this.Strength = strength;
      this.Speed = speed;
      this.Magic = magic;
      this.Alchemy = alchemy;
      this.HP = hp;
      this.XP = xp;
      this.Gold = gold;
      this.Level = level;
      this.Storage = storage;
    }

    // public void UpStrength() { this.Strength += 5; this.HP += 5; }
    // public void UpDexterity() { this.Dexterity += 5; }
    // public void UpIntelligence() { this.Intelligence += 5; this.MP += (3 * this.Intelligence); }
    public static void Save(JObject hero, Hero hero1)
    {
      JObject Hero = hero;
      Hero["Strength"] = hero1.Strength;
      Hero["Speed"] = hero1.Speed;
      Hero["Magic"] = hero1.Magic;
      Hero["Alchemy"] = hero1.Alchemy;
      Hero["HP"] = hero1.HP;
      Hero["XP"] = hero1.XP;
      Hero["Storage"] = hero1.Storage;
      Hero["Gold"] = hero1.Gold;
      Hero["Level"] = hero1.Level;
      Hero["Name"] = hero1.Name;

      File.WriteAllText("./hero.json", Hero.ToString());
    }
  }
  class Program
  {
    static void Main(string[] args)
    {
      string data = File.ReadAllText("./hero.json");
      JObject load = JObject.Parse(data);

      Hero hero = new Hero(load);

      //TODO: Wprowadzenie

      //Samouczek

      if (hero.Name == "")
      {
        NewGame(hero);
      }

      //TODO: Wybór akcji zapentlony

      //TODO: SYSTEM WALKI

      //Hero.Save(load, hero);
    }
    public static void NewGame(Hero hero)
    {
      Console.Clear();
      Console.WriteLine("Jak masz na imię podróżniku?");
      Console.Write("-> ");
      string nazwa = Console.ReadLine();
      hero.Name = (nazwa.Length > 1) ? nazwa : "V";
      int pktStart = 3;

      Console.Clear();
      Console.WriteLine($"Rozdziel punkty umiejętności: {pktStart}/3");
      Console.WriteLine($"1. Siła {hero.Strength}\n2. Szybkość {hero.Speed}\n3. Magia {hero.Magic}\n4. Alchemia {hero.Alchemy}\n");
      Console.Write("-> ");

      while (pktStart > 0)
      {
        string? option = Console.ReadLine();
        switch (option)
        {
          case "1": hero.Strength = hero.Strength + 1; pktStart--; break;
          case "2": hero.Speed = hero.Speed + 1; pktStart--; break;
          case "3": hero.Magic = hero.Magic + 1; pktStart--; break;
          case "4": hero.Alchemy = hero.Alchemy + 1; pktStart--; break;
          default: Console.WriteLine("\nUmiesz liczyć do 4?"); Console.ReadKey(); break;
        }
        Console.Clear();
        Console.WriteLine($"Rozdziel punkty umiejętności: {pktStart}/3");
        Console.WriteLine($"1. Siła {hero.Strength}\n2. Szybkość {hero.Speed}\n3. Magia {hero.Magic}\n4. Alchemia {hero.Alchemy}\n");
        if (pktStart > 0) Console.Write("-> ");
      }
      Console.WriteLine("Aby kontynuować wciśnij przycisk.");
      Console.ReadKey();

      Console.Clear();
      string tutorial = $"Wieśniak:\n\n\tWitaj {hero.Name},\n\tW naszej wiosce zalęgły się ghule, czy mógłbyś się ich pozbyć? (Samouczek)";
      bool wybor = Wybor(tutorial);

      if (wybor)
      {
        Console.Clear();
        Console.WriteLine("//Samouczek");

        Monster ghul = new Monster("ghul");

        ghul.Walka();

      }
      else
      {
        Console.WriteLine("//Wypierdalaj na szlak odmieńcu!");
      }
    }
    public static bool Wybor(string tutorial)
    {
      Console.CursorVisible = false;
      int aktywnaPozycjaMenu = 0;
      string[] pozycjeMenu = { "Tak", "Nie" };
      while (true)
      {
        PokazMenu(aktywnaPozycjaMenu, pozycjeMenu, tutorial);
        WybieranieOpcji(aktywnaPozycjaMenu, pozycjeMenu, tutorial);
        return UruchomOpcje(aktywnaPozycjaMenu);
      }
    }
    static void PokazMenu(int aktywnaPozycjaMenu, string[] pozycjeMenu, string tutorial)
    {
      Console.BackgroundColor = ConsoleColor.Black;
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine(tutorial);
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
    static void WybieranieOpcji(int aktywnaPozycjaMenu, string[] pozycjeMenu, string tutorial)
    {
      do
      {
        ConsoleKeyInfo klawisz = Console.ReadKey();
        if (klawisz.Key == ConsoleKey.UpArrow)
        {
          aktywnaPozycjaMenu = (aktywnaPozycjaMenu > 0) ? aktywnaPozycjaMenu - 1 : pozycjeMenu.Length - 1;
          PokazMenu(aktywnaPozycjaMenu, pozycjeMenu, tutorial);
        }
        else if (klawisz.Key == ConsoleKey.DownArrow)
        {
          aktywnaPozycjaMenu = (aktywnaPozycjaMenu + 1) % pozycjeMenu.Length;
          PokazMenu(aktywnaPozycjaMenu, pozycjeMenu, tutorial);
        }
        else if (klawisz.Key == ConsoleKey.Enter)
        {
          break;
        }
      } while (true);
    }
    static bool UruchomOpcje(int aktywnaPozycjaMenu)
    {
      switch (aktywnaPozycjaMenu)
      {
        case 0: return true;
        case 1: return false;
        default: return false;
      }
    }
  }
}

//Wzór JSON
// {
//   "Name": "",
//   "Speed": 2,
//   "Strength": 2,
//   "Magic": 2,
//   "Alchemy": 2,
//   "HP": 100,
//   "XP": 0,
//   "Storage": 10,
//   "Gold": 10,
//   "Level": 1
// }
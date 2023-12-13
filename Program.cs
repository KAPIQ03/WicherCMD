using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;
// using System.Text.Json;

namespace projekt
{
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

      if (hero.Name == "")
      {
        NewGame(hero);
      }

      //TODO: Wprowadzenie


      // Console.WriteLine($"Wieśniak: Witaj {hero.Name}, W naszej wiosce zalęgły się ghule, Czy mógłbyś je dla nas pokonać?");

      //Samouczek

      //TODO: Wybór akcji zapentlony

      //TODO: SYSTEM WALKI

      Hero.Save(load, hero);
    }
    public static void NewGame(Hero hero)
    {
      Console.Clear();
      Console.WriteLine("Jak masz na imię podróżniku?");
      Console.Write("-> ");
      string nazwa = Console.ReadLine();
      hero.Name = nazwa;
      int i = 3;

      Console.Clear();
      Console.WriteLine($"Rozdziel punkty umiejętności: {i}/3");
      Console.WriteLine($"1. Siła {hero.Strength}\n2. Szybkość {hero.Speed}\n3. Magia {hero.Magic}\n4. Alchemia {hero.Alchemy}\n");

      while (i > 0)
      {
        char option = char.Parse(Console.ReadLine());
        switch (option)
        {
          case '1': hero.Strength = hero.Strength + 1; i--; break;
          case '2': hero.Speed = hero.Speed + 1; i--; break;
          case '3': hero.Magic = hero.Magic + 1; i--; break;
          case '4': hero.Alchemy = hero.Alchemy + 1; i--; break;
          default: break;
        }
        Console.Clear();
        Console.WriteLine($"Rozdziel punkty umiejętności: {i}/3");
        Console.WriteLine($"1. Siła {hero.Strength}\n2. Szybkość {hero.Speed}\n3. Magia {hero.Magic}\n4. Alchemia {hero.Alchemy}\n");
        if (i > 0) Console.Write("-> ");
      }
      Console.WriteLine("Aby kontynuować wciśnij przycisk.");
      Console.ReadKey();

      Console.Clear();

      Console.SetCursorPosition(12, 4);
      string tutorial = $"Witaj {hero.Name},\nW naszej wiosce zalęgły się ghule, czy mógłbyś się ich pozbyć?";
      bool wybor = Wybor(tutorial);

      if (wybor)
      {
        Console.WriteLine("//Samouczek");
      }
      else
      {
        Console.WriteLine("//Bez samouczka");
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
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine(tutorial);
      Console.WriteLine();
      for (int i = 0; i < pozycjeMenu.Length; i++)
      {
        if (i == aktywnaPozycjaMenu)
        {
          Console.BackgroundColor = ConsoleColor.White;
          Console.ForegroundColor = ConsoleColor.Black;
          Console.WriteLine("⚔️  {0,-10} ", pozycjeMenu[i], pozycjeMenu[i].Length);
          Console.BackgroundColor = ConsoleColor.Black;
          Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
          Console.WriteLine(pozycjeMenu[i]);
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
        else if (klawisz.Key == ConsoleKey.Escape)
        {
          aktywnaPozycjaMenu = pozycjeMenu.Length - 1;
          break;
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
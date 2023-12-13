using System;
using System.IO;
using Gra.Klasy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace projekt
{
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

      //TODO: Wybór akcji zapentlony

      //TODO: SYSTEM WALKI

      //Hero.Save(load, hero);
    }
    public static void NewGame(Hero hero)
    {
      Console.Clear();
      Console.WriteLine("Jak masz na imię podróżniku?");
      Console.Write("-> ");
      string? nazwa = Console.ReadLine();
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

      MenuDialog menuDialog = new MenuDialog();
      bool wybor = menuDialog.Wybor(tutorial);

      if (wybor)
      {
        MonsterTutorial ghul = new MonsterTutorial();
        NPC("W takim razie chodź za mną... ", "Wieśniak");
        Narrator("Wieśniak prowadzi Cię do stodoły na skraju wsi.");
        Gracz("Zajmę się tym. Schowaj się w jakiejś chacie i ostrzeż ludzi.");
        Narrator("Wieśniak kiwa głową i odchodzi.\nOstrożnie obchodzisz stodołę, w hałdzie gnoju za nią widać niewielkie gniazdo, w którym siedzi jeden Ghul.");
        Narrator("Widzisz jak potwór podnosi łeb i wietrzy Twój zapach. Susem wyskakuje z gniazda i zaczyna się do Ciebie zbliżać.\nSięgasz po miecz...");
        ghul.Walka();
      }
      else
      {
        Console.WriteLine("//Wypierdalaj na szlak odmieńcu!");
      }

    }
    public static void NPC(string text, string nazwa)
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine($"{nazwa}:\n\t{text}");
      Console.ForegroundColor = ConsoleColor.White;
      Console.ReadKey();
    }
    public static void Narrator(string text)
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.DarkGray;
      Console.WriteLine($"\n{text}");
      Console.ForegroundColor = ConsoleColor.White;
      Console.ReadKey();
    }
    public static void Gracz(string text)
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"Ty:\n\t{text}");
      Console.ForegroundColor = ConsoleColor.White;
      Console.ReadKey();
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
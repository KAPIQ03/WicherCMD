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
      Console.CursorVisible = false;
      if (hero.Name != "")
      {
        LoadGame(hero);
        if (!hero.isAlive)
        {
          Narrator("Umierasz. Następna gra rozpocznie się od nowa");
          hero.Death();
          Hero.Save(load, hero);
        }
        else
        {
          Hero.Save(load, hero);
          Narrator("Pomyślnie zapisano stan gry");
        }
      }
      //TODO: SYSTEM WALKI
    }
    public static void NewGame(Hero hero)
    {
      Console.ForegroundColor = ConsoleColor.White;
      Console.Clear();
      Console.WriteLine("Jak Ciebie wołają odmieńcze?");
      Console.Write("-> ");
      string? nazwa = Console.ReadLine();
      hero.Name = (nazwa.Length >= 1) ? nazwa : "Wiedźmin";
      int pktStart = 3;

      Console.Clear();
      Console.WriteLine($"Rozdziel punkty umiejętności (wpisując liczbę i klikając ENTER): {pktStart}/3");
      Console.WriteLine($"1. Siła {hero.StrengthLvl}\n2. Szybkość {hero.Speed}\n3. Magia {hero.MagicLvl}\n4. Alchemia {hero.AlchemyLvl}\n");
      Console.Write("-> ");

      while (pktStart > 0)
      {
        string? option = Console.ReadLine();
        switch (option)
        {
          case "1": hero.StrengthLvl = hero.StrengthLvl + 1; pktStart--; break;
          case "2": hero.Speed = hero.Speed + 1; pktStart--; break;
          case "3": hero.MagicLvl = hero.MagicLvl + 1; pktStart--; break;
          case "4": hero.AlchemyLvl = hero.AlchemyLvl + 1; pktStart--; break;
          default: Console.WriteLine("\nW waszym cechu nie uczą liczyć do 4?"); Console.ReadKey(); break;
        }
        Console.Clear();
        Console.WriteLine($"Rozdziel punkty umiejętności: {pktStart}/3");
        Console.WriteLine($"1. Siła {hero.StrengthLvl}\n2. Szybkość {hero.Speed}\n3. Magia {hero.MagicLvl}\n4. Alchemia {hero.AlchemyLvl}\n");
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
        MonsterTutorial ghul = new MonsterTutorial(hero);
        NPC("W takim razie chodź za mną... ", "Wieśniak");
        Narrator("Wieśniak prowadzi Cię do stodoły na skraju wsi.");
        Gracz("Zajmę się tym. Schowaj się w jakiejś chacie i ostrzeż ludzi.");
        Narrator("Wieśniak kiwa głową i odchodzi.\nOstrożnie obchodzisz stodołę, w hałdzie gnoju za nią widać niewielkie gniazdo, w którym siedzi jeden Ghul.");
        Narrator("Widzisz jak potwór podnosi łeb i wietrzy Twój zapach. Susem wyskakuje z gniazda i zaczyna się do Ciebie zbliżać.\nSięgasz po miecz...");
        ghul.Walka();
        Narrator("Potwór pada na ziemię i lekko tylko drga.\nDobijasz go zdecydowanym ruchem.");
        Narrator("Wychodzisz przed stodołę i wołasz wieśniaka. Widzisz go, jak wychodzi z sąsiedniej chaty.");
        Gracz("Załatwiłem tego ghula. Pamiętajcie, aby spalić jego truchło i gniazdo. ");
        NPC("Oczywiście panie wiedźmin. Dziękujemy za pomoc. Oto pieniądze. Złożyliśmy się całą wsią na tego ghula. ", "Wieśniak");
        Narrator("Oczywiście panie wiedźmin. Dziękujemy za pomoc. Oto pieniądze. Złożyliśmy się całą wsią na tego ghula. ");
        Gracz("Mhm... Bywajcie.");
        Narrator("Wieśniak kiwa Ci głową na pożegnanie i odchodzi. Pora, aby ruszać w dalszą drogę. ");
        Narrator("Ukończyłeś samouczek! Powodzenia na szlaku.");
      }
      else
      {
        NPC("Ah tak..\n\tTo wynoś się z naszej wioski na szlak odmieńcze!", "Wieśniak");
      }
      Narrator("Wyruszasz teraz w samotną drogę gdzie spotaksz wiele niebezpicznych stworzeń z którymi przyjdzie Ci się zmierzyć.");
    }
    public static void LoadGame(Hero hero)
    {
      Narrator("Witaj na szlaku! Pokonaj jak najwięcej potworów i nie daj się zabić. Powodzenia!");
      bool wyjscie = false;

      while (!wyjscie)
      {
        wyjscie = MenuNaSzlaku(hero);
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
    public static bool MenuNaSzlaku(Hero hero)
    {
      bool wyjscieGracz = true;
      int aktywnaPozycjaMenu = 0;
      string[] pozycjeMenu = { "Walka", "Znaki", "Eliksiry", "Zapisz stan gry (WYJŚĆIE)" };
      while (wyjscieGracz)
      {
        PokazMenu(aktywnaPozycjaMenu, pozycjeMenu, hero);
        aktywnaPozycjaMenu = WybieranieOpcji(aktywnaPozycjaMenu, pozycjeMenu, hero);
        wyjscieGracz = UruchomOpcje(aktywnaPozycjaMenu, hero);
      }
      return true;
    }
    public static void PokazMenu(int aktywnaPozycjaMenu, string[] pozycjeMenu, Hero hero)
    {
      Console.BackgroundColor = ConsoleColor.Black;
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine($"Na Szlaku\tPokonane potwory: {hero.MonsterKill}");
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
      ;
    }
    public static int WybieranieOpcji(int aktywnaPozycjaMenu, string[] pozycjeMenu, Hero hero)
    {
      do
      {
        ConsoleKeyInfo klawisz = Console.ReadKey();
        if (klawisz.Key == ConsoleKey.UpArrow)
        {
          aktywnaPozycjaMenu = (aktywnaPozycjaMenu > 0) ? aktywnaPozycjaMenu - 1 : pozycjeMenu.Length - 1;
          PokazMenu(aktywnaPozycjaMenu, pozycjeMenu, hero);
        }
        else if (klawisz.Key == ConsoleKey.DownArrow)
        {
          aktywnaPozycjaMenu = (aktywnaPozycjaMenu + 1) % pozycjeMenu.Length;
          PokazMenu(aktywnaPozycjaMenu, pozycjeMenu, hero);
        }
        else if (klawisz.Key == ConsoleKey.Enter)
        {
          break;
        }
      } while (true);
      return aktywnaPozycjaMenu;
    }
    public static bool UruchomOpcje(int aktywnaPozycjaMenu, Hero hero)
    {
      switch (aktywnaPozycjaMenu)
      {
        case 0:
          Random rnd = new Random();
          string[] monsters = { "ghul", "utopiec", "baba_wodna", "nekker", "wampir", "syrena", "arachnomorf", "kikimora", "leszy", "strzyga", "bies" };
          int random = rnd.Next() % monsters.Length;
          Monster monster = new Monster(monsters[random], hero);
          if (monster.Walka())
          {
            return false;
          }
          else
          {
            return true;
          }
        case 1:; break;
        case 2:; break;
        case 3: return false;
      }
      return true;
    }
  }
}

//Wzór JSON
// {
//   "Name": "",
//   "Speed": 2,
//   "StrengthLvl": 2,
//   "MagicLvl": 2,
//   "AlchemyLvl": 2,
//   "HP": 100,
//   "XP": 0,
//   "Storage": 10,
//   "Gold": 10,
//   "Level": 1,
//   "MonsterKill":0
// }
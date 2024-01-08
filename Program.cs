using System;
using System.ComponentModel;
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
        LoadGame(hero, load);
        if (!hero.isAlive)
        {
          Narrator("Następna gra rozpocznie się od nowa");

          File.Delete("./hero.txt");
          StreamWriter sw = new StreamWriter("./hero.txt");

          sw.WriteLine($"{hero.Name}");
          sw.Close();

          hero.Death();
          hero.Save(load);

        }
        else
        {
          hero.Save(load);
          Narrator("Pomyślnie zapisano stan gry");
        }
      }
    }
    public static void NewGame(Hero hero)
    {
      Console.CursorVisible = false;
      Narrator("Kolejną godzinę siedzisz w karczmie i powoli sączysz kolejne, mocno zaprawione wodą piwo. Stół lepi się od brudu, karczmarz łypie na Ciebie spode łba.");
      Narrator("To tylko kolejny przystanek na Twojej drodze po opuszczeniu Siedliszcza.\nWydawałoby się, że świat pełen jest potworów,jednak na razie nie natknąłeś się na żadnego z nich.\nByć może tym razem ktoś się do Ciebie zwróci?");

      String line;
      StreamReader sr = new StreamReader("./hero.txt");
      line = sr.ReadLine();
      sr.Close();

      Narrator($"Wyciągasz na wierzch koszuli swój medalion.\nMoże to zwróci uwagę potencjalnego klienta. Oby się ktoś znalazł bo w sakiewce nie został Ci już ani jeden grosz.\nWtapiasz wzrok w stółna którym ktoś wyrył swoje imię \"{line}\". ");
      Narrator("Czujesz że ktoś łapie Cię za ramię.");

      NPC("Przepraszam najmocniej, nie widziałem Pana nigdy w mojej wsi.\n\tNazwyam się Bolko. Jestem sołtysem tej wioski.\n\tA Wy kim jesteście?", "???");

      Console.Clear();
      Console.CursorVisible = true;
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("\nWprowadź swoje imie: ");
      Console.Write("\n-> ");
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
        Console.WriteLine($"Rozdziel punkty umiejętności wpisując liczbę i klikając ENTER): {pktStart}/3");
        Console.WriteLine($"1. Siła {hero.StrengthLvl}\n2. Szybkość {hero.Speed}\n3. Magia {hero.MagicLvl}\n4. Alchemia {hero.AlchemyLvl}\n");
        if (pktStart > 0) Console.Write("-> ");
      }
      Console.WriteLine("Aby kontynuować wciśnij przycisk.");
      Console.ReadKey();
      Console.Clear();

      Console.CursorVisible = false;
      Gracz($"Jestem {hero.Name}, jestem wiedźminem");

      string tutorial = $"Balko:\n\n\tAaa pan wiedźmin! Jak to dobrze żeście tu trafili. Mamy problem z Ghulami. Pomoglibyście?";

      MenuDialog menuDialog = new MenuDialog();
      bool wybor = menuDialog.Wybor(tutorial);

      if (wybor)
      {
        MonsterTutorial ghul = new MonsterTutorial(hero);
        NPC("W takim razie chodź za mną... ", "Balko");
        Narrator("Balko prowadzi Cię do stodoły na skraju wsi.");
        Gracz("Zajmę się tym. Schowaj się w jakiejś chacie i ostrzeż ludzi.");
        Narrator("Sołtys kiwa głową i odchodzi.\nOstrożnie obchodzisz stodołę, w hałdzie gnoju za nią widać niewielkie gniazdo, w którym siedzi jeden Ghul.");
        Narrator("Widzisz jak potwór podnosi łeb i wietrzy Twój zapach. Susem wyskakuje z gniazda i zaczyna się do Ciebie zbliżać.\nSięgasz po miecz...");
        ghul.Walka();
        Narrator("Potwór pada na ziemię i lekko tylko drga.\nDobijasz go zdecydowanym ruchem.");
        Narrator("Wychodzisz przed stodołę i wołasz sołtysa. Widzisz go, jak wychodzi z sąsiedniej chaty.");
        Gracz("Załatwiłem tego ghula. Pamiętajcie, aby spalić jego truchło i gniazdo. ");
        NPC("Oczywiście panie wiedźmin. Dziękujemy za pomoc. Oto pieniądze. Złożyliśmy się całą wsią na tego ghula. ", "Balko");
        Narrator("Wieśniak podaje Ci sakiewkę. Zaglądasz do środka - jest w niej 100 monet. ");
        Gracz("Mhm... Bywajcie.");
        Narrator("Balko kiwa Ci głową na pożegnanie i odchodzi. Pora, aby ruszać w dalszą drogę. ");
        Narrator("Ukończyłeś samouczek!");
      }
      else
      {
        NPC("Ah tak..\n\tW takim razie nie ma tu dla was miejsca! Wynoście się stąd!", "Balko");
        Narrator("Spokojny i opanowany wychodzisz z karczmy i ruszasz w dalszą drogę. Wiesz że po drodze spotaksz wiele niebezpicznych stworzeń z którymi przyjdzie Ci się zmierzyć.");
      }

    }
    public static void LoadGame(Hero hero, JObject load)
    {
      Narrator("Witaj na szlaku! Pokonaj jak najwięcej potworów i nie daj się zabić. Powodzenia!");
      bool wyjscie = false;

      while (!wyjscie)
      {
        wyjscie = MenuNaSzlaku(hero, load);
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
    public static bool MenuNaSzlaku(Hero hero, JObject load)
    {
      bool wyjscieGracz = true;
      int aktywnaPozycjaMenu = 0;
      string[] pozycjeMenu = { "Walka", "Znaki", "Eliksiry", "WYJŚĆIE" };
      while (wyjscieGracz)
      {
        hero.Refresh();
        PokazMenu(aktywnaPozycjaMenu, pozycjeMenu, hero);
        aktywnaPozycjaMenu = WybieranieOpcji(aktywnaPozycjaMenu, pozycjeMenu, hero);
        wyjscieGracz = UruchomOpcje(aktywnaPozycjaMenu, hero);
        hero.Save(load);
      }
      return true;
    }
    public static void PokazMenu(int aktywnaPozycjaMenu, string[] pozycjeMenu, Hero hero)
    {
      Console.BackgroundColor = ConsoleColor.Black;
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine($"\nImie: {hero.Name} | Lvl: {hero.Level} | HP: {hero.HP}\n");
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.WriteLine($"\t~ Witaj na Wiedźmińskim Szlaku ~");
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
      Console.ForegroundColor = ConsoleColor.DarkBlue;
      Console.Write($"\nPokonane potwory: {hero.MonsterKill}");
      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.Write($"\tZnak: {hero.znaki[hero.SelectedSign]}");
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.Write($"\tEliksir: {hero.eliksiry[hero.SelectedPotion]}");
      Console.ForegroundColor = ConsoleColor.White;
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
      string text;
      switch (aktywnaPozycjaMenu)
      {
        case 0:
          Random rnd = new Random();
          // string[] monsters = { "ghul", "utopiec", "baba_wodna", "nekker", "wampir", "syrena", "arachnomorf", "kikimora", "leszy", "strzyga", "bies" };
          int random = rnd.Next() % hero.monsters.Count;
          Monster monster = new Monster(hero.monsters[random], hero);
          if (monster.Walka())
          {
            return false;
          }
          break;
        case 1:
          text = "Wybierz znak z którego będziesz kożystać w walce";
          MenuOption menuZnak = new MenuOption(hero.SelectedSign, hero.znaki, hero, hero.znakiDesc);
          hero.SelectedSign = menuZnak.Wybor(text);
          break;
        case 2:
          text = "Wybierz Eliksir z którego będziesz kożystać w walce";
          MenuOption menuEliksiry = new MenuOption(hero.SelectedPotion, hero.eliksiry, hero, hero.eliksiryDesc);
          hero.SelectedPotion = menuEliksiry.Wybor(text);
          break;
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
//   "XP": 0,
//   "Level": 1,
//   "MonsterKill":0,
//   "SelectedSign": 0,
//   "SelectedPotion": 0
// }
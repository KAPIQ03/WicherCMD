using System.Runtime.InteropServices;

namespace Gra.Klasy;

public class MenuOption
{
  private int aktywnaPozycjaMenu;
  private int aktywnaPozycjaMenuCopy;
  private List<string> pozycjeMenu;
  private List<string> Desc;
  private Hero hero;

  public MenuOption(int selectedPosicion, List<string> option, Hero heroLoad, List<string> desc)
  {
    aktywnaPozycjaMenu = selectedPosicion;
    aktywnaPozycjaMenuCopy = selectedPosicion;
    pozycjeMenu = option;
    hero = heroLoad;
    Desc = desc;
  }
  public int Wybor(string text)
  {
    Console.CursorVisible = false;

    while (true)
    {
      PokazMenu(text);
      WybieranieOpcji(text);
      return UruchomOpcje();
    }
  }
  private void PokazMenu(string text)
  {
    Console.BackgroundColor = ConsoleColor.Black;
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine("\n\t~ " + text + " ~");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
    for (int i = 0; i < pozycjeMenu.Count; i++)
    {
      if (i == aktywnaPozycjaMenu)
      {
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("\t➤  {0,-10} ", pozycjeMenu[i]);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
      }
      else
      {
        Console.WriteLine("\t" + pozycjeMenu[i]);
      }
    }
    if (aktywnaPozycjaMenu != pozycjeMenu.Count - 1)
    {
      Console.ForegroundColor = ConsoleColor.DarkGray;
      Console.WriteLine("\n\tOpis:");
      Console.WriteLine($"\t{Desc[aktywnaPozycjaMenu]}");
    }
  }
  private void WybieranieOpcji(string text)
  {
    do
    {
      ConsoleKeyInfo klawisz = Console.ReadKey();
      if (klawisz.Key == ConsoleKey.UpArrow)
      {
        aktywnaPozycjaMenu = (aktywnaPozycjaMenu > 0) ? aktywnaPozycjaMenu - 1 : pozycjeMenu.Count - 1;
        PokazMenu(text);
      }
      else if (klawisz.Key == ConsoleKey.DownArrow)
      {
        aktywnaPozycjaMenu = (aktywnaPozycjaMenu + 1) % pozycjeMenu.Count;
        PokazMenu(text);
      }
      else if (klawisz.Key == ConsoleKey.Enter)
      {
        break;
      }
    } while (true);
  }
  private int UruchomOpcje()
  {
    if (aktywnaPozycjaMenu == pozycjeMenu.Count - 1)
    {
      return aktywnaPozycjaMenuCopy;
    }
    else
    {
      return aktywnaPozycjaMenu;
    }
  }

}


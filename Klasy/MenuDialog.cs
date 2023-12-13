namespace Gra.Klasy;

public class MenuDialog
{
  private int aktywnaPozycjaMenu = 0;
  private string[] pozycjeMenu = { "Tak", "Nie" };

  public bool Wybor(string text)
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
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(text);
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
    for (int i = 0; i < pozycjeMenu.Length; i++)
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
  }
  private void WybieranieOpcji(string text)
  {
    do
    {
      ConsoleKeyInfo klawisz = Console.ReadKey();
      if (klawisz.Key == ConsoleKey.UpArrow)
      {
        aktywnaPozycjaMenu = (aktywnaPozycjaMenu > 0) ? aktywnaPozycjaMenu - 1 : pozycjeMenu.Length - 1;
        PokazMenu(text);
      }
      else if (klawisz.Key == ConsoleKey.DownArrow)
      {
        aktywnaPozycjaMenu = (aktywnaPozycjaMenu + 1) % pozycjeMenu.Length;
        PokazMenu(text);
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
      case 0: return true;
      case 1: return false;
      default: return false;
    }
  }

}

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Gra.Klasy;

public class Hero
{
  public string? Name;
  public int Strength;
  public int StrengthLvl;
  public int Speed;
  public int MagicLvl;
  public string Eliksir;
  public int AlchemyLvl;
  public int HP;
  public int XP;
  public int Level;
  public int MonsterKill;
  public int SelectedSign;
  public int SelectedPotion;
  public List<string> znaki = new List<string>();
  public List<string> znakiDesc = new List<string>();
  public List<string> eliksiry = new List<string>();
  public List<string> eliksiryDesc = new List<string>();
  public List<string> monsters = new List<string>();
  public bool isAlive = true;
  public Hero(JObject load)
  {
    JObject Load = load;
    Init((int)Load["StrengthLvl"], (int)Load["Speed"], (int)Load["MagicLvl"], (int)Load["AlchemyLvl"], (int)Load["XP"], (int)Load["Level"], (string)Load["Name"], (int)Load["MonsterKill"], (int)Load["SelectedSign"], (int)Load["SelectedPotion"]);
  }
  private void Init(int strength, int speed, int magic, int alchemy, int xp, int level, string name, int monsterKill, int selectedSign, int selectedPotion)
  {
    this.Name = name;
    this.StrengthLvl = strength;
    this.Speed = speed;
    this.Strength = 10;
    this.MagicLvl = magic;
    this.SelectedSign = selectedSign;
    this.AlchemyLvl = alchemy;
    this.SelectedPotion = selectedPotion;
    this.XP = xp;
    this.Level = level;
    this.HP = 100;
    this.MonsterKill = monsterKill;
  }
  public void Refresh()
  {
    znaki.Clear();
    znakiDesc.Clear();
    eliksiry.Clear();
    eliksiryDesc.Clear();
    monsters.Clear();

    if (StrengthLvl >= 2 && StrengthLvl < 5)
    {
      this.Strength = 10;
    }
    else if (StrengthLvl >= 5 && StrengthLvl < 8)
    {
      this.Strength = 15;
    }
    else if (StrengthLvl >= 8 && StrengthLvl < 11)
    {
      this.Strength = 20;
    }
    else
    {
      this.Strength = 30;
    }

    if (MagicLvl >= 2)
    {
      znaki.Add("Yrden");
      znakiDesc.Add("Zatrzymuje przeciwnika w bezruchu. Przeciwnik nie może się ruszyć przez 1 turę");
    }
    if (MagicLvl >= 5)
    {
      znaki.Add("Aksi");
      znakiDesc.Add("Omamia przeciwnika. Przeciwnik nie może się ruszyć przez 2 turę");
    }
    if (MagicLvl >= 8)
    {
      znaki.Add("Quen");
      znakiDesc.Add("Tworzy barierę ochronną. Dodaje 50 HP");
    }
    if (MagicLvl >= 11)
    {
      znaki.Add("Ard");
      znakiDesc.Add("Atakuje przeciwnika podmuchem wiatru. Zadajesz 20pkt obrażeń");
    }
    if (MagicLvl >= 14)
    {
      znaki.Add("Igni");
      znakiDesc.Add("Atakuje przeciwnika ognistymi iskrami. Zadajesz 50pkt obrażeń");
    }
    znaki.Add("Powrót");

    if (AlchemyLvl >= 2)
    {
      eliksiry.Add("Jaskółka");
      eliksiryDesc.Add("Uzdrowicielski Eliksir. Po zarzyciu uzdrawia 5HP na ture");
    }
    if (AlchemyLvl >= 5)
    {
      eliksiry.Add("Zamieć");
      eliksiryDesc.Add("Wpływa na czas reakcji wiedźmina, przyśpieszając jego refleks i ruchy. Po zarzyciu zwiększa szybkosc o 20%");
    }
    if (AlchemyLvl >= 8)
    {
      eliksiry.Add("Grom");
      eliksiryDesc.Add("Wypicie eliksiru inicjuje trans bitewny. Po zarzyciu zwiększa obrażenia o 10%");
    }
    if (AlchemyLvl >= 11)
    {
      eliksiry.Add("Ulepszona Jaskółka");
      eliksiryDesc.Add("Uzdrowicielski Eliksir. Po zarzyciu uzdrawia 10HP na ture");
    }
    if (AlchemyLvl >= 14)
    {
      eliksiry.Add("Ulepszony Grom");
      eliksiryDesc.Add("Wypicie eliksiru inicjuje trans bitewny. Po zarzyciu zwiększa obrażenia o 20%");
    }
    eliksiry.Add("Powrót");

    if (Level >= 1)
    {
      HP = 100;
      monsters.Add("ghul");
      monsters.Add("utopiec");
    }
    if (Level >= 5)
    {
      HP = 150;
      monsters.Add("baba_wodna");
      monsters.Add("nekker");
    }
    if (Level >= 8)
    {
      HP = 200;
      monsters.Add("wampir");
      monsters.Add("syrena");
    }
    if (Level >= 11)
    {
      HP = 250;
      monsters.Add("arachnomorf");
      monsters.Add("kikimora");
    }
    if (Level >= 14)
    {
      HP = 300;
      monsters.Add("leszy");
      monsters.Add("strzyga");
      monsters.Add("bies");
    }
  }
  public void levelUP()
  {
    Level++;
    StrengthLvl++;
    AlchemyLvl++;
    MagicLvl++;
    if (Level % 3 == 0) Speed++;
    XP = 0;
    Console.Clear();
    Console.WriteLine("LEVEL UP!");
    Console.WriteLine($"\tSiła: {StrengthLvl - 1} -> {StrengthLvl}");
    if (Level % 3 == 0) Console.WriteLine($"\tSzybkość: {Speed - 1} -> {Speed}");
    Console.WriteLine($"\tMagia: {MagicLvl - 1} -> {MagicLvl}");
    Console.WriteLine($"\tAlchemia: {AlchemyLvl - 1} -> {AlchemyLvl}");
    Console.ReadKey();
  }
  public void Save(JObject load)
  {
    JObject Hero = load;
    Hero["StrengthLvl"] = StrengthLvl;
    Hero["Speed"] = Speed;
    Hero["MagicLvl"] = MagicLvl;
    Hero["AlchemyLvl"] = AlchemyLvl;
    Hero["XP"] = XP;
    Hero["Level"] = Level;
    Hero["Name"] = Name;
    Hero["MonsterKill"] = MonsterKill;
    Hero["SelectedSign"] = SelectedSign;
    Hero["SelectedPotion"] = SelectedPotion;

    File.WriteAllText("./hero.json", Hero.ToString());
  }
  public void Death()
  {
    this.Name = "";
    this.Speed = 2;
    this.StrengthLvl = 2;
    this.MagicLvl = 2;
    this.AlchemyLvl = 2;
    this.HP = 100;
    this.XP = 0;
    this.Level = 1;
    this.MonsterKill = 0;
    this.SelectedPotion = 0;
    this.SelectedSign = 0;
  }
}

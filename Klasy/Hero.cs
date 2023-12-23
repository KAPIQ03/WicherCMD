using Newtonsoft.Json.Linq;

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
  public int Gold;
  public int Level;
  public int Storage;
  public int MonsterKill;
  public string Znak;
  public bool isAlive = true;

  public Hero(JObject load)
  {
    JObject Load = load;
    Init((int)Load["StrengthLvl"], (int)Load["Speed"], (int)Load["MagicLvl"], (int)Load["AlchemyLvl"], (int)Load["XP"], (int)Load["Gold"], (int)Load["Level"], (int)Load["Storage"], (string)Load["Name"], (int)Load["MonsterKill"]);
  }
  private void Init(int strength, int speed, int magic, int alchemy, int xp, int gold, int level, int storage, string name, int monsterKill)
  {
    this.Name = name;
    if (strength >= 2 && strength < 5)
    {
      this.Strength = 10;
    }
    else if (strength >= 5 && strength < 8)
    {
      this.Strength = 15;
    }
    else if (strength >= 8 && strength < 11)
    {
      this.Strength = 20;
    }
    else
    {
      this.Strength = 30;
    }
    this.StrengthLvl = strength;
    this.Speed = speed;
    this.MagicLvl = magic;
    if (magic >= 2 && magic < 5)
    {
      this.Znak = "Yrden";
    }
    else if (magic >= 5 && magic < 8)
    {
      this.Znak = "Axi";
    }
    else if (magic >= 8 && magic < 11)
    {
      this.Znak = "Quen";
    }
    else if (magic >= 11 && magic < 14)
    {
      this.Znak = "Ard";
    }
    else
    {
      this.Znak = "Igni";
    }
    this.AlchemyLvl = alchemy;
    if (alchemy >= 2 && alchemy < 5)
    {
      this.Eliksir = "Jaskółka";
    }
    else if (magic >= 5 && magic < 8)
    {
      this.Znak = "Grom";
    }
    else if (magic >= 8 && magic < 11)
    {
      this.Znak = "Filtr Periego";
    }
    else if (magic >= 11 && magic < 14)
    {
      this.Znak = "Pełnia";
    }
    else
    {
      this.Znak = "Zamieć";
    }
    this.XP = xp;
    this.Gold = gold;
    this.Level = level;
    if (level >= 1 && level < 5)
    {
      this.HP = 100;
    }
    else if (level >= 5 && level < 8)
    {
      this.HP = 150;
    }
    else if (level >= 8 && level < 11)
    {
      this.HP = 200;
    }
    else if (level >= 11 && level < 14)
    {
      this.HP = 250;
    }
    else
    {
      this.HP = 300;
    }
    this.Storage = storage;
    this.MonsterKill = monsterKill;
  }

  // public void UpStrength() { this.Strength += 5; this.HP += 5; }
  // public void UpDexterity() { this.Dexterity += 5; }
  // public void UpIntelligence() { this.Intelligence += 5; this.MP += (3 * this.Intelligence); }
  public static void Save(JObject load, Hero hero)
  {
    JObject Hero = load;
    Hero["StrengthLvl"] = hero.StrengthLvl;
    Hero["Speed"] = hero.Speed;
    Hero["MagicLvl"] = hero.MagicLvl;
    Hero["AlchemyLvl"] = hero.AlchemyLvl;
    Hero["HP"] = hero.HP;
    Hero["XP"] = hero.XP;
    Hero["Storage"] = hero.Storage;
    Hero["Gold"] = hero.Gold;
    Hero["Level"] = hero.Level;
    Hero["Name"] = hero.Name;
    Hero["MonsterKill"] = hero.MonsterKill;

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
    this.Storage = 10;
    this.Gold = 10;
    this.Level = 1;
    this.MonsterKill = 0;
  }
}

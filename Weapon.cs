using System;

namespace DungeonExplorer
{
    public class Weapon : Item
    {
        public int Damage { get; set; }
        // Constructor for weapon class, inherits most things from item.
        public Weapon(string name, string description, int damage)
            : base(name, description)
        {
            Damage = damage;
        }

        public override void Use(Creature target)
        {
            Console.WriteLine($"{Name} is a weapon and can't be 'used' directly like a potion.");
        }
    }
    // Sword class that inherits from weapon.
    public class Sword : Weapon
    {
        public Sword(string name, string description, int damage)
            : base(name, description, damage)
        {
        }
    }
}

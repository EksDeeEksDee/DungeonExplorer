using System;

namespace DungeonExplorer
{
    public class Weapon : Item
    {
        public int Damage { get; set; }

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

    public class Sword : Weapon
    {
        public Sword(string name, string description, int damage)
            : base(name, description, damage)
        {
        }
    }
}

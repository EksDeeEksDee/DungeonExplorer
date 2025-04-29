using System;


namespace DungeonExplorer
{
    public class Potion : Item
    {
        public Potion(string name, string description)
            : base(name, description)
        {
        }

        // Default behavior for using a potion
        public override void Use(Creature target)
        {
            Console.WriteLine($"{Name} has no defined use.");
        }
    }

    public class StrengthPotion : Potion
    {
        public StrengthPotion() : base("Strength Potion", "Multiplies the damage of your next attack by 1.5.") { }

        public override void Use(Creature target)
        {
            target.DamageMultiplier = 1.5;
            Console.WriteLine($"{target.Name} used {Name} and increased their strength for 1 turn!");
        }
    }
    public class HealingPotion : Potion
    {

        public HealingPotion() : base("Healing Potion", "Heals 20 health.") { }
        public override void Use(Creature target)
        {
            Console.WriteLine($"{target.Name} used {Name}");
            target.Heal(20);
        }
    }
}

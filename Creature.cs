using System;

namespace DungeonExplorer
{
    public abstract class Creature : IDamageable // Inherits from IDamageable interface
    {

        // Creating parameters.
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Damage { get; set; }
        public double DamageMultiplier { get; set; }

        // Constructor for creature class.
        public Creature(string name, int health)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            Damage = 1; // Default damage.
            DamageMultiplier = 1.0; // Default damage multiplier.
        }
        // Method for taking damage.
        public virtual void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                Health = 0;
                Console.Clear();
                Console.WriteLine($"{Name} has been killed!");
            }
        }

        // Method for healing.
        public virtual void Heal(int amount)
        {
            Health += amount;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            Console.WriteLine($"{Name} healed for {amount} health!");
            Console.WriteLine($"{Name}'s health is now {Health}!");
        }
        // Method for attacking - left blank as every creatue attacks differently.
        public abstract int Attack(Creature target);
    }
}

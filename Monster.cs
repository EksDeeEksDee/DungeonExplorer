using System;

namespace DungeonExplorer
{
    public abstract class Monster : Creature
    {
        protected int MinDamage;
        protected int MaxDamage;
        public int XPReward { get; protected set; }
        protected Random random = new Random();
        protected int healsRemaining;
        protected double healChance;


        // Constructor for Monster, inherits from creature but also adds extra attributes.
        public Monster(string name, int health, int minDamage, int maxDamage, int xPReward)
            : base(name, health)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            XPReward = xPReward;
        }

        // Healing logic - only allows for a specified amount of heals.
        protected virtual bool TryHeal()
        {
            if (healsRemaining > 0 && Health < MaxHealth / 2 && random.NextDouble() < healChance)
            {
                int healAmount = 20;
                Heal(healAmount);
                healsRemaining--;
                return true;
            }
            return false;
        }
    }

    public class Spider : Monster
    {
        public Spider() : base("Spider", 30, 3, 8, 30)
        {
            healsRemaining = 0; // Spiders cannot heal
            healChance = 0.0;
        }

        public override int Attack(Creature target)
        {
            int damage = random.Next(MinDamage, MaxDamage + 1);
            Console.WriteLine($"The Spider bites you and deals {damage} damage!");
            target.TakeDamage(damage);
            return damage;
        }
    }

    public class Goblin : Monster
    {
        public Goblin() : base("Goblin", 50, 5, 12, 50)
        {
            healsRemaining = 1;
            healChance = 0.2; // 20% chance
        }

        public override int Attack(Creature target)
        {
            if (TryHeal()) return 0;

            int damage = random.Next(MinDamage, MaxDamage + 1);
            Console.WriteLine($"The Goblin lunges at you and deals {damage} damage!");
            target.TakeDamage(damage);
            return damage;
        }
    }

    public class GoblinWarrior : Monster
    {
        public GoblinWarrior() : base("Goblin Warrior", 100, 9, 19, 80)
        {
            healsRemaining = 1;
            healChance = 0.1; // 10% chance
        }

        public override int Attack(Creature target)
        {
            if (TryHeal()) return 0;

            int damage = random.Next(MinDamage, MaxDamage + 1);
            Console.WriteLine($"The Goblin Warrior strikes you and deals {damage} damage!");
            target.TakeDamage(damage);
            return damage;
        }
    }

    public class GoblinChief : Monster
    {
        public GoblinChief() : base("Goblin Chief", 80, 8, 15, 100)
        {
            healsRemaining = 2;
            healChance = 0.05; // 5% chance
        }

        public override int Attack(Creature target)
        {
            if (TryHeal()) return 0;

            int damage = random.Next(MinDamage, MaxDamage + 1);
            Console.WriteLine($"The Goblin Chief shoots a magic ball and deals {damage} damage!");
            target.TakeDamage(damage);
            return damage;
        }
    }

    public class StoneKnight : Monster
    {
        public StoneKnight() : base("Stone Knight", 120, 10, 18, 150)
        {
            healsRemaining = 2;
            healChance = 0.15; // 15% chance
        }

        public override int Attack(Creature target)
        {
            if (TryHeal()) return 0;

            int damage = random.Next(MinDamage, MaxDamage + 1);
            Console.WriteLine($"The Stone Knight slashes with his giant sword and deals {damage} damage!");
            target.TakeDamage(damage);
            return damage;
        }
    }

    public class Dragon : Monster
    {
        private bool enraged = false;

        public Dragon() : base("Ancient Dragon", 200, 15, 24, 300)
        {
            healsRemaining = 1;      // Only one big heal
            healChance = 1.0;        // Always heals if conditions met
        }

        // Dragon has it's own heal logic as it heals more than all the other enemies.
        protected override bool TryHeal()
        {
            if (healsRemaining > 0 && Health < MaxHealth / 2 && random.NextDouble() < healChance)
            {
                int healAmount = 60 // Dragon heals more
                Heal(healAmount);
                healsRemaining--;
                Console.WriteLine($"{Name} channels ancient power and heals for {healAmount} HP!");
                return true;
            }
            return false;
        }

        public override int Attack(Creature target)
        {
            TryHeal();
            // Checks to see if the dragon isn't enraged already.
            if (!enraged && Health < MaxHealth / 2) // The dragon is enraged when it's health is lower than 50%.
            {
                enraged = true;
                MinDamage += 8; // Being enraged increases the dragon's overall damage.
                MaxDamage += 7;
                Console.WriteLine($"{Name} enters a furious rage!");
            }
            
            int fireBreath = random.Next(MinDamage, MaxDamage + 1);
            Console.WriteLine($"{Name} breathes fire for {fireBreath} damage!");
            target.TakeDamage(fireBreath);
            // Dragon has a chance to strike twice.
            if (random.NextDouble() < 0.5)
            {
                int clawDamage = random.Next(10, 16);
                Console.WriteLine($"{Name} strikes with claws for {clawDamage} bonus damage!");
                target.TakeDamage(clawDamage);
            }

            return 0;
        }
    }
}

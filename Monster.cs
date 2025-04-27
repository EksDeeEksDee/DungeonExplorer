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

        public Monster(string name, int health, int minDamage, int maxDamage, int xPReward)
            : base(name, health)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            XPReward = xPReward;
        }

        // Healing logic - only allows for a specified amount of heals.
        protected bool TryHeal()
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
        public Goblin() : base("Goblin", 50, 7, 17, 50)
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
        public GoblinWarrior() : base("Goblin Warrior", 60, 9, 19, 80)
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
        public GoblinChief() : base("Goblin Chief", 80, 12, 18, 100)
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
        public StoneKnight() : base("Stone Knight", 120, 15, 22, 150)
        {
            healsRemaining = 1;
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

        public Dragon() : base("Ancient Dragon", 200, 20, 30, 300)
        {
            healsRemaining = 1;      // Only one big heal
            healChance = 1.0;        // Always heals if conditions met
        }

        public override int Attack(Creature target)
        {
            TryHeal();

            if (!enraged && Health < MaxHealth / 2)
            {
                enraged = true;
                MinDamage += 8;
                MaxDamage += 12;
                Console.WriteLine($"{Name} enters a furious rage!");
            }

            int fireBreath = random.Next(MinDamage, MaxDamage + 1);
            Console.WriteLine($"{Name} breathes fire for {fireBreath} damage!");
            target.TakeDamage(fireBreath);

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

using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    public class Player : Creature
    {
        private Inventory inventory = new Inventory();

        public int Experience;
        public int Level;

        public Player(string name, int health, int experience) : base(name, health) 
        {
            Experience = experience;
            Level = 1;
        }

        public Statistics Stats { get; private set; } = new Statistics();

        public void PickUpItem(string itemName, Room currentRoom)
        {
            var item = currentRoom.GetRoomItems().FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                Console.WriteLine($"Item '{itemName}' not found in the room.");
                return;
            }
            inventory.AddItem(item);
            Stats.ItemPickedUp();
            currentRoom.RemoveItem(item);
        }

        public void UseItem(string itemName)
        {
            inventory.UseItem(itemName, this);
            Stats.PotionUsed();
        }

        public override int Attack(Creature target)
        {
            var weaponItems = inventory.GetAllItems().OfType<Weapon>().ToList();

            if (weaponItems.Count == 0)
            {
                // No weapons, use fist
                int damage = (int)(5 * DamageMultiplier);
                Console.WriteLine($"{Name} punches {target.Name} with their fist for {damage} damage!");
                target.TakeDamage(damage);
                DamageMultiplier = 1.0;
                return damage;
            }

            // If they have weapons, ask them to choose
            Console.WriteLine("Available weapons: " + string.Join(", ", weaponItems.Select(w => w.Name)));

            Weapon selectedWeapon = null;
            while (selectedWeapon == null)
            {
                Console.Write("Choose your weapon: ");
                string weaponName = Console.ReadLine().Trim();
                var item = inventory.GetItemByName(weaponName);

                if (item is Weapon weapon)
                {
                    selectedWeapon = weapon;
                }
                else
                {
                    Console.WriteLine("Invalid weapon. Please select a valid weapon from your inventory.");
                }
            }

            int bonusDamage = 0;

            // Apply bonus damage depending on the weapon and enemy
            if (selectedWeapon.Name.IndexOf("Goblin Slayer", StringComparison.OrdinalIgnoreCase) >= 0 &&
                (target.Name.IndexOf("Goblin", StringComparison.OrdinalIgnoreCase) >= 0 || target.Name.IndexOf("Goblin Chief", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                bonusDamage = 10;
            }
            else if (selectedWeapon.Name.IndexOf("Stone Cutter", StringComparison.OrdinalIgnoreCase) >= 0 &&
                     target.Name.IndexOf("Stone Knight", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                bonusDamage = 15;
            }

            int totalDamage = (int)((selectedWeapon.Damage + bonusDamage) * DamageMultiplier);

            Console.WriteLine($"{Name} attacks {target.Name} with {selectedWeapon.Name} for {totalDamage} damage!");
            target.TakeDamage(totalDamage);
            DamageMultiplier = 1.0;
            return totalDamage;
        }

        public override void TakeDamage(int amount)
        {
            Health -= amount;
            Stats.DamageTaken(amount);
            
        }

        public List<Item> GetInventoryItems()
        {
            return inventory.GetAllItems();
        }

        // Returns all item names (for saving)
        public List<string> GetInventoryNames()
        {
            return inventory.GetAllItems().Select(item => item.Name).ToList();
        }

        // Add an item by name (for loading)
        public void AddItemByName(string itemName)
        {
            // Add known items based on names
            if (itemName.Equals("Rusty Key", StringComparison.OrdinalIgnoreCase))
                inventory.AddItem(new Key("Rusty Key", "An old, corroded key."));
            else if (itemName.Equals("Sword", StringComparison.OrdinalIgnoreCase))
                inventory.AddItem(new Sword("Sword", "A basic sword.", 10));
            else if (itemName.Equals("Healing Potion", StringComparison.OrdinalIgnoreCase))
                inventory.AddItem(new HealingPotion());
            else if (itemName.Equals("Strength Potion", StringComparison.OrdinalIgnoreCase))
                inventory.AddItem(new StrengthPotion());
            else if (itemName.Equals("Goblin Slayer Sword", StringComparison.OrdinalIgnoreCase))
                inventory.AddItem(new Sword("Goblin Slayer Sword", "A sword made to destroy goblins.", 10));
            else if (itemName.Equals("Stone Cutter Sword", StringComparison.OrdinalIgnoreCase))
                inventory.AddItem(new Sword("Stone Cutter Sword", "A sword sharp enough to cut through stone.", 20));
        }


        public bool InventoryContains(string itemName)
        {
            return inventory.Contains(itemName);
        }

        public string InventoryContents()
        {
            return inventory.InventoryContents();
        }

        public void SortInventoryByName()
        {
            inventory.SortByName();
        }

        public void SortInventoryByType()
        {
            inventory.SortByType();
        }

        public void SortInventoryByDamage()
        {
            inventory.SortByWeaponDamage();
        }

        public void GainExperience(int amount)
        {
            Experience += amount;
            Console.WriteLine($"{Name} gained {amount} XP!");

            int xpThreshold = Level * 100;
            if (Experience >= xpThreshold)
            {
                Experience -= xpThreshold;
                Level++;
                Health += 20;
                DamageMultiplier += 0.1;
                Console.WriteLine($"{Name} leveled up to Level {Level}! Health and damage multiplier increased!.");
            }
        }
    }
}

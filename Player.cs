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
        // Makes a new instance of statistics to keep track of the player's statistics.
        public Statistics Stats { get; private set; } = new Statistics();

        // Method used for picking up items.
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
        // Method for handling the use of items.
        public void UseItem(string itemName)
        {
            inventory.UseItem(itemName, this);
            Stats.PotionUsed();
        }
        // Metho for attacking allowing the player to choose their weapon, and depending on weapon choice an enemy attacked, the weapon might have a special effect.
        public override int Attack(Creature target)
        {
            var weaponItems = inventory.GetAllItems().OfType<Weapon>().ToList();
            Console.WriteLine("Available weapons: " + string.Join(", ", weaponItems.Select(w => w.Name)));
            Console.Write("Choose your weapon: ");
            string weaponName = Console.ReadLine().ToLower();
            var item = inventory.GetItemByName(weaponName);

            if (item is Weapon weapon)
            {
                int baseDamage = (int)(weapon.Damage * DamageMultiplier);
                int bonusDamage = 0;

                // Special weapon bonuses
                string targetName = target.Name.ToLower();

                if (weaponName.Contains("goblin slayer") && (targetName.Contains("goblin") || targetName.Contains("goblin chief")))
                {
                    bonusDamage = 10;
                }
                else if (weaponName.Contains("magic sword") && targetName.Contains("stone knight"))
                {
                    bonusDamage = 10;
                }

                int totalDamage = baseDamage + bonusDamage;
                Console.WriteLine($"{Name} attacks {target.Name} with {weapon.Name} for {totalDamage} damage!");
                target.TakeDamage(totalDamage);
                DamageMultiplier = 1.0;
                return totalDamage;
            }
            else
            {
                Console.WriteLine("That item is not a usable weapon.");
                return 0;
            }
        }
        // Method for taking damage.
        public override void TakeDamage(int amount)
        {
            Health -= amount;
        }
        // Returns all the players' items as items.
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
                inventory.AddItem(new Key("Rusty Key", "An old, corroded key.", "rusty_001"));
            else if (itemName.Equals("Sword", StringComparison.OrdinalIgnoreCase))
                inventory.AddItem(new Sword("Sword", "A basic sword.", 10));
            else if (itemName.Equals("Healing Potion", StringComparison.OrdinalIgnoreCase))
                inventory.AddItem(new HealingPotion());
            else if (itemName.Equals("Strength Potion", StringComparison.OrdinalIgnoreCase))
                inventory.AddItem(new StrengthPotion());
        }

        // Method used to check if the player has a certain item.
        public bool InventoryContains(string itemName)
        {
            return inventory.Contains(itemName);
        }
        // Method that returns the items in the players' inventory as strings.
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
        // Method used to calculate experience gain and level ups.
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

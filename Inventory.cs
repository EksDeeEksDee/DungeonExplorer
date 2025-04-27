using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    public class Inventory
    {
        private List<Item> items; // A list to store the items in the inventory.

        public Inventory()
        {
            items = new List<Item>(); // Initialize the list of items.
        }

        // Add an item to the inventory.
        public void AddItem(Item item)
        {
            items.Add(item);
            Console.WriteLine($"{item.Name} has been added to your inventory.");
        }

        // Remove an item from the inventory.
        public void RemoveItem(Item item)
        {
            if (items.Contains(item))
            {
                items.Remove(item);
                Console.WriteLine($"{item.Name} has been removed from your inventory.");
            }
            else
            {
                Console.WriteLine($"You do not have {item.Name} in your inventory.");
            }
        }

        // Check if the inventory contains a specific item.
        public bool Contains(string itemName)
        {
            return items.Any(item => item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        }

        // List all items in the inventory.
        public string InventoryContents()
        {
            if (items.Count == 0)
            {
                return "Your inventory is empty.";
            }
            else
            {
                return string.Join(", ", items.Select(item => item.Name));
            }
        }

        // Use an item from the inventory.
        public void UseItem(string itemName, Creature target)
        {
            Item itemToUse = items.FirstOrDefault(item => item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (itemToUse != null)
            {
                itemToUse.Use(target); // Use the item.
            }
            else
            {
                Console.WriteLine($"You don't have {itemName} in your inventory.");
            }
        }


        public List<Item> GetAllItems()
        {
            return items;
        }


        public Item GetItemByName(string itemName)
        {
            return items.FirstOrDefault(item => item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        }

        public void SortByName()
        {
            items = items.OrderBy(item => item.Name).ToList();
            Console.WriteLine("Inventory sorted by name.");
        }

        // Sort weapons by descending damage
        public void SortByWeaponDamage()
        {
            items = items
                .OrderByDescending(item => item is Weapon weapon ? weapon.Damage : 0)
                .ThenBy(item => item.Name)
                .ToList();

            Console.WriteLine("Inventory sorted by weapon damage.");
        }

        // Sort by item type (e.g. all Potions together, then Weapons, etc.)
        public void SortByType()
        {
            items = items
                .OrderBy(item => item.GetType().Name)
                .ThenBy(item => item.Name)
                .ToList();

            Console.WriteLine("Inventory sorted by item type.");
        }
    }
}

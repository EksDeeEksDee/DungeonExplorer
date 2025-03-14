﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;


namespace DungeonExplorer
{
    public class Player
    {
        private string name;
        private int health;
        private List<string> inventory = new List<string>(); // List that hold the player's items.

        // Property used to get and set the player's name.
        public string Name
        {
            get { return name; }
            set
            {
                while (string.IsNullOrEmpty(value)) // Checks to see if the name is empty, if so the user is asked to enter the name again.
                {
                    Console.WriteLine("Player name can't be empty.");
                    Console.Write("Enter the player name: ");
                    value = Console.ReadLine();
                }
                name = value; // Sets the name.
                Console.WriteLine($"That's it! {new_name} was your name!");
                Console.WriteLine("After getting your strength back, you stand up wondering where you are, not knowing what awaits you next.");
                Console.WriteLine("=============================================================================================================");
            }
        }
        // Property used to set the player's health.
        public int Health
        {
            get { return health; }
            set
            {
                if (value <= 0) // Checks if the health is 0 or negative.
                {
                    Console.WriteLine("Player health cannot be set to zero or be negative.");

                    Console.WriteLine("Player health will be set to 100.");
                    health = 100; // Sets the health to 100 as a default value.

                }
                else
                {
                    health = value; // Sets the health if it isn't 0 or negative.
                }
            }
        }


        // Method which allows the player to pick up items from a room.
        public void PickUpItem(string item, Room currentRoom)
        {

            // Gets the list of items that are in the room.
            List<string> roomItems = currentRoom.GetRoomItems();
            // Finds the specified item within the room (case sensitive).
            string itemToPickUp = roomItems.Find(r => r.Equals(item, StringComparison.OrdinalIgnoreCase));

            if (itemToPickUp != null) // Checks if the item is actually in the room.
            {
                if (!inventory.Contains(itemToPickUp)) // Checks if player already has the item.
                {
                    inventory.Add(itemToPickUp); // Adds the item to the player's inventory.
                    currentRoom.RemoveItem(itemToPickUp); // Removes the item from the room.
                    Console.WriteLine("Picked up: " + itemToPickUp); // Displays pick up message.
                }
                else
                {
                    Console.WriteLine(itemToPickUp + " already in inventory!"); // If player already has this item.
                }
            }
            else
            {
                Console.WriteLine($"{item} not found."); // If the item isn't in the room.
            }
        }

        // Method for player attacking.
        public void Attack(Enemy target, string weapon)
        {
            int damage = 0;
            if (inventory.Any(item => item.Equals(weapon, StringComparison.OrdinalIgnoreCase)))
            {
                if (weapon.ToLower() == "sword") // If player has a sword they do 10 damage, if they don't then they do 5.
                {
                    Console.WriteLine("You used a sword!");
                    damage = 10;
                }
                else if (weapon.ToLower() == "healing potion")
                {
                    Console.WriteLine("You used a .... healing potion?");
                    RemoveItem("Healing Potion");
                    damage = -10;
                }
            }
            else
            {
                Console.WriteLine("You used your fist!");
                damage = 5;
            }

            target.Health -= damage;

            if (target.Health <= 0)
            {
                target.Health = 0;
                Console.WriteLine($"{target.Name} defeated!"); // If enemy health is 0 then the defeated message is displayed.
            }
            else
            {
                if (damage > 0)
                {
                    Console.WriteLine($"You attacked {target.Name} for {damage} damage!"); // Attack enemy message.
                }
                else
                {
                    Console.WriteLine($"You healed {target.Name} for {-damage} health!"); // Heal enemy message.
                }
                Console.WriteLine($"{target.Name} remaining health: {target.Health}");
            }

        }


        // Method for using items.
        public void UseItem(string item)
        {
            if (item.ToLower() == "healing potion" && inventory.Contains(item))
            {
                health += 10;
                Console.WriteLine($"Used {item} and healed for 10 health!");
                RemoveItem("Healing Potion");
            }
            else if (!inventory.Contains(item))
            {
                Console.WriteLine("You don't have that item.");
            }
            else
            {
                Console.WriteLine("Can't use this item.");
            }
        }

        // Method used to remove items.
        public void RemoveItem(string item)
        {
            foreach (var i in inventory)
            {
                if (item == i && inventory.Count > 0)
                {
                    inventory.Remove(i);
                }
                else if (inventory.Count == 0)
                {
                    Console.WriteLine("No items in inventory.");
                }
                else
                {
                    Console.WriteLine("You do not have that item.");
                }
            }
        }

        // Method which returns the contents of the player's inventory.
        public string InventoryContents()
        {
            if (inventory.Count == 0)
            {
                return ("Inventory is empty.");
            }
            else
            {
                return string.Join(", ", inventory);
            }
        }
    }
}

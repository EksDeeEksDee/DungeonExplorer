using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    public class Room
    {
        private string description; // Description of the room.  
        public string Name { get; set; }

        // Property used to get and set the room's description.
        public string Description
        {
            get { return description; }
            set
            {
                if (string.IsNullOrEmpty(value)) // Checks if the description is empty.
                {
                    Console.WriteLine("Room description cannot be empty.");
                }
                else
                {
                    description = value; // Sets the description.
                }
            }
        }

        private List<Item> roomItems = new List<Item>(); // List of items within the room.
        private List<string> roomPaths = new List<string>(); // List of paths to other rooms.
        private List<Monster> roomMonsters = new List<Monster>(); // List of monsters in the room.

        public Room(string name, string description)
        {
            Name = name;
            Description = description;
        }

        // Method used to add paths to the room.
        public void AddPath(string path)
        {
            if (!roomPaths.Contains(path))
            {
                roomPaths.Add(path);
            }
        }

        // Method used to add items to the room.
        public void AddItem(Item item)
        {
            if (!roomItems.Contains(item))
            {
                roomItems.Add(item);
            }
        }

        // Method used to remove items from the room.
        public void RemoveItem(Item item)
        {
            if (roomItems.Contains(item))
            {
                roomItems.Remove(item);
            }
        }

        // Method that returns the items that are in the room.
        public List<Item> GetRoomItems()
        {
            return roomItems; // Return list of items in the room.
        }

        // Method that returns all the paths to other rooms.
        public List<string> GetRoomPaths()
        {
            return roomPaths;
        }

        // Add a monster to the room.
        public void AddMonster(Monster monster)
        {
            roomMonsters.Add(monster);
        }

        // Get the monsters in the room.
        public List<Monster> GetMonsters()
        {
            return roomMonsters;
        }
        public void RemoveMonster(Monster monster)
        {
            if (roomMonsters.Contains(monster))
            {
                roomMonsters.Remove(monster);
            }
        }

        // Method that returns the description of the room along with what items are inside of it.
        public void GetDescription()
        {
            Console.WriteLine(description);
            if (roomMonsters.Count > 0)
            {
                Console.WriteLine("Monsters present: " + string.Join(", ", roomMonsters.Select(m => m.Name)));
            }
            Console.WriteLine("Following paths in the room: " + string.Join(", ", roomPaths));
        }
    }
}

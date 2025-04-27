using System;


namespace DungeonExplorer
{
    public class Item : ICollectible
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // Constructor for items.
        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }
        // Method for using items.
        public virtual void Use(Creature target)
        {
            Console.WriteLine($"{Name} has no special use.");
        }
        // Metho for displaying information about item such as name and description.
        public void ShowItemInfo()
        {
            Console.WriteLine($"Item: {Name}\nDescription: {Description}");
        }
    }
}

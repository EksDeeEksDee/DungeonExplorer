using System;


namespace DungeonExplorer
{
    public class Item : ICollectible
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public virtual void Use(Creature target)
        {
            Console.WriteLine($"{Name} has no special use.");
        }

        public void ShowItemInfo()
        {
            Console.WriteLine($"Item: {Name}\nDescription: {Description}");
        }
    }
}

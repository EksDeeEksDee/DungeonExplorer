using System;

namespace DungeonExplorer
{
    public class Key : Item
    {
        public string KeyId { get; set; }  // Unique identifier for matching with doors

        public Key(string name, string description, string keyId)
            : base(name, description)
        {
            KeyId = keyId;
        }

        public override void Use(Creature target)
        {
            Console.WriteLine($"{target.Name} uses the {Name} (Key ID: {KeyId}).");
            // Later: Check if there's a door in the room with matching ID
        }
    }
}

using System;

namespace DungeonExplorer
{
    public class Key : Item
    {

        public Key(string name, string description)
            : base(name, description) { }

        public override void Use(Creature target)
        {
            Console.WriteLine("Keys don't need to be used.");
        }
    }
}

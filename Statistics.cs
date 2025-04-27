using System;
using System.IO;

namespace DungeonExplorer
{
    public class Statistics
    {

        // Gettets and setters for different attributes.
        public int EnemiesDefeated { get; private set; }
        public int TotalDamageTaken { get; private set; }
        public int PotionsUsed { get; private set; }
        public int ItemsPickedUp { get; private set; }
        public int RoomsVisited { get; private set; }
        
        public void EnemyDefeated()
        {
            EnemiesDefeated++;
        }

        public void PotionUsed()
        {
            PotionsUsed++;
        }

        public void ItemPickedUp()
        {
            ItemsPickedUp++;
        }

        public void RoomVisited()
        {
            RoomsVisited++;
        }
        // Method used to display stats.
        public void DisplayStats()
        {
            Console.WriteLine("===== Player Stats =====");
            Console.WriteLine($"Enemies Defeated: {EnemiesDefeated}");
            Console.WriteLine($"Damage Taken: {TotalDamageTaken}");
            Console.WriteLine($"Potions Used: {PotionsUsed}");
            Console.WriteLine($"Items Picked Up: {ItemsPickedUp}");
            Console.WriteLine($"Rooms Visited: {RoomsVisited}");
            Console.WriteLine("=============================");
        }
        // Method used to save stats to a file this is for saving the game.
        public void SaveToFile(string path = "PlayerStats.txt")
        {
            File.WriteAllLines(path, new[]
            {
                "===== Player Statistics =====",
                $"Enemies Defeated: {EnemiesDefeated}",
                $"Damage Taken: {TotalDamageTaken}",
                $"Potions Used: {PotionsUsed}",
                $"Items Picked Up: {ItemsPickedUp}",
                $"Rooms Visited: {RoomsVisited}",
                "============================="
            });
        }
        // Method used to load the stats.
        public void LoadStats(int enemiesDefeated, int totaldamageTaken, int potionsUsed, int itemsPickedUp, int roomsVisited)
        {
            EnemiesDefeated = enemiesDefeated;
            TotalDamageTaken = totaldamageTaken;
            PotionsUsed = potionsUsed;
            ItemsPickedUp = itemsPickedUp;
            RoomsVisited = roomsVisited;
        }
    }
}

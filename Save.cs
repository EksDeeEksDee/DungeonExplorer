using System;
using System.IO;

namespace DungeonExplorer
{
    public class Save
    {
        private const string SaveFilePath = "savegame.txt";

        public static void SaveGame(Player player, string currentRoomName)
        {
            using (StreamWriter writer = new StreamWriter(SaveFilePath))
            {
                writer.WriteLine(player.Name);
                writer.WriteLine(player.Health);
                writer.WriteLine(player.Experience);
                writer.WriteLine(player.Level);
                writer.WriteLine(currentRoomName);
                writer.WriteLine(string.Join("|", player.GetInventoryNames()));

                // Save statistics
                writer.WriteLine(player.Stats.EnemiesDefeated);
                writer.WriteLine(player.Stats.TotalDamageTaken);
                writer.WriteLine(player.Stats.PotionsUsed);
                writer.WriteLine(player.Stats.ItemsPickedUp);
                writer.WriteLine(player.Stats.RoomsVisited);
            }

            Console.WriteLine("Game saved successfully!");
        }

        public static (Player, string) LoadGame()
        {
            if (!File.Exists(SaveFilePath))
            {
                Console.WriteLine("No save file found.");
                return (null, null);
            }

            using (StreamReader reader = new StreamReader(SaveFilePath))
            {
                string name = reader.ReadLine();
                int health = int.Parse(reader.ReadLine());
                int experience = int.Parse(reader.ReadLine());
                int level = int.Parse(reader.ReadLine());
                string currentRoomName = reader.ReadLine();
                string inventoryLine = reader.ReadLine();

                Player player = new Player(name, health, experience)
                {
                    Level = level
                };

                if (!string.IsNullOrEmpty(inventoryLine))
                {
                    string[] itemNames = inventoryLine.Split('|');
                    foreach (var itemName in itemNames)
                    {
                        player.AddItemByName(itemName);
                    }
                }

                // Load statistics
                int enemiesDefeated = int.Parse(reader.ReadLine());
                int totalDamageDealt = int.Parse(reader.ReadLine());
                int potionsUsed = int.Parse(reader.ReadLine());
                int itemsPickedUp = int.Parse(reader.ReadLine());
                int roomsVisited = int.Parse(reader.ReadLine());

                player.Stats.LoadStats(enemiesDefeated, totalDamageDealt, potionsUsed, itemsPickedUp, roomsVisited);

                Console.WriteLine("Game loaded successfully!");
                return (player, currentRoomName);
            }
        }
    }
}

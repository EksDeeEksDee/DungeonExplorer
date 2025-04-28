using System;
using System.IO;

namespace DungeonExplorer
{
    public class Save
    {
        // The file where all save data will be stored
        private const string SaveFilePath = "savegame.txt";

        // Save the current game state (player and room name) to a file
        public static void SaveGame(Player player, string currentRoomName)
        {
            using (StreamWriter writer = new StreamWriter(SaveFilePath))
            {
                // Write basic player info
                writer.WriteLine(player.Name);        // Player name
                writer.WriteLine(player.Health);      // Player health
                writer.WriteLine(player.Experience);  // Player experience points
                writer.WriteLine(player.Level);       // Player level
                writer.WriteLine(currentRoomName);    // Name of the current room the player is in

                // Write inventory items (separated by "|")
                writer.WriteLine(string.Join("|", player.GetInventoryNames()));

                // Write player statistics
                writer.WriteLine(player.Stats.EnemiesDefeated);
                writer.WriteLine(player.Stats.TotalDamageTaken);
                writer.WriteLine(player.Stats.PotionsUsed);
                writer.WriteLine(player.Stats.ItemsPickedUp);
                writer.WriteLine(player.Stats.RoomsVisited);
            }

            Console.WriteLine("Game saved successfully!");
        }

        // Load a saved game state from the file
        public static (Player, string) LoadGame()
        {
            if (!File.Exists(SaveFilePath))
            {
                Console.WriteLine("No save file found.");
                return (null, null); // No save found
            }

            using (StreamReader reader = new StreamReader(SaveFilePath))
            {
                // Read and rebuild player basic info
                string name = reader.ReadLine();
                int health = int.Parse(reader.ReadLine());
                int experience = int.Parse(reader.ReadLine());
                int level = int.Parse(reader.ReadLine());
                string currentRoomName = reader.ReadLine();
                string inventoryLine = reader.ReadLine();

                // Create a new Player object with read values
                Player player = new Player(name, health, experience)
                {
                    Level = level
                };

                // Rebuild inventory if there are saved items
                if (!string.IsNullOrEmpty(inventoryLine))
                {
                    string[] itemNames = inventoryLine.Split('|');
                    foreach (var itemName in itemNames)
                    {
                        player.AddItemByName(itemName);
                    }
                }

                // Read and rebuild player statistics
                int enemiesDefeated = int.Parse(reader.ReadLine());
                int totalDamageTaken = int.Parse(reader.ReadLine());
                int potionsUsed = int.Parse(reader.ReadLine());
                int itemsPickedUp = int.Parse(reader.ReadLine());
                int roomsVisited = int.Parse(reader.ReadLine());

                // Apply loaded stats to the player
                player.Stats.LoadStats(enemiesDefeated, totalDamageTaken, potionsUsed, itemsPickedUp, roomsVisited);

                Console.WriteLine("Game loaded successfully!");
                return (player, currentRoomName); // Return the loaded player and room name
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace DungeonExplorer
{
    public class Testing
    {

        // Initialize testing objects.
        private Room testRoom = new Room("TestRoom", "Room used for testing.");
        private Room secondTestRoom = new Room("SecondTestRoom", "Another test room.");
        private Spider testEnemy = new Spider();
        private Player testPlayer = new Player("Tester", 100, 0);
        private MapManager testMap = new MapManager();

        private string logPath = "TestResults.txt"; // Saves to this file.
        private List<string> logLines = new List<string>();

        public void RunAllTests()
        {
            logLines.Add("=====Running Unit Tests=====");
            try
            {
                RoomTest();
                EnemyTest();
                InventoryTest();
                CombatTest();
                XPTest();
                MovingRoomsTest();
            }
            catch (Exception ex)
            {
                logLines.Add("Exception during tests: " + ex.Message);
            }

            logLines.Add("=====Tests Complete=====");
            File.WriteAllLines(logPath, logLines);
            Console.WriteLine($"Test results saved to {logPath}");
        }
        // Method for testing the rooms.
        private void RoomTest()
        {
            Debug.Assert(!string.IsNullOrEmpty(testRoom.Description), "Room description cannot be empty.");
            logLines.Add("RoomTest passed.");
        }
        // Method for testing enemies.
        private void EnemyTest()
        {
            testEnemy.Health = 50;
            testEnemy.Name = "Enemy";
            Debug.Assert(testEnemy.Health > 0, "Enemy health should be positive.");
            Debug.Assert(!string.IsNullOrEmpty(testEnemy.Name), "Enemy name cannot be empty.");
            logLines.Add("EnemyTest passed.");
        }
        // Method for testing inventory.
        private void InventoryTest()
        {
            var sword = new Sword("Test Sword", "A testing weapon.", 25);
            testRoom.AddItem(sword);
            testPlayer.PickUpItem("Test Sword", testRoom);
            Debug.Assert(testPlayer.InventoryContains("Test Sword"), "Player should have picked up the sword.");
            logLines.Add("InventoryTest passed.");
        }
        // Method for testing combat.
        private void CombatTest()
        {
            int initialEnemyHealth = testEnemy.Health;
            testPlayer.Attack(testEnemy);
            Debug.Assert(testEnemy.Health < initialEnemyHealth, "Enemy should take damage after attack.");
            logLines.Add("CombatTest passed.");
        }
        // Method for testing experience gain.
        private void XPTest()
        {
            int initialLevel = testPlayer.Level;
            testPlayer.GainExperience(150);
            Debug.Assert(testPlayer.Level > initialLevel, "Player should level up after enough XP.");
            logLines.Add("XPTest passed.");
        }
        // Metho for testing movement between rooms.
        private void MovingRoomsTest()
        {
            testRoom.AddPath("SecondTestRoom");
            testMap.AddRoom(testRoom);
            testMap.AddRoom(secondTestRoom);
            testMap.SetStartingRoom("TestRoom");

            bool movedSuccessfully = testMap.MoveToRoom("SecondTestRoom");
            Debug.Assert(movedSuccessfully, "Should be able to move to SecondTestRoom.");
            Debug.Assert(testMap.CurrentRoom.Name == "SecondTestRoom", "Current room should now be SecondTestRoom.");

            // Try moving back to TestRoom (but no path added back)
            bool moveBack = testMap.MoveToRoom("TestRoom");
            Debug.Assert(!moveBack, "Should NOT be able to move back to TestRoom (no reverse path).");

            logLines.Add("MovingRoomsTest passed (forward and blocked movement checked).");
        }
    }
}

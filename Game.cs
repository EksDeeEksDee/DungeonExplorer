using System;
using System.IO;
using System.Linq;

namespace DungeonExplorer
{
    internal class Game
    {
        private Player player;
        private MapManager map = new MapManager();

        public Game()
        {
            Room room1 = new Room("Room1", File.ReadAllText(@"Descriptions/room1.txt"));
            Room room2 = new Room("Room2", File.ReadAllText(@"Descriptions/room2.txt"));
            Room room3 = new Room("Room3", File.ReadAllText(@"Descriptions/room3.txt"));
            Room room4 = new Room("Room4", File.ReadAllText(@"Descriptions/room4.txt"));
            Room room5 = new Room("Room5", File.ReadAllText(@"Descriptions/room5.txt"));
            Room room6 = new Room("Room6", File.ReadAllText(@"Descriptions/room6.txt"));
            Room room7 = new Room("Room7", File.ReadAllText(@"Descriptions/room7.txt"));

            room1.AddItem(new Key("Rusty Key", "An old, corroded key.", "rusty_001"));
            room1.AddPath("Room2");

            Spider spider = new Spider();
            Goblin goblin = new Goblin();
            GoblinWarrior goblinWarrior = new GoblinWarrior();
            GoblinChief goblinChief = new GoblinChief();
            StoneKnight stoneKnight = new StoneKnight();
            Dragon dragon = new Dragon();
            new Sword("Goblin Slayer Sword", "A sword made to destroy goblins.", 10);
            new Sword("Stone Cutter Sword", "A sword sharp enough to cut through stone.", 20);

            room2.AddItem(new Sword("Sword", "A basic sword.", 15));
            room2.AddMonster(spider);
            room2.AddPath("Room1");
            room2.AddPath("Room3");

            room3.AddItem(new HealingPotion());
            room3.AddPath("Room2");
            room3.AddPath("Room4");
            room3.AddMonster(goblin);

            room4.AddItem(new StrengthPotion());
            room4.AddMonster(goblinWarrior);
            room4.AddMonster(goblinChief);
            room4.AddPath("Room3");
            room4.AddPath("Room5");
            room4.AddPath("Room6");

            room5.AddItem(new HealingPotion());
            room5.AddItem(new HealingPotion());
            room5.AddItem(new StrengthPotion());
            room5.AddMonster(stoneKnight);
            room5.AddPath("Room4");

            room6.AddPath("Room4");
            room6.AddPath("Room7");

            room7.AddMonster(dragon);


            map.AddRoom(room1);
            map.AddRoom(room2);
            map.AddRoom(room3);
            map.AddRoom(room4);
            map.AddRoom(room5);
            map.AddRoom(room6);
            map.AddRoom(room7);
            map.SetStartingRoom("Room1");
            Console.WriteLine("This is a text-based puzzle dungeon crawler game. You will be able to fight enemies and pick up items which are hinted at in room descriptions.");
            Console.WriteLine("Good Luck & Have Fun!");

            string choice = "";
            while (choice != "new" && choice != "load")
            {
                Console.Write("Type 'new' for a new game or 'load' to load a save: ");
                choice = Console.ReadLine().Trim().ToLower();

                if (choice != "new" && choice != "load")
                {
                    Console.WriteLine("Invalid input. Please type 'new' or 'load'.");
                }
            }
            if (choice == "load")
            {
                var (loadedPlayer, loadedRoomName) = Save.LoadGame();
                if (loadedPlayer != null)
                {
                    Console.Clear();
                    player = loadedPlayer;
                    map.SetStartingRoom(loadedRoomName);
                    return;
                }
            }
            else
            {
                Console.WriteLine("Failed to load. Starting a new game instead.");
            }
                Console.WriteLine("You wake up after a long sleep, you can't remember anything except your name which is...");
            Console.Write("Enter your name: ");
            string new_name = Console.ReadLine();

            if (new_name.Equals("RunTests", StringComparison.OrdinalIgnoreCase))
            {
                Testing tests = new Testing();
                tests.RunAllTests();
                Console.WriteLine("Tests completed. Check TestResults.txt.");
                Environment.Exit(0); // Exit without starting the game
            }
            Console.WriteLine($"That's it! {new_name} was your name!");
            Console.WriteLine("You stand up not sure of where you are or what's to come.");
            player = new Player(new_name, 100, 0);

        }

        public void Start()
        {
            bool description_checked = false;
            bool playing = true;

            while (playing)
            {
                Console.WriteLine("Type help for a list of commands");
                Console.Write("Enter command: ");
                string user_input = Console.ReadLine().ToLower();
                Console.WriteLine("===============================================");

                if (user_input == "help")
                {
                    ShowHelp();
                }
                else if (user_input == "status")
                {
                    ShowStatus();
                }
                else if (user_input == "description")
                {
                    ShowRoomDescription();
                    description_checked = true;
                }
                else if (user_input.StartsWith("pick up") && description_checked)
                {
                    HandlePickUp(user_input);
                }
                else if (user_input.StartsWith("go to") && description_checked)
                {
                    HandleGoTo(user_input);
                }
                else if (user_input.StartsWith("use"))
                {
                    HandleUse(user_input);
                }
                else if (user_input == "attack")
                {
                    HandleCombatTurn();
                }
                else if (user_input == "save")
                {
                    Save.SaveGame(player, map.CurrentRoom.Name);
                }
                else if (user_input == "quit")
                {
                    Console.WriteLine("Exiting....");
                    playing = false;
                }
                else Console.WriteLine("Invalid command!");
            }
        }

        private void ShowHelp()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("Status - displays the player's name, health and inventory.");
            Console.WriteLine("Description - displays the description of the room.");
            Console.WriteLine("Pick up [item name] - picks up an item from the room.");
            Console.WriteLine("Go to [room name] - goes to specified room.");
            Console.WriteLine("Use [item name] - uses the item if it can be used.");
            Console.WriteLine("Attack - puts you into combat against enemies (if there are any).");
            Console.WriteLine("Save - saves your current game progress.");
            Console.WriteLine("Quit - exits the game.");
            Console.WriteLine("=========================================================================================");
        }

        private void ShowStatus()
        {
            Console.WriteLine($"Name: {player.Name}");
            Console.WriteLine($"Health: {player.Health}");
            Console.WriteLine($"Level: {player.Level}");
            Console.WriteLine($"Experience: {player.Experience}");
            if (player.GetInventoryItems().Count > 0)
            {
                Console.WriteLine("Inventory:");
                foreach (var item in player.GetInventoryItems())
                {
                    item.ShowItemInfo();
                }
            }
            else
            {
                Console.WriteLine("Inventory is empty.");
            }


            if (player.GetInventoryItems().Count >= 2)
            {
                Console.Write("Sort inventory? (name/type/damage/none): ");
                string sortOption = Console.ReadLine().Trim().ToLower();

                switch (sortOption)
                {
                    case "name":
                        player.SortInventoryByName();
                        break;
                    case "type":
                        player.SortInventoryByType();
                        break;
                    case "damage":
                        player.SortInventoryByDamage();
                        break;
                    default:
                        Console.WriteLine("No sorting applied.");
                        break;
                }
                Console.WriteLine("Sorted Inventory:");
                foreach (var item in player.GetInventoryItems())
                {
                    Console.WriteLine($"- {item.Name}: {item.Description}");
                }
            }
            else
            {
                Console.WriteLine("(Not enough items to sort.)");
            }

            Console.WriteLine("================================================");
        }


        private void ShowRoomDescription()
        {
            map.CurrentRoom.GetDescription();
            Console.WriteLine("====================================================");
        }

        private void HandlePickUp(string user_input)
        {
            string itemName = user_input.Substring(8).Trim();
            if (!string.IsNullOrEmpty(itemName))
            {
                // Room 5 has a hidden enemy that only appears if you try to pick up an item, in other rooms you can pick up items without attacking enemies, but you get a lower score and no experience.
                if (map.CurrentRoom.Name == "Room5")
                {
                    var stoneKnight = map.CurrentRoom.GetMonsters().FirstOrDefault(m => m.Name == "Stone Knight");

                    if (stoneKnight == null) // Knight not spawned.
                    {
                        Console.WriteLine("As you reach for the treasure, the stone statue rumbles to life!");
                        map.CurrentRoom.AddMonster(new StoneKnight()); // Spawns the Knight.
                        return;
                    }
                    else if (map.CurrentRoom.GetMonsters().Any(m => m.Name == "Stone Knight"))
                    {
                        Console.WriteLine("The Stone Knight blocks your path! Defeat him first!");
                        return;
                    }
                }

                // Regular pick up if not in room 5.
                player.PickUpItem(itemName, map.CurrentRoom);
            }
            else
            {
                Console.WriteLine("No item provided. Please specify which item you want to pick up.");
            }
        }

        private void HandleGoTo(string user_input)
        {
            string roomName = user_input.Substring(6).Trim();
            // In room 6, to access room 7 you need to solve a riddle
            if (map.CurrentRoom.Name == "Room6" && roomName.Equals("Room7", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("The door is sealed by a riddle. Solve it to pass.");

                Console.WriteLine("\"I speak without a mouth and hear without ears. I have no body, but I come alive with the wind. What am I?\"");
                Console.Write("Your answer: ");
                string answer = Console.ReadLine().Trim().ToLower();

                if (answer == "echo")
                {
                    Console.WriteLine("The door rumbles and opens. You may proceed!");
                    map.MoveToRoom(roomName);
                    Console.WriteLine($"You have entered {roomName}.");
                    player.Stats.RoomVisited();
                }
                else
                {
                    Console.WriteLine("The runes glow red. The door remains shut. You feel a sharp pain in your mind (-10 HP).");
                    player.TakeDamage(10);
                }
                return;
            }

            // Existing movement logic for other rooms
            if (map.MoveToRoom(roomName))
            {
                Console.WriteLine($"You have entered {roomName}.");
                player.Stats.RoomVisited();
            }
            else
            {
                Console.WriteLine("No direct path or room doesn't exist.");
            }
        }

        private void HandleUse(string user_input)
        {
            string itemName = user_input.Substring(3).Trim();
            player.UseItem(itemName);
            Console.WriteLine("==========================================");
        }

        private void HandleCombatTurn()
        {
            if (map.CurrentRoom.GetMonsters().Count == 0)
            {
                Console.WriteLine("There are no enemies in the room.");
                return;
            }

            // Loop until all monsters are dead or player dies
            while (map.CurrentRoom.GetMonsters().Any() && player.Health > 0)
            {
                Console.WriteLine("Enemies in the room:");
                foreach (var m in map.CurrentRoom.GetMonsters())
                {
                    Console.WriteLine($"- {m.Name} (HP: {m.Health})");
                }

                string usePotion;
                do
                {
                    Console.Write("Do you want to use a potion first? (Y/N): ");
                    usePotion = Console.ReadLine().Trim().ToLower();
                } while (usePotion != "y" && usePotion != "n");

                if (usePotion == "y")
                {
                    var potions = player.InventoryContents().OfType<Potion>().ToList();
                    if (potions.Count == 0)
                    {
                        Console.WriteLine("You have no potions.");
                    }
                    else
                    {
                        Console.WriteLine("Available potions: " + string.Join(", ", potions.Select(p => p.Name)));
                        Console.Write("Enter the potion name: ");
                        string potionName = Console.ReadLine();
                        player.UseItem(potionName);
                    }
                }

                // Player selects a monster to attack
                Console.Write("Which enemy do you want to attack?: ");
                string targetName = Console.ReadLine();
                var target = map.CurrentRoom.GetMonsters().FirstOrDefault(m => m.Name.Equals(targetName, StringComparison.OrdinalIgnoreCase));

                if (target == null)
                {
                    Console.WriteLine("No such enemy in the room.");
                }
                else
                {
                    player.Attack(target);
                    if (target.Health <= 0)
                    {
                        if (target is Dragon)
                        {
                            Console.WriteLine("Congratulations! You have defeated the Ancient Dragon and completed the dungeon!");
                            player.Stats.SaveToFile(); // Save final stats
                            Environment.Exit(0);
                        }
                        else if (target is Goblin)
                        {
                            Console.WriteLine("You found a Goblin Slayer Sword!");
                            player.PickUpItem("Goblin Slayer Sword", map.CurrentRoom);
                        }
                        else if (target is GoblinChief)
                        {
                            Console.WriteLine("You found a Magic Sword!");
                            player.PickUpItem("Magic Sword", map.CurrentRoom);
                        }
                        else if (map.CurrentRoom.Name == "Room5" && target is StoneKnight)
                        {
                            Console.WriteLine("The room falls silent. You may now collect the treasures.");

                            var items = map.CurrentRoom.GetRoomItems().ToList();
                            foreach (var item in items)
                            {
                                player.PickUpItem(item.Name, map.CurrentRoom);
                            }
                        }
                        player.GainExperience(target.XPReward);
                        map.CurrentRoom.RemoveMonster(target);
                    }

                    // Remaining monsters attack the player
                    foreach (var monster in map.CurrentRoom.GetMonsters())
                    {
                        monster.Attack(player);

                    }

                    Console.WriteLine($"Player HP: {player.Health}");
                }

                if (player.Health <= 0)
                {
                    Console.WriteLine("You have fallen in battle. Game Over.");
                    Environment.Exit(0);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    public class MapManager
    {
        private Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        public Room CurrentRoom { get; private set; }

        // Add a room to the map
        public void AddRoom(Room room)
        {
            var lowerName = room.Name.ToLower();
            if (!rooms.ContainsKey(lowerName))
            {
                rooms.Add(lowerName, room);
            }
        }

        // Set the starting room
        public void SetStartingRoom(string roomName)
        {
            roomName = roomName.ToLower();
            if (rooms.ContainsKey(roomName))
            {
                CurrentRoom = rooms[roomName];
            }
            else
            {
                Console.WriteLine($"Room '{roomName}' does not exist in the map.");
            }
        }

        // Try to move to another room by name
        public bool MoveToRoom(string roomName)
        {
            roomName = roomName.ToLower();
            if (rooms.TryGetValue(roomName, out Room targetRoom) &&
                CurrentRoom.GetRoomPaths().Any(path => path.Equals(roomName, StringComparison.OrdinalIgnoreCase)))
            {
                CurrentRoom = targetRoom;
                Console.Clear();
                return true;
            }
            return false;
        }


        // Get all rooms
        public List<Room> GetAllRooms()
        {
            return rooms.Values.ToList();
        }
    }
}

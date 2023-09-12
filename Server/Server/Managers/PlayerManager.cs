using DarkRift;
using DarkRift.Server;
using MUD;
using MUD.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MUD.Net;
using MUD.Ability;
using MUD.SQL;
using MUD.Combat;
using MUD.Characters;

namespace MUD.Managers
{
    /* 
     * Manager for Players, stores the players by their ID, in addition to their Player 
     * object and their IClient so that messages can be sent to them.
     */
    class PlayerManager
    {
        public static PlayerManager instance = new PlayerManager();

        private Dictionary<int, Tuple<IClient, Player>> players = new Dictionary<int, Tuple<IClient, Player>>();
        
        // Update the player token, and add the player from the database to the dictionary of logged in players.
        public void LoginPlayer(IClient client, string username, ushort id, string token)
        {
            Player player = new Player(username, id, Settings.activeSlots, Settings.passiveSlots, Settings.armorSlots);
            player.SetToken(token);
            Database.instance.UpdatePlayerFromDB(player);
            players[id] = new Tuple<IClient, Player>(client, player);
        }

        // Remove the player from memory.
        public void LogoutPlayer(int id)
        {
            players.Remove(id);
            // TODO: Update SQL Database
        }

        // Verify the player's ID corresponds with their Token
        public bool VerifyPlayer(int id, string token)
        {
            if (players.ContainsKey(id))
            {
                if (players[id].Item2.Token == token)
                {
                    return true;
                }
            }

            return false;
        }

        // Get the Player with the corresponding Client ID
        public Player GetPlayer(ushort id)
        {
            if (players.ContainsKey(id))
                return players[id].Item2;

            return null;
        }

        // For Calling Events On All Passives of Player
        public void ForEach(Player player, Action<Passive> action)
        {
            foreach (Passive passive in player.Passives)
            {
                if (passive != null)
                    action(passive);
            }
        }

        public void SendPlayerInfo(ushort id)
        {
            Player player = GetPlayer(id);
            PlayerManager.instance.SendToClient(
                id,
                Tags.Tags.PLAYER_UPDATE,
                player.Net());

            NetRoom room = Database.instance.GetRoomFromPlayer(player.Name);
            PlayerManager.instance.SendToClient(
                id,
                Tags.Tags.ROOM,
                room);

            NetArea area = Database.instance.GetAreaFromPlayer(player.Name);
            PlayerManager.instance.SendToClient(
                id,
                Tags.Tags.AREA,
                area);
        }

        // Send Message to Given Client ID
        public void SendToClient(ushort id, Tags.Tags t, IDarkRiftSerializable obj, SendMode mode = SendMode.Reliable)
        {
            using (Message m = Message.Create<IDarkRiftSerializable>((ushort)t, obj))
            {
                if (players.ContainsKey(id))
                {
                    players[id].Item1.SendMessage(m, mode);
                }
            }
        }

        // Send Message to Multiple Given Client IDs
        public void SendToClients(List<ushort> characters, Tags.Tags t, IDarkRiftSerializable obj, SendMode mode = SendMode.Reliable)
        {
            foreach (ushort id in characters)
                using (Message m = Message.Create<IDarkRiftSerializable>((ushort)t, obj))
                    if (players.ContainsKey(id))
                        players[id].Item1.SendMessage(m, mode);
        }

        // Send Message to All Clients
        public void SendToAll(Tags.Tags t, IDarkRiftSerializable obj, SendMode mode = SendMode.Reliable)
        {
            foreach (KeyValuePair<int, Tuple<IClient, Player>> connectedClient in players)
                using (Message m = Message.Create<IDarkRiftSerializable>((ushort)t, obj))
                    connectedClient.Value.Item1.SendMessage(m, mode);
        }

        // Send Message to All Clients Except Given Client ID
        public void SendToAllExcept(ushort id, Tags.Tags t, IDarkRiftSerializable obj, SendMode mode = SendMode.Reliable)
        {
            using (Message m = Message.Create<IDarkRiftSerializable>((ushort)t, obj))
                foreach (KeyValuePair<int, Tuple<IClient, Player>> connectedClient in players)
                    if (connectedClient.Key != id)
                        connectedClient.Value.Item1.SendMessage(m, mode);
        }

        // Handle a command typed into a client. 
        // Takes in the Player object and the NetCommand containing the actual 
        // command in plaintext.
        public Error HandleCommand(Player player, NetCommand command)
        {
            if (CommandSettings.leftCommands.Contains(command.command.ToLower().Trim()))
            {
                if (Database.instance.GetRoomFromPlayer(player.Name).Left == -1)
                    return Error.CreateError(ErrorType.DIRECTION_DOES_NOT_EXIST);

                if (player.InCombat)
                    return Error.CreateError(ErrorType.MOVE_IN_COMBAT);

                Database.instance.MovePlayerToRoom(player.Name, Database.instance.GetRoomFromPlayer(player.Name).Left);
                OnMoveToRoom(player);
            }

            else if (CommandSettings.rightCommands.Contains(command.command.ToLower().Trim()))
            {
                if (Database.instance.GetRoomFromPlayer(player.Name).Right == -1)
                    return Error.CreateError(ErrorType.DIRECTION_DOES_NOT_EXIST);

                if (player.InCombat)
                    return Error.CreateError(ErrorType.MOVE_IN_COMBAT);

                Database.instance.MovePlayerToRoom(player.Name, Database.instance.GetRoomFromPlayer(player.Name).Right);
                OnMoveToRoom(player);
            }

            else if (CommandSettings.upCommands.Contains(command.command.ToLower().Trim()))
            {
                if (Database.instance.GetRoomFromPlayer(player.Name).Up == -1)
                    return Error.CreateError(ErrorType.DIRECTION_DOES_NOT_EXIST);

                if (player.InCombat)
                    return Error.CreateError(ErrorType.MOVE_IN_COMBAT);

                Database.instance.MovePlayerToRoom(player.Name, Database.instance.GetRoomFromPlayer(player.Name).Up);
                OnMoveToRoom(player);
            }

            else if (CommandSettings.downCommands.Contains(command.command.ToLower().Trim()))
            {
                if (Database.instance.GetRoomFromPlayer(player.Name).Down == -1)
                    return Error.CreateError(ErrorType.DIRECTION_DOES_NOT_EXIST);

                if (player.InCombat)
                    return Error.CreateError(ErrorType.MOVE_IN_COMBAT);

                Database.instance.MovePlayerToRoom(player.Name, Database.instance.GetRoomFromPlayer(player.Name).Down);
                OnMoveToRoom(player);
            }

            return null;
        }

        public void OnMoveToRoom(Player player)
        {
            CombatInstanceEventArgs c = new CombatInstanceEventArgs();
            ForEach(player, (Passive p) => p.OnRoomEnter(c));

            NetRoom room = Database.instance.GetRoomFromPlayer(player.Name);

            List<Enemy> enemies = EnemyManager.instance.GetEnemies(room.ID);
            if (enemies != null && enemies.Count > 0)
            {
                CombatManager.instance.StartCombat(player, room.ID);
            }
        }
    }
}

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
using MUD.SQL;

namespace MUD.Managers
{
    /* 
     * Manager for Players, stores the players by their ID, in addition to their Player 
     * object and their IClient so that messages can be sent to them.
     */
    class PlayerManager
    {
        public static PlayerManager instance = new PlayerManager();

        private Dictionary<int, Tuple<IClient, NetPlayer>> players = new Dictionary<int, Tuple<IClient, NetPlayer>>();
        
        // Update the player token, and add the player from the database to the dictionary of logged in players.
        public void LoginPlayer(IClient client, string username, int id, string token)
        {
            NetPlayer player = NetPlayer.CreatePlayer(username);
            player.SetToken(token);
            Database.instance.UpdatePlayerFromDB(player);
            players[id] = new Tuple<IClient, NetPlayer>(client, player);
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
        public NetPlayer GetPlayer(int id)
        {
            if (players.ContainsKey(id))
                return players[id].Item2;

            return null;
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

        // Send Message to All Clients
        public void SendToAll(Tags.Tags t, IDarkRiftSerializable obj, SendMode mode = SendMode.Reliable)
        {
            foreach (KeyValuePair<int, Tuple<IClient, NetPlayer>> connectedClient in players)
            {
                using (Message m = Message.Create<IDarkRiftSerializable>((ushort)t, obj))
                {
                    connectedClient.Value.Item1.SendMessage(m, mode);
                }
            }
        }

        // Send Message to All Clients Except Given Client ID
        public void SendToAllExcept(ushort id, Tags.Tags t, IDarkRiftSerializable obj, SendMode mode = SendMode.Reliable)
        {
            using (Message m = Message.Create<IDarkRiftSerializable>((ushort)t, obj))
            {
                foreach (KeyValuePair<int, Tuple<IClient, NetPlayer>> connectedClient in players)
                {
                    if (connectedClient.Key != id)
                    {
                        connectedClient.Value.Item1.SendMessage(m, mode);
                    }
                }
            }
        }

        // Handle a command typed into a client. 
        // Takes in the Player object and the NetCommand containing the actual 
        // command in plaintext.
        public Error HandleCommand(NetPlayer player, NetCommand command)
        {
            if (CommandSettings.leftCommands.Contains(command.command.ToLower().Trim()))
            {
                if (Database.instance.GetRoomFromPlayer(player.Name).Left == -1)
                    return Error.CreateError(MUD.ErrorType.DIRECTION_DOES_NOT_EXIST);

                Database.instance.MovePlayerToRoom(player.Name, Database.instance.GetRoomFromPlayer(player.Name).Left);
            }

            else if (CommandSettings.rightCommands.Contains(command.command.ToLower().Trim()))
            {
                if (Database.instance.GetRoomFromPlayer(player.Name).Right == -1)
                    return Error.CreateError(MUD.ErrorType.DIRECTION_DOES_NOT_EXIST);

                Database.instance.MovePlayerToRoom(player.Name, Database.instance.GetRoomFromPlayer(player.Name).Right);
            }

            else if (CommandSettings.upCommands.Contains(command.command.ToLower().Trim()))
            {
                if (Database.instance.GetRoomFromPlayer(player.Name).Up == -1)
                    return Error.CreateError(MUD.ErrorType.DIRECTION_DOES_NOT_EXIST);

                Database.instance.MovePlayerToRoom(player.Name, Database.instance.GetRoomFromPlayer(player.Name).Up);
            }

            else if (CommandSettings.downCommands.Contains(command.command.ToLower().Trim()))
            {
                if (Database.instance.GetRoomFromPlayer(player.Name).Down == -1)
                    return Error.CreateError(MUD.ErrorType.DIRECTION_DOES_NOT_EXIST);

                Database.instance.MovePlayerToRoom(player.Name, Database.instance.GetRoomFromPlayer(player.Name).Down);
            }

            return null;
        }
    }
}

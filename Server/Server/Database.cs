using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using DarkRift.Server;
using System.Xml.Linq;
using System.Numerics;
using System.Data;
using System.Diagnostics;
using System.Threading.Channels;
using MUD.Net;
using MUD.Managers;
using MUD.Ability;
using MUD.Items;
using MUD.Characters;

namespace MUD.SQL
{
    public class Database
    {
        static DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static Database instance = new Database();
        private static String startingArea;
        private SQLiteConnection data;

        public Database()
        {
            // Get from Settings
            startingArea = Settings.startingArea;

            data = new SQLiteConnection("Data Source=data.db");

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();

                // Create All Tables
                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS actives (
                        activeID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        description TEXT NOT NULL,
                        cooldown INTEGER NOT NULL,
                        uses INTEGER NOT NULL
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS passives (
                        passiveID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        description TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS titles (
                        titleID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        title TEXT NOT NULL
                    );
                    INSERT OR REPLACE INTO titles(titleID, title) 
                        VALUES(0, $title);
                ";
                command.Parameters.AddWithValue("$title", "");
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS areas (
                        areaID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        areaName TEXT NOT NULL,
                        width INTEGER NOT NULL,
                        height INTEGER NOT NULL,
                        startingRoomID INTEGER,

                        FOREIGN KEY (startingRoomID) 
                            REFERENCES rooms(roomID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS rooms (
                        roomID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        roomName TEXT NOT NULL,
                        roomDesc TEXT NOT NULL,
                        areaID INTEGER NOT NULL,
                        roomIDUp INTEGER NOT NULL,
                        roomIDDown INTEGER NOT NULL,
                        roomIDLeft INTEGER NOT NULL,
                        roomIDRight INTEGER NOT NULL,
                        x INTEGER NOT NULL,
                        y INTEGER NOT NULL,

                        FOREIGN KEY (areaID) 
                            REFERENCES areas(areaID),
                        FOREIGN KEY (roomIDUp) 
                            REFERENCES rooms(roomID),
                        FOREIGN KEY (roomIDDown) 
                            REFERENCES rooms(roomID),
                        FOREIGN KEY (roomIDLeft) 
                            REFERENCES rooms(roomID),
                        FOREIGN KEY (roomIDRight) 
                            REFERENCES rooms(roomID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS roomModifers (
                        modiferID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        roomID INTEGER NOT NULL,
                        text TEXT NOT NULL,
                        playerID INTEGER NOT NULL,
                        activeID INTEGER,
                        expiration INTEGER NOT NULL,

                        FOREIGN KEY (roomID) 
                            REFERENCES rooms(roomID),
                        FOREIGN KEY (playerID) 
                            REFERENCES players(playerID),
                        FOREIGN KEY (activeID) 
                            REFERENCES actives(activeID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS items (
                        itemID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,

                        itemName TEXT NOT NULL,
                        itemDesc TEXT NOT NULL,
                        itemType INTEGER NOT NULL,

                        stackable BIT NOT NULL,

                        appliesPassiveID INTEGER DEFAULT NULL,
                        appliesActiveID INTEGER DEFAULT NULL,

                        healthIncrease INTEGER NOT NULL DEFAULT 0,

                        healthOnKill INTEGER NOT NULL DEFAULT 0,
                        lifesteal DOUBLE NOT NULL DEFAULT 0,
                        healthOnHit INTEGER NOT NULL DEFAULT 0,

                        critChance DOUBLE NOT NULL DEFAULT 0,
                        critDamage DOUBLE NOT NULL DEFAULT 0,

                        resist DOUBLE NOT NULL DEFAULT 0,
                        bleedResist DOUBLE NOT NULL DEFAULT 0,
                        posionResist DOUBLE NOT NULL DEFAULT 0,

                        thorns INTEGER NOT NULL DEFAULT 0,

                        baseDamage DOUBLE NOT NULL DEFAULT 0,
                        skillDamage DOUBLE NOT NULL DEFAULT 0,
                        allDamage DOUBLE NOT NULL DEFAULT 0,

                        poisonDamage DOUBLE NOT NULL DEFAULT 0,
                        bleedDamage DOUBLE NOT NULL DEFAULT 0,
                        markDamage DOUBLE NOT NULL DEFAULT 0,

                        whilePoisonDamage DOUBLE NOT NULL DEFAULT 0,
                        whileBleedDamage DOUBLE NOT NULL DEFAULT 0,
                        whileMarkDamage DOUBLE NOT NULL DEFAULT 0,

                        healingOutgoing DOUBLE NOT NULL DEFAULT 0,
                        healingIncoming DOUBLE NOT NULL DEFAULT 0,

                        damageWhileMaxHealth DOUBLE NOT NULL DEFAULT 0,
                        damageWhileLowHealth DOUBLE NOT NULL DEFAULT 0,
                    

                        FOREIGN KEY (appliesPassiveID) 
                            REFERENCES passives(passiveID),
                        FOREIGN KEY (appliesActiveID) 
                            REFERENCES actives(activeID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS enemyClasses (
                        enemyID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        description TEXT NOT NULL,
                        enterCombatText TEXT NOT NULL,
                        enterRoomText TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS enemyTemplates (
                        enemyTemplateID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        enemyClassID INTEGER NOT NULL,
                        roomID INTEGER NOT NULL,
                        count INTEGER NOT NULL,

                        FOREIGN KEY (roomID) 
                            REFERENCES rooms(roomID),

                        FOREIGN KEY (enemyClassID) 
                            REFERENCES enemyClasses(enemyID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS enemyDrops (
                        enemyDropID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        enemyClassID INTEGER NOT NULL,
                        passiveID INTEGER DEFAULT NULL,
                        activeID INTEGER DEFAULT NULL,
                        itemID INTEGER DEFAULT NULL,
                        chance DOUBLE NOT NULL DEFAULT 1.0,

                        FOREIGN KEY (passiveID) 
                            REFERENCES passives(passiveID),

                        FOREIGN KEY (activeID) 
                            REFERENCES actives(activeID),

                        FOREIGN KEY (itemID) 
                            REFERENCES items(itemID),

                        FOREIGN KEY (enemyClassID) 
                            REFERENCES enemyClasses(enemyID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS players (
                        playerID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL,
                        titleID INTEGER NOT NULL,
                        areaID INTEGER NOT NULL,
                        roomID INTEGER NOT NULL,
                        passiveSlots INTEGER NOT NULL DEFAULT 5,
                        activeSlots INTEGER NOT NULL DEFAULT 5,

                        FOREIGN KEY (titleID) 
                            REFERENCES titles(titleID),

                        FOREIGN KEY (areaID) 
                            REFERENCES areas(areaID),

                        FOREIGN KEY (roomID) 
                            REFERENCES rooms(roomID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS playerActives (
                        playerID INTEGER NOT NULL,
                        activeID INTEGER NOT NULL,
                        activeSlot INTEGER NOT NULL,

                        FOREIGN KEY (playerID) 
                            REFERENCES players(playerID),

                        FOREIGN KEY (activeID) 
                            REFERENCES actives(activeID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS playerPassives (
                        playerID INTEGER NOT NULL,
                        passiveID INTEGER NOT NULL,
                        passiveSlot INTEGER NOT NULL,

                        FOREIGN KEY (playerID) 
                            REFERENCES players(playerID),

                        FOREIGN KEY (passiveID) 
                            REFERENCES passives(passiveID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS playerItems (
                        playerID INTEGER NOT NULL,
                        itemID INTEGER NOT NULL,
                        quantity INTEGER NOT NULL,

                        FOREIGN KEY (playerID) 
                            REFERENCES players(playerID),

                        FOREIGN KEY (itemID) 
                            REFERENCES items(itemID)
                    );
                ";
                command.ExecuteNonQuery();

                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS enemyActives (
                        enemyID INTEGER NOT NULL,
                        activeID INTEGER NOT NULL,
                        activeSlot INTEGER NOT NULL,

                        FOREIGN KEY (enemyID) 
                            REFERENCES enemyClasses(enemyID),

                        FOREIGN KEY (activeID) 
                            REFERENCES actives(activeID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS enemyPassives (
                        enemyID INTEGER NOT NULL,
                        passiveID INTEGER NOT NULL,
                        passiveSlot INTEGER NOT NULL,

                        FOREIGN KEY (enemyID) 
                            REFERENCES enemyClasses(enemyID),

                        FOREIGN KEY (passiveID) 
                            REFERENCES passives(passiveID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS enemyItems (
                        enemyID INTEGER NOT NULL,
                        itemID INTEGER NOT NULL,
                        quantity INTEGER NOT NULL,

                        FOREIGN KEY (enemyID) 
                            REFERENCES enemyClasses(enemyID),

                        FOREIGN KEY (itemID) 
                            REFERENCES items(itemID)
                    );
                ";
                command.ExecuteNonQuery();


                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS users (
                        authID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL,
                        password TEXT NOT NULL,
                        playerID int NOT NULL,

                        FOREIGN KEY (playerID) 
                            REFERENCES players(playerID)
                    );
                ";
                command.ExecuteNonQuery();

                data.Close();
            }
        }

        // Get PlayerID From Username
        public int GetPlayerID(string username)
        {
            int player = -1;

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                @"
                    SELECT playerID
                    FROM users
                    WHERE username = $user
                ";
                command.Parameters.AddWithValue("$user", username);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        player = reader.GetInt32(0);
                    }
                }

                data.Close();
            }

            return player;
        }

        // Get Username from PlayerID
        public string GetPlayerUsername(int id)
        {
            string player = "";

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                @"
                    SELECT username
                    FROM players
                    WHERE playerID = $id
                ";
                command.Parameters.AddWithValue("$id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        player = reader.GetString(0);
                    }
                }

                data.Close();
            }

            return player;
        }

        // Check for username and password (hash) in database
        public bool VerifyPlayerCredentials(string user, string pass)
        {
            bool playerExists = false;

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                @"
                    SELECT username
                    FROM users
                    WHERE username = $user AND password = $pass
                ";
                command.Parameters.AddWithValue("$user", user);
                command.Parameters.AddWithValue("$pass", pass);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        playerExists = true;
                    }
                }

                data.Close();
            }

            return playerExists;
        }

        // Add Username and Password (Hash) to Database
        // Create Player In Database
        // TODO
        public void AddPlayerCredentials(string user, string passHash)
        {
            int player = -1;

            int areaID = GetAreaID(startingArea);
            int roomID = GetAreaStartingRoom(startingArea);

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();

                command.CommandText =
                @"
                    INSERT INTO players (username, titleID, areaID, roomID)
                        VALUES ($user, 0, $areaID, $roomID)
                ";
                command.Parameters.AddWithValue("$user", user);
                command.Parameters.AddWithValue("$areaID", areaID);
                command.Parameters.AddWithValue("$roomID", roomID);
                command.ExecuteNonQuery();

                command.CommandText =
                @"
                    SELECT playerID
                    FROM players
                    WHERE username = $user
                ";
                command.Parameters.AddWithValue("$user", user);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        player = reader.GetInt32(0);
                    }
                }

                command.CommandText =
                @"
                    INSERT INTO users (username, password, playerID)
                    VALUES ($user, $pass, $playerID)
                ";
                command.Parameters.AddWithValue("$user", user);
                command.Parameters.AddWithValue("$pass", passHash);
                command.Parameters.AddWithValue("$playerID", player);
                command.ExecuteNonQuery();

                data.Close();
            }
        }

        // Get ID of an area from its name
        public int GetAreaID(string areaName)
        {
            int areaID = -1;

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();

                command.CommandText =
                @"
                    SELECT areaID
                    FROM areas
                    WHERE areaName = $areaName
                ";
                command.Parameters.AddWithValue("$areaName", areaName);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        areaID = reader.GetInt32(0);
                    }
                }

                data.Close();
            }

            return areaID;
        }

        // Get the initial room ID of an area
        public int GetAreaStartingRoom(string areaName)
        {
            int roomID = -1;

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();

                command.CommandText =
                @"
                    SELECT startingRoomID
                    FROM areas
                    WHERE areaName = $areaName
                ";
                command.Parameters.AddWithValue("$areaName", areaName);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        roomID = reader.GetInt32(0);
                    }
                }

                data.Close();
            }

            return roomID;
        }

        // Get room ID
        public int GetRoomID(int area, int x, int y)
        {
            int roomID = -1;

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();

                command.CommandText =
                @"
                    SELECT roomID
                    FROM rooms
                    WHERE x = $x AND y = $y AND areaID = $area
                ";
                command.Parameters.AddWithValue("$x", x);
                command.Parameters.AddWithValue("$y", y);
                command.Parameters.AddWithValue("$area", area);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        roomID = reader.GetInt32(0);
                    }
                }

                data.Close();

                return roomID;
            }
        }

        // Get Title, use UpdatePlayerFromDB
        private String GetTitleFromPlayer(String playerName)
        {
            String title = "";

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                 @"
                    SELECT titles.title
                    FROM players
                    JOIN titles ON players.titleID = titles.titleID
                    WHERE players.username = $user
                ";
                command.Parameters.AddWithValue("$user", playerName);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        title = reader.GetString(0);
                    }
                }

                data.Close();
            }

            return title;
        }

        // Get Area Object from player
        public NetArea GetAreaFromPlayer(String playerName)
        {
            NetArea area = null;

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                @"
                    SELECT areas.areaName, areas.width, areas.height
                    FROM players
                    JOIN areas ON areas.areaID = players.areaID
                    WHERE players.username = $user
                ";
                command.Parameters.AddWithValue("$user", playerName);

                String areaName = "";
                int width = -1;
                int height = -1;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        areaName = reader.GetString(0);
                        width = reader.GetInt32(1);
                        height = reader.GetInt32(2);
                    }
                }

                area = NetArea.CreateArea(areaName, width, height);

                data.Close();
            }

            return area;
        }
        
        // Get Room Object from Player
        public NetRoom GetRoomFromPlayer(String playerName)
        {
            NetRoom room = null;

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                @"
                    SELECT rooms.roomID, rooms.roomName, rooms.roomDesc, 
                           rooms.roomIDUp, rooms.roomIDDown, roomIDLeft, roomIDRight
                    FROM players
                    JOIN rooms ON rooms.roomID = players.roomID
                    WHERE players.username = $user
                ";
                command.Parameters.AddWithValue("$user", playerName);

                int roomID = -1;
                String roomName = "";
                String roomDesc = "";
                int up = -1;
                int down = -1;
                int left = -1;
                int right = -1;
                List<Modifier> modifiers = new List<Modifier>();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        roomID = reader.GetInt32(0);
                        roomName = reader.GetString(1);
                        roomDesc = reader.GetString(2);
                        up = reader.GetInt32(3);
                        down = reader.GetInt32(4);
                        left = reader.GetInt32(5);
                        right = reader.GetInt32(6);
                    }
                }

                command.CommandText =
                @"
                    SELECT roomModifers.text
                    FROM players
                    JOIN roomModifers ON roomModifers.roomID = players.roomID
                    WHERE players.username = $user
                ";
                command.Parameters.AddWithValue("$user", playerName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        modifiers.Add(Modifier.CreateModifier(reader.GetString(0)));
                    }
                }

                // Enemies

                room = NetRoom.CreateRoom(roomID, roomName, roomDesc, up, down, left, right, modifiers.ToArray());

                data.Close();
            }

            return room;
        }

        // Moves player to a room
        public void MovePlayerToRoom(string playerName, int roomID)
        {
            if (roomID > 0)
            {
                lock (data)
                {
                    data.Open();

                    var command = data.CreateCommand();
                    command.CommandText =
                    @"
                        UPDATE players
                        SET roomID = $roomID
                        WHERE username = $user
                    ";
                    command.Parameters.AddWithValue("$roomID", roomID);
                    command.Parameters.AddWithValue("$user", playerName);
                    command.ExecuteNonQuery();

                    data.Close();
                }
            }
        }

        public void UpdateRoomModifers()
        {
            TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
            int current = (int)Math.Floor(diff.TotalSeconds);

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                @"
                    DELETE FROM roomModifers
                    WHERE expiration < $current
                ";
                command.Parameters.AddWithValue("$current", current);
                int changed = command.ExecuteNonQuery();

                if (changed > 0)
                {
                    Server.logger.Info("Removed " + changed + " Expired Room Modifers");
                }

                data.Close();
            }
        }

        public void UpdateEnemyManager(EnemyManager enemyManager)
        {
            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                @"
                    SELECT enemyClasses.enemyID, enemyTemplates.roomID, enemyClasses.name, 
                           enemyClasses.description, enemyClasses.enterCombatText, 
                           enemyClasses.enterRoomText
                    FROM enemyTemplates
                    JOIN enemyClasses ON enemyClasses.enemyID = enemyTemplates.enemyClassID
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int enemyID = reader.GetInt32(0);
                        int roomID = reader.GetInt32(1);
                        string name = reader.GetString(2);
                        string description = reader.GetString(3);
                        string enterCombatText = reader.GetString(4);
                        string enterRoomText = reader.GetString(5);

                        List<ItemDrop> itemDrops = new List<ItemDrop>();
                        List<PassiveDrop> passiveDrops = new List<PassiveDrop>();
                        List<ActiveDrop> activeDrops = new List<ActiveDrop>();

                        var dropCommand = data.CreateCommand();
                        dropCommand.CommandText =
                        @"
                            SELECT chance, passiveID, activeID, itemID
                            FROM enemyDrops
                            WHERE enemyClassID = $enemyID
                        ";
                        dropCommand.Parameters.AddWithValue("$enemyID", enemyID);

                        using (var dropReader = dropCommand.ExecuteReader())
                        {
                            while (dropReader.Read())
                            {
                                double chance = dropReader.GetDouble(0);

                                if (!dropReader.IsDBNull(1))
                                {
                                    int passiveID = dropReader.GetInt32(1);
                                    NetPassive passive = AbilityManager.instance.GetPassive(passiveID).Net();
                                    passiveDrops.Add(PassiveDrop.CreatePassiveDrop(passive, chance));
                                }
                                if (!dropReader.IsDBNull(2))
                                {
                                    int activeID = dropReader.GetInt32(2);
                                    NetActive active = AbilityManager.instance.GetActive(activeID).Net();
                                    activeDrops.Add(ActiveDrop.CreateActiveDrop(active, chance));
                                }
                                if (!dropReader.IsDBNull(3))
                                {
                                    int itemID = dropReader.GetInt32(3);
                                    NetItem item = ItemManager.instance.GetItem(itemID).Net();
                                    itemDrops.Add(ItemDrop.CreateItemDrop(item, chance));
                                }
                            }
                        }

                        Enemy enemy = new Enemy(name, description, 
                            NetDrops.CreateDrops(itemDrops.ToArray(), passiveDrops.ToArray(), activeDrops.ToArray()), 
                            enterCombatText, enterRoomText);
                        enemyManager.AddEnemy(roomID, enemy);
                    }
                }

                data.Close();
            }
        }


        public void UpdateItemManager(ItemManager itemManager)
        {
            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                @"
                    SELECT itemID, 
                           itemName, itemDesc, itemType,
                           stackable, 
                           appliesPassiveID, appliesActiveID,
                           healthIncrease, healthOnKill, lifesteal, healthOnHit,
                           critChance, critDamage,
                           resist, bleedResist, posionResist, thorns,
                           baseDamage, skillDamage, allDamage,
                           poisonDamage, bleedDamage, markDamage,
                           whilePoisonDamage, whileBleedDamage, whileMarkDamage,
                           healingOutgoing, healingIncoming,
                           damageWhileMaxHealth, damageWhileLowHealth
                    FROM items
                ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // TODO
                        int itemID = reader.GetInt32(0);

                        Item item = new Item(reader.GetString(1), reader.GetString(2),
                            (ItemType)reader.GetInt32(3), reader.GetBoolean(4), 
                            AbilityManager.instance.GetPassive(reader.GetInt32(5)), 
                            AbilityManager.instance.GetActive(reader.GetInt32(6)),
                            reader.GetInt32(7), reader.GetInt32(8), reader.GetDouble(9), reader.GetInt32(10), 
                            reader.GetDouble(11), reader.GetDouble(12), reader.GetDouble(13), reader.GetDouble(14),
                            reader.GetDouble(15), reader.GetInt32(16), reader.GetDouble(17), reader.GetDouble(18), 
                            reader.GetDouble(19), reader.GetDouble(20), reader.GetDouble(21), reader.GetDouble(22), 
                            reader.GetDouble(23), reader.GetDouble(24), reader.GetDouble(25), reader.GetDouble(26), 
                            reader.GetDouble(27), reader.GetDouble(28), reader.GetDouble(29));
                        itemManager.AddItem(itemID, item);
                    }
                }

                data.Close();
            }
        }

        public void UpdateAbilityManager(AbilityManager abilityManager)
        {
            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                @"
                    SELECT activeID, name, description, 
                           cooldown, uses
                    FROM actives
                ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);

                        Active active = new Active(id,
                            reader.GetString(1), reader.GetString(2),
                            reader.GetInt32(3), reader.GetInt32(4));
                        abilityManager.AddActive(id, active);
                    }
                }

                command = data.CreateCommand();
                command.CommandText =
                @"
                    SELECT passiveID, name, description
                    FROM passives
                ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);

                        Passive passive = new Passive(id,
                            reader.GetString(1), reader.GetString(2));
                        abilityManager.AddPassive(id, passive);
                    }
                }

                data.Close();
            }
        }



        // TODO
        public void UpdatePlayerFromDB(Player player)
        {
            // Get Title
            player.SetTitle(GetTitleFromPlayer(player.Name));

            // Get Slots
            var command = data.CreateCommand();
            command.CommandText =
            @"
                    SELECT activeSlots, passiveSlots
                    FROM players
                    WHERE username = $user
                ";
            command.Parameters.AddWithValue("$user", player.Name);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                player.SetActiveSlots(reader.GetInt32(0));
                player.SetPassiveSlots(reader.GetInt32(1));
            }
        }

        public void GetInventoryFromPlayer(string player)
        {
            // SELECT items.name FROM items, inventory WHERE inventory.character_id = $player
        }

        public bool EquipPassive(int playerID, int passiveID, int slot)
        {
            Passive passive = AbilityManager.instance.GetPassive(passiveID);
            Player player = PlayerManager.instance.GetPlayer((ushort)playerID);

            if (passive == null || slot >= player.PassiveSlots)
            { 
                Server.logger.Warning(String.Format(
                    "{0} attempted to place Passive ID {1} in slot {2}", player.Name, passiveID, slot));
                return false;
            }

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                @"
                    UPDATE playerPassives
                    SET slot = 0
                    WHERE playerID = $playerID and slot = $slot
                ";
                command.Parameters.AddWithValue("$playerID", playerID);
                command.Parameters.AddWithValue("$slot", slot);
                command.ExecuteNonQuery();

                command = data.CreateCommand();
                command.CommandText =
                @"
                    UPDATE playerPassives
                    SET slot = $slot
                    WHERE playerID = $playerID and passiveID = $passiveID
                ";
                command.Parameters.AddWithValue("$playerID", playerID);
                command.Parameters.AddWithValue("$passiveID", passiveID);
                command.Parameters.AddWithValue("$slot", slot);
                command.ExecuteNonQuery();

                data.Close();
            }

            return player.Equip(slot, passive);
        }

        public bool EquipActive(int playerID, int activeID, int slot)
        {
            Active active = AbilityManager.instance.GetActive(activeID);
            Player player = PlayerManager.instance.GetPlayer((ushort)playerID);

            if (active == null || slot >= player.ActiveSlots)
            {
                Server.logger.Warning(String.Format(
                    "{0} attempted to place Active ID {1} in slot {2}", player.Name, activeID, slot));
                return false;
            }

            lock (data)
            {
                data.Open();

                var command = data.CreateCommand();
                command.CommandText =
                @"
                    UPDATE playerActives
                    SET slot = 0
                    WHERE playerID = $playerID and slot = $slot
                ";
                command.Parameters.AddWithValue("$playerID", playerID);
                command.Parameters.AddWithValue("$slot", slot);
                command.ExecuteNonQuery();

                command = data.CreateCommand();
                command.CommandText =
                @"
                    UPDATE playerActives
                    SET slot = $slot
                    WHERE playerID = $playerID and activeID = $activeID
                ";
                command.Parameters.AddWithValue("$playerID", playerID);
                command.Parameters.AddWithValue("$activeID", activeID);
                command.Parameters.AddWithValue("$slot", slot);
                command.ExecuteNonQuery();

                data.Close();
            }

            return player.Equip(slot, active);
        }
    }
}

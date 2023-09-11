using DarkRift;
using DarkRift.Server;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Timers;
using MUD.Net;
using MUD.Encryption;
using MUD.Managers;
using MUD.SQL;
using MUD.Characters;

namespace MUD
{

    /* 
     * Main plugin class. Manages clients connecting, disconnecting and 
     * base initialization of the game and the Managers.
     */
    public class Server : Plugin
    {
        public static Logger logger;
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);
        private RSAEncryption rsa;

        // Initialize ServerManager and Hooks.
        // Create RSA Encryption
        public Server(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            ClientManager.ClientConnected += OnClientConnected;
            ClientManager.ClientDisconnected += OnClientDisconnected;
            rsa = new RSAEncryption();

            System.Timers.Timer timer = new System.Timers.Timer(30000);
            timer.Elapsed += UpdateRoomModifers;
            timer.AutoReset = true;
            timer.Enabled = true;

            timer = new System.Timers.Timer(50);
            timer.Elapsed += Tick;
            timer.AutoReset = true;
            timer.Enabled = true;

            logger = Logger;

            Database.instance.UpdateItemManager(ItemManager.instance);
            Database.instance.UpdateAbilityManager(AbilityManager.instance);
            Database.instance.UpdateEnemyManager(EnemyManager.instance);

            if (Database.instance.GetPlayerID("FourOfSpades") == -1)
                Database.instance.AddPlayerCredentials("FourOfSpades", Authentication.Hash("!!!!!!"));
        }

        private static void UpdateRoomModifers(object source, ElapsedEventArgs e)
        {
            Database.instance.UpdateRoomModifers();
        }

        private static void Tick(object source, ElapsedEventArgs e)
        {
            CombatManager.instance.TickCombats();
        }

        // Add the MessageRecieved hooks to every client that connects. 
        // Give each client the Public Key so they can sign in.
        void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Verification v = new Verification();
            v.key = rsa.GetPublicKey();
            using (Message message = Message.Create((ushort)Tags.Tags.PUBLIC_KEY, v))
            {
                e.Client.SendMessage(message, SendMode.Reliable);
            }

            e.Client.MessageReceived += OnMessageRecieved;
        }

        // Logout Player on Disconnect
        void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            PlayerManager.instance.LogoutPlayer(e.Client.ID);
        }

        // On Message Recieved from Client
        void OnMessageRecieved(object sender, MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage())
            {
                // If the message was a command
                if (message.Tag == (ushort)Tags.Tags.COMMAND)
                {
                    OnCommandRecieve(message, e);
                }

                // If the message was a chat message
                if (message.Tag == (ushort)Tags.Tags.CHAT)
                {
                    OnChatRecieve(message, e);
                }

                // If the message was a login request
                if (message.Tag == (ushort)Tags.Tags.LOGIN)
                {
                    OnLoginRecieve(message, e);
                }

                // If the message was a player update request
                if (message.Tag == (ushort)Tags.Tags.PLAYER_UPDATE)
                {
                    OnPlayerUpdateRecieve(e);
                }
            }
        }

        void OnCommandRecieve(Message message, MessageReceivedEventArgs e)
        {
            using (DarkRiftReader reader = message.GetReader())
            {
                NetCommand command = reader.ReadSerializable<NetCommand>();

                // Ensure the chat message came from the correct client
                if (PlayerManager.instance.VerifyPlayer(e.Client.ID, command.token))
                {
                    Player player = PlayerManager.instance.GetPlayer(e.Client.ID);

                    PlayerManager.instance.HandleCommand(player, command);

                    PlayerManager.instance.SendPlayerInfo(e.Client.ID);
                }
            }
        }

        void OnChatRecieve(Message message, MessageReceivedEventArgs e)
        {
            using (DarkRiftReader reader = message.GetReader())
            {
                NetChat chat = reader.ReadSerializable<NetChat>();
                // Ensure the chat message came from the correct client
                if (PlayerManager.instance.VerifyPlayer(e.Client.ID, chat.token))
                {
                    // Get Player using their ID
                    Player player = PlayerManager.instance.GetPlayer(e.Client.ID);

                    // Send the chat message to everyone connected
                    ChatResponse publicChat = new ChatResponse();
                    publicChat.username = player.Name;
                    publicChat.title = player.Title;
                    publicChat.chatMessage = chat.chatMessage;

                    PlayerManager.instance.SendToAll(Tags.Tags.CHAT, publicChat);
                    // Console.WriteLine("Chat Message  : " + chat.chatMessage);
                }
            }
        }

        void OnLoginRecieve(Message message, MessageReceivedEventArgs e)
        {
            using (DarkRiftReader reader = message.GetReader())
            {
                Login login = reader.ReadSerializable<Login>();
                // Decrypt their password using the Server's Private Key
                login.Decrypt(rsa);
                // Verify that the player's username and password are correct
                if (Authentication.VerifyCredentials(login.username, login.password))
                {
                    // Get a token and send it to the player
                    string token = Authentication.GetToken();
                    // Initialize the Player in Server Memory. Load from database
                    PlayerManager.instance.LoginPlayer(e.Client, login.username, e.Client.ID, token);

                    Verification v = new Verification();
                    v.key = token;

                    using (Message m = Message.Create((ushort)Tags.Tags.TOKEN, v))
                    {
                        e.Client.SendMessage(m, SendMode.Reliable);
                    }

                    // Get player information
                    Player player = PlayerManager.instance.GetPlayer(e.Client.ID);

                    logger.Info(player.Name + " has logged in.");
                }
            }
        }

        void OnPlayerUpdateRecieve(MessageReceivedEventArgs e)
        {
            PlayerManager.instance.SendPlayerInfo(e.Client.ID);
        }

    }
}

using System;
using System.Net;
using System.Linq;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using MUD;
using MUD.Tags;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NetworkConnection
{
    public class ConnectionManager : MonoBehaviour
    {
        public static ConnectionManager Instance { get; private set; }
        public UnityClient Client { get; private set; }
        public string Token { get; private set; }

        void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
            }

            Instance = this; 
            DontDestroyOnLoad(this); 
        }

        void Start() {
            RSAEncryptionPublic rsa = new RSAEncryptionPublic();
            rsa.Initialize();
            Client = GetComponent<UnityClient>();
            Client.ConnectInBackground(IPAddress.Loopback, Client.Port, false, ConnectCallback);
            Client.MessageReceived += OnMessageRecieve;
        }

        private void ConnectCallback(Exception e) {
            if (Client.ConnectionState == ConnectionState.Connected) {
                Debug.Log("Connected to server!"); 
            }
            else {
                Debug.LogError($"Unable to connect to server. Reason: {e.Message} "); 
            }
        }

        private void OnMessageRecieve(object sender, MessageReceivedEventArgs e) {
            using (Message m = e.GetMessage()) {
                using (DarkRiftReader reader = m.GetReader()) {
                    if (m.Tag == (ushort)Tags.PUBLIC_KEY) {
                        Verification key = reader.ReadSerializable<Verification>();
                        RSAEncryptionPublic.Instance.SetPublicKey(key.key);
                    }
                    if (m.Tag == (ushort)Tags.TOKEN) {
                        Verification key = reader.ReadSerializable<Verification>();
                        Token = key.key;
                        if (Token != "")
                            SceneManager.LoadScene("MainScene");
                    }
                    if (m.Tag == (ushort)Tags.CHAT) {
                        ChatResponse chat = reader.ReadSerializable<ChatResponse>();
                        Debug.Log(chat.username);
                        Debug.Log(chat.title);
                        Debug.Log(chat.chatMessage);
                    }
                    if (m.Tag == (ushort)Tags.PLAYER_UPDATE) {
                        Player player = reader.ReadSerializable<Player>();
                    }
                    if (m.Tag == (ushort)Tags.AREA) {
                        Area area = reader.ReadSerializable<Area>();
                        SceneHandler.Instance.UpdateArea(area.ToString());
                    }
                    if (m.Tag == (ushort)Tags.ROOM) {
                        Room room = reader.ReadSerializable<Room>();
                        SceneHandler.Instance.UpdateScene(room.ToString());
                    }
                }
            }
        }

        public short VerifyCredentials(string user, string pass) {
            if (user.Length < 3) {
                return -1;
            }
            if (!user.All(char.IsLetterOrDigit)) {
                return -2;
            }
            if (pass.Length <= 5) {
                return -3;
            }
            return 0;
        }

        public void SendLoginRequest(string user, string pass) {
            Login login = new Login();
            login.username = user;
            login.password = RSAEncryptionPublic.Instance.Encrypt(pass);
            Debug.Log(login.username + "    " + login.password);
            using (Message message = Message.Create((ushort)Tags.LOGIN, login)) {
                ConnectionManager.Instance.Client.SendMessage(message, SendMode.Reliable);
            }
        }
    }
}
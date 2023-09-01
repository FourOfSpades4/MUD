using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using MUD;
using MUD.Tags;
using UnityEngine;
using NetworkConnection;
using TMPro;

namespace Input {
    public class ChatHandler : MonoBehaviour
    {
        private TMP_InputField _inputField;

        void Start() {
            _inputField = GetComponentInChildren<TMP_InputField>();
        }

        public void SendChat(string data) {
            if (data != "") {
                Chat chat = new Chat();
                chat.chatMessage = data;
                chat.token = ConnectionManager.Instance.Token;
                using (Message message = Message.Create((ushort)Tags.CHAT, chat)) {
                    ConnectionManager.Instance.Client.SendMessage(message, SendMode.Reliable);
                }
                _inputField.text = "";
                _inputField.Select();
                _inputField.ActivateInputField();
            }
        }
    }
}

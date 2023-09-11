using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using MUD.Net;
using MUD.Tags;
using UnityEngine;
using NetworkConnection;
using TMPro;

namespace Input {
    public class CommandHandler : MonoBehaviour
    {
        private TMP_InputField _inputField;

        void Start() {
            _inputField = GetComponentInChildren<TMP_InputField>();
        }

        public void SubmitCommand(string data) {
            if (data != "") {
                NetCommand command = new NetCommand();
                command.command = data;
                command.token = ConnectionManager.Instance.Token;
                using (Message message = Message.Create((ushort)Tags.COMMAND, command)) {
                    ConnectionManager.Instance.Client.SendMessage(message, SendMode.Reliable);
                }
                _inputField.text = "";
                _inputField.Select();
                _inputField.ActivateInputField();
            }
        }
    }
}

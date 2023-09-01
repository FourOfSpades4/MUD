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
    public class PasswordHandler : MonoBehaviour
    {
        [SerializeField] private LoginHandler _login;
        private TMP_InputField _passwordField;

        void Start() {
            _passwordField = GetComponentInChildren<TMP_InputField>();
        }
        public void Submit() {
            _passwordField.Select();
            _login.AttemptLogin();
            _passwordField.text = "";
        }
    }
}

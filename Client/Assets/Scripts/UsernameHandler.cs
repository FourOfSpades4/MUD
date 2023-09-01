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
    public class UsernameHandler : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _passwordField;
        
        private void Start() {
            GetComponentInChildren<TMP_InputField>().ActivateInputField();
        }

        public void Submit() {
            _passwordField.Select();
            _passwordField.ActivateInputField();
        }
    }
}

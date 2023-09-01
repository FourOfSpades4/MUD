using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NetworkConnection;

public class LoginHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameField;
    [SerializeField] private TMP_InputField _passwordField;
    [SerializeField] private TMP_Text _errorText;

    public void AttemptLogin() {
        string user = _usernameField.text;
        string pass = _passwordField.text;
        short valid = ConnectionManager.Instance.VerifyCredentials(user, pass);
        if (valid == 0) {
            ConnectionManager.Instance.SendLoginRequest(user, pass);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance { get; private set; }
    [SerializeField] private TMP_Text _sceneText;
    [SerializeField] private TMP_Text _areaText;



    void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdateScene(string str) {
        _sceneText.text = str;
    }

    public void UpdateArea(string str) {
        _areaText.text = str;
    }
}

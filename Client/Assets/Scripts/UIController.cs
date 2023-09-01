using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Image[] _elements;
        [SerializeField] private Color _backgroundColor;
        
        void Start() {
            foreach (Image im in _elements) {
                im.color = _backgroundColor;
            }
        }
    }
}

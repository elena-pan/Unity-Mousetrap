using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;

namespace MouseTrap
{
    public class PopupWindow : MonoBehaviour
    {

        [SerializeField]
        private Text windowText;

        public void DisplayText(string text) {
            windowText.text = text;
            this.gameObject.SetActive(true);
        }
    }
}
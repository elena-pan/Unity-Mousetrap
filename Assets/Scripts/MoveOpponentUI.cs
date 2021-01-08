using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

namespace MouseTrap
{
    public class MoveOpponentUI : MonoBehaviour
    {
        [SerializeField]
        private Dropdown dropdown;

        private List<int> validPlayers;
        private int currentDropdown = 0;

        public void MoveOpponentClicked()
        {
            GameManager.instance.MoveOpponent(validPlayers[currentDropdown]);
            this.gameObject.SetActive(false);
        }

        public void UpdateDropdownOptions()
        {
            dropdown.ClearOptions();
            List<string> options = new List<string>();
            validPlayers = new List<int>();
 
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++) {
                Player player = GameManager.instance.players[i];
                if (player == PhotonNetwork.LocalPlayer) continue;
                // Player not on cheese wheel
                if ((int)player.CustomProperties["Location"] != 49) {
                    options.Add(player.NickName);
                    validPlayers.Add(i);
                }
            }

            dropdown.AddOptions(options);
            dropdown.value = 0;
            dropdown.RefreshShownValue();
            currentDropdown = 0;
        }
        public void OnClickDropdownOption(Dropdown dropdown) {
            currentDropdown = dropdown.value;
        }

    }
}
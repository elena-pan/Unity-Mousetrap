using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace MouseTrap
{
    public class PlayerUI : MonoBehaviour
    {

        [SerializeField]
        private Text activityPanelLine1;
        [SerializeField]
        private Text activityPanelLine2;
        [SerializeField]
        private Text activityPanelLine3;
        [SerializeField]
        private Text activityPanelLine4;
        [SerializeField]
        private Text statsBody;

        [SerializeField]
        private Button moveOpponentButton;
        [SerializeField]
        private Button turnCrankButton;
        [SerializeField]
        private Button endTurnButton;

        void Update()
        {
            // Update player stats
            if (statsBody != null) {
                string temp = "Cheeses: " + PlayerManager.balance.ToString();
                if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Alive"] == true) {
                    temp = temp + "\nAlive: Yes";
                } else {
                    temp = temp + "\nAlive: No";
                }
                statsBody.text = temp;
            }
        }

        public void AddActivityText(string text) {
            activityPanelLine4.text = activityPanelLine3.text;
            activityPanelLine3.text = activityPanelLine2.text;
            activityPanelLine2.text = activityPanelLine1.text;
            activityPanelLine1.text = text;
        }

        public void EnableButton(string button) {
            switch (button) {
                case "moveOpponentButton":
                    moveOpponentButton.interactable = true;
                    break;
                case "turnCrankButton":
                    turnCrankButton.interactable = true;
                    break;
                case "endTurnButton":
                    endTurnButton.interactable = true;
                    break;
            }
        }
        
        public void DisableButton(string button)
        {
            switch (button) {
                case "moveOpponentButton":
                    moveOpponentButton.interactable = false;
                    break;
                case "turnCrankButton":
                    turnCrankButton.interactable = false;
                    break;
                case "endTurnButton":
                    endTurnButton.interactable = false;
                    break;
            }
        }
    }
}
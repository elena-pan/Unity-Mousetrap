using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

namespace MouseTrap
{
    public class BuildHouseUI : MonoBehaviour
    {
        [SerializeField]
        private Dropdown propertyDropdown;

        private List<int> validProperties;
        private int currentDropdown = 0;

        public void UpdateDropdownOptions()
        {
            propertyDropdown.ClearOptions();
            List<string> options = new List<string>();
            validProperties = new List<int>();

            bool[] ownedProp = (bool[])PhotonNetwork.LocalPlayer.CustomProperties["OwnedProperties"];

            propertyDropdown.AddOptions(options);
            propertyDropdown.value = 0;
            propertyDropdown.RefreshShownValue();
            currentDropdown = 0;
        }
        public void OnClickDropdownOption(Dropdown dropdown) {
            currentDropdown = dropdown.value;
        }

    }
}
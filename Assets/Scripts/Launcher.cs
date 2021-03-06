﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace MouseTrap
{
    public class Launcher : MonoBehaviourPunCallbacks
    {

        /// This client's version number. Users are separated from each other by gameVersion
        string gameVersion = "1";
        private int maxPlayers = 4;

        [SerializeField]
        private GameObject startingPanel;
        [SerializeField]
        private GameObject createGamePanel;
        [SerializeField]
        private GameObject joinGamePanel;
        [SerializeField]
        private GameObject connectingPanel;
        [SerializeField]
        private GameObject lobbyPanel;
        [SerializeField]
        private GameObject textPanel;
        [SerializeField]
        private Text textPanelText;

        [SerializeField]
        private Text joinedPlayersText;
        [SerializeField]
        private Text numPlayersText;
        [SerializeField]
        private Text hostText;

        [SerializeField]
        private InputField joinGameNameInput;
        [SerializeField]
        private InputField createGameNameInput;

        [SerializeField]
        private Button startGameButton;
        [SerializeField]
        private Button createGameButton;
        [SerializeField]
        private Button joinGameButton;

        public Player[] playerList;

        void Awake()
        {
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            SwitchPanels(connectingPanel);
            Connect();
        }

        void Update()
        {
            ToggleButtons();
        }

        public void SwitchPanels(GameObject panel)
        {
            // Set all panels as invisible and reveal selected panel
            startingPanel.SetActive(false);
            createGamePanel.SetActive(false);
            joinGamePanel.SetActive(false);
            lobbyPanel.SetActive(false);
            textPanel.SetActive(false);
            connectingPanel.SetActive(false);

            panel.SetActive(true);
        }

        public void Connect()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
            else {
                SwitchPanels(startingPanel);
            }
        }

        public void JoinGame()
        {
            Connect();
            SwitchPanels(connectingPanel);
            PhotonNetwork.JoinRoom(joinGameNameInput.text);
        }

        public void CreateGame()
        {
            Connect();
            SwitchPanels(connectingPanel);
            PhotonNetwork.CreateRoom(createGameNameInput.text, new RoomOptions { MaxPlayers = (byte)maxPlayers });
        }

        public void StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false; // Close room so nobody can join
                SwitchPanels(connectingPanel);
                PhotonNetwork.LoadLevel("Game");
            }
        }

        private void ToggleButtons()
        {
            if (string.IsNullOrEmpty(joinGameNameInput.text) || string.IsNullOrEmpty(PhotonNetwork.NickName)) {
                joinGameButton.interactable = false;
            } else {
                joinGameButton.interactable = true;
            }

            if (string.IsNullOrEmpty(createGameNameInput.text) || string.IsNullOrEmpty(PhotonNetwork.NickName)) {
                createGameButton.interactable = false;
            } else {
                createGameButton.interactable = true;
            }

            if (PhotonNetwork.IsMasterClient) {
                startGameButton.gameObject.SetActive(true);
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 2) {
                    startGameButton.interactable = true;
                } else {
                    startGameButton.interactable = false;
                }
            } else {
                startGameButton.gameObject.SetActive(false);
            }
        }

        private void UpdateTexts()
        {
            numPlayersText.text = "("+PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/4)";
            hostText.text = "";
            joinedPlayersText.text = "";
            foreach(Player player in playerList) {
                joinedPlayersText.text = joinedPlayersText.text + player.NickName + "\n";
                if (player.IsMasterClient) {
                    hostText.text = player.NickName;
                }
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            SwitchPanels(textPanel);
            textPanelText.text = cause.ToString();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            SwitchPanels(textPanel);
            textPanelText.text = message;
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            SwitchPanels(textPanel);
            textPanelText.text = message;
        }

        public override void OnJoinedRoom()
        {
            SwitchPanels(lobbyPanel);
            playerList = PhotonNetwork.PlayerList;

            // Set custom player properties
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("Alive", true);
            hash.Add("Balance", 0);
            hash.Add("Location", 0);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            
            UpdateTexts();
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            playerList = PhotonNetwork.PlayerList;
            UpdateTexts();
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            playerList = PhotonNetwork.PlayerList;
            UpdateTexts();
        }

        public override void OnConnectedToMaster()
        {
            SwitchPanels(startingPanel);
        }

    }
}
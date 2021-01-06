using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace MouseTrap
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager instance;
    
        public Dice dice;
        public PhotonView dicePhotonView;
        public Board board;

        public int currentPlayer;
        public Player[] players;

        public GameObject mousePrefab;

        public GameObject PlayerUi;
        public GameObject popUpWindow;
        
        void Awake()
        {
            instance = this;
        }

        void Start ()
        {
            currentPlayer = 0;
            players = PhotonNetwork.PlayerList;

            DisableButton("buyPropertyButton");
            DisableButton("buildHouseButton");
            DisableButton("sellPropertyButton");
            DisableButton("endTurnButton");
            
            board.SetUpLocations();

            if (PlayerManager.LocalPlayerInstance == null)
            {
                // Rotate by 270 because our prefabs are rotated 
                Quaternion rotation = Quaternion.Euler(-90, 0, 0);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(mousePrefab.name, board.locations[0].gridPoint, rotation, 0);
            }

            if (PhotonNetwork.IsMasterClient) {
                NextPlayer();
            }
        }
        public void Move(int steps)
        {
            int location;
            
            location = PlayerManager.location;
            location+=steps;
            if (location > 39)
            {
                location = location - 40;
                if (location != 0) { // Passed but not landed on GO
                    ReceiveMoney(200);
                }
            }
            MoveToLocation(location);
        }

        public void MoveToLocation(int location)
        {
            PlayerManager.location = location;
            PlayerManager.isMoving = true;
        }

        public void LandedOn(int? diceRoll = null)
        {
            string landedOnText = "landed on " + board.locations[PlayerManager.location].name;
            PhotonEvents.SendActivityMessage(landedOnText, currentPlayer);

            
        }

        public void NextPlayer()
        {
            DisableButton("buyPropertyButton");

            if (currentPlayer >= PhotonNetwork.CurrentRoom.PlayerCount-1) {
                currentPlayer = 0;
            } else {
                currentPlayer++;
            }

            if ((bool)players[currentPlayer].CustomProperties["Bankrupt"]) {
                NextPlayer();
                return;
            }

            PhotonEvents.SendEvent(PhotonEvents.PlayerTurnCode, currentPlayer);
        }

        public void StartTurn()
        {
            CameraController.viewDiceRoll = true;
            if (players[currentPlayer] == PhotonNetwork.LocalPlayer)
            {
                // Transfer ownership to current player so we can control and sync movement of the dice
                dicePhotonView.TransferOwnership(PhotonNetwork.LocalPlayer);

                popUpWindow.SetActive(true);
                popUpWindow.SendMessage("DisplayText", "It's your turn! Press space to roll the dice", SendMessageOptions.RequireReceiver);

                StartCoroutine(WaitForDiceRoll(diceNum => {
                    EnableButton("endTurnButton");
                    Move(diceNum);
                    CameraFollow.isFollowing = true; // Move camera to target for current player
                    LandedOn(diceNum);
                }));
            }
            else {
                string text = "It's " + players[currentPlayer].NickName + "'s turn!";
                popUpWindow.SetActive(true);
                popUpWindow.SendMessage("DisplayText", text, SendMessageOptions.RequireReceiver);
            }
        }

        public void ReceiveMoney(int amount)
        {
            PlayerManager.balance += amount;
        }

        public void PayMoney(int amount, Player recipient = null)
        {

            int paidAmount = amount;
            if (PlayerManager.balance < amount) {
                paidAmount = PlayerManager.balance;
                PlayerManager.balance = 0;
            }
            else {
                PlayerManager.balance -= amount;
            }
            if (recipient != null) {
                int[] targetActors = {recipient.ActorNumber};
                PhotonEvents.SendEvent(PhotonEvents.ReceiveMoneyCode, paidAmount, targetActors);
            }
        }

        public void CheckNumAlive()
        {
            // Check how many players are not bankrupt
            List<Player> alive = new List<Player>();
            foreach (Player player in players) {
                if ((bool)player.CustomProperties["Alive"] == false) {
                    alive.Add(player);
                }
            }

            if (alive.Count == 0) {
                EndGame(null);
                return;
            } else if (alive.Count == 1) { // Only one person left, they are the winner
                EndGame(alive[0]);
                return;
            }
        }

        public void Captured()
        {
            // Set custom player property as captured
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("Alive", false);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            PhotonEvents.SendActivityMessage("was captured!", currentPlayer);

            // Check how many players are not bankrupt
            // We do this here because the custom properties are not set fast enough
            List<Player> alive = new List<Player>();
            foreach (Player player in players) {
                if (player == PhotonNetwork.LocalPlayer) {
                    continue;
                }
                if ((bool)player.CustomProperties["Alive"] == false) {
                    alive.Add(player);
                }
            }

            if (alive.Count == 0) {
                EndGame(null);
                return;
            } else if (alive.Count == 1) { // Only one person left, they are the winner
                EndGame(alive[0]);
                return;
            }

            NextPlayer();
        }

        public void EndGame(Player winner, bool receivedSignal = false)
        {
            // We are sending the bankrupt signal
            // If we received the signal we don't want to send another signal out to everybody
            if (receivedSignal == false) {
                int playerNum = 0;
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++) {
                    if (players[i] == winner) {
                        playerNum = i;
                    }
                }
                PhotonEvents.SendEvent(PhotonEvents.EndGameCode, playerNum);
            }

            if (winner == null) {
                WinnerScene.winner = "Game over - no winners";
            }
            else if (winner == PhotonNetwork.LocalPlayer) {
                WinnerScene.winner = "You are the winner!";
            } else {
                WinnerScene.winner = winner.NickName + " is the winner!";
            }
            SceneManager.LoadScene(2);
        }
        
        private IEnumerator WaitForDiceRoll(System.Action<int> callback) {
        
            // wait for player to press space
            yield return WaitForKeyPress(KeyCode.Space); // wait for this function to return
            popUpWindow.SetActive(false);
        
            // Roll dice after player presses space
            dice.RollDice();
            while (DiceResult.diceNum == -1) {
                yield return new WaitForSeconds(2);
            }

            callback(DiceResult.diceNum); // Use callback to do something with result
        }
        
        private IEnumerator WaitForKeyPress(KeyCode key)
        {
            bool done = false;
            while(!done) // essentially a "while true", but with a bool to break out naturally
            {
                if(Input.GetKeyDown(key))
                {
                    done = true; // breaks the loop
                }
                yield return null; // wait until next frame, then continue execution from here (loop continues)
            }
        
            // now this function returns
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            string text = other.NickName + " has left the game.";
            PlayerUi.SendMessage ("AddActivityText", text, SendMessageOptions.RequireReceiver);

            if (PhotonNetwork.IsMasterClient) {
                if (players[currentPlayer] == other) {
                    players = PhotonNetwork.PlayerList;
                    NextPlayer();
                }
                else {
                    players = PhotonNetwork.PlayerList;
                    CheckNumAlive();
                }
            }
            else {
                players = PhotonNetwork.PlayerList;
            }
        }

        public void BuildHouse(int propertyNum)
        {
            
            // This will change the property on our local player since the message is also sent to us
            //object[] data = new object[] {propertyNum, "buildHouse"};
            //SendEvent(PropertyChangeCode, data);

            //property.houses[numHouses-1] = PhotonNetwork.Instantiate(house.name, location, property.houseRotation, 0).GetComponent<House>();
        }

        private void DisableButton(string button)
        {
            PlayerUi.SendMessage ("DisableButton", button, SendMessageOptions.RequireReceiver);
        }

        private void EnableButton(string button)
        {
            PlayerUi.SendMessage ("EnableButton", button, SendMessageOptions.RequireReceiver);
        }
    }
}
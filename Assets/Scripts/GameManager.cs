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
        public StopSign stopSign;

        public GameObject buildPanel;
        
        void Awake()
        {
            instance = this;
        }

        void Start ()
        {
            currentPlayer = 0;
            players = PhotonNetwork.PlayerList;

            DisableButton("moveOpponentButton");
            DisableButton("turnCrankButton");
            DisableButton("endTurnButton");
            
            board.SetUpLocations();

            if (PlayerManager.LocalPlayerInstance == null)
            {
                // Rotate because our prefabs are rotated 
                Quaternion rotation = Quaternion.Euler(-90, 0, 0);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                GameObject mouse = PhotonNetwork.Instantiate(mousePrefab.name, board.locations[0].gridPoint, rotation, 0);
                mouse.transform.localScale = new Vector3(50, 50, 50);
            }

            if (PhotonNetwork.IsMasterClient) {
                NextPlayer();
            }
        }
        public int Move(int steps)
        {
            int location = PlayerManager.location;           
            location += steps;
            if (location > 49)
            {
                location = location - 6;
            }
            MoveToLocation(location);
            return location;
        }

        public void MoveToLocation(int location)
        {
            CameraFollow.isFollowing = true; // Move camera to target for current player

            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("Location", location);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            PlayerManager.location = location;
            PlayerManager.isMoving = true;

            if (board.locations[location].name == "Build") {
                // Check if this is a build space where we can actually build
                if (PhotonNetwork.CurrentRoom.PlayerCount <= board.locations[location].buildNum) {
                    string landedOnText = "landed on " + board.locations[location].name;
                    PhotonEvents.SendActivityMessage(landedOnText, currentPlayer);
                }
            }
            else {
                string landedOnText = "landed on " + board.locations[location].name;
                PhotonEvents.SendActivityMessage(landedOnText, currentPlayer);
            }
            
        }

        public void LandedOn(int location)
        {
            Location currentLocation = board.locations[location];
            if (currentLocation.name == "Build") {
                // Check if this is a build space where we can actually build
                if (PhotonNetwork.CurrentRoom.PlayerCount <= currentLocation.buildNum && Build.numBuilt < 23) {
                    OpenBuild(1);
                }
            }
            else if (currentLocation.name == "Build 2") {
                if (PhotonNetwork.CurrentRoom.PlayerCount <= currentLocation.buildNum) {
                    if (Build.numBuilt < 22) {
                        OpenBuild(2);
                    }
                    else if (Build.numBuilt < 23) {
                        OpenBuild(1);
                    }
                }
            }
            else if (currentLocation.name == "TURN CRANK") {
                TurnCrankOptions();
            }
            else if (currentLocation.name == "Take 3 Pieces of Cheese From the Rival With the Most") {
                Player rival = players[0];
                int greatestNum = 0;
                foreach (Player player in players) {
                    if (player == PhotonNetwork.LocalPlayer) continue;

                    int numCheese = (int)player.CustomProperties["Balance"];
                    if (numCheese >= greatestNum) {
                        rival = player;
                        greatestNum = numCheese;
                    }
                }

                int amount = 3;
                if (greatestNum < 3) {
                    amount = greatestNum;
                }

                ReceiveMoney(amount);
                int[] targetActors = {rival.ActorNumber};
                PhotonEvents.SendEvent(PhotonEvents.PayMoneyCode, amount, targetActors);
            }
            else {
                if (currentLocation.numCheese < 0) {
                    PayMoney(-currentLocation.numCheese);
                } 
                else if (currentLocation.numCheese > 0) {
                    ReceiveMoney(currentLocation.numCheese);
                }

                if (currentLocation.moveSteps != 0) {
                    Move(currentLocation.moveSteps);
                }
                else if (currentLocation.moveTo >= 0) {
                    MoveToLocation(currentLocation.moveTo);
                }
            }
        }

        public void NextPlayer()
        {
            DisableButton("moveOpponentButton");
            DisableButton("turnCrankButton");
            DisableButton("endTurnButton");

            if (currentPlayer >= PhotonNetwork.CurrentRoom.PlayerCount-1) {
                currentPlayer = 0;
            } else {
                currentPlayer++;
            }

            if ((bool)players[currentPlayer].CustomProperties["Alive"] == false) {
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
                    int location = Move(diceNum);
                    LandedOn(location);
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
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("Balance", PlayerManager.balance);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            string text = "took " + amount + " pieces of cheese";
            PhotonEvents.SendActivityMessage(text, currentPlayer);
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

            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("Balance", PlayerManager.balance);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            string text = "lost " + amount + " pieces of cheese";
            PhotonEvents.SendActivityMessage(text, currentPlayer);

            if (recipient != null) {
                int[] targetActors = {recipient.ActorNumber};
                PhotonEvents.SendEvent(PhotonEvents.ReceiveMoneyCode, paidAmount, targetActors);
            }
        }

        public void OpenBuild(int num)
        {
            PhotonTransferOwnership.TransferOwnershipToSelf();
            if (num > 1) {
                Build.buildAnother = true;
            }
            DragandDrop.UpdateImage(Build.numBuilt);
            buildPanel.SetActive(true);
        }

        public void MoveOpponent(int playerNum)
        {
            PayMoney(1);
            CameraController.viewDiceRoll = true;
            // Transfer ownership to current player so we can control and sync movement of the dice
            dicePhotonView.TransferOwnership(PhotonNetwork.LocalPlayer);

            popUpWindow.SetActive(true);
            popUpWindow.SendMessage("DisplayText", "Press space to roll the dice", SendMessageOptions.RequireReceiver);

            StartCoroutine(WaitForDiceRoll(diceNum => {
                
                int location = (int)players[playerNum].CustomProperties["Location"];           
                location += diceNum;
                if (location > 49)
                {
                    location = location - 6;
                }
                
                string text = "moved " + players[playerNum].NickName + " to " + board.locations[location].name;
                PhotonEvents.SendActivityMessage(text, currentPlayer);

                int[] targetActors = {players[playerNum].ActorNumber};
                PhotonEvents.SendEvent(PhotonEvents.MoveCode, location, targetActors);

                // Make sure updates have synced
                StartCoroutine(WaitForUpdate(playerNum, location, () => {
                    TurnCrankOptions();
                }));
            }));
        }

        public void TurnCrank()
        {
            DisableButton("moveOpponentButton");
            DisableButton("turnCrankButton");

            string text = "turned the crank!";
            PhotonEvents.SendActivityMessage(text, currentPlayer);

            PhotonEvents.SendEvent(PhotonEvents.StartContraptionCode, 0);

            stopSign.StartContraption();
        }

        public void TurnCrankOptions()
        {
            int numOnCheeseWheel = 0;
                
            foreach (Player player in players) {
                if (player == PhotonNetwork.LocalPlayer) continue;
                if ((int)player.CustomProperties["Location"] == 49) {
                    numOnCheeseWheel++;
                }
            }

            int numNotOnCheeseWheel = PhotonNetwork.CurrentRoom.PlayerCount - numOnCheeseWheel - 1;
            if (numOnCheeseWheel > 0 && Build.numBuilt == 23) {
                EnableButton("turnCrankButton");
            }
            if (numNotOnCheeseWheel > 0 && PlayerManager.balance >= 1) {
                EnableButton("moveOpponentButton");
            }
        }

        public void CheckNumAlive()
        {
            // Check how many players are alive
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

            // Check how many players are alive
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

        private IEnumerator WaitForUpdate(int playerNum, int location, System.Action callback)
        {
            bool done = false;
            while(!done)
            {
                if ((int)players[playerNum].CustomProperties["Location"] == location)
                {
                    done = true; // breaks the loop
                }
                yield return null;
            }

            callback();
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
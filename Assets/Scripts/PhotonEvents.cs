using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace MouseTrap {
    public class PhotonEvents : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public const byte SendNewActivityLineCode = 1;
        public const byte PlayerTurnCode = 2;
        public const byte ReceiveMoneyCode = 3;
        public const byte PayMoneyCode = 4;
        public const byte MoveCode = 5;
        public const byte StartContraptionCode = 6;
        public const byte NumBuiltCode = 7;
        public const byte EndGameCode = 8;

        public GameObject PlayerUi;

        public static void SendActivityMessage(string content, int? player = null)
        {
            object[] data = new object[] {player, content};
            SendEvent(SendNewActivityLineCode, data);
        }

        public void ReceiveActivityMessage(object[] arr)
        {
            string content = (string)arr[1];
            if (arr[0] == null) {
                PlayerUi.SendMessage ("AddActivityText", content, SendMessageOptions.RequireReceiver);
            } else {
                Player player = GameManager.instance.players[(int)arr[0]];
                string text;
                if (player == PhotonNetwork.LocalPlayer) {
                    text = "You " + content;
                } else {
                    text = player.NickName + " " + content;
                }
                PlayerUi.SendMessage ("AddActivityText", text, SendMessageOptions.RequireReceiver);
            }
        }

        public static void SendEvent(byte eventCode, object content, int[] targetActors = null)
        {
            RaiseEventOptions raiseEventOptions = raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // Set Receivers to All to receive this event on the local client as well
            if (targetActors != null && targetActors.Length != 0) {
                raiseEventOptions = new RaiseEventOptions { TargetActors = targetActors }; // Send to specific clients
            }
            PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            switch (eventCode) {
                case SendNewActivityLineCode:
                    object[] arr = (object[])photonEvent.CustomData;
                    ReceiveActivityMessage(arr);
                    break;
                case PlayerTurnCode:
                    GameManager.instance.currentPlayer = (int)photonEvent.CustomData;
                    GameManager.instance.StartTurn();
                    break;
                case ReceiveMoneyCode:
                    int amount = (int)photonEvent.CustomData;
                    GameManager.instance.ReceiveMoney(amount);
                    break;
                case PayMoneyCode:
                    int amount2 = (int)photonEvent.CustomData;
                    GameManager.instance.PayMoney(amount2);
                    break;
                case MoveCode:
                    int location = (int)photonEvent.CustomData;
                    GameManager.instance.MoveToLocation(location);
                    break;
                case StartContraptionCode:
                    GameManager.instance.isTurningCrank = true;
                    CameraController.viewContraption = true;
                    GameManager.instance.CapturedCoroutine();
                    break;
                case NumBuiltCode:
                    int newNumBuilt = (int)photonEvent.CustomData;
                    GameManager.instance.build.LocalBuild(newNumBuilt);
                    break;
                case EndGameCode:
                    int? playerNum = (int?)photonEvent.CustomData;
                    if (playerNum == null) {
                        GameManager.instance.EndGame(null, true);
                    }
                    else {
                        GameManager.instance.EndGame(GameManager.instance.players[(int)playerNum], true);
                    }
                    break;
            }
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
    }
}

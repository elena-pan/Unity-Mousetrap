using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace MouseTrap 
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        public GameObject PlayerFloatingName;
        
        public static GameObject LocalPlayerInstance;
        public static bool isMoving;
        public static int location;
        public static int balance;
        
        public float lerpSpeed = 5f;
        public float slerpSpeed = 10f;

        void Awake()
        {
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }
        
        void Start()
        {
            if (PlayerFloatingName != null)
            {
                GameObject _uiGo =  Instantiate(PlayerFloatingName);
                _uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
        }

        void Update()
        {
            // Continually update location of our piece
            if (photonView.IsMine && LocalPlayerInstance && isMoving) {
                Vector3 newLocation = GameManager.instance.board.locations[location].gridPoint;
                LocalPlayerInstance.transform.position = Vector3.Lerp(LocalPlayerInstance.transform.position, newLocation, lerpSpeed*Time.deltaTime);

                // Also rotate back to upright
                Quaternion upright = Quaternion.Euler(-90, 0, 0);
                LocalPlayerInstance.transform.rotation = Quaternion.Slerp(LocalPlayerInstance.transform.rotation, upright, slerpSpeed*Time.deltaTime);

                if (Vector3.Distance(LocalPlayerInstance.transform.position, newLocation) < 1.0f) {
                    isMoving = false;
                }
            }
        }
    }
}
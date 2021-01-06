using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace MouseTrap {
    public class PhotonTransferOwnership : MonoBehaviour
    {

        public PhotonView dice;
        public PhotonView ball1;
        public PhotonView ball2;
        public PhotonView cage;
        public PhotonView shoe;
        public PhotonView stopSign;
        public PhotonView bucket;
        public PhotonView hand;
        public PhotonView handRod;
        public PhotonView part15;
        public PhotonView teeterTotter;
        public PhotonView diver;
        public PhotonView baseC;
        public PhotonView tub;
        public PhotonView post;
        private PhotonView[] objects;

        void Start()
        {
            objects = new PhotonView[] {dice, ball1, ball2, cage, shoe, stopSign, bucket, hand, handRod, part15, teeterTotter, diver, baseC, tub, post};
        }
        public void TransferOwnershipToSelf()
        {
            foreach (PhotonView obj in objects) {
                obj.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }
    }
}
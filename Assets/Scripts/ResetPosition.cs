using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

namespace MouseTrap
{
    public class ResetPosition : MonoBehaviour
    {
        private Vector3 startPos;
        private Quaternion startRotation;
        void Awake()
        {
            startPos = this.transform.position;
            startRotation = this.transform.rotation;
        }

        public void ResetPos()
        {
            this.transform.position = startPos;
            this.transform.rotation = startRotation;
        }
    }
}
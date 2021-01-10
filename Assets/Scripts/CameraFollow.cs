using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace MouseTrap
{
    public class CameraFollow : MonoBehaviourPunCallbacks
    {
        [Tooltip("The distance in the local x-z plane to the target")]
        [SerializeField]
        private float distance = 5.0f;

        [Tooltip("The height we want the camera to be above the target")]
        [SerializeField]
        private float height = 3.0f;

        [Tooltip("The Smoothing for the camera to follow the target")]
        [SerializeField]
        private float lerpSpeed = 7f;

        // cached transform of the target
        Transform cameraTransform;

        public static bool isFollowing = false;

        public static Player target = null;

        // Cache for camera offset
        Vector3 cameraOffset = Vector3.zero;
        void Start()
        {
            cameraTransform = Camera.main.transform;
        }

        void LateUpdate()
        {
            
            if (isFollowing && photonView.IsMine) {
                Follow();
            }
            else if (target != null && photonView.Owner == target) {
                FollowOther();
            }
        }

        /// Follow the target smoothly
        void Follow()
        {
            CameraController.viewContraption = false; // Prevents both happening at once
            CameraController.viewDiceRoll = false;
            target = null;
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            Vector3 newPos = this.transform.position + cameraOffset;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, newPos, lerpSpeed*Time.deltaTime);
            cameraTransform.LookAt(this.transform.position);

            // Stop following once we reach the position
            if (cameraTransform.position == newPos) {
                isFollowing = false;
            }
        }

        void FollowOther()
        {
            CameraController.viewContraption = false; // Prevents both happening at once
            CameraController.viewDiceRoll = false;
            isFollowing = false;
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            Vector3 newPos = this.transform.position + cameraOffset;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, newPos, lerpSpeed*Time.deltaTime);
            cameraTransform.LookAt(this.transform.position);

            // Stop following once we reach the position
            if (Vector3.Distance(cameraTransform.position, newPos) < 5f) {
                target = null;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MouseTrap {
    public class Diver : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody cage;
        [SerializeField]
        private Collider hook;
        private bool triggerActive = true;

        void OnCollisionExit(Collision info) {
            if (triggerActive && info.collider.name == "PART 18 TEETER TOTTER") {
                cage.constraints = RigidbodyConstraints.None;
            }
        }
        void OnCollisionEnter(Collision info) {
            if (triggerActive && info.collider.name == "PART 21 TUB") {
                hook.enabled = false;
                triggerActive = false;
            }
        }
    }
}
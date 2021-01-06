using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MouseTrap {
    public class IncreaseGravity : MonoBehaviour
    {
        private Rigidbody rb;
        private float increaseFactor = 100f;
        private bool triggered = false;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (triggered) {
                rb.AddForce(Physics.gravity*increaseFactor, ForceMode.Acceleration);
            }
        }

        void OnTriggerEnter(Collider other) {
            if (other.name == "Ball detector") {
                triggered = true;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MouseTrap {
    public class HandCentreOfMass : MonoBehaviour
    {
        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = new Vector3(0f, 0f, 0.002f);
        }
    }
}
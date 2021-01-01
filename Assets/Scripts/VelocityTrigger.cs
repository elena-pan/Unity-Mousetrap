using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTrigger : MonoBehaviour
{
    private Rigidbody rb;
    private float increaseFactor = 40f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnTriggerStay(Collider other) {
        if (other.name == "Velocity trigger") {
            rb.AddForce(rb.velocity.normalized*increaseFactor, ForceMode.Impulse);
        }
    }

}

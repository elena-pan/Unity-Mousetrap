using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTrigger : MonoBehaviour
{
    private Rigidbody rb;
    private float increaseFactor = 40f;
    private float increaseFactor2 = 1.5f;
    private Vector3 slideLocation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slideLocation = new Vector3(-7.8f, 1.65f, -5.63f);
    }
    void OnTriggerStay(Collider other) {
        if (other.name == "Velocity trigger") {
            rb.AddForce(rb.velocity.normalized*increaseFactor, ForceMode.Impulse);
        }
        else if (other.name == "Velocity trigger 2") {
            Vector3 direction = (slideLocation - rb.transform.position).normalized;
            rb.AddForce(direction*increaseFactor2, ForceMode.Impulse);
        }
    }

}

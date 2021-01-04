using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTrigger : MonoBehaviour
{
    private Rigidbody rb;
    private float increaseFactor = 40f;
    private float increaseFactor2 = 1.5f;
    private Vector3 slideLocation;

    private bool triggerActive1 = true;
    private bool triggerActive2 = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slideLocation = new Vector3(-7.6f, 1.65f, -5.63f);
    }
    void OnTriggerStay(Collider other) {
        if (triggerActive1 && other.name == "Velocity trigger") {
            rb.AddForce(rb.velocity.normalized*increaseFactor, ForceMode.Impulse);
        }
        else if (triggerActive2 && other.name == "Velocity trigger 2") {
            Vector3 direction = (slideLocation - rb.transform.position).normalized;
            rb.AddForce(direction*increaseFactor2, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision info) {
        if (info.gameObject.name == "PART 14 HAND ROD") {
            triggerActive1 = false;
        }
        else if (info.gameObject.name == "PART 18 TEETER TOTTER") {
            triggerActive2 = false;
        }
    }

}

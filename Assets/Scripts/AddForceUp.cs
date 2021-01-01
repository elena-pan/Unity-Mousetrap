using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceUp : MonoBehaviour
{
    private Rigidbody rb;
    private float increaseFactor = 15f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnCollisionEnter(Collision info) {
        if (info.collider.name == "BALL 1 (1)") {
            rb.AddForce(Vector3.up*increaseFactor, ForceMode.VelocityChange);
        }
    }

}

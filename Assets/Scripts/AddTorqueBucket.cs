using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTorqueBucket : MonoBehaviour
{
    private Rigidbody rb;
    private float increaseFactor = 5f;
    private bool triggerActive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision info) {
        if (info.gameObject.name == "SHOE") {
            rb.AddForce(info.impulse.normalized*increaseFactor, ForceMode.Impulse);
        }
    }
}

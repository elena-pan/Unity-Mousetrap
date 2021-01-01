using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseGravity : MonoBehaviour
{
    private Rigidbody rb;
    private float increaseFactor = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity*increaseFactor, ForceMode.Acceleration);
    }
}

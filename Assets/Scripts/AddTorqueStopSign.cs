using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTorqueStopSign : MonoBehaviour
{
    private Rigidbody rb;
    private float increaseFactor = -200f;
    private bool triggerActive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.AddRelativeTorque(transform.up * increaseFactor, ForceMode.Impulse);
    }
}

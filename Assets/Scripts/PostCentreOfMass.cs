using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostCentreOfMass : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0.02f, 0f, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    private Rigidbody _rigidBody;
    public float forwardSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidBody.AddForce(Vector3.forward * forwardSpeed,ForceMode.Acceleration);
    }
}

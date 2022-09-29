using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    GameObject[] wheelMesh = new GameObject[4];

    public float power = 100f;
    public float rot = 45f;

    Rigidbody rb;
    InputManager IM;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1, 0);

        wheelMesh = GameObject.FindGameObjectsWithTag("WheelMesh");

        IM = GetComponent<InputManager>();

        for(int i = 0; i < wheelMesh.Length; i ++)
        {
            wheels[i].transform.position = wheelMesh[i].transform.position;
        }
    }

    private void FixedUpdate() 
    {
        for (int i = 0; i < wheels.Length; i ++)
            wheels[i].motorTorque = IM.vertical * power;
        
        for (int i = 0; i < 2; i ++)
            wheels[i].steerAngle = IM.horizontal * rot;
        WheelPosAndAni();   
    }

    private void WheelPosAndAni()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for(int i = 0; i < 4; i ++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float slipRate = 1.0f;
    float handbreakSlipRate = 0.4f;
    public float maxTorque = 50f;
    public Transform centerOfMass;

    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Transform[] tireMeshes = new Transform[4];

    Rigidbody rb;
    private WheelFrictionCurve fFrictionRWL;
    private WheelFrictionCurve sFrictionRWL;
    private WheelFrictionCurve fFrictionRWR;
    private WheelFrictionCurve sFrictionRWR;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
        wheelColliders[2] = GetComponent<WheelCollider>();
        wheelColliders[3] = GetComponent<WheelCollider>();

        fFrictionRWL = wheelColliders[2].forwardFriction;
        sFrictionRWL = wheelColliders[2].sidewaysFriction;
        fFrictionRWR = wheelColliders[3].forwardFriction;
        sFrictionRWR = wheelColliders[3].sidewaysFriction;
    }

    private void Update() 
    {
        UpdateMeshesPositions();

        if(Input.GetKey(KeyCode.LeftShift))
        {
            fFrictionRWL.stiffness = handbreakSlipRate;
            wheelColliders[2].forwardFriction = fFrictionRWL;

            sFrictionRWL.stiffness = handbreakSlipRate;
            wheelColliders[2].sidewaysFriction = sFrictionRWL;

            fFrictionRWR.stiffness = handbreakSlipRate;
            wheelColliders[3].forwardFriction = fFrictionRWR;

            sFrictionRWR.stiffness = handbreakSlipRate;
            wheelColliders[3].sidewaysFriction = sFrictionRWR;
            print("aaaaaaaaaaaaaaaa");
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            fFrictionRWL.stiffness = slipRate;
            wheelColliders[2].forwardFriction = fFrictionRWL;

            sFrictionRWL.stiffness = slipRate;
            wheelColliders[2].sidewaysFriction = sFrictionRWL;

            fFrictionRWR.stiffness = slipRate;
            wheelColliders[3].forwardFriction = fFrictionRWR;

            sFrictionRWR.stiffness = slipRate;
            wheelColliders[3].sidewaysFriction = sFrictionRWR;
        }
    }

    private void FixedUpdate() 
    {
        float steer = Input.GetAxis("Horizontal");
        float accelerate = Input.GetAxis("Vertical");

        float finalAngle = steer * 45f;

        wheelColliders[0].steerAngle = finalAngle;
        wheelColliders[1].steerAngle = finalAngle;   

        for(int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = accelerate * maxTorque;
        } 
    }

    private void UpdateMeshesPositions()
    {
        for(int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 pos;
            wheelColliders[i].GetWorldPose(out pos, out quat);

            tireMeshes[i].position = pos;
            tireMeshes[i].rotation = quat;
        }
    }
}

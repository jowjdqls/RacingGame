using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum driveType
    {
        FRONTDRIVE,
        REARDRIVE,
        ALLDRIVE
    }
    [SerializeField]driveType drive;

    public WheelCollider[] wheels = new WheelCollider[4];
    GameObject[] wheelMesh = new GameObject[4];

    public float power = 100f;
    public float rot = 45f;
    public float downForceValue;
    public float radius = 6;

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
        // 전륜 구동일 때
        if (drive == driveType.ALLDRIVE)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = IM.vertical * (power/4);
            }
            
            if (IM.vertical == 0)	// 전진 중이 아닐 때
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].brakeTorque = power/4;
                }
            }   
            else	// 키를 눌렀을 때
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].brakeTorque = 0; // 브레이크 해제
                }
            }
        }   
        else if (drive == driveType.REARDRIVE)	// 후륜구동일 때
        {
            // 뒷바퀴에만.
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = IM.vertical * (power / 2);
            }

            if (IM.vertical == 0)
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].brakeTorque = power/2;
                }
            }
            else
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].brakeTorque = 0;
                }
            }
        }
        else	// 전륜 구동일 때
        {	// 앞바퀴에만
            for (int i = 0; i < 2; i++)
            {
                wheels[i].motorTorque = IM.vertical * (power / 2);
            }
            if (IM.vertical == 0)
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].brakeTorque = power/2;
                }
            }
            else
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                        wheels[i].brakeTorque = 0;
                }
            }
        }

        SteerVehicle();
        WheelPosAndAni();
        AddDownForce();
    }

    private void SteerVehicle()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {   // rear tracks size is set to 1.5f          wheel base has been set to 2.55f
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * IM.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * IM.horizontal;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * IM.horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * IM.horizontal;
            // transform.Rotate(Vector3.up * steerHelping)
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }
    }

    private void AddDownForce()
    {
        rb.AddForce(-transform.up * downForceValue * rb.velocity.magnitude);
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

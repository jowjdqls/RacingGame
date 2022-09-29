using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public bool handbreak;

    private void FixedUpdate() 
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        handbreak = Input.GetAxis("Drift") != 0 ? true : false;    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 locationOffset;
    public Vector3 rotationOffset;


   
    void Update()
    {
       
        this.transform.position = target.position + locationOffset;
       
    }
}

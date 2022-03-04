using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObject : MonoBehaviour
{
    
    public int angle = -1;
    public float rotateSpeed = 80f;
    // Update is called once per frame
    void Update()
    {
        RotateLeft();        
    }
    void RotateLeft()
    {
        // transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
        transform.RotateAround(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.forward, Time.deltaTime * -rotateSpeed);
    }
    // void OnCollisionEnter2D(Collision2D c)
    // {
    //     angle = 0;
    // }
}

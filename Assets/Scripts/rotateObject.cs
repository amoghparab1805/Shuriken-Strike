using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObject : MonoBehaviour
{
    
    public int angle = -1;
    // Update is called once per frame
    void Update()
    {
        RotateLeft();        
    }
    void RotateLeft()
    {
        transform.Rotate(Vector3.forward * angle);
    }
    void OnCollisionEnter2D(Collision2D c)
    {
        angle = 0;
    }
}

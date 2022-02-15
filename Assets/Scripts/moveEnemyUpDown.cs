using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveEnemyUpDown : MonoBehaviour
{
    public float delta = 2.0f; 
    public float speed = 1.0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 v = startPos;
        v.y += delta * Mathf.PingPong(Time.time * speed, 1);
        transform.position = v;
    }
}

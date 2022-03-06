using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_down : MonoBehaviour
{
    public float speed = -1f;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, speed);
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.transform.position.y < -76f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Block"){
            // Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}

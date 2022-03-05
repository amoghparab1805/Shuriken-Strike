using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    private GameObject currentTeleporter;
    private float distance = 2f;
    private bool canTeleport = true;
    public Vector3 lv;
    public Rigidbody2D rb;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Teleporter") && Vector2.Distance(transform.position, other.transform.position) > distance) {
            currentTeleporter = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        
        if(other.CompareTag("Teleporter")){
            if(other.gameObject == currentTeleporter){
                // Debug.Log("Cleaning up");
                currentTeleporter = null;

            }
        }
    }
    void Update()
    {
        lv = rb.velocity;
        if(currentTeleporter != null && canTeleport)
        {   
            StartCoroutine(Teleport());
            StartCoroutine(waitForTeleport());
        }
    }

    IEnumerator Teleport()
    {
        canTeleport = false;
        // Debug.Log(canTeleport);
        var speed = lv.magnitude;
        // var direction = lv.normalized;
        var angle = -currentTeleporter.GetComponent<Teleport>().getDestination().rotation.eulerAngles.z;
        var direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0);
        // Debug.Log(lv.normalized);
        // Debug.Log(direction);
        rb.velocity = direction * Mathf.Max(speed, 0f);
        transform.position = currentTeleporter.GetComponent<Teleport>().getDestination().position;
        yield return new WaitForSeconds(2);
    }

    IEnumerator waitForTeleport()
    {
        yield return new WaitForSeconds(1);
        canTeleport = true;
        // Debug.Log(canTeleport);
    }

    
}

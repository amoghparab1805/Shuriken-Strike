using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    private GameObject currentTeleporter;
    private float distance = 2f;
    private void OnTriggerEnter2D(Collider2D other) {
        // if(Vector2.Distance(transform.position, other.transform.position)<distance) {
        //     Debug.Log("Distance "+Vector2.Distance(transform.position, other.transform.position));
        //     Debug.Log("Player "+ transform.position);
        //     Debug.Log("Collider "+ other.transform.position);
        // }
        if(other.CompareTag("Teleporter") && Vector2.Distance(transform.position, other.transform.position)>distance) {
            currentTeleporter = other.gameObject;
            Debug.Log("Entering teleport object");
        }
    //     currentTeleporter = null;
    }

    private void OnTriggerExit2D(Collider2D other) {
        
        if(other.CompareTag("Teleporter")){
            if(other.gameObject == currentTeleporter){
                currentTeleporter = null;

            }
        }
    }
    void Update()
    {
        if(currentTeleporter != null)
        {   
            StartCoroutine(Teleport());

        }
    }

    IEnumerator Teleport()
    {
        transform.position = currentTeleporter.GetComponent<Teleport>().getDestination().position;
        yield return new WaitForSeconds(2);
        
    }

    
}

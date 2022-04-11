using UnityEngine;
using System;
public class Block : MonoBehaviour
{

    public AudioSource killSound;

    public event Action onBeingHit;
    PlayerController pc;

    // Hit by powerup
    private void OnTriggerEnter2D(Collider2D other){
        // && other.gameObject.tag=="Powerup"
        Debug.Log("OnTriggerEnter2D");
        if(onBeingHit != null && other.tag=="bullet") {
            onBeingHit();
            killSound.Play();
            gameObject.SetActive(false);
        }
    }

    // Hit by ball
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("OnCollisionEnter2D");
        if(onBeingHit != null) {
            onBeingHit();
            killSound.Play();
            gameObject.SetActive(false);
        }
    }
}

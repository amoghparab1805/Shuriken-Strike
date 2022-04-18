using UnityEngine;
using System;
public class Block : MonoBehaviour
{

    public AudioSource killSound;

    public event Action onBeingHit;
    private ShieldEffect shield;
    PlayerController pc;


    void Start() {
        pc=FindObjectOfType<PlayerController>();
    }
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

    private void OnCollisionStay2D(Collision2D other) {
        shield = gameObject.GetComponent<ShieldEffect>();
        if (transform.childCount > 0)
        {
            if (!shield.ActiveShield)
            {
                if (onBeingHit != null)
                {
                    onBeingHit();
                    gameObject.SetActive(false);
                }
            }

        }
        else if(onBeingHit!=null) {
            onBeingHit();
            killSound.Play();
            gameObject.SetActive(false);
        }
    }
    // Hit by ball
    private void OnCollisionEnter2D(Collision2D other) {
        // Debug.Log("OnCollisionEnter2D pc.showstartKilling-> "+ pc.showstartKilling); pc.showstartKilling && 

        
        shield = gameObject.GetComponent<ShieldEffect>();
        if (transform.childCount > 0)
        {
            if (!shield.ActiveShield)
            {
                if (onBeingHit != null)
                {

                    onBeingHit();
                    gameObject.SetActive(false);
                }
            }

        }
        else if(onBeingHit != null) {
            onBeingHit();
            killSound.Play();
            gameObject.SetActive(false);
        }
    }
}

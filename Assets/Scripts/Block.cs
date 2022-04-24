using UnityEngine;
using System;
public class Block : MonoBehaviour
{

    public AudioSource killSound;
    [SerializeField] ParticleSystem enemy_killed_animation = null;

    public event Action onBeingHit;
    public bool isMoving;
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
            if(isMoving){
                var contact = other.ClosestPoint(transform.position);
                // var contact = other.contacts[0].point;
                // Debug.Log("Contact Point: " + contact);
                enemy_killed_animation.transform.position = new Vector3(contact.x + 18.0f, contact.y, 0f); 
            }
            enemy_killed_animation.Play();
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
            if(isMoving){
                var contact = other.contacts[0].point;
                enemy_killed_animation.transform.position = new Vector3(contact.x + 18.0f, contact.y, 0f); 
            }
            enemy_killed_animation.Play();
            gameObject.SetActive(false);
        }
    }
}

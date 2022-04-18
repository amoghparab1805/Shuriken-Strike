using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    public GameObject shield;
    // public PlayerController pc;
    private bool activeShield;
    void Start()
    {
        activeShield = true;
        shield.SetActive(true);
        // GetComponent<PlayerVFX>()
        // pc = 
        // Debug.Log("PC------->"+pc.shieldDestroyer);
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("--"+ FindObjectOfType<PlayerController>().shieldDestroyer);
        if (other.gameObject.tag == "Player" && PlayerController.shieldDestroyer)
        {
            shield.SetActive(false);
            activeShield = false;
        }
    }

    public bool ActiveShield
    {
        get
        {
            return activeShield;
        }
        set
        {
            activeShield = value;
        }
    }
}

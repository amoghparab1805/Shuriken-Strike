using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveEnemyUpDown : MonoBehaviour
{
    public float delta = 2.0f; 
    public float speed = 1.0f;
    public Button FreezeButton;
    bool isFreezed;
    [SerializeField] ParticleSystem freeze_animation = null;
    Rigidbody2D rigidbody;
    private Vector3 startPos;

    public AudioSource freezeSound;

    void Start()
    {
        isFreezed = false;
        rigidbody = GetComponent<Rigidbody2D>();
        if(FreezeButton){
            FreezeButton.onClick.AddListener(freezeObjects);
        }
        startPos = transform.position;
    }

    void Update()
    {
        if(!isFreezed){
            Vector3 v = startPos;
            v.y += delta * Mathf.PingPong(Time.time * speed, 1);
            transform.position = v;
        }
        else{
            FreezeButton.interactable = false;
            FreezeButton.enabled = false;
        }
    }

    void freezeObjects()
    {
        freezeSound.Play();
        isFreezed = true;
        freeze_animation.Play();
    }
}

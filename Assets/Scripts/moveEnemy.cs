using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveEnemy : MonoBehaviour
{
    public float delta = 1.5f; 
    public float speed = 2.0f;
    private Vector3 startPos;
    public Button FreezeButton;
    bool isFreezed;
    [SerializeField] ParticleSystem freeze_animation = null;
    Rigidbody2D rigidbody;
    public AudioSource freezeSound;

    void Start()
    {
        isFreezed = false;
        if(FreezeButton){
            FreezeButton.onClick.AddListener(freezeObjects);
        }
        startPos = transform.position;
    }

    void Update()
    {
        if(!isFreezed){
            Vector3 v = startPos;
            v.x += delta * Mathf.Sin(Time.time * speed);
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

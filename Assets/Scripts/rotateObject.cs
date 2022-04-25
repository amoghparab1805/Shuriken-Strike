using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rotateObject : MonoBehaviour
{
    
    public int angle = -1;
    public float rotateSpeed = 80f;
    [SerializeField] ParticleSystem freeze_animation = null;
    public AudioSource freezeSound;

    public Button FreezeButton;
    bool isFreezed;
    Rigidbody2D rigidbody;
    private Vector3 startPos;

    void Start()
    {
        isFreezed = false;
        if(FreezeButton){
            FreezeButton.onClick.AddListener(freezeObjects);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isFreezed){
            RotateLeft();    
        }
        else{
            FreezeButton.interactable = false;
            FreezeButton.enabled = false;
        }
    }
    void RotateLeft()
    {
        // transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
        transform.RotateAround(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.forward, Time.deltaTime * -rotateSpeed);
    }
    // void OnCollisionEnter2D(Collision2D c)
    // {
    //     angle = 0;
    // }
    void freezeObjects()
    {
        freezeSound.Play();
        freeze_animation.Play();
        isFreezed = true;
    }
}

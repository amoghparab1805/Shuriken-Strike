using UnityEngine;
using System;
public class PlayerController : MonoBehaviour
{
    public InputData inputData;

    public LayerMask collideWithLayer;
    public float moveSpeed = 20f;
    public float hitPoint = 5f;
    public event Action onMouseClick;
    Vector3 clickedPosVector;
    Vector3 releasedPosVector;
    Vector3 directionPosVector;
    Camera mainCam;
    Rigidbody2D rigidbody2D;
    
    bool hitBlock;

    PlayerVFX playerVFX;
    
    void Start()
    {
        //changePlayerState(false);
        getComponents();
    }

    void getComponents() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        mainCam = FindObjectOfType<Camera>();
        playerVFX = GetComponent<PlayerVFX>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Update");
        handleMovement();
    }

    void handleMovement() {
        if(inputData.isPressed) {
           // changePlayerState(true);
            Debug.Log("isPressed ");
            hitBlock = checkIfHitBlock();
            if(hitBlock) {
                // changePlayerState(false);
                return;
            }

            clickedPosVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
            clickedPosVector = new Vector3(clickedPosVector.x, clickedPosVector.y, 0f);
            resetPlayerPosition();
            playerVFX.changeActiveDots(true);
            playerVFX.changeTrailState(false, 0f);
            Debug.Log("isPressed");

            onMouseClick?.Invoke();
        }

        if(inputData.isHeld) {
            hitBlock = checkIfHitBlock();
            if(hitBlock) return;
            playerVFX.setDotPosition(clickedPosVector, mainCam.ScreenToWorldPoint(Input.mousePosition));
        }

        if(inputData.isReleased) {
            // hitBlock = checkIfHitBlock();
            // if(hitBlock) {
            //     playerVFX.changeActiveDots(false);
            //     return;
            // }
            Debug.Log("isReleased "+clickedPosVector);
            // if()

            releasedPosVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
            releasedPosVector = new Vector3(releasedPosVector.x, releasedPosVector.y, 0f);
            playerVFX.changeActiveDots(false);
            calculateDirection();
            playerVFX.changeTrailState(true, 0.75f);
            movePlayerInDirection();
        }
    }
    void calculateDirection() {
        directionPosVector = (releasedPosVector-clickedPosVector).normalized;

    }

    void movePlayerInDirection() {
        rigidbody2D.velocity = directionPosVector*moveSpeed;
    }

    void resetPlayerPosition() {
        transform.position = clickedPosVector;
        rigidbody2D.velocity = Vector3.zero;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Wall")) {
            Vector2 wallNormal = other.contacts[0].normal;
            directionPosVector = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

            rigidbody2D.velocity = directionPosVector*moveSpeed;
            
        }
    }

    bool checkIfHitBlock() {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitBlock = Physics2D.Raycast(ray.origin, ray.direction, 100f, collideWithLayer);
        return hitBlock;
    }

    void changePlayerState(bool state) {
        // gameObject.visibility = state;
    }
    
}

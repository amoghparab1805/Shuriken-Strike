using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public event Action OnMouseClick;

    public InputData inputData;
    public BlockManager bm;
    public GameObject gameObj;
    public LayerMask collideWithLayer;
    public float moveSpeed = 200f;
    public float hitPoint = 5f;
    public GameObject bullet;
    public GameObject bulletdown;
    public Vector3 lastvelocity;
    Vector3 clickedPosVector;
    Vector3 releasedPosVector;
    Vector3 directionPosVector;
    Camera mainCam;
    Rigidbody2D rigidbody2D;
    public bool velocity = false;
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
        lastvelocity = rigidbody2D.velocity;
        // Debug.Log("Update");
        handleMovement();
    }

    void handleMovement() {
        if(inputData.isPressed) {
            hitBlock = checkIfHitBlock();
            if(hitBlock) {
                return;
            }

            clickedPosVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
            clickedPosVector = new Vector3(clickedPosVector.x, clickedPosVector.y, 0f);
            resetPlayerPosition();
            playerVFX.changeActiveDots(true);
            playerVFX.changeTrailState(false, 0f);
            OnMouseClick?.Invoke();
        }

        if(inputData.isHeld) {
            hitBlock = checkIfHitBlock();
            if(hitBlock) {

                return;
            }
            playerVFX.setDotPosition(clickedPosVector, mainCam.ScreenToWorldPoint(Input.mousePosition));
        }

        if(inputData.isReleased) {
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
        if(directionPosVector*moveSpeed == Vector3.zero) {
            velocity=false;
            return;
        }
        velocity=true;
        rigidbody2D.velocity = directionPosVector*moveSpeed;
    }

    void resetPlayerPosition() {
        transform.position = clickedPosVector;
        rigidbody2D.velocity = Vector3.zero;
    }

    public void shootup(float x, float y)
    {
        GameObject b = Instantiate(bullet) as GameObject;
        b.transform.position = new Vector3(x, y, 0f);
    }

    public void shootdown(float x, float y)
    {
        GameObject b = Instantiate(bulletdown) as GameObject;
        b.transform.position = new Vector3(x, y, 0f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Wall"))
        {
            Vector2 wallNormal = other.contacts[0].normal;
            directionPosVector = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

            rigidbody2D.velocity = directionPosVector * moveSpeed;

        }
        if (other.gameObject.tag == "Powerup")
        {
            Destroy(other.gameObject);
            shootup(other.transform.position.x, other.transform.position.y);
            shootdown(other.transform.position.x, other.transform.position.y);
            var speed = lastvelocity.magnitude;
            var direction = lastvelocity.normalized;
            rigidbody2D.velocity = direction * Mathf.Max(speed, 0f);
        }

        if (other.gameObject.tag == "obstacle")
        {
            var speed = lastvelocity.magnitude;
            var direction = Vector3.Reflect(lastvelocity.normalized, other.contacts[0].normal);
            rigidbody2D.velocity = direction * Mathf.Max(speed, 0f);
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

    void OnBecameInvisible() {
        
            
        if(!ResetBtn.quitGame){
            if(BlockManager.blockCount>0){
                Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if(ResetBtn.quitGame){
            ResetBtn.quitGame = false;
        }


    }
    
}

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
           // changePlayerState(true);
            // Debug.Log("isPressed ");
            // gameObj.active = true;

            hitBlock = checkIfHitBlock();
            if(hitBlock) {
                // changePlayerState(false);
                return;
            }

            clickedPosVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
            clickedPosVector = new Vector3(clickedPosVector.x, clickedPosVector.y, 0f);
            // resetPlayerPosition();
            // playerVFX.changeActiveDots(true);
            // playerVFX.changeTrailState(false, 0f);
            // Debug.Log("isPressed");

            OnMouseClick?.Invoke();
        }

        if(inputData.isHeld) {
            
            hitBlock = checkIfHitBlock();
            Debug.Log("Hit Block" +hitBlock);
            if(hitBlock) {
                Debug.Log("Hit Block");
                return;
            }
            Vector3 held = new Vector3(mainCam.ScreenToWorldPoint(Input.mousePosition).x, mainCam.ScreenToWorldPoint(Input.mousePosition).y, 0f);
            // Debug.Log("Moved "++" Clicked "+ clickedPosVector);
            // Vector2 
            if(Vector3.Distance(held, clickedPosVector)>8) {
                resetPlayerPosition();
                playerVFX.changeActiveDots(true);
                playerVFX.changeTrailState(false, 0f);
            }
            // Debug.Log(clickedPosVector);
            playerVFX.setDotPosition(clickedPosVector, mainCam.ScreenToWorldPoint(Input.mousePosition));
        }

        if(inputData.isReleased) {
            // hitBlock = checkIfHitBlock();
            // if(hitBlock) {
            //     playerVFX.changeActiveDots(false);
            //     return;
            // }
            // Debug.Log("isReleased "+clickedPosVector);
            // if()

            releasedPosVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
            releasedPosVector = new Vector3(releasedPosVector.x, releasedPosVector.y, 0f);
            playerVFX.changeActiveDots(false);
            calculateDirection();
            playerVFX.changeTrailState(true, 0.75f);
            if(Vector3.Distance(releasedPosVector, clickedPosVector)>8) {
                movePlayerInDirection();
            }
            // movePlayerInDirection();
        }
    }
    void calculateDirection() {
        directionPosVector = (releasedPosVector-clickedPosVector).normalized;

    }

    void movePlayerInDirection() {
        if(directionPosVector*moveSpeed == Vector3.zero) {
            // gameObj.active = false;
            // playerVFX.changeActiveDots(false);
            // playerVFX.changeTrailState(false, 0f);
            // transform.position =new Vector3(1000, 1000, 0f);
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
    public void shootup(float x)
    {
        GameObject b = Instantiate(bullet) as GameObject;
        b.transform.position = new Vector3(x, 270f, 0f);
        // Debug.Log(b.transform.position);
    }

    public void shootdown(float x)
    {
        GameObject b = Instantiate(bulletdown) as GameObject;
        b.transform.position = new Vector3(x, 250f, 0f);
        // Debug.Log(b.transform.position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(velocity+" JDL");
        // if(!velocity) return;
        if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("JDL");
            Vector2 wallNormal = other.contacts[0].normal;
            directionPosVector = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

            rigidbody2D.velocity = directionPosVector * moveSpeed;

        }
        if (other.gameObject.tag == "Powerup")
        {
            // Debug.Log("JDL "+);
            float x = other.gameObject.transform.position.x;
            Destroy(other.gameObject);
            shootup(x);
            shootdown(x);
            var speed = lastvelocity.magnitude;
            var direction = lastvelocity.normalized;
            rigidbody2D.velocity = direction * Mathf.Max(speed, 0f);
        }

        if (other.gameObject.tag == "obstacle")
        {
            var speed = lastvelocity.magnitude;
            var direction = Vector3.Reflect(lastvelocity.normalized, other.contacts[0].normal);
            rigidbody2D.velocity = direction * Mathf.Max(speed, 0f);
            // rigidbody2D.velocity = new Vector2(0, 0);
            //gameObject.transform.parent = c.gameObject.transform;
            // FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            // joint.anchor = other.contacts[0].point;
            // joint.anchor = new Vector2(0, 0);
            // Debug.Log("Obstacle");
            // Debug.Log(other.contacts[0].point);
            // joint.connectedBody = other.gameObject.transform.GetComponentInParent<Rigidbody2D>();
            // joint.connectedAnchor = new Vector2(-0.5f, 0);
            // joint.enableCollision = false;
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
                Debug.Log("PPPPPPPPPPPPPPPPPPPPP");
                Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if(ResetBtn.quitGame){
            Debug.Log("QQQQQQQQQ");
            ResetBtn.quitGame = false;
        }


    }
    
}

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
    CircleCollider2D circleCollider;
    public bool showstartKilling = false;
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
        circleCollider = GetComponent<CircleCollider2D>();
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
            // showstartKilling=false;
           // changePlayerState(true);
            // Debug.Log("isPressed ");
            // gameObj.active = true;
            circleCollider.isTrigger=true;
            hitBlock = checkIfHitBlock();
            if(hitBlock) {
                return;
            }

            clickedPosVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
            clickedPosVector = new Vector3(clickedPosVector.x, clickedPosVector.y, 0f);
            // resetPlayerPosition();
            // playerVFX.changeActiveDots(true);
            // playerVFX.changeTrailState(false, 0f);
            // Debug.Log("isPressed");
            if(clickedPosVector.x == 0 || clickedPosVector.y == 0) return;
            if(showstartKilling) {
                Debug.Log("AM Love Ell");
                OnMouseClick?.Invoke();
            }
            // OnMouseClick?.Invoke();
            // if(BlockManager.resetBlockCalled){
            //     transform.position = new Vector3(clickedPosVector.x, clickedPosVector.y, 0f);
            //     rigidbody2D.velocity = Vector3.zero;
            // }
        }

        if(inputData.isHeld) {
            hitBlock = checkIfHitBlock();
            // Debug.Log("Hit Block" +hitBlock);
            if(hitBlock) {
                // Debug.Log("Hit Block");
                return;
            }
            Vector3 held = new Vector3(mainCam.ScreenToWorldPoint(Input.mousePosition).x, mainCam.ScreenToWorldPoint(Input.mousePosition).y, 0f);
            // Debug.Log("Moved "++" Clicked "+ clickedPosVector);
            // Vector2 
            if(clickedPosVector.x == 0 || clickedPosVector.y == 0) return;
            if(Vector3.Distance(held, clickedPosVector)>8) {
                resetPlayerPosition();
                playerVFX.changeActiveDots(true);
                playerVFX.changeTrailState(false, 0f);
            }
            // Debug.Log(clickedPosVector);
            playerVFX.setDotPosition(clickedPosVector, mainCam.ScreenToWorldPoint(Input.mousePosition));
        }

        if(inputData.isReleased) {
            releasedPosVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
            releasedPosVector = new Vector3(releasedPosVector.x, releasedPosVector.y, 0f);
            playerVFX.changeActiveDots(false);
            calculateDirection();
            circleCollider.isTrigger=false;
            playerVFX.changeTrailState(true, 0.75f);
            if(clickedPosVector.x == 0 || clickedPosVector.y == 0) return;
            if(Vector3.Distance(releasedPosVector, clickedPosVector)>8) {
                showstartKilling=true;
                
                movePlayerInDirection();
            }
            else
                showstartKilling=false;
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
            // v=false;
            return;
        }
        // v=true;
        rigidbody2D.velocity = directionPosVector*moveSpeed;
    }

    public void resetPlayerPosition() {
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
        // Debug.Log(velocity+" JDL");
        // if(!velocity) return;
        if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Wall"))
        {
            // Debug.Log("JDL");
            Vector2 wallNormal = other.contacts[0].normal;
            directionPosVector = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

            rigidbody2D.velocity = directionPosVector * moveSpeed;

        }
        if (other.gameObject.tag == "Powerup")
        {
            // Debug.Log("JDL "+);
            float x = other.gameObject.transform.position.x;
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

using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public event Action OnMouseClick;
    // public Animator powerup_animation;

    public InputData inputData;
    public BlockManager bm;
    public GameObject gameObj;
    public LayerMask collideWithLayer;
    public LayerMask collideWithWall;
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
    bool hitWall;
    bool ifBallCanMove = false;

    bool enterOne=false;
    bool enterWall = false;
    PlayerVFX playerVFX;

    public Button AdditionalShot;

    bool isAddShot;
    public static int playerCount;

    public static bool isShieldUp = true;
    public static bool shieldDestroyer = false;
    
    void Start()
    {
        changePlayerState(false);
        getComponents();
        playerCount = 1;
        isAddShot = false;
        if(AdditionalShot){
            AdditionalShot.onClick.AddListener(addPlayer);
        }
    }

    void addPlayer(){
        isAddShot = true;
        playerCount = 2;
        // Debug.Log("Player Count Clicked: " + playerCount);
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
        if(isAddShot){
            AdditionalShot.interactable = false;
            AdditionalShot.enabled = false;
        }
        lastvelocity = rigidbody2D.velocity;
        // Debug.Log("Update");
        handleMovement();
    }

    void handleMovement() {
        if(inputData.isPressed) {
            // showstartKilling=false;
           changePlayerState(true);
            // Debug.Log("isPressed ");
            // gameObj.active = true;
             circleCollider.enabled = false;

            // circleCollider.isTrigger=true;
            // Debug.Log("It's a collider "+true);
            hitBlock = checkIfHitBlock();
            hitWall = checkIfHitWall();
            Debug.Log("Hit Wall"+hitWall);
            if(hitBlock || hitWall) {
                ifBallCanMove=false;
                // Debug.Log("Hit Block");
                return;
            } else {
                ifBallCanMove=true;
            }

            clickedPosVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
            clickedPosVector = new Vector3(clickedPosVector.x, clickedPosVector.y, 0f);
            // resetPlayerPosition();
            playerVFX.changeActiveDots(true);
            playerVFX.changeTrailState(false, 0f);
            // Debug.Log("isPressed");
            if(clickedPosVector.x == 0 || clickedPosVector.y == 0) return;
            if(showstartKilling) {
                
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
            hitWall = checkIfHitWall();
            Debug.Log("Hit Wall"+hitWall);
            if(hitBlock || hitWall) {
                // Debug.Log("Hit Block");
                ifBallCanMove=false;
                return;
            } else {
                ifBallCanMove=true;
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
            
            // Debug.Log("Done Jay bbay girl"+clickedPosVector);
            // Debug.Log(Vector3.Distance(releasedPosVector, clickedPosVector));
            playerVFX.changeActiveDots(false);
            calculateDirection();
            // circleCollider.isTrigger=false;
            // Debug.Log("circleCollider trigger is false now");
            playerVFX.changeTrailState(true, 0.75f);
            if(clickedPosVector.x == 0 || clickedPosVector.y == 0) return;
            if(Vector3.Distance(releasedPosVector, clickedPosVector)>8) {
                showstartKilling=true;
                 circleCollider.enabled = true;


                Debug.Log("isKinematic true");
                // circleCollider.isTrigger=false;
                // Debug.Log("It's a /collider "+false);
                movePlayerInDirection();
                // if(ifBallCanMove) {
                //     movePlayerInDirection();
                // } else {
                //     showstartKilling=false;
                // circleCollider.enabled = false;
                // transform.position = new Vector3(clickedPosVector.x, clickedPosVector.y, -100f);
                // }
                
            }
            else {
                showstartKilling=false;
                circleCollider.enabled = false;
                transform.position = new Vector3(clickedPosVector.x, clickedPosVector.y, -100f);
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
            playerVFX.changeActiveDots(false);
            playerVFX.changeTrailState(false, 0f);
            // transform.position =new Vector3(1000, 1000, 0f);
            // v=false;
            return;
        }
        // v=true;
        rigidbody2D.velocity = -1*directionPosVector*moveSpeed;
        // rigidbody2D.velocity = directionPosVector*moveSpeed;
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

    void OnCollisionStay2D(Collision2D other) {
        Debug.Log("OnCollisionStay2D im here");
        if(!enterOne) {
            if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Wall"))
        {
            // Debug.Log("JDL");
            Vector2 wallNormal = other.contacts[0].normal;
            directionPosVector = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

            rigidbody2D.velocity = directionPosVector * moveSpeed;

        }
        if (other.gameObject.tag == "Powerup")
        {
            BlockManager.increasePowerUpCount();
            float x = other.gameObject.transform.position.x;
            //Add animation here
            // Debug.Log("Setting trigger");
            // powerup_animation.SetTrigger("powerup_animation_trigger");

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

        if(other.gameObject.tag == "shieldDestroy")
        {
            shieldDestroyer = true;
            // var speed = lastvelocity.magnitude;
            // var direction = lastvelocity.normalized;
            // rigidbody2D.velocity = direction * moveSpeed;
        }

        if(other.gameObject.tag == "shield")
        {
            if (isShieldUp && !shieldDestroyer)
            {
                var direction = lastvelocity.normalized;
                rigidbody2D.velocity = direction * 0f;
                Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
                isShieldUp=true;
            } else {
                if(shieldDestroyer) {
                    isShieldUp=false;
                    Vector2 wallNormal = other.contacts[0].normal;
                    directionPosVector = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

                    rigidbody2D.velocity = directionPosVector * moveSpeed;
                }
            }
            
        }
        Debug.Log("OnCollisionStay2D from player");
        } else {
            // if(other.gameObject.CompareTag("Wall") && !enterWall) {
            //     enterWall=true;
            //     Vector2 wallNormal = other.contacts[0].normal;
            //     directionPosVector = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

            //     rigidbody2D.velocity = directionPosVector * moveSpeed;
            // }
        }
        
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("OnCollisionEnter2D from player "+other.gameObject.tag);
        enterOne=true;
        if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall is on boisssssssssss");
            // if(other.gameObject.CompareTag("Wall")) enterWall=true;
            Vector2 wallNormal = other.contacts[0].normal;
            directionPosVector = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

            rigidbody2D.velocity = directionPosVector * moveSpeed;

        }
        if (other.gameObject.tag == "Powerup")
        {
            BlockManager.increasePowerUpCount();
            float x = other.gameObject.transform.position.x;
            //Add animation here
            // Debug.Log("Setting trigger");
            // Debug.Log("Setting trigger" + powerup_animation.GetBool("anim"));
            // // powerup_animation.SetTrigger("powerup_animation_trigger");
            // // powerup_animation.SetBool("anim", true);
            // powerup_animation.SetTrigger("trig");
            // Debug.Log("Setting trigger"+powerup_animation.GetBool("anim"));
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

        if(other.gameObject.tag == "shieldDestroy")
        {
            shieldDestroyer = true;
            // var speed = lastvelocity.magnitude;
            // var direction = lastvelocity.normalized;
            // rigidbody2D.velocity = direction * Mathf.Max(speed, 0f);
        }

        if(other.gameObject.tag == "shield")
        {
            if (isShieldUp && !shieldDestroyer)
            {
                var direction = lastvelocity.normalized;
                rigidbody2D.velocity = direction * 0f;
                Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
            } else {
                if(shieldDestroyer) {
                    isShieldUp=false;
                    Vector2 wallNormal = other.contacts[0].normal;
                    directionPosVector = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

                    rigidbody2D.velocity = directionPosVector * moveSpeed;
                }
            }
            
        }
    }

    

    bool checkIfHitBlock() {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitBlock = Physics2D.Raycast(ray.origin, ray.direction, 100f, collideWithLayer);
        // Debug.Log(hitBlock);
        return hitBlock;
    }

    bool checkIfHitWall() {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitBlock = Physics2D.Raycast(ray.origin, ray.direction, 100f, collideWithWall);
        // Debug.Log(hitBlock);
        return hitBlock;
    }

    void changePlayerState(bool state) {
        // gameObject.visibility = state;
    }

    void OnBecameInvisible() {

        // Debug.Log("Player Count: " + playerCount);
        // playerCount -= 1;
        // Debug.Log("Player Count U: " + playerCount);
        isShieldUp=true;
        shieldDestroyer=false;
        if(playerCount > 1){
            if(ResetBtn.quitGame){
                // Debug.Log("QQQQQQQQQ");
                ResetBtn.quitGame = false;
            }
        }
        else{
            
            if(!ResetBtn.quitGame){
                if(BlockManager.blockCount>0){
                    // Debug.Log(BlockManager.blockCount);
                    // Debug.Log("PPPPPPPPPPPPPPPPPPPPP");

                    BlockManager.send_level_enemy_killed();
                    // BlockManager.send_level_completion_time();

                    if (BlockManager.pup){
                        BlockManager.send_power_ups_used();
                        BlockManager.pup=false;
                    }
                    BlockManager.send_which_enemy_killed();

                    playerCount = 1;

                    Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
                    
                }
            }
            else if(ResetBtn.quitGame){
                // Debug.Log("QQQQQQQQQ");
                ResetBtn.quitGame = false;
            }
        }
    }
    
}

using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public event Action OnMouseClick;

    public InputData inputData;
    public BlockManager bm;

    public LayerMask collideWithLayer;
    public float moveSpeed = 20f;
    public float hitPoint = 5f;
    public GameObject bullet;
    public GameObject bulletdown;
    public Vector3 lastvelocity;
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
        lastvelocity = rigidbody2D.velocity;
        // Debug.Log("Update");
        handleMovement();
    }

    void handleMovement() {
        if(inputData.isPressed) {
           // changePlayerState(true);
            // Debug.Log("isPressed ");
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
            // Debug.Log("isPressed");

            OnMouseClick?.Invoke();
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
            // Debug.Log("isReleased "+clickedPosVector);
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
    public void shootup()
    {
        GameObject b = Instantiate(bullet, new Vector3(290f, 270f, 0f), Quaternion.identity) as GameObject;
        //b.transform.position = new Vector3(290f, 270f, 0f);
        // Debug.Log(b.transform.position);
    }

    public void shootdown()
    {
        GameObject b = Instantiate(bulletdown) as GameObject;
        //b.transform.position = new Vector3(290f, 250f, 0f);
        // Debug.Log(b.transform.position);
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
            shootup();
            shootdown();
            var speed = lastvelocity.magnitude;
            var direction = lastvelocity.normalized;
            rigidbody2D.velocity = direction * Mathf.Max(speed, 0f);
            // count --;
            // if(count == 0){
            //     // rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //     // circle.transform.localScale = new Vector3(0, 0, 0);
            // }

            Vector3 pos1 = new Vector3(-2.492358f, 3.471171f, 0f);
            Vector3 pos2 = new Vector3(-2.514237f, -2.479917f, 0f);

            // GameObject[] objs = GameObject.FindSceneObjectsOfType(typeof(GameObject));
            GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();

            // foreach (GameObject go in objs) {
            //     if (go.transform.position == pos1 || go.transform.position == pos2) {
            //         Destroy(go);
            //     }
            // }

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
        Debug.Log("I`m gone :(");
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

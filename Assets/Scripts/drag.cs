using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class drag : MonoBehaviour
{
    public float power = 3f;
    public Rigidbody2D rb;
    public Transform t;
    public Vector3 startpoint;
    public Vector3 endpoint;
    public Vector2 maxpower;
    public Vector2 minpower;
    public Vector3 lastvelocity;
    public GameObject bullet;
    public GameObject bulletdown;
    public int isShot = 0;
    Camera cam;
    Vector2 force;
    TrajectoryLine tl;
    public GameObject circle;
    int count = 4;

    private void Start(){
        cam = Camera.main;
        tl = GetComponent<TrajectoryLine>();
        circle.transform.localScale = new Vector3(0, 0, 0);
    }
    private void Update(){
        lastvelocity = rb.velocity;

        if (Input.GetKeyDown(KeyCode.R))
         {
             Application.LoadLevel(3);
         }

        if(Input.GetMouseButtonDown(0)){
            startpoint = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(0)){
            Vector3 curr_point = cam.ScreenToWorldPoint(Input.mousePosition);
            curr_point.z = 15;
            tl.RenderLine(startpoint, curr_point);
        }
        if(Input.GetMouseButtonUp(0)){
            endpoint = cam.ScreenToWorldPoint(Input.mousePosition);
            force = new Vector2(Mathf.Clamp(startpoint.x - endpoint.x, minpower.x, maxpower.x), Mathf.Clamp(startpoint.y - endpoint.y, minpower.y, maxpower.y));
            if(isShot == 0){
                isShot = 1;
                circle.transform.position = new Vector3(startpoint.x, startpoint.y, 15);
                circle.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                rb.AddForce(force.normalized * power, ForceMode2D.Impulse);
            }
            tl.EndLine();
        }
    }


    public void nextLevel(){
        // Debug.Log("Next level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void shootup(){
        GameObject b = Instantiate(bullet) as GameObject;
        b.transform.position = new Vector3(-2.48f, 0.48f, 0f);
    }

    public void shootdown(){
        GameObject b = Instantiate(bulletdown) as GameObject;
        b.transform.position = new Vector3(-2.48f, 0.48f, 0f);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Wall")
        {
            // rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Application.LoadLevel(1);
        }
        if(c.gameObject.tag == "Powerup"){
            Destroy(c.gameObject);
            shootup();
            shootdown();
            var speed = lastvelocity.magnitude;
            var direction = lastvelocity.normalized;
            rb.velocity = direction * Mathf.Max(speed, 0f);
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
        if(c.gameObject.tag == "Easy_Enemy"){
            Destroy(c.gameObject);
            var speed = lastvelocity.magnitude;
            var direction = lastvelocity.normalized;
            rb.velocity = direction * Mathf.Max(speed, 0f);
            count --;
            // Debug.Log("count = " + count);
            if(count == 0){
                nextLevel();
                // rb.constraints = RigidbodyConstraints2D.FreezeAll;
                // circle.transform.localScale = new Vector3(0, 0, 0);
            }
        }
        if (c.gameObject.tag == "Enemy")
        {
            var speed = lastvelocity.magnitude;
            var direction = Vector3.Reflect(lastvelocity.normalized, c.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speed, 0f);
            Destroy(c.gameObject);
            count --;
            // Debug.Log("count = " + count);
            if(count == 0){
                nextLevel();
                // rb.constraints = RigidbodyConstraints2D.FreezeAll;
                // circle.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        if (c.gameObject.tag == "obstacle")
        {
            //var speed = lastvelocity.magnitude;
            //var direction = Vector3.Reflect(lastvelocity.normalized, c.contacts[0].normal);
            //rb.velocity = direction * Mathf.Max(speed, 0f);
            rb.velocity = new Vector2(0,0);
            //gameObject.transform.parent = c.gameObject.transform;
            FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            joint.anchor = c.contacts[0].point;
            joint.connectedBody = c.gameObject.transform.GetComponentInParent<Rigidbody2D>();
            joint.enableCollision = false;
        }
    }
}

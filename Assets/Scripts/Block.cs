using UnityEngine;
using System;
public class Block : MonoBehaviour
{

    public event Func<int, bool> onBeingHit;
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D other) {
        if(onBeingHit != null) {
            // onBeingHit(gameObject.GetInstanceID());
            if(onBeingHit(gameObject.GetInstanceID())) {
                gameObject.SetActive(false);
            }
        }
            
        // }
        // Debug.Log("gameObject "+ gameObject.GetInstanceID());

        // gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

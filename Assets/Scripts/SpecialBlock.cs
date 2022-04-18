using UnityEngine;
using System;

public class SpecialBlock : MonoBehaviour
{

    public event Action onBeingHit;
    
    private ShieldEffect shield;
    
    // private void OnCollisionEnter2D(Collision2D other) {
    //     shield = gameObject.GetComponent<ShieldEffect>();
    //     if (transform.childCount > 0)
    //     {
    //         if (!shield.ActiveShield)
    //         {
    //             if (onBeingHit != null)
    //             {
    //                 onBeingHit();
    //                 gameObject.SetActive(false);
    //             }
    //         }

    //     }
    // }
}

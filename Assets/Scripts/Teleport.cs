using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform destination;
    // [SerializeField] int hello;

    public Transform getDestination() {
        return destination;
    }
}

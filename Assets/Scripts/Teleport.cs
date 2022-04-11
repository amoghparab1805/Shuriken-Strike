using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public AudioSource teleportationSound;
    public Transform destination;
    // [SerializeField] int hello;

    public Transform getDestination() {
        teleportationSound.Play();
        return destination;
    }
}

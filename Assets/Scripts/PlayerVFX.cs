using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{

    public GameObject dotPrefab;
    public int dotAmount;

    GameObject[] dotArray;
    float dotGap;
    TrailRenderer trailRenderer;
    public SpriteRenderer spriteRenderer;
    void Start()
    {
        dotGap = 1f/dotAmount;
        spawnDots();
        getComponents();
    }
    void spawnDots() {
        dotArray = new GameObject[dotAmount];

        for(int i=0; i<dotAmount; i+=1) {
            GameObject dot = Instantiate(dotPrefab);
            dot.SetActive(false);
            dotArray[i]=dot;
        }
    }

    public void setDotPosition(Vector3 startPos, Vector3 endPos) {
        // Debug.Log("startPos "+startPos);
        // Debug.Log("endPos "+endPos);
        for(int i=0; i<dotAmount; i++) {
            Vector3 targetPos = Vector2.Lerp(startPos, 2*startPos-endPos, i*dotGap);
            dotArray[i].transform.position = targetPos;
        }
    }

    public void changeActiveDots(bool state) {
        for(int i=0; i<dotAmount; i++) {
            dotArray[i].SetActive(state);
        }
    }

    void getComponents() {
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    public void changeTrailState(bool emitting, float time) {
        trailRenderer.emitting = false;
        trailRenderer.time = time;
    }
}

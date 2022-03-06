using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlockManager : MonoBehaviour
{

    public Block[] blockArray;
    AsyncOperation levelLoading;

    public Image animImg;
    public Animator anim;
    int[] hitPoints={5,5,5,5};
    [SerializeField] public static int blockCount;

    void Start() {
        blockArray = FindObjectsOfType<Block>();
        blockCount = blockArray.Length;
        SubscribeToEvent();
    }

    public void nextLevel(){
        int nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextSceneLoad == 10){
            Debug.Log("You Win!!");
        }
        else{
            levelLoading = SceneManager.LoadSceneAsync(nextSceneLoad);
            if (nextSceneLoad > PlayerPrefs.GetInt("lvlAt")){
                PlayerPrefs.SetInt("lvlAt", nextSceneLoad);
            }
            levelLoading.allowSceneActivation = false;
            StartCoroutine(waitForNextLevel());
        }
    }

    IEnumerator waitForNextLevel()
    {
        anim.SetBool("Fade", true);

        yield return new WaitUntil(()=>animImg.color.a==1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        levelLoading.allowSceneActivation = true;
    }

    void SubscribeToEvent() {
        foreach(Block block in blockArray) {
            block.onBeingHit+=decreseBlockCount;

        }

        FindObjectOfType<PlayerController>().OnMouseClick+=resetAllBlocks;
        // FindObjectOfType<PlayerController>().OnMouseClick+=resetLevel;
    }

    void resetLevel() {
        Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    void decreseBlockCount() {
        blockCount--;
        if(blockCount==0){
            nextLevel();
            return;
        }

        
    }
    PlayerController pc;
    public void resetAllBlocks() {
        foreach (Block block in blockArray) {
            if(block.gameObject.activeSelf == false) {
                block.gameObject.SetActive(true);
            }
        }
        blockCount=blockArray.Length;

     }

    private Block[] RemoveIndices(Block[] IndicesArray, int RemoveAt)
    {
        Block[] newIndicesArray = new Block[IndicesArray.Length - 1];

        int i = 0;
        int j = 0;
        while (i < IndicesArray.Length)
        {
            if (i != RemoveAt)
            {
                newIndicesArray[j] = IndicesArray[i];
                j++;
            }

            i++;
        }

        return newIndicesArray;
    }

}

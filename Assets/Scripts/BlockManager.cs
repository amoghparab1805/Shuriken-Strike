using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockManager : MonoBehaviour
{

    public Block[] blockArray;

    int[] hitPoints={5,5,5,5};

    public int blockCount;
    // Start is called before the first frame update

    void Start() {
        blockArray = FindObjectsOfType<Block>();
        // Debug.Log("blockArray "+ blockArray[i].]);
        blockCount = blockArray.Length;
        subscribeToEvent();
    }

    public void nextLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void subscribeToEvent() {
        foreach(Block block in blockArray) {
            block.onBeingHit+=decreseBlockCount;

        }

        FindObjectOfType<PlayerController>().onMouseClick+=resetAllBlocks;
    }
    bool decreseBlockCount(int id) {
        
        
        int i=0;
        for(i=0; i<blockCount; i+=1) {
            
            Block block = blockArray[i];
            
            if(block.gameObject.GetInstanceID()==id) {
                hitPoints[i]-=5;
                if(hitPoints[i]<=0) {
                    blockArray = RemoveIndices(blockArray, i);
                    blockCount-=1;
                    if(blockCount==0) nextLevel();
                    return true;
                } 
                break;
            }
            
            
        }
        
        return false;
    }
     void resetAllBlocks() {
         foreach(Block block in blockArray) {
             if(!block.gameObject.activeSelf) {
                 block.gameObject.SetActive(true);
             }

             blockCount=blockArray.Length;
         }
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

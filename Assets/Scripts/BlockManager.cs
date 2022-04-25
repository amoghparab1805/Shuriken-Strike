using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System;
public class BlockManager : MonoBehaviour
{

    public Block[] blockArray;
    public int winAmount;
    AsyncOperation levelLoading;
    public AudioSource freezeSound;

    public Image animImg;
    public Animator anim;
    // int[] hitPoints={5,5,5,5};
    public static bool resetBlockCalled = false;
    [SerializeField] public static int blockCount;

    public static long milliseconds;
    public static int coins;

    public static bool pup=false;

    int playerCount = PlayerController.playerCount;

    public static Dictionary<string, object> which_enemy_killed_dict = new Dictionary<string, object>();
    public static Dictionary<string, object> powerup_analytics = new Dictionary<string, object>();
    // PlayerController pc;

    void Start() {
        milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Debug.Log(milliseconds);
        blockArray = FindObjectsOfType<Block>();
        blockCount = blockArray.Length;
        int enemy_count=0;
        // Debug.Log("On Start");
        int level = SceneManager.GetActiveScene().buildIndex-1;
        which_enemy_killed_dict.Clear();
        which_enemy_killed_dict.Add("Level", SceneManager.GetActiveScene().buildIndex-1);
        // coins = PlayerPrefs.GetInt("totalCoins");

        foreach (Block b in blockArray) 
          {
            if(b.gameObject.name.StartsWith("l")){
                bool keyExists = which_enemy_killed_dict.ContainsKey(b.gameObject.name);
                if (keyExists) {
                    // Debug.Log("Skip");
                }
                else {
                    // Debug.Log(b.gameObject.name);
                    which_enemy_killed_dict.Add(b.gameObject.name, 0);
                }
            }
          }

        // if (level==15){
        //     powerup_analytics.Clear();
        //     // Debug.Log("In level 7");
        //     pup=true;
        //     powerup_analytics.Add("Level", SceneManager.GetActiveScene().buildIndex-1);
        //     powerup_analytics.Add("powerups_available", 2);
        //     powerup_analytics.Add("powerups_used", 0);
        // }

        if (level==15){
            powerup_analytics.Clear();
            // Debug.Log("In level 7");
            pup=true;
            powerup_analytics.Add("Level", SceneManager.GetActiveScene().buildIndex-1);
            powerup_analytics.Add("powerups_available", 1);
            powerup_analytics.Add("powerups_used", 0);
        }

        if (level==16){
            // Debug.Log("In level 8");
            powerup_analytics.Clear();
            pup=true;
            powerup_analytics.Add("Level", SceneManager.GetActiveScene().buildIndex-1);
            powerup_analytics.Add("powerups_available", 2);
            powerup_analytics.Add("powerups_used", 0);
        }

        if (level==19){
            // Debug.Log("In level 8");
            powerup_analytics.Clear();
            pup=true;
            powerup_analytics.Add("Level", SceneManager.GetActiveScene().buildIndex-1);
            powerup_analytics.Add("powerups_available", 1);
            powerup_analytics.Add("powerups_used", 0);
        }
        // Debug.Log(which_enemy_killed_dict.Count);
        SubscribeToEvent();
    }

    public void nextLevel(){
        // send_power_ups_used();
        send_level_enemy_killed();
        send_level_completion_time();
        send_which_enemy_killed();
        
        if (pup){
            send_power_ups_used();
            pup=false;
        }

        AfterLevelWin();

        // Debug.Log("Next Level");
        int nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextSceneLoad == 10){
            // Debug.Log("You Win!!");
            SceneManager.LoadScene(10);
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
            block.onBeingHit+=() => decreseBlockCount(block.gameObject.name);

        }

        // FindObjectOfType<PlayerController>().OnMouseClick+=resetAllBlocks;
        FindObjectOfType<PlayerController>().OnMouseClick+=resetLevel;
    }

    void Update(){
        playerCount = PlayerController.playerCount;
        // if(PlayerController.playerCount<=1){
        //     FindObjectOfType<PlayerController>().OnMouseClick+=resetLevel;
        // }
    }

    void resetLevel() {
        playerCount -= 1;
        PlayerController.playerCount = playerCount;
        // Debug.Log("Player COUNT Block Manager: " + playerCount);
        if(playerCount<1){
            send_level_enemy_killed();
            Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }
        PlayerController.isShieldUp=true;
        PlayerController.shieldDestroyer=false;
    }
    void decreseBlockCount(string s) {
        which_enemy_killed_dict[s]=1;

    foreach (KeyValuePair<string, object> kvp in which_enemy_killed_dict)
    {
    // Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
    }
        blockCount--;
        if(blockCount==0){
            nextLevel();
            return;
        }        
    }
    
    public void resetAllBlocks() {
        // foreach (Block block in blockArray) {
        //     if(block.gameObject.activeSelf == false) {
        //         block.gameObject.SetActive(true);
        //     }
        // }
        FindObjectOfType<PlayerController>().gameObject.SetActive(false);
        blockCount=blockArray.Length;
        resetBlockCalled = true;

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

    public static void increasePowerUpCount() {
        powerup_analytics["powerups_used"]=(int) powerup_analytics["powerups_used"]+1;
    }


    //Analytics Methods
    public static void send_which_enemy_killed() {
        // foreach (KeyValuePair<string, object> kvp in which_enemy_killed_dict)
        // {
        //     Debug.Log("----> Which ENEMIES KILLED: " + string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        // }

        AnalyticsResult ar = Analytics.CustomEvent("which_enemy_killed", which_enemy_killed_dict);
        Debug.Log(ar);
    }

    public static void send_level_completion_time(){
        long current_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        int send_time= (int) ((current_time-milliseconds)/1000);
        // Debug.Log("----> Sending Level END Time: " + send_time);

        
        AnalyticsResult ar = Analytics.CustomEvent("level_completion_time", new Dictionary<string, object>
        {
            { "level", SceneManager.GetActiveScene().buildIndex-1 },
            { "time_taken", send_time  }
        });
        Debug.Log(ar);

    }

    public static void send_level_enemy_killed(){
        int kill_count=0;
        foreach (KeyValuePair<string, object> kvp in which_enemy_killed_dict)
        {
            bool chk = Convert.ToBoolean(kvp.Value);
            if (chk){
                kill_count+=1;
            }
        }
        if(kill_count == 0){
            // Debug.Log("----> Enemies killed: " +  (0).ToString());
            AnalyticsResult ar = Analytics.CustomEvent("level_enemy_killed", new Dictionary<string, object>
            {
                { "level", SceneManager.GetActiveScene().buildIndex-1 },
                { "enemy_count", 0  }
            });
            Debug.Log(ar);
        }
        else{
            // Debug.Log("----> Enemies killed: " +  (kill_count - 1).ToString());
            AnalyticsResult ar = Analytics.CustomEvent("level_enemy_killed", new Dictionary<string, object>
            {
                { "level", SceneManager.GetActiveScene().buildIndex-1 },
                { "enemy_count", kill_count-1  }
            });
            Debug.Log(ar);
        }

        // AnalyticsResult ar = Analytics.CustomEvent("level_enemy_killed", new Dictionary<string, object>
        // {
        //     { "level", SceneManager.GetActiveScene().buildIndex-1 },
        //     { "enemy_count",kill_count-1  }
        // });
    }

    public static void send_power_ups_used(){
        // foreach (KeyValuePair<string, object> kvp in powerup_analytics)
        // {
        //     Debug.Log("----> PowerUp Used: " + string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        // }
    
    
        AnalyticsResult ar = Analytics.CustomEvent("level_completion_time", powerup_analytics);
        Debug.Log(ar);

    }

    public void AfterLevelWin() {
        PlayerController.playerCount = 1;
        coins = PlayerPrefs.GetInt("totalCoins") + 10;
        // coins += winAmount;
        PlayerPrefs.SetInt("totalCoins", coins);
        Debug.Log(coins);
    }

}

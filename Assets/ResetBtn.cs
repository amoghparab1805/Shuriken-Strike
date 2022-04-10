using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System;
// using Unity.Services.Analytics;
// using Unity.Services.Core;
// using Unity.Services.Core.Environments;

public class ResetBtn : MonoBehaviour
{
    public static bool quitGame = false;
    private void Start(){
        Debug.Log("quit game start function");
    }
    public void mainMenuStart(){
        Application.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void mainMenuQuit(){
        Application.Quit();
    }
    // Reset button
    public void hello(){


        // Start
        AnalyticsResult ar = Analytics.CustomEvent("level-reset", new Dictionary<string, object> {
            {"Level", (SceneManager.GetActiveScene().buildIndex - 1)}
        });

        // Sending the number of enemies killed
        BlockManager.send_level_enemy_killed();

        // Sending which enemy killed
        BlockManager.send_which_enemy_killed();
        
        if (BlockManager.pup){
            BlockManager.send_power_ups_used();
            BlockManager.pup=false;
        }

        // End
        Debug.Log(ar);


        Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void quit(){
        quitGame = true;
        AnalyticsResult ar = Analytics.CustomEvent("level-quit", new Dictionary<string, object> {
            {"Level", (SceneManager.GetActiveScene().buildIndex - 1)}
        });
        SceneManager.LoadScene(1);
        Debug.Log(ar);
    }
    
    public void quitAfterWin(){
        SceneManager.LoadScene(0);
    }
}

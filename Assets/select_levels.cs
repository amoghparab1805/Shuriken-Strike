using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class select_levels : MonoBehaviour
{
    // Start is called before the first frame update
    public Button[] lvlButtons;

    void Start(){
        int lvlAt = PlayerPrefs.GetInt("lvlAt", 2);
        for(int i=0; i<lvlButtons.Length; i++){
            if(i+2 > lvlAt){
                lvlButtons[i].interactable = false;
            }
        }
    }

    public void loadLevel(int lvl){
        SceneManager.LoadScene(lvl);
    }
}

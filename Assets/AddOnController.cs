using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOnController : MonoBehaviour
{
    public Button FreezeButton;
    // Start is called before the first frame update
    void Start()
    {
        int coins = PlayerPrefs.GetInt("totalCoins");
        if(coins>=10){
            FreezeButton.interactable = true;
        }
        else{
            FreezeButton.interactable = false;
        }
        FreezeButton.onClick.AddListener(() => updateCoins(10));
        // if(coins>=10){
        //     AddOn2.interactable = true;
        // }
        // else{
        //     AddOn2.interactable = false;
        // }
        // if(coins>=10){
        //     AddOn3.interactable = true;
        // }
        // else{
        //     AddOn3.interactable = false;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        int coins = PlayerPrefs.GetInt("totalCoins");
        if(coins>=10){
            FreezeButton.interactable = true;
        }
        else{
            FreezeButton.interactable = false;
        }
        // if(coins>=10){
        //     AddOn2.interactable = true;
        // }
        // else{
        //     AddOn2.interactable = false;
        // }
        // if(coins>=10){
        //     AddOn3.interactable = true;
        // }
        // else{
        //     AddOn3.interactable = false;
        // }
    }
    void updateCoins(int subAmount){
        int coins = PlayerPrefs.GetInt("totalCoins");
        Debug.Log(coins-subAmount);
        PlayerPrefs.SetInt("totalCoins", coins-subAmount);
    }
}

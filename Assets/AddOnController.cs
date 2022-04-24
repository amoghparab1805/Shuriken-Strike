using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOnController : MonoBehaviour
{
    public Button FreezeButton;
    public Button AddShotButton;
    public Button HintButton;
    public SpriteRenderer startingPoint;
    public SpriteRenderer startImage;
    public SpriteRenderer hereImage;
    public Image startArrow;
    // Start is called before the first frame update
    void Start()
    {
        int coins = PlayerPrefs.GetInt("totalCoins");
        HintButton.interactable = true;
        if(startingPoint.gameObject.activeSelf){
            startingPoint.gameObject.SetActive(true);
        }
        // startingPoint = GetComponent<SpriteRenderer>();
        // startingPoint.enabled = false;
        if(coins>=20){
            FreezeButton.interactable = true;
        }
        else{
            FreezeButton.interactable = false;
            FreezeButton.enabled = false;
        }
        FreezeButton.onClick.AddListener(() => updateCoins(20));
        
        if(coins>=40){
            AddShotButton.interactable = true;
        }
        else{
            AddShotButton.interactable = false;
            AddShotButton.enabled = false;
        }
        AddShotButton.onClick.AddListener(() => updateCoins(40));
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
        if(coins>=20){
            FreezeButton.interactable = true;
        }
        else{
            FreezeButton.interactable = false;
            FreezeButton.enabled = false;
        }

        HintButton.onClick.AddListener(showHint);
        
        if(coins>=40){
            AddShotButton.interactable = true;
        }
        else{
            AddShotButton.interactable = false;
            AddShotButton.enabled = false;
        }
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

    void showHint(){
        startingPoint.gameObject.SetActive(true);
        if(startImage && hereImage && startArrow){
            startImage.gameObject.SetActive(true);
            hereImage.gameObject.SetActive(true);
            startArrow.gameObject.SetActive(true);
        }
    }
}

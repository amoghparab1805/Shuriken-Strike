using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class coinCounter : MonoBehaviour
{
    TMP_Text counterText;

    // Start is called before the first frame update
    void Start()
    {
        counterText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //Set the current number of coins to display
        Debug.Log("HERE: " + counterText);
        Debug.Log("HERE: " + PlayerPrefs.GetInt("totalCoins"));
        if(counterText.text != PlayerPrefs.GetInt("totalCoins").ToString())
        {
            counterText.text = PlayerPrefs.GetInt("totalCoins").ToString();
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YouWinScreen : MonoBehaviour
{
    public void MainMenuButton(){
      SceneManager.LoadScene("MainMenu");
    }
}

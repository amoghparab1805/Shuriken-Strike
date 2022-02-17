using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetBtn : MonoBehaviour
{
    public void hello(){
        Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void quit(){
        SceneManager.LoadScene(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class select_levels : MonoBehaviour
{
    public void level_one(){
        SceneManager.LoadScene(2);
    }

    public void level_two(){
        SceneManager.LoadScene(3);
    }

    public void level_three(){
        SceneManager.LoadScene(4);
    }

    public void level_four(){
        SceneManager.LoadScene(5);
    }
}

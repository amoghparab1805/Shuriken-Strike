using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneSwitcher : MonoBehaviour
{
    AsyncOperation levelLoading;
    public void startGame(){
        // Debug.Log("Inside next level -- --");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public IEnumerator nextLevel(){
        // Debug.Log("Inside next level");
        levelLoading = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        levelLoading.allowSceneActivation = false;
        yield return new WaitForSeconds(5);
        levelLoading.allowSceneActivation = true;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void quitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}

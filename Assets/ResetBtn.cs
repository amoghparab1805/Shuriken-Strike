using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBtn : MonoBehaviour
{
    public void hello(){
        Debug.Log("Hello World");
        Application.LoadLevel(1);
    }
}

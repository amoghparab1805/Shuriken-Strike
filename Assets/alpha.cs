using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class alpha : MonoBehaviour
{
    public Image image;
    public float newAlpha;

    public void ChangeAlpha()
    {
        Color newColor = image.color;
        newColor.a = newAlpha;
        image.color = newColor;
    }
}

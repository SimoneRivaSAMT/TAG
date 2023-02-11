using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu1 : MonoBehaviour
{
    
    [SerializeField] private Image myImage;
    [SerializeField] private Color myColor;
    private void Start()
    {
        myColor.a = 1;
        myImage = GetComponent<Image>();
        
    }

    public void mouseOver()
    {
        myImage.color = new Color32(143, 0, 255, 255);
    }

    public void mouseNotOver()
    {
        myImage.color = new Color32(255, 255, 255, 1);
    }
}

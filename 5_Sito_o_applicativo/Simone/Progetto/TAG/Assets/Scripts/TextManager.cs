using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public TMP_InputField text;
    public GameObject button;
    ColorBlock cb;

    

    private void Start()
    {
        
    }
    void Update()
    {
        string lobbyName = text.text;
        if (text.text.Length > 0 && !string.IsNullOrWhiteSpace(text.text))
        {
            button.GetComponent<Button>().enabled = true;
            cb.normalColor = Color.white;
            cb.selectedColor = Color.white;
            cb.disabledColor = Color.white;
            cb.pressedColor = Color.white;
            cb.highlightedColor = Color.white;
            button.GetComponent<Button>().colors = cb;
            
        }
        else
        {
            button.GetComponent<Button>().enabled = false;
            cb = new ColorBlock();
            cb.normalColor = Color.gray;
            cb.colorMultiplier = 1;
            button.GetComponent<Button>().colors = cb;
        }
        lobbyName.Trim();
        lobbyName.Replace(" ", "_");
        if(!lobbyName.Contains(" ") && !lobbyName.Contains(""))
        {
            Debug.Log(lobbyName);
        }

    }
    
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateManager : MonoBehaviour
{
    public TMP_InputField text;
    public GameObject button;
    ColorBlock cb;

    string lobbyName;

    private void Start()
    {
        lobbyName = text.text;
    }
    void Update()
    {
        //Verifico se la stringa è vuota
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
    }

    public void ValueChanged()
    {
        lobbyName = SanizizeString(text.text);
    }

    //Tolgo dalla stringa i caratteri vietati
    public string SanizizeString(string text)
    {
        text = text.Replace(" ", "_");
        text = text.Replace("<", "");
        text = text.Replace(">", "");
        text = text.Replace("'", "");
        text = text.Replace("`", "");
        text = text.Replace("\"", "");
        text = text.Replace("[", "");
        text = text.Replace("]", "");
        text = text.Replace("{", "");
        text = text.Replace("}", "");
        text = text.Replace("\\", "");
        text = text.Replace("/", "");
        text = text.Replace(",", "");
        text = text.Replace(".", "");
        text = text.Replace("-", "");
        return text;
    }
}

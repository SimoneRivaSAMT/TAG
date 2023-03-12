using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public TMP_InputField ipAddressInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void StartAsHost()
    {
        PlayerPrefs.SetString("multiplayerMode", "host");
        SceneManager.LoadScene(1);
    }

    public void StartAsClient()
    {
        if(IsValidIP(ipAddressInput.text))
        {
            PlayerPrefs.SetString("multiplayerMode", "client");
            PlayerPrefs.SetString("connectIp", ipAddressInput.text);
            SceneManager.LoadScene(1);
        }
        else
        {
            ipAddressInput.image.color = Color.red;
        }
    }

    private bool IsValidIP(string Address)
    {
        //Match pattern for IP address    
        string Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
        //Regular Expression object    
        Regex check = new Regex(Pattern);

        //check to make sure an ip address was provided    
        if (string.IsNullOrEmpty(Address))
            //returns false if IP is not provided    
            return false;
        else
            //Matching the pattern    
            return check.IsMatch(Address, 0);
    }
}

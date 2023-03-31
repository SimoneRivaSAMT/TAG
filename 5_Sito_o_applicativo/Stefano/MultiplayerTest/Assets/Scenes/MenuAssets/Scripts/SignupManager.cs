using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignupManager : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField pass;
    public TMP_InputField nick;
    public GameObject container;
    public void SignUp()
    {
        StartCoroutine(InsertUserInDb());
    }

    private IEnumerator InsertUserInDb()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest signupRequest;
        form.AddField("user_nickname", nick.text);
        form.AddField("user_password", pass.text);
        form.AddField("user_email", email.text);
        signupRequest = UnityWebRequest.Post(FindObjectOfType<StartGame>().BASE_URL + "userManager/signup", form);
        yield return signupRequest.SendWebRequest();
        if(signupRequest.result == UnityWebRequest.Result.Success)
        {
            if (signupRequest.downloadHandler.text == "Done")
            {
                container.SetActive(false);
            }
            else
            {
                Debug.Log("Missing parameters");
            }
            
        }
    }
}

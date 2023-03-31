using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField passw;
    public TMP_InputField nickname;
    public GameObject loginContainer;
    public Button refresh;
    public Button createMatch;

    private string login_json;
    public void LogIn()
    {
        StartCoroutine(LoginRequest());
    }

    public void UnlockGame()
    {
        if (login_json.Contains("Id"))
        {
            Debug.Log(login_json);
            refresh.interactable = true;
            createMatch.interactable = true;
            User user = JsonUtility.FromJson<User>(login_json);
            nickname.text = user.Nickname;
            PlayerPrefs.SetString("user_nickname", user.Nickname);
            PlayerPrefs.SetString("user_email", user.Email);
            PlayerPrefs.SetInt("user_id", user.Id);
            loginContainer.SetActive(false);
        }
    }

    private IEnumerator LoginRequest()
    {
        UnityWebRequest loginRequest;
        WWWForm form = new WWWForm();
        form.AddField("email", email.text);
        form.AddField("password", passw.text);
        loginRequest = UnityWebRequest.Post(FindObjectOfType<StartGame>().BASE_URL + "userManager/login", form);
        yield return loginRequest.SendWebRequest();
        if (loginRequest.result == UnityWebRequest.Result.Success)
        {
            login_json = loginRequest.downloadHandler.text;
            UnlockGame();
        }
    }
}

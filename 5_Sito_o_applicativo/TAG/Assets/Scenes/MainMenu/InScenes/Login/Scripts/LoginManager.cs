using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Assets.PlayerPreferences;
using UnityEngine.SceneManagement;
using Assets.Scenes;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField passw;
    public GameObject error;

    public Button passButton;

    private string login_json;

    private void Start()
    {
        passw.inputType = TMP_InputField.InputType.Password;
    }

    public void LogIn()
    {
        StartCoroutine(LoginRequest());
    }

    public void UnlockGame()
    {
        if (login_json.Contains("Id"))
        {
            User user = JsonUtility.FromJson<User>(login_json);
            PlayerPrefs.SetString(PlayerPreference.USER_UNAME, user.Nickname);
            PlayerPrefs.SetString(PlayerPreference.USER_EMAIL, user.Email);
            PlayerPrefs.SetInt(PlayerPreference.USER_ID, user.Id);
            SceneManager.LoadScene((int)SceneToId.mainMenu);
        }
        else
        {
            error.SetActive(true);
        }
    }

    public void ManagePassword()
    {
        if (passw.inputType == TMP_InputField.InputType.Standard)
            HidePassword();
        else
            ShowPassword();
    }

    public void ShowPassword()
    {
        passw.inputType = TMP_InputField.InputType.Standard;
    }

    public void HidePassword()
    {
        passw.inputType = TMP_InputField.InputType.Password;
    }

    private IEnumerator LoginRequest()
    {
        UnityWebRequest loginRequest;
        WWWForm form = new WWWForm();
        form.AddField("email", email.text);
        form.AddField("password", passw.text);
        loginRequest = UnityWebRequest.Post(GlobalVars.BASE_URL + "userManager/login", form);
        yield return loginRequest.SendWebRequest();
        if (loginRequest.result == UnityWebRequest.Result.Success)
        {
            login_json = loginRequest.downloadHandler.text;
            print(login_json);
            UnlockGame();
        }
        else
        {
            Debug.LogError("Error: " + loginRequest.error + " // URL: " + loginRequest.url);
        }
    }
}

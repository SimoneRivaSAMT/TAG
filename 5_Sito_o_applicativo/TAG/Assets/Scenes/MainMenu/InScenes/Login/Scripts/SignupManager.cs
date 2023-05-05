using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SignupManager : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField pass;
    public TMP_InputField nick;
    public GameObject button;
    public GameObject success;
    public GameObject error;

    private void Start()
    {
        pass.inputType = TMP_InputField.InputType.Password;
    }

    public void SignUp()
    {
        StartCoroutine(InsertUserInDb());
    }

    public void ManagePassword()
    {
        if (pass.inputType == TMP_InputField.InputType.Standard)
            HidePassword();
        else
            ShowPassword();
    }

    public void ShowPassword()
    {
        pass.inputType = TMP_InputField.InputType.Standard;
    }

    public void HidePassword()
    {
        pass.inputType = TMP_InputField.InputType.Password;
    }

    //Verifico se ho già registrato un account
    private IEnumerator InsertUserInDb()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest signupRequest;
        form.AddField("user_nickname", nick.text);
        form.AddField("user_password", pass.text);
        form.AddField("user_email", email.text);
        signupRequest = UnityWebRequest.Post(GlobalVars.BASE_URL + "userManager/signup/", form);
        yield return signupRequest.SendWebRequest();
        if (signupRequest.result == UnityWebRequest.Result.Success)
        {
            if (signupRequest.downloadHandler.text == "Done")
            {
                error.SetActive(false);
                success.SetActive(true);
                button.SetActive(false);
            }
            else
            {
                error.SetActive(true);
            }

        }
        else
        {
            Debug.LogError("Error: " + signupRequest.error + " // URL: " + signupRequest.url);
        }
    }
}

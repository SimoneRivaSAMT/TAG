using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Assets.Scenes;
using UnityEngine.UI;
using Assets.PlayerPreferences;

public class MenuManager : MonoBehaviour
{
    [Header("Intro Settings")]
    public GameObject menuObj;
    public GameObject introObj;
    public int introDuration;

    [Header("Privacy")]
    public GameObject privacyObj;
    public GameObject acceptButton;

    private bool privacyAccepted = false;
    private InputManager inputManager;
    private int id;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod() //viene eseguito solo all'avvio del gioco
    {
        PlayerPrefs.DeleteKey(PlayerPreference.MENU_IS_FIRST_START);
        
    }

    private void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        UpdatePrivacyState();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneToId.mainMenu)
        {
            if (!PlayerPrefs.HasKey(PlayerPreference.MENU_IS_FIRST_START))
            {
                menuObj.SetActive(false);
                introObj.SetActive(true);
                StartCoroutine(IntroWait());
            }
            else
            {
                ShowMainMenu();
                CheckPrivacyPolicyState();
            }
        }
    }

    private void Update()
    {
        if (inputManager.ui.Back.triggered && SceneManager.GetActiveScene().buildIndex == (int)SceneToId.mainMenu)
        {
            StopCoroutine(IntroWait());
            ShowMainMenu();
            CheckPrivacyPolicyState();
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene((int)SceneToId.mainMenu);
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene((int)SceneToId.settings);
    }

    public void StartGame()
    {
        SceneManager.LoadScene((int)SceneToId.lobby);
    }

    public void CreateLobby()
    {
        SceneManager.LoadScene((int)SceneToId.createLobby);
    }

    public void ViewGuide()
    {
        SceneManager.LoadScene((int)SceneToId.howToPlay);
    }

    public void ViewMatchHistory()
    {
        SceneManager.LoadScene((int)SceneToId.matchHistory);
    }

    public void ExitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    public void ReadPrivacyInfo()
    {
        acceptButton.GetComponent<Button>().interactable = true;
    }

    public void AcceptPrivacyInfo()
    {
        PlayerPrefs.SetInt(PlayerPreference.MENU_PRIVACY_ACCEPTED, 1);
        UpdatePrivacyState();
        CheckPrivacyPolicyState();
    }

    
    public void ViewCredits()
    {
        StartCoroutine(LoadSceneAsync((int)SceneToId.credits));
    }

    public void SetInt(int id)
    {
        this.id = id;
    }

    private void UpdatePrivacyState()
    {
        if (!PlayerPrefs.HasKey(PlayerPreference.MENU_PRIVACY_ACCEPTED))
            PlayerPrefs.SetInt(PlayerPreference.MENU_PRIVACY_ACCEPTED, 0);
        privacyAccepted = PlayerPrefs.GetInt(PlayerPreference.MENU_PRIVACY_ACCEPTED) switch
        {
            0 => false,
            1 => true,
            _ => false,
        };
    }

    private void CheckPrivacyPolicyState()
    {
        privacyObj.SetActive(!privacyAccepted);
    }

    private void ShowMainMenu()
    {
        menuObj.SetActive(true);
        introObj.SetActive(false);
        PlayerPrefs.SetInt(PlayerPreference.MENU_IS_FIRST_START, 0);
    }

    private IEnumerator LoadSceneAsync(int index)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.LoadScene(index);
    }

    private IEnumerator IntroWait()
    {
        yield return new WaitForSeconds(introDuration);
        ShowMainMenu();
        CheckPrivacyPolicyState();
    }
}

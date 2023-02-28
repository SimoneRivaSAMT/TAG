using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scenes;

public class MenuManager : MonoBehaviour
{
    [Header("Intro Settings")]
    public GameObject menuObj;
    public GameObject introObj;
    public int introDuration;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod() //viene eseguito solo all'avvio del gioco
    {
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        if(!PlayerPrefs.HasKey("FirstStart"))
        {
            menuObj.SetActive(false);
            introObj.SetActive(true);
            StartCoroutine(IntroWait());
        }
        else
        {
            ShowMainMenu();
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopCoroutine(IntroWait());
            ShowMainMenu();
        }
    }

    public void OpenSettings()
    {

    }

    public void StartGame()
    {

    }

    public void ViewGuide()
    {

    }

    public void ExitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    public void ViewCredits()
    {
        StartCoroutine(LoadSceneAsync((int)SceneToId.credits));
    }

    private void ShowMainMenu()
    {
        menuObj.SetActive(true);
        introObj.SetActive(false);
        PlayerPrefs.SetInt("FirstStart", 0);
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
    }
}

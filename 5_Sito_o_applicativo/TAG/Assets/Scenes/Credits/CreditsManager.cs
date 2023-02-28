using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Assets.Scenes;

public class CreditsManager : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource audioSource;
    [Header("Video")]
    public VideoPlayer videoPlayer;
    public Animator transitionAnimator;
    [Header("Misc")]
    public float videoStartDelay = 0f;
    public GameObject[] childrens;
    public int timeToReturnInPreviusScene = 30;

    private float deltaTime = 0f;
    private bool isPlaying = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioSource.Play();
        StartCoroutine(ReturnBack());
    }

    private void Update()
    {
        if(!isPlaying)
            deltaTime += Time.deltaTime;
        if(deltaTime >= videoStartDelay && !isPlaying)
        {  
            videoPlayer.Play();
            childrens[0].SetActive(false);
            childrens[1].SetActive(true);
            isPlaying = true;
            deltaTime = 0f;
        }
    }

    private IEnumerator ReturnBack()
    {
        yield return new WaitForSeconds(timeToReturnInPreviusScene - 5);
        transitionAnimator.SetTrigger("start");
        yield return new WaitForSeconds(5);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene((int)SceneToId.mainMenu);
    }
}

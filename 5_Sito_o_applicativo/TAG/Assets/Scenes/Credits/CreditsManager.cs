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
    public Animator camAnimator;
    [Header("Misc")]
    public float videoStartDelay = 0f;
    public GameObject[] childrens;
    public int timeToReturnInPreviusScene = 30;
    public float cameraAndVideoStartDelay = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioSource.Play();
        StartCoroutine(WaitToStart());
    }

    private void StartCamera()
    {
        camAnimator.SetTrigger("start");
        StartCoroutine(ReturnBack());
    }

    private void StartVideo()
    {
        videoPlayer.Play();
        childrens[0].SetActive(false);
        childrens[1].SetActive(true);
    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(cameraAndVideoStartDelay);
        StartCamera();
        yield return new WaitForSeconds(videoStartDelay);
        StartVideo();
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

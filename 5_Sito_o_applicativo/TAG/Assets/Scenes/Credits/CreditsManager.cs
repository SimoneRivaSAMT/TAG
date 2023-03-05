using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Assets.Scenes;

public class CreditsManager : MonoBehaviour
{
    [Header("Objects")]
    public VideoPlayer videoPlayer;
    public Animator cameraAnimator;
    public Animator[] lightsAnimators;
    public Material glowMat;

    [Header("Settings")]
    public float videoStartDelaySeconds = 0;
    public float cameraStartDelaySeconds = 0;
    public float generalStartDelaySeconds = 0;
    public float videoDuration = 0;


    private void Start()
    {
        glowMat.DisableKeyword("_EMISSION");
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        for (int i = 0; i < lightsAnimators.Length; i++)
        {
            lightsAnimators[i] = lights[i].GetComponent<Animator>();
        }
        if (generalStartDelaySeconds > 0)
            StartCoroutine(StartDelay(generalStartDelaySeconds));
        else
            GeneralStart();
    }

    private void GeneralStart() //starts lights and screens
    {
        foreach (Animator animator in lightsAnimators)
        {
            animator.SetTrigger("start");
        }
        glowMat.EnableKeyword("_EMISSION");
        if (cameraStartDelaySeconds > 0)
            StartCoroutine(CameraStartDelay(cameraStartDelaySeconds));
        else
            StartCamera();
    }

    private void StartCamera()
    {
        cameraAnimator.SetTrigger("start");
        if (videoStartDelaySeconds > 0)
            StartCoroutine(VideoStartDelay(videoStartDelaySeconds));
        else
            StartVideo();
    }

    private void StartVideo()
    {
        videoPlayer.Play();
        StartCoroutine(VideoManager(videoDuration));
    }

    private IEnumerator StartDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        GeneralStart();
    }

    private IEnumerator CameraStartDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        StartCamera();
    }

    private IEnumerator VideoStartDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        StartVideo();
    }

    private IEnumerator VideoManager(float sec)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene((int)SceneToId.mainMenu);
    }
}

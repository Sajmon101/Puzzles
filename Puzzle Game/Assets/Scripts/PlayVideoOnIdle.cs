using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideoOnIdle : MonoBehaviour
{
    private Vector3 lastMousePosition;
    private float idleTime;
    private float timeToPlayVideo = 10f;
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if (Input.anyKeyDown || Input.mousePosition != lastMousePosition)
        {
            lastMousePosition = Input.mousePosition;
            idleTime = 0f;

            if (videoPlayer.isPlaying)
            {
                StopVideo();
            }
        }
        else
        {
            idleTime += Time.deltaTime;

            if (idleTime >= timeToPlayVideo && !videoPlayer.isPlaying)
            {
                PlayVideo();
            }
        }
    }

    public void PlayVideo()
    {
        videoPlayer.enabled = true;
        videoPlayer.Play();
    }

    void StopVideo()
    {
        idleTime = 0f;
        videoPlayer.Stop();
        videoPlayer.enabled = false;
    }
    void OnVideoEnd(VideoPlayer vp)
    {
        StopVideo();
    }
}
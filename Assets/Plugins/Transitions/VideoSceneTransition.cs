using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSceneTransition : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public Image preloadImage;

    VideoClip fadeIn;
    public VideoClip fadeOut;

    void Start(){
        fadeIn = videoPlayer.clip;
        FadeIn();
    }

    public void FadeIn(){
        StartCoroutine(FadeInLoadVideo());
        videoPlayer.clip = fadeIn;
        videoPlayer.Play();
    }

    public void FadeOut(){
        videoPlayer.clip = fadeOut;
        videoPlayer.Play();
        videoPlayer.loopPointReached += FadeOutComplete;
    }

    IEnumerator FadeInLoadVideo(){
        preloadImage.enabled = true;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        preloadImage.enabled = false;
    }

    void FadeOutComplete(VideoPlayer vp){
        ChangeLevel.Instance.OnFadeComplete();
    }
}

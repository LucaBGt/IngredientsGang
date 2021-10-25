using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    int levelIndex = 0;

    public bool isImageTransition = false;
    public ImageSceneTransition imageSceneTransition;
    public VideoSceneTransition videoSceneTransition;

    public static ChangeLevel Instance;

    void Awake()
    {
        Instance = this;
    }

    //Triggers FadeOut animation
    public void LoadLevel(int index)
    {
        levelIndex = index;
        if (isImageTransition)
        {
            imageSceneTransition.FadeOut();
        }
        else
        {
            videoSceneTransition.FadeOut();
        }
    }

    //Load next level after FadeOut animation finishes
    public void OnFadeComplete()
    {
        //SceneManager.LoadScene(levelIndex);
    }
}

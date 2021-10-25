using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class ImageSceneTransition : MonoBehaviour
{

    public Material mat;
    float propertyValue = 0f;
    bool isFadeOut = false;

    void Start()
    {
        FadeIn();
    }

    private void Awake()
    {

    }

    public void FadeIn()
    {
        isFadeOut = false;
        UpdateMaterial(1f);
        AnimateProperty(60, 0f);
    }

    public void FadeOut()
    {
        isFadeOut = true;
        UpdateMaterial(0f);
        AnimateProperty(1f, 1f);
    }

    //iTween documentation
    //http://www.pixelplacement.com/itween/documentation.php
    public void AnimateProperty(float time, float value)
    {
        Tween.Value(propertyValue, value, UpdateMaterial, time, 0, Tween.EaseInOut, Tween.LoopType.None, null, FadeOutComplete);
        /*iTween.ValueTo(gameObject, iTween.Hash(
            "from", propertyValue,
            "to", value,
            "time", time,
            "delay", 1f,
            "onupdatetarget", gameObject,
            "onupdate", "UpdateMaterial",
            "easetype", iTween.EaseType.linear,
            "oncomplete", "FadeOutComplete",
            "oncompletetarget", this.gameObject
            )
        );*/

    }

    void UpdateMaterial(float newValue)
    {
        propertyValue = newValue;
        mat.SetFloat("_Threshold", newValue);
    }

    void FadeOutComplete()
    {
        if (isFadeOut)
        {
            ChangeLevel.Instance.OnFadeComplete();
        }
    }
}

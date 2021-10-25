using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class FadeScreen : MonoBehaviour
{

    public CanvasGroup myCanvasGroup;
    private void Awake()
    {
        StaticEvents.fadeScreen.AddListener(DoFadeScreen);
    }

    void Start()
    {
        myCanvasGroup.alpha = 1;
        DoFadeScreen(false, 1);
    }

    void DoFadeScreen(bool _fade, float _time)
    {
        if (_fade)
            Tween.CanvasGroupAlpha(myCanvasGroup, 1, _time, 0, Tween.EaseInOut, Tween.LoopType.None);
        else
            Tween.CanvasGroupAlpha(myCanvasGroup, 0, _time, 0, Tween.EaseInOut, Tween.LoopType.None);
    }
}

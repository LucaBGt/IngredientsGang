using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class SpriteFlasher : MonoBehaviour {

    Color tempColor = Color.white;
    Color originalColor;
    SpriteRenderer currentSprite;

    private void Awake () {
        StaticEvents.flashSprite.AddListener (FlashSprite);
    }

    void FlashSprite (SpriteRenderer _spr, float _time) {

        currentSprite = _spr;
        originalColor = currentSprite.color;
        StartCoroutine (DoFlashSprite (_time));
    }

    IEnumerator DoFlashSprite (float _time) {

        //Debug.Log ("Flashing Sprite...");

        Tween.Value (1f, originalColor.r, UpdateSpriteAlpha, _time, 0);

        yield return new WaitForSeconds (_time);

        Color c = originalColor;
        currentSprite.color = c;
    }

    void UpdateSpriteAlpha (float _new) {
        //Debug.Log ("FLASH!");

        float _a = Mathf.PingPong (Time.time, .1f);

        tempColor = currentSprite.color;

        tempColor.g = _a;
        tempColor.b = _a;

        currentSprite.color = tempColor;
    }

    void Complete () {

    }
}
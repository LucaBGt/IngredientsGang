using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_ShowOnly : MonoBehaviour
{
    public GameObject indicator;

    private void Start()
    {
        indicator.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        indicator.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        indicator.SetActive(false);
    }
}

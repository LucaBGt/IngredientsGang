using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour
{
    public PlayerParty party;
    bool isLoading = false;
    private void Awake()
    {
        StaticEvents.goToNextScene.AddListener(DoSwitchScene);
    }

    void DoSwitchScene(int _old, int _new, int goToDoor)
    {
        if (isLoading == false)
        {
            isLoading = true;
            StaticEvents.nextDoor = goToDoor;
            Debug.Log("Unloading Scene " + _old + ", Loading Scene " + _new);
            StartCoroutine(UnloadScene(_old, _new));
        }
    }

    IEnumerator UnloadScene(int _old, int _new)
    {
        StaticEvents.fadeScreen.Invoke(true, .25f);
        yield return new WaitForSeconds(.25f);

        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(_old, UnloadSceneOptions.None);
        yield return asyncOperation;
        StartCoroutine(LoadScene(_new));
    }

    IEnumerator LoadScene(int _new)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_new, LoadSceneMode.Additive);
        yield return asyncOperation;
        isLoading = false;
    }
}

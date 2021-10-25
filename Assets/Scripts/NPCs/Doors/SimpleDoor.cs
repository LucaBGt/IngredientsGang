using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleDoor : MonoBehaviour
{

    public bool instantWarp = true;
    bool canWarp = false;
    [Space]
    public int myID;
    public Transform spawnPlayerHere;

    public int currentScene;
    [Space]
    public int nextScene;
    public int GoalDoor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (instantWarp)
                DoWarp();
            else
                canWarp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canWarp = false;
        }
    }
    private void Update()
    {
        if (canWarp && !instantWarp && Input.GetKeyDown(KeyCode.W))
        {
            DoWarp();
        }
    }

    void DoWarp()
    {
        int currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
        StaticEvents.goToNextScene.Invoke(currentScene, nextScene, GoalDoor);
        Destroy(this.gameObject);
    }
}

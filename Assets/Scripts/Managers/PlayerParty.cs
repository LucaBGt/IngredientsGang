using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerParty : MonoBehaviour
{
    public Player_Base currentPlayer;

    public Rigidbody2D Player1;
    public Rigidbody2D Player2;
    private void Awake()
    {
        StaticEvents.updateGameState.AddListener(UpdateGameState);
        StaticEvents.startNewScene.AddListener(StartNewScene);
        StaticEvents.goToNextScene.AddListener(GoToNextScene);
    }


    private void Start()
    {
#if !UNITY_EDITOR
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
#endif
    }
    void StartNewScene(Vector2 pos)
    {
        Player1.transform.position = pos;

        Player2.transform.position = pos;

        StaticEvents.followTarget.Invoke(currentPlayer.transform);

        currentPlayer.canMove = true;
        //currentPlayer.enabled = true;
        //currentPlayer.rb.isKinematic = false;
    }

    void GoToNextScene(int _doesntMatter, int _doesntMatter2, int _doesntMatter3)
    {
        currentPlayer.canMove = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Player1.gameObject.SetActive(!Player1.gameObject.activeSelf);
            Player2.gameObject.SetActive(!Player2.gameObject.activeSelf);

            if (Player2.gameObject.activeSelf)
            {
                Player2.transform.position = Player1.transform.position;
                Player2.velocity = Player1.velocity;
                StaticEvents.followTarget.Invoke(Player2.transform);
            }
            else
            {
                Player1.transform.position = Player2.transform.position;
                Player1.velocity = Player2.velocity;
                StaticEvents.followTarget.Invoke(Player1.transform);

            }
        }
    }
    void UpdateGameState(gameState gs)
    {
        if (gs == gameState.move)
        {
            StartCoroutine(waitAndUpdate());
        }
        else if (gs == gameState.ui)
        {
            currentPlayer.canMove = false;
        }
    }

    public void UpdateGameState()
    {
        StaticEvents.GameState = gameState.move;
    }

    IEnumerator waitAndUpdate()
    {
        yield return new WaitForSeconds(.25f);
        currentPlayer.canMove = true;
        currentPlayer.CheckIfFlipCorrect();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using PixelCrushers.DialogueSystem;

public class NPC_Base : MonoBehaviour
{

    public GameObject Indicator;
    //DialogueSystemTrigger dialogueSystemTrigger;
    public SpriteRenderer mySprite;

    Transform player;

    public bool canInteract;
    public bool isTalking = false;
    public SpriteRenderer playerRenderer;

    private void Awake()
    {
        //dialogueSystemTrigger = GetComponent<DialogueSystemTrigger>();
        StaticEvents.updateGameState.AddListener(UpdateGameState);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerRenderer = other.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && StaticEvents.isPlayerGrounded && !isTalking)
        {
            player = other.transform;
            Indicator.SetActive(true);
            canInteract = true;
        }
        else
        {
            Indicator.SetActive(false);
            canInteract = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Indicator.SetActive(false);
        canInteract = false;
    }

    //Replace with UnityEvent
    private void Update()
    {
        if (canInteract && Input.GetKeyUp(KeyCode.W) && !isTalking)
        {
            /*
            //canInteract = false;
            isTalking = true;
            StaticEvents.updateGameState.Invoke(gameState.ui);
            Indicator.SetActive(false);

            FlipToSee();
*/
            //dialogueSystemTrigger.OnUse(this.transform);
            //DialogueManager.StartConversation("Debug/TestNPC1", this.transform, player.transform);

        }
    }

    void FlipToSee()
    {
        if (playerRenderer != null)
        {
            float playerX = playerRenderer.transform.position.x;
            float myX = this.transform.position.x;

            if (playerX > myX)
            {
                playerRenderer.flipX = true;
                mySprite.flipX = false;
            }
            else if (playerX < myX)
            {
                playerRenderer.flipX = false;
                mySprite.flipX = true;
            }
        }
    }

    public void UpdateGameState(gameState gs)
    {
        if (gs == gameState.move)
        {
            isTalking = false;
        }

        if (canInteract)
        {
            Indicator.SetActive(true);
        }
    }
}

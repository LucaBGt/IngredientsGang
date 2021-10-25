using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.NPC
{
    public class Interactable_TriggerAndAction : Interactable
    {
        public GameObject FlowChart;

        public GameObject Indicator;
        //DialogueSystemTrigger dialogueSystemTrigger;
        public SpriteRenderer mySprite;

        Transform player;

        public bool canInteract;
        public bool isTalking = false;
        public SpriteRenderer playerRenderer;

        [Header("Settings")]
        public bool flipToPlayer;
        public bool shouldFocus;


        public override void Setup()
        {
            if (FlowChart == null)
                FlowChart = transform.GetChild(0).gameObject;
            base.Setup();
        }

        public override void DoInteraction()
        {
            base.DoInteraction();

            FlowChart.SetActive(true);
        }

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

                if (flipToPlayer)
                {
                    FlipToSee();
                }

                if (shouldFocus)
                {
                    Vector3 pos = this.transform.position - ((this.transform.position - player.position) / 2);
                    StaticEvents.setDetailCameraTarget.Invoke(pos);
                }

                //canInteract = false;
                isTalking = true;
                StaticEvents.updateGameState.Invoke(gameState.ui);
                Indicator.SetActive(false);

                DoInteraction();

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
}
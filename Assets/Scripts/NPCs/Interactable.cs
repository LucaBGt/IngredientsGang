using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using RPG.Data;

namespace RPG.NPC
{
    public class Interactable : MonoBehaviour
    {

        public virtual void Setup()
        {
            if (_excecuteInstantly)
                DoInteraction();
        }

        public bool _excecuteInstantly = false;

        public bool _excecuteOnEntry = false;

        public virtual void DoInteraction()
        {
            StaticEvents.GameState = gameState.ui;
        }

        public virtual void EndInteraction()
        {
            StaticEvents.GameState = gameState.move;
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.NPC
{
    public class NPC : MonoBehaviour
    {
        public List<NPCPage> pages = new List<NPCPage>();

        SceneManager sceneManager;

        public void Setup(SceneManager sm)
        {
            sceneManager = sm;
            //pages.AddRange(GetComponentsInChildren<NPCPage>());
            foreach (NPCPage page in pages)
            {
                page.pageObject.SetActive(false);
            }
        }

        public void CheckActivePage()
        {
            foreach (NPCPage page in pages)
            {
                page.pageObject.SetActive(false);
            }

            foreach (NPCPage page in pages)
            {
                if (page.isActive())
                {
                    page.pageObject.SetActive(true);

                    Interactable activeInteractable = page.pageObject.GetComponent<Interactable>();

                    if (activeInteractable != null)
                    {
                        activeInteractable.Setup();
                    }
                    return;
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using RPG.Data;

namespace RPG.NPC {
    public class SceneManager : MonoBehaviour {
        public List<NPC> NPCs = new List<NPC> ();

        public List<SimpleDoor> Doors = new List<SimpleDoor> ();

        public List<TickBase> Objects = new List<TickBase> ();

        public Transform camRef;

        Vector3 editedRotation = new Vector3 (0, 0, 0);

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake () {
            StaticEvents.updateScene.AddListener (UpdateList);
            camRef = Camera.main.transform;
        }
        void Start () {
            NPCs.AddRange (GetComponentsInChildren<NPC> ());
            Doors.AddRange (GetComponentsInChildren<SimpleDoor> ());
            Objects.AddRange (GetComponentsInChildren<TickBase> ());

            camRef = Camera.main.transform;

            UpdateList ();

            StaticEvents.startNewScene.Invoke (GetPositionFromDoorID ());
            StaticEvents.fadeScreen.Invoke (false, .5f);
        }

        Vector2 GetPositionFromDoorID () {
            int goalDoor = StaticEvents.nextDoor;

            Debug.Log ("Looking for Door ID " + goalDoor);

            foreach (SimpleDoor sd in Doors) {
                if (sd.myID == goalDoor) {
                    Debug.Log ("Door found!");
                    return sd.spawnPlayerHere.position;
                }
            }
            Debug.Log ("Door not found!");
            return Doors[0].spawnPlayerHere.position;
        }
        void UpdateList () {
            foreach (NPC n in NPCs) {
                n.Setup (this);
                n.CheckActivePage ();
            }

            //sprites.Clear();
            //sprites.AddRange(FindObjectsOfType<SpriteToCam>());
        }

        private void Update () {
            if (camRef == null) {
                camRef = Camera.main.transform;
            }

            foreach (TickBase tb in Objects) {
                if(tb != null)
                    tb.Tick ();
            }
        }
    }
}
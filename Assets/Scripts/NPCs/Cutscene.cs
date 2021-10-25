using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using RPG.Data;

namespace RPG.NPC
{
    public class Cutscene : Interactable
    {

        GameObject FlowChart;

        public override void Setup()
        {
            FlowChart = transform.GetChild(0).gameObject;
            base.Setup();
        }

        public override void DoInteraction()
        {
            base.DoInteraction();

            FlowChart.SetActive(true);
        }
        // Start is called before the first frame update
    }
}

using UnityEngine;
using Fungus;
using Sirenix.OdinInspector;
//using RPG.Data;

[CommandInfo("Gameplay",
             "End Cutscene",
             "Sets the Global GameState to be Moving, then turns off the Flowchart")]
[AddComponentMenu("")]

public class EndCutscene : Command
{
    public override void OnEnter()
    {
        StaticEvents.GameState = gameState.move;
        this.GetFlowchart().gameObject.SetActive(false);
        Continue();
    }
}

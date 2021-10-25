using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class StaticEvents
{

    static gameState _gameState;

    public static int nextDoor = 0;

    //The current Gamestate
    public static gameState GameState
    {
        get
        {
            return _gameState;
        }
        set
        {
            _gameState = value;
            //If the GameState gets changes, let everyone know
            StaticEvents.updateGameState.Invoke(_gameState);
        }
    }

    public static bool isPlayerGrounded;

    public static UpdateGameState updateGameState = new UpdateGameState();
    public static UnityEvent updateScene = new UnityEvent();
    public static DoFadeScreen fadeScreen = new DoFadeScreen();
    public static SetCamTarget setDetailCameraTarget = new SetCamTarget();
    public static FollowTarget followTarget = new FollowTarget();

    //Is called whenever a new scene is loaded in to notify the persistant objects
    public static StartNewScene startNewScene = new StartNewScene();

    //Passes the old scene to unload and the new scene to load
    public static GoToNextScene goToNextScene = new GoToNextScene();


    /// <summary>Sends an event that flashes the given sprite for a given amount of time.</summary>
    public static FlashSprite flashSprite = new FlashSprite();

}

public class UpdateGameState : UnityEvent<gameState> { }
public class DoFadeScreen : UnityEvent<bool, float> { }
public class SetCamTarget : UnityEvent<Vector3> { }
public class FollowTarget : UnityEvent<Transform> { }
public class StartNewScene : UnityEvent<Vector2> { }
public class GoToNextScene : UnityEvent<int, int, int> { }
public class FlashSprite : UnityEvent<SpriteRenderer, float> { }


public enum gameState
{
    move, ui
}
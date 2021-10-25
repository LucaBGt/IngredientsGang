using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{

    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera detailCam;
    public Transform detailTarget;
    private void Awake()
    {
        detailCam.enabled = false;
        mainCam.enabled = true;

        StaticEvents.updateGameState.AddListener(GetGameStateUpdate);
        StaticEvents.setDetailCameraTarget.AddListener(SetDetailTarget);
        StaticEvents.followTarget.AddListener(FollowTarget);

    }


    void FollowTarget(Transform t)
    {
        mainCam.Follow = t;
    }
    void SetDetailTarget(Vector3 pos)
    {
        Debug.Log("Setting Target to: " + pos);
        detailTarget.position = pos;
    }

    void GetGameStateUpdate(gameState gs)
    {
        switch (gs)
        {
            case gameState.move:
                detailCam.enabled = false;
                mainCam.enabled = true;
                break;
            case gameState.ui:
                detailCam.enabled = true;
                mainCam.enabled = false;
                break;
        }
    }
}

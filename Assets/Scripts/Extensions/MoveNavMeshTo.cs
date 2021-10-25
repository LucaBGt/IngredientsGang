using UnityEngine;
using Fungus;
using UnityEngine.AI;
//using System.Collections.Generic;
using System.Collections;
//using RPG.Data;
//using MEC;

[CommandInfo("Cutscenes",
             "MoveNaveAgentTo",
             "Moves a NaveMeshAgent towards a position.")]
[AddComponentMenu("")]
public class MoveNavMeshTo : Command
{
    public NavMeshAgent agent;
    public Transform goal;

    //public bool animate;
    public Animator animator;

    public bool WaitForCompletion = true;

    public override void OnEnter()
    {

        agent.enabled = true;

        agent.destination = goal.position;

        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }

        if (WaitForCompletion)
            // Timing.RunCoroutine(_DoRun());
            StartCoroutine(_DoRun());
        else
        {
            StartCoroutine(_DoAnimation());
            Continue();
        }
    }

    /*
        IEnumerator<float> _DoRun()
        {
            while (agent.remainingDistance > 0.1)
            {
                Debug.Log("Path not complete!");
                yield return Timing.WaitForOneFrame;
            }
            Debug.Log("Timing Complete!" + agent.remainingDistance);
            Continue();
        }*/

    IEnumerator _DoRun()
    {
        while (Vector3.Distance(agent.transform.position, goal.position) > 0.3)
        {
            /*
                        if (Input.GetButtonDown(strings.Accept))
                        {
                            agent.transform.position = goal.position;
                        }
            */
            // Debug.Log("Path not complete! " + Vector3.Distance(agent.transform.position, goal.position));
            yield return null;
        }

        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }

        Continue();
    }

    IEnumerator _DoAnimation()
    {
        while (Vector3.Distance(agent.transform.position, goal.position) > 0.3)
        {
            /*
                        if (Input.GetButtonDown(strings.Accept))
                        {
                            agent.transform.position = goal.position;
                        }
            */
            // Debug.Log("Path not complete! " + Vector3.Distance(agent.transform.position, goal.position));
            yield return null;
        }

        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }
    }
}
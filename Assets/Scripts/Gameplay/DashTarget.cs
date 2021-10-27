using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;
public class DashTarget : TickBase
{

    [Header("Settings")]

    public bool canDashToMe;
    public bool destroyAfterHit;

    [Header("References")]
    GameObject target;
    public ParticleSystem hit;
    public Collider2D actualCollider;
    public Player_Base player;

    private void Awake()
    {
        target = transform.GetChild(0).gameObject;
        if (hit == null)
            hit = GetComponent<ParticleSystem>();
        target.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<Player_Base>();
            player.dashTarget = this.transform;
            canDashToMe = true;
        }
    }

    public override void Tick()
    {
        if (canDashToMe)
        {

            if (player.isFacingObject(this.transform) || !player.mustFaceDash)
            {
                target.SetActive(true);
            }
            else
            {
                target.SetActive(false);
            }

            if (!player.isDashing)
            {
                target.transform.right = player.transform.position - target.transform.position;
                if (actualCollider != null)
                    actualCollider.enabled = true;
            }
            else
            {
                if (Vector3.Distance(player.transform.position, this.transform.position) < .3f)
                {
                    hit.Play();
                    if (destroyAfterHit)
                        hit.transform.parent = null;
                    Destroy(this.gameObject);
                }
                else if (Vector3.Distance(player.transform.position, this.transform.position) < 1f)
                {
                    if (actualCollider != null)
                        actualCollider.enabled = false;
                }
                else
              if (actualCollider != null)
                    actualCollider.enabled = true;
            }
        }
        else
        {
            target.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //player = other.gameObject.GetComponent<Player_Base>();
            player.dashTarget = null;
            player.isDashing = false;
            canDashToMe = false;
        }
    }
}
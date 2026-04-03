using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NSMB.Utils;

public class PlayerTether : MonoBehaviour
{
    private PlayerController con;
    public PlayerTether tetheredTo;
    public float restLength, tension;
    private void Awake()
    {
        con = GetComponent<PlayerController>();
    }

    private void Start()
    {
        foreach(PlayerController ply in FindObjectsOfType<PlayerController>())
        {
            if (ply == con)
                continue;
            if (ply.team != con.team)
                continue;
            tetheredTo = ply.tether;
            return;
        }
    }

    public bool holding = false;
    public void Tick()
    {
        CheckHolding();
        Attract();
    }

    public void CheckHolding()
    {
        holding = con.crouching && con.running && con.onGround && Mathf.Abs(con.body.velocity.x) < 0.1f;
    }

    public void Attract()
    {
        if (holding || tetheredTo == null || tetheredTo.con.dead)
        {
            return;
        }

        if (Utils.WrappedDistance(con.body.position, tetheredTo.con.body.position) > restLength)
        {
            Vector2 target = (Vector2)Utils.UnwrapWorldLocation(tetheredTo.con.body.position, con.body.position);

            Vector2 diff = target - con.body.position;
            Vector2 dir = diff.normalized;

            con.body.velocity += (dir * (diff.magnitude - restLength)) * tension;
        }
    }
}

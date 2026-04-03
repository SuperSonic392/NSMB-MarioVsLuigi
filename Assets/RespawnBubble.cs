using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBubble : MonoBehaviour
{
    public PlayerController containing;

    private void LateUpdate()
    {
        if(containing != null)
        {
            if (Vector2.Distance(containing.tether.tetheredTo.transform.position + (Vector3.up * .125f), transform.position) < .75f)
            {
                containing.inBubble = false;
                containing.body.velocity = Vector2.up * 15;
                containing = null;
                return;
            }

            transform.position = containing.body.position + (Vector2.up * .125f);
            containing.inBubble = true;
        }
    }
}

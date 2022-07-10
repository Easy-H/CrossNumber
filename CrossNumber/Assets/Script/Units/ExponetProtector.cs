using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExponetProtector : Protector
{

    public override void Set()
    {
        RaycastHit2D hit = Unit.ObjectCheck(transform.position, 1);
        if (hit)
        {
            if (!hit.collider.GetComponent<CharUnit>())
            {
                gameObject.SetActive(true);
                return;
            }
        }
        gameObject.SetActive(false);
    }
}

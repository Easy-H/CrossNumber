using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharUnit : Unit
{
    [SerializeField] GameObject protect = null;

    public override void Pick()
    {
        base.Pick();
        protect.SetActive(false);
    }

    public override Vector3 Place() {
        
        gameObject.layer = 0;
        protect.SetActive(true);

        return transform.position;

    }

    protected override Vector3 CollideCheck(Vector3 pos)
    {
        if (ObjectCheck(pos) || ObjectCheck(pos, 8))
        {
            pos = CollideCheck(pos - Vector3.up);
        }

        return pos;
    }

}

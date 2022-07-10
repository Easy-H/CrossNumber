using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverUnit : Unit
{
    [SerializeField] Unit overedUnit;
    [SerializeField] string defaultValue = null;

    protected override void ResetValue() {

        base.ResetValue();

        gameObject.layer = defaultLayer;

        if (overedUnit) {
            overedUnit.gameObject.layer = 0;
            overedUnit.BreakOvered();
        }
        
        RaycastHit2D hit = ObjectCheck(transform.position, 1);

        if (hit) {
            overedUnit = hit.collider.GetComponent<Unit>();
            value = overedUnit.value + defaultValue;
            gameObject.layer = 0;
            overedUnit.Overed();
        }
        else {
            value = null;
        }
        if (peaked)
            gameObject.layer = 2;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverUnit : Unit
{
    [SerializeField] Unit _overedUnit;
    [SerializeField] string _defaultValue = null;

    public override void SetStateUnCalced() {

        base.SetStateUnCalced();

        gameObject.layer = _defaultLayer;

        if (_overedUnit) {
            _overedUnit.gameObject.layer = 0;
            _overedUnit.BreakOvered();
        }
        
        RaycastHit2D hit = ObjectCheck(transform.position, 1);

        if (hit) {
            _overedUnit = hit.collider.GetComponent<Unit>();

            _value = _overedUnit.value + _defaultValue;
            gameObject.layer = 0;

            _overedUnit.Overed();
        }
        else {
            _value = null;
        }
        if (_peaked)
            gameObject.layer = 2;
    }

    public override void Calced()
    {
        base.Calced();
        if (_overedUnit) {
            UnitManager.instance.unCalcedUnitCount--;
        }
    }

}

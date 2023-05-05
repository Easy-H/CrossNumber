using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class OverUnit : Unit
{
    NumUnit _overedUnit;

    [SerializeField] string _defaultValue = null;

    public override void SetStateUnCalced() {

        base.SetStateUnCalced();
        
        // 기존의 겹쳐진 유닛과 좌표차이가 없다면 함수를 종료한다.
        // 기존의 겹쳐진 유닛과 좌표차이가 생겼다면 _overedUnit을 초기화한다.
        if (_overedUnit) {

            if ((_overedUnit.transform.position - transform.position).magnitude < 0.1f) {
                return;
            }

            _overedUnit.BreakOvered();
            _overedUnit = null;

        }

        int layerBeforeChange = gameObject.layer;

        // 동일한 위치에 유닛이 존재하는지 확인한다.
        Unit unit = UnitManager.ObjectCheck(transform.position, 1);

        if (unit) {

            // 존재한다면 그 유닛을 _overedUnit으로 설정하고,
            // 이 유닛의 _value를 _overedUnit.value + _defualtValue로 설정한다.
            _overedUnit = unit.GetComponent<NumUnit>();
            _value = _overedUnit.Value + _defaultValue;

            _overedUnit.Overed();

        }
        else {
            // 존재하지 않는다면 _value를 null로 설정한다.
            _value = null;
        }

        // 유닛이 있는지 확인하기 위해 변경한 layer를 원래 상태로 돌린다.
        gameObject.layer = layerBeforeChange;

    }

    // 만약 계산된다면 _overedUnit도 함께 계산된 것으로 처리한다.
    public override void SetStateCalced()
    {
        base.SetStateCalced();

        if (_overedUnit)
            _overedUnit.SetStateCalced();

    }

    // 유닛이 있는지, 있다면 겹칠 수 있는 유닛인지 확인한다.
    public override bool CanPlace(Vector3 pos) {

        Unit existUnit = UnitManager.ObjectCheck(pos, 5);

        if (!existUnit) {
            return true;
        }

        bool result = false;

        return result;
    }

}
*/
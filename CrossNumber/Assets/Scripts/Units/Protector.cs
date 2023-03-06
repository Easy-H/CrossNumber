using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : MonoBehaviour
{
    [SerializeField] UnitType[] _apearAtThisSelected = null;
    
    public void Clear() {
        gameObject.SetActive(false);
    }

    public virtual void SetProtectorApear() {
        gameObject.SetActive(false);

        if (!IsNeedExcute()) {
            return;
        }

        if (Unit.ObjectCheck<Unit>(transform.position)) {
            return;
        }

        gameObject.SetActive(true);
    }

    protected bool IsNeedExcute() {
        

        for (int i = 0; i < _apearAtThisSelected.Length; i++) {
            if (_apearAtThisSelected[i] == UnitManager.Instance.SelectedUnitType) {
                return true;
            }

        }
        return false;

    }

}

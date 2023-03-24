using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExponetProtector : Protector {

    public override void SetProtectorApear() {

        if (!IsNeedExcute()) {
            gameObject.SetActive(false);
            return;
        }

        Unit unit = Unit.ObjectCheck(transform.position, 5);

        if (unit) {

            if (!unit.GetComponent<CharUnit>()) {

                gameObject.SetActive(true);
                return;

            }

        }

        gameObject.SetActive(false);

    }
}

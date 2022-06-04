using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ValueAndSprite {

    public Sprite sprite;
    public string value;

}

public class UnitDataScript : MonoBehaviour
{

    [SerializeField] ValueAndSprite[] VAS = null;

    public Sprite UnitSprite(string unitValue) {

        for (int i = 0; i < VAS.Length; i++) {

            if (VAS[i].value == unitValue)
                return VAS[i].sprite;

        }

        return null;

    }
}

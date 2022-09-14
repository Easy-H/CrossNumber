using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSetter : MonoBehaviour {
    [SerializeField] protected NeedColor _skinNeed = NeedColor.Unit;

    // Start is called before the first frame update
    void Start() {
        Setting();
    }

    public virtual void Setting() {

        if (!enabled)
            return;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = GameManager.Instance.SkinInfor.Skin.GetSkinColor(_skinNeed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkinSetter : SkinSetter
{

    private void OnEnable() {
        Setting();
    }

    // Start is called before the first frame update
    public override void Setting() {
        if (!GameManager.Instance)
            return;

        Image img = GetComponent<Image>();
        img.color = GameManager.Instance.SkinInfor.Skin.GetSkinColor(_skinNeed);
    }
    
}

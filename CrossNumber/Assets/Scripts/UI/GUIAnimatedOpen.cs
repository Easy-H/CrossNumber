using UnityEngine;
using UnityEngine.UI;

public class GUIAnimatedOpen : MonoBehaviour {

    [SerializeField] private UIAnimSequence _openSequence;
    [SerializeField] private UIAnimSequence _closeSequence;

    public void SetOn()
    {
        gameObject.SetActive(true);
        _openSequence.Action();
    }

    public void SetOff()
    {
        _closeSequence.Action(() => {
            gameObject.SetActive(false);
        });
    }

}
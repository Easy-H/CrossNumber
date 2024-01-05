using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIAnimUnit {

    public enum UIAnimationType {
        Rest,
        Fade,
        FillImage,
        Close
    }

    public UIAnimationType _eventType;

    public Image EventTarget = null;

    public float EventValue = 0f;
    public float EventTime = 0f;

    public AudioSource _sound;

    public IEnumerator GetAnim()
    {
        EventTarget.gameObject.SetActive(true);
        switch (_eventType) {
            case UIAnimationType.Rest:
                return Justrest();
            case UIAnimationType.Fade:
                return Fade();
            case UIAnimationType.Close:
                EventTarget.gameObject.SetActive(false);
                return Justrest();

        }
        return FillImage();
    }

    IEnumerator FillImage()
    {

        float startTime = Time.realtimeSinceStartup;
        float useTime = startTime;
        float goalTime = startTime + EventTime;

        float firstFill = EventTarget.fillAmount;

        while (useTime < goalTime)
        {

            useTime = Time.realtimeSinceStartup;
            EventTarget.fillAmount = Mathf.Lerp(firstFill, EventValue, (useTime - startTime) / EventTime);

            yield return new WaitForEndOfFrame();

        }
        EventTarget.fillAmount = EventValue;

    }

    IEnumerator Justrest()
    {

        float startTime = Time.realtimeSinceStartup;
        float useTime = startTime;
        float goalTime = startTime + EventTime;

        while (useTime < goalTime)
        {

            useTime = Time.realtimeSinceStartup;
            yield return new WaitForEndOfFrame();

        }

    }

    IEnumerator Fade()
    {

        float startTime = Time.realtimeSinceStartup;
        float useTime = startTime;
        float goalTime = startTime + EventTime;

        float firstAlpha = EventTarget.color.a;

        EventTarget.color = new Color(EventTarget.color.r, EventTarget.color.r, EventTarget.color.r, firstAlpha);

        while (useTime < goalTime)
        {

            useTime = Time.realtimeSinceStartup;

            float alpha = Mathf.Lerp(firstAlpha, EventValue, (useTime - startTime) / EventTime);

            yield return new WaitForEndOfFrame();

            EventTarget.color = new Color(EventTarget.color.r, EventTarget.color.r, EventTarget.color.r, alpha);

        }

        EventTarget.color = new Color(EventTarget.color.r, EventTarget.color.r, EventTarget.color.r, EventValue);

    }

}

[System.Serializable]
public class UIAnimSequence {

    public string actionName;
    public string audioName;

    public void InitialSet() { 
    
    }

    public UIAnimUnit[] animations = null;
    
}

[RequireComponent(typeof(CanvasRenderer))]
public class GUIAnimatedOpen : MonoBehaviour {

    [SerializeField] UIAnimSequence openSequence;
    [SerializeField] UIAnimSequence closeSequence;

    [SerializeField] bool _stopAll = true;
    [SerializeField] bool _closeDestroy = false;

    bool _isAnimating = false;

    public void Open()
    {
        if (_isAnimating)
            return;

        gameObject.SetActive(true);

        GameManager.Instance._pause = _stopAll;
        Action(openSequence);
    }

    public void Close()
    {
        if (_isAnimating)
            return;

        GameManager.Instance._pause = false;
        Action(closeSequence, true);


    }

    public void Toggle()
    {
        if (gameObject.activeSelf)
        {
            Close();
            return;
        }
        Open();

    }

    void Action(UIAnimSequence sequence, bool isClose = false) {
        gameObject.SetActive(true);
        StartCoroutine(_UIAnim(sequence, isClose));
    }

    IEnumerator _UIAnim(UIAnimSequence sequence, bool isClose)
    {
        SoundManager.Instance.PlayAudio(sequence.audioName);
        _isAnimating = true;

        for (int i = 0; i < sequence.animations.Length; i++) {
            StartCoroutine(sequence.animations[i].GetAnim());
            yield return new WaitForSeconds(sequence.animations[i].EventTime);
        }

        if (isClose) {
            if (_closeDestroy)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }

        _isAnimating = false;
    }

}
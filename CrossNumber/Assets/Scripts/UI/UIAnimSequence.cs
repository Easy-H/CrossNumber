using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
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
        switch (_eventType)
        {
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

public class UIAnimSequence : MonoBehaviour {

    [SerializeField] string _actionName;
    [SerializeField] string _audioName;

    [SerializeField] UIAnimUnit[] _animations = null;

    Action _animCallback;

    public void Action(Action callback = null)
    {
        gameObject.SetActive(true);
        StartCoroutine(_UIAnim());

        _animCallback = callback;
    }

    IEnumerator _UIAnim()
    {
        SoundManager.Instance.PlayAudio(_audioName);

        for (int i = 0; i < _animations.Length; i++)
        {
            StartCoroutine(_animations[i].GetAnim());
            yield return new WaitForSeconds(_animations[i].EventTime);
        }

        _animCallback?.Invoke();
    }

}
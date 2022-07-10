using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIAnimation
{
    public string eventType;
    public float eventValue;
    public Image panel;
    public float time;
    public AudioSource sound;
    
}

[System.Serializable]
public class UIAnimationGenerator
{
    public string actionName;
    [SerializeField] UIAnimation[] animations = null;

    UIAnimation ua;

    int actnum = 0;

    public void Action()
    {
        if (actnum >= animations.Length)
        {
            actnum = 0;
            return;
        }
        ua = animations[actnum];

        actnum++;

        if (ua.sound)
            ua.sound.Play();

        if (ua.eventType == "JustRest")
        {
            UIManager.instance.StartCoroutine(Justrest(ua.time));
        }
        else
        {
            ua.panel.gameObject.SetActive(true);

            if (ua.eventType == "FadeIn")
            {
                ua.panel.color = new Color(ua.panel.color.r, ua.panel.color.r, ua.panel.color.r, 0f);
                UIManager.instance.StartCoroutine(Fade(ua.eventValue, ua.panel, ua.time));
            }
            else if (ua.eventType == "FillImage")
            {
                UIManager.instance.StartCoroutine(FillImage(ua.eventValue, ua.panel, ua.time));
            }
            else if (ua.eventType == "Close")
            {
                ua.panel.gameObject.SetActive(false);
                Action();
            }
        }

    }

    IEnumerator FillImage(float goalFill, Image panel, float time)
    {
        float useTime = 0;
        float firstFill = panel.fillAmount;

        while (useTime < time)
        {
            useTime += Time.deltaTime;
            panel.fillAmount = Mathf.Lerp(firstFill, goalFill, useTime / time);
            yield return new WaitForSeconds(0.01f);
        }
        panel.fillAmount = goalFill;

        Action();
    }

    IEnumerator Justrest(float time)
    {
        float useTime = 0;
        while (useTime < time)
        {
            useTime += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
        Action();
    }

    IEnumerator Fade(float goalAlpha, Image panel, float time)
    {
        float useTime = 0;
        float firstAlpha = panel.color.a;

        while (useTime < time)
        {
            useTime += Time.deltaTime;
            float alpha = Mathf.Lerp(firstAlpha, goalAlpha, useTime / time);
            yield return new WaitForSeconds(0.01f);
            panel.color = new Color(panel.color.r, panel.color.r, panel.color.r, alpha);
        }
        panel.color = new Color(panel.color.r, panel.color.r, panel.color.r, goalAlpha);

        Action();

    }

}
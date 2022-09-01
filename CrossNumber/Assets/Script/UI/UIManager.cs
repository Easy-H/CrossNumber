using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    
    public static UIManager instance;


    [SerializeField] UIAnimationGenerator[] actions = null;
    [SerializeField] SceneChange changer = null;

    private void Start()
    {
        instance = this;
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GetBack() {
        GameManager.instance.GetBack();
    }

    public void Foward()
    {
        GameManager.instance.Foward();
    }

    public void StartAnimation(int i) {
        actions[i].Action();
    }

    public void StartAnimation(string str) {

        for (int i = 0; i < actions.Length; i++) {

            if (actions[i].actionName == str)
                actions[i].Action();

        }

    }
    public void GoNextScene()
    {
        StartCoroutine(changer.CaptureScreen());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void GoScene(int i)
    {
        StartCoroutine(changer.CaptureScreen());
        SceneManager.LoadScene(i);
    }

    public void OpenWeb(string link) {
        Application.OpenURL(link);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public static UIManager Instance { get; private set; }
    
    [SerializeField] UiAnimationGenerator[] actions = null;

    private void Start() {
        Instance = this;
    }

    public void StartAnimation(int i) {
        actions[i].Action();
    }

    public void StartAnimation(string aniName) {

        for (int i = 0; i < actions.Length; i++) {

            if (actions[i].actionName == aniName) {
                actions[i].Action();
                break;
            }

        }

    }

}

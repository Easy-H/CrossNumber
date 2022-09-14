using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMethod: MonoBehaviour {

    [SerializeField] SceneChange changer = null;

    public void ResetScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoNextScene() {
        StartCoroutine(changer.CaptureScreen());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoScene(int i) {
        StartCoroutine(changer.CaptureScreen());
        SceneManager.LoadScene(i);
    }

    public void OpenWeb(string link) {
        Application.OpenURL(link);
    }

    public void GetBack() {
        MoveData.Instance.GetBack();
    }

    public void Foward() {
        MoveData.Instance.Foward();
    }

    public void ChangeTheme(int idx) {
        GameManager.Instance.SkinInfor.SetSkinIdx(idx);
    }
}

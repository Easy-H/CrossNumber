using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMethod: MonoBehaviour {

    [SerializeField] SceneChange changer = null;

    public void ResetScene() {
        StartCoroutine(changer.CaptureScreen());
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

    public void Undo() {
        MoveData.Instance.Undo();
    }

    public void Redo() {
        MoveData.Instance.Redo();
    }

    public void ChangeTheme(string key) {

        if (key == SkinManager.Instance.nowSkin)
            return;

        SoundManager.instance.PlayAudio("changeScene", true);

        StartCoroutine(SetSkin(key));
    }

    IEnumerator SetSkin(string key) {

        StartCoroutine(changer.CaptureScreen());
        yield return new WaitForEndOfFrame();
        StartCoroutine(changer.Animation());
        yield return new WaitForEndOfFrame();

        SkinManager.Instance.SetSkinIdx(key);
    }

    public void Pause() {
        GameManager.Instance._pause = true;
    }
    public void Continue() {
        GameManager.Instance._pause = false;
    }
}

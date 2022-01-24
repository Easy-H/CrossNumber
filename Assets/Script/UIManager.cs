using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    GameControllerScript manager;

    [SerializeField] Image[] Panels = null;

    [SerializeField] Image loading = null;
    [SerializeField] float fillingSpeed = 0.5f;


    private void Start()
    {
        if (GameObject.FindWithTag("GameController"))
            manager = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Check() {
        StartCoroutine(CheckProcess());
    }

    public IEnumerator CheckProcess() {

        OpenPanel(0);

        bool clear = manager.CheckClear();

        for (float f = 0; f < 1;)
        {
            f += Time.deltaTime * fillingSpeed;
            loading.fillAmount = f;

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.2f);

        ClosePanel(0);

        if (clear) {
            OpenPanel(1);
            
        }
        else {
            OpenPanel(2);

        }
    }

    public void OpenPanel(int i) {
        Panels[i].gameObject.SetActive(true);
    }

    public void ClosePanel(int i) {
        Panels[i].gameObject.SetActive(false);

    }

    public void GoNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GotoScene(int i) {
        SceneManager.LoadScene(i);
    }
    
}

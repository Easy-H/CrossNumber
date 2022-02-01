using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    GameManager manager;

    [SerializeField] GameObject[] Panels = null;
    [SerializeField] GameObject[] texts = null;

    [SerializeField] Image loading = null;
    [SerializeField] float fillingSpeed = 0.5f;


    private void Start()
    {
        if (GameObject.FindWithTag("GameController"))
            manager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GetBack() {
        manager.GetBack();
    }

    public void Check() {
        StartCoroutine(CheckProcess());
    }

    public IEnumerator CheckProcess() {

        OpenPanel(0);

        int clear = manager.CheckClear();

        for (float f = 0; f < 1;)
        {
            f += Time.deltaTime * fillingSpeed;
            loading.fillAmount = f;

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.2f);

        ClosePanel(0);

        if (clear == -1) {
            OpenPanel(1);
            
        }
        else {
            OpenPanel(2);
            OpenText(clear);

        }
    }

    public void OpenPanel(int i) {
        Panels[i].gameObject.SetActive(true);
    }
    public void OpenText(int i)
    {
        texts[i].gameObject.SetActive(true);
    }

    public void ClosePanel(int i) {
        Panels[i].gameObject.SetActive(false);

    }

    public void GoNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void GoScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void GotoScene(int i) {
        SceneManager.LoadScene(i);
    }
    
}

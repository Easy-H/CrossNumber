using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public bool _overed = false;

    [SerializeField] Protector[] _protector = null;
    [SerializeField] GameObject _underline = null;

    protected int _defaultLayer;
    [SerializeField] int[] _carefulLayer = null;
    int _careful;

    [SerializeField] protected string _value = "1";

    public string value {
        get {
            return _value;
        }
    }

    protected bool _peaked = false;
    bool _isCalced = true;
    
    protected virtual void Start() {
        
        _careful = 0;
        _defaultLayer = gameObject.layer;

        // carefulLayer를 계산한다
        for (int i = 0; i < _carefulLayer.Length; i++) {
            _careful = _careful | (1 << _carefulLayer[i]);
        }
    }

    public virtual void SetStateUnCalced() {
        if (!_overed) {
            _isCalced = false;
            StartCoroutine(DrawUnderline());
        }
        SetProtector();
    }

    public void Overed() {
        gameObject.GetComponent<Collider2D>().enabled = false;
        _overed = true;
        Calced();
    }

    public void BreakOvered() {
        _overed = false;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public virtual void Calced()
    {
        if (!_isCalced) {
            UnitManager.instance.unCalcedUnitCount--;
            _underline.SetActive(false);
        }

        _isCalced = true;
        
    }

    IEnumerator DrawUnderline()
    {
        yield return new WaitForEndOfFrame();

        if (!_isCalced) {
            _underline.SetActive(true);
        }

    }

    public int Pick() {
        gameObject.layer = 2;
        _peaked = true;
        ClearProtector();
        return _careful;
    }

    public bool Hold(Vector3 pos) {

        if (ObjectCheck(pos, _careful))
            return false;

        Vector3 resultPos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);

        if ((transform.position - resultPos).magnitude < 0.1f)
            return false;

        UnitManager.instance.CalculateWorld();
        SoundManager.instance.PlayAudio("moveSound", true);
        transform.position = resultPos;
        
        return true;
        
    }

    public Vector3 Place() {

        UnitManager.instance.CheckClear();

        gameObject.layer = _defaultLayer;
        _peaked = false;

        return transform.position;
    }

    void ClearProtector() {
        for (int i = 0; i < _protector.Length; i++) {
            _protector[i].Clear();
        }
    }

    protected void SetProtector() {
        if (_peaked)
            return;
        
        for (int i = 0; i < _protector.Length; i++) {
            _protector[i].Set();
        }
    }

    public static RaycastHit2D ObjectCheck(Vector3 pos, int layerValue) {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, layerValue);

        return hit;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType {
    NumUnit,            NumZero,
    UseCalcAndSignChar, UseOnlyCalcChar,    EqualUnit,
    OverUnit,           Null
};

public class Unit : MonoBehaviour {

    [SerializeField] public UnitType _unitType = UnitType.Null;

    [SerializeField] Protector[] _protector = null;
    [SerializeField] GameObject _underline = null;
    
    private int _defaultLayer;

    private Vector3 posWhenPeak;

    [SerializeField] protected string _value = "1";
    public string Value { get { return _value; } }

    protected bool _isPeaked = false;
    bool _isCalced = true;

    protected virtual void Start() {
        _defaultLayer = gameObject.layer;

    }

    public virtual void SetStateUnCalced() {

        _isCalced = false;

        StartCoroutine(DrawUnderline());
        SetProtector();

    }
    
    // 연산이 되지 않은 상태였다면 UnitManager의 연산되지 않은 유닛 개수 카운트 변수를 1 감소하고 계산된 상태로 바꾸어준다.
    public virtual void Calced() {

        if (!_isCalced) {
            UnitManager.Instance.unCalcedUnitCount--;
            _underline.SetActive(false);
        }

        _isCalced = true;

    }

    IEnumerator DrawUnderline() {

        yield return new WaitForEndOfFrame();

        // 연산이 되지 않았더라면 유닛 밑에 빨간 밑줄을 긋는다.
        if (!_isCalced) {
            _underline.SetActive(true);
        }

    }

    public void Pick() {

        posWhenPeak = transform.position;

        gameObject.layer = 2;
        _isPeaked = true;

        UnitManager.Instance.CalculateWorld();
        UnitManager.Instance.SelectedUnitType = _unitType;
        ClearProtector();
        
    }

    protected virtual bool CanPlace(Vector3 pos) {

        if (Physics2D.Raycast(pos, Vector2.down, 0.1f))
            return false;

        return true;
    }

    public void Hold(Vector3 pos) {

        if (!CanPlace(pos))
            return;

        // 직전의 위치와 이동하고자 하는 위치를 비교했을 때 변화가 있는지 확인한다.
        Vector3 resultPos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);

        if ((transform.position - resultPos).magnitude < 0.1f)
            return;
        
        // 위치에 변화가 있다면 위치를 옮기고 스테이지에 대한 연산을 시작한다.
        transform.position = resultPos;
        UnitManager.Instance.CalculateWorld();
        SoundManager.instance.PlayAudio("moveSound", true);

        return;

    }

    public void Place(bool addData) {

        if ((posWhenPeak - transform.position).magnitude > 0.1f && addData)
            MoveData.Instance.AddData(this, posWhenPeak, transform.position);

        UnitManager.Instance.CalculateCanClear();

        SetProtector();

        gameObject.layer = _defaultLayer;
        _isPeaked = false;

    }

    void ClearProtector() {

        for (int i = 0; i < _protector.Length; i++) {
            _protector[i].Clear();
        }

    }

    protected void SetProtector() {

        if (_isPeaked) {
            return;
        }

        for (int i = 0; i < _protector.Length; i++) {
            _protector[i].SetProtectorApear();
        }

    }


    public static T ObjectCheck<T>(Vector3 pos) {
        T unit = default(T);

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f);

        if (hit)
            unit = hit.transform.GetComponent<T>();

        return unit;
    }

    public static T ObjectCheck<T>(Vector3 pos, int layerValue) {
        T unit = default(T);

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 0.1f, layerValue);

        if (hit)
            unit = hit.transform.GetComponent<T>();

        return unit;
    }

}

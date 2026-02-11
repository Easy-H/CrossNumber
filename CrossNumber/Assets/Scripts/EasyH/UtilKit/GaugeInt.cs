using UnityEngine;

namespace EasyH.UtilKit {
    [System.Serializable]
    public class GaugeInt : GaugeValue<int> {
        [SerializeField] private int _curValue;
        [SerializeField] private int _maxValue;
        [SerializeField] private int _minValue;

        public int Value {
            get {
                return _curValue;
            }
        }

        public int MaxValue {
            get {
                return _maxValue;
            }
        }

        public GaugeInt()
        {
            _curValue = default;
            _maxValue = default;
            _minValue = default;
        }

        public GaugeInt(int curValue)
        {
            _curValue = curValue;
            _maxValue = curValue;
            _minValue = default;
        }

        public GaugeInt(int curValue, int maxValue) : this(curValue)
        {
            _maxValue = maxValue;
        }

        public GaugeInt(int curValue, int maxValue, int minValue) : this(curValue, maxValue)
        {
            _minValue = minValue;
        }


        public void AddValue(int addAmout)
        {
            _curValue = _curValue + addAmout;

            if (_curValue.CompareTo(_maxValue) > 0)
                _curValue = _maxValue;
        }

        public void SubValue(int subAmout)
        {
            _curValue = _curValue - subAmout;

            if (_curValue.CompareTo(_minValue) < 0)
                _curValue = _minValue;

        }

        public void FillMax()
        {
            _curValue = _maxValue;
        }

        public void SetValueMin()
        {
            _curValue = _minValue;
        }

        public float ConvertToRate()
        {
            return (_curValue - _minValue) / (_maxValue - _minValue);
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

namespace EasyH.Gaming.PathBased
{

    public class BranchPlateSelector : NextPlateSelectorBase
    {

        [System.Serializable]
        public class NextPlateInfor
        {
            [SerializeField] internal Plate _from;
            [SerializeField] internal Plate _to;
        }

        [SerializeField] NextPlateInfor[] _infor;
        IDictionary<Plate, Plate> _fromTo;

        [SerializeField] Plate _nextPlate;

        void Start()
        {
            _fromTo = new Dictionary<Plate, Plate>();
            for (int i = 0; i < _infor.Length; i++)
            {
                _fromTo.Add(_infor[i]._from, _infor[i]._to);
            }
        }

        public override Plate NextPlate(Plate from)
        {
            if (from == null)
            {
                return _nextPlate;
            }
            if (!_fromTo.ContainsKey(from)) return _nextPlate;

            return _fromTo[from];

        }

        public override int GetValue()
        {
            return 5;
        }


    }
}
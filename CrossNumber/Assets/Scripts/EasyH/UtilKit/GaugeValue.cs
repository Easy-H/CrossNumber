using System;

namespace EasyH.UtilKit {

    public interface GaugeValue<T> where T : IComparable, IConvertible {
        public int Value { get; }

        public int MaxValue { get; }

        public void AddValue(int addAmout);

        public void SubValue(int subAmout);

        public void FillMax();

        public void SetValueMin();

        public float ConvertToRate();

    }
}

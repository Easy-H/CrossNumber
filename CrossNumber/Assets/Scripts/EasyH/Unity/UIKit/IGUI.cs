using System;
namespace EasyH.Unity.UI
{
    public interface IGUI : IComparable<IGUI>
    {

        public uint Priority { get; }

        public void SetOn();
        public void SetOff();
        public void Open();
        public void Open(Action callback);
        public void Close();

    }
}
using System;


namespace EasyH.Gaming.Inventory
{
    public interface IInventorySlot
    {
        public string ItemCode { get; }
        public int Count { get; }
    }

    public interface IInventory : IObservable<IInventory>
    {
        public int AddItem(string itemCode, int cnt = 1);
        public int UseItem(string itemCode, int cnt = 1);
        public int GetItemCnt(string itemCode);
        public int SlotCount();
        public IInventorySlot GetItemAt(int idx);
    }
}
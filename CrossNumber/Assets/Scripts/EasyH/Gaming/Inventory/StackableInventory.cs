using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyH.Gaming.Inventory
{
    public class StackableInventory : IInventory
    {
        class InventoryUnitMeta
        {
            private string _itemCode;
            private IList<int> _sons;

            private StackableInventory _target;

            public InventoryUnitMeta(StackableInventory target, string itemCode)
            {
                _target = target;
                _itemCode = itemCode;
                _sons = new List<int>();
            }

            public int AddItem(int count)
            {
                int left = count;

                for (int i = 0; i < _sons.Count; i++)
                {
                    left = _target._slots[_sons[i]].AddItem(left);
                }

                while (left > 0 && _target._emptySlots.Count > 0)
                {
                    int newSon = _target._emptySlots[0];
                    _target._emptySlots.RemoveAt(0);
                    _target._slots[newSon].SetItemCode(_itemCode);

                    _sons.Add(newSon);

                    left = _target._slots[newSon].AddItem(left);
                }

                return left;
            }

            public int UseItem(int count)
            {
                int ret = 0;

                // sons 리스트를 역순으로 순회
                for (int i = _sons.Count - 1; i >= 0; i--)
                {
                    int idx = _sons[i];
                    ret += _target._slots[idx].UseItem(count - ret);

                    if (_target._slots[idx].Count == 0)
                    {
                        // 빈 슬롯을 emptySlots에 추가
                        _target._emptySlots.Add(idx);
                        // sons 리스트에서 해당 슬롯을 안전하게 제거
                        _sons.RemoveAt(i);
                    }

                    if (ret >= count) break;
                }

                return ret;
            }

            public int GetCount()
            {
                int ret = 0;

                for (int i = 0; i < _sons.Count; i++)
                {
                    ret += _target._slots[_sons[i]].Count;
                }

                return ret;

            }

        }

        class InventorySlot : IInventorySlot
        {
            public string ItemCode { get; private set; }
            public int Count { get; private set; }
            public static int Capacity = 99;

            public void SetItemCode(string itemCode)
            {
                ItemCode = itemCode;
            }

            public int UseItem(int count)
            {
                int tmp = Mathf.Min(count, Count);
                Count -= tmp;
                return tmp;
            }

            public int AddItem(int count)
            {
                int tmp = Mathf.Min(count, Capacity - Count);
                Count += tmp;

                return count - tmp;
            }

        }

        private IDictionary<string, InventoryUnitMeta> _meta =
            new Dictionary<string, InventoryUnitMeta>();

        private IList<InventorySlot> _slots;
        private IList<int> _emptySlots;

        private ISet<IObserver<IInventory>> _observers
            = new HashSet<IObserver<IInventory>>();


        public IDisposable Subscribe(IObserver<IInventory> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);

                observer.OnNext(this);
            }

            return new Unsubscriber<IInventory>(_observers, observer);
        }

        private void _NotifyToObserver()
        {
            foreach (IObserver<IInventory> target in _observers)
            {
                target.OnNext(this);
            }
        }

        public StackableInventory()
        {
            _slots = new List<InventorySlot>(20);
            _emptySlots = new List<int>(20);

            for (int i = 0; i < 20; i++)
            {
                _slots.Add(new InventorySlot());
                _emptySlots.Add(i);
            }
        }

        public int AddItem(string itemCode, int cnt)
        {
            if (!_meta.ContainsKey(itemCode))
            {
                _meta.Add(itemCode, new InventoryUnitMeta(this, itemCode));
            }

            int ret = _meta[itemCode].AddItem(cnt);

            if (ret != cnt)
                _NotifyToObserver();

            return ret;
        }

        public int UseItem(string itemCode, int cnt)
        {
            if (!_meta.ContainsKey(itemCode)) return 0;

            int ret = _meta[itemCode].UseItem(cnt);

            if (ret != cnt)
                _NotifyToObserver();

            return ret;
        }

        public int GetItemCnt(string itemCode)
        {
            if (!_meta.ContainsKey(itemCode)) return 0;
            return _meta[itemCode].GetCount();
        }

        public int SlotCount()
        {
            return _slots.Count;
        }

        public IInventorySlot GetItemAt(int idx)
        {
            return _slots[idx];
        }
    }
}
using System;
using UnityEngine;

namespace EasyH.Gaming.Inventory
{
    public class GUIUnitInventory : MonoBehaviour, IObserver<IInventory>
    {
        [SerializeField] private GUIUnitInventorySlotBase[] _guiInventoryUnit;

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(IInventory target)
        {
            Debug.Log("GUIInventory Update");
            for (int i = 0; i < target.SlotCount(); i++)
            {
                if (i >= _guiInventoryUnit.Length) return;
                if (_guiInventoryUnit[i] == null) continue;

                IInventorySlot inventorySlot = target.GetItemAt(i);
                _guiInventoryUnit[i].SetData(
                    inventorySlot.ItemCode, inventorySlot.Count);
            }
        }

        public void SetTargetInventory(IInventory target)
        {
        }
    }
}
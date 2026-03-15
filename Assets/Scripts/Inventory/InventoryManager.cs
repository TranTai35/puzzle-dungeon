using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InventoryItem itemPrefab;
    //[SerializeField] private InventoryData data;
    [SerializeField] private List<InventorySlot> slotList = new List<InventorySlot>();
    public int status;

    private const int MaxSelectSlot = 5;
    private int _selectedSlotIndex;
    private void Update()
    {
        if (player.IsAttacking || player.IsMoving) return;
        if (Input.inputString != null)
        {
            bool isInputNumber = int.TryParse(Input.inputString, out int number);
            if (isInputNumber && number > 0 && number <= MaxSelectSlot)
            {
                SelectSlot(number - 1);
            }
        }

       
    }

    public void SelectSlot(int index)
    {
        if (slotList == null || slotList.Count == 0 || index < 0 || index >= slotList.Count)
        {
            return;
        }

        var item = slotList[index].GetComponentInChildren<InventoryItem>();
        if (item == null)
        {
            Debug.Log("Khong the chon");
            return;
        }
        slotList[_selectedSlotIndex].OnUnselected();
        slotList[index].OnSelected();
        _selectedSlotIndex = index;
        status = index;
    }

    public void AddItem(InventoryItemData item)
    {
        for (var i = 0; i < slotList.Count; ++i)
        {
            var slot = slotList[i];
            var itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null) continue;

            SpawnItem(item, slot);
            return;
        }
    }

    private void SpawnItem(InventoryItemData itemData, InventorySlot slot)
    {
        var item = Instantiate(itemPrefab, slot.transform);
        item.Init(itemData);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private Player player;
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private List<InventorySlot> slotList = new List<InventorySlot>();
    //public string status;
    public InventoryItemData itemSelecting;

    private const int MaxSelectSlot = 5;
    private int _selectedSlotIndex;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
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
          
            return;
        }
       
        slotList[_selectedSlotIndex].OnUnselected();
        slotList[index].OnSelected();
        _selectedSlotIndex = index;
        //status = index;
        //Debug.Log(item.Data.Name);
        itemSelecting = item.Data;
        //status = item.Data.Name;
        AudioController.Instance.PlaySoundSelect();
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
        if (itemData.Name == "Bow")
        {
            player.canShot = true;
        }
        var item = Instantiate(itemPrefab, slot.transform);
        item.Init(itemData);
    }

}

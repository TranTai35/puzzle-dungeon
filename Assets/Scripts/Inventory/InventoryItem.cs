using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image image;

    
    public InventoryItemData Data { get; set; }


    public void Init(InventoryItemData data)
    {
        if (data == null) return;

        image.sprite = data.sprite;
        Data = data;
    }
    
}

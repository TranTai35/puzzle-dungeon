using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color unselectedColor;
    [SerializeField] private Color selectedColor;

    public void OnSelected()
    {
        image.color = selectedColor;
    }

    public void OnUnselected()
    {
        image.color = unselectedColor;
    }
}

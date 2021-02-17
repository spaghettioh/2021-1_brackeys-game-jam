using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public FloatVariable batteryCharge;
    public TMP_Text batteryChargeUI;

    public FloatVariable catsInInventory;
    public TMP_Text catsInInventoryUI;

    void Update()
    {
        batteryChargeUI.text = catsInInventory.Value + "%";
        catsInInventoryUI.text = catsInInventory.Value.ToString();
    }
}

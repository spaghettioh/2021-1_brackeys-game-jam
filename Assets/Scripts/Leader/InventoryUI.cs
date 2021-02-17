using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    FloatVariable batteryCharge;
    [SerializeField]
    TMP_Text batteryChargeUI;

    [SerializeField]
    FloatVariable catsInInventory;
    [SerializeField]
    TMP_Text catsInInventoryUI;

    [SerializeField]
    FloatVariable UI_ActionPosition;
    [SerializeField]
    GameObject laserFrame;
    [SerializeField]
    GameObject catsFrame;

    void Update()
    {
        batteryChargeUI.text = Mathf.Round(batteryCharge.Value) + "%";
        catsInInventoryUI.text = catsInInventory.Value.ToString();

        if (UI_ActionPosition.Value == -1)
        {
            laserFrame.SetActive(true);
            catsFrame.SetActive(false);
        }
        if (UI_ActionPosition.Value == 1)
        {
            laserFrame.SetActive(false);
            catsFrame.SetActive(true);
        }
    }
}

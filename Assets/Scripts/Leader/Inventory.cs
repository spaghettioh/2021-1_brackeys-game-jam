using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public FloatVariable batteryCharge;
    public TMP_Text batteryChargeUI;

    public FloatVariable catsInInventory;
    public TMP_Text catsInInventoryUI;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        batteryChargeUI.text = catsInInventory.Value + "%";
        catsInInventoryUI.text = catsInInventory.Value.ToString();
    }
}

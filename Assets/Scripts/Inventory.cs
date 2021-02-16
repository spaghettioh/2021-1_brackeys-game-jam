using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public FloatVariable batteryCharge;
    public TMP_Text batteryChargeUI;

    public FloatVariable slowCatCount;
    public TMP_Text slowCatCountUI;

    public FloatVariable averageCatCount;
    public TMP_Text averageCatCountUI;

    public FloatVariable fastCatCount;
    public TMP_Text fastCatCountUI;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        batteryChargeUI.text = slowCatCount.Value + "%";
        slowCatCountUI.text = slowCatCount.Value.ToString();
        averageCatCountUI.text = averageCatCount.Value.ToString();
        fastCatCountUI.text = fastCatCount.Value.ToString();
    }
}

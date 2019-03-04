using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class postSellPrice : MonoBehaviour
{
    public towerCost tower;
    float sellRatio;
    Text thisText;

    // Start is called before the first frame update
    void Awake()
    {
        thisText = GetComponent<Text>();
        sellRatio = FindObjectOfType<towerPlacement>().sellRatio;
        thisText.text = ((int)(tower.cost * sellRatio)).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        thisText.text = ((int)(tower.cost*sellRatio)).ToString();
    }
}

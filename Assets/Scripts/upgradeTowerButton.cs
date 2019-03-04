using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class upgradeTowerButton : MonoBehaviour
{
    public towerCost tower;
    public int cost;
    public int upgradeVersion;
    Text thisText;

    private void Awake()
    {
        thisText = GetComponent<Text>();
        thisText.text = cost.ToString();
    }

    public void upgrade()
    {
        tower.upgradeTower(upgradeVersion, cost);
    }

    private void Update()
    {
        thisText.text = cost.ToString();
    }
}

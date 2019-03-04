using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerCost : MonoBehaviour
{
    public GameObject[] upgrades;
    public GameObject personalMenu;
    internal towerPlacement unitMenu;
    internal int cost;
    private bool menuActive;

    // Start is called before the first frame update
    void Start()
    {
        menuActive = false;
        personalMenu.SetActive(false);
        unitMenu = FindObjectOfType<towerPlacement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = new RaycastHit2D[10];
            ContactFilter2D triggerCheck = new ContactFilter2D();
            triggerCheck.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            triggerCheck.useTriggers = true;
            Physics2D.Raycast(world, Vector2.zero, triggerCheck, hits);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit && hit.collider.gameObject == gameObject)
                {
                    if (menuActive)
                    {
                        menuActive = false;
                        personalMenu.SetActive(false);
                    }
                    else
                    {
                        menuActive = true;
                        personalMenu.SetActive(true);
                    }
                }
            }
        }
    }

    public void upgradeTower(int version, int upgradeCost)
    {
        if (upgradeCost <= unitMenu.money)
        {
            GameObject newTower = Instantiate(upgrades[version], transform.position, Quaternion.identity);
            newTower.GetComponentInChildren<towerCost>().cost = cost + upgradeCost;
            unitMenu.money -= upgradeCost;
            string direction = GetComponent<projectileTower>().direction;
            newTower.GetComponentInChildren<projectileTower>().direction = direction;
            if (direction == "left")
            {
                newTower.transform.GetChild(0).Rotate(Vector3.forward * -90);
            }
            if (direction == "right")
            {
                newTower.transform.GetChild(0).Rotate(Vector3.forward * 90);
            }
            if (direction == "up")
            {
                newTower.transform.GetChild(0).Rotate(Vector3.forward * 180);
            }
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public void sellTower()
    {
        print("Sell tower");
        unitMenu.money += (int)(cost * unitMenu.sellRatio);
        Destroy(gameObject.transform.parent.gameObject);
    }
}

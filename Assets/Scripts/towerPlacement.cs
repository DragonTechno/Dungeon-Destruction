using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class towerPlacement : MonoBehaviour
{
    public GameObject[] placementObject;
    public int money = 0;
    public float sellRatio = .6f;
    public Text moneyText;
    GameObject activeObject;
    placeRequirements activeRequirements;
    int menuIndex;
    bool menuOpen;

    // Start is called before the first frame update
    void Start()
    {
        menuOpen = false;
        menuIndex = 0;
        moneyText.text = money.ToString();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (menuOpen)
        {
            Vector2 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeObject.transform.position = new Vector2(Mathf.RoundToInt(world.x+.5f)-.5f, Mathf.RoundToInt(world.y +.5f) - .5f);
            if (activeRequirements.cost <= money && (Input.GetMouseButtonUp(0) || (Input.GetMouseButton(0) && 
                (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftControl))) )&& 
                activeRequirements.RequirementsMet())
            {
                money -= activeRequirements.cost;
                activeRequirements.InstantiateTower(new Vector2(Mathf.RoundToInt(world.x+.5f)-.5f, Mathf.RoundToInt(world.y + .5f) - .5f));
            }
            if (Input.GetMouseButtonDown(1))
            {
                activeRequirements.Rotate("counter");
            }
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                menuIndex = (menuIndex + 1) % placementObject.Length;
                string direction = activeRequirements.direction;
                Destroy(activeObject);
                activeObject = Instantiate(placementObject[menuIndex], new Vector2(Mathf.RoundToInt(world.x + .5f) - .5f, Mathf.RoundToInt(world.y + .5f) - .5f), Quaternion.identity);
                activeRequirements = activeObject.GetComponent<placeRequirements>();
                while (activeRequirements.direction != direction)
                {
                    activeRequirements.Rotate("counter");
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(menuOpen)
            {
                Destroy(activeObject);
                menuOpen = false;
            }
            else
            {
                menuOpen = true;
                activeObject = Instantiate(placementObject[menuIndex]);
                activeRequirements = activeObject.GetComponent<placeRequirements>();
                Vector2 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                activeObject.transform.position = new Vector2(Mathf.RoundToInt(world.x + .5f) - .5f, Mathf.RoundToInt(world.y + .5f) - .5f);
            }
        }
        //if (Input.GetMouseButtonDown(1) && (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)))
        //{
        //    Vector2 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    print("Control click");
        //    RaycastHit2D[] hits = new RaycastHit2D[10];
        //    ContactFilter2D triggerCheck = new ContactFilter2D();
        //    triggerCheck.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        //    triggerCheck.useTriggers = true;
        //    Physics2D.Raycast(world, Vector2.zero, triggerCheck, hits);
        //    foreach (RaycastHit2D hit in hits)
        //    {
        //        if (hit)
        //        {
        //            print(hit.collider.gameObject.name);
        //            towerCost tower = hit.collider.gameObject.GetComponent<towerCost>();
        //            if (tower)
        //            {
        //                money += (int)(tower.cost * sellRatio);
        //                Destroy(tower.gameObject);
        //            }
        //        }
        //    }
        //}
        moneyText.text = money.ToString();
    }
}

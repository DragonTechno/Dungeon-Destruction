using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeRequirements : MonoBehaviour
{
    public BoxCollider2D directionalCollider; //Starts with left, works clockwise
    public GameObject tower;
    public int cost;
    public bool directional;
    internal string direction = "down";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] hit = new RaycastHit2D[2];
        ContactFilter2D triggerCheck = new ContactFilter2D();
        triggerCheck.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        triggerCheck.useTriggers = true;
        Physics2D.Raycast(transform.position, Vector2.zero, triggerCheck, hit);

        //If something was hit, the RaycastHit2D.collider will not be null.
        if (hit[1])
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void Rotate(string rotateDir)
    {
        if (direction == "down")
        {
            if (rotateDir == "clockwise")
            {
                direction = "left";
            }
            else
            {
                direction = "right";
            }
        }
        else if (direction == "left")
        {
            if (rotateDir == "clockwise")
            {
                direction = "up";
            }
            else
            {
                direction = "down";
            }
        }
        else if (direction == "up")
        {
            if (rotateDir == "clockwise")
            {
                direction = "right";
            }
            else
            {
                direction = "left";
            }
        }
        else if (direction == "right")
        {
            if (rotateDir == "clockwise")
            {
                direction = "down";
            }
            else
            {
                direction = "up";
            }
        }
        if(directional)
        {
            if (rotateDir == "clockwise")
            {
                transform.Rotate(Vector3.forward * -90);
            }
            else
            {
                transform.Rotate(Vector3.forward * 90);
            }
        }
    }

    public bool RequirementsMet()
    {
        bool requirements = true;

        RaycastHit2D[] hit = new RaycastHit2D[2];
        ContactFilter2D triggerCheck = new ContactFilter2D();
        triggerCheck.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        triggerCheck.useTriggers = true;
        Physics2D.Raycast(transform.position, Vector2.zero, triggerCheck, hit);

        //If something was hit, the RaycastHit2D.collider will not be null.
        if (hit[1])
        {
            requirements = false;
        }
        if (directional)
        {
            ContactFilter2D overlapFilter = new ContactFilter2D();
            Collider2D[] singleOverlap = new Collider2D[1];
            overlapFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(directionalCollider.gameObject.layer));
            directionalCollider.OverlapCollider(overlapFilter, singleOverlap);
            if (singleOverlap[0])
            {
                print("Collision with directional collider");
                requirements = false;
            }
        }
        return requirements;
    }

    public void InstantiateTower(Vector2 position)
    {
        if (RequirementsMet())
        {
            GameObject newTower = Instantiate(tower, position, Quaternion.identity);
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
            newTower.GetComponentInChildren<towerCost>().cost = cost;
        }
    }
}

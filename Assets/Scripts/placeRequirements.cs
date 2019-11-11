using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeRequirements : MonoBehaviour
{
    public BoxCollider2D directionalCollider; //Starts with left, works clockwise
    public GameObject tower;
    public int cost;
    public bool directional;
    public string direction = "down";

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
            if(direction == "down")
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0f);
            }
            else if(direction == "left")
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90f);
            }
            else if (direction == "up")
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180f);
            }
            else if (direction == "right")
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90f);
            }
        }
    }

    public bool RequirementsMet()
    {
        bool requirements = true;

        RaycastHit2D[] hits = new RaycastHit2D[2];

        ContactFilter2D triggerCheck = new ContactFilter2D();
        triggerCheck.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        triggerCheck.useTriggers = true;

        Physics2D.Raycast(transform.position, Vector2.zero, triggerCheck, hits);

        //If something was hit, the RaycastHit2D.collider will not be null.
        foreach (RaycastHit2D hit in hits)
        {
            if (hit)
            {
                if (hit.transform.gameObject != gameObject)
                {
                    requirements = false;
                }
            }
        }

        if (directional)
        {
            ContactFilter2D overlapFilter = new ContactFilter2D();
            RaycastHit2D[] overlaps = new RaycastHit2D[3];

            overlapFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(directionalCollider.gameObject.layer));
            overlapFilter.useTriggers = true;

            Vector3 offset = Vector3.zero;

            if (direction == "down")
            {
                offset = Vector3.down;
            }
            else if(direction == "up")
            {
                offset = Vector3.up;
            }
            else if (direction == "left")
            {
                offset = Vector3.left;
            }
            else if (direction == "right")
            {
                offset = Vector3.right;
            }

            Physics2D.Raycast(transform.position + offset, Vector2.zero, overlapFilter, overlaps);

            //If something was hit, the RaycastHit2D.collider will not be null.
            foreach (RaycastHit2D overlap in overlaps)
            {
                if (overlap && overlap.transform.gameObject != directionalCollider.gameObject)
                {
                    requirements = false;
                    print(overlap.transform.name);
                }
                else if(overlap)
                {
                    print(overlap.transform.name);
                }
            }
        }

        return requirements;
    }

    public void InstantiateTower(Vector2 position)
    {
        transform.position = position;
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

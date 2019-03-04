using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collidingAlert : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ContactFilter2D overlapFilter = new ContactFilter2D();
        overlapFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        Collider2D[] singleOverlap = new Collider2D[1];
        GetComponent<BoxCollider2D>().OverlapCollider(overlapFilter, singleOverlap);
        if (!singleOverlap[0])
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}

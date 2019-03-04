using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pitfall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if(other.tag == "Enemy")
        {
            //print("Enemy pitfall");
            if (GetComponent<CompositeCollider2D>().OverlapPoint(other.transform.position))
            {
                other.GetComponent<enemyStats>().Die();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Enemy")
        {
            //print("Enemy pitfall");
            if (GetComponent<CompositeCollider2D>().OverlapPoint(other.transform.position))
            {
                other.GetComponent<enemyStats>().Die();
            }
        }
    }
}

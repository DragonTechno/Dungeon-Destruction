using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public GameObject spawn;
    public towerPlacement menu;
    public bool death;

    // Start is called before the first frame update
    void Start()
    {
        menu = FindObjectOfType<towerPlacement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (death)
        {
            --menu.lives;
            Destroy(other);
        }
        else
        {
            Vector2 relativePos = other.transform.position - transform.position;
            other.transform.position = (Vector2)spawn.transform.position + relativePos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firingBox : MonoBehaviour
{
    projectileTower parentTower;
    bool firing;

    // Start is called before the first frame update
    void Start()
    {
        firing = false;
        parentTower = GetComponentInParent<projectileTower>();
    }

    private void Update()
    {
        if (firing)
        {
            parentTower.StartFire();
            //print("Hitting enemy");
        }
        firing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        firing = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        firing = true;
    }

    private void LateUpdate()
    {
    }
}

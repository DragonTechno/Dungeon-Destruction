using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireOnPoint : projectileTower
{
    protected override void Fire()
    {
        if (!delayed && charge >= shotCost)
        {
            StartCoroutine("delay");
            charge -= shotCost;
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
        }
    }
}

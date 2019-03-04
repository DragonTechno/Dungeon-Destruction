using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiProjectileTower : projectileTower
{
    public int count;

    protected override void Fire()
    {
        if (!delayed && charge >= shotCost)
        {
            StartCoroutine("delay");
            charge -= shotCost;
            if (direction == "left")
            {
                for (int i = 1; i < count + 1; ++i)
                {
                    Vector2 offset = transform.position + Vector3.left * width / 2 + Vector3.up * (height / 2 - height * i / (count + 2));
                    GameObject projectileInstance = Instantiate(projectile, offset, Quaternion.identity);
                    projectileInstance.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.forward) * Vector2.left * velocity;
                    projectileInstance.transform.right = Vector2.up;
                }
            }
            if (direction == "right")
            {
                for (int i = 1; i < count + 1; ++i)
                {
                    Vector2 offset = transform.position + Vector3.right * width / 2 + Vector3.up * (height / 2 - height * i / (count + 2));
                    GameObject projectileInstance = Instantiate(projectile, offset, Quaternion.identity);
                    projectileInstance.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.forward) * Vector2.right * velocity;
                    projectileInstance.transform.right = Vector2.down;
                }
            }
            if (direction == "up")
            {
                for (int i = 1; i < count + 1; ++i)
                {
                    Vector2 offset = transform.position + Vector3.up * height / 2 + Vector3.right * (width / 2 - width * i / (count + 2));
                    GameObject projectileInstance = Instantiate(projectile, offset, Quaternion.identity);
                    projectileInstance.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.forward) * Vector2.up * velocity;
                    projectileInstance.transform.right = Vector2.right;
                }
            }
            if (direction == "down")
            {
                for (int i = 1; i < count + 1; ++i)
                {
                    Vector2 offset = transform.position + Vector3.down * height / 2 + Vector3.right * (width / 2 - width * i / (count + 2));
                    GameObject projectileInstance = Instantiate(projectile, offset, Quaternion.identity);
                    projectileInstance.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.forward) * Vector2.down * velocity;
                    projectileInstance.transform.right = Vector2.left;
                }
            }
        }
    }
}

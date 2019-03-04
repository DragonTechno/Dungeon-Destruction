using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testProjectTower : projectileTower
{
    public bool alongEdge;

    protected override void Fire()
    {
        if (!delayed && charge >= shotCost)
        {
            StartCoroutine("delay");
            charge -= shotCost;
            if (direction == "left")
            {
                Vector2 offset = transform.position + Vector3.left * width / 2;
                if (alongEdge)
                {
                    offset += Vector2.up * Random.Range(-height / 2, height / 2);
                }
                GameObject projectileInstance = Instantiate(projectile, offset, Quaternion.identity);
                projectileInstance.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.forward) * Vector2.left * velocity;
                projectileInstance.transform.right = Vector2.up;
            }
            if (direction == "right")
            {
                Vector2 offset = transform.position + Vector3.right * width / 2;
                if (alongEdge)
                {
                    offset += Vector2.up * Random.Range(-height / 2, height / 2);
                }
                GameObject projectileInstance = Instantiate(projectile, offset, Quaternion.identity);
                projectileInstance.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.forward) * Vector2.right * velocity;
                projectileInstance.transform.right = Vector2.down;
            }
            if (direction == "up")
            {
                Vector2 offset = transform.position + Vector3.up * height / 2;
                if (alongEdge)
                {
                    offset += Vector2.left * Random.Range(-width / 2, width / 2);
                }
                GameObject projectileInstance = Instantiate(projectile, offset, Quaternion.identity);
                projectileInstance.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.forward) * Vector2.up * velocity;
                projectileInstance.transform.right = Vector2.right;
            }
            if (direction == "down")
            {
                Vector2 offset = transform.position + Vector3.down * height / 2;
                if (alongEdge)
                {
                    offset += Vector2.left * Random.Range(-width / 2, width / 2);
                }
                GameObject projectileInstance = Instantiate(projectile, offset, Quaternion.identity);
                projectileInstance.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.forward) * Vector2.down * velocity;
                projectileInstance.transform.right = Vector2.left;
            }
        }
    }
}

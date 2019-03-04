using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyStats : MonoBehaviour
{
    public float health;
    public float damageVelocity;
    public int reward;
    towerPlacement menu;
    float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        menu = FindObjectOfType<towerPlacement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void changeHealth(float diff)
    {
        currentHealth += diff;
    }

    public void Die()
    {
        menu.money += reward;
        Destroy(gameObject);
    }
}

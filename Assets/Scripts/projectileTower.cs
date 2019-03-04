using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileTower : MonoBehaviour
{
    public GameObject projectile;
    public float width;
    public float height;
    public float velocity;
    public float angle;
    public string direction;
    public float chargeCap;
    public float chargeRate;
    public float shotCost;
    public float shotDelay;

    public float charge = 0;
    public bool delayed = false;

    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        charge = Mathf.Clamp(charge + chargeRate, 0, chargeCap);
    }

    protected virtual void Fire()
    {
        
    }

    public void StartFire()
    {
        Fire();
    }

    IEnumerator delay()
    {
        delayed = true;
        yield return new WaitForSeconds(shotDelay);
        delayed = false;
        yield return null;
    }
}

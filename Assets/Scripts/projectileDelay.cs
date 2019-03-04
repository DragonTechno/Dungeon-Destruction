using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class projectileDelay : MonoBehaviour
{
    public int damage;
    public bool oneHit;
    public bool destroyOnContact;
    public float piercingCoefficient;
    public float delayTime;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TempAvoid(GameObject.FindGameObjectWithTag("wall").GetComponent<TilemapCollider2D>(),.1f));
        StartCoroutine(DestroyAfterDelay(delayTime));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        enemyStats enemy = collision.collider.gameObject.GetComponent<enemyStats>();
        if(enemy)
        {
            enemy.changeHealth(-damage);
            if (oneHit)
            {
                Physics2D.IgnoreCollision(collision.otherCollider, GetComponent<Collider2D>());
            }
            if (destroyOnContact)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyStats enemy = collision.gameObject.GetComponent<enemyStats>();
        if (enemy)
        {
            enemy.changeHealth(-damage);
            if (oneHit)
            {
                Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
            }
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            enemyRb.AddForce(rb.velocity.normalized * piercingCoefficient);
            rb.AddForce(-rb.velocity.normalized * piercingCoefficient);
            if (destroyOnContact)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        enemyStats enemy = collision.collider.gameObject.GetComponent<enemyStats>();
        if (enemy)
        {
            enemy.changeHealth(-damage);
            if (oneHit)
            {
                Physics2D.IgnoreCollision(collision.otherCollider, GetComponent<Collider2D>());
            }
            if (destroyOnContact)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        enemyStats enemy = collision.gameObject.GetComponent<enemyStats>();
        if (enemy)
        {
            enemy.changeHealth(-damage);
            if (oneHit)
            {
                Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
            }
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            enemyRb.AddForce(rb.velocity.normalized * piercingCoefficient);
            rb.AddForce(-rb.velocity.normalized * piercingCoefficient);
            if (destroyOnContact)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator TempAvoid(Collider2D collider, float duration)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
        yield return new WaitForSeconds(duration);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider,false);
        yield return null;
    }

    IEnumerator DestroyAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}

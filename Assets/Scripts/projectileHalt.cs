using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class projectileHalt : MonoBehaviour
{
    public int damage;
    public bool oneHit;
    public bool destroyOnContact;
    public float piercingCoefficient;
    public float speedRatio = 5f;
    Rigidbody2D rb;
    float initialVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TempAvoid(GameObject.FindGameObjectWithTag("wall").GetComponent<TilemapCollider2D>(), .1f));
        initialVelocity = rb.velocity.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude < 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        enemyStats enemy = collision.collider.gameObject.GetComponent<enemyStats>();
        if (enemy)
        {
            if (rb.velocity.magnitude > initialVelocity/speedRatio)
            {
                enemy.changeHealth(-damage);
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
            if (rb.velocity.magnitude > initialVelocity / 3)
            {
                print("Hit enemy");
                enemy.changeHealth(-damage);
                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                float massRatio = 1f / enemyRb.mass;
                //print(massRatio);
                enemyRb.AddForce(rb.velocity.normalized * piercingCoefficient * massRatio,ForceMode2D.Impulse);
                rb.AddForce(-rb.velocity.normalized * piercingCoefficient * massRatio, ForceMode2D.Impulse);
            }
            if (destroyOnContact)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!oneHit)
        {
            enemyStats enemy = collision.collider.gameObject.GetComponent<enemyStats>();
            if (enemy)
            {
                if (rb.velocity.magnitude > initialVelocity / 3)
                {
                    enemy.changeHealth(-damage);
                }
                if (destroyOnContact)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!oneHit)
        {
            enemyStats enemy = collision.gameObject.GetComponent<enemyStats>();
            if (enemy)
            {
                if (rb.velocity.magnitude > initialVelocity / 3)
                {
                    print("Hit enemy");
                    enemy.changeHealth(-damage);
                    Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                    float massRatio = 1f / enemyRb.mass;
                    print(massRatio);
                    enemyRb.AddForce(rb.velocity.normalized * piercingCoefficient * massRatio, ForceMode2D.Impulse);
                    rb.AddForce(-rb.velocity.normalized * piercingCoefficient * massRatio, ForceMode2D.Impulse);
                }
                if (destroyOnContact)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    IEnumerator TempAvoid(Collider2D collider, float duration)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
        yield return new WaitForSeconds(duration);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, false);
        yield return null;
    }
}

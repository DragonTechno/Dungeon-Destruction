using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBall : MonoBehaviour
{
    public float force;
    public float pushForce;
    public float minVelocity;
    public LayerMask enemyLayer;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.velocity = rb.velocity.normalized*Mathf.Max(rb.velocity.magnitude,minVelocity);
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.up * force);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector3.down * force);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.left * force);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * force);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D other = collision.otherRigidbody;
        if(other.gameObject.layer == enemyLayer)
        {
            collision.otherRigidbody.AddForce((transform.position - other.transform.position)*pushForce);
        }
    }
}

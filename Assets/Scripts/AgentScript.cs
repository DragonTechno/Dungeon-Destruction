using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentScript : MonoBehaviour
{
    public float turnSpeed;
	public float waypointDis;
    public float walkForce;
    public float recalculateDistance;
    public float pushRadius = 2f;
    public float pushForce = .5f;
    public Navigation4Tilemap aStar;

    [Tooltip("Layer for enemies")]
    public LayerMask enemyLayer;

    Rigidbody2D rb;
    bool moving;
    int nodeIndex;

    List<NodeItem> pathNodes = new List<NodeItem>();

    // Start is called before the first frame update
    void Start()
    {
        nodeIndex = 0;
        moving = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool recalculate = false;
        if(pathNodes.Count != 0)
        {
            recalculate = (pathNodes[nodeIndex].pos + new Vector2(0.5f, 0.5f) - (Vector2)transform.position).magnitude > recalculateDistance;
        }
        if(Input.GetMouseButtonDown(0))
        {
            moving = true;
            Vector3 world = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            StopAllCoroutines ();
            NodeItem endNode = aStar.getItem(world);
            pathNodes = null;
            pathNodes = aStar.FindingPath (new Vector2 (transform.position.x, transform.position.y), new Vector2 (world.x, world.y));
            StartCoroutine("SmoothMovePlayer");
        }
        else if(moving && recalculate)
        {
            Vector2 world = pathNodes[pathNodes.Count - 1].pos;
            StopAllCoroutines();
            pathNodes = null;
            pathNodes = aStar.FindingPath(new Vector2(transform.position.x, transform.position.y), new Vector2(world.x, world.y));
            StartCoroutine("SmoothMovePlayer");
        }
    }

    void FixedUpdate()
    {
        // an empty list that we'll add to if there are any overlapping enemies
        Collider2D[] nearby = new Collider2D[10];
        // filter states that we're only looking for objects with the enemy layer
        ContactFilter2D enemyFilter = new ContactFilter2D();
        enemyFilter.SetLayerMask(enemyLayer);
        Physics2D.OverlapCircle(transform.position, pushRadius, enemyFilter, nearby);
        foreach (Collider2D collider in nearby)
        {
            if (collider)
            {
                Vector2 bvec = transform.position - collider.gameObject.transform.position;
                bvec = bvec.normalized * Mathf.Clamp(pushForce / bvec.magnitude, 1, 100);
                rb.AddForce(bvec);
            }
        }
    }

    IEnumerator SmoothMovePlayer ()
	{
		for (int i = 0, max = pathNodes.Count; i < max; i++) {
            nodeIndex = i;
			bool isOver = false;
			while (!isOver) {
				Vector2 distance = (Vector2)pathNodes [i].pos + new Vector2 (0.5f, 0.5f) - (Vector2)transform.position;
				// right keeps track of where the pointer is pointing to, or where its right side points to
				transform.right = (Vector2)Vector3.RotateTowards(transform.right, distance, turnSpeed, 1);
                // the math part of this line makes sure that enemies goes faster when they're further away and is capped
                rb.AddForce(transform.right * walkForce);
                //print(GetComponent<Rigidbody2D>().velocity);
				if (Vector2.Distance (pathNodes [i].pos + new Vector2(0.5f,0.5f), new Vector2 (transform.position.x, transform.position.y)) < waypointDis) 
                {
					isOver = true;
				}
				yield return new WaitForFixedUpdate ();
			}
		}
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        moving = false;
        nodeIndex = 0;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("HeavyProjectile"))
        {

        }
    }

    void Stun(float duration)
    {

    }
}

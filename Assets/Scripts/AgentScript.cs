using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentScript : MonoBehaviour
{
    public float turnSpeed;
	public float waypointDis;
    public float walkForce;
    public float recalculateAngle;
    public float pushRadius = 2f;
    public float pushForce = .5f;
    public float stunDuration = 2f;
    public GameObject goal;
    public GameObject line1;
    public GameObject line2;
    public TilemapNavigation aStar;

    [Tooltip("Layer for enemies")]
    public LayerMask enemyLayer;

    Vector2 direction;
    Rigidbody2D rb;
    bool moving;
    bool stunned;
    int nodeIndex;

    List<NodeItem> pathNodes = new List<NodeItem>();

    // Start is called before the first frame update
    void Start()
    {
        nodeIndex = 0;
        moving = false;
        stunned = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool recalculate = false;
        if(pathNodes.Count != 0)
        {
            recalculate = Vector2.Angle(direction, rb.velocity.normalized) > recalculateAngle;
        }
        if (aStar.setupComplete && !moving)
        {
            moving = true;
            Vector3 target = goal.transform.position;
            NodeItem endNode = aStar.getItem(target);
            aStar.StartFindingPath(new Vector2(transform.position.x, transform.position.y), new Vector2(target.x, target.y), setPath);
        }
        if (moving && recalculate)
        {
            Vector2 world = pathNodes[pathNodes.Count - 1].pos;
            aStar.StartFindingPath(new Vector2(transform.position.x, transform.position.y), new Vector2(world.x, world.y), setPath);
        }
    }

    void setPath(List<NodeItem> newPath)
    {
        StopCoroutine("SmoothMovePlayer");
        pathNodes = newPath;
        StartCoroutine("SmoothMovePlayer");
    }

    void FixedUpdate()
    {
        if(!stunned)
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
                    bvec = bvec.normalized * Mathf.Clamp(pushForce / bvec.magnitude, -100, 100);
                    float dotProduct = bvec.magnitude*Mathf.Cos(Vector2.Angle(transform.right, bvec));
                    Rigidbody2D otherRB = collider.gameObject.GetComponent<Rigidbody2D>();
                    otherRB.AddForce(bvec*-1+(Vector2)transform.right.normalized* (dotProduct * -2));
                }
            }
        }
    }

    IEnumerator SmoothMovePlayer ()
	{
        if (pathNodes.Count != 0)
        {
            direction = (pathNodes[0].pos - (Vector2)transform.position).normalized;
        }
        else
        {
            direction = new Vector2(1, 1);
        }
        for (int i = 0, max = pathNodes.Count; i < max; i++) {
            nodeIndex = i;
            if (i - 1 >= 0 && i + 1 < max)
            {
                direction = (pathNodes[i-1].pos - pathNodes[i+1].pos).normalized;
            }
            Vector2 rotatedDirection = Rotate(direction, 90).normalized;
            Vector2 segmentStart = (pathNodes[i].pos + rotatedDirection * .4f + new Vector2(0.5f, 0.5f));
            Vector2 segmentEnd = (pathNodes[i].pos - rotatedDirection * .4f + new Vector2(0.5f, 0.5f));
            bool isOver = false;
			while (!isOver) {
                if (!stunned)
                {
                    Vector2 distance = pathNodes[i].pos + new Vector2(0.5f, 0.5f) - (Vector2)transform.position;
                    // right keeps track of where the pointer is pointing to, or where its right side points to
                    if(!intersection(transform.position, transform.right, segmentStart, segmentEnd))
                    {
                        transform.right = (Vector2)Vector3.RotateTowards(transform.right, distance, turnSpeed, 1);
                    }
                    // the math part of this line makes sure that enemies goes faster when they're further away and is capped
                    rb.AddForce(transform.right * walkForce);
                    //print(GetComponent<Rigidbody2D>().velocity);
                    if (i != max-1 && Vector2.Distance(pathNodes[i].pos + new Vector2(0.5f, 0.5f), new Vector2(transform.position.x, transform.position.y)) < waypointDis)
                    {
                        isOver = true;
                    }
                }
				yield return new WaitForFixedUpdate ();
			}
		}
        //rb.velocity = Vector2.zero;
        //rb.angularVelocity = 0;
        moving = false;
        nodeIndex = 0;
        yield return null;
	}

    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    bool intersection(Vector3 rayOrigin, Vector3 rayDirection, Vector3 segmentStart, Vector3 segmentEnd)
    {
        const double lengthErrorThreshold = 1e-3;

        Vector3 da = rayDirection;
        Vector3 db = segmentEnd - segmentStart;
        Vector3 dc = segmentStart - rayOrigin;

        float s = Vector3.Dot(Vector3.Cross(dc,db),Vector3.Cross(da,db)) / Vector3.Cross(da,db).sqrMagnitude;

        if (s >= 0.0 && s <= 1.0)   // Means we have an intersection
        {
            Vector3 intersection = rayOrigin + s * da;

            // See if this lies on the segment
            if ((intersection - segmentStart).sqrMagnitude + (intersection - segmentEnd).sqrMagnitude <= (segmentEnd - segmentStart).sqrMagnitude + lengthErrorThreshold)
                return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("HeavyProjectile"))
        {
            StartCoroutine(Stun(stunDuration));
        }
    }

    IEnumerator Stun(float duration)
    {
        stunned = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
        yield return null;
    }
}

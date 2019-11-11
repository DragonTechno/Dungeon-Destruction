/**
 * Author: Chris Zhu 
 * Created Data: 01/01/2018
 * A solution for navigation with Unity3D tilemap
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TilemapNavigation : MonoBehaviour
{
	public bool showWallMark = true;
	public bool showPath = true;
    public bool savePaths = true;
	public GameObject WallMark;
	public GameObject PathMark;
	public PathMode pathMode = PathMode.vertical;
	public WalkMode walkMode = WalkMode.smooth;

	public float Radius = 0.4f;
	public float turnSpeed;
    public float nearWallCost;
	public float waypointDis;

	[Tooltip("Layer non walkable")]
	public LayerMask wallLayer;

	[Tooltip("Layer non walkable, but projectiles can pass over")]
	public LayerMask pitLayer;

	[Tooltip("Layer for the floor")]
	public LayerMask floorLayer;

	public Transform player;

	private Vector3 destPos;

	List<NodeItem> pathNodes;

	[Tooltip("Make sense when walk mode is 'smooth', suggest 0 to 10")]
	public float smoothMoveSpeed = 5f;
	[Tooltip("Make sense when walk mode is 'step by step'")]
	public float StepByStepInterval = 0.5f;

	[Tooltip("Position of tilemap's left bottom corner")]
	public Vector2 tilemapStart;
	[Tooltip("Position of tilemap's right top corner")]
	public Vector2 tilemapEnd;

    public bool setupComplete = false;

    GameObject wall;
    GameObject pit;

	private NodeItem[,] map;
	private int w, h;

	private GameObject WallMarks, PathMarks;
	private List<GameObject> pathObj = new List<GameObject> ();
    private Dictionary<((int, int), (int, int)), List<NodeItem>> calculatedPaths;

	void Awake ()
	{
		WallMarks = new GameObject ("WallMarks");
		PathMarks = new GameObject ("PathMarks");
        wall = GameObject.FindGameObjectWithTag("wall");
        pit = GameObject.FindGameObjectWithTag("pit");
        initNavigationMap();
	}

	void Start()
	{
        setupComplete = true;
	}

	/**
	* initiate navigation map
*/
	public void initNavigationMap ()
	{
        calculatedPaths = new Dictionary<((int, int), (int, int)), List<NodeItem>>();
        for (int i = 0; i < WallMarks.transform.childCount; i++) {  
			Destroy (WallMarks.transform.GetChild (i).gameObject);  
		}  
		for (int i = 0; i < PathMarks.transform.childCount; i++) {  
			PathMarks.transform.GetChild (i).gameObject.SetActive (false);
		}  
		w = Mathf.RoundToInt (tilemapEnd.x - tilemapStart.x + 1);
		h = Mathf.RoundToInt (tilemapEnd.y - tilemapStart.y + 1);
		map = new NodeItem[w, h];

        CompositeCollider2D wallCollider = wall.GetComponent<CompositeCollider2D>();
        CompositeCollider2D floorCollider = GetComponent<CompositeCollider2D>();
        CompositeCollider2D pitCollider = pit.GetComponent<CompositeCollider2D>();

        // write unwalkable node 
        for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {
				Vector2 pos = new Vector2 (tilemapStart.x + x, tilemapStart.y + y);
                // check walkable or not
                bool isWall = wallCollider.OverlapPoint(pos + new Vector2(0.5f, 0.5f));
                bool isPit = pitCollider.OverlapPoint(pos + new Vector2(0.5f, 0.5f));
                int nearWall = 0;
                bool nearPit = false;
                // print("isWall: " + isWall.ToString());
                for(int i = 0;i<3;++i)
                {
                    for(int j = 0;j<3;++j)
                    {
                        if (!(i == 1 && j == 1))
                        {
                            if (wallCollider.OverlapPoint(pos + new Vector2(0.5f + i - 1, 0.5f + j - 1)))
                            {
                                ++nearWall;
                            }
                        }
                        nearPit = nearPit || pitCollider.OverlapPoint(pos + new Vector2(0.5f + i - 1, 0.5f + j - 1));
                    }
                }
                // print("nearWall: " + nearWall.ToString());
                // new a node
                map [x, y] = NodeItem.init (isWall || isPit, pos, x, y, 100000);
                if (floorCollider.OverlapPoint(pos + new Vector2(0.5f, 0.5f)))
                {
                    // print("Setting floor cost...");
                    if (nearWall > 0 || nearPit)
                    {
                        if(nearWall > 1)
                        {
                            map[x, y].cost = GetComponent<NodeCost>().cost * nearWallCost;
                        }
                        else
                        {
                            map[x, y].cost = GetComponent<NodeCost>().cost;
                        }
                        if (nearPit)
                        {
                            map[x, y].cost = GetComponent<NodeCost>().cost * nearWallCost;
                        }
                    }
                    else if (!(isWall || isPit))
                    {
                        map[x, y].cost = GetComponent<NodeCost>().cost;
                    }
                }
                // mark unwalkable node
                if ((nearWall > 1 || nearPit) && !(isWall || isPit) && showWallMark && WallMark)
                {
                    GameObject obj = Instantiate(WallMark, new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0), Quaternion.identity);
                    obj.transform.SetParent(WallMarks.transform);
                }
            }
		}
	}

	void Update ()
	{
		// if (startFinding) {
		// 	FindingPath (new Vector2 (player.position.x, player.position.y), new Vector2 (destPos.x, destPos.y));
		// }
        //if(Input.GetMouseButtonDown(0))
        //{
        //    initNavigationMap();
        //}
	}

	/**
	* get Node by position
	* @param position node's world position
*/
	public NodeItem getItem (Vector2 position)
	{
		int x = Mathf.FloorToInt (position.x - tilemapStart.x);
		int y = Mathf.FloorToInt (position.y - tilemapStart.y);
		x = Mathf.Clamp (x, 0, w - 1);
		y = Mathf.Clamp (y, 0, h - 1);
		return map [x, y];
	}

	/**
	* get Nodes around
	* @param node
*/
	public List<NodeItem> getNeighbourNodes (NodeItem node)
	{
		List<NodeItem> list = new List<NodeItem> ();
		switch (pathMode) {
		case PathMode.diagonal:
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					// skip self
					if (i == 0 && j == 0)
						continue;
					int x = node.x + i;
					int y = node.y + j;
					// check out of bound or not, if not add to map
					if (x < w && x >= 0 && y < h && y >= 0)
						list.Add (map [x, y]);
				}
			}
			break;
		case PathMode.vertical:
			if (node.x + 1 < w)
				list.Add (map [node.x + 1, node.y]);
			if (node.x - 1 >= 0)
				list.Add (map [node.x - 1, node.y]);
			if (node.y + 1 < h)
				list.Add (map [node.x, node.y + 1]);
			if (node.y - 1 >= 0)
				list.Add (map [node.x, node.y - 1]);
			break;
		}

		return list;
	}

	/**
	* update path, draw the path
	*/ 
	public List<NodeItem> updatePath (List<NodeItem> lines)
	{
		int curListSize = pathObj.Count;
		if (PathMark && showPath) {
			for (int i = 0, max = lines.Count; i < max; i++) {
				if (i < curListSize) {
					pathObj [i].transform.position = lines [i].pos + new Vector2 (0.5f, 0.5f);
					pathObj [i].SetActive (true);
				} else {
					GameObject obj =Instantiate (PathMark, new Vector3 (lines [i].pos.x + 0.5f,
					            	lines [i].pos.y + 0.5f, 0), Quaternion.identity);
					obj.transform.SetParent (PathMarks.transform);
					pathObj.Add (obj);
				}
			}
			for (int i = lines.Count; i < curListSize; i++) {
				pathObj [i].SetActive (false);
			}
		}
		pathNodes = lines;
		return lines;
	}

	/**
	* move player setp by step
	*/ 
	IEnumerator StepByStepMovePlayer ()
	{
		for (int i = 0, max = pathNodes.Count; i < max; i++) {
			player.position = new Vector3 (pathNodes [i].pos.x + 0.5f, pathNodes [i].pos.y + 0.5f, 0);
			yield return new WaitForSeconds (StepByStepInterval);
		}
	}

    public void StartFindingPath(Vector2 s, Vector2 e, Action<List<NodeItem>> callback)
    {
        StartCoroutine(FindingPath(s,e,callback));
    }

    /**
	* A star Algorithm
	*/
    public IEnumerator FindingPath (Vector2 s, Vector2 e, Action<List<NodeItem>> callback)
	{
        NodeItem startNode = getItem (s);
		NodeItem endNode = getItem (e);

        if (!endNode.isWall)
        {
            if (savePaths && calculatedPaths.ContainsKey(((startNode.x, startNode.y), (endNode.x, endNode.y))))
            {
                //print("Path found!");
                callback(generatePath(startNode, endNode));
                yield break;
            }
            else
            {
                Heap<NodeItem> openSet = new Heap<NodeItem>(300);
                HashSet<NodeItem> closeSet = new HashSet<NodeItem>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    NodeItem curNode = openSet.RemoveFirst();
                    //print(curNode.costToEnd);
                    closeSet.Add(curNode);

                    // find target node
                    if (curNode == endNode)
                    {
                        callback(generatePath(startNode, endNode));
                        yield break;
                    }

                    // select best node in neighbour
                    foreach (var item in getNeighbourNodes(curNode))
                    {
                        if (item.isWall || closeSet.Contains(item))
                            continue;
                        ContactFilter2D enemyCheck = new ContactFilter2D();
                        enemyCheck.SetLayerMask(LayerMask.GetMask("Enemy"));
                        Collider2D[] results = new Collider2D[8];
                        Physics2D.OverlapCircle(curNode.pos, Radius, enemyCheck, results);
                        int trueLength = 0;
                        for(int i = 0;i < results.Length;++i)
                        {
                            if(results[i])
                            {
                                //print(results[i].gameObject.name);
                                ++trueLength;
                            }
                        }
                        float trafficModifier = 0f;
                        float newCost = curNode.costToStart + getDistanceBetweenNodes(curNode, item) * item.cost + trafficModifier*trueLength;
                        if (item.x != curNode.x && item.y != curNode.y)
                        {
                            newCost = curNode.costToStart + getDistanceBetweenNodes(curNode, item) * item.cost + .25f * (map[item.x, curNode.y].cost + map[curNode.x, item.y].cost) + trafficModifier * trueLength;
                        }
                        if (newCost < item.costToStart || !openSet.Contains(item))
                        {
                            item.costToStart = newCost;
                            item.costToEnd = getDistanceBetweenNodes(item, endNode) * item.cost;
                            item.parent = curNode;
                            if (!openSet.Contains(item))
                            {
                                openSet.Add(item);
                            }
                        }
                    }
                }
            }
        }
        callback(generatePath(startNode, endNode));
        yield break;
    }

	/**
	* generate path
	*/
	List<NodeItem> generatePath (NodeItem startNode, NodeItem endNode)
	{
        List<NodeItem> path = new List<NodeItem> ();
        if (!(endNode == null))
        {
            if (savePaths && calculatedPaths.ContainsKey(((startNode.x, startNode.y), (endNode.x, endNode.y))))
            {
                //print("Path found!");
                path = calculatedPaths[((startNode.x, startNode.y), (endNode.x, endNode.y))];
            }
            else
            {
                if (endNode != null)
                {
                    NodeItem temp = endNode;
                    while (temp != startNode)
                    {
                        path.Add(temp);
                        temp = temp.parent;
                    }
                    path.Reverse();
                }
                calculatedPaths[((startNode.x, startNode.y), (endNode.x, endNode.y))] = path;
                //print("Add path!");
            }
        }
        path = simplifyPath(path);
		return updatePath (path);
	}

    List<NodeItem> simplifyPath(List<NodeItem> path)
    {
        List<NodeItem> newPath = new List<NodeItem>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector2 directionNew = new Vector2(path[i].x - path[i + 1].x, path[i].y - path[i + 1].y);
            if (directionNew != directionOld)
            {
                newPath.Add(path[i]);
            }
            directionOld = directionNew;
        }
        newPath.Add(path[path.Count - 1]);
        return newPath;
    }

    /**
		* get distance between nodes
		* using diagonal distance
	*/
    int getDistanceBetweenNodes (NodeItem a, NodeItem b)
	{
        int cntX = Mathf.Abs(a.x - b.x);
        int cntY = Mathf.Abs(a.y - b.y);
        return (int)Mathf.Sqrt(cntX * cntX + cntY * cntY);
    }

	//	int getDistanceBetweenNodes(NavGround.NodeItem a, NavGround.NodeItem b) {
	//		int cntX = Mathf.Abs (a.x - b.x);
	//		int cntY = Mathf.Abs (a.y - b.y);
	//		return cntX + cntY;
	//	}
}

public enum PathMode {
	diagonal,
	vertical
}

public enum WalkMode {
	stepByStep,
	smooth,
	blink
}

public enum WalkDirection{
	idle,
	up,
	down,
	left,
	right
}
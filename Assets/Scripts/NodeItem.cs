using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeItem : ScriptableObject
{
    // walkable
    public bool isWall;
    // node cost
    public float cost;
    // node position
    public Vector2 pos;
    // position in node array
    public int x, y;

    // distance to start
    public float costToStart;
    // distance to end
    public float costToEnd;

    // total distance
    public float costTotal {
        get { return costToStart + costToEnd; }
    }

    // parent node
    public NodeItem parent;

    public static NodeItem init(bool isWall, Vector2 pos, int x, int y, float cost)
    {
        NodeItem returnNode = CreateInstance<NodeItem>();
        returnNode.isWall = isWall;
        returnNode.pos = pos;
        returnNode.x = x;
        returnNode.y = y;
        returnNode.cost = cost;
        return returnNode;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

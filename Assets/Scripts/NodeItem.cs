using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeItem : IHeapItem<NodeItem>
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

    private int heapIndex;

    public static NodeItem init(bool isWall, Vector2 pos, int x, int y, float cost)
    {
        NodeItem returnNode = new NodeItem();
        returnNode.isWall = isWall;
        returnNode.pos = pos;
        returnNode.x = x;
        returnNode.y = y;
        returnNode.cost = cost;
        return returnNode;
    }

    public static bool operator <(NodeItem lhs, NodeItem rhs)
    {
        bool comparison = lhs.costTotal > rhs.costTotal;
        return comparison;
    }

    public static bool operator >(NodeItem lhs, NodeItem rhs)
    {
        bool comparison = lhs.costTotal < rhs.costTotal;
        return comparison;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public static bool operator ==(NodeItem lhs, NodeItem rhs)
    {
        if((object)rhs == null || (object)lhs == null)
        {
            return false;
        }
        bool comparison = lhs.x == rhs.x && lhs.y == rhs.y;
        return comparison;
    }

    public static bool operator !=(NodeItem lhs, NodeItem rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator >=(NodeItem lhs, NodeItem rhs)
    {
        bool comparison = lhs.costTotal >= rhs.costTotal;
        return comparison;
    }

    public static bool operator <=(NodeItem lhs, NodeItem rhs)
    {
        bool comparison = lhs.costTotal <= rhs.costTotal;
        return comparison;
    }

    public override int GetHashCode()
    {
        return (int)(Mathf.Pow(2,x) * Mathf.Pow(3,y))%1000000;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(NodeItem nodeToCompare)
    {
        if(this < nodeToCompare)
        {
            return -1;
        }
        else if(this > nodeToCompare)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree  {

    private QuadTreeNode root;
    private int depth;
    private List<QuadTreeNode> nodes = new List<QuadTreeNode>();

    public QuadTree(Vector2 position, float size, int depth)
    {
        this.root = new QuadTreeNode(null, position, size);
        this.depth = depth;
        //node.SubdivideNode(depth);
    }

    public QuadTreeNode GetRoot()
    {
        return root;
    }

    public QuadTreeNode Insert(AABB aabb, Vector2 position)
    {
        var qd = (QuadTreeCollisionSystem)GameManager.Instance.ColSys;
        qd.AddObjects(4);
        return root.SubdivideNode(position, aabb, depth);
    }

    public QuadTreeNode Insert(AABB aabb)
    {
        var qd = (QuadTreeCollisionSystem)GameManager.Instance.ColSys;
        qd.AddObjects(4);
        return root.SubdivideNode(aabb.transform.position, aabb, depth);
    }

    public void AddNode(QuadTreeNode node)
    {
        nodes.Add(node);
    }

    public List<QuadTreeNode> GetNodes()
    {
        return nodes;
    }

}

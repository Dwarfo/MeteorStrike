using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBNode  {

    public int height;
    public AABBNode parent;
    public AABBNode left = null;
    public AABBNode right = null;
    //public bool isLeaf;
    public AABB nodeBounds;

    public GameObject Meteor = null;

    public AABBNode(AABB nodeBounds)
    {
        this.nodeBounds = nodeBounds;
    }

    public AABBNode(AABB nodeBounds, GameObject go)
    {
        this.nodeBounds = nodeBounds;
        //Sprite size is 2.56, but meteor pic itself is like 0.8
        nodeBounds.size = go.GetComponent<SpriteRenderer>().size.x / 3;
        Meteor = go;
        Debug.Log("Size is: " + go.GetComponent<SpriteRenderer>().size.x);
    }


    public bool IsLeaf()
    {
        return left == null && right == null;
    }

    public AABBNode GetSibling()
    {
        if (this.parent == null)
            return null;
        if (this == parent.left)
            return parent.right;
        else
            return parent.left;
    }

    //If a node is leaf, update AABB position, else add child bounds 
    public void UpdateAABB()
    {
        if (IsLeaf())
            nodeBounds.UpdateAABB(Meteor.transform);
        else
            nodeBounds = AABB.AddTwoBounds(left.nodeBounds, right.nodeBounds);

    }

    

}
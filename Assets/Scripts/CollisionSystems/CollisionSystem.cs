using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : MonoBehaviour {


    public float sizeForMeteor = 0.8f;
    public AABBNode root;
    public float margin = 0.5f;
    List<KeyValuePair<AABBNode, AABBNode>> pairs = new List<KeyValuePair<AABBNode, AABBNode>>();
    List<AABBNode> leaves = new List<AABBNode>();


	void Start ()
    {
		
	}
	
	void Update ()
    {
        BuildTree();
        CheckCollisions();
        //AdjustAllBounds();
    }

    public void AddNode(AABBNode node)
    {
        node.UpdateAABB();
        if (leaves.Capacity == 0)
        {
            root = node;
            leaves.Add(node);
        }
        else
        {
            AABBNode currentNode = root;

            //Find a sibling for a new node
            while (!currentNode.IsLeaf())
            {
                currentNode = SearchNode(node, currentNode);
            }

            currentNode = InsertNode(currentNode, node);
            //Check if it's a 2nd iteration and if tree can be balanced
            Balance(currentNode);

            leaves.Add(node);

        }
    }


    public void BuildTree()
    {
        List<AABBNode> newleaf = new List<AABBNode>(leaves);
        leaves.Clear();
        foreach (AABBNode node in newleaf)
        {
            AddNode(node);
        }
    }

    //Find deepest node that does not contain placingNode 's bounds and makes smallest AABB with new node
    private AABBNode SearchNode(AABBNode placingNode, AABBNode currentNode)
    {
        if (currentNode.left.nodeBounds.Contains(placingNode.nodeBounds))
        {
            return currentNode.left;
        }
        else if (currentNode.right.nodeBounds.Contains(placingNode.nodeBounds))
        {
            return currentNode.right;
        }
        else
            return CheckSize(placingNode, currentNode);
    }

    public void check2N()
    {
        foreach (var node in leaves)
        {
            node.UpdateAABB();
        }

        foreach (var node in leaves)
        {
            foreach (var node2 in leaves)
            {
                if (node == node2)
                    continue;

                if (node.nodeBounds.Intersects(node2.nodeBounds))
                {
                    Debug.Log("Intersection!");
                }
            }
        }
    }

    //return out of children of currentNode node that makes smallest AABB with nodeToAdd
    public AABBNode CheckSize(AABBNode nodeToAdd, AABBNode currentNode)
    {
        return AABB.GetSize(nodeToAdd.nodeBounds, currentNode.left.nodeBounds) < AABB.GetSize(nodeToAdd.nodeBounds, currentNode.right.nodeBounds) ? currentNode.left : currentNode.right;
    }

    //Insert new nodeToAdd to node tree, creating new parent if needed and reassigning hierarchy
    private AABBNode InsertNode(AABBNode currentNode, AABBNode nodeToAdd)
    {
        AABBNode newParent = new AABBNode(new AABB(currentNode.nodeBounds, nodeToAdd.nodeBounds));
        AABBNode oldParent = null;

        if (currentNode.parent != null)
        {
            oldParent = currentNode.parent;
            if (currentNode == currentNode.parent.left)
                currentNode.parent.left = newParent;
            else
                currentNode.parent.right = newParent;
        }
        else
        {
            root = newParent;
        }

        newParent.left = currentNode;
        newParent.right = nodeToAdd;

        currentNode.parent = newParent;
        nodeToAdd.parent = newParent;

        if (oldParent != null)
            newParent.parent = oldParent;

        AdjustSizeOfOne(newParent);

        return newParent;
        //MassAdjustment
    }

    //Adjust sizes of AABB's after insertion of a new Node
    private void AdjustSizeOfOne(AABBNode node)
    {
        while (node != null)
        {
            node.UpdateAABB();
            node = node.parent;
        }
    }

    //check if newParents sibling makes smaller AABB with newParents parent's sibling than current newParents parent AABB, and if it does restructure tree accordingly
    private void Balance(AABBNode newParent)
    {
        if (leaves.Count < 4)
            return;

        while(AABB.GetSize(newParent.GetSibling().nodeBounds, newParent.parent.GetSibling().nodeBounds) < AABB.GetSize(newParent.parent.nodeBounds))
        {
            InsertNode(newParent.parent.GetSibling(), newParent.GetSibling());
            if (newParent.parent.parent.left == newParent.parent)
                newParent.parent.parent.left = newParent;
            else
                newParent.parent.parent.right = newParent;
            newParent.parent = newParent.parent.parent;

            newParent = newParent.parent;
        }

    }

    //If needed all the bounds can be recalculated using this function
    private void AdjustAllBounds()
    {
        List<AABBNode> nodestoCheck2 = new List<AABBNode>(leaves);
        List<AABBNode> nodestoCheck1 = new List<AABBNode>();
        int c = 1;


        while (!nodestoCheck2.Contains(root) && !nodestoCheck1.Contains(root))
        {
            if (c % 2 == 0)
            {
                nodestoCheck2.Clear();
                foreach (AABBNode node in nodestoCheck1)
                {

                    if (!nodestoCheck2.Contains(node.parent))
                        nodestoCheck2.Add(node.parent);

                    node.UpdateAABB();
                }
                c = 1;
            }
            else
            {
                nodestoCheck1.Clear();
                foreach (AABBNode node in nodestoCheck2)
                {
                    if (!nodestoCheck1.Contains(node.parent))
                        nodestoCheck1.Add(node.parent);

                    node.UpdateAABB();

                }
                c++;
            }
        }

        root.UpdateAABB();
    }

    public void CheckCollisions()
    {
        List<AABBNode> cols = new List<AABBNode>(leaves);

        foreach (AABBNode node in cols)
        {
            if (node.GetSibling() == null)
                continue;
            if (!node.GetSibling().IsLeaf())
                continue;
            if (node.nodeBounds.Intersects(node.GetSibling().nodeBounds))
                Debug.Log("Intersection, node: " + node.Meteor.name + " his sibling " + node.GetSibling().Meteor.name); //Add debug info

            //cols.Remove(node);
            //cols.Remove(node.GetSibling());
        }

    }

}

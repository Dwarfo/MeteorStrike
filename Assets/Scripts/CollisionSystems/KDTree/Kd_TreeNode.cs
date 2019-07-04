using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Kd_TreeNode : INode  {

    private Kd_TreeNode parent = null;
    private bool isLeaf = false;
    private Kd_TreeNode[] children;
    private List<AABB> content;

    private float minX = 0;
    private float maxX = 0;
    private float minY = 0;
    private float maxY = 0;

    public IEnumerable<INode> Children { get { return children; } }
    public List<AABB> Content { get { return content; } }
    public Vector2 Position { get { return new Vector2((minX + maxX)/2, (minY + maxY)/2);} }
    public Vector2 MaxVector { get { return new Vector2(maxX, maxY); } }
    public string NodeType { get {return "KDNode";} }

    public bool IsLeaf()
    {
        return isLeaf;
    }

    public void AddForm(AABB form)
    {
        content.Add(form);
    }

    public void RemoveForm(AABB form)
    {
        content.Remove(form);
    }
    
    public Kd_TreeNode Divide(List<AABB> X, List<AABB> Y, int axis, Kd_TreeNode parent = null)
    {
        int middleIndex;
        var qd = (Kd_TreeCollisionSystem)GameManager.Instance.ColSys;
        qd.AddObjects(4);

        if (axis == 0)
        {
            if (X.Count <= qd.maxObjNum)
            {
                makeLeaf(X);
                MakeBounds(X, Y);
                //Debug.Log("This is a leaf now!");
                return this;
            }

            middleIndex = X.Count / 2;
            List<AABB> leftX = X.GetRange(0, middleIndex);
            List<AABB> rightX = X.GetRange(middleIndex, X.Count - middleIndex);

            List<AABB> leftY = new List<AABB>();
            List<AABB> rightY = new List<AABB>();

            foreach (AABB aabb in Y)
            {
                if (aabb.transform.position.x < X[middleIndex].transform.position.x)
                    leftY.Add(aabb);
                else
                    rightY.Add(aabb);
            }

            MakeBounds(X, Y);

            axis++;

            children = new Kd_TreeNode[2];
            Kd_TreeNode leftNode = new Kd_TreeNode();
            Kd_TreeNode rightNode = new Kd_TreeNode();
            children[0] = leftNode;
            children[1] = rightNode;
            leftNode.Divide(leftX, leftY, axis, this);
            rightNode.Divide(rightX, rightY, axis, this);
        }
        else
        {
            if (Y.Count <= qd.maxObjNum)
            {
                makeLeaf(Y);
                MakeBounds(X, Y);
                return this;
            }

            middleIndex = Y.Count / 2;
            List<AABB> leftY = Y.GetRange(0, middleIndex);
            List<AABB> rightY = Y.GetRange(middleIndex, Y.Count - middleIndex);

            List<AABB> leftX = new List<AABB>();
            List<AABB> rightX = new List<AABB>();

            foreach (AABB aabb in X)
            {
                if (aabb.transform.position.y < Y[middleIndex].transform.position.y)
                    leftX.Add(aabb);
                else
                    rightX.Add(aabb);
            }

            axis = 0;

            MakeBounds(X, Y);


            children = new Kd_TreeNode[2];
            Kd_TreeNode leftNode = new Kd_TreeNode();
            Kd_TreeNode rightNode = new Kd_TreeNode();
            children[0] = leftNode;
            children[1] = rightNode;
            leftNode.Divide(leftX, leftY, axis, this);
            rightNode.Divide(rightX, rightY, axis, this);
        }

        return this;
    }

    private void makeLeaf(List<AABB> newChildren)
    {
        content = newChildren;
        isLeaf = true;
        var qd = (Kd_TreeCollisionSystem)GameManager.Instance.ColSys;
        qd.AddLeaf(this);
    }

    private void MakeBounds(List<AABB> X, List<AABB> Y)
    {
        minX = X[0].transform.position.x;
        maxX = X[X.Count - 1].transform.position.x;
        minY = Y[0].transform.position.y;
        maxY = Y[Y.Count - 1].transform.position.y;
    }
}

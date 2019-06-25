using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Kd_TreeNode : INode  {

    public Kd_TreeNode parent = null;
    public bool isLeaf = false;
    public List<AABB> children;

    public float minX = 0;
    public float maxX = 0;
    public float minY = 0;
    public float maxY = 0;

    public IEnumerable<INode> Children => throw new System.NotImplementedException();

    public Vector2 Position => throw new System.NotImplementedException();

    //TODO implement
    public List<AABB> Content => throw new System.NotImplementedException();

    public Kd_TreeNode Divide(List<AABB> X, List<AABB> Y, int axis, Kd_TreeNode parent = null)
    {
        int middleIndex;
        Kd_TreeCollisionSystem.Instance.AddObjects(2);

        if (axis == 0)
        {
            if (X.Count < Kd_TreeCollisionSystem.Instance.maxObjNum)
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

            Kd_TreeNode leftNode = new Kd_TreeNode();
            Kd_TreeNode rightNode = new Kd_TreeNode();
            leftNode.Divide(leftX, leftY, axis, this);
            rightNode.Divide(rightX, rightY, axis, this);
        }
        else
        {
            if (Y.Count < Kd_TreeCollisionSystem.Instance.maxObjNum)
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


            Kd_TreeNode leftNode = new Kd_TreeNode();
            Kd_TreeNode rightNode = new Kd_TreeNode();
            leftNode.Divide(leftX, leftY, axis, this);
            rightNode.Divide(rightX, rightY, axis, this);
        }

        return this;
    }

    private void makeLeaf(List<AABB> newChildren)
    {
        children = newChildren;
        isLeaf = true;
        Kd_TreeCollisionSystem.Instance.AddLeaf(this);
    }

    private void MakeBounds(List<AABB> X, List<AABB> Y)
    {
        /*minX = X[0].transform.position.x;
        maxX = X[X.Count - 1].transform.position.x;
        minY = Y[0].transform.position.y;
        maxY = Y[Y.Count - 1].transform.position.y;*/
    }

    public bool IsLeaf()
    {
        throw new System.NotImplementedException();
    }

    public void AddForm(AABB form)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveForm(AABB form)
    {
        throw new System.NotImplementedException();
    }
}

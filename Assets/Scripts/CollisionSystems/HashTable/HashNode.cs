using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashNode : INode
{
    private Vector2 position;
    public AABB bound;
    public int hashNum;
    public List<AABB> objects = new List<AABB>();

    public HashNode(Vector2 pos, int hashNum)
    {
        this.position = pos;
        this.hashNum = hashNum;
    }

    public Vector2 Position
    {
        get { return position; }
    }

    public IEnumerable<INode> Children
    {
        get
        {
            return null;
        }
    }

    public void AddForm(AABB bound)
    {
        objects.Add(bound);
    }

    public void RemoveForm(AABB bound)
    {
        objects.Remove(bound);
    }

    public bool IsLeaf()
    {
        return false;
    }
}

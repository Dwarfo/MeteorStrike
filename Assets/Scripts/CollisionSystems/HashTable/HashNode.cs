﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashNode : INode
{
    private Vector2 position;
    private AABB bound;
    public int hashNum;
    private List<AABB> content = new List<AABB>();

    public HashNode(Vector2 pos, int hashNum)
    {
        this.position = pos;
        this.hashNum = hashNum;
    }

    public Vector2 Position
    {
        get { return position; }
    }

    public IEnumerable<INode> Children { get { return null; } }

    public List<AABB> Content { get { return content; } }

    public void AddForm(AABB bound)
    {
        content.Add(bound);
    }

    public void RemoveForm(AABB bound)
    {
        content.Remove(bound);
    }

    public bool IsLeaf()
    {
        return false;
    }
}

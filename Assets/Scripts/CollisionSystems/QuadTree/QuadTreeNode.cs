using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeNode : INode {

    public enum QuadTreeIndex
    {
        TopLeft = 0,     //00
        TopRight = 1,    //01
        BottomLeft = 2,  //10
        BottomRight = 3  //11
    }

    public QuadTreeNode parent;
    Vector2 position;
    private float size;
    private QuadTreeNode[] subNodes;
    private List<AABB> content;

    public void AddForm(AABB form)
    {
        if (content == null)
            content = new List<AABB>();
        content.Add(form);
    }

    public QuadTreeNode(QuadTreeNode parent, Vector2 position, float size)
    {
        this.position = position;
        this.size = size;
        this.parent = parent;
    }

    public List<AABB> Content
    {
        get { return content; }
    }

    public Vector2 Position
    {
        get { return position; }
    }

    public float Size
    {
        get { return size; }
    }

    public IEnumerable<INode> Children { get{return subNodes;}
    }

    public bool IsLeaf()
    {
        return subNodes == null;
    }

    public QuadTreeNode SubdivideNode(Vector2 targetPosition, AABB aabb, int depth = 0)
    {
        var subdivIndex = GetIndexPosition(targetPosition, position);


        if (subNodes == null && depth > 0)
        {
            QuadTreeCollisionSystem.Instance.AddObjects(4);
            subNodes = new QuadTreeNode[4];

            for (int i = 0; i < subNodes.Length; i++)
            {
                Vector2 newPos = position;
                if ((i & 2) == 2)
                {
                    newPos.y += size * 0.25f;
                }
                else
                {
                    newPos.y -= size * 0.25f;
                }

                if ((i & 1) == 1)
                {
                    newPos.x += size * 0.25f;
                }
                else
                {
                    newPos.x -= size * 0.25f;
                }

                subNodes[i] = new QuadTreeNode(this, newPos, size * 0.5f);
            }
        }

        if (depth > 0)
        {
            return subNodes[subdivIndex].SubdivideNode(targetPosition, aabb, depth - 1);
        }
        else
        {
            AddForm(aabb);
            return this;
        }
    }

    public static int GetIndexPosition(Vector2 lookupPosition, Vector2 nodePosition)
    {
        int index = 0;

        index |= lookupPosition.y > nodePosition.y ? 2 : 0;
        index |= lookupPosition.x > nodePosition.x ? 1 : 0;

        return index;
    }

    public void RemoveForm(AABB form)
    {
        content.Remove(form);
    }

    public bool HasSomething()
    {
        if (IsLeaf())
        {
            if (content == null)
                return false;
            else
                return content.Count != 0;
        }
        else
            return subNodes != null;
        
    }
    public void BackPropagate()
    {
        if (this.IsLeaf() && parent != null)
        {
            parent.BackPropagate();
        }
        else if(parent != null)
        {
            foreach (var child in subNodes)
            {
                if (child.HasSomething())
                    return;
            }
            subNodes = null;
            parent.BackPropagate();
        }
    }
}


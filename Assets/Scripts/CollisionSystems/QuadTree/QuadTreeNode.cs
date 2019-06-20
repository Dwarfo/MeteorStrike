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

    Vector2 position;
    float size;
    QuadTreeNode[] subNodes;
    public List<AABB> values;

    public void AddForm(AABB form)
    {
        if (values == null)
            values = new List<AABB>();
        values.Add(form);
    }

    public QuadTreeNode(Vector2 position, float size)
    {
        this.position = position;
        this.size = size;
    }

    public IEnumerable<QuadTreeNode> Nodes
    {
        get { return subNodes; }
    }

    public Vector2 Position
    {
        get { return position; }
    }

    public float Size
    {
        get { return size; }
    }

    public IEnumerable<INode> Children
    {
        get
        {
            return subNodes;
        }
    }

    public bool IsLeaf()
    {
        return subNodes == null;
    }

    public QuadTreeNode SubdivideNode(Vector2 targetPosition, AABB aabb, int depth = 0)
    {
        
        var subdivIndex = GetIndexPosition(targetPosition, position);

        if (subNodes == null)
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

                subNodes[i] = new QuadTreeNode(newPos, size * 0.5f);
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

    private int GetIndexPosition(Vector2 lookupPosition, Vector2 nodePosition)
    {
        int index = 0;

        index |= lookupPosition.y > nodePosition.y ? 2 : 0;
        index |= lookupPosition.x > nodePosition.x ? 1 : 0;

        return index;
    }

    public void RemoveForm(AABB form)
    {
        throw new System.NotImplementedException();
    }
}

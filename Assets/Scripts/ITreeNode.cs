using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITreeNode
{
    bool IsLeaf();
    //AABB getData();
    int Depth { get; set; }
    ITreeNode Parent { get; set; }

    void UpdateAABB();
    int CountObjects();
    List<ITreeNode> GetChildren();
    List<AABB> GetContents();
    AABB NodeBounds();
    void Insert(AABB aabb);
    void Remove(AABB aabb);
    void Reset();
    List<AABB> Query(AABB area);
}

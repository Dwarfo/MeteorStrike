using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode {

    IEnumerable<INode> Children { get; }
    List<AABB> Content { get; }
    Vector2 Position { get; }
    bool IsLeaf();
    void AddForm(AABB form);
    void RemoveForm(AABB form);
    
}

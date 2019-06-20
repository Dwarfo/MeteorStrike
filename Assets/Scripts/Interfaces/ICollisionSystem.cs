using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionSystem  {

    void Insert(GameObject node);
    INode GetRoot();
    void Delete(INode node, AABB obj);
    Framestats GetStats();
    void InsertToStatic(List<GameObject> staticGos);

}

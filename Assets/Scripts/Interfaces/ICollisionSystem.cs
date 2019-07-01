using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionSystem  {

    void Insert(GameObject node);
    INode GetRoot();
    void Delete(GameObject obj);
    Framestats GetStats();
    void InsertToStatic(List<GameObject> staticGos);
    KeyValuePair<AABB, float> GetNearestNeighbour(GameObject obj);
    INode FindNode(GameObject go);
    void CheckCollisions();
}

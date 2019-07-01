using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionSystem  {

    INode GetRoot();
    void Build();
    void Insert(GameObject node);
    void Delete(GameObject obj);
    INode FindNode(GameObject go);
    void InsertToStatic(List<GameObject> staticGos);
    void CheckCollisions();
    KeyValuePair<AABB, float> GetNearestNeighbour(GameObject obj);
    Framestats GetStats();
}

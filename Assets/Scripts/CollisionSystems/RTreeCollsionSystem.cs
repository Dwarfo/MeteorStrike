using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTreeCollsionSystem : MonoBehaviour, ICollisionSystem
{
    public void Build()
    {
        throw new System.NotImplementedException();
    }

    public void CheckCollisions()
    {
        throw new System.NotImplementedException();
    }

    public void Delete(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    public INode GetRoot()
    {
        throw new System.NotImplementedException();
    }

    public Framestats GetStats()
    {
        throw new System.NotImplementedException();
    }

    public void Insert(GameObject node)
    {
        throw new System.NotImplementedException();
    }

    public void InsertToStatic(List<GameObject> staticGos)
    {
        throw new System.NotImplementedException();
    }

    void ICollisionSystem.Delete(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    INode ICollisionSystem.FindNode(GameObject go)
    {
        throw new System.NotImplementedException();
    }

    KeyValuePair<AABB, float> ICollisionSystem.GetNearestNeighbour(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    INode ICollisionSystem.GetRoot()
    {
        throw new System.NotImplementedException();
    }

    Framestats ICollisionSystem.GetStats()
    {
        throw new System.NotImplementedException();
    }

    void ICollisionSystem.Insert(GameObject node)
    {
        throw new System.NotImplementedException();
    }

    void ICollisionSystem.InsertToStatic(List<GameObject> staticGos)
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}

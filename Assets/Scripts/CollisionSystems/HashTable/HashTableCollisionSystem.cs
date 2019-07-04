using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class HashTableCollisionSystem : MonoBehaviour, ICollisionSystem {

    public float bucketSize;
    public int fieldSize;
    public Dictionary<int, HashNode> buckets;

    private List<HashNode> toCheck = new List<HashNode>();
    private Dictionary<AABB, int> objects = new Dictionary<AABB, int>();
    private Dictionary<AABB, int> staticObjects = new Dictionary<AABB, int>();
    private int frameCounter = 0;
    private int collisionChecks = 0;
    private int checkedBuckets = 0;

    private void Awake()
    {

    }
    void Start ()
    {
        buckets = new Dictionary<int, HashNode>();
        if (fieldSize % 2 != 0)
            fieldSize++;
        BuildBuckets();
    }
	
	void Update ()
    {
        frameCounter++;
        collisionChecks = 0;
        checkedBuckets = 0;
        frameCounter = 0;
        UI.Instance.UpdateCollisionStatistics(collisionChecks);
	}
    #region Interface_implementation
    
    public int CollisionChecks { get {return collisionChecks; } }
    public int NumOfObjects { get {return checkedBuckets; } }
    public string ColSysName { get {return "Hash_table"; } }
    //Root is irrelevant and nonexistant in hash table
    public INode GetRoot()
    {
        return buckets[0];
    }
    //In hashTable there is no need to rebuild buckets, but rather to only recalculate hashes of objects
    public void Build()
    {
        if(GameManager.Instance.StaticSystem)
            staticObjects = AddToBuckets(staticObjects);

        objects = AddToBuckets(objects);
    }

    public void Insert(GameObject go)
    {
        //Initial hash is -1, because newly added elements do not get builded if they have the same hash
        //after initial insert, rendering static system and first few frames unusable
        objects.Add(go.GetComponent<AABB>(), -1);
    }

    public void Delete(GameObject obj)
    {
        HashNode node = buckets[HashIt(obj.transform.position)];
        node.RemoveForm(obj.GetComponent<AABB>());
    }
    public INode FindNode(GameObject go)
    {
        return buckets[HashIt(go.transform.position)];
    }

    public void InsertToStatic(List<GameObject> staticGos)
    {
        foreach(GameObject go in staticGos)
            staticObjects.Add(go.GetComponent<AABB>(), -1);

        Debug.Log("Size again: " + staticObjects.Count);
    }
    //if bucket contains more than 1 object forcecheck collision between them
    public void CheckCollisions()
    {
        foreach (var node in buckets.Values)
        {
            if (node.Content.Count > 1)
            {
                collisionChecks += BoundsInteraction.CheckN2(node.Content);
                checkedBuckets++;
            }
        }

        if (GameManager.Instance.WriteStats)
        {
            StatsExcelSender.Instance.WriteStat(GetStats());
        }
    }

    public KeyValuePair<AABB, float> GetNearestNeighbour(GameObject obj)
    {
        int hashOfAAbb = HashIt(obj.transform.position);
        HashNode node = buckets[hashOfAAbb];
        AABB nearestNeighbour = obj.GetComponent<AABB>();
        float distance = float.MaxValue;

        foreach (AABB aabb in node.Content)
        {
            if (aabb.Equals(obj))
                continue;

            float newDist = BoundsInteraction.GetDistance(aabb, obj.GetComponent<AABB>());
            if (newDist < distance)
            {
                distance = newDist;
                nearestNeighbour = aabb;
            }
        }
        
        Debug.Log("Distance: " + distance + "| Object: " + nearestNeighbour.gameObject.name);

        return new KeyValuePair<AABB, float>(nearestNeighbour, distance);
    }
    public Framestats GetStats()
    {
        return (new Framestats(GameManager.Instance.GetExecTime(), Time.deltaTime, collisionChecks, checkedBuckets));
    }

    #endregion
    
    //Initially create buckets according to the size of field defined by fieldSize
    private void BuildBuckets()
    {
        Vector2 offset = new Vector2(bucketSize / 2, bucketSize / 2);

        for (int i = -fieldSize / 2; i < fieldSize / 2; i++)
        {
            for (int j = -fieldSize / 2; j < fieldSize / 2; j++)
            {
                var position = new Vector2(i * bucketSize, j * bucketSize);
                position += offset;

                var node = new HashNode(position, HashIt(position));
                buckets.Add(node.hashNum, node);
            }
        }
    }

    private int HashIt(Vector2 position)
    {
        return Mathf.FloorToInt(position.x / bucketSize) + Mathf.FloorToInt(position.y / bucketSize) * fieldSize;
    }

    private int HashIt(Vector2 position, out bool outOfBounds)
    {
        outOfBounds = position.x > (fieldSize / 2) * bucketSize || position.x < (-fieldSize / 2) * bucketSize || position.y > (fieldSize / 2) * bucketSize || position.y < (-fieldSize / 2) * bucketSize;
        return HashIt(position);
    }

    private void OnDrawGizmos()
    {
        if (buckets != null)
        {
            foreach (var node in buckets.Values)
            {
                DrawNode(node);
            }
        }
    }

    private void DrawNode(HashNode node)
    {
        if (node.Content.Count > 1)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(node.Position, new Vector3(0.95f, 0.95f, 0.1f) * bucketSize);
    }

    private Dictionary<AABB, int> AddToBuckets(Dictionary<AABB, int> objects)
    {
        Dictionary<AABB, int> newObjects = new Dictionary<AABB, int>();
        bool outOfBounds = false;
        foreach (KeyValuePair<AABB, int> bound in objects)
        {
            //If object goes out of scene bounds it gets deleted and is not calculated anymore
            int newHash = HashIt(bound.Key.transform.position, out outOfBounds);
            if (/*!buckets.ContainsKey(newHash) ||*/ outOfBounds)
            {
                GameManager.Instance.FormOutOfBounds(bound.Key);
                buckets[bound.Value].RemoveForm(bound.Key);
                continue;
            }
            if (bound.Value != newHash)
            {
                buckets[bound.Value].RemoveForm(bound.Key);
                buckets[newHash].AddForm(bound.Key);
            }

            newObjects.Add(bound.Key, newHash);
        }
        return newObjects;
    }

    private void UpdatePositionsAndHashes()
    {
        objects = AddToBuckets(objects);
    }

}
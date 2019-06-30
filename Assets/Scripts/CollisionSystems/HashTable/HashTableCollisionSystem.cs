using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//Check inside of a bucket via BVH algorithm
public class HashTableCollisionSystem : Singleton_MB<HashTableCollisionSystem>, ICollisionSystem {

    public float bucketSize;
    public int fieldSize;
    public Dictionary<int, HashNode> buckets;

    private List<HashNode> toCheck = new List<HashNode>();
    private Dictionary<AABB, int> objects = new Dictionary<AABB, int>();
    private Dictionary<AABB, int> staticObjects = new Dictionary<AABB, int>();
    private int frameCounter = 0;
    private int collisionChecks = 0;
    private int checkedBuckets = 0;
    [SerializeField]
    private GameObject player;

    private void Awake()
    {
        base.Awake();
        GameManager.Instance.OnPlayerReady.AddListener(HandlePlayerReady);
    }
    void Start ()
    {
        buckets = new Dictionary<int, HashNode>();
        if (fieldSize % 2 != 0)
            fieldSize++;
        BuildBuckets();
        if(GameManager.Instance.StaticSystem)
            AddToBuckets(staticObjects);
    }
	
	void Update ()
    {
        UpdatePositionsAndHashes();

        frameCounter++;
        if (frameCounter == 3)
        {
            collisionChecks = 0;
            checkedBuckets = 0;
            CheckCollisions();
            frameCounter = 0;
            UI.Instance.UpdateCollisionStatistics(collisionChecks);
        }

        //GetNearestNeighbour(player.GetComponent<AABB>());

	}

    //GameObject is added to dict of all objects with a hash representing it's position
    public void Insert(GameObject go)
    {
        //Initial hash is -1, because newly added elements do not get builded if they have the same hash
        //after initial insert, rendering static system and first few frames unusable
        objects.Add(go.GetComponent<AABB>(), -1);
    }
    public void InsertToStatic(List<GameObject> staticGos)
    {
        foreach(GameObject go in staticGos)
            staticObjects.Add(go.GetComponent<AABB>(), -1);

        Debug.Log("Size again: " + staticObjects.Count);
    }

    //In classic way one clears lists every frame, but in our case we check if hash was changed and create a new list only if needed
    private void UpdatePositionsAndHashes()
    {
        objects = AddToBuckets(objects);
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

                //Delete(buckets[bound.Value], bound.Key);
                buckets[newHash].AddForm(bound.Key);
            }

            newObjects.Add(bound.Key, newHash);
        }
        return newObjects;
    }

    //if bucket contains more than 1 object forcecheck collision between them
    private void CheckCollisions()
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
        
        //buckets.Add(-1, new HashNode(new Vector2(float.MaxValue, float.MaxValue), -1));
    }

    //Simple 2D hash
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

    //Root is irrelevant in hash table
    public INode GetRoot()
    {
        return null;
    }

    public Framestats GetStats()
    {
        return (new Framestats(GameManager.Instance.GetExecTime(), Time.deltaTime, collisionChecks, checkedBuckets));
    }

    public void Delete(GameObject obj)
    {
        HashNode node = buckets[HashIt(obj.transform.position)];
        node.RemoveForm(obj.GetComponent<AABB>());
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

    private void HandlePlayerReady(GameObject player)
    {
        this.player = player;
    }

    public INode FindNode(GameObject go)
    {
        int hashOfAAbb = HashIt(go.transform.position);
        return buckets[hashOfAAbb];
    }
}
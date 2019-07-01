using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeCollisionSystem : Singleton_MB<QuadTreeCollisionSystem>, ICollisionSystem {

    public int depth = 2;
    public float size = 5;
    public GameObject player;

    private List<AABB> objects = new List<AABB>();
    //private List<AABB> staticObjects = new List<AABB>();

    public HashSet<QuadTreeNode> leaves;

    private int count = 0;
    private QuadTree qtree;
    private int collisionChecks = 0;
    private int numOfObjects = 0;

    private void Awake()
    {
        base.Awake();
        GameManager.Instance.OnPlayerReady.AddListener(HandlePlayerReady);
    }
    private void Start()
    {
        // Objects are inserted in static system before creation of a tree
        if(!GameManager.Instance.StaticSystem)
            qtree = new QuadTree(transform.position, size, depth);
    }

    private void Update()
    {
        if (GameManager.Instance.StaticSystem)
        {
            QuadTreeNode nodeWithPlayer;
            nodeWithPlayer = qtree.Insert(player.GetComponent<AABB>(), player.transform.position);
            CheckCollisions();
            GetNearestNeighbour(player);
            Delete(player);
            return;
        }

        if (count == 3)
        {
            numOfObjects = 0;
            collisionChecks = 0;
            Build();
            CheckCollisions();
            count = 0;
        }
        else
            count++;
    }

    public void CheckCollisions()
    {
        foreach (var node in leaves)
        {
            if (node.Content.Count > 1)
                collisionChecks += BoundsInteraction.CheckN2(node.Content);
        }

        if (GameManager.Instance.WriteStats)
        {
            StatsExcelSender.Instance.WriteStat(GetStats());
        }
    }

    public void Insert(GameObject obj)
    {
        objects.Add(obj.GetComponent<AABB>());
    }

    public void Delete(GameObject go)
    {
        QuadTreeNode currentNode = (QuadTreeNode)FindNode(go);
        currentNode.RemoveForm(go.GetComponent<AABB>());
        currentNode.BackPropagate();
    }

    public INode FindNode(GameObject go)
    {
        QuadTreeNode currentNode = qtree.GetRoot();
        QuadTreeNode[] subNodes;
        int index;

        do
        {
            subNodes = (QuadTreeNode[])currentNode.Nodes;
            index = QuadTreeNode.GetIndexPosition(go.transform.position, currentNode.Position);
            currentNode = subNodes[index];
        } while (!currentNode.IsLeaf());

        return currentNode;
    }

    private void OnDrawGizmos()
    {
        if (qtree != null)
            DrawNode(qtree.GetRoot());
    }


    private void DrawNode(QuadTreeNode node, int nodeDepth = 0)
    {
        if (!node.IsLeaf())
        {
            foreach (var subnode in node.Nodes)
            {
                if(subnode != null)
                    DrawNode(subnode, nodeDepth + 1);
            }
        }

        Gizmos.color = Color.Lerp(Color.magenta, Color.white, nodeDepth / (float)depth);
        Gizmos.DrawWireCube(node.Position, new Vector3(1, 1, 0.1f) * node.Size);
    }

    public INode GetRoot()
    {
        return qtree.GetRoot();
    }

    public Framestats GetStats()
    {
        return (new Framestats(GameManager.Instance.GetExecTime(), Time.deltaTime, collisionChecks, numOfObjects));
    }

    public void AddObjects(int num)
    {
        numOfObjects += num;
    }

    public void InsertToStatic(List<GameObject> staticGos)
    {
        leaves = new List<QuadTreeNode>();
        qtree = new QuadTree(transform.position, size, depth);

        foreach (GameObject go in staticGos)
        {
            QuadTreeNode newLeaf = qtree.Insert(go.GetComponent<AABB>());
            if (!leaves.Contains(newLeaf))
                leaves.Add(newLeaf);
        }
    }

    private void HandlePlayerReady(GameObject player)
    {
        this.player = player;
        Insert(player);
    }

    public KeyValuePair<AABB, float> GetNearestNeighbour(GameObject obj)
    {
        INode node = FindNode(obj);
        AABB objectBound = obj.GetComponent<AABB>();
        AABB nearestNeighbour;
        float distance = float.MaxValue;

        foreach (AABB aabb in node.Content)
        {
            if (aabb.Equals(objectBound))
                continue;

            float newDist = BoundsInteraction.GetDistance(aabb, objectBound);
            if (newDist < distance)
            {
                distance = newDist;
                nearestNeighbour = aabb;
            }
        }

        Debug.Log("Distance: " + distance + "| Object: " + nearestNeighbour.gameObject.name);

        return new KeyValuePair<AABB, float>(nearestNeighbour, distance);
    }

    public void Build()
    {
        qtree = new QuadTree(this.transform.position, size, depth);
        leaves = new List<QuadTreeNode>();
        
        foreach (var obj in objects)
        {
            leaves.Add(qtree.Insert(obj, obj.transform.position));
        }
    }
}

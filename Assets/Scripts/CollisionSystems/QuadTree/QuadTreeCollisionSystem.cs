using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeCollisionSystem : Singleton_MB<QuadTreeCollisionSystem>, ICollisionSystem {

    public int depth = 2;
    public float size = 5;
    public GameObject player;

    private List<AABB> objects = new List<AABB>();
    private List<AABB> staticObjects = new List<AABB>();

    public List<QuadTreeNode> leaves;
    public List<QuadTreeNode> staticLeaves;

    private int count = 0;
    private QuadTree qtree;
    private QuadTree staticTree;
    private int collisionChecks = 0;
    private int numOfObjects = 0;

    private void Awake()
    {
        base.Awake();
        GameManager.Instance.OnPlayerReady.AddListener(HandlePlayerReady);
    }
    private void Start()
    {
        staticLeaves = new List<QuadTreeNode>();
        qtree = new QuadTree(transform.position, size, depth);

    }

    private void Update()
    {
        if (count == 3)
        {
            numOfObjects = 0;
            collisionChecks = 0;
            qtree = new QuadTree(this.transform.position, size, depth);


            leaves = new List<QuadTreeNode>();
            
            foreach (var node in objects)
            {
                leaves.Add(qtree.Insert(node, node.transform.position));
            }

            CheckCollisions();
            count = 0;
        }
        else
            count++;
    }

    private void CheckCollisions()
    {
        foreach (var node in leaves)
        {
            if (node.values.Count > 1)
                collisionChecks += BoundsInteraction.CheckN2(node.values);
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

    public void Delete()
    {

    }

    private void OnDrawGizmos()
    {
        if (qtree != null)
            DrawNode(qtree.GetRoot());
        if (staticTree != null)
            DrawNode(staticTree.GetRoot());
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

    public void Delete(INode node, AABB obj)
    {
        
    }

    public void InsertToStatic(List<GameObject> staticGos)
    {
        staticLeaves = new List<QuadTreeNode>();

        foreach (GameObject go in staticGos)
            staticObjects.Add(go.GetComponent<AABB>());

        staticTree = new QuadTree(this.transform.position, size, depth);

        foreach (AABB aabb in staticObjects)
        {
            staticLeaves.Add(staticTree.InsertStaticList(aabb));
        }
    }

    private void HandlePlayerReady(GameObject player)
    {
        this.player = player;
        Insert(player);
    }
}

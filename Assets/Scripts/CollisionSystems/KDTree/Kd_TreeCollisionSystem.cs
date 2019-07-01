using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kd_TreeCollisionSystem : Singleton_MB<Kd_TreeCollisionSystem>, ICollisionSystem {

    public float size;
    public int maxObjNum = 4;
    public GameObject player;

    //K - index of an axis, 1 is max
    private int count = 0;
    private List<AABB> objects = new List<AABB>();
    private Kd_TreeNode root = new Kd_TreeNode();

    private List<Kd_TreeNode> leaves = new List<Kd_TreeNode>();
    private List<AABB> sortedX = new List<AABB>();
    private List<AABB> sortedY = new List<AABB>();

    private int collisionChecks = 0;
    private int numOfObjects = 0;

    // Use this for initialization
    private void Awake()
    {
        base.Awake();
        GameManager.Instance.OnPlayerReady.AddListener(HandlePlayerReady);
    }

    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        collisionChecks = 0;
        numOfObjects = 0;
        count++;
        Build();
        //InsertInTree(player);
        CheckCol();
        GetNearestNeighbour(player);
        //Delete(player);
        return;
    }

    public void Insert(GameObject obj)
    {
        objects.Add(obj.GetComponent<AABB>());
    }

    public void InsertInTree(GameObject obj)
    {
        FindNode(obj).AddForm(obj.GetComponent<AABB>());
    }

    public void InsertBulk(List<GameObject> objects)
    {
        foreach (GameObject go in objects)
            this.objects.Add(go.GetComponent<AABB>());
    }

    public void Build()
    {
        SortOnAxis();
        leaves.Clear();
        root = new Kd_TreeNode();
        root.Divide(sortedX, sortedY, 0);
    }

    public void CheckCollisions()
    {
        foreach (Kd_TreeNode lst in leaves)
            collisionChecks += BoundsInteraction.CheckN2(lst.Content);

        Framestats fst = GetStats();
        //Debug.Log("Deltatime: " + fst.deltaTime + " Framerate: " + fst.framerate);
        if (GameManager.Instance.WriteStats)
        {
            StatsExcelSender.Instance.WriteStat(GetStats());
        }
    }

    private void AddLeaf(Kd_TreeNode leaf)
    {
        leaves.Add(leaf);
    }

    public INode GetRoot()
    {
        return root;
    }

    private void SortOnAxis()
    {
        sortedX = new List<AABB>(objects);
        sortedY = new List<AABB>(objects);

        sortedX.Sort(delegate (AABB p1, AABB p2)
        {
            if (p1.transform.position.x > p2.transform.position.x) return 1;
            else if (p1.transform.position.x < p2.transform.position.x)
                return -1;
            else
                return 0;
        });

        sortedY.Sort(delegate (AABB p1, AABB p2)
        {
            if (p1.transform.position.y > p2.transform.position.y) return 1;
            else if(p1.transform.position.y < p2.transform.position.y)
                return -1;
            else
                return 0;
        });
    }

    private void DrawDebugz() {
        
        Debug.Log("Leaves " + leaves.Count);
        foreach (Kd_TreeNode node in leaves)
        {  
            string debugstr = "";
            foreach (AABB aabb in node.Content)
            {
                debugstr += aabb.gameObject.name + " ";
                Debug.Log("Group " + debugstr);
            } 
        }      
    }

    private void OnDrawGizmos()
    {
        var cachedLeaves = leaves;
        Debug.Log("LeavesNum: " + leaves.Count);

        if (leaves.Count != 0)
        {
            foreach (Kd_TreeNode node in cachedLeaves)
            {
                Vector2[] ends = GetBoundsOfLeaves(node.Content);

                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(new Vector3((ends[1].x + ends[0].x) / 2, (ends[1].y + ends[0].y) / 2, 0.1f), new Vector3(ends[1].x - ends[0].x - 0.1f, ends[1].y - ends[0].y - 0.1f, 0.1f));
            }
            
        }

    }

    private Vector2[] GetBoundsOfLeaves(List<AABB> toUnite)
    {
        float minx = toUnite[0].min.x;
        float maxx = toUnite[0].max.x;
        float miny = toUnite[0].min.y;
        float maxy = toUnite[0].max.y;

        foreach (AABB aabb in toUnite)
        {
            if (minx > aabb.min.x)
                minx = aabb.min.x;
            if (maxx < aabb.max.x)
                maxx = aabb.max.x;
            if (miny > aabb.min.y)
                miny = aabb.min.y;
            if (maxy < aabb.max.y)
                maxy = aabb.max.y;
        }
        Vector2 min = new Vector2(minx, miny);
        Vector2 max = new Vector2(maxx, maxy);

        return new Vector2[2] {min,max};
    }
    public Framestats GetStats()
    {
        return (new Framestats(GameManager.Instance.GetExecTime(), Time.deltaTime, collisionChecks, numOfObjects));
    }

    public INode FindNode(GameObject go)
    {
        Kd_TreeNode currentNode = (Kd_TreeNode)this.GetRoot();
        int k = 0;
        Kd_TreeNode[] children = (Kd_TreeNode[])currentNode.Children;

        while (!currentNode.IsLeaf())
        {
            if (k == 0)
            {
                currentNode = go.transform.position.x < children[0].maxX ? children[0] : children[1];
                children = (Kd_TreeNode[])currentNode.Children;
                k++;
            }
            else
            {
                currentNode = go.transform.position.y < children[0].maxY ? children[0] : children[1];
                children = (Kd_TreeNode[])currentNode.Children;
                k = 0;
            }
        }

        return currentNode;
    }

    public KeyValuePair<AABB, float> GetNearestNeighbour(GameObject obj)
    {
        INode node = FindNode(obj);
        AABB objectBound = obj.GetComponent<AABB>();
        AABB nearestNeighbour = objectBound;
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
    public void InsertToStatic(List<GameObject> staticGos)
    {
        foreach (GameObject go in staticGos)
            objects.Add(go.GetComponent<AABB>());
    }

    public void Delete(GameObject obj)
    {
        FindNode(obj).RemoveForm(obj.GetComponent<AABB>());
        objects.Remove(obj.GetComponent<AABB>());
    }
    private void HandlePlayerReady(GameObject player)
    {
        this.player = player;
        Insert(player);
    }


}

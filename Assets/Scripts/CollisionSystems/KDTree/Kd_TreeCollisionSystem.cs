using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kd_TreeCollisionSystem : Singleton_MB<Kd_TreeCollisionSystem>, ICollisionSystem {

    public float size;
    public int maxObjNum = 4;

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
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("Number of leaves: " + leaves.Count);
        collisionChecks = 0;
        count++;
        //CheckCol();
        //DrawDebugz();
        //if (count % 3 == 0 && objects.Count != 0)
        //{
            numOfObjects = 0;
            SortOnAxis();
            BuildTree();
            CheckCol();
            return;
        //}
    }

    public void Insert(GameObject obj)
    {
        objects.Add(obj.GetComponent<AABB>());
    }

    public void InsertBulk(List<GameObject> objects)
    {
        foreach (GameObject go in objects)
            this.objects.Add(go.GetComponent<AABB>());
    }

    private void BuildTree()
    {
        leaves.Clear();
        root = new Kd_TreeNode();
        root.Divide(sortedX, sortedY, 0);
    }

    private void CheckCol()
    {
        foreach (Kd_TreeNode lst in leaves)
            collisionChecks += BoundsInteraction.CheckN2(lst.children);

        Framestats fst = GetStats();
        Debug.Log("Deltatime: " + fst.deltaTime + " Framerate: " + fst.framerate);
        if (GameManager.Instance.WriteStats)
        {
            StatsExcelSender.Instance.WriteStat(GetStats());
        }
    }

    public void AddLeaf(Kd_TreeNode leaf)
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
        //sortedX.Sort((p1, p2) => (p1.transform.position.x).CompareTo(p2.transform.position.x));
        //sortedY.Sort((p1, p2) => (p1.transform.position.y).CompareTo(p2.transform.position.y));
        

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

        /*string debugstr = "";

        for (int i = 0; i < sortedX.Count; i++)
        {
            debugstr += sortedX[i].gameObject.transform.position.x + " ";
        }
        Debug.Log("SortedX " + debugstr);
        debugstr = "";
        for (int i = 0; i < sortedY.Count; i++)
        {
            debugstr += sortedY[i].gameObject.transform.position.y + " ";
        }
        Debug.Log("SortedY " + debugstr);
        */
    }

    private void DrawDebugz() {
        
        Debug.Log("Leaves " + leaves.Count);
        foreach (Kd_TreeNode node in leaves)
        {  
            //Vector2[] ends = GetBoundsOfLeaves(node.children);

            string debugstr = "";
            foreach (AABB aabb in node.children)
            {
                debugstr += aabb.gameObject.name + " ";
                Debug.Log("Group " + debugstr);

            }
            
        }
      
    }

    //Prettty much working
    private void OnDrawGizmos()
    {
        var cachedLeaves = leaves;
        Debug.Log("LeavesNum: " + leaves.Count);

        if (leaves.Count != 0)
        {
            foreach (Kd_TreeNode node in cachedLeaves)
            {

                Vector2[] ends = GetBoundsOfLeaves(node.children);

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

        return new Vector2[2] { min,max};
    }

    public void AddObjects(int obj)
    {
        numOfObjects += obj;
    }
    public Framestats GetStats()
    {
        return (new Framestats(GameManager.Instance.GetExecTime(), Time.deltaTime, collisionChecks, numOfObjects));
    }

    public void InsertToStatic(List<GameObject> staticGos)
    {
        throw new System.NotImplementedException();
    }

    public void Delete(GameObject obj)
    {
        throw new System.NotImplementedException();
    }
}

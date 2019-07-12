using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaPCollisionSystem : MonoBehaviour, ICollisionSystem {

    //K - index of an axis, 1 is max
    private int count = 0;
    private List<AABB> objects = new List<AABB>();
    private Kd_TreeNode root = new Kd_TreeNode();

    private List<AABB> sortedX = new List<AABB>();

    private int collisionChecks = 0;
    private int numOfObjects = 0;

    // Use this for initialization
    private void Awake()
    {

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
        return;
    }

    #region Interface_implementation
    public int CollisionChecks { get {return collisionChecks; } }
    public int NumOfObjects { get {return numOfObjects; } }
    public string ColSysName { get {return "Sweep_and_prune"; } }

    public GameObject getGameObject => throw new System.NotImplementedException();

    public INode GetRoot()
    {
        return root;
    }
    public void Build()
    {
        SortOnAxis();
    }
    public void Insert(GameObject obj)
    {
        objects.Add(obj.GetComponent<AABB>());
    }

    public void Delete(GameObject obj)
    {
        objects.Remove(obj.GetComponent<AABB>());
    }

    public INode FindNode(GameObject go)
    {
        return null;
    }

    public void InsertToStatic(List<GameObject> staticGos)
    {
        foreach (GameObject go in staticGos)
            objects.Add(go.GetComponent<AABB>());
    }

    public void CheckCollisions()
    {
        List<AABB> currentlyChecking = new List<AABB>();
        List<AABB> newCurrentlyChecking = new List<AABB>();
        List<Pair<AABB,AABB>> pairs = new List<Pair<AABB, AABB>>();
        collisionChecks = 0;

        foreach(AABB aabb in sortedX)
        {
            foreach(AABB active in currentlyChecking)
            {
                if(BoundsInteraction.CheckOverlap1D(aabb, active, true))
                {
                    newCurrentlyChecking.Add(active);
                    pairs.Add(new Pair<AABB, AABB>(aabb, active));
                }
            }
            currentlyChecking = new List<AABB>(newCurrentlyChecking);
            currentlyChecking.Add(aabb);
            newCurrentlyChecking = new List<AABB>();
        }

        foreach(Pair<AABB,AABB> pair in pairs)
        {
            collisionChecks++;
            if(BoundsInteraction.CheckOverlap1D(pair.First, pair.Second, false))
            {
                Debug.Log("Intersection " + pair.First.gameObject.name + "  and  " + pair.Second.gameObject.name);
            }
        }
    }

    public KeyValuePair<AABB, float> GetNearestNeighbour(GameObject obj)
    {
        if(!objects.Contains(obj.GetComponent<AABB>()))
            objects.Add(obj.GetComponent<AABB>());
        Build();
        AABB objectBound = obj.GetComponent<AABB>();
        AABB nearestNeighbour = objectBound;
        float distance = float.MaxValue;

        //Debug.Log("Distance: " + distance + "| Object: " + nearestNeighbour.gameObject.name);

        return new KeyValuePair<AABB, float>(nearestNeighbour, distance);
    }

    public Framestats GetStats()
    {
        return (new Framestats(GameManager.Instance.GetExecTime(), Time.deltaTime, collisionChecks, numOfObjects));
    }

    #endregion

    private void SortOnAxis()
    {
        sortedX = new List<AABB>(objects);

        sortedX.Sort(delegate (AABB p1, AABB p2)
        {
            if (p1.transform.position.x > p2.transform.position.x) return 1;
            else if (p1.transform.position.x < p2.transform.position.x)
                return -1;
            else
                return 0;
        });

    }

}

public class Pair<T, U> {
    public Pair() {
    }

    public Pair(T first, U second) {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};
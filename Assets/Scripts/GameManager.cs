﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton_MB<GameManager> {

    public CollisionSystems collision = CollisionSystems.HashTable;
    public int meteorsNum;
    public GameObject Meteor;
    public GameObject Player;
    public bool WriteStats = true;
    public bool StaticSystem = false;
    public float timeToSimulate = 3;

    [SerializeField]
    private float tileSize;
    [SerializeField]
    private bool isDebuging;
    private int namePart = 0;
    private ICollisionSystem CS;
    private List<AsyncOperation> loadOperations;
    private DebugGatherer debugGatherer = new DebugGatherer();
    private LineRenderer line;
    private Vector3[] positions = new Vector3[2];
    private GameObject PlayerInstance;

    public GameObjectEvent OnPlayerReady;
    public AABBEvent OnOutOfBounds;

    [SerializeField]
    private float execTime;
    private bool written = true;
    private bool draw = false;


    void Start()
    {
        OnPlayerReady.AddListener(HandleOnPlayerReady);
        loadOperations = new List<AsyncOperation>();
        Random.InitState(1);
        tileSize = 3 * Meteor.GetComponent<SpriteRenderer>().sprite.bounds.max.x;
        CS = ChooseCollision();

        GenerateField();
        line = this.gameObject.AddComponent<LineRenderer>();
        // Set the width of the Line Renderer
        line.SetWidth(0.05F, 0.05F);
        // Set the number of vertex fo the Line Renderer
        line.positionCount = 2;
    }

    void Update()
    {
        execTime += Time.deltaTime;
        if (execTime > timeToSimulate & written)
        {
            StatsExcelSender.Instance.WriteToFile("D:\\UnityPrj\\newStats1.xml");
            written = false;
        }

        debugGatherer.InitDebug();

        CS.Build();
        CS.CheckCollisions();
        //CheckNearestNeighbour(PlayerInstance);
        /*
        //Debug.Log(debugGatherer.WholeDebugInfo());
        ColSystemChanged();
        if (!draw)
        {
            DrawGraph();
            draw = true;
        }
        */
    }

    public ICollisionSystem ColSys { get { return CS; } }
    private void GenerateField()
    {
        if (meteorsNum % 2 != 0)
            meteorsNum++;

        List<GameObject> staticObjects = new List<GameObject>();

        for (int i = -meteorsNum / 2; i < meteorsNum / 2; i++)
        {
            for (int j = -meteorsNum / 2; j < meteorsNum / 2; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                GameObject go = MakeMeteor(i, j);
                go.name = go.name + namePart;
                namePart++;
                CS.Insert(go);
                staticObjects.Add(go);
            }
        }

        OnPlayerReady.Invoke(Instantiate(Player));
        //CS.InsertToStatic(staticObjects);
    }

    private GameObject MakeMeteor(int i, int j)
    {
        GameObject go = Instantiate(Meteor, new Vector2(i * tileSize, j * tileSize), Quaternion.AngleAxis(Random.Range(0, 360), new Vector3(0, 0, 1)));
        return go;
    }

    public void MakeTestMeteor()
    {
        GameObject go = Instantiate(Meteor, new Vector2(0, 0), Quaternion.AngleAxis(Random.Range(0, 360), new Vector3(0, 0, 1)));

        Vector3 dir;
        if (StaticSystem)
            dir = Vector3.zero;
        else
            dir = go.transform.up;

        go.GetComponent<Rigidbody2D>().AddForce(dir * (transform.rotation.z) * 10f, ForceMode2D.Impulse);
        go.name = go.name + namePart;
        namePart++;
        CS.Insert(go);
    }

    public void MakeTestMeteors(int number = 1, bool stop = false)
    {
        GameObject go;

        for (int i = -number; i <= number; i++)
        {
            for (int j = -number; j <= number; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                go = Instantiate(Meteor, new Vector2(i * tileSize, j * tileSize), Quaternion.AngleAxis(Random.Range(0, 360), new Vector3(0, 0, 1)));
                go.name = go.name + meteorsNum;
                meteorsNum++;
                CS.Insert(go);
            }
        }
        //CS.Insert(go, go.transform.position);
    }

    public void RestartGame()
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadSceneAsync(0);
    }

    public bool IsDebuging
    {
        get { return isDebuging; }
    }

    public void FormOutOfBounds(AABB aabb)
    {
        OnOutOfBounds.Invoke(aabb);
    }

    private ICollisionSystem ChooseCollision()
    {
        Behaviour col;
        switch (collision)
        {
            case CollisionSystems.HashTable:
                col = gameObject.GetComponent<HashTableCollisionSystem>();
                col.enabled = true;
                return (ICollisionSystem)col;
            case CollisionSystems.QuadTree:
                col = gameObject.GetComponent<QuadTreeCollisionSystem>();
                col.enabled = true;
                return (ICollisionSystem)col;
            case CollisionSystems.KdTree:
                col = gameObject.GetComponent<Kd_TreeCollisionSystem>();
                col.enabled = true;
                return (ICollisionSystem)col;
            case CollisionSystems.SweepAndPrune:
                col = gameObject.GetComponent<SaPCollisionSystem>();
                col.enabled = true;
                return (ICollisionSystem)col;
            default:
                col = gameObject.GetComponent<HashTableCollisionSystem>();
                col.enabled = true;
                return (ICollisionSystem)col;
        }
    }

    public float GetExecTime()
    {
        return this.execTime;
    }

    public string GetDebugInfo()
    {
        return debugGatherer.WholeDebugInfo();
    }
    public void CheckNearestNeighbour(GameObject toCheck)
    {
        GameObject go = CS.GetNearestNeighbour(toCheck).Key.gameObject;
        if (go == null)
        {
            Debug.Log("NULL!");
            go = toCheck;
        }

        debugGatherer.time = execTime;
        debugGatherer.deltaTime = Time.deltaTime;
        debugGatherer.n2calculations = CS.CollisionChecks;
        debugGatherer.numberOfObjects = CS.NumOfObjects;

        positions[0] = PlayerInstance.transform.position;
        positions[1] = go.transform.position;
        line.SetPositions(positions);
    }

    private void ColSystemChanged()
    {
        if (CS.ColSysName != ChooseCollision().ColSysName)
        {
            CS = null;
            CS = ChooseCollision();
        }
    }

    private void HandleOnPlayerReady(GameObject player)
    {
        PlayerInstance = player;
    }

    public void DrawGraph()
    {
        GraphVisualizer.Instance.DrawGraph(CS.GetRoot());
    }
}


public enum CollisionSystems
{
    HashTable,
    QuadTree,
    KdTree,
    SweepAndPrune
}

[System.Serializable]
public class GameObjectEvent :  UnityEvent<GameObject> { }

[System.Serializable]
public class AABBEvent : UnityEvent<AABB> { }
[System.Serializable]
public class EmptyEvent : UnityEvent { }
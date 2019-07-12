using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//TODO Change INode to actual visualizable nodes
public class GraphVisualizer : Singleton_MB<GraphVisualizer>, IDragHandler
{
    public GameObject elNode;
    public GameObject line;
    public static float nodeRadius = 25;
    public static float levelHeight = 100;
    public float scale = 0.05f;
    public DrawType type = DrawType.Symmetrical;

    private GameObject linesInHierarchy;
    List<VisualNode> graphNodes = new List<VisualNode>();
    List<INode> nodes = new List<INode>();
    Dictionary<INode, VisualNode> nodeToVisual = new Dictionary<INode, VisualNode>();
    List<List<VisualNode>> nodesInDepth = new List<List<VisualNode>>();
    List<GameObject> lines;
    INode root;
    int maxDepth;

    public EmptyEvent OnSizeChanged;

    private void Start()
    {
        linesInHierarchy = transform.Find("Lines").gameObject;
    }
    //TODO make hierarchy object for assigning parents to the "lines" object and "nodes" object on scene
    public void DrawGraph(INode root)
    {
        //nodeRadius = nodeRadius / gameObject.GetComponent<RectTransform>().localScale.x;
        //levelHeight = levelHeight / gameObject.GetComponent<RectTransform>().localScale.y;
        if (root != null)
        {
            foreach (var node in nodeToVisual.Values)
                Destroy(node.gameObject);

            graphNodes = new List<VisualNode>();
            nodes = new List<INode>();
            nodeToVisual = new Dictionary<INode, VisualNode>();
            nodesInDepth = new List<List<VisualNode>>();
        }
        ExtractGraph(root);
        Reposition();
        DrawConnections();
    }

    private void OnGUI()
    {
        transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x + Input.mouseScrollDelta.y * scale, 0.1f, 1),
            Mathf.Clamp(transform.localScale.y + Input.mouseScrollDelta.y * scale, 0.1f, 1), 0);
        OnSizeChanged.Invoke();
    }

    public void Scale(bool sc)
    {

    }
    
    public void MoveGraph()
    {

    }

    public void ExtractGraph(INode root)
    {
        this.root = root;
        List<INode> toCheck = new List<INode>();
        List<INode> newToCheck = new List<INode>();
        int depth = 0;
        toCheck.Add(root);

        VisualNode rootV = Instantiate(elNode).GetComponent<VisualNode>();

        rootV.Initialize(root, null, depth);
        rootV.transform.SetParent(transform, false);
        rootV.gameObject.name = "root";
        rootV.nodeName.text = rootV.gameObject.name;

        nodeToVisual.Add(root, rootV);

        nodesInDepth.Add(new List<VisualNode>());
        nodesInDepth[0].Add(rootV);

        while (toCheck.Count != 0)
        {
            depth++;
            nodesInDepth.Add(new List<VisualNode>());
            foreach (INode node in toCheck)
            {
                int i = 0;
                if (node.Children != null)
                {
                    VisualNode parentVisual = nodeToVisual[node];
                    foreach (INode childNode in node.Children)
                    {
                        newToCheck.Add(childNode);
                        VisualNode vn = Instantiate(elNode).GetComponent<VisualNode>();
                        vn.Initialize(childNode, parentVisual, depth);
                        vn.gameObject.name = "[" + depth + "]{" + i + "}";
                        vn.nodeName.text = vn.gameObject.name;
                        //vn.transform.parent = gameObject.transform;
                        nodeToVisual.Add(childNode, vn);
                        nodesInDepth[depth].Add(vn);
                        i++;
                    }
                }
            }
            toCheck = new List<INode>(newToCheck);
            newToCheck = new List<INode>();
        }

        maxDepth = depth;
    }

    //Reposition a graphical nodes to be visible as a hierarchy
    private void Reposition()
    {
        int depth = nodesInDepth.Count - 1;
        int divider = 1;

        if (type == DrawType.Compact)
            divider = 2;
        
        while (depth >= 0)
        {
            for (int i = 0; i < nodesInDepth[depth].Count; i++)
            {
                VisualNode currentNode = nodesInDepth[depth][i];
                currentNode.CountWidth();
            }
            depth--;
        }
        
        foreach (VisualNode node in nodeToVisual.Values)
        {
            int currentWidth = 0;
            foreach (VisualNode childe in node.childrenVisual)
            {
                childe.transform.SetParent(node.transform, false);
                childe.SetPosition(currentWidth + childe.width / 2 - node.width / 2);
                currentWidth += childe.width / divider;
            }
        }

    }

    private void DrawConnections()
    {
        lines = new List<GameObject>();
        List<VisualNode> toDraw = new List<VisualNode>();
        List<VisualNode> newToDraw = new List<VisualNode>();

        toDraw.Add(nodeToVisual[this.root]);

        while (toDraw.Count != 0)
        {
            foreach (VisualNode node in toDraw)
            {
                foreach (VisualNode childNode in node.childrenVisual)
                {
                    MakeLine(node, childNode);
                    newToDraw.Add(childNode);
                }
            }
            toDraw = new List<VisualNode>(newToDraw);
            newToDraw = new List<VisualNode>();
        }
    }

    private GameObject MakeLine(VisualNode start, VisualNode end)
    {
        GameObject newLine = Instantiate(line);
        lines.Add(newLine);
        var lr = newLine.GetComponent<LineRenderer>();
        newLine.name = start.gameObject.name + "---" + end.gameObject.name;
        //lr.material = new Material(Shader.Find("Prefab/GreenForLines"));
        lr.startWidth = 5F;
        lr.endWidth = 5F;
        lr.positionCount = 2;
        Vector3[] positions = new Vector3[2];
        positions[0] = start.transform.position;
        positions[1] = end.transform.position;
        lr.startColor = Color.green;
        lr.endColor = Color.green;
        lr.SetPositions(positions);
        newLine.transform.SetParent(linesInHierarchy.transform, false);
        return newLine;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var rt = GetComponent<RectTransform>();
        rt = eventData.pointerEnter.transform as RectTransform;
    }
}

public enum DrawType
{
    Symmetrical,
    Compact,
    CompactSymmetrical
}
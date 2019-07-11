using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Change INode to actual visualizable nodes
public class GraphVisualizer : Singleton_MB<GraphVisualizer>
{
    public GameObject elNode;
    public static float nodeRadius = 25;
    public static float levelHeight = 100;

    List<VisualNode> graphNodes = new List<VisualNode>();
    List<INode> nodes = new List<INode>();
    Dictionary<INode, VisualNode> nodeToVisual = new Dictionary<INode, VisualNode>();
    List<List<VisualNode>> nodesInDepth = new List<List<VisualNode>>();
    List<GameObject> lines;
    INode root;
    float scale = 1;
    int maxDepth;

    //TODO make hierarchy object for assigning parents to the "lines" object and "nodes" object on scene
    public void DrawGraph(INode root)
    {
        //nodeRadius = nodeRadius / gameObject.GetComponent<RectTransform>().localScale.x;
        //levelHeight = levelHeight / gameObject.GetComponent<RectTransform>().localScale.y;
        ExtractGraph(root);
        Reposition();
        DrawConnections();
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

        nodeToVisual.Add(root, rootV);

        nodesInDepth.Add(new List<VisualNode>());
        nodesInDepth[0].Add(rootV);

        while (toCheck.Count != 0)
        {
            depth++;
            nodesInDepth.Add(new List<VisualNode>());
            foreach (INode node in toCheck)
            {
                if (node.Children != null)
                {
                    VisualNode parentVisual = nodeToVisual[node];
                    foreach (INode childNode in node.Children)
                    {
                        newToCheck.Add(childNode);
                        VisualNode vn = Instantiate(elNode).GetComponent<VisualNode>();
                        vn.Initialize(childNode, parentVisual, depth);
                        //vn.transform.parent = gameObject.transform;
                        nodeToVisual.Add(childNode, vn);
                        nodesInDepth[depth].Add(vn);
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
                    GameObject line = new GameObject();
                    lines.Add(line);
                    var lr = line.AddComponent<LineRenderer>();
                    //lr.material = new Material(Shader.Find("Prefab/GreenForLines"));
                    lr.startWidth = 5F;
                    lr.endWidth = 5F;
                    lr.positionCount = 2;
                    Vector3[] positions = new Vector3[2];
                    positions[0] = node.transform.position;
                    positions[1] = childNode.transform.position;
                    lr.startColor = Color.green;
                    lr.endColor = Color.green;
                    lr.SetPositions(positions);
                    line.AddComponent<CanvasRenderer>();
                    newToDraw.Add(childNode);
                    line.transform.SetParent(transform, false);
                }
            }
            toDraw = new List<VisualNode>(newToDraw);
            newToDraw = new List<VisualNode>();
        }
    }

}

public enum DrawType
{
    Symmetrical,
    Compact,
    CompactSymmetrical
}
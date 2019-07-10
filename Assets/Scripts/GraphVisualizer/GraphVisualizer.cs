using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Change INode to actual visualizable nodes
public class GraphVisualizer : MonoBehaviour
{
    public GameObject elNode;
    public static float nodeRadius = 9;
    public static float levelHeight = 36;
    List<VisualNode> graphNodes = new List<VisualNode>();
    List<INode> nodes = new List<INode>();
    Dictionary<INode, VisualNode> nodeToVisual = new Dictionary<INode, VisualNode>();
    List<List<VisualNode>> nodesInDepth = new List<List<VisualNode>>();
    INode root;
    float scale = 1;
    int maxDepth;

    public void DrawGraph()
    {

    }

    public void Scale(bool sc)
    {

    }
    
    public void MoveGraph()
    {

    }

    public void ExtractGraph(INode root)
    {
        List<INode> toCheck = new List<INode>();
        List<INode> newToCheck = new List<INode>();
        int depth = 0;
        toCheck.Add(root);

        VisualNode rootV = Instantiate(elNode).GetComponent<VisualNode>();

        rootV.Initialize(root, null, depth);
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
                        vn.transform.parent = gameObject.transform;
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

    private void AddNode(INode node)
    {
        nodes.Add(node);
    }

    //Reposition a graphical nodes to be visible in a sensible way
    private void Reposition(List<VisualNode> nodes, VisualNode parent)
    {
        int depth = nodesInDepth.Count - 1;

        int numOfNodes = nodesInDepth[depth].Count;
        int totalWidth = CountWidth(nodesInDepth[depth]);
        int currentWidth = 0;

        nodesInDepth[depth][0].SetPosition(0);
        currentWidth += nodesInDepth[depth][0].width;

        for (int i = 1; i < nodesInDepth[depth].Count; i++)
        {
            VisualNode currentNode = nodesInDepth[depth][i];
            currentNode.SetPosition(currentWidth + currentNode.width / 2);
            currentWidth += currentNode.width;

        }

    }

    private int CountWidth(List<VisualNode> nodes)
    {
        int totalWidth = 0;
        int numOfNodes = nodes.Count;
        for (int i = 0; i < numOfNodes; i++)
        {
            totalWidth += nodes[i].width;
        }

        return totalWidth;
    }
}
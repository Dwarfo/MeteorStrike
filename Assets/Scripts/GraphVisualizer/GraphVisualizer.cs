using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Change INode to actual visualizable nodes
public class GraphVisualizer : MonoBehaviour
{
    GameObject elNode;
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

        toCheck.Add(root);

        VisualNode rootV = Instantiate(elNode).GetComponent<VisualNode>();
        rootV.Initialize(root, null, 0);
        nodeToVisual.Add(root, rootV);

        nodesInDepth.Add(new List<VisualNode>());
        nodesInDepth[0].Add(rootV);
        int depth = 1;

        while (toCheck.Count != 0)
        {
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
            depth++;
        }

        maxDepth = depth;
        Reposition();

    }

    private void AddNode(INode node)
    {
        nodes.Add(node);
    }

    //Reposition a graphical nodes to be visible in a sensible way
    private void Reposition()
    {
        //Two steps:
        //First bottom-up for placing graph elements 

        //Second top-down for drawing connections
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Change INode to actual visualizable nodes
public class GraphVisualizer : MonoBehaviour
{
    GameObject elNode;
    List<VisualNode> graphNodes = new List<VisualNode>();
    List<INode> nodes = new List<INode>();
    INode root;
    float scale = 1;

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
        rootV.Initialize(root, null);
        graphNodes.Add(rootV);

        while (toCheck.Count != 0)
        {
            foreach (INode node in toCheck)
            {
                if (node.Children != null)
                {
                    foreach (INode childNode in node.Children)
                    {
                        newToCheck.Add(childNode);
                        VisualNode vn = Instantiate(elNode).GetComponent<VisualNode>();
                        vn.Initialize(childNode, node);
                        graphNodes.Add(vn);
                    }
                }
            }
            toCheck = new List<INode>(newToCheck);
            newToCheck = new List<INode>();
        }

        Reposition();

    }

    private void AddNode(INode node)
    {
        nodes.Add(node);
    }

    //Reposition a graphical nodes to be visible in a sensible way
    private void Reposition()
    {

    }
}
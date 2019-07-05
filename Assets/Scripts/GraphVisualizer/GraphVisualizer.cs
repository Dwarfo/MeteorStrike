using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Change INode to actual visualizable nodes
public class GraphVisualizer : MonoBehaviour
{
    GameObject elNode;
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

    public void ExtractGraph()
    {

    }

    private void AddNode(INode node)
    {
        nodes.Add(node);
    }

}
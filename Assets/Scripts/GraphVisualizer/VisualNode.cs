using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Change INode to actual visualizable nodes
public class VisualNode : MonoBehaviour
{

    GameObject tableWithNodeInfo;
    int depth;
    int numOfChildren;
    string nodeType;
    List<AABB> Content;
    List<VisualNode> childrenVisual = new List<VisualNode>();
         
    void OnMouseOver()
    {
        tableWithNodeInfo.SetActive(true);
    }

    void OnMouseExit()
    {
        tableWithNodeInfo.SetActive(false);
    }

    public void Initialize(INode node, VisualNode parentNode, int depth)
    {
        parentNode.AddChild(this);
        nodeType = node.NodeType;
        this.depth = depth;
        Content = node.Content;
    }

    public void AddChild(VisualNode vn)
    {
        childrenVisual.Add(vn);
    }
}
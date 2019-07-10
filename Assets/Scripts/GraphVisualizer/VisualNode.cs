using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Change INode to actual visualizable nodes
public class VisualNode : MonoBehaviour
{

    public GameObject tableWithNodeInfo;
    public int width = 4;
    int depth;
    int numOfChildren;
    string nodeType;
    List<AABB> Content;
    public List<VisualNode> childrenVisual = new List<VisualNode>();
         
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
        Reparent(parentNode);
        nodeType = node.NodeType;
        this.depth = depth;
        Content = node.Content;
    }

    public void AddChild(VisualNode vn)
    {
        childrenVisual.Add(vn);
    }

    private void Reparent(VisualNode parentNode)
    {
        parentNode.AddChild(this);
        transform.parent = parentNode.transform;
    }

    public void SetPosition(int pos)
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(pos * GraphVisualizer.nodeRadius, -1 * depth * GraphVisualizer.levelHeight);
    }
}
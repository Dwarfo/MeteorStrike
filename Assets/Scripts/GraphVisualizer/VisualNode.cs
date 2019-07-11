using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Change INode to actual visualizable nodes
public class VisualNode : MonoBehaviour
{
    public GameObject tableWithNodeInfo;
    public int width = 2;
    public int depth;
    public VisualNode parent;
    int numOfChildren;
    public int currentPos;
    public string nodeType;
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
        vn.SetParent(this);
    }

    private void Reparent(VisualNode parentNode)
    {
        if (parentNode != null)
        {
            parentNode.AddChild(this);
            transform.parent = GraphVisualizer.Instance.transform;
        }
    }

    public void SetPosition(int pos)
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(pos * GraphVisualizer.nodeRadius, -1 * GraphVisualizer.levelHeight);
        currentPos = pos;
        GetComponent<InfoPanel>().UpdateInfo(this);
    }

    public void RepositionOnParent(int posDiff)
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.localPosition = new Vector3((currentPos - posDiff) * GraphVisualizer.nodeRadius, -1 * depth * GraphVisualizer.levelHeight);
    }

    public void CountWidth()
    {
        if (childrenVisual.Count == 0)
            return;

        width = childrenVisual[0].width;

        for (int i = 1; i < childrenVisual.Count; i++)
        {
            width += childrenVisual[i].width;
        }

    }

    public void SetParent(VisualNode batya)
    {
        parent = batya;
    }
}
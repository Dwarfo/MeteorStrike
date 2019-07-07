using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Change INode to actual visualizable nodes
public class VisualNode : MonoBehaviour
{

    GameObject tableWithNodeInfo;
    
    void OnMouseOver()
    {
        tableWithNodeInfo.SetActive(true);
    }

    void OnMouseExit()
    {
        tableWithNodeInfo.SetActive(false);
    }

    public void Initialize(INode node, INode parentNode)
    {
        
    }
}